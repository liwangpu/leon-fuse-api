using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Apps.Base.Common;
using Apps.Base.Common.Controllers;
using Apps.Base.Common.Interfaces;
using Apps.FileSystem.Data.Entities;
using Apps.FileSystem.Export.Models;
using Apps.FileSystem.Service.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Apps.FileSystem.Service.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class FileController : ServiceBaseController<FileAsset>
    {
        private IHostingEnvironment hostEnv;
        private string uploadPath;

        #region 构造函数
        public FileController(IHostingEnvironment env, IRepository<FileAsset> repository)
                : base(repository)
        {
            hostEnv = env;
            uploadPath = Path.Combine(hostEnv.WebRootPath, "upload");
            if (Directory.Exists(uploadPath) == false)
                Directory.CreateDirectory(uploadPath);
        }
        #endregion

        [HttpGet("{id}")]
        public override async Task<IActionResult> Get(string id)
        {
            var res = await Task.FromResult(id);
            return Ok(res);
        }


        [Route("UploadFormFile")]
        [HttpPost]
        public async Task<IActionResult> UploadFormFile(IFormFile file)
        {
            if (file == null)
                return BadRequest();

            Action<string> saveFile = (path) =>
            {
                using (FileStream fs = System.IO.File.Create(path))
                {
                    file.CopyTo(fs);
                    // 清空缓冲区数据
                    fs.Flush();
                }
            };
            return await SaveUpload(saveFile);
        }

        #region private SaveUpload 真实保存上传文件过程
        /// <summary>
        /// 真实保存上传文件过程
        /// </summary>
        /// <param name="saveFile"></param>
        /// <returns></returns>
        private async Task<IActionResult> SaveUpload(Action<string> saveFile)
        {
            //string fileState = "";
            //string fileExt = "";
            //string localPath = "";
            //string description = "";
            //Microsoft.Extensions.Primitives.StringValues headerVar;
            //Request.Headers.TryGetValue("fileState", out headerVar); if (headerVar.Count > 0) fileState = headerVar[0].Trim();
            //Request.Headers.TryGetValue("fileExt", out headerVar); if (headerVar.Count > 0) fileExt = headerVar[0].Trim();
            //Request.Headers.TryGetValue("localPath", out headerVar); if (headerVar.Count > 0) localPath = headerVar[0].Trim();
            //Request.Headers.TryGetValue("description", out headerVar); if (headerVar.Count > 0) description = headerVar[0].Trim();


            ////几个头信息decode
            //if (!string.IsNullOrWhiteSpace(fileState))
            //    fileState = System.Web.HttpUtility.UrlDecode(fileState);
            //if (!string.IsNullOrWhiteSpace(fileExt))
            //    fileExt = System.Web.HttpUtility.UrlDecode(fileExt);
            //if (!string.IsNullOrWhiteSpace(localPath))
            //    localPath = System.Web.HttpUtility.UrlDecode(localPath);
            //if (!string.IsNullOrWhiteSpace(description))
            //    description = System.Web.HttpUtility.UrlDecode(description);

            ////确保扩展名以 .开头，比如.jpg
            //if (fileExt.Length > 0 && fileExt[0] != '.')
            //    fileExt = "." + fileExt;

            //var accid = AuthMan.GetAccountId(this);
            //var account = await _Repository._DbContext.Accounts.FindAsync(accid);




            //FileAsset res = new FileAsset();
            //res.Id = GuidGen.NewGUID(); //先生成临时ID，用于保存文件
            //res.Name = localPath.Length > 0 ? Path.GetFileName(localPath) : res.Id;
            //res.Url = "/upload/" + res.Id;
            //int fstate = 0;
            //res.FileState = int.TryParse(fileState, out fstate) ? fstate : 0;
            //res.FileExt = fileExt;
            //res.LocalPath = localPath;
            //res.Description = description;
            //res.Creator = accid;
            //res.Modifier = accid;
            //res.OrganizationId = account.OrganizationId;


            //保存
            string savePath = Path.Combine(uploadPath, Guid.NewGuid().ToString());
            try
            {
                saveFile(savePath);
            }
            catch
            {
                //res.Id = "";
                //res.Url = "";
                //return Ok(res);
            }

            return Ok();
        }
        #endregion
    }
}