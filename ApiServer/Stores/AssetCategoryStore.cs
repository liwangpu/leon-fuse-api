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

        #region CreateAsync 创建分类信息
        /// <summary>
        /// 创建分类信息
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="data"></param>
        /// <returns></returns>
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
        #endregion
    }
}
