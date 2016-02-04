using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _360_Staff_Survey_Web.Class
{
    public class AutoEmail
    {
        private DateTime autoemaildate;
        private string autoemailstring;

        
        public AutoEmail(DateTime autoemaildate, string autoemailstring)
        {
            this.autoemaildate = autoemaildate;
            this.autoemailstring = autoemailstring;
        }

        public DateTime AutoEmailDate
        {
            get { return autoemaildate; }
            set { autoemaildate = value; }
        }
        public string AutoEmailString
        {
            get { return autoemailstring; }
            set { autoemailstring = value; }
        }
    }
}