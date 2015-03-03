using Mvc.Mailer;
using PAS.Models;
using System;
using System.Collections.Generic;

namespace PAS.Mailers
{
    public class UserMailer : MailerBase, IUserMailer
    {
        public UserMailer()
        {
            MasterName = "_Layout";
        }

        public virtual MvcMailMessage Welcome()
        {
            ViewBag.Title = "This is the Title for the Welcome test";
            ViewBag.Body = "This is the body.. it has data";
            return Populate(x =>
            {
                x.Subject = "Welcome";
                x.ViewName = "Welcome";
                x.To.Add("john.grabanski@gatewaycdi.com");
            });
        }

        public virtual MvcMailMessage SendStatusUpdate(List<EmailTo> emailTos, string title, string link, Company company = null, Campaign campaign = null, Product product = null)
        {
            string subject = "";// Check if we have an object
            if(product != null)
            {
                subject = company.CompanyName + ": Product Status Update " + DateTime.Now.ToString("f") + " : " + product.ProductName;
                ViewBag.Type = "Product";
                ViewBag.Name = product.ProductName;
                ViewBag.Status = product.ProductStatus;
                ViewBag.Link = link;

                ViewBag.CampaignData = "Campaign Name: " + campaign.CampaignName;
                ViewBag.CompanyData = "Company Name: " + company.CompanyName;
            }
            else if(campaign != null)
            {
                subject = company.CompanyName + ": Campaign Status Update - " + campaign.CampaignName;
                ViewBag.Type = "Campaign";
                ViewBag.Name = campaign.CampaignName;
                ViewBag.Status = campaign.CampaignStatus;
                ViewBag.Link = link;

                ViewBag.CompanyData = "Company Name: " + company.CompanyName;
            }
            else if(company != null)
            {
                subject = company.CompanyName + ": Company Status Update";
                ViewBag.Type = "Company";
                ViewBag.Name = company.CompanyName;
                ViewBag.Status = company.CompanyStatus;
                ViewBag.Link = link;
            }
            else
            {
                throw new System.ArgumentException("All Objects are Null", "UserMailer.cs");
            }

            return Populate(x =>
            {
                x.Subject = subject;
                x.ViewName = "SendStatusUpdate";
                foreach(EmailTo emailTo in emailTos)
                {
                    switch(emailTo)
                    {
                        case EmailTo.Account_Manager:
                            x.To.Add(company.CompanyAccountManagerEmail);
                            break;
                        case EmailTo.Inventory_Buyers:
                            x.To.Add("NPRInventoryBuyers@gatewaycdi.com");
                            break;
                        case EmailTo.Merchandisers:
                            x.To.Add("NPRMerchandisers@gatewaycdi.com");
                            break;
                        case EmailTo.Mentor:
                            x.To.Add(company.CompanyMentorEmail);
                            break;
                        default:
                            break;
                    }
                }
            });
        }
    }
}