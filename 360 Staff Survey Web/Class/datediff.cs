using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _360_Staff_Survey_Web.Class
{

    public class datediff
    {

        public static int dateDiff(DateTime stpdate)
        {
            DateTime d1 = (DateTime)(stpdate);//stopday
            DateTime d2 = DateTime.Today; //today

            TimeSpan diff = d1.Subtract(d2);
            string x = Convert.ToString(diff);
            if (x.StartsWith("0"))
            {
                return 0;
            }
            string[] p = x.Split('.');
            int i = Convert.ToInt32(p[0]);
            return i;
        }

    }
}