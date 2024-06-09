using Cube.Business.Services.Email.dto;
using Cube.Business.Utilities;
using System.Runtime.CompilerServices;

namespace Cube.Business.Services.Email
{
    public interface IEmailService
    {
        Task<Response<bool, EmailConfirmationResut>> ConfirmEmail(ConfirmEmailDto dto, CancellationToken token = default);
        Task<Response<bool, SendEmailResult>> SendConfirmationEmail(EmailDto dto, CancellationToken token = default);
        

    }
}
