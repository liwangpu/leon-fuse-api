using Apps.Base.Common.Consts;
using Apps.Basic.Data.Entities;
using Apps.Basic.Service.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apps.Basic.Service
{
    public class DatabaseInitTool
    {
        /// <summary>
        /// 竹烛组织Id
        /// </summary>
        public const string BambooOrganId = "bamboo";

        public static void InitDatabase(AppDbContext context)
        {
            #region 创建基础用户角色
            {
                var innerRoles = context.UserRoles.Where(x => x.IsInner == true).ToList();
                if (innerRoles.Count <= 0)
                    Console.WriteLine("Auto Create Inner UserRoles");

                //超级管理员
                if (innerRoles.Where(x => x.KeyWord == UserRoleConst.SysAdmin).Count() <= 0)
                {
                    var role = new UserRole();
                    role.Id = UserRoleConst.SysAdmin;
                    role.KeyWord = UserRoleConst.SysAdmin;
                    role.Name = "系统超级管理员";
                    role.Creator = "admin";
                    role.Modifier = "admin";
                    role.CreatedTime = DateTime.Now;
                    role.ModifiedTime = DateTime.Now;
                    role.ActiveFlag = 1;
                    role.IsInner = true;
                    context.UserRoles.Add(role);
                }
                //系统客服
                if (innerRoles.Where(x => x.KeyWord == UserRoleConst.SysService).Count() <= 0)
                {
                    var role = new UserRole();
                    role.Id = UserRoleConst.SysService;
                    role.KeyWord = UserRoleConst.SysService;
                    role.Name = "系统客服";
                    role.Creator = "admin";
                    role.Modifier = "admin";
                    role.CreatedTime = DateTime.Now;
                    role.ModifiedTime = DateTime.Now;
                    role.ActiveFlag = 1;
                    role.IsInner = true;
                    context.UserRoles.Add(role);
                }
                //品牌商管理员
                if (innerRoles.Where(x => x.KeyWord == UserRoleConst.BrandAdmin).Count() <= 0)
                {
                    var role = new UserRole();
                    role.Id = UserRoleConst.BrandAdmin;
                    role.KeyWord = UserRoleConst.BrandAdmin;
                    role.Name = "品牌商管理员";
                    role.Creator = "admin";
                    role.Modifier = "admin";
                    role.CreatedTime = DateTime.Now;
                    role.ModifiedTime = DateTime.Now;
                    role.ActiveFlag = 1;
                    role.IsInner = true;
                    context.UserRoles.Add(role);
                }
                //品牌商管用户
                if (innerRoles.Where(x => x.KeyWord == UserRoleConst.BrandMember).Count() <= 0)
                {
                    var role = new UserRole();
                    role.Id = UserRoleConst.BrandMember;
                    role.KeyWord = UserRoleConst.BrandMember;
                    role.Name = "品牌商用户";
                    role.Creator = "admin";
                    role.Modifier = "admin";
                    role.CreatedTime = DateTime.Now;
                    role.ModifiedTime = DateTime.Now;
                    role.ActiveFlag = 1;
                    role.IsInner = true;
                    context.UserRoles.Add(role);
                }
                //代理商管理员
                if (innerRoles.Where(x => x.KeyWord == UserRoleConst.PartnerAdmin).Count() <= 0)
                {
                    var role = new UserRole();
                    role.Id = UserRoleConst.PartnerAdmin;
                    role.KeyWord = UserRoleConst.PartnerAdmin;
                    role.Name = "代理商管理员";
                    role.Creator = "admin";
                    role.Modifier = "admin";
                    role.CreatedTime = DateTime.Now;
                    role.ModifiedTime = DateTime.Now;
                    role.ActiveFlag = 1;
                    role.IsInner = true;
                    context.UserRoles.Add(role);
                }
                //代理商用户
                if (innerRoles.Where(x => x.KeyWord == UserRoleConst.PartnerMember).Count() <= 0)
                {
                    var role = new UserRole();
                    role.Id = UserRoleConst.PartnerMember;
                    role.KeyWord = UserRoleConst.PartnerMember;
                    role.Name = "代理商用户";
                    role.Creator = "admin";
                    role.Modifier = "admin";
                    role.CreatedTime = DateTime.Now;
                    role.ModifiedTime = DateTime.Now;
                    role.ActiveFlag = 1;
                    role.IsInner = true;
                    context.UserRoles.Add(role);
                }
                //供应商管理员
                if (innerRoles.Where(x => x.KeyWord == UserRoleConst.SupplierAdmin).Count() <= 0)
                {
                    var role = new UserRole();
                    role.Id = UserRoleConst.SupplierAdmin;
                    role.KeyWord = UserRoleConst.SupplierAdmin;
                    role.Name = "供应商管理员";
                    role.Creator = "admin";
                    role.Modifier = "admin";
                    role.CreatedTime = DateTime.Now;
                    role.ModifiedTime = DateTime.Now;
                    role.ActiveFlag = 1;
                    role.IsInner = true;
                    context.UserRoles.Add(role);
                }
                //供应商用户
                if (innerRoles.Where(x => x.KeyWord == UserRoleConst.SupplierMember).Count() <= 0)
                {
                    var role = new UserRole();
                    role.Id = UserRoleConst.SupplierMember;
                    role.KeyWord = UserRoleConst.SupplierMember;
                    role.Name = "供应商用户";
                    role.Creator = "admin";
                    role.Modifier = "admin";
                    role.CreatedTime = DateTime.Now;
                    role.ModifiedTime = DateTime.Now;
                    role.ActiveFlag = 1;
                    role.IsInner = true;
                    context.UserRoles.Add(role);
                }
                context.SaveChanges();
            }
            #endregion

            #region 创建基础组织
            {
                var existBambooOrgan = context.Organizations.Where(x => x.Id == "bamboo").Count() > 0;
                if (!existBambooOrgan)
                {
                    Console.WriteLine("Auto Create Bamboo Organization");
                    var organ = new Organization();
                    organ.Id = BambooOrganId;
                    organ.Name = "竹烛信息科技有限公司";
                    organ.Creator = "admin";
                    organ.Modifier = "admin";
                    organ.CreatedTime = DateTime.Now;
                    organ.ModifiedTime = DateTime.Now;
                    organ.ActiveFlag = 1;
                    context.Organizations.Add(organ);
                    context.SaveChanges();
                }
            }
            #endregion

            #region 创建基础用户
            {
                var existAdmin = context.Accounts.Where(x => x.Id == "admin").Count() > 0;
                if (!existAdmin)
                {
                    Console.WriteLine("Auto Create Admin Account");
                    var admin = new Account();
                    admin.Id = "admin";
                    admin.Mail = "admin";
                    admin.Password = "1111";
                    admin.InnerRoleId = UserRoleConst.SysAdmin;
                    admin.Creator = "admin";
                    admin.Modifier = "admin";
                    admin.CreatedTime = DateTime.Now;
                    admin.ModifiedTime = DateTime.Now;
                    admin.ActiveFlag = 1;
                    admin.OrganizationId = BambooOrganId;
                    context.Accounts.Add(admin);
                    context.SaveChanges();
                }
            }
            #endregion

            //#region 创建基础导航栏项
            //{
            //    var navs = context.Navigations.ToList();
            //    if (navs.Where(x => x.Id == "navigation-setting").Count() <= 0)
            //    {
            //        var nav = new Navigation();
            //        nav.Id = "navigation-setting";
            //        nav.Name = "导航栏项设计器";
            //        nav.Title = "nav.NavigationItemDesigner";
            //        nav.Resource = "Nav";
            //        nav.Icon = "assignment";
            //        nav.Url = "";
            //    }
            //}
            //#endregion
        }
    }
}
