using MediatR;
using Server.Models.Response;

namespace Server.Models.Request
{
    public class DeleteBook : IRequest<DeletedBook>
    {
        public int Id { get; set; }
    }
}
