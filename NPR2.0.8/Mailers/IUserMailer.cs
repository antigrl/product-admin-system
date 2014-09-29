using Mvc.Mailer;
using NPRModels;
using System.Collections.Generic;

namespace NPR2._0._8.Mailers
{
    public interface IUserMailer
    {
        MvcMailMessage Welcome();
        MvcMailMessage SendStatusUpdate(List<EmailTo> emailTos, string title, string link, Company company = null, Campaign campaign = null, Product product = null);
    }
}