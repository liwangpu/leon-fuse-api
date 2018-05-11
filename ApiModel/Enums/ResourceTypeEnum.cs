namespace ApiModel.Enums
{
    public enum ResourceTypeEnum
    {
        /// <summary>
        /// 私人的,除了本人谁也无法访问
        /// </summary>
        Personal = 0,
        /// <summary>
        /// 部门的资源,对部门完全开放
        /// </summary>
        Departmental = 100,
        /// <summary>
        /// 组织的资源,该组织人员可以查看
        /// </summary>
        Organizational = 200,
        /// <summary>
        /// 资源文件不限制,完全开放状态,全平台共享
        /// </summary>
        NoLimit = 300,
    }
}
