namespace Kebormed.Core.OrganizationService.Web.Configuration
{
    public class EmailSettings
    {
        public string MailServer { get; set; }
        public int MailPort { get; set; }
        public string SenderName { get; set; }
        public string SenderAddress { get; set; }
        public string SenderKey { get; set; }
        public string Password { get; set; }
    }
}