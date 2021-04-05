﻿using JM.BlzrUrlList.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JM.BlzrUrlList.Models.Repository
{
    public class InMemoryUrlRepository : IUrlRepository
    {
        private static Dictionary<string, UrlList> _inMemoryList = new Dictionary<string, UrlList>();
        public InMemoryUrlRepository()
        {
           
        }
        public void Delete(string urlId)
        {
            if(!_inMemoryList.ContainsKey(urlId))
            {
                throw new UrlListNotFoundException(urlId);
            }
            _inMemoryList.Remove(urlId);
        }

        public UrlList Get(string urlId)
        {
            UrlList match;
           if(!_inMemoryList.TryGetValue(urlId, out match))
            {
                throw new UrlListNotFoundException(urlId);
            }
            return match;
        }

        public IList<UrlList> GetListForUser(string userId)
        {
            throw new NotImplementedException();
        }

        public void Save(UrlList urlList)
        {
            _inMemoryList.Add(urlList.UrlId, urlList);
        }
    }
}
