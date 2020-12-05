using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project1
{
    public static class PageSizes
    {
        public static SelectList PgSizes
        {
            get
            {
                return (new SelectList(new List<int> { 6, 12, 18, 36, 72, 96 },
                selectedValue: 6));
            }
        }
    }
}