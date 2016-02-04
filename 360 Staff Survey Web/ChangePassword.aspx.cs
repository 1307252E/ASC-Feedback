using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Text;
using System.Web.UI.WebControls;
using System.Collections;
using System.Web.Security;
using _360_Staff_Survey_Web.Class;

namespace _360_Staff_Survey_Web
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["ChangePassw"] = true;
            if (Session["UserID"] != null)
            {
                string nameid = (string)(Session["UserID"]);
                StfNumlbl.Text = nameid;
            }
            else
            {
                Response.Redirect("default.aspx");
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string userid = Session["UserID"].ToString();
            Messagelbl.Text = "";
            //String salt = CreateSalt(10);
            bool isvaliduser = dbmanager.CheckValidUser(userid);
            string current = FormsAuthentication.HashPasswordForStoringInConfigFile(CurrentPassw.Text, "sha1");
            // String current = GenerateSHA1Hash(CurrentPassw.Text, salt);
            string newpass = FormsAuthentication.HashPasswordForStoringInConfigFile(NewPassw.Text, "sha1");
            // String newpass = GenerateSHA1Hash(CurrentPassw.Text, salt);
            string confirmpass = FormsAuthentication.HashPasswordForStoringInConfigFile(ConfirmPasw.Text, "sha1");
            // String confirmpass = GenerateSHA1Hash(CurrentPassw.Text, salt);

            if (isvaliduser == true)
            {
                if (CurrentPassw.Text.Length != 0 && NewPassw.Text.Length != 0 && ConfirmPasw.Text.Length != 0)
                {
                    if (confirmpass == current)
                    {
                        Messagelbl.Text = "New password is similar to previous. Please try again.";
                    }
                    else if (newpass == confirmpass)
                    {
                        bool checkvalidpassw = dbmanager.CheckValidCurrentPassword(userid, current);

                        if (checkvalidpassw == true)
                        {
                            bool result = dbmanager.UpdatePassw(userid, confirmpass);
                            if (result == true)
                            {
                                MessageBoxShow("Password changed successfully.");
                            }
                            else
                            {
                                Messagelbl.Text = "Fail to change password.";
                            }
                        }
                        else
                        {
                            Messagelbl.Text = "Invalid current password.";
                        }
                    }
                }
            }
        }

        /* public String CreateSalt(int size)
        {
            var rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            var buff = new byte[size];
            rng.GetBytes(buff);
            return Convert.ToBase64String(buff);
        }

        public String GenerateSHA1Hash(String input, String salt)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(input + salt);
            System.Security.Cryptography.SHA1Managed sha1hashstring = new System.Security.Cryptography.SHA1Managed();
            byte[] hash = sha1hashstring.ComputeHash(bytes);

            return Convert.ToBase64String(hash);
        } */

        private void MessageBoxShow(string message)
        {
            string strScript = "<script>";
            strScript += "alert('" + message + "');";
            strScript += "window.location='default.aspx';";
            strScript += "</script>";
            this.ClientScript.RegisterStartupScript(this.GetType(), "Startup", strScript);
        }
        protected void CancelBtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("default.aspx");
        }
    }

}