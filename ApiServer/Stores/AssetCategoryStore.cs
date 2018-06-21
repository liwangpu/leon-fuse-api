using ApiModel.Consts;
using ApiModel.Entities;
using ApiModel.Enums;
using ApiServer.Data;
using BambooCore;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiServer.Stores
{
    public class AssetCategoryStore : StoreBase<AssetCategory, AssetCategoryDTO>, IStore<AssetCategory, AssetCategoryDTO>
    {
        /// <summary>
        /// 资源访问类型
        /// </summary>
        public override ResourceTypeEnum ResourceTypeSetting
        {
            get
            {
                return ResourceTypeEnum.Organizational;
            }
        }

        protected readonly AssetCategoryTreeStore _categoryTreeStore;

        #region 构造函数
        public AssetCategoryStore(ApiDbContext context)
        : base(context)
        {
            _categoryTreeStore = new AssetCategoryTreeStore(context);
        }
        #endregion

        /// <summary>
        ///  获取所有分类，已经整理为树结构
        /// </summary>
        /// <param name="type"></param>
        /// <param name="organId"></param>
        /// <returns></returns>
        public async Task<AssetCategoryDTO> GetCategoryAsync(string type, string organId)
        {

            List<AssetCategory> templist = await DbContext.AssetCategories.Where(d => d.Type == type && d.OrganizationId == organId && d.ActiveFlag == AppConst.I_DataState_Active).ToListAsync();
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
                rootNode.ResourceType = (int)ResourceTypeEnum.Organizational;
                DbContext.AssetCategories.Add(rootNode);
                await DbContext.SaveChangesAsync();

                #region 添加tree节点
                {
                    var oTree = new AssetCategoryTree();
                    oTree.NodeType = type;
                    oTree.Name = rootNode.Name;
                    oTree.ObjId = rootNode.Id;
                    oTree.OrganizationId = organId;
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
            var data = await DbContext.AssetCategories.FindAsync(id);
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

            AssetCategory parent = await DbContext.AssetCategories.FirstOrDefaultAsync(d => d.Type == dto.Type && d.Id == dto.ParentId);

            if (parent == null)
            {
                return null; // want to create a child node, but parent node not found.
            }

            int childrenCount = await DbContext.AssetCategories.Where(d => d.ParentId == dto.ParentId).CountAsync();

            AssetCategory cat = new AssetCategory();
            cat.Id = GuidGen.NewGUID();
            cat.Name = dto.Name;
            cat.Description = dto.Description;
            cat.Icon = dto.Icon;
            cat.Type = dto.Type;
            cat.ParentId = dto.ParentId;
            cat.DisplayIndex = childrenCount; //index start from 0.
            cat.OrganizationId = dto.OrganizationId;
            cat.ResourceType = (int)ResourceTypeEnum.Organizational;
            DbContext.AssetCategories.Add(cat);
            await DbContext.SaveChangesAsync();

            #region 添加tree节点
            {
                var pNode = await DbContext.AssetCategoryTrees.Where(x => x.ObjId == dto.ParentId && x.OrganizationId == dto.OrganizationId).FirstOrDefaultAsync();
                if (pNode != null)
                {
                    var oTree = new AssetCategoryTree();
                    oTree.NodeType = dto.Type;
                    oTree.Name = cat.Name;
                    oTree.ObjId = cat.Id;
                    oTree.OrganizationId = dto.OrganizationId;
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
            AssetCategory cat = await DbContext.AssetCategories.FirstOrDefaultAsync(d => d.Type == value.Type && d.Id == value.Id);
            if (cat == null)
            {
                return null;
            }
            cat.Name = value.Name;
            cat.Icon = value.Icon;
            cat.Description = value.Description;
            //cat.ParentId = value.ParentId;
            await DbContext.SaveChangesAsync();

            #region 父分类有变,修改tree信息
            if (value.ParentId != cat.ParentId)
            {

            }
            #endregion

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
            AssetCategory childCategory = await DbContext.AssetCategories.FirstOrDefaultAsync(d => d.Type == type && d.ParentId == catid);
            if (childCategory != null)
            {
                return "此分类下还有子分类，不可以删除"; //此分类下还有子分类，不可以删除。
            }
            AssetCategory target = await DbContext.AssetCategories.FirstOrDefaultAsync(d => d.Type == type && d.Id == catid);
            if (target == null)
            {
                return "category " + catid + " not found"; // not found.
            }

            //hack
            bool haveAssets = false;
            if (type == "product")
            {
                var asset = await DbContext.Set<Product>().FirstOrDefaultAsync(d => d.CategoryId == catid);
                haveAssets = (asset != null);
            }
            else if (type == "material")
            {
                var asset = await DbContext.Set<Material>().FirstOrDefaultAsync(d => d.CategoryId == catid);
                haveAssets = (asset != null);
            }

            if (haveAssets)
            {
                return "can not delete this category, until delete or move all asset to another category";
            }

            DbContext.AssetCategories.Remove(target);
            await DbContext.SaveChangesAsync();
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
            AssetCategory cat = await DbContext.AssetCategories.FirstOrDefaultAsync(d => d.Type == type && d.Id == catid);
            if (cat == null)
            {
                return "category " + catid + " not found"; // not found.
            }

            AssetCategory targetCat = await DbContext.AssetCategories.FirstOrDefaultAsync(d => d.Type == type && d.Id == targetCatId);
            if (targetCat == null)
            {
                return "target category " + targetCatId + " not found"; // not found.
            }

            int childrenCount = await DbContext.AssetCategories.Where(d => d.ParentId == targetCatId).CountAsync();

            cat.ParentId = targetCatId;
            cat.DisplayIndex = childrenCount; // index start from 0;
            await DbContext.SaveChangesAsync();
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
            AssetCategory cat = await DbContext.AssetCategories.FirstOrDefaultAsync(d => d.Type == type && d.Id == catid);
            if (cat == null)
            {
                return "category " + catid + " not found"; // not found.
            }

            AssetCategory targetCat = await DbContext.AssetCategories.FirstOrDefaultAsync(d => d.Type == type && d.Id == targetCatId);
            if (targetCat == null)
            {
                return "target category " + targetCatId + " not found"; // not found.
            }

            AssetCategory targetChildCat = await DbContext.AssetCategories.FirstOrDefaultAsync(d => d.Type == type && d.ParentId == targetCatId);
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
            int rows = await DbContext.Database.ExecuteSqlCommandAsync(sql);
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
            AssetCategory target = await DbContext.AssetCategories.FirstOrDefaultAsync(d => d.Type == type && d.Id == catid);
            if (target == null)
            {
                return null; // not found.
            }
            AssetCategory parent = await DbContext.AssetCategories.FirstOrDefaultAsync(d => d.Type == type && d.Id == target.ParentId);
            if (parent == null)
            {
                return null; // not found.
            }

            AssetCategoryDTO result = parent.ToDTO();

            List<AssetCategory> children = await DbContext.AssetCategories.Where(d => d.ParentId == target.ParentId).OrderBy(d => d.DisplayIndex).ToListAsync();
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

            await DbContext.SaveChangesAsync();

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


        #region SatisfyCreateAsync 判断数据是否满足存储规范
        /// <summary>
        /// 判断数据是否满足存储规范
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="data"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public async Task SatisfyCreateAsync(string accid, AssetCategory data, ModelStateDictionary modelState)
        {
            if (!string.IsNullOrWhiteSpace(data.ParentId))
            {
                var pnode = await DbContext.AssetCategories.FirstOrDefaultAsync(x => x.Id == data.ParentId);
                if (pnode == null)
                    modelState.AddModelError("ParentId", string.Format("没有找到ParentId为{0}的记录", data.ParentId));
            }
        }
        #endregion

        #region SatisfyUpdateAsync 判断数据是否满足更新规范
        /// <summary>
        /// 判断数据是否满足更新规范
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="data"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public async Task SatisfyUpdateAsync(string accid, AssetCategory data, ModelStateDictionary modelState)
        {
            await Task.FromResult(string.Empty);
        }
        #endregion

        public override async Task CreateAsync(string accid, AssetCategory data)
        {
            await base.CreateAsync(accid, data);

            #region 添加tree节点
            {
                var pNode = await DbContext.AssetCategoryTrees.Where(x => x.ObjId == data.ParentId && x.OrganizationId == data.OrganizationId).FirstOrDefaultAsync();
                if (pNode != null)
                {
                    var oTree = new AssetCategoryTree();
                    oTree.NodeType = data.Type;
                    oTree.Name = data.Name;
                    oTree.ObjId = data.Id;
                    oTree.OrganizationId = data.OrganizationId;
                    oTree.ParentId = pNode.Id;
                    await _categoryTreeStore.AddChildNode(oTree);
                }
            }
            #endregion
        }
    }
}
