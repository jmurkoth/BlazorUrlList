using JM.BlzrUrlList.Exceptions;
using JM.BlzrUrlList.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace JM.BlzrUrlList.Core.Repository
{
    public class InMemoryUrlRepository : IUrlRepository
    {
        private static Dictionary<string, UrlList> _inMemoryList = new Dictionary<string, UrlList>();
        public InMemoryUrlRepository()
        {
           
        }
        public Task<UrlList> Delete(string urlId)
        {
            UrlList match;
            if (!_inMemoryList.TryGetValue(urlId, out match))
            {
                throw new UrlListNotFoundException(urlId);
            }
            _inMemoryList.Remove(urlId);
            return Task.FromResult(match);
        }

        public Task<UrlList> Get(string urlId)
        {
            UrlList match;
           if(!_inMemoryList.TryGetValue(urlId, out match))
            {
                throw new UrlListNotFoundException(urlId);
            }
            return Task.FromResult(match);
        }

        public Task<IList<UrlList>> GetListForUser(string userId)
        {
            IList<UrlList> matches = _inMemoryList.Where(c => c.Value.UserId.Equals(userId, StringComparison.OrdinalIgnoreCase)).Select(d => d.Value).ToList();
            return Task.FromResult(matches);
        }

        public Task<UrlList> Save(UrlList urlList)
        {
            _inMemoryList.Add(urlList.UrlId, urlList);
            return Task.FromResult(urlList);
        }
    }
}
