using MediatR;
using Server.Models.Request;
using Server.Models.Response;

namespace Server.Handlers
{
    public class CreateBookHandler : IRequestHandler<CreateBook, CreatedBook>
    {
        public Task<CreatedBook> Handle(CreateBook request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
