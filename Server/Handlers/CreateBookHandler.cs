using Amazon.S3.Model;
using AutoMapper;
using MediatR;
using Server.Models.Request;
using Server.Models.Response;
using Server.Services;

namespace Server.Handlers
{
    public class CreateBookHandler : IRequestHandler<CreateBook, CreatedBook>
    {
        private readonly IAwsS3Service _awsS3Service;
        private readonly ILogger<CreateBookHandler> _logger;
        private readonly IMapper _mapper;

        public CreateBookHandler(IAwsS3Service awsS3Service, ILogger<CreateBookHandler> logger, IMapper mapper)
        {
            _awsS3Service = awsS3Service;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<CreatedBook> Handle(CreateBook request, CancellationToken cancellationToken)
        {
            byte[] decodedBytes = Convert.FromBase64String(request.CoverImage);
            var fileExt = IsImage(decodedBytes);

            if (String.IsNullOrEmpty(fileExt))
                return new CreatedBook(); //thow Error if its not image!

            await using var memoryStream = new MemoryStream(decodedBytes);
            
            var docName = $"{Guid.NewGuid()}{fileExt}";
            var s3Obj = new UploadS3Object()
            {
                InputStream = memoryStream,
                Name = docName
            };
            await _awsS3Service.UploadFileAsync(s3Obj);
            return new CreatedBook();

        }
        private static string IsImage(byte[] data)
        {
            // Check for a valid image file header (e.g., PNG, JPEG, GIF)
            // For simplicity, this example checks for a JPEG header
            if (data.Length >= 2 && data[0] == 0xFF && data[1] == 0xD8)
                return ".jpeg";
            
            // Compare the first 8 bytes of the data with the PNG signature
            byte[] pngSignature = { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };
            var isPng = true;

            for (int i = 0; i < pngSignature.Length; i++)
            {
                if (data[i] != pngSignature[i])
                    isPng = false;
            }

            if (isPng)
                return ".png";

            return string.Empty;
        }
    }
}
