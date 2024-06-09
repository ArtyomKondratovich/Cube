namespace Cube.Business.Services.Email
{
    public class SmtpSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public bool EnableSsl { get; set; }
        public SmtpCredentials Credentials { get; set; }
    }

    public class SmtpCredentials
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
