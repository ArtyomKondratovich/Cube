using Cube.Services.Services;
using Cube.Services.Services.Email;
using Cube.Services.Services.Email.dto;
using Microsoft.AspNetCore.Mvc;

namespace Cube.Api.Controllers
{
    [Route("api/[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _service;

        public EmailController(IEmailService service) 
        {
            _service = service;
        }

        [Route("confirm")]
        [HttpPost]
        public async Task<Response<bool, EmailConfirmationResut>> EmailConfirmationl([FromQuery] ConfirmEmailDto dto)
        {
            return await _service.ConfirmEmail(dto);
        }

        [Route("send")]
        [HttpPost]
        public async Task<Response<bool, SendEmailResult>> SendEmailConfirmation([FromBody] EmailDto dto) 
        {
            return await _service.SendConfirmationEmail(dto);
        }
    }
}
