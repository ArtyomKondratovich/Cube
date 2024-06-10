using Cube.Business.Services;
using Cube.Business.Services.Email;
using Cube.Business.Services.Email.dto;
using Cube.Business.Utilities;
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
        public async Task<Response<bool, EmailConfirmationResut>> EmailConfirmationl([FromBody] ConfirmEmailDto dto)
        {
            return await _service.ConfirmEmail(dto);
        }

        [Route("send")]
        [HttpPost]
        public async Task<Response<bool, SendEmailResult>> SendConfirmationCode([FromBody] EmailDto dto) 
        {
            return await _service.SendConfirmationCode(dto);
        }
    }
}
