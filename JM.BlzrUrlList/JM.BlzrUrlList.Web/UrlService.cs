using JM.BlzrUrlList.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace JM.BlzrUrlList.Web
{
    public class UrlService
    {
       
        private readonly IHttpClientFactory httpClientFactory;

        public UrlService(IHttpClientFactory httpClientFactory)
        {
           
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<UrlList> SaveUrlList(UrlList urlList)
        {
            var clnt = this.httpClientFactory.CreateClient("UrlApi");

            var result = await clnt.PostAsJsonAsync($"/api/UrlList/", urlList);
            if (result.IsSuccessStatusCode)
            {
                return await result.Content.ReadFromJsonAsync<UrlList>();
            }
            else
            {
                return null;
            }
        }
        public async Task<UrlList> GetUrlList(string urlId)
        {
            var clnt = this.httpClientFactory.CreateClient("UrlApi");
            var result = await clnt.GetAsync($"api/UrlList/{urlId}");
            if (result.IsSuccessStatusCode)
            {
                return await result.Content.ReadFromJsonAsync<UrlList>();
            }
            else
            {
                return null;
            }
        }
        public async Task<OpenGraphResult> GetUrlMetaData(string url)
        {
            var clnt = this.httpClientFactory.CreateClient("UrlApi");
            
            var result = await clnt.PostAsJsonAsync($"/api/OpenGraph/",new OpenGraphRequest(  url ));
            //return await Task.FromResult<OpenGraphResult>( new OpenGraphResult("www.news.google.com", "Google News",
            //               "https://lh3.googleusercontent.com/J6_coFbogxhRI9iM864NL_liGXvsQp2AupsKei7z0cNNfDvGUmWUy20nuUhkREQyrpY4bEeIBuc=w300",
            //                "Comprehensive up-to-date news coverage, aggregated from sources all over the world by Google News."
            //                            ));
            if(result.IsSuccessStatusCode)
            {
                return await result.Content.ReadFromJsonAsync<OpenGraphResult>();
            }
            else
            {
                return null;
            }
            
        }
    }
}
