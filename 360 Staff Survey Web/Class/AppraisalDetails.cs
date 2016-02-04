using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _360_Staff_Survey_Web.Class
{
    public class AppraisalDetails
    {
        private int appraisalID;
        private string appraisalDescription;

        public AppraisalDetails(int appraisalID, string appraisalDescription)
        {
            this.appraisalDescription = appraisalDescription;
            this.appraisalID = appraisalID;
        }

        public int AppraisalID
        {
            get { return appraisalID; }
            set { appraisalID = value; }
        }

        public string AppraisalDescription
        {
            get { return appraisalDescription; }
            set { appraisalDescription = value; }
        }
    }
}