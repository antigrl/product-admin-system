using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PAS.Models;
using System.Reflection;
using System.ComponentModel;
using System.Data;
using System.Globalization;

namespace PAS.Helpers
{
    public static class MyExtensions
    {
        // Generates a List for creating a Dropdown for Enums
        public static List<SelectListItem> GetGenericEnumDropDown<T>() where T : struct
        {
            List<SelectListItem> dropdownList = new List<SelectListItem>();
            Array itemValues = Enum.GetValues(typeof(T));
            foreach (Enum value in itemValues)
            {
                SelectListItem item = new SelectListItem();
                item.Text = GetEnumDescription(value);
                item.Value = GetEnumDescription(value);
                dropdownList.Add(item);
            }
            return dropdownList;
        }

        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
            {
                return attributes[0].Description;
            }
            else
            {
                return value.ToString();
            }
        }

        public static List<EmailTo> GetEmailTo(string objectStatus)
        {
            List<EmailTo> emailToList = new List<EmailTo>();

            if (objectStatus == MyExtensions.GetEnumDescription(Status.Send_To_Account_Manager))
            {
                emailToList.Add(EmailTo.Account_Manager);
            }
            else if (objectStatus == MyExtensions.GetEnumDescription(Status.Send_To_Mentor))
            {
                emailToList.Add(EmailTo.Mentor);
            }
            else if (objectStatus == MyExtensions.GetEnumDescription(Status.Send_To_Inventory_Buyers) ||
                        objectStatus == MyExtensions.GetEnumDescription(Status.Mentor_Approved) ||
                        objectStatus == MyExtensions.GetEnumDescription(Status.Rejected))
            {
                emailToList.Add(EmailTo.Inventory_Buyers);
            }
            else if (objectStatus == MyExtensions.GetEnumDescription(Status.Send_To_Merchandisers))
            {
                emailToList.Add(EmailTo.Merchandisers);
            }

            return emailToList;
        }

        public static void UpdateActiveMarginsBasedOnCompany(Company company, decimal defaultMargin, ref  NPREntities db)
        {
            string archived = MyExtensions.GetEnumDescription(Status.Archived);

            // loop though active campaigns
            foreach (Campaign campaign in company.Campaigns.Where(c => c.CampaignStatus != archived))
            {
                foreach (Product product in campaign.Products.Where(c => c.ProductStatus != archived))
                {
                    foreach (ProductSellPrice sellPrice in product.ProductSellPrices)
                    {
                        sellPrice.SellPriceMarginPercent = defaultMargin;
                    }

                    FeeCalculator newCalculator = new FeeCalculator(product);

                    try
                    {
                        newCalculator.ComputeAllProductPrices(true);
                        newCalculator.ComputeMarginBasedOnSellprice();
                    }
                    catch (Exception ex)
                    {
                        Console.Write("FeeCalculator.cs: UpdateActiveMarginsBasedOnCompany()" + ex.ToString());
                    }
                    // update  SellPriceFees
                    foreach (var sellPrice in product.ProductSellPrices)
                    {
                        // Update sellprice 
                        db.Entry(sellPrice).State = EntityState.Modified;
                    }
                }
            }
        }

        public static string DisplayPrintFriendlyName(string name)
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            return textInfo.ToTitleCase(name.Replace(@"GCDI\", "").Replace(".", " "));
        }
    }
}