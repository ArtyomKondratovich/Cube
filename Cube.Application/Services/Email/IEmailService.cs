using Cube.Services.Services.Email.dto;
using System.Runtime.CompilerServices;

namespace Cube.Services.Services.Email
{
    public interface IEmailService
    {
        Task<Response<bool, EmailConfirmationResut>> ConfirmEmail(ConfirmEmailDto dto, CancellationToken token = default);
        Task<Response<bool, SendEmailResult>> SendConfirmationEmail(EmailDto dto, CancellationToken token = default);
        

    }
}
