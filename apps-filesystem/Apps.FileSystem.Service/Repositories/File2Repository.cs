using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Apps.FileSystem.Data.Entities;

namespace Apps.FileSystem.Service.Repositories
{
    public class FileRepository
    {
        private AppConfig appConfig { get; }
        private readonly IMongoCollection<FileAsset> _FilesDB;

        #region 构造函数
        public FileRepository(IOptions<AppConfig> settingsOptions)
        {
            appConfig = settingsOptions.Value;
            var client = new MongoClient(appConfig.MongoDBConnectionString);
            var database = client.GetDatabase("FileStoreDb");
            _FilesDB = database.GetCollection<FileAsset>("FileAssets");
        }
        #endregion

        public List<FileAsset> Get()
        {
            return _FilesDB.Find(fs => true).ToList();
        }

        public FileAsset Get(string id)
        {
            return _FilesDB.Find(x => x.Id == id).FirstOrDefault();
        }

        public void Create(FileAsset file)
        {
            _FilesDB.InsertOne(file);
        }

        public void Update(FileAsset file)
        {
            //var docId = new ObjectId(file.Id);
            //_FilesDB.ReplaceOne(x => x.Id == docId, file);
        }
    }
}
