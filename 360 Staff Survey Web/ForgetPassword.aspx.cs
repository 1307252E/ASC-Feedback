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
    public partial class ForgetPassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Role"] != null)
            {
                string role = Session["Role"].ToString();
                if (role != "Admin")
                {
                    Response.Redirect("accessdenied.aspx");
                }
                else
                {
                    emailTbx.Focus();
                }
            }
            else
            {
                Response.Redirect("accessdenied.aspx");
            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            Response.Redirect("default.aspx");
        }

        private static string CreateRandomPassword(int passwordLength)
        {
            string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789@#&";
            char[] chars = new char[passwordLength];
            Random rd = new Random();

            for (int i = 0; i < passwordLength; i++)
            {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }

            return new string(chars);
        }

        protected void retrievebtn_Click(object sender, System.EventArgs e)
        {
            string y = emailTbx.Text;
            string passw = "";

            if (emailTbx.Text != "")
            {
                string[] splitinfo = emailTbx.Text.Split('@');
                if (splitinfo.LongLength > 1)
                {
                    bool checkisuservalid = dbmanager.CheckValidUser(splitinfo[0].ToLower());
                    //if (checkisuservalid == true && splitinfo[1].Equals("tp.edu.sg"))
                    if (checkisuservalid == true && splitinfo[1].Equals("hotmail.com"))
                    {
                        string q = CreateRandomPassword(8);

                        //bool sentemail = email.SendMail("Thank you for using the Online 360° leadership system." + Environment.NewLine + Environment.NewLine + "Your new password for login to the system is: " + q + Environment.NewLine + Environment.NewLine + "You are advised to change your password once you have logged in." + Environment.NewLine + "For login, please visit: " + "http://ascapps.tp.edu.sg/360Leadership/LoginPage.aspx" + Environment.NewLine + Environment.NewLine + "For any enquiries to the system, please contact asc webmaster or (x5376)", splitinfo[0].ToLower() + "@tp.edu.sg", "360° Leadership System Password Recovery");

                        bool sentemail = email.SendMail("<p>Thank you for using the Online 360° leadership system.</p>" + Environment.NewLine + Environment.NewLine + "Your new password for login to the system is: " + q + Environment.NewLine + Environment.NewLine + "<br>You are advised to change your password once you have logged in.</br>" + Environment.NewLine + "<br>For login, please visit: " + "<a href= 'http://ascapps.tp.edu.sg/360Leadership/LoginPage.aspx' target= '_blank'>http://ascapps.tp.edu.sg/360Leadership/LoginPage.aspx</a></br>" + Environment.NewLine + Environment.NewLine + "<br>For any enquiries to the system, please contact asc webmaster or (x5376)</br>", splitinfo[0].ToLower() + "@hotmail.com", "360° Leadership System Password Recovery");
                        passw = FormsAuthentication.HashPasswordForStoringInConfigFile(q, "sha1");

                        if (sentemail == true)
                        {
                            dbmanager.UpdatePassw(splitinfo[0].ToLower(), passw);
                            messagelbl.Text = ("Password generated and sent to email.");
                        }
                    }
                    else
                    {
                        messagelbl.Text = "Invalid email.";
                    }
                }
            }
        }
    }
}