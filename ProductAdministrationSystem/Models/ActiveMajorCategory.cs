using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PAS.Models
{
    public class ActiveMajorCategory : IComparable
    {
        public string majorCategory { get; set; }
        public List<string> minorCategories { get; set; }

        private class sortMajorCategories : IComparer
        {
            int IComparer.Compare(object a, object b)
            {
                ActiveMajorCategory c1 = (ActiveMajorCategory)a;
                ActiveMajorCategory c2 = (ActiveMajorCategory)b;
                return String.Compare(c2.majorCategory, c1.majorCategory);
            }
        }
        public ActiveMajorCategory()
        {
            minorCategories = new List<string>();
        }

        int IComparable.CompareTo(object obj)
        {
            ActiveMajorCategory cat = (ActiveMajorCategory)obj;
            return String.Compare(this.majorCategory, cat.majorCategory);
        }
    }
}