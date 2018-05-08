using ApiModel.Entities;
using ApiServer.Data;
using ApiServer.Stores;
using BambooCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiServer.Services
{
    public class CategoryMan
    {
        ApiDbContext context;
        DbSet<AssetCategory> dbset;
        protected readonly AssetCategoryTreeStore _categoryTreeStore;
        public CategoryMan(ApiDbContext context)
        {
            this.context = context;
            dbset = context.Set<AssetCategory>();
            _categoryTreeStore = new AssetCategoryTreeStore(context);
        }


        /// <summary>
        ///  获取所有分类，已经整理为树结构
        /// </summary>
        /// <param name="type"></param>
        /// <param name="organId"></param>
        /// <returns></returns>
        public async Task<AssetCategoryDTO> GetCategoryAsync(string type, string organId)
        {
            List<AssetCategory> templist = await dbset.Where(d => d.Type == type && d.OrganizationId == organId).ToListAsync();
            LinkedList<AssetCategory> list = new LinkedList<AssetCategory>();
            foreach (var item in templist)
            {
                list.AddLast(item);
            }
            AssetCategoryDTO root = FindRoot(list);
            FindChildren(list, root);

            if (root == null)
            {
                AssetCategory rootNode = new AssetCategory();
                rootNode.Id = GuidGen.NewGUID();
                rootNode.ParentId = "";
                rootNode.OrganizationId = organId;
                rootNode.Type = type;
                rootNode.Name = type + "_root";
                rootNode.Icon = "";
                rootNode.Description = "auto generated node for " + type + ", do not need to display this node";
                dbset.Add(rootNode);
                await context.SaveChangesAsync();

                #region 添加tree节点
                {
                    var oTree = new AssetCategoryTree();
                    oTree.NodeType = type;
                    oTree.Name = rootNode.Name;
                    oTree.ObjId = rootNode.Id;
                    oTree.OrganizationId = organId;
                    oTree.CreatedTime = DateTime.Now;
                    await _categoryTreeStore.AddNewNode(oTree);
                }
                #endregion

                root = rootNode.ToDTO();
            }

            return root;
        }

        /// <summary>
        /// 获取扁平结构的分类信息
        /// </summary>
        /// <param name="type"></param>
        /// <param name="organId"></param>
        /// <returns></returns>
        public async Task<List<AssetCategoryDTO>> GetFlatCategory(string type, string organId)
        {
            return await _categoryTreeStore.GetFlatCategory(type, organId);
        }

        public async Task<AssetCategoryDTO> GetById(string id)
        {
            var data = await dbset.FindAsync(id);
            if (data != null)
                return data.ToDTO();
            return new AssetCategoryDTO();
        }

        /// <summary>
        /// 创建一个分类，一次只能创建单个分类，不会层级创建。必须指定一个父级ID，不能主动创建根节点，根节点在get时会自动创建。
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<AssetCategoryDTO> CreateAsync(AssetCategoryDTO dto)
        {
            //必须指定一个父级ID，不能主动创建根节点，根节点在get时会自动创建。
            if (string.IsNullOrEmpty(dto.ParentId))
                return null;

            AssetCategory parent = await dbset.FirstOrDefaultAsync(d => d.Type == dto.Type && d.Id == dto.ParentId);

            if (parent == null)
            {
                return null; // want to create a child node, but parent node not found.
            }

            int childrenCount = await dbset.Where(d => d.ParentId == dto.ParentId).CountAsync();

            AssetCategory cat = new AssetCategory();
            cat.Id = GuidGen.NewGUID();
            cat.Name = dto.Name;
            cat.Description = dto.Description;
            cat.Icon = dto.Icon;
            cat.Type = dto.Type;
            cat.ParentId = dto.ParentId;
            cat.DisplayIndex = childrenCount; //index start from 0.
            cat.OrganizationId = dto.OrganizationId;
            dbset.Add(cat);
            await context.SaveChangesAsync();

            #region 添加tree节点
            {
                var pNode = await context.AssetCategoryTrees.Where(x => x.ObjId == dto.ParentId && x.OrganizationId == dto.OrganizationId).FirstOrDefaultAsync();
                if (pNode != null)
                {
                    var oTree = new AssetCategoryTree();
                    oTree.NodeType = dto.Type;
                    oTree.Name = cat.Name;
                    oTree.ObjId = cat.Id;
                    oTree.OrganizationId = dto.OrganizationId;
                    oTree.CreatedTime = DateTime.Now;
                    oTree.ParentId = pNode.Id;
                    await _categoryTreeStore.AddChildNode(oTree);
                }
            }
            #endregion

            return cat.ToDTO();
        }

        /// <summary>
        /// 修改一个分类的标准信息，名称，图标，描述。不能修改位置，子分类等内容
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<AssetCategoryDTO> ModifyAsync(AssetCategoryDTO value)
        {
            AssetCategory cat = await dbset.FirstOrDefaultAsync(d => d.Type == value.Type && d.Id == value.Id);
            if (cat == null)
            {
                return null;
            }
            cat.Name = value.Name;
            cat.Icon = value.Icon;
            cat.Description = value.Description;
            await context.SaveChangesAsync();
            return cat.ToDTO();
        }

        /// <summary>
        /// 删除一个分类，只能删除空分类，如果有子分类或者此分类下有资源存在则不让删除此分类。需要把子分类或者资源转移到其他分类下或者删除干净才能删除此分类。
        /// </summary>
        /// <param name="type">分类的类型，比如产品product，材质material</param>
        /// <param name="catid">要被删除的分类的id</param>
        /// <returns>error message</returns>
        public async Task<string> DeleteAsync(string type, string catid)
        {
            AssetCategory childCategory = await dbset.FirstOrDefaultAsync(d => d.Type == type && d.ParentId == catid);
            if (childCategory != null)
            {
                return "cannot delete this category, until delete or move all of children category."; //此分类下还有子分类，不可以删除。
            }
            AssetCategory target = await dbset.FirstOrDefaultAsync(d => d.Type == type && d.Id == catid);
            if (target == null)
            {
                return "category " + catid + " not found"; // not found.
            }

            //hack
            bool haveAssets = false;
            if (type == "product")
            {
                var asset = await context.Set<Product>().FirstOrDefaultAsync(d => d.CategoryId == catid);
                haveAssets = (asset != null);
            }
            else if (type == "material")
            {
                var asset = await context.Set<Material>().FirstOrDefaultAsync(d => d.CategoryId == catid);
                haveAssets = (asset != null);
            }

            if (haveAssets)
            {
                return "can not delete this category, until delete or move all asset to another category";
            }

            dbset.Remove(target);
            await context.SaveChangesAsync();
            return "";
        }

        /// <summary>
        /// 移动一个分类到另外一个分类下
        /// </summary>
        /// <param name="type"></param>
        /// <param name="catid"></param>
        /// <param name="targetCatId"></param>
        /// <returns>error message</returns>
        public async Task<string> MoveAsync(string type, string catid, string targetCatId)
        {
            AssetCategory cat = await dbset.FirstOrDefaultAsync(d => d.Type == type && d.Id == catid);
            if (cat == null)
            {
                return "category " + catid + " not found"; // not found.
            }

            AssetCategory targetCat = await dbset.FirstOrDefaultAsync(d => d.Type == type && d.Id == targetCatId);
            if (targetCat == null)
            {
                return "target category " + targetCatId + " not found"; // not found.
            }

            int childrenCount = await dbset.Where(d => d.ParentId == targetCatId).CountAsync();

            cat.ParentId = targetCatId;
            cat.DisplayIndex = childrenCount; // index start from 0;
            await context.SaveChangesAsync();
            return "";
        }

        /// <summary>
        /// 转移一个分类下的所有资产到另外一个分类下
        /// </summary>
        /// <param name="type"></param>
        /// <param name="catid"></param>
        /// <param name="targetCatId"></param>
        /// <returns></returns>
        public async Task<string> TransferAsync(string type, string catid, string targetCatId)
        {
            AssetCategory cat = await dbset.FirstOrDefaultAsync(d => d.Type == type && d.Id == catid);
            if (cat == null)
            {
                return "category " + catid + " not found"; // not found.
            }

            AssetCategory targetCat = await dbset.FirstOrDefaultAsync(d => d.Type == type && d.Id == targetCatId);
            if (targetCat == null)
            {
                return "target category " + targetCatId + " not found"; // not found.
            }

            AssetCategory targetChildCat = await dbset.FirstOrDefaultAsync(d => d.Type == type && d.ParentId == targetCatId);
            if (targetChildCat != null)
            {
                return "target category " + targetCatId + " have child category. ";
            }

            string tableName = "";
            if (type == "product")
                tableName = "Products";
            else if (type == "material")
                tableName = "Materials";

            string sql = string.Format("update \"{0}\" set \"CategoryId\"='{1}' where \"CategoryId\"='{2}'", tableName, targetCatId, catid);
            int rows = await context.Database.ExecuteSqlCommandAsync(sql);
            return "";
        }

        /// <summary>
        /// 设置一个分类在父级分类中的显示顺序，index的范围为0 - childrenCount - 1.非法的index会被自动矫正
        /// 返回父级分类对象。
        /// </summary>
        /// <param name="type"></param>
        /// <param name="catid"></param>
        /// <param name="displayIndex"></param>
        /// <returns>返回父级分类对象</returns>
        public async Task<AssetCategoryDTO> SetDisplayIndex(string type, string catid, int displayIndex)
        {
            AssetCategory target = await dbset.FirstOrDefaultAsync(d => d.Type == type && d.Id == catid);
            if (target == null)
            {
                return null; // not found.
            }
            AssetCategory parent = await dbset.FirstOrDefaultAsync(d => d.Type == type && d.Id == target.ParentId);
            if (parent == null)
            {
                return null; // not found.
            }

            AssetCategoryDTO result = parent.ToDTO();

            List<AssetCategory> children = await dbset.Where(d => d.ParentId == target.ParentId).OrderBy(d => d.DisplayIndex).ToListAsync();
            int total = children.Count;
            if (displayIndex < 0)
                displayIndex = 0;
            if (displayIndex >= total)
                displayIndex = total - 1;

            children.Remove(target);
            children.Insert(displayIndex, target);

            int index = 0;
            foreach (var item in children)
            {
                item.DisplayIndex = index++;
                result.Children.Add(item.ToDTO());
            }

            await context.SaveChangesAsync();

            return result;
        }

        AssetCategoryDTO FindRoot(LinkedList<AssetCategory> list)
        {
            foreach (var item in list)
            {
                if (string.IsNullOrEmpty(item.ParentId))
                {
                    return item.ToDTO();
                }
            }
            return null;
        }

        void FindChildren(LinkedList<AssetCategory> list, AssetCategoryDTO parent)
        {
            if (parent == null)
                return;
            var node = list.First;
            while (node != null)
            {
                var next = node.Next;

                var item = node.Value;
                if (item.ParentId == parent.Id)
                {
                    AssetCategoryDTO child = item.ToDTO();
                    parent.Children.Add(child);
                    list.Remove(node);
                }

                node = next;
            }

            foreach (var item in parent.Children)
            {
                FindChildren(list, item);
            }
        }

    }
}
