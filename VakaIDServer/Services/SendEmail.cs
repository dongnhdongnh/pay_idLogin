using System;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using Microsoft.Extensions.Configuration;
using VakaxaIDServer.Quickstart.Account;

namespace VakaxaIDServer.Services
{
    public class SendEmail
    {
        private IConfiguration Configuration { get; }

        public SendEmail( IConfiguration configuration)
        {
            Configuration = configuration;
        }       

        public string Send(EmailViewModel model)
        {
            var values = new NameValueCollection
            {
                {"apikey", Configuration.GetSection("Elastic:api").Value},
                {"from", Configuration.GetSection("Elastic:email").Value},
                {"fromName", "VakaxaId"},
                {"to", model.Email},
                {"subject", model.Subject},
                {"bodyText", model.Body},
                {"bodyHtml", model.Body},
                {"isTransactional", "true"}
            };

            const string address = "https://api.elasticemail.com/v2/email/send";
            
            using (var client = new WebClient())
            {
                try
                {
                    var apiResponse = client.UploadValues(address, values);
                    return Encoding.UTF8.GetString(apiResponse);

                }
                catch (Exception ex)
                {
                    return "Exception caught: " + ex.Message + "\n" + ex.StackTrace;
                }
            }
        }
    }
}
