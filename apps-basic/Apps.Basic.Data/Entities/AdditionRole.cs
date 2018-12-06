namespace Apps.Basic.Data.Entities
{
    /// <summary>
    /// 用户附属角色
    /// </summary>
    public class AdditionRole
    {
        public string Id { get; set; }
        public string AccountId { get; set; }
        public Account Account { get; set; }
        public string UserRoleId { get; set; }
        public UserRole UserRole { get; set; }
    }
}
