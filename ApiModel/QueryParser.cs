using ApiModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApiModel
{
    public class QueryParser
    {
        public static List<QueryFilter> Parse(string q)
        {
            var filters = new List<QueryFilter>();
            if (!string.IsNullOrWhiteSpace(q))
            {

            }
            return filters;
        }
    }
}
