using System;
using System.Collections.Generic;

namespace JM.BlzrUrlList.Models
{
    public record  UrlList
    {
        public string UserId { get; set; }
        public string UrlId { get; set; }
        public string Description { get; set; }
        public IList<CustomUrl>   Urls { get; set; }
    }
}
