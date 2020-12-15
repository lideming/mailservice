using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MailKit.Net.Smtp;
using System.Text.Json.Serialization;
using MimeKit;

namespace mailservice.Controllers
{
    [ApiController]
    [Route("/api/mail")]
    public class MailController : ControllerBase
    {
        private readonly ILogger<MailController> _logger;
        private readonly MailConfiguration _mailConf;

        public MailController(ILogger<MailController> logger, MailConfiguration mailConf)
        {
            _logger = logger;
            _mailConf = mailConf;
        }

        public class SmtpSendRequest
        {
            [JsonPropertyName("token")]
            public string Token { get; set; }
            [JsonPropertyName("mails")]
            public List<Mail> Mails { get; set; }
        }

        public class Mail
        {
            [JsonPropertyName("fromName")]
            public string FromName { get; set; }
            [JsonPropertyName("to")]
            public string To { get; set; }
            [JsonPropertyName("toName")]
            public string ToName { get; set; }
            [JsonPropertyName("subject")]
            public string Subject { get; set; }
            [JsonPropertyName("body")]
            public string Body { get; set; }
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] SmtpSendRequest arg)
        {
            if (arg == null) return BadRequest();
            if (string.IsNullOrEmpty(arg.Token)) return Forbid();
            var conf = _mailConf.Accounts.FirstOrDefault(x => x.Token == arg.Token);
            if (conf == null) return Forbid();

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(conf.Host, conf.Port, conf.Tls);
                await client.AuthenticateAsync(conf.Username, conf.Password);
                foreach (var item in arg.Mails)
                {
                    var msg = new MimeMessage();
                    msg.From.Add(new MailboxAddress(item.FromName ?? conf.FromName, conf.From));
                    msg.To.Add(new MailboxAddress(item.ToName, item.To));
                    msg.Subject = item.Subject;
                    msg.Body = new TextPart(item.Body);
                    await client.SendAsync(msg);
                }
                await client.DisconnectAsync(true);
            }
            return Ok();
        }
    }
}
