using Cube.Business.Services.Email.dto;
using Cube.DataAccess.Repositories;
using System.Net.Mail;
using Cube.Business.Utilities;
using System.Net;

namespace Cube.Business.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly IRepositoryWrapper _repository;
        private readonly SmtpClient _smtp;
        private readonly MailAddress _from;

        public EmailService(IRepositoryWrapper repository,
            SmtpSettings smtpSettings) 
        {
            _repository = repository;
            _smtp = new SmtpClient
            {
                Host = smtpSettings.Host,
                Port = smtpSettings.Port,
                EnableSsl = smtpSettings.EnableSsl,
                Credentials = new NetworkCredential(smtpSettings.Credentials.UserName, smtpSettings.Credentials.Password)
            };
            _from = new MailAddress(smtpSettings.Credentials.UserName, "Support");
        }

        public Task<Response<bool, EmailConfirmationResut>> ConfirmEmail(ConfirmEmailDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<bool, SendEmailResult>> SendConfirmationEmail(EmailDto dto, CancellationToken token = default)
        {
            var to = new MailAddress(dto.Email);

            var m = new MailMessage(_from, to)
            {
                Subject = "Тест",
                Body = "<h2>Письмо-тест работы smtp-клиента</h2>",
                IsBodyHtml = true
            };

            _smtp.Send(m);

            return new Response<bool, SendEmailResult>();
        }
    }
}
