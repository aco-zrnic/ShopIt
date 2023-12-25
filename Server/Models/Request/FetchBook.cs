using MediatR;
using Server.Models.Response;

namespace Server.Models.Request
{
    public class FetchBook :IRequest<FetchedBook>
    {
        public int Id { get; set; }
    }
}
