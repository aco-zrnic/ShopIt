using AutoMapper;
using Client.Models.Request;
using Server.Entities;
using Server.Models.Request;
using Server.Models.Response;

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
            CreateMap<CreatBookRequest, CreateBook>();
            CreateMap<CreateBook, Book>();
            
        }
        private void CreateResponseMappings()
        {
            CreateMap<Book, CreatedBook>();
            CreateMap<Book, FetchedBook>();
        }
    }
}
