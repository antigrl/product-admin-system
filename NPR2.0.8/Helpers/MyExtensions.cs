using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NPR2._0._8.Models;
using System.Reflection;
using System.ComponentModel;

namespace NPR2._0._8.Helpers
{
    public static class MyExtensions
    {
        // Generates a List for creating a Dropdown for Enums
        public static List<SelectListItem> GetGenericEnumDropDown<T>() where T : struct
        {
            List<SelectListItem> dropdownList = new List<SelectListItem>();
            Array itemValues = Enum.GetValues(typeof(T));
            foreach(Enum value in itemValues)
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

            if(attributes != null && attributes.Length > 0)
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

            if(objectStatus == MyExtensions.GetEnumDescription(Status.Send_To_Account_Manager))
            {
                emailToList.Add(EmailTo.Account_Manager);
            }
            else if(objectStatus == MyExtensions.GetEnumDescription(Status.Send_To_Mentor))
            {
                emailToList.Add(EmailTo.Mentor);
            }
            else if(objectStatus == MyExtensions.GetEnumDescription(Status.Send_To_Inventory_Buyers))
            {
                emailToList.Add(EmailTo.Inventory_Buyers);
            }
            else if(objectStatus == MyExtensions.GetEnumDescription(Status.Send_To_Merchandisers))
            {
                emailToList.Add(EmailTo.Merchandisers);
            }

            return emailToList;
        }
    }
}