using JM.BlzrUrList.Core.DB;
using JM.BlzrUrlList.Core.Repository;
using JM.BlzrUrlList.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JM.BlzrUrList.Core.Repository
{
    public class MongoUrlRepository : IUrlRepository
    {
        private readonly IMongoCollection<UrlList> _urlListCollection;
        public MongoUrlRepository(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _urlListCollection = database.GetCollection<UrlList>(settings.CollectionName);
        }
        public async Task<UrlList> Delete(string urlId)
        {
            var result = await _urlListCollection.FindOneAndDeleteAsync<UrlList>(c => c.UrlId == urlId);
            return result;
        }

        public async Task<UrlList> Get(string urlId)
        {
            var result = await _urlListCollection.FindAsync(c => c.UrlId == urlId);
            return result.FirstOrDefault();
        }

        public async Task<IList<UrlList>> GetListForUser(string userId)
        {
            var result = await _urlListCollection.FindAsync(c => c.UserId == userId);
            return result.ToList();
        }

        public async Task<UrlList> Save(UrlList urlList)
        {
            await _urlListCollection.InsertOneAsync(urlList);
            return urlList;
        }
    }
}
