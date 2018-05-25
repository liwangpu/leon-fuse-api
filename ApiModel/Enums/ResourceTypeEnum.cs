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
        /// 品牌商资源
        /// </summary>
        Brand = 3000,
        /// <summary>
        /// 合伙人资源
        /// </summary>
        Partner = 3100,
        /// <summary>
        /// 供应商资源
        /// </summary>
        Supplier = 3200,
        /// <summary>
        /// 组织的资源共享方案之一
        /// 组织的下级可以看到父组织的资源但是不能修改
        /// 组织(包括父组织和子组织)资源的编辑权限在自己本身,即父组织的资源由它自己编辑,子组织的资源也由它自己编辑
        /// 子组织之间无法查看对方的,只能查看父组织的资源,但是不能编辑
        /// </summary>
        Organizational_DownView_UpView_OwnEdit = 3300,
        /// <summary>
        /// 资源文件不限制,完全开放状态,全平台共享
        /// </summary>
        NoLimit = -1,
    }
}
