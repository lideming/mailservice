using System.Collections.Generic;

namespace mailservice
{
    public class MailConfiguration
    {
        public List<MailAccount> Accounts { get; set; }
    }

    public class MailAccount
    {
        public string Token { get; set; }

        public string From { get; set; }
        public string FromName { get; set; }

        public string Host { get; set; }
        public int Port { get; set; } = 465;
        public bool Tls { get; set; } = true;
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
