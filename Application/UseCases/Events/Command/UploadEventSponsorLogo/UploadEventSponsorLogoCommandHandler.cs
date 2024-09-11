using Application.ExternalServices.Images;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Domain.Entities;
using Domain.Repositories;
using Domain.Repositories.UnitOfWork;
using MediatR;

namespace Application.UseCases.Events.Command.UploadEventSponsorLogo
{
    public class UploadEventSponsorLogoCommandHandler : IRequestHandler<UploadEventSponsorLogoCommand, string?>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogoRepository _logoRepository;
        private readonly IImageService _imageService;

        public UploadEventSponsorLogoCommandHandler(IEventRepository eventRepository, IUnitOfWork unitOfWork, ILogoRepository logoRepository, IImageService imageService)
        {
            _eventRepository = eventRepository;
            _unitOfWork = unitOfWork;
            _logoRepository = logoRepository;
            _imageService = imageService;
        }

        public async Task<string?> Handle(UploadEventSponsorLogoCommand request, CancellationToken cancellationToken)
        {
            var logoTemp = await _logoRepository.GetByName(request.sponsorName);
            if (logoTemp != null)
            {
                return null;
            }
            Event eventData = await _eventRepository.GetById(request.EventId);
            if (eventData != null)
            {
                var httpHeaders = new BlobHttpHeaders
                {
                    ContentType = "image/png" // file type
                };
                BlobContainerClient blobContainerClient = _imageService.GetBlobContainerClient();
                BlobClient blobClient = blobContainerClient.GetBlobClient(request.EventId.ToString() + "sponsor-" + request.sponsorName);

                // Decode base64 string to byte array
                byte[] imageBytes = Convert.FromBase64String(request.base64);

                using (var memoryStream = new MemoryStream(imageBytes))
                {
                    await blobClient.UploadAsync(memoryStream, httpHeaders);
                }

                string absPath = blobClient.Uri.AbsoluteUri;

                Logo newLogo = new Logo
                {
                    SponsorBrand = request.sponsorName,
                    LogoUrl = absPath
                };
                await _logoRepository.Add(newLogo);
                eventData.Logos.Add(newLogo);
                await _unitOfWork.SaveChangesAsync();
                return absPath;
            }
            return null;
        }
    }
}
