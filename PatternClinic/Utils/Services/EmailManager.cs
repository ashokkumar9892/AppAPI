

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;


namespace PatternClinic.Utils.Services
{
    public class EmailManager
    {
        public static async Task<string> SendEmailSendGridAsync(string UserName, string Subject, string Body, string Email)
        {
            return "";
        }
        public  static async Task<string> MailSendToClient(string UserName, string Subject, string Body, string Email)
        {
            return "";
        }
    }
}
