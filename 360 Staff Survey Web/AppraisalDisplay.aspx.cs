using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using _360_Staff_Survey_Web.Class;

namespace _360_Staff_Survey_Web
{
    public partial class AppraisalDisplay : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session["Role"] != null)
                {
                    string role = Session["Role"].ToString();
                    if (role != "Admin")
                    {
                        Response.Redirect("accessdenied.aspx");
                    }
                }

                try
                {
                    AppraisalDetails apprasial = dbmanager.GetAppraisalDetails();
                    if (apprasial != null)
                    {
                        Session["AppraisalID"] = apprasial.AppraisalID;
                        tbxDetails.Text = Server.HtmlDecode(apprasial.AppraisalDescription);
                    }
                }
                catch (Exception ex)
                {}
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (tbxDetails.Text != "")
                {
                    int id = Convert.ToInt32(Session["AppraisalID"].ToString());
                    //string details = tbxDetails.Value.ToString();
                    string details = tbxDetails.Text;
                    string s1 = details.Replace("<script>", "");
                    string s2 = s1.Replace("</script>","");
                    string details2 = Server.HtmlEncode(details);
                    bool result = dbmanager.UpdateAppraisalDetails(id, s2);

                    if (result == true)
                    {
                        MessageBoxShow("Update Successfully");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void MessageBoxShow(string message)
        {
            string strScript = "<script>";
            strScript += "alert('" + message + "');";
            strScript += "window.location='AppraisalDisplay.aspx';";
            strScript += "</script>";
            this.ClientScript.RegisterStartupScript(this.GetType(), "Startup", strScript);
        }
    }
}