using ChatAssistant.Application.Commands.CreateChatSession;
using ChatAssistant.Application.Commands.DisconnectedSession;
using ChatAssistant.Application.Commands.PollChatSession;
using ChatAssistant.Infrastructure.Data.Dtos.RequestDtos.ChatSessions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ChatAssistant.Support.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Produces("application/json")]
    public class ChatController  : ControllerBase
    {
        private readonly IMediator _mediator;

        public ChatController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost("CreateSession")]
        public async Task<IActionResult> CreateSession([FromBody] CreateSessionRequestModel command)
        {
            if (command == null)
            {
                return BadRequest($"{nameof(CreateSessionRequestModel)} cannot be null.");
            }
            try
            {
                var result = await _mediator.Send(new CreateChatSessionCommand { RequestorConnectionId = command.RequestorConnectionId });

                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                else
                {
                    var errorResponse = string.IsNullOrWhiteSpace(result.AggregatedUFExceptions) ? "An error occurred during the creation of the chat session." : result.AggregatedUFExceptions;
                    return StatusCode((int)result.StatusCode, errorResponse);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [HttpPost("DisconnectedSession")]
        public async Task<IActionResult> DisconnectedSession([FromBody] DisconnectedSessionRequestModel command)
        {
            if (command == null)
            {
                return BadRequest($"{nameof(DisconnectedSessionRequestModel)} cannot be null.");
            }

            var result = await _mediator.Send(new DisconnectedSessionCommand { ConnectionId = command.ConnectionId });

            if (result.IsSuccess)
            {
                return Ok(result);
            }
            else
            {
                var errorResponse = string.IsNullOrWhiteSpace(result.AggregatedUFExceptions) ? "An error occurred during the disconnect of the chat session." : result.AggregatedUFExceptions;
                return StatusCode((int)result.StatusCode, errorResponse);
            }
        }

        [HttpPost("CreatePollChat")]
        public async Task<IActionResult> CreatePollChat([FromBody] PollChatSessionRequestModel command)
        {
            if (command == null)
            {
                return BadRequest($"{nameof(PollChatSessionRequestModel)} cannot be null.");
            }

            var result = await _mediator.Send(new CreatePollChatSessionCommand { ConnectionId = command.ConnectionId });

            if (result.IsSuccess)
            {
                return Ok(result);
            }
            else
            {
                var errorResponse = string.IsNullOrWhiteSpace(result.AggregatedUFExceptions) ? "An error occurred during the poll of the chat session." : result.AggregatedUFExceptions;
                return StatusCode((int)result.StatusCode, errorResponse);
            }
        }

        [HttpPost("ChatPing")]
        public async Task<IActionResult> ChatPing()
        {
            return Ok();
        }



    }
}
