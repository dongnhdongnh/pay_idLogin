using System;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MimeKit;
using Newtonsoft.Json.Linq;
using VakaxaIDServer.Commons.Constants;
using VakaxaIDServer.Commons.Helpers;
using VakaxaIDServer.Models.Domains;
using VakaxaIDServer.Models.Entities;
using VakaxaIDServer.Models.Repositories;

namespace SendEmailBusiness
{
    public sealed class SendMailBusiness
    {
        private readonly IVakaxaIdServerIdRepositoryFactory _vakaxaIdRepositoryFactory;

        private readonly IDbConnection _connectionDb;

        public SendMailBusiness(IVakaxaIdServerIdRepositoryFactory vakaxaIdRepositoryFactory,
            bool isNewConnection = true)
        {
            _vakaxaIdRepositoryFactory = vakaxaIdRepositoryFactory;
            _connectionDb = isNewConnection
                ? vakaxaIdRepositoryFactory.GetDbConnection()
                : vakaxaIdRepositoryFactory.GetOldConnection();
        }

        public async Task<ReturnObject> SendEmailAsync(string apiUrl, string apiKey, string from,
            string fromName)
        {
            var sendEmailRepository = _vakaxaIdRepositoryFactory.GetSendEmailRepository(_connectionDb);
            var pendingEmail = sendEmailRepository.FindRowPending();

            if (pendingEmail?.Id == null)
                return new ReturnObject
                {
                    Status = Status.STATUS_SUCCESS,
                    Message = "Pending email not found"
                };

            if (_connectionDb.State != ConnectionState.Open)
                _connectionDb.Open();

            //begin first email
            var transactionScope = _connectionDb.BeginTransaction();
            try
            {
                var lockResult = await sendEmailRepository.LockForProcess(pendingEmail);
                if (lockResult.Status == Status.STATUS_ERROR)
                {
                    transactionScope.Rollback();
                    return new ReturnObject
                    {
                        Status = Status.STATUS_SUCCESS,
                        Message = "Cannot Lock For Process"
                    };
                }

                transactionScope.Commit();
            }
            catch (Exception e)
            {
                transactionScope.Rollback();
                return new ReturnObject
                {
                    Status = Status.STATUS_ERROR,
                    Message = e.ToString()
                };
            }

            //update Version to Model
            pendingEmail.Version += 1;

            var transactionSend = _connectionDb.BeginTransaction();
            try
            {
                var sendResult = SendEmail(pendingEmail, apiUrl, apiKey, from, fromName);


                pendingEmail.Status = sendResult.Status;
                pendingEmail.UpdatedAt = CommonHelper.GetUnixTimestamp();
                pendingEmail.IsProcessing = 0;

                var updateResult = await sendEmailRepository.SafeUpdate(pendingEmail);
                if (updateResult.Status == Status.STATUS_ERROR)
                {
                    transactionSend.Rollback();
                    return new ReturnObject
                    {
                        Status = Status.STATUS_ERROR,
                        Message = "Cannot update email status"
                    };
                }

                transactionSend.Commit();
                return updateResult;
            }
            catch (Exception)
            {
                // release lock
                transactionSend.Rollback();
                var releaseResult = sendEmailRepository.ReleaseLock(pendingEmail);
                Console.WriteLine(JsonHelper.SerializeObject(releaseResult));
                throw;
            }
        }

        private static ReturnObject SendEmail(EmailQueue emailQueue, string apiUrl, string apiKey, string from,
            string fromName)
        {
            var emailBody = CreateEmailBody(emailQueue);
            if (emailBody == null)
                return new ReturnObject
                {
                    Status = Status.STATUS_ERROR,
                    Message = "Cannot find template"
                };
            var values = new NameValueCollection
            {
                {"apikey", apiKey},
                {"from", from},
                {"fromName", fromName},
                {"to", emailQueue.ToEmail},
                {"subject", emailQueue.Subject},
                {"bodyHtml", emailBody},
                {"isTransactional", "true"}
            };

            using (var client = new WebClient())
            {
                try
                {
                    byte[] apiResponse = client.UploadValues(apiUrl, values);

                    var result = JsonHelper.DeserializeObject<JObject>(Encoding.UTF8.GetString(apiResponse));

                    var status = (bool) result["success"] ? Status.STATUS_SUCCESS : Status.STATUS_ERROR;

                    return new ReturnObject
                    {
                        Status = status,
                        Message = Encoding.UTF8.GetString(apiResponse)
                    };
                }
                catch (Exception ex)
                {
                    return new ReturnObject
                    {
                        Status = Status.STATUS_ERROR,
                        Message = ex.Message
                    };
                }
            }
        }

        private static string CreateEmailBody(EmailQueue emailQueue)
        {
            try
            {
                string body;

                var directory = Directory.GetParent(Directory.GetCurrentDirectory()) +
                                "/MailTemplate/BodyEmail.html";


                var builder = new BodyBuilder();
                using (var sourceReader = File.OpenText(directory))
                {
                    builder.HtmlBody = sourceReader.ReadToEnd();
                }

                switch (emailQueue.Template)
                {
                    case 1:
                        body = string.Format(builder.HtmlBody,
                            emailQueue.LinkEmail,
                            "https://i.imgur.com/8idVPQD.png",
                            "https://i.imgur.com/ooQLCzZ.png",
                            "Your account is being locked. Verify email to activate your account."
                        );
                        break;

                    case 2:
                        body = string.Format(builder.HtmlBody,
                            emailQueue.LinkEmail,
                            "https://i.imgur.com/8idVPQD.png",
                            "https://i.imgur.com/ooQLCzZ.png",
                            "In order to start using your VakaxaId account, you need to confirm your email address."
                        );
                        break;
                    default:
                        return null;
                }

                return body;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
    }
}