using System.Threading.Tasks;
using CoinFill.Emails.EmailTypes;

namespace CoinFill.Emails
{
    public interface IMailService
    {
        Task SendEmailAsync(Email email);
        Task SendEmailConfirmationEmailAsync(ConfirmationEmail email);
        Task SendPasswordResetEmailAsync(PasswordResetEmail email);
        Task SendNewNewsletterSubscriptionEmailAsync(WelcomeEmail email);
        Task SendWelcomeEmailAsync(WelcomeEmail email);
        Task SendPasswordChangedEmailAsync(PasswordChangedEmail email);
        Task SendNetworkAddressGeneratedAsync(NetworkAddressGeneratedEmail email);
        Task SendCardGeneratedAsync(CardGeneratedEmail email);
        Task SendCaptureTransactionSuccessfulAsync(CaptureTransactionEmail email);
        Task SendCaptureStakingTransactionSuccessfulAsync(CaptureTransactionEmail email);
        Task SendBankAccountGeneratedAsync(BankAccountGeneratedEmail email);
        Task SendCardAccountGeneratedAsync(CardAccountGeneratedEmail email);
        Task SendSupportTicketAsync(WelcomeEmail email);
        Task SendCardAcceptedAsync(CardAcceptedEmail email);
        Task SendCardPendingAsync(CardGeneratedEmail email);
        Task SendNow(string toEmail);
    }
}
