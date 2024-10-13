using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using MimeKit;
using System.IO;
using System.Threading.Tasks;
using CoinFill.Emails.EmailTypes;
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using MailKit;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using CoinFill.Models;
using CoinFill.Interfaces;
using CoinFill.Implementations;
using CoinFill.Helpers.Extensions;
using System.Media;
using System.Globalization;
using CoinFill.Helpers;
using System.Net;

namespace CoinFill.Emails
{
    public class MailService : IMailService
    {
        private const string PROJECT = "CoinFill";
        private const string CLASS = "MailService";

        private readonly IConfiguration _configuration;
        private readonly ISystemErrorLogger _systemError;
        private readonly IWebHostEnvironment _environment;
        private readonly MailSettings _mailSettings;
        private readonly UserManager<CustomClient> _userManager;
        private readonly LinkGenerator _linkGenerator;

        public MailService(IOptions<MailSettings> mailSettings,
            IServiceScopeFactory serviceScopeFactory,
            IWebHostEnvironment environment)
        {
            _systemError = new SystemErrorLogger();
            _mailSettings = mailSettings.Value;
            _environment = environment;

            var scope = serviceScopeFactory.CreateScope();
            _linkGenerator = scope.ServiceProvider.GetRequiredService<LinkGenerator>();
            _configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
            _userManager = scope.ServiceProvider.GetRequiredService<UserManager<CustomClient>>();
        }

