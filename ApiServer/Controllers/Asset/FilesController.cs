using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using ApiModel;
using BambooCore;
using ApiServer.Services;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using ApiModel.Entities;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiServer.Controllers.Asset
{
    [Authorize]
    [Route("/[controller]")]
    public class FilesController : Controller
    {

        private readonly Repository<FileAsset> repo;
        private IHostingEnvironment hostEnv;
        private string uploadPath;

        public FilesController(Data.ApiDbContext context, IHostingEnvironment env)
        {
            repo = new Repository<FileAsset>(context);
            hostEnv = env;

            uploadPath = hostEnv.WebRootPath + "/upload/";
            if (Directory.Exists(uploadPath) == false)
                Directory.CreateDirectory(uploadPath);
        }

        [HttpGet]
        public async Task<PagedData> Get(string search, int page, int pageSize, string orderBy, bool desc)
        {
            PagingMan.CheckParam(ref search, ref page, ref pageSize);
            return await repo.GetAsync(AuthMan.GetAccountId(this), page, pageSize, orderBy, desc,
                d => d.Id.Contains(search) || d.Name.Contains(search) || d.Description.Contains(search));
        }

        [HttpGet("{id}")]
        [Produces(typeof(FileAsset))]
        public async Task<IActionResult> Get(string id)
        {
            //var res = await repo.GetAsync(AuthMan.GetAccountId(this), id);
            var res = await repo.Context.Set<FileAsset>().FindAsync(id); //获取单个文件的记录改为跨账号全局查询，以便客户端判断重复文件
            if (res == null)
                return NotFound();
            return Ok(res);//return Forbid();
        }

        //[HttpGet("{id}")]
        //public async Task<IActionResult> Get(string id)
        //{
        //    var res = repo.Get(AuthMan.GetAccountId(this), id);
        //    if (res == null)
        //        return NotFound();
        //    string wwwrootPath = hostEnv.WebRootPath;
        //    var fileName = Path.Combine(wwwrootPath, res.Url);
        //    //FileInfo file = new FileInfo(Path.Combine(wwwrootPath, res.Url));

        //    //if (file.Exists)
        //    //{
        //    //    file.Delete();
        //    //    file = new FileInfo(Path.Combine(wwwrootPath, fileName));
        //    //}
        //    var response = File(fileName, "application/octet-stream"); // FileStreamResult
        //    return response;
        //}

        [Route("NewOne")]
        [HttpGet]
        public FileAsset NewOne()
        {
            return EntityFactory<FileAsset>.New();
        }

        [HttpPost]
        [Produces(typeof(FileAsset))]
        public async Task<IActionResult> Post([FromBody]FileAsset value)
        {
            if (ModelState.IsValid == false)
                return BadRequest(ModelState);

            value = await repo.CreateAsync(AuthMan.GetAccountId(this), value);
            return CreatedAtAction("Get", value);
        }

        [HttpPut]
        [Produces(typeof(FileAsset))]
        public async Task<IActionResult> Put([FromBody]FileAsset value)
        {
            if (ModelState.IsValid == false)
                return BadRequest(ModelState);

            var res = await repo.UpdateAsync(AuthMan.GetAccountId(this), value);
            if (res == null)
                return NotFound();
            return Ok(value);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            bool bOk = await repo.DeleteAsync(AuthMan.GetAccountId(this), id);
            if (bOk)
                return Ok();
            return NotFound();//return Forbid();
        }

        /// <summary>
        /// 上传一个文件，文件放在body中。服务器会把此文件存在upload文件夹中，并在账号上创建一个FileAsset，并返回数据给客户端。
        /// 文件的内容可以在Header里面附加，也可以再次使用PUT /files API来修改此FileAsset。
        /// 在Header里面附加的内容一定要用urlencode封装一下，否则遇到中文会被框架拦截，返回500错误。
        /// </summary>
        /// <returns></returns>
        [Route("Upload")]
        [HttpPost]
        public async Task<FileAsset> Upload()
        {
            string fileExt = "";
            string localPath = "";
            string description = "";
            Microsoft.Extensions.Primitives.StringValues headerVar;
            Request.Headers.TryGetValue("fileExt", out headerVar); if (headerVar.Count > 0) fileExt = headerVar[0].Trim();
            Request.Headers.TryGetValue("localPath", out headerVar); if (headerVar.Count > 0) localPath = headerVar[0].Trim();
            Request.Headers.TryGetValue("description", out headerVar); if (headerVar.Count > 0) description = headerVar[0].Trim();

            localPath = System.Net.WebUtility.UrlDecode(localPath);
            description = System.Net.WebUtility.UrlDecode(description);

            //确保扩展名以 .开头，比如.jpg
            if (fileExt.Length > 0 && fileExt[0] != '.')
                fileExt = "." + fileExt;

            FileAsset res = new FileAsset();
            res.Id = GuidGen.NewGUID(); //先生成临时ID，用于保存文件
            res.Name = localPath.Length > 0 ? Path.GetFileName(localPath) : res.Id;
            res.Url = "/upload/" + res.Id;
            res.FileExt = fileExt;
            res.LocalPath = localPath;
            res.Description = description;

            //保存
            string savePath = uploadPath + res.Id;
            try
            {
                using (StreamWriter sw = new StreamWriter(savePath))
                {
                    HttpContext.Request.Body.CopyTo(sw.BaseStream);
                }
            }
            catch
            {
                res.Id = "";
                res.Url = "";
                return res;
            }
            FileInfo fi = new FileInfo(savePath);
            res.Size = fi.Length;
            res.Md5 = Md5.CalcFile(savePath); //计算md5
            res.Id = res.Md5; //将ID和url改为md5
            res.Url = "/upload/" + res.Id + res.FileExt;
            string renamedPath = uploadPath + res.Id + res.FileExt;

            // 检查是否已经上传过此文件
            var existRecord = await repo.Context.Set<FileAsset>().FindAsync(res.Id);
            if (existRecord != null)
            {
                // 数据库记录还在，但是文件不在了，重新保存下文件。
                if (System.IO.File.Exists(renamedPath) == false)
                {
                    System.IO.File.Move(savePath, renamedPath); //重命名文件
                }
                return existRecord;
            }
            else // 没有上传记录
            {
                //没上传记录，但是已经有这个文件了，先删除已有的文件，使用用户的文件覆盖
                if (System.IO.File.Exists(renamedPath))
                {
                    System.IO.File.Delete(renamedPath);
                }
                System.IO.File.Move(savePath, renamedPath); //重命名文件
            }


            await repo.CreateAsync(AuthMan.GetAccountId(this), res, false); //记录到数据库

            return res;
        }

        [Route("UploadFormFile")]
        [HttpPost]
        public async Task<IActionResult> UploadFormFile(IFormFile file)
        {
            if (file == null)
                return BadRequest();

            // 原文件名（包括路径）
            var filename = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName;
            // 扩展名
            var extName = filename.Substring(filename.LastIndexOf('.')).Replace("\"", "");
            // 新文件名
            string shortfilename = $"{Guid.NewGuid()}{extName}";
            // 新文件名（包括路径）
            filename = hostEnv.WebRootPath + @"\upload\" + shortfilename;
            // 设置文件大小
            long size = file.Length;
            // 创建新文件
            using (FileStream fs = System.IO.File.Create(filename))
            {
                file.CopyTo(fs);
                // 清空缓冲区数据
                fs.Flush();
            }

            FileAsset res = new FileAsset();
            res.Name = file.FileName;
            res.FileExt = extName;
            res.Size = file.Length;
            res.Md5 = Md5.CalcFile(filename); //计算md5
            res.Id = res.Md5; //将ID和url改为md5
            res.Url = "/upload/" + res.Id + res.FileExt;

            string renamedPath = uploadPath + res.Id + res.FileExt;

            // 检查是否已经上传过此文件
            var existRecord = await repo.Context.Set<FileAsset>().FindAsync(res.Id);
            if (existRecord != null)
            {
                // 数据库记录还在，但是文件不在了，重新保存下文件。
                if (System.IO.File.Exists(renamedPath) == false)
                {
                    System.IO.File.Move(filename, renamedPath); //重命名文件
                }
                return Ok(existRecord);
            }
            else // 没有上传记录
            {
                //没上传记录，但是已经有这个文件了，先删除已有的文件，使用用户的文件覆盖
                if (System.IO.File.Exists(renamedPath))
                {
                    System.IO.File.Delete(renamedPath);
                }
                System.IO.File.Move(filename, renamedPath); //重命名文件
            }

            var ass = await repo.CreateAsync(AuthMan.GetAccountId(this), res, false); //记录到数据库
            return Ok(ass);
        }
    }
}
