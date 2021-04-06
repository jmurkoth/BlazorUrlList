namespace JM.BlzrUrlList.Models
{
    public record OpenGraphResult
    {
        public string Id { get;}
        public string Title { get;}
        public string Image { get;  }
        public string Description { get;}
        public OpenGraphResult(string id, string title, string image, string description) 
            => (Id, Title, Image, Description) = (id, title, image, description);

      
    }
}