using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JM.BlzrUrlList.Models.Repository
{
    public interface IUrlRepository
    {
        UrlList Get(string urlId);
        void Save(UrlList urlList);
        void Delete(string urlId);
        IList<UrlList> GetListForUser(string userId);
    }
}
