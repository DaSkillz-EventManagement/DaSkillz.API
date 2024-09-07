namespace Infrastructure.ExternalServices.ElasticSearch.Setting
{
    public class ElasticSetting
    {
        public string? Url { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? DefaultIndex { get; set; }
    }
}
