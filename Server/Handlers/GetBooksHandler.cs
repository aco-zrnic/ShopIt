using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Server.Entities;
using Server.Models.Request;
using Server.Models.Response;
using System.Linq;

namespace Server.Handlers
{
    public class GetBooksHandler : IRequestHandler<FetchBooks, FetchedBooks>
    {
        private readonly ShopItContext _context;
        private readonly ILogger<GetBooksHandler> _logger;
        private readonly IMapper _mapper;
        public GetBooksHandler(ShopItContext context, ILogger<GetBooksHandler> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<FetchedBooks> Handle(FetchBooks request, CancellationToken cancellationToken)
        {
            var query = _context.Books.AsQueryable();
            if (!string.IsNullOrEmpty(request.Title))
                query = query.Where(a => a.Title.Contains(request.Title));

            if (request.ReleaseDateBefore is not null)
                query = query.Where(
                    a => (a.ReleaseDate.Year < request.ReleaseDateBefore.Value.Year) ||
                         ((a.ReleaseDate.Year == request.ReleaseDateBefore.Value.Year) && (a.ReleaseDate.Month < request.ReleaseDateBefore.Value.Month))
                         );

            if (request.ReleaseDateAfter is not null)
                query = query.Where(
                    a => (a.ReleaseDate.Year > request.ReleaseDateBefore.Value.Year) ||
                         ((a.ReleaseDate.Year == request.ReleaseDateBefore.Value.Year) && (a.ReleaseDate.Month > request.ReleaseDateBefore.Value.Month))
                         );

            if (request.OnlyComics)
                query = query.Where(a => a.IsComic == true);

            var list = await query.Include(a=>a.Review).ToListAsync();
            var filteredListOfBooks = new List<BookScore>();
            list.ForEach(a =>
            {
                var scores = a.Review.Select(a => a.Score).ToArray();
                var averageScore = scores.Length > 0 ? (double)scores.Sum() / scores.Length : 0;
                var mappedBookScore = _mapper.Map<BookScore>(a);
                mappedBookScore.Score = averageScore;
                filteredListOfBooks.Add(mappedBookScore);
            });

            if (request.MinScore is not null && request.MaxScore is not null)
                filteredListOfBooks = filteredListOfBooks.Where(a => a.Score > request.MinScore && a.Score < request.MaxScore).ToList();
            else if (request.MinScore is not null && request.MaxScore is null)
                filteredListOfBooks = filteredListOfBooks.Where(a => a.Score > request.MinScore).ToList();
            else if (request.MinScore is null && request.MaxScore is not null)
                filteredListOfBooks = filteredListOfBooks.Where(a => a.Score < request.MinScore).ToList();

            return new FetchedBooks { Books = filteredListOfBooks.AsQueryable() };
        }
    }
}
