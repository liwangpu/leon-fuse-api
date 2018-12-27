
using Apps.Base.Common.Consts;
using Apps.OMS.Data.Consts;
using Apps.OMS.Data.Entities;
using Apps.OMS.Service.Contexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apps.OMS.Service
{
    public class DatabaseInitTool
    {
        /// <summary>
        /// 初始化数据库信息
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="context"></param>
        public static void InitDatabase(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider, AppDbContext context)
        {

            #region 积分参数初始值
            {
                var hierachies = context.MemberHierarchyParams.Where(x => x.IsInner == true).ToList();
                if (hierachies.Count <= 0)
                    Console.WriteLine("Auto Create Inner Member Hierarchy Param");

                if (hierachies.Where(x => x.Id == MemberHierarchyParamConst.FirstHierarchy).Count() <= 0)
                {
                    var param = new MemberHierarchyParam();
                    param.Id = MemberHierarchyParamConst.FirstHierarchy;
                    param.Name = "第一级";
                    param.Creator = AppConst.BambooAdminId;
                    param.Modifier = AppConst.BambooAdminId;
                    param.CreatedTime = DateTime.Now;
                    param.ModifiedTime = DateTime.Now;
                    param.IsInner = true;
                    context.MemberHierarchyParams.Add(param);
                }

                if (hierachies.Where(x => x.Id == MemberHierarchyParamConst.SecondHierarchy).Count() <= 0)
                {
                    var param = new MemberHierarchyParam();
                    param.Id = MemberHierarchyParamConst.SecondHierarchy;
                    param.Name = "第二级";
                    param.Creator = AppConst.BambooAdminId;
                    param.Modifier = AppConst.BambooAdminId;
                    param.CreatedTime = DateTime.Now;
                    param.ModifiedTime = DateTime.Now;
                    param.IsInner = true;
                    context.MemberHierarchyParams.Add(param);
                }

                if (hierachies.Where(x => x.Id == MemberHierarchyParamConst.ThirdHierarchy).Count() <= 0)
                {
                    var param = new MemberHierarchyParam();
                    param.Id = MemberHierarchyParamConst.ThirdHierarchy;
                    param.Name = "第三级";
                    param.Creator = AppConst.BambooAdminId;
                    param.Modifier = AppConst.BambooAdminId;
                    param.CreatedTime = DateTime.Now;
                    param.ModifiedTime = DateTime.Now;
                    param.IsInner = true;
                    context.MemberHierarchyParams.Add(param);
                }
                context.SaveChanges();
            }
            #endregion
        }
    }
}
