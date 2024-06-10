using Cube.Business.Services.Email.dto;
using Cube.DataAccess.Repositories;
using System.Net.Mail;
using Cube.Business.Utilities;
using System.Net;
using Microsoft.AspNetCore.Mvc.Formatters;
using Cube.Domain.Entities;
using System.Linq.Expressions;

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

        public async Task<Response<bool, EmailConfirmationResut>> ConfirmEmail(ConfirmEmailDto dto, CancellationToken token = default)
        {
            var response = new Response<bool, EmailConfirmationResut>();

            Expression<Func<UserEntity, bool>> userPredicate = (user) => user.Email == dto.Email;

            var emailExist = await _repository.UserRepository.GetByPredicateAsync(userPredicate, token);

            if (emailExist != null)
            {
                response.ResponseResult = EmailConfirmationResut.EmailExists;
                response.Value = false;
                return response;
            }

            Expression<Func<EmailTokenEntity, bool>> tokenPredicate = (t) => t.Email == dto.Email && t.Token == dto.Token;

            var emailToken = await _repository.EmailTokenRepository.GetByPredicateAsync(tokenPredicate, token);

            if (emailToken == null) 
            {
                response.ResponseResult = EmailConfirmationResut.ValidationError;
                response.Value = false;
                return response;
            }

            if (emailToken.ExpiredAt < DateTime.UtcNow)
            {
                response.ResponseResult = EmailConfirmationResut.TokenExpired;
                response.Value = false;
                await _repository.EmailTokenRepository.ClearExpiredTokensAsync(token);
                return response;
            }

            await _repository.EmailTokenRepository.ClearExpiredTokensAsync(token);

            response.ResponseResult = EmailConfirmationResut.Success;
            response.Value = true;
            return response;
        }

        public async Task<Response<bool, SendEmailResult>> SendConfirmationCode(EmailDto dto, CancellationToken token = default)
        {
            var response = new Response<bool, SendEmailResult>();

            Expression<Func<UserEntity, bool>> predicate = (user) => user.Email == dto.Email;

            var emailExist = await _repository.UserRepository.GetByPredicateAsync(predicate, token);

            if (emailExist != null) 
            {
                response.ResponseResult = SendEmailResult.EmailExists;
                response.Value = false;
                return response;
            }

            var to = new MailAddress(dto.Email);

            var random = new Random();

            var emailToken = new EmailTokenEntity
            {
                Token = random.NextInt64(100000, 999999).ToString(),
                CreatedAt = DateTime.UtcNow,
                ExpiredAt = DateTime.UtcNow.AddMinutes(5),
                Email = dto.Email
            };

            var createdResult = await _repository.EmailTokenRepository.CreateAsync(emailToken, token);

            if (createdResult == null) 
            {
                response.ResponseResult = SendEmailResult.DataBaseError;
                response.Value = false;
                return response;
            }

            var m = new MailMessage(_from, to)
            {
                Subject = "Тест",
                Body = $"<h2>проверочный код: {createdResult.Token}</h2>",
                IsBodyHtml = true
            };

            try
            {
                _smtp.Send(m);
                response.ResponseResult = SendEmailResult.Success;
                response.Value = true;
            }
            catch (Exception ex) 
            {
                response.ResponseResult = SendEmailResult.EmailSendingError;
                response.Value = false;
                response.Messages.Add(ex.Message);
            }  

            return response;
        }
    }
}
