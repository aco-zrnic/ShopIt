using AutoMapper;
using Client.Models.Request;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Behavior;
using Server.Models.Request;
using Server.Util.Keycloak;
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

        //[Authorize(Roles = KeycloakRoles.All)]
        [HttpGet]
        public async Task<IActionResult> GetALL()
        {
            var response = await _mediator.Send(new FetchBook { Id = 2 });
            return Ok(response);
        }
        // GET api/<BookController>/5
        //[Authorize(Roles = KeycloakRoles.All)]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var response = await _mediator.Send(new FetchBook { Id = id });
            return Ok(response);
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
