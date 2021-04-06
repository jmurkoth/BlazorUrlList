using JM.BlzrUrlList.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JM.BlzrUrlList.Core.Repository
{
    public interface IUrlRepository
    {
        Task<UrlList> Get(string urlId);
        Task<UrlList> Save(UrlList urlList);
        Task<UrlList> Delete(string urlId);
        IList<UrlList> GetListForUser(string userId);
    }
}
