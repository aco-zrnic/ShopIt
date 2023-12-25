using Amazon.S3.Model;
using AutoMapper;
using MediatR;
using Server.Entities;
using Server.Exceptions;
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
        private readonly ShopItContext _context;

        public CreateBookHandler(IAwsS3Service awsS3Service, ILogger<CreateBookHandler> logger, IMapper mapper, ShopItContext context)
        {
            _awsS3Service = awsS3Service;
            _logger = logger;
            _mapper = mapper;
            _context = context;
        }

        public async Task<CreatedBook> Handle(CreateBook request, CancellationToken cancellationToken)
        {
            byte[] decodedBytes = Convert.FromBase64String(request.CoverImage);
            var fileExt = IsImage(decodedBytes);

            if (string.IsNullOrEmpty(fileExt))
                throw new UserFriendlyException(ErrorCode.BAD_REQUEST,"Cover image is not supported type");     

            await using var memoryStream = new MemoryStream(decodedBytes);
            
            var docName = $"{Guid.NewGuid()}{fileExt}";
            var s3Obj = new UploadS3Object()
            {
                InputStream = memoryStream,
                Name = docName
            };
            await _awsS3Service.UploadFileAsync(s3Obj);

            var book = _mapper.Map<Book>(request);
            book.CoverImage = s3Obj.Name;
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            var response = _mapper.Map<CreatedBook>(book);

            return response;

        }
        private static string IsImage(byte[] data)
        {
            // Check for a valid image file header (e.g., PNG, JPEG)
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
