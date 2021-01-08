namespace Kebormed.Core.OrganizationService.Web.Business.Services.Email.Models
{
    public class SendEmailCommand
    {
        public string Content { get; set; }
        public string Subject { get; set; }
        public string RecipientAddress { get; set; }
        public string RecipientFullName { get; set; }
    }
}