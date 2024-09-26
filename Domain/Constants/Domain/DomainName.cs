namespace Domain.Constants.Domain
{
    public class DomainName
    {
        public const string DOMAIN_NAME = "daskillz-g9h7g9cpcrghamby.southeastasia-01.azurewebsites.net";
        public const string LOCAL = "localhost:60098";
        public const string LOCATION_URL = $"https://{LOCAL}/api/v1/redirect";
        public const string PRODUCTION_URL = $"https://{DOMAIN_NAME}/api/v1/redirect";
    }
}

