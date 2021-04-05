using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JM.BlzrUrlList.Models.Repository
{
    public interface IUrlRepository
    {
        Task<UrlList> Get(string urlId);
        Task<UrlList> Save(UrlList urlList);
        Task<UrlList> Delete(string urlId);
        IList<UrlList> GetListForUser(string userId);
    }
}
