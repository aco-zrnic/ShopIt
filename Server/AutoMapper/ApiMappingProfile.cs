using AutoMapper;
using Client.Models.Request;
using Server.Models.Request;

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
        }
        private void CreateResponseMappings()
        {

        }
    }
}
