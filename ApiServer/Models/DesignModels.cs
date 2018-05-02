namespace ApiServer.Models
{
    #region ProductEditModel 产品编辑模型
    /// <summary>
    /// 产品编辑模型
    /// </summary>
    public class ProductEditModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string FolderId { get; set; }
        public string CategoryId { get; set; }
    }
    #endregion

    #region ProductSpecEditModel 产品规格编辑模型
    /// <summary>
    /// 产品规格编辑模型
    /// </summary>
    public class ProductSpecEditModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ProductId { get; set; }
        public string CategoryId { get; set; }
    }
    #endregion

    #region SolutionEditModel 解决方案编辑模型
    /// <summary>
    /// 解决方案编辑模型
    /// </summary>
    public class SolutionEditModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
    #endregion
}
