using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _360_Staff_Survey_Web.Class
{
    public class staffappraisal
    {
        private string uid;
        private int questionid;
        private string appstaffuid;
        private double appresult;
        private string appremarks;
        private DateTime dateTime;
        private DateTime systemEndTime;

        public staffappraisal(string uid, int questionid, string appstaffuid, double appresult, string appremarks, DateTime dateTime, DateTime systemEndTime)
        {
            this.uid = uid;
            this.questionid = questionid;
            this.appstaffuid = appstaffuid;
            this.appresult = appresult;
            this.appremarks = appremarks;
            this.dateTime = dateTime;
            this.systemEndTime = systemEndTime;
        }

        public string Uid
        {
            get { return uid; }
            set { uid = value; }
        }
        public int Questionid
        {
            get { return questionid; }
            set { questionid = value; }
        }
        public string Appstaffuid
        {
            get { return appstaffuid; }
            set { appstaffuid = value; }
        }
        public double Appresult
        {
            get { return appresult; }
            set { appresult = value; }
        }
        public string Appremarks
        {
            get { return appremarks; }
            set { appremarks = value; }
        }
        public DateTime DateTime
        {
            get { return dateTime; }
            set { dateTime = value; }
        }
        public DateTime SystemEndTime
        {
            get { return systemEndTime; }
            set { systemEndTime = value; }
        }
    }
}