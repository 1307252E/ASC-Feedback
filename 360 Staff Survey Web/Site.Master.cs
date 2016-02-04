using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using _360_Staff_Survey_Web.Class;

namespace _360_Staff_Survey_Web
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        public static string starttimee = Convert.ToString(DateTime.Now.ToString());
        protected void Page_Load(object sender, EventArgs e)
        {
            defaultCountdown.Visible = false;
            try
            {

                var start = Convert.ToDateTime(Session["DateTime"].ToString());
                datetime.Text = (TimeSpan.FromHours(2) - (DateTime.Now - start)).ToString("hh\\:mm\\:ss");
                string role = Session["Role"].ToString();
            }
            catch (Exception ee)
            {

            }

                
            
            //if (role == "Admin")
            //{
            //    //Page.Controls.Add(new LiteralControl("<p>New<br />Line</p>"));
            //    b1.Attributes["style"] = string.Format("background: url({0});", "Image/green.jpg");
                
            //}
            //else if (role == "Staff")
            //{
            //    b1.Attributes["style"] = string.Format("background: url({0});", "Image/lightgreen.jpg");
            //}

            #region timer time
            DateTime time = DateTime.Now;
            if (Session["DueTimer"] == null)
            {
                DateTime duetime = time.AddMinutes(120);
                Session["DueTimer"] = duetime;
            }
            else
            {
                DateTime timeleft = (DateTime)Session["DueTimer"];
                DateTime passCurrentTime = DateTime.Now;
                TimeSpan timeleft1 = timeleft.Subtract(passCurrentTime);
                Session["leftTime"] = timeleft1.TotalSeconds;
            }

            Label hidden = (Label)FindControl("hiddenTime");
            if (hiddenTime != null)
            {
                if (hiddenTime.Text == "")
                {
                    TimeSpan timeleft = Convert.ToDateTime(Session["DueTimer"].ToString()).Subtract(time);
                    hiddenTime.Text = Convert.ToString(Convert.ToUInt32(timeleft.TotalSeconds));
                    HiddenField1.Value = hiddenTime.Text;
                }
                else
                {
                    try
                    {
                        hiddenTime.Text = Session["leftTime"].ToString();
                        HiddenField1.Value = hiddenTime.Text;
                    }
                    catch (Exception ex)
                    {
                        Session.Clear();
                        Session.Abandon();
                        ViewState.Clear();
                        FormsAuthentication.SignOut();
                        Page.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        Response.Redirect("~/LoginPage.aspx");
                    }
                }
            }

            #endregion
            
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();

            Page.Title = "Online 360° Leadership System";

            copyRightDate.Text = " " + DateTime.Today.Date.Year.ToString();

            //HomeLinkBtn.Style.Add("color", "Blue");
            //ChangePasswLink.Style.Add("color", "Blue");
            //LogoutLinkBtn.Style.Add("color", "Blue");
            //if (Session["HomeClicked"] != null)
            //{
            //    HomeLinkBtn.Style.Add("color", "Purple");
            //    Session["HomeClicked"] = null;
            //}
            //if (Session["ChangePassw"] != null)
            //{
            //    ChangePasswLink.Style.Add("color", "Purple");
            //    Session["ChangePassw"] = null;
            //}

            try
            {
                string uid = Session["UserID"].ToString();
                string username = Session["Name"].ToString();

                if (Session["UserID"] != null && Session["Name"] != null)
                {
                    staffName.Text = username;
                    staffName.ForeColor = System.Drawing.Color.Brown;
                }

            }
            catch
                (Exception ex) { }
        }
        

        protected void HomeLinkBtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/default.aspx");
        }

        protected void LogoutLinkBtn_Click(object sender, EventArgs e)
        {
            try
            {
                Session.Clear();
                Session.Abandon();
                ViewState.Clear();
                FormsAuthentication.SignOut();
                //FormsAuthentication.RedirectToLoginPage();
                Page.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Redirect("~/LoginPage.aspx");
            }
            catch (Exception) { }
        }

        protected void ChangePasswLink_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ChangePassword.aspx");
        }
    }
}
