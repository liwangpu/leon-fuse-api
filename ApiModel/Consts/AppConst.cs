namespace ApiModel.Consts
{
    public class AppConst
    {
        public const string AccountType_SysAdmin = "sysadmin";
        public const string AccountType_SysService = "sysservice";
        public const string AccountType_OrganAdmin = "organadmin";
        public const string AccountType_OrganMember = "organmember";


        public const int I_DataState_InActive = 0;
        public const int I_DataState_Active = 1;
        //权限树节点类型
        public const string S_NodeType_Organization = "Organization";
        public const string S_NodeType_Department = "Department";
        public const string S_NodeType_Account = "Account";
        //分类树节点类型
        public const string S_NodeType_Product = "Product";

        public const string S_QueryOperate_Eq = "eq";
        public const string S_QueryOperate_Lt = "lt";
        public const string S_QueryOperate_Le = "le";
        public const string S_QueryOperate_Gt = "gt";
        public const string S_QueryOperate_Ge = "ge";
        public const string S_QueryOperate_Like = "like";

        /// <summary>
        /// 不同层级之间差距
        /// </summary>
        public const int I_Permission_GradeStep = 10;
        /// <summary>
        /// 不同类型直接差距
        /// </summary>
        public const int I_Permission_TypeStep = 100;


        public const string S_Category_Product = "product";

    }
}
