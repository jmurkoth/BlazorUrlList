using JM.BlzrUrlList.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JM.BlzrUrlList.Core.Repository
{
    public interface IOpenGraphRepository
    {
        Task<OpenGraphResult> GetInfo(string url);
    }
}
