namespace LMS.Application.Common
{
    public class EmailSettings
    {
        public string DisplayName { get; set; }
        public string From { get; set; }
        public string AppPassword { get; set; }
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
    }
}
