using AutoMapper;
using MediatR;
using Server.Entities;
using Server.Exceptions;
using Server.Models.Request;
using Server.Models.Response;
using Server.Services;
using Server.Util;

namespace Server.Handlers
{
    public class CreateBookBulkHandler : IRequestHandler<CreateBooks, CreatedBooks>
    {
        private readonly IMapper _mapper;
        private readonly ILogger<CreateBookBulkHandler> _logger;
        private readonly IAwsS3Service _awsS3Service;
        private readonly ShopItContext _context;

        public CreateBookBulkHandler(IMapper mapper, ILogger<CreateBookBulkHandler> logger, IAwsS3Service awsS3Service, ShopItContext context)
        {
            _mapper = mapper;
            _logger = logger;
            _awsS3Service = awsS3Service;
            _context = context;
        }

        public async Task<CreatedBooks> Handle(CreateBooks request, CancellationToken cancellationToken)
        {  
            var response = new CreatedBooks() { };
            foreach (var bookOfBulk in request.CreateBook)
            {
                try
                {
                    byte[] decodedBytes = Convert.FromBase64String(bookOfBulk.CoverImage);
                    var fileExt = ImageCheck.IsImage(decodedBytes);

                    if (string.IsNullOrEmpty(fileExt))
                        throw new UserFriendlyException(ErrorCode.BAD_REQUEST, "Cover image is not supported type");

                    await using var memoryStream = new MemoryStream(decodedBytes);

                    var docName = $"{Guid.NewGuid()}{fileExt}";
                    var s3Obj = new UploadS3Object()
                    {
                        InputStream = memoryStream,
                        Name = docName
                    };
                    await _awsS3Service.UploadFileAsync(s3Obj);

                    var book = _mapper.Map<Book>(bookOfBulk);
                    book.CoverImage = s3Obj.Name;
                    _context.Books.Add(book);
                    await _context.SaveChangesAsync();

                    response.AddedBooks.Add(_mapper.Map<SuccesfulAddedBook>(book));
                }catch( Exception exc)
                {
                    _logger.LogInformation($"Failed to add book Title: {bookOfBulk.Title}, ReleaseDate: {bookOfBulk.ReleaseDate}, exception: {exc.Message}");
                    response.FailedToAddBooks.Add(_mapper.Map<SuccesfulAddedBook>(bookOfBulk));
                }
                
            }

            return response;
        }
    }
}
