
namespace JM.BlzrUrlList.Web
{
    public class UrlApiConfig
    {
        public string EndPoint { get; set; }
        public Scopes Scopes { get; set; }
    }
    public class Scopes
    {
        public string Default { get; set; }
        public string Read { get; set; }
        public string Write { get; set; }
    }
}