        public async Task SendEmailAsync(Email email)
        {
            try
            {
                try
                {
                    SoundPlayer sound = new SoundPlayer(Path.GetFullPath(Path.Combine(_environment.ContentRootPath, @"..\CoinFill\Emails\notify.wav")));
                    sound.Play();
                }
                catch (Exception e2)
                {
                    await _systemError.SaveErrorAsync(e2, PROJECT, CLASS, "SendEmailAsync -> PlaySound");
                }

                //string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                string fileName = "Error in Directory creation";

                try
                {
                    fileName = DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss.fff", CultureInfo.InvariantCulture).Replace(':', '-') + ".txt";
                    string fullPath = Path.GetFullPath(Path.Combine(_environment.ContentRootPath, @"..\CoinFill\Emails\EmailLogs\" + fileName));

                    Helper.EnsureDirectoryExists(fullPath);

                    using (StreamWriter outputFile = new StreamWriter(fullPath))
                    {
                        outputFile.WriteLine($"{email.Subject}: {email.Body}.");
                    }
                }
                catch (Exception e3)
                {
                    await _systemError.SaveErrorAsync(e3, PROJECT, CLASS, "SendEmailAsync -> Directory creation");
                }

                var mime = new MimeMessage
                {
                    Sender = MailboxAddress.Parse(_mailSettings.Mail),
                    Subject = email.Subject
                };

                mime.To.Add(MailboxAddress.Parse(email.ToEmail));

                var builder = new BodyBuilder();
                if (email.Attachments != null)
                {
                    byte[] fileBytes;
                    foreach (var file in email.Attachments)
                    {
                        if (file.Length > 0)
                        {
                            using (var ms = new MemoryStream())
                            {
                                file.CopyTo(ms);
                                fileBytes = ms.ToArray();
                            }
                            builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                        }
                    }
                }

                string host = "smtp.gmail.com";
                int port = 587;
                string mail = "noreply.counterapathy@gmail.com";
                string password = "fhfffubblavttxmx";

                builder.HtmlBody = email.Body + " --- Local file name: " + fileName;
                mime.Body = builder.ToMessageBody();
                using var smtp = new SmtpClient();
                smtp.Connect(host, port, SecureSocketOptions.StartTls);
                smtp.Authenticate(mail, password);
                await smtp.SendAsync(mime);
                smtp.Disconnect(true);

                return;
            }
            catch (Exception e)
            {
                await _systemError.SaveErrorAsync(e, PROJECT, CLASS, "SendEmailAsync");
                return;
            }
        }

        public async Task SendEmailConfirmationEmailAsync(ConfirmationEmail email)
        {
            try
            {
                var mime = new MimeMessage
                {
                    Subject = "Confirm your email",
                    Sender = MailboxAddress.Parse(_mailSettings.Mail)
                };

                mime.To.Add(MailboxAddress.Parse(email.ToEmail));

                var builder = new BodyBuilder();

                var filePath = Path.GetFullPath(Path.Combine(_environment.ContentRootPath, @"..\CoinFill\Emails\EmailTemplates\ConfirmEmail\ConfirmEmail.html"));

                if (new FileInfo(filePath).Exists)
                {
                    builder.HtmlBody = (await new StreamReader(filePath).ReadToEndAsync()).Replace("{{confirmurl}}", _linkGenerator.GetPathByAction("EmailConfirmation", "Authorization", new { uid = email.UserId, token = email.Token }, new PathString($"/{_configuration.GetSection("Application:AppDomain")?.Value}"))[1..]);
                    mime.Body = builder.ToMessageBody();
                    using var smtp1 = new SmtpClient();
                    smtp1.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                    smtp1.Authenticate(_mailSettings.Mail, _mailSettings.Password);
                    await smtp1.SendAsync(mime);
                    smtp1.Disconnect(true);

                    return;
                }

                return;
            }
            catch (Exception e)
            {
                await _systemError.SaveErrorAsync(e, PROJECT, CLASS, "SendEmailConfirmationEmailAsync");
                return;
            }
        }

        public async Task SendPasswordResetEmailAsync(PasswordResetEmail email)
        {
            try
            {
                var mime = new MimeMessage
                {
                    Subject = $"Reset your password",
                    Sender = MailboxAddress.Parse(_mailSettings.Mail)
                };

                mime.To.Add(MailboxAddress.Parse(email.ToEmail));

                var builder = new BodyBuilder();

                var filePath = Path.GetFullPath(Path.Combine(_environment.ContentRootPath, @"..\CoinFill\Emails\EmailTemplates\ResetPassword\ResetPassword.html"));

                if (new FileInfo(filePath).Exists)
                {
                    builder.HtmlBody = (await new StreamReader(filePath).ReadToEndAsync()).Replace("{{reseturl}}", _linkGenerator.GetPathByAction("PasswordReset", "Authorization", new { uid = email.UserId, token = email.Token }, new PathString($"/{_configuration.GetSection("Application:AppDomain")?.Value}"))[1..]);
                    mime.Body = builder.ToMessageBody();
                    using var smtp = new SmtpClient();
                    smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                    smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
                    await smtp.SendAsync(mime);
                    smtp.Disconnect(true);

                    return;
                }

                await _systemError.SaveErrorAsync($"Email file path: {filePath} does not exist.", PROJECT, CLASS, "SendPasswordResetEmailAsync");
                return;
            }
            catch (Exception e)
            {
                await _systemError.SaveErrorAsync(e, PROJECT, CLASS, "SendPasswordResetEmailAsync");
                return;
            }
        }

        public async Task SendNewNewsletterSubscriptionEmailAsync(WelcomeEmail email)
        {
            try
            {
                var mime = new MimeMessage
                {
                    Subject = $"Claim Your Exclusive Welcome Rewards",
                    Sender = MailboxAddress.Parse(_mailSettings.Mail)
                };

                mime.To.Add(MailboxAddress.Parse(email.ToEmail));

                var builder = new BodyBuilder();

                var filePath = Path.GetFullPath(Path.Combine(_environment.ContentRootPath, @"..\CoinFill\Emails\EmailTemplates\NewNewsletterSubscriber\NewNewsletterSubscriber.html"));

                if (new FileInfo(filePath).Exists)
                {
                    var mailText = await new StreamReader(filePath).ReadToEndAsync();

                    mailText = mailText
                        .Replace("{{rewardsurl}}", _linkGenerator.GetPathByAction("ExclusiveRewards", "Home", null, new PathString($"/{_configuration.GetSection("Application:AppDomain")?.Value}"))[1..]);

                    builder.HtmlBody = mailText;
                    mime.Body = builder.ToMessageBody();
                    using var smtp = new SmtpClient();
                    smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                    smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
                    await smtp.SendAsync(mime);
                    smtp.Disconnect(true);

                    return;
                }

                await _systemError.SaveErrorAsync($"Email file path: {filePath} does not exist.", PROJECT, CLASS, "SendNewNewsletterSubscriptionEmailAsync");
                return;
            }
            catch (Exception e)
            {
                await _systemError.SaveErrorAsync(e, PROJECT, CLASS, "SendNewNewsletterSubscriptionEmailAsync");
                return;
            }
        }

        public async Task SendWelcomeEmailAsync(WelcomeEmail email)
        {
            try
            {
                var mime = new MimeMessage
                {
                    Subject = $"Claim Your Exclusive Welcome Rewards",
                    Sender = MailboxAddress.Parse(_mailSettings.Mail)
                };

                mime.To.Add(MailboxAddress.Parse(email.ToEmail));

                var builder = new BodyBuilder();

                var filePath = Path.GetFullPath(Path.Combine(_environment.ContentRootPath, @"..\CoinFill\Emails\EmailTemplates\Welcome\Welcome.html"));

                if (new FileInfo(filePath).Exists)
                {
                    var mailText = await new StreamReader(filePath).ReadToEndAsync();

                    mailText = mailText
                        .Replace("{{rewardsurl}}", _linkGenerator.GetPathByAction("ExclusiveRewards", "Home", null, new PathString($"/{_configuration.GetSection("Application:AppDomain")?.Value}"))[1..]);

                    builder.HtmlBody = mailText;
                    mime.Body = builder.ToMessageBody();
                    using var smtp = new SmtpClient();
                    smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                    smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
                    await smtp.SendAsync(mime);
                    smtp.Disconnect(true);

                    return;
                }

                await _systemError.SaveErrorAsync($"Email file path: {filePath} does not exist.", PROJECT, CLASS, "SendWelcomeEmailAsync");
                return;
            }
            catch (Exception e)
            {
                await _systemError.SaveErrorAsync(e, PROJECT, CLASS, "SendWelcomeEmailAsync");
                return;
            }
        }

        public async Task SendPasswordChangedEmailAsync(PasswordChangedEmail email)
        {
            try
            {
                var mime = new MimeMessage
                {
                    Subject = "Lozinka uspešno promenjena",
                    Sender = MailboxAddress.Parse(_mailSettings.Mail)
                };

                mime.To.Add(MailboxAddress.Parse(email.ToEmail));

                var builder = new BodyBuilder();


                var filePath = Path.GetFullPath(Path.Combine(_environment.ContentRootPath, @"..\CoinFill\Emails\EmailTemplates\PasswordChangedTemplate\password-changed.html"));

                if (new FileInfo(filePath).Exists)
                {
                    var mailText = await new StreamReader(filePath).ReadToEndAsync();

                    //ovde treba da stoje razlicite {{}} stvari, ali trenutno nemam template pa nije ni bitno
                    //mailText = mailText.Replace("{{FirstName}}", email.FirstName);
                    //mailText = Replace() nesto drugo itd...

                    builder.HtmlBody = mailText;
                    mime.Body = builder.ToMessageBody();
                    using var smtp1 = new SmtpClient();
                    smtp1.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                    smtp1.Authenticate(_mailSettings.Mail, _mailSettings.Password);
                    await smtp1.SendAsync(mime);
                    smtp1.Disconnect(true);

                    return;
                }

                return;
            }
            catch (Exception e)
            {
                await _systemError.SaveErrorAsync(e, PROJECT, CLASS, "SendPasswordChangedEmailAsync");
                return;
            }
        }

        public async Task SendNetworkAddressGeneratedAsync(NetworkAddressGeneratedEmail email)
        {
            try
            {
                var mime = new MimeMessage
                {
                    Subject = $"{email.CoinName} ({email.NetworkName}) address generated successfully",
                    Sender = MailboxAddress.Parse(_mailSettings.Mail)
                };

                mime.To.Add(MailboxAddress.Parse(email.ToEmail));

                var builder = new BodyBuilder();

                var filePath = Path.GetFullPath(Path.Combine(_environment.ContentRootPath, @"..\CoinFill\Emails\EmailTemplates\GenerateNetworkAddress\GenerateNetworkAddress.html"));

                if (new FileInfo(filePath).Exists)
                {
                    var mailText = await new StreamReader(filePath).ReadToEndAsync();

                    mailText = mailText
                        .Replace("{{cryptocurrencyname}}", email.CoinName)
                        .Replace("{{networkname}}", email.NetworkName)
                        .Replace("{{address}}", email.Address);

                    builder.HtmlBody = mailText;
                    mime.Body = builder.ToMessageBody();
                    using var smtp = new SmtpClient();
                    smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                    smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
                    await smtp.SendAsync(mime);
                    smtp.Disconnect(true);

                    return;
                }

                await _systemError.SaveErrorAsync($"Email file path: {filePath} does not exist.", PROJECT, CLASS, "SendNetworkAddressGeneratedAsync");
                return;
            }
            catch (Exception e)
            {
                await _systemError.SaveErrorAsync(e, PROJECT, CLASS, "SendNetworkAddressGeneratedAsync");
                return;
            }
        }

        public async Task SendCardGeneratedAsync(CardGeneratedEmail email)
        {
            try
            {
                var mime = new MimeMessage
                {
                    Subject = $"{email.CardType} crypto {email.CardBrand} card generated successfully",
                    Sender = MailboxAddress.Parse(_mailSettings.Mail)
                };

                mime.To.Add(MailboxAddress.Parse(email.ToEmail));

                var builder = new BodyBuilder();

                var filePath = Path.GetFullPath(Path.Combine(_environment.ContentRootPath, @"..\CoinFill\Emails\EmailTemplates\GenerateCard\GenerateCard.html"));

                if (new FileInfo(filePath).Exists)
                {
                    var mailText = await new StreamReader(filePath).ReadToEndAsync();

                    mailText = mailText
                        .Replace("{{type}}", email.CardType)
                        .Replace("{{brand}}", email.CardBrand)
                        .Replace("{{endingdigits}}", "*" + email.CardNumberEndingDigits)
                        .Replace("{{cardurl}}", _linkGenerator.GetPathByAction("Cards", "Dashboard", new { cardId = email.CardId }, new PathString($"/{_configuration.GetSection("Application:AppDomain")?.Value}"))[1..]);

                    builder.HtmlBody = mailText;
                    mime.Body = builder.ToMessageBody();
                    using var smtp = new SmtpClient();
                    smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                    smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
                    await smtp.SendAsync(mime);
                    smtp.Disconnect(true);

                    return;
                }

                await _systemError.SaveErrorAsync($"Email file path: {filePath} does not exist.", PROJECT, CLASS, "SendCardGeneratedAsync");
                return;
            }
            catch (Exception e)
            {
                await _systemError.SaveErrorAsync(e, PROJECT, CLASS, "SendCardGeneratedAsync");
                return;
            }
        }

        public async Task SendCaptureTransactionSuccessfulAsync(CaptureTransactionEmail email)
        {
            try
            {
                var mime = new MimeMessage
                {
                    Subject = email.Subject,
                    Sender = MailboxAddress.Parse(_mailSettings.Mail)
                };

                mime.To.Add(MailboxAddress.Parse(email.ToEmail));

                var builder = new BodyBuilder();

                var filePath = Path.GetFullPath(Path.Combine(_environment.ContentRootPath, @"..\CoinFill\Emails\EmailTemplates\CaptureTransaction\CaptureTransaction.html"));

                if (new FileInfo(filePath).Exists)
                {
                    var mailText = await new StreamReader(filePath).ReadToEndAsync();

                    mailText = mailText
                        .Replace("{{headerholder}}", email.Header)
                        .Replace("{{bodyholder}}", email.Body);

                    builder.HtmlBody = mailText;
                    mime.Body = builder.ToMessageBody();
                    using var smtp = new SmtpClient();
                    smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                    smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
                    await smtp.SendAsync(mime);
                    smtp.Disconnect(true);

                    return;
                }

                await _systemError.SaveErrorAsync($"Email file path: {filePath} does not exist.", PROJECT, CLASS, "SendCaptureTransactionSuccessfulAsync");
                return;
            }
            catch (Exception e)
            {
                await _systemError.SaveErrorAsync(e, PROJECT, CLASS, "SendCaptureTransactionSuccessfulAsync");
                return;
            }
        }

        public async Task SendCaptureStakingTransactionSuccessfulAsync(CaptureTransactionEmail email)
        {
            try
            {
                var mime = new MimeMessage
                {
                    Subject = email.Subject,
                    Sender = MailboxAddress.Parse(_mailSettings.Mail)
                };

                mime.To.Add(MailboxAddress.Parse(email.ToEmail));

                var builder = new BodyBuilder();

                var filePath = Path.GetFullPath(Path.Combine(_environment.ContentRootPath, @"..\CoinFill\Emails\EmailTemplates\CaptureStakingTransaction\CaptureStakingTransaction.html"));

                if (new FileInfo(filePath).Exists)
                {
                    var mailText = await new StreamReader(filePath).ReadToEndAsync();

                    mailText = mailText
                        .Replace("{{headerholder}}", email.Header)
                        .Replace("{{bodyholder}}", email.Body)
                        .Replace("{{stakesurl}}", _linkGenerator.GetPathByAction("MyStakes", "Staking", null, new PathString($"/{_configuration.GetSection("Application:AppDomain")?.Value}"))[1..]);

                    builder.HtmlBody = mailText;
                    mime.Body = builder.ToMessageBody();
                    using var smtp = new SmtpClient();
                    smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                    smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
                    await smtp.SendAsync(mime);
                    smtp.Disconnect(true);

                    return;
                }

                await _systemError.SaveErrorAsync($"Email file path: {filePath} does not exist.", PROJECT, CLASS, "SendCaptureTransactionSuccessfulAsync");
                return;
            }
            catch (Exception e)
            {
                await _systemError.SaveErrorAsync(e, PROJECT, CLASS, "SendCaptureTransactionSuccessfulAsync");
                return;
            }
        }

        public async Task SendBankAccountGeneratedAsync(BankAccountGeneratedEmail email)
        {
            try
            {
                var mime = new MimeMessage
                {
                    Subject = email.Subject,
                    Sender = MailboxAddress.Parse(_mailSettings.Mail)
                };

                mime.To.Add(MailboxAddress.Parse(email.ToEmail));

                var builder = new BodyBuilder();

                var filePath = Path.GetFullPath(Path.Combine(_environment.ContentRootPath, @"..\CoinFill\Emails\EmailTemplates\GenerateBankAccount\GenerateBankAccount.html"));

                if (new FileInfo(filePath).Exists)
                {
                    var mailText = await new StreamReader(filePath).ReadToEndAsync();

                    mailText = mailText
                        .Replace("{{bankcurrency}}", email.BankAccountCurrency)
                        .Replace("{{endingdigits}}", "*" + email.BankAccountEndingDigits)
                        .Replace("{{depositfee}}", email.DepositFee + "%")
                        .Replace("{{bankurl}}", _linkGenerator.GetPathByAction("BankAccounts", "Banks", new { bankId = email.BankId }, new PathString($"/{_configuration.GetSection("Application:AppDomain")?.Value}"))[1..]);

                    builder.HtmlBody = mailText;
                    mime.Body = builder.ToMessageBody();
                    using var smtp = new SmtpClient();
                    smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                    smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
                    await smtp.SendAsync(mime);
                    smtp.Disconnect(true);

                    return;
                }

                await _systemError.SaveErrorAsync($"Email file path: {filePath} does not exist.", PROJECT, CLASS, "SendBankAccountGeneratedAsync");
                return;
            }
            catch (Exception e)
            {
                await _systemError.SaveErrorAsync(e, PROJECT, CLASS, "SendBankAccountGeneratedAsync");
                return;
            }
        }

        public async Task SendCardAccountGeneratedAsync(CardAccountGeneratedEmail email)
        {
            try
            {
                var mime = new MimeMessage
                {
                    Subject = email.Subject,
                    Sender = MailboxAddress.Parse(_mailSettings.Mail)
                };

                mime.To.Add(MailboxAddress.Parse(email.ToEmail));

                var builder = new BodyBuilder();

                var filePath = Path.GetFullPath(Path.Combine(_environment.ContentRootPath, @"..\CoinFill\Emails\EmailTemplates\GenerateBankAccount\GenerateCardAccount.html"));

                if (new FileInfo(filePath).Exists)
                {
                    var mailText = await new StreamReader(filePath).ReadToEndAsync();

                    mailText = mailText
                        .Replace("{{cardcurrency}}", email.CardAccountCurrency)
                        .Replace("{{endingdigits}}", "*" + email.CardAccountEndingDigits)
                        .Replace("{{depositfee}}", email.DepositFee + "%")
                        .Replace("{{cardurl}}", _linkGenerator.GetPathByAction("BankAccounts", "Banks", new { bankId = email.CardId }, new PathString($"/{_configuration.GetSection("Application:AppDomain")?.Value}"))[1..]);

                    builder.HtmlBody = mailText;
                    mime.Body = builder.ToMessageBody();
                    using var smtp = new SmtpClient();
                    smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                    smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
                    await smtp.SendAsync(mime);
                    smtp.Disconnect(true);

                    return;
                }

                await _systemError.SaveErrorAsync($"Email file path: {filePath} does not exist.", PROJECT, CLASS, "SendCardAccountGeneratedAsync");
                return;
            }
            catch (Exception e)
            {
                await _systemError.SaveErrorAsync(e, PROJECT, CLASS, "SendCardAccountGeneratedAsync");
                return;
            }
        }

        public async Task SendSupportTicketAsync(WelcomeEmail email)
        {
            try
            {
                var mime = new MimeMessage
                {
                    Subject = "Support ticket submitted successfully",
                    Sender = MailboxAddress.Parse(_mailSettings.Mail)
                };

                mime.To.Add(MailboxAddress.Parse(email.ToEmail));

                var builder = new BodyBuilder();

                var filePath = Path.GetFullPath(Path.Combine(_environment.ContentRootPath, @"..\CoinFill\Emails\EmailTemplates\SupportTicket\SupportTicket.html"));

                if (new FileInfo(filePath).Exists)
                {
                    var mailText = await new StreamReader(filePath).ReadToEndAsync();

                    mailText = mailText
                        .Replace("{{dashboardurl}}", _linkGenerator.GetPathByAction("Index", "Dashboard", null, new PathString($"/{_configuration.GetSection("Application:AppDomain")?.Value}"))[1..]);

                    builder.HtmlBody = mailText;
                    mime.Body = builder.ToMessageBody();
                    using var smtp = new SmtpClient();
                    smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                    smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
                    await smtp.SendAsync(mime);
                    smtp.Disconnect(true);

                    return;
                }

                await _systemError.SaveErrorAsync($"Email file path: {filePath} does not exist.", PROJECT, CLASS, "SendSupportTicketAsync");
                return;
            }
            catch (Exception e)
            {
                await _systemError.SaveErrorAsync(e, PROJECT, CLASS, "SendSupportTicketAsync");
                return;
            }
        }

        public async Task SendCardAcceptedAsync(CardAcceptedEmail email)
        {
            try
            {
                var mime = new MimeMessage
                {
                    Subject = $"{email.CardType} crypto {email.CardBrand} card generated successfully",
                    Sender = MailboxAddress.Parse(_mailSettings.Mail)
                };

                mime.To.Add(MailboxAddress.Parse(email.ToEmail));

                var builder = new BodyBuilder();

                var filePath = Path.GetFullPath(Path.Combine(_environment.ContentRootPath, @"..\CoinFill\Emails\EmailTemplates\CardAccepted\CardAccepted.html"));

                if (new FileInfo(filePath).Exists)
                {
                    var mailText = await new StreamReader(filePath).ReadToEndAsync();

                    mailText = mailText
                        .Replace("{{type}}", email.CardType)
                        .Replace("{{brand}}", email.CardBrand)
                        .Replace("{{endingdigits}}", "*" + email.CardNumberEndingDigits)
                        .Replace("{{coinname}}", email.CoinNameUsedForPayment)
                        .Replace("{{cardurl}}", _linkGenerator.GetPathByAction("Cards", "Dashboard", new { cardId = email.CardId }, new PathString($"/{_configuration.GetSection("Application:AppDomain")?.Value}"))[1..]);

                    builder.HtmlBody = mailText;
                    mime.Body = builder.ToMessageBody();
                    using var smtp = new SmtpClient();
                    smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                    smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
                    await smtp.SendAsync(mime);
                    smtp.Disconnect(true);

                    return;
                }

                await _systemError.SaveErrorAsync($"Email file path: {filePath} does not exist.", PROJECT, CLASS, "SendCardAcceptedAsync");
                return;
            }
            catch (Exception e)
            {
                await _systemError.SaveErrorAsync(e, PROJECT, CLASS, "SendCardAcceptedAsync");
                return;
            }
        }

        public async Task SendCardPendingAsync(CardGeneratedEmail email)
        {
            throw new NotImplementedException();
        }

        public async Task SendNow(string toEmail)
        {
            try
            {
                var mime = new MimeMessage
                {
                    Subject = $"Crypto Deposits With 20% Bonus & Earn up to 82% Staking APY",
                    Sender = MailboxAddress.Parse(_mailSettings.Mail)
                };

                mime.To.Add(MailboxAddress.Parse(toEmail));

                var builder = new BodyBuilder();

                builder.HtmlBody = (await new StreamReader(Path.GetFullPath(Path.Combine(_environment.ContentRootPath, @"..\CoinFill\Emails\EmailTemplates\SendNow\NewSubscriber.html"))).ReadToEndAsync()).Replace("{{usource}}", WebUtility.UrlEncode(toEmail));
                mime.Body = builder.ToMessageBody();
                using var smtp = new SmtpClient();
                smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
                await smtp.SendAsync(mime);
                smtp.Disconnect(true);

                return;
            }
            catch (Exception e)
            {
                await _systemError.SaveErrorAsync(e, PROJECT, CLASS, "SendNow");
                return;
            }
        }
    }
}