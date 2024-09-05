//using AutoMapper;
//using Microsoft.Extensions.Configuration;

//namespace Event_Management.Application.Service.FileService
//{
//    public class ImageService : IImageService
//    {      
//        private readonly IConfiguration _config;
//        private readonly IUnitOfWork _unitOfWork;
//        private readonly IMapper _mapper;

//        public ImageService(IConfiguration configuration, IUnitOfWork unitOfWork, IMapper mapper)
//        {           
//            _config = configuration;
//            _unitOfWork = unitOfWork;
//            _mapper = mapper;
//        }
//        private BlobContainerClient GetBlobContainerClient()
//        {
//            string? containerName = _config["AzureStorageSettings:ContainerName"];
//            string? connectionString = _config["AzureStorageSettings:ConnectionString"];
//            BlobContainerClient blobContainerClient = new BlobContainerClient(connectionString, containerName);
//            return blobContainerClient;
//        }
//        public async Task<string?> UploadImage(String base64, Guid EventId)
//        {
//            /*var eventExist = await _unitOfWork.EventRepository.GetById(EventId);
//            if (eventExist == null)
//            {
//                throw new Exception(MessageEvent.EventIdNotExist);
//            }*/

//            var httpHeaders = new BlobHttpHeaders
//            {
//                ContentType = "image/png" // file type
//            };
//            BlobContainerClient blobContainerClient = GetBlobContainerClient();
//            BlobClient blobClient = blobContainerClient.GetBlobClient(EventId.ToString());

//            // Decode base64 string to byte array
//            byte[] imageBytes = Convert.FromBase64String(base64);

//            using (var memoryStream = new MemoryStream(imageBytes))
//            {
//                await blobClient.UploadAsync(memoryStream, httpHeaders);
//            }

//            string absPath = blobClient.Uri.AbsoluteUri;
//            //eventExist.Image = absPath;
//            return absPath;
//        }
        
//        public async Task<List<SponsorLogoDto>> GetAllBlobUris()
//        {
//            List<SponsorLogoDto> response = new List<SponsorLogoDto>();
//            var logos = await _unitOfWork.LogoRepository.GetAll();
//            response = _mapper.Map<List<SponsorLogoDto>>(logos);
//            return response;
//        }

//        public async Task<Dictionary<string, List<string>>> GetAllEventBlobUris(Guid eventId)
//        {
//            Event eventData= await _unitOfWork.EventRepository.getAllEventInfo(eventId);
//            List<string> blobUris = new List<string>();
//            List<string> eventTheme = new List<string>();
//            if (eventData != null)
//            {
//                eventTheme.Add(eventData.Image!);
//                foreach(var item in eventData.Logos)
//                {
//                    blobUris.Add(item.LogoUrl!);
//                }
//            }
//            Dictionary<string, List<string>> result = new Dictionary<string, List<string>>
//            {
//                { "event avatar", eventTheme },
//                { "event sponsor logos", blobUris }
//            };
//            return result;
//        }

//        public async Task<SponsorLogoDto> GetBlobUri(string blobName)
//        {
//            /*BlobContainerClient blobContainerClient = GetBlobContainerClient();
//            BlobClient blobClient = blobContainerClient.GetBlobClient(blobName);
//            if (!await blobClient.ExistsAsync())
//            {
//                return null;
//            }

//            string absPath = blobClient.Uri.AbsoluteUri;
//            return absPath;*/
//            var result = await _unitOfWork.LogoRepository.GetByName(blobName);
//            SponsorLogoDto response = _mapper.Map<SponsorLogoDto>(result);
//            return response;
//        }

        

//        // blob name format of the image uploaded by this function: eventId + sponsor name + "logo"
//        public async Task<string?> UploadEventSponsorLogo(string base64, Guid EventId, string sponsorName)
//        {
//            var logoTemp = await _unitOfWork.LogoRepository.GetByName(sponsorName);
//            if (logoTemp != null) { 
//                return null;
//            }
//            Event eventData = await _unitOfWork.EventRepository.getAllEventInfo(EventId);
//            if (eventData != null) { 
//            var httpHeaders = new BlobHttpHeaders
//            {
//                ContentType = "image/png" // file type
//            };
//            BlobContainerClient blobContainerClient = GetBlobContainerClient();
//            BlobClient blobClient = blobContainerClient.GetBlobClient(EventId.ToString() + "sponsor-" + sponsorName);

//            // Decode base64 string to byte array
//            byte[] imageBytes = Convert.FromBase64String(base64);

//            using (var memoryStream = new MemoryStream(imageBytes))
//            {
//                await blobClient.UploadAsync(memoryStream, httpHeaders);
//            }

//            string absPath = blobClient.Uri.AbsoluteUri;

//            Logo newLogo = new Logo
//            {
//                SponsorBrand = sponsorName,
//                LogoUrl = absPath
//            };
//            await _unitOfWork.LogoRepository.Add(newLogo);
//            eventData.Logos.Add(newLogo);
//            await _unitOfWork.SaveChangesAsync();
//            return absPath;
//            }
//            return null;
//        }

//        public async Task<bool> DeleteBlob(string blobName)
//        {
//            try
//            {
//                BlobContainerClient blobContainerClient = GetBlobContainerClient();
//                BlobClient blobClient = blobContainerClient.GetBlobClient(blobName);
//                await blobClient.DeleteAsync();
//                return true;
//            }catch (Exception)
//            {
//                return false;
//            }
//        }
//    }
//}
