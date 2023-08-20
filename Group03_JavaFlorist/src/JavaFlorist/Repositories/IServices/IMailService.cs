using JavaFlorist.Models.Emails;

namespace JavaFlorist.Repositories.IServices
{
    public interface IMailService
    {
        Task<bool> SendAsync(MailData mailData, CancellationToken ct);
        Task<bool> SendMailAsync(VoucherMail mailData, CancellationToken ct);
        Task<bool> SendWithAttachmentsAsync(MailDataWithAttacments mailData, CancellationToken ct);

    }
}
