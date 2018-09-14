using Newtonsoft.Json;
using System;

namespace ApiModel.Entities
{
    public class OrderStateItem : DataBase
    {
        public string Description { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime ModifiedTime { get; set; }
        public string Creator { get; set; }
        public string Modifier { get; set; }
        public string OldState { get; set; }
        public string NewState { get; set; }

        [JsonIgnore]
        public string OrderId { get; set; }
        [JsonIgnore]
        public Order Order { get; set; }
        [JsonIgnore]
        public string SolutionId { get; set; }
        [JsonIgnore]
        public Solution Solution { get; set; }
    }
}
