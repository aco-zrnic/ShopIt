﻿using Amazon.Auth.AccessControlPolicy;
using AutoMapper;
using Client.Models.Request;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Behavior;
using Server.Models.Request;
using Server.Util.Auth0;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(LoggingActionFilterBehavior))]
    public class ReviewsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public ReviewsController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        

        [HttpPost]
        [Authorize(Policy = PermissionResource.Create)]
        public async Task<IActionResult> Post([FromBody] AddScoreRequest request)
        {
            var response = await _mediator.Send(_mapper.Map<AddScore>(request));
            return StatusCode(StatusCodes.Status201Created, response);
        }

        // PUT api/<ReviewsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
