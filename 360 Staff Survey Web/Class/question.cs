using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _360_Staff_Survey_Web.Class
{
    public class Question
    {
        private int questionID;
        private string questionDetails;
        private bool questionInclude;
        private string questionRateOne;
        private string questionRateSeven;

        public Question(int qid, string ques, bool qninclude)
        {
            questionID = qid;
            questionDetails = ques;
            questionInclude = qninclude;
        }

        public Question(int qid, string ques, bool qninclude, string rateOne, string rateSeven)
        {
            questionID = qid;
            questionDetails = ques;
            questionInclude = qninclude;
            questionRateOne = rateOne;
            questionRateSeven = rateSeven;
        }

        public int QuestionID
        {
            get { return questionID; }
            set { questionID = value; }
        }

        public string QuestionDetails
        {
            get { return questionDetails; }
            set { questionDetails = value; }
        }
        public string QuestionRateOne
        {
            get { return questionRateOne; }
            set { questionRateOne = value; }
        }
        public string QuestionRateSeven
        {
            get { return questionRateSeven; }
            set { questionRateSeven = value; }
        }

        public bool QuestionInclude
        {
            get { return questionInclude; }
            set { questionInclude = value; }
        }
    
    }
}