using ApiModel;
using ApiModel.Enums;
using ApiServer.Filters;
using ApiServer.Models;
using ApiServer.Services;
using ApiServer.Stores;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ApiServer.Controllers
{
    /// <summary>
    /// 通用RestFull CRUD常用请求处理控制器
    /// </summary>
    /// <typeparam name="T">实体对象</typeparam>
    /// <typeparam name="DTO">DTO实体对象</typeparam>
    public class CommonController<T, DTO> : Controller
         where T : class, IEntity, IDTOTransfer<DTO>, new()
           where DTO : class, IData, new()
    {
        protected bool RequestValid;
        protected IStore<T, DTO> _Store;

        #region 构造函数
        public CommonController(IStore<T, DTO> store)
        {
            _Store = store;
        }
        #endregion

        #region _GetPagingRequest 根据查询参数获取分页信息
        /// <summary>
        /// 根据查询参数获取分页信息
        /// </summary>
        /// <param name="model"></param>
        /// <param name="qMapping"></param>
        /// <param name="advanceQuery"></param>
        /// <returns></returns>
        protected async Task<IActionResult> _GetPagingRequest(PagingRequestModel model, Action<List<string>> qMapping = null, Func<IQueryable<T>, Task<IQueryable<T>>> advanceQuery = null)
        {
            var accid = AuthMan.GetAccountId(this);
            var qs = new List<string>();
            qMapping?.Invoke(qs);
            if (qs.Count > 0)
            {
                var builder = new StringBuilder();
                foreach (var item in qs)
                    builder.AppendFormat(";{0}", item);
                model.Q = builder.ToString();
            }
            var result = await _Store.SimplePagedQueryAsync(model, accid, advanceQuery);
            return Ok(StoreBase<T, DTO>.PageQueryDTOTransfer(result));
        }
        #endregion

        #region _GetByIdRequest 根据id获取信息
        /// <summary>
        /// 根据id获取信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected async Task<IActionResult> _GetByIdRequest(string id)
        {
            var accid = AuthMan.GetAccountId(this);
            var exist = await _Store.ExistAsync(id);
            if (!exist)
                return NotFound();
            var canRead = await _Store.CanReadAsync(accid, id);
            if (!canRead)
                return Forbid();
            var dto = await _Store.GetByIdAsync(id);
            return Ok(dto);
        }
        #endregion

        #region _PostRequest 处理Post请求信息
        /// <summary>
        /// 处理Post请求信息
        /// </summary>
        /// <param name="mapping"></param>
        /// <param name="handle"></param>
        /// <returns></returns>
        protected async Task<IActionResult> _PostRequest(Func<T, Task<T>> mapping, Func<T, Task<IActionResult>> handle = null)
        {
            var accid = AuthMan.GetAccountId(this);
            var metadata = new T();
            var canCreate = await _Store.CanCreateAsync(accid);
            if (!canCreate)
                return Forbid();
            var data = await mapping(metadata);
            await _Store.SatisfyCreateAsync(accid, data, ModelState);
            if (!ModelState.IsValid)
                return new ValidationFailedResult(ModelState);
            //如果handle不为空,由handle掌控Create流程和ActionResult
            if (handle != null)
                return await handle(data);
            await _Store.CreateAsync(accid, data);
            var dto = await _Store.GetByIdAsync(metadata.Id);
            return Ok(dto);
        }
        #endregion

        #region _PutRequest 处理Put请求
        /// <summary>
        /// 处理Put请求
        /// </summary>
        /// <param name="id"></param>
        /// <param name="mapping"></param>
        /// <param name="handle"></param>
        /// <returns></returns>
        protected async Task<IActionResult> _PutRequest(string id, Func<T, Task<T>> mapping, Func<T, Task<IActionResult>> handle = null)
        {
            var exist = await _Store.ExistAsync(id);
            if (!exist)
                return NotFound();
            var accid = AuthMan.GetAccountId(this);
            var permission = await _Store.CanUpdateAsync(accid, id);
            if (!permission)
                return Forbid();
            var metadata = await _Store._GetByIdAsync(id);
            var entity = await mapping(metadata);
            metadata.Id = id;
            await _Store.SatisfyUpdateAsync(accid, entity, ModelState);
            if (!ModelState.IsValid)
                return new ValidationFailedResult(ModelState);
            //如果handle不为空,由handle掌控Update流程和ActionResult
            if (handle != null)
                return await handle(entity);
            await _Store.UpdateAsync(accid, entity);
            var dto = await _Store.GetByIdAsync(entity.Id);
            return Ok(dto);
        }
        #endregion

        #region _DeleteRequest 处理Delete请求
        /// <summary>
        /// 处理Delete请求
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected async Task<IActionResult> _DeleteRequest(string id)
        {
            var accid = AuthMan.GetAccountId(this);
            var exist = await _Store.ExistAsync(id);
            if (!exist)
                return NotFound();

            var canDelete = await _Store.CanDeleteAsync(accid, id);
            if (!canDelete)
                return Forbid();
            await _Store.DeleteAsync(accid, id);
            return Ok();
        }
        #endregion

        #region _ImportRequest 处理导入请求
        /// <summary>
        /// 处理导入请求
        /// </summary>
        /// <typeparam name="CSV"></typeparam>
        /// <param name="file"></param>
        /// <param name="importOp"></param>
        /// <param name="doneOp"></param>
        /// <returns></returns>
        protected async Task<IActionResult> _ImportRequest<CSV>(IFormFile file, Func<CSV, Task<string>> importOp, Action doneOp = null)
              where CSV : class, ImportData, new()
        {
            if (file == null)
                return BadRequest();
            var tmpFileName = Path.GetTempFileName();
            var errorRecords = new List<CSV>();
            try
            {
                using (FileStream fs = System.IO.File.Create(tmpFileName))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
                using (var sr = new StreamReader(tmpFileName, Encoding.UTF8))
                {
                    var reader = new CsvReader(sr);
                    reader.Configuration.HeaderValidated = null;
                    reader.Configuration.MissingFieldFound = null;
                    var records = reader.GetRecords<CSV>().ToList();
                    foreach (var item in records)
                    {
                        var msg = await importOp(item);
                        if (!string.IsNullOrWhiteSpace(msg))
                        {
                            item.ErrorMsg = msg;
                            errorRecords.Add(item);
                        }
                    }
                }
                doneOp?.Invoke();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Import Data", ex.Message);
                return new ValidationFailedResult(ModelState);
            }
            finally
            {
                if (System.IO.File.Exists(tmpFileName))
                    System.IO.File.Delete(tmpFileName);
            }

            if (errorRecords.Count > 0)
            {
                var tmpResFileName = Path.GetTempFileName();
                using (var fs = new FileStream(tmpResFileName, FileMode.OpenOrCreate))
                {
                    using (TextWriter writer = new StreamWriter(fs, Encoding.UTF8))
                    {
                        var config = new Configuration();
                        config.Encoding = Encoding.UTF8;
                        var csv = new CsvWriter(writer, config);
                        csv.WriteRecords(errorRecords);
                    }
                }
                return File(new FileStream(tmpResFileName, FileMode.Open), "application/octet-stream");
            }

            return Ok();
        }
        #endregion

        /**************** common method ****************/

        #region Delete 删除数据信息
        /// <summary>
        /// 删除用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete(string id)
        {
            return await _DeleteRequest(id);
        }
        #endregion

        #region KeyWordSearchQ 基本关键词查询
        /// <summary>
        /// 基本关键词查询
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public static List<string> KeyWordSearchQ(string keyword)
        {
            var list = new List<string>();
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                list.Add(string.Format("Name like {0}", keyword));
                //list.Add(string.Format("Description={0}", keyword));
            }
            return list;
        }
        #endregion


        protected class ImportMap<U> : ClassMap<U>, ImportData
              where U : class, new()
        {
            public ImportMap()
            {
                AutoMap();
            }
            public string ErrorMsg { get; set; }
        }

        protected interface ImportData
        {
            string ErrorMsg { get; set; }
        }

    }
}



