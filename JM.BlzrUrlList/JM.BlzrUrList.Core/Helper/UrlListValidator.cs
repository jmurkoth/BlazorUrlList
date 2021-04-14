using JM.BlzrUrlList.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace JM.BlzrUrList.Core.Helper
{
    public static class UrlListValidator
    {
        public static bool Validate (this UrlList urlList )
        {
            bool isValid = false;
            isValid = (urlList != null) && urlList.Urls.Any();
            if (!isValid)
            {
                //TODO: may be add some custom message later on
            }
            return isValid;
        }
    }
}
