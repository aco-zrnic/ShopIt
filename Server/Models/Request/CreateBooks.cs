using MediatR;
using Server.Models.Response;

namespace Server.Models.Request
{

    public class CreateBooks : IRequest<CreatedBooks>
    {
        public CreateBook[] CreateBook { get; set; }
    }
}
