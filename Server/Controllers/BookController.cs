using AutoMapper;
using Client.Models.Request;
using Client.Models.Response;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Behavior;
using Server.Models.Request;
using Server.Util.Auth0;
using Server.Util.Pagination;
using System.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(LoggingActionFilterBehavior))]
    public class BookController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public BookController(IMapper mapper, IMediator mediator)
        {
            _mapper = mapper;
            _mediator = mediator;
        }


        [HttpGet]
        [Authorize(Policy = PermissionResource.View)]
        public async Task<IActionResult> GetBooks([FromQuery] GetBooksRequest request, [FromQuery] PaginateRequest paginateRequest)
        {
            //i would also use Redis here, for storing in memory array of books
            var mappedRequest = _mapper.Map<FetchBooks>(request);
            var response = await _mediator.Send(mappedRequest);

            if(paginateRequest.PaginationRequired)
                return Ok(_mapper.Map<PaginateResponse<FetchedBooksResponse>>(PaginateService.ToPaginateList(response.Books, paginateRequest)));
            else
                return Ok(_mapper.Map<FetchedBooksResponse[]>(response.Books.ToArray()));
        }

        [HttpGet("{id}")]
        [Authorize(Policy = PermissionResource.View)]
        public async Task<IActionResult> Get(int id)
        {
            var response = await _mediator.Send(new FetchBook { Id = id });
            return Ok(response);
        }

        [HttpPost]
        [RequestSizeLimit(5*1024*1024)]
        [Authorize(Policy = PermissionResource.Create)]
        public async Task<IActionResult> Post([FromBody] CreatBookRequest request)
        {
            var mappedRequest = _mapper.Map<CreateBook>(request);
            var response = await _mediator.Send(mappedRequest);
            return StatusCode(StatusCodes.Status201Created, response);
        }
        [HttpPost("books-bulk")]
        [RequestSizeLimit(10 * 1024 * 1024)]
        [Authorize(Policy = PermissionResource.Create)]
        public async Task<IActionResult> PostBooks([FromBody] CreatBookRequest[] request)
        {
            var mappedRequest = _mapper.Map<CreateBook[]>(request);
            var response = await _mediator.Send(new CreateBooks { CreateBook = mappedRequest });
            return StatusCode(StatusCodes.Status201Created, response);   
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = PermissionResource.Delete)]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _mediator.Send(new DeleteBook {  Id = id });
            return Ok();
        }
    }
}
