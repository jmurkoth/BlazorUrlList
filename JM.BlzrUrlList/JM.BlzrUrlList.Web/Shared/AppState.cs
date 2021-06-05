using JM.BlzrUrlList.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JM.BlzrUrlList.Web.Shared
{
    public class AppState
    {
        private readonly UrlList _urlList = new UrlList();
        public UrlList CurrentList { get { return _urlList; } }

        public void SetTitleAndDescription(string title, string description)
        {
            _urlList.Description = description;
            _urlList.UrlId = title;
        }
        public void AddUrl(CustomUrl url)
        {
            if(_urlList.Urls==null)
            {
                _urlList.Urls = new List<CustomUrl>();
            }    
            _urlList.Urls.Add(url);
        }

        public bool Validate()
        {
            bool isValid = false;
            isValid = _urlList.Urls?.Count >0 ;
            return isValid;
        }
    }
}
