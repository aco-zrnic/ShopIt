using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Server.Entities;
using Server.Exceptions;
using Server.Models.Request;
using Server.Models.Response;

namespace Server.Handlers
{
    public class AddScoreHandler : IRequestHandler<AddScore, AddedScore>
    {
        private readonly ILogger<AddScoreHandler> _logger;
        private readonly ShopItContext _context;
        private readonly IMapper _mapper;

        public AddScoreHandler(ILogger<AddScoreHandler> logger, ShopItContext context, IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }

        public async Task<AddedScore> Handle(AddScore request, CancellationToken cancellationToken)
        {
            var book = await _context.Books.FirstOrDefaultAsync(a => a.Id == request.BookId);
            
            ItemNotFoundException.ThrowIfNull<Book>(book);

            var review = _mapper.Map<Review>(request);
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            var response = _mapper.Map<AddedScore>(review);
            return response;
        }
    }
}
