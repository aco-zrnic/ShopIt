using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Server.Entities;
using Server.Exceptions;
using Server.Models.Request;
using Server.Models.Response;
using Server.Services;

namespace Server.Handlers
{
    public class GetBookHandler : IRequestHandler<FetchBook, FetchedBook>
    {
        private readonly IAwsS3Service _awsS3Service;
        private readonly ILogger<CreateBookHandler> _logger;
        private readonly IMapper _mapper;
        private readonly ShopItContext _context;
        public GetBookHandler(IAwsS3Service awsS3Service, ILogger<CreateBookHandler> logger, IMapper mapper, ShopItContext context)
        {
            _awsS3Service = awsS3Service;
            _logger = logger;
            _mapper = mapper;
            _context = context;
        }

        public async Task<FetchedBook> Handle(FetchBook request, CancellationToken cancellationToken)
        {
            var book = await _context.Books.FirstOrDefaultAsync(a=>a.Id == request.Id);

            ItemNotFoundException.ThrowIfNull<Book>(book);

            var awsS3Response = await _awsS3Service.GetFileAsync(book.CoverImage);
            var response = _mapper.Map<FetchedBook>(book);
            response.CoverImage = awsS3Response.Base64File;

            return response;
        }
    }
}
