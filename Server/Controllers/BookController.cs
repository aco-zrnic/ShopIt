using AutoMapper;
using Client.Models.Request;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Server.Behavior;
using Server.Models.Request;

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
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<BookController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost]
        [RequestSizeLimit(5*1024*1024)]
        public async Task<IActionResult> Post([FromBody] CreatBookRequest request)
        {
            var mappedRequest = _mapper.Map<CreateBook>(request);
            var response = await _mediator.Send(mappedRequest);
            return StatusCode(StatusCodes.Status201Created, response);
        }

        // PUT api/<BookController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<BookController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
