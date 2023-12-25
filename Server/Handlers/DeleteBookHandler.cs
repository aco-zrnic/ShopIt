using MediatR;
using Microsoft.EntityFrameworkCore;
using Server.Entities;
using Server.Exceptions;
using Server.Models.Request;
using Server.Models.Response;
using Server.Services;

namespace Server.Handlers
{
    public class DeleteBookHandler : IRequestHandler<DeleteBook, DeletedBook>
    {
        private readonly ILogger<DeleteBookHandler> _logger;
        private readonly ShopItContext _context;
        private readonly IAwsS3Service _awsS3Service;

        public DeleteBookHandler(ILogger<DeleteBookHandler> logger, ShopItContext context, IAwsS3Service awsS3Service)
        {
            _logger = logger;
            _context = context;
            _awsS3Service = awsS3Service;
        }

        public async Task<DeletedBook> Handle(DeleteBook request, CancellationToken cancellationToken)
        {
            var book = await _context.Books.FirstOrDefaultAsync(a => a.Id == request.Id);

            ItemNotFoundException.ThrowIfNull<Book>(book);

            await _awsS3Service.DeleteFileAsync(book.CoverImage);

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return new DeletedBook();
        }
    }
}
