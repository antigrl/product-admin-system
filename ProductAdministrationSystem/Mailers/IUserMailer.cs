using Mvc.Mailer;
using PAS.Models;
using System.Collections.Generic;

namespace PAS.Mailers
{
    public interface IUserMailer
    {
        MvcMailMessage Welcome();
        MvcMailMessage SendStatusUpdate(List<EmailTo> emailTos, string title, string link, Company company = null, Campaign campaign = null, Product product = null);
    }
}