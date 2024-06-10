using Cube.Business.Services.Email.dto;
using Cube.Business.Utilities;
using Cube.Domain.Entities;
using System.Runtime.CompilerServices;

namespace Cube.Business.Services.Email
{
    public interface IEmailService
    {
        Task<Response<bool, EmailConfirmationResut>> ConfirmEmail(ConfirmEmailDto dto, CancellationToken token = default);
        Task<Response<bool, SendEmailResult>> SendConfirmationCode(EmailDto dto, CancellationToken token = default);
    }
}
