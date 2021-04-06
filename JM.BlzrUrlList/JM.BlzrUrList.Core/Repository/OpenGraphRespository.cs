using HtmlAgilityPack;
using JM.BlzrUrlList.Core.Repository;
using JM.BlzrUrlList.Models;
using OpenGraphNet;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace JM.BlzrUrList.Core.Repository
{
    public class OpenGraphRespository : IOpenGraphRepository
    {
        public async Task<OpenGraphResult> GetInfo(string url)
        {
            OpenGraphResult result=null;
            OpenGraph openGraph = null;
            try
            {
                openGraph = await OpenGraph.ParseUrlAsync(url);
            }
            catch 
            {
                result = new OpenGraphResult(url, string.Empty, string.Empty, string.Empty);

            }

            if(openGraph!=null)
            {
                var image = openGraph.Image?.ToString();
                var titleFromMetaData = openGraph.Title;
                var descriptionFromMetaData = openGraph.Metadata.FirstOrDefault(c => c.Key.Equals("og:description")).Value?.FirstOrDefault().Value;
                HtmlDocument htmldoc = new HtmlDocument();
                htmldoc.LoadHtml(openGraph.OriginalHtml);
                // grab the description metatag
                var descriptionFromTag = htmldoc.DocumentNode.SelectSingleNode("//meta[@name='description']")?.Attributes["content"]?.Value;
                //grab the title metatag
                var titleFromTag = htmldoc.DocumentNode.SelectSingleNode("//head/title").InnerText?.Trim();

                var title = HtmlEntity.DeEntitize( String.IsNullOrEmpty(titleFromMetaData)? titleFromTag : titleFromMetaData);
                var description = HtmlEntity.DeEntitize(String.IsNullOrEmpty(descriptionFromMetaData) ? descriptionFromTag: descriptionFromMetaData);
                result = new OpenGraphResult(url, title, image, description);
            }

            return result;
        }
    }
}
