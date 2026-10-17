using Newtonsoft.Json;

namespace Weekender.ApiModels
{
    

    //1) REST API page summary,  https://en.wikipedia.org/api/rest_v1/page/summary/Lisbon
    public class WikipediaSummaryResponse
    {
        [JsonProperty("type")]
        public string? Type { get; set; } 

        [JsonProperty("title")]
        public string? Title { get; set; }

        [JsonProperty("description")]
        public string? Description { get; set; }

        [JsonProperty("extract")]
        public string? Extract { get; set; }

        [JsonProperty("thumbnail")]
        public WikipediaImage? Thumbnail { get; set; }

        [JsonProperty("originalimage")]
        public WikipediaImage? OriginalImage { get; set; }

        [JsonProperty("content_urls")]
        public WikipediaContentUrls? ContentUrls { get; set; }

        [JsonProperty("coordinates")]
        public WikipediaCoordinate? Coordinates { get; set; }
    }

    public class WikipediaContentUrls
    {
        [JsonProperty("desktop")]
        public WikipediaPageLink? Desktop { get; set; }
    }

    public class WikipediaPageLink
    {
        [JsonProperty("page")]
        public string? Page { get; set; }
    }

    //2) Action API geosearch: articles about places near some coordinates, 
    //https://en.wikipedia.org/w/api.php?action=query&generator=geosearch&ggscoord=38.72|-9.15&ggsradius=10000&prop=pageimages|description|coordinates&format=json&formatversion=2
    public class WikipediaQueryResponse
    {
        [JsonProperty("query")]
        public WikipediaQuery? Query { get; set; }
    }

    public class WikipediaQuery
    {
        [JsonProperty("pages")]
        public List<WikipediaPage>? Pages { get; set; }
    }

    public class WikipediaPage
    {
        [JsonProperty("title")]
        public string Title { get; set; } = string.Empty;

        [JsonProperty("description")]
        public string? Description { get; set; }

        [JsonProperty("thumbnail")]
        public WikipediaImage? Thumbnail { get; set; }

        [JsonProperty("coordinates")]
        public List<WikipediaCoordinate>? Coordinates { get; set; }
    }

   
    public class WikipediaImage
    {
        [JsonProperty("source")]
        public string? Source { get; set; }
    }

    public class WikipediaCoordinate
    {
        [JsonProperty("lat")]
        public double Lat { get; set; }

        [JsonProperty("lon")]
        public double Lon { get; set; }
    }
}