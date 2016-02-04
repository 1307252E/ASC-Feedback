using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Text;
using System.Web.UI.WebControls;
using System.Collections;
using System.Web.Security;
using System.Security.Cryptography;
using _360_Staff_Survey_Web.Class;

namespace _360_Staff_Survey_Web
{
    public partial class LoginPage : System.Web.UI.Page
    {
        public ArrayList emaill()
        {
            
            ArrayList abc = new ArrayList();
           



               

                    int count = dbmanager.CheckNotCompletedAppraisal();

                  

                        ArrayList listofFunction = dbmanager.GetDistinctFunctions();

                        foreach (string funct in listofFunction)
                        {
                            ArrayList listofCompleted = dbmanager.GetDistinctNameUidCompletedAppraisal(funct);

                            if (listofCompleted.Count != 0)
                            {
                               

                                foreach (staffappraisal stfapp in listofCompleted)
                                {
                                    string staffname = dbmanager.GetNameViaUserID(stfapp.Uid);
                                    staffinfo stf = dbmanager.GetStaffDetailsViaUid(stfapp.Uid);
                                   
                                    abc.Add(stfapp.Uid.ToString());



                                  
                                }
                                
                            }
                        }
                        
              return abc;
                       
                    }
         
          




        
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Systemtime st = dbmanager.GetSystemTime();
                AutoEmail ae = dbmanager.GetAutoEmailCountandDate();
                DateTime currenttime = DateTime.Now;
                if (currenttime.Date > ae.AutoEmailDate.Date)
                {
                    if (ae.AutoEmailString == "N")
                    {
                        ArrayList emailID = emaill();

                        for (int i = 0; i < emailID.Count; i++)
                        {



                            string y = emailID[i].ToString() + "@student.tp.edu.sg";

                            string passw = "";

                            Systemtime stt = dbmanager.GetSystemTime();
                            string enddate = "";
                            if (st != null)
                            {

                                enddate = st.Enddate.ToLongDateString();
                            }

                            if (emailID[i].ToString() != "")
                            {

                                bool sentemail = email.SendMail("Dear Staff,<br/><br/>" + Environment.NewLine + Environment.NewLine + "The system has not received your participation of the 360 Survey as of today.  <br/><br/> " + Environment.NewLine + Environment.NewLine + "ASC looks forward to your survey participation before the survey closes on " + dbmanager.GetSystemEndTime() + " <br/><br/>" + Environment.NewLine + "Please write to the Administrator should you encounter any system problems. <a><i><font color='#0000ff' size='3' face='Arial'></font></i></a> " + Environment.NewLine + Environment.NewLine + "Together, let us make ASC a school of choice for staff, and a better place to be for all. <br/><br/>", y, "360 Staff Feedback Survey Incomplete.");


                                if (sentemail == true)
                                {
                                    bool truefalse = dbmanager.UpdateAutoEmail(ae.AutoEmailDate, "Y");
                                    if (truefalse == false)
                                    {
                                        dbmanager.UpdateAutoEmails(ae.AutoEmailDate, "Y");
                                    }

                                }

                                else
                                {

                                }
                            }
                        }
                    }
                }
            }
            catch
            {
            }
            staffnumber.Focus();
        }

        protected void login_button_Click(object sender, EventArgs e)
        {
            string staffnum = staffnumber.Text;
            string passw = password.Text;
            string normalhashpassw = "";

            if (passw.Length != 0)
            {
                normalhashpassw = FormsAuthentication.HashPasswordForStoringInConfigFile(passw, "sha1");

                bool result = dbmanager.ValidateLogin(staffnum, normalhashpassw);

                if (result == true)
                {
                    FormsAuthentication.SetAuthCookie(staffnum, false);
                    Session["LoginName"] = staffnum;
                    Session["DateTime"] = DateTime.Now.ToString();
                    Response.Redirect("~/default.aspx");
                }
                else
                {
                    messagelbl.Text = "Invalid username or password.";
                }
            }
        }
    }
}
