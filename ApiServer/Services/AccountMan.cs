﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiModel;
using BambooCore;
using BambooCommon;
using Microsoft.EntityFrameworkCore;
using ApiServer.Models;
using ApiModel.Entities;

namespace ApiServer.Services
{
    public class AccountMan
    {
        public Data.ApiDbContext context;

        public AccountMan(Data.ApiDbContext context)
        {
            this.context = context;
        }

        public async Task<AccountModel> Register(RegisterAccountModel param)
        {
            var model = new AccountModel();
            if (param == null)
                return null;
            if (string.IsNullOrWhiteSpace(param.Email))
                return null;
            string mail = param.Email.Trim().ToLower();

            Account acc = await context.Accounts.FirstOrDefaultAsync(d => d.Mail == mail);
            if (acc == null)
            {
                acc = new Account();
                acc.Id = GuidGen.NewGUID();
                acc.Name = param.Name;
                acc.Password = param.Password;
                acc.Mail = mail;
                acc.Password = param.Password;
                acc.Frozened = false;
                acc.ActivationTime = DateTime.UtcNow;
                acc.ExpireTime = DateTime.UtcNow.AddYears(10);
                acc.Type = "";
                context.Accounts.Add(acc);
                await context.SaveChangesAsync();
            }
            model.Name = param.Name;
            model.Id = acc.Id;
            model.Mail = acc.Mail;
            model.Password = acc.Password;
            return model;
        }

        public async Task<bool> ChangePasswordAsync(string accid, NewPasswordModel param)
        {
            if (param == null)
                return false;
            Account acc = await context.Accounts.FindAsync(accid);
            if (acc == null)
                return false;
            if (param.OldPassword != acc.Password)
                return false;
            acc.Password = param.NewPassword;

            await context.SaveChangesAsync();
            return true;
        }

        public async Task<AccountProfileModel> GetProfile(string accid)
        {
            Account acc = await context.Accounts.FindAsync(accid);
            if (acc == null)
                return null;
            AccountProfileModel p = new AccountProfileModel();
            p.Nickname = acc.Name;
            p.Avatar = acc.Icon;
            p.Brief = acc.Description;
            p.Location = acc.Location;
            return p;
        }

        public async Task<bool> UpdateProfile(string accid, AccountProfileModel param)
        {
            if (param == null)
                return false;
            Account acc = await context.Accounts.FindAsync(accid);
            if (acc == null)
                return false;
            acc.Name = param.Nickname;
            acc.Icon = param.Avatar;
            acc.Description = param.Brief;
            acc.Location = param.Location;

            await context.SaveChangesAsync();
            return true;
        }

        public async Task<NavigationModel> GetNavigation(string accid)
        {
            Account acc = await context.Accounts.FindAsync(accid);
            if (acc != null)
            {
                NavigationModel mm;
                if (SiteConfig.Instance.GetItem("navi_" + acc.Type, out mm))
                    return mm;
            }
            return null;
        }
    }
}
