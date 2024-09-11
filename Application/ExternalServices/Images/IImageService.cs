namespace Application.ExternalServices.Images
{
    public interface IImageService
    {
        public Task<string?> UploadImage(string base64, Guid EventId);

        public Task<Dictionary<string, List<string>>> GetAllEventBlobUris(Guid eventId);

        public Task<string?> UploadEventSponsorLogo(string base64, Guid EventId, string sponsorName);
        public Task<bool> DeleteBlob(string blobName);
    }
}
