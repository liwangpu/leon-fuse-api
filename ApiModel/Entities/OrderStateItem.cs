using Newtonsoft.Json;
using System;

namespace ApiModel.Entities
{
    public class OrderStateItem : DataBase
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string OldState { get; set; }
        public string NewState { get; set; }
        public string OperatorAccount { get; set; }
        public DateTime OperateTime { get; set; }
        public string Reason { get; set; }
        public string Detail { get; set; }

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
