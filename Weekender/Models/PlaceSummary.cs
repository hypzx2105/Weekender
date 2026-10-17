namespace Weekender.Models
{
   
    public class PlaceSummary
    {
        
        private string title;
        private string description;
        private string extract;
        private string imageUrl;
        private string pageUrl;

       
        public string Title { get { return title; } set { title = value; } }
        public string Description { get { return description; } set { description = value; } }
        public string Extract { get { return extract; } set { extract = value; } }   
        public string ImageUrl { get { return imageUrl; } set { imageUrl = value; } }
        public string PageUrl { get { return pageUrl; } set { pageUrl = value; } }

        public bool HasImage
        {
            get { return !string.IsNullOrWhiteSpace(imageUrl); }
        }

        
        public PlaceSummary()
        {
            title = string.Empty;
            description = string.Empty;
            extract = string.Empty;
            imageUrl = string.Empty;
            pageUrl = string.Empty;
        }

        public PlaceSummary(string title, string description, string extract, string imageUrl, string pageUrl)
        {
            this.title = title;
            this.description = description;
            this.extract = extract;
            this.imageUrl = imageUrl;
            this.pageUrl = pageUrl;
        }

       
        public override string ToString()
        {
            return $"PlaceSummary - {title}: {description}";
        }
    }
}