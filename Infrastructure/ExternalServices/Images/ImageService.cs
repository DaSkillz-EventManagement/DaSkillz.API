using Application.ExternalServices.Images;
using AutoMapper;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Domain.Entities;
using Domain.Repositories;
using Domain.Repositories.UnitOfWork;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.ExternalServices.Images
{
    public class ImageService : IImageService
    {
        private readonly IConfiguration _config;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;
        private readonly ILogoRepository _logoRepository;

        public ImageService(IConfiguration config, IUnitOfWork unitOfWork, IEventRepository eventRepository, IMapper mapper, ILogoRepository logoRepository)
        {
            _config = config;
            _unitOfWork = unitOfWork;
            _eventRepository = eventRepository;
            _mapper = mapper;
            _logoRepository = logoRepository;
        }

        private BlobContainerClient GetBlobContainerClient()
        {
            string? containerName = _config["AzureStorageSettings:ContainerName"];
            string? connectionString = _config["AzureStorageSettings:ConnectionString"];
            BlobContainerClient blobContainerClient = new BlobContainerClient(connectionString, containerName);
            return blobContainerClient;
        }

        public async Task<bool> DeleteBlob(string blobName)
        {
            try
            {
                BlobContainerClient blobContainerClient = GetBlobContainerClient();
                BlobClient blobClient = blobContainerClient.GetBlobClient(blobName);
                await blobClient.DeleteAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<Dictionary<string, List<string>>> GetAllEventBlobUris(Guid eventId)
        {
            Event? eventData = await _eventRepository.GetById(eventId);
            List<string> blobUris = new List<string>();
            List<string> eventTheme = new List<string>();
            if (eventData != null)
            {
                eventTheme.Add(eventData.Image!);
                foreach (var item in eventData.Logos)
                {
                    blobUris.Add(item.LogoUrl!);
                }
            }
            Dictionary<string, List<string>> result = new Dictionary<string, List<string>>
            {
                { "event avatar", eventTheme },
                { "event sponsor logos", blobUris }
            };
            return result;
        }

        public async Task<string?> UploadEventSponsorLogo(string base64, Guid EventId, string sponsorName)
        {
            var logoTemp = await _logoRepository.GetByName(sponsorName);
            if (logoTemp != null)
            {
                return null;
            }
            Event? eventData = await _eventRepository.GetById(EventId);
            if (eventData != null)
            {
                var httpHeaders = new BlobHttpHeaders
                {
                    ContentType = "image/png" // file type
                };
                BlobContainerClient blobContainerClient = GetBlobContainerClient();
                BlobClient blobClient = blobContainerClient.GetBlobClient(EventId.ToString() + "sponsor-" + sponsorName);

                // Decode base64 string to byte array
                byte[] imageBytes = Convert.FromBase64String(base64);

                using (var memoryStream = new MemoryStream(imageBytes))
                {
                    await blobClient.UploadAsync(memoryStream, httpHeaders);
                }

                string absPath = blobClient.Uri.AbsoluteUri;

                Logo newLogo = new Logo
                {
                    SponsorBrand = sponsorName,
                    LogoUrl = absPath
                };
                await _logoRepository.Add(newLogo);
                eventData.Logos.Add(newLogo);
                await _unitOfWork.SaveChangesAsync();
                return absPath;
            }
            return null;
        }

        public async Task<string?> UploadImage(string base64, Guid EventId)
        {
            var httpHeaders = new BlobHttpHeaders
            {
                ContentType = "image/png" // file type
            };
            BlobContainerClient blobContainerClient = GetBlobContainerClient();
            BlobClient blobClient = blobContainerClient.GetBlobClient(EventId.ToString());

            // Decode base64 string to byte array
            byte[] imageBytes = Convert.FromBase64String(base64);

            using (var memoryStream = new MemoryStream(imageBytes))
            {
                await blobClient.UploadAsync(memoryStream, httpHeaders);
            }

            string absPath = blobClient.Uri.AbsoluteUri;
            //eventExist.Image = absPath;
            return absPath;
        }
    }
}
