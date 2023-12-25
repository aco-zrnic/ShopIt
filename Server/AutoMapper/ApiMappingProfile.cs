using AutoMapper;
using Client.Models.Request;
using Client.Models.Response;
using Server.Entities;
using Server.Models.Request;
using Server.Models.Response;
using Server.Util.Pagination;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Server.AutoMapper
{
    public class ApiMappingProfile : Profile
    {
        public ApiMappingProfile()
        {
            CreateRequestMappings();
            CreateResponseMappings();
        }
        private void CreateRequestMappings()
        {
            //Book
            CreateMap<CreatBookRequest, CreateBook>();
            CreateMap<CreateBook, Book>();
            CreateMap<GetBooksRequest, FetchBooks>();
            
            //Review
            CreateMap<AddScoreRequest, AddScore>();
            CreateMap<AddScore, Review>();
        }
        private void CreateResponseMappings()
        {
            CreateMap<Book, CreatedBook>();
            CreateMap<Book, FetchedBook>();
            CreateMap<Book, BookScore>();
            CreateMap<BookScore, FetchedBooksResponse>();
            CreateMap<PaginateResponse<BookScore>, PaginateResponse<FetchedBooksResponse>>();
            //Review
            CreateMap<Review,AddedScore>();
        }
    }
}
