namespace Tokiota.Store.Domain.Identity.Services
{
    using Microsoft.AspNet.Identity;
    using System.Net.Mail;
    using System.Threading.Tasks;

    internal class EmailService : IEmailService, IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            return Task.FromResult(0);
        }

        public Task SendAsync(MailMessage message)
        {
            return Task.FromResult(0);
        }
    }
}
