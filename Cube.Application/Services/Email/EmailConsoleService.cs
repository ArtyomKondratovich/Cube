using Cube.Application.Services.Email.dto;
using Cube.EntityFramework.Repository;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace Cube.Application.Services.Email
{
    public class EmailConsoleService : IEmailService
    {
        private readonly IRepositoryWrapper _repository;
        private readonly ConfigurationManager _configuration;

        public EmailConsoleService(IRepositoryWrapper repository, ConfigurationManager configuration) 
        {
            _repository = repository;
            _configuration = configuration;
        }

        public Task<Response<bool, EmailConfirmationResut>> ConfirmEmail(ConfirmEmailDto dto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<bool, SendEmailResult>> SendConfirmationEmail(EmailDto dto, CancellationToken token = default)
        {
            var response = new Response<bool, SendEmailResult>();

            var user = await _repository.UserRepository.GetByIdAsync(dto.UserId);

            if (user == null) 
            {
                response.ResponseResult = SendEmailResult.UserNotFound;
                return response;
            }

            if (!dto.Email.IsValidEmail())
            {
                response.ResponseResult = SendEmailResult.ValidationError;
                return response;
            }

            var smtpClient = _configuration.GetSection("SmtpConfiguration").Get<SmtpClient>();
            var credentialsSection = _configuration.GetSection("EmailCredentials");
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential
                (
                credentialsSection.GetSection("Username").Value,
                credentialsSection.GetSection("Password").Value
                );

            var from = new MailAddress("kondratovich443gmail.com");
            var to = new MailAddress("kondratovich443@gmail.com");
            var m = new MailMessage(from, to)
            {
                Subject = "Тест",
                // текст письма
                Body = "<h2>Письмо-тест работы smtp-клиента</h2>",
                // письмо представляет код html
                IsBodyHtml = true
            };

            smtpClient.Send(m);
            Console.WriteLine($"Send email from {smtpClient.Host} to {dto.Email} with message = [ hi! ]");
            response.ResponseResult = SendEmailResult.Success;

            return response;

        }
    }
}
