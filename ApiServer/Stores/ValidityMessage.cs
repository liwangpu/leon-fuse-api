namespace ApiServer.Stores
{
    public class ValidityMessage
    {
        public static string V_NoPermissionMsg = "对不起,您没有权限操作该条记录";
        public static string V_SubmitDataMsg = "对不起,提交数据不能为空";
        public static string V_NotDataOrPermissionMsg = "对不起,操作记录不存在或您没有权限进行该操作";
        public static string V_StringLengthRejectMsg = "对不起,{0}必须为1-{1}个字符信息";
        public static string V_RequiredRejectMsg = "对不起,{0}为必填信息";
        public static string V_NotReferenceMsg = "对不起,关联{0}记录不存在";
        public static string V_NoCreateAccPermissionMsg = "对不起,您没有权限创建该类型用户";
        public static string V_DuplicatedMsg = "对不起,系统已经存在{0}为\"{1}\"的信息";
    }
}
