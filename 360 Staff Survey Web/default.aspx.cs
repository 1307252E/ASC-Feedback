using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using _360_Staff_Survey_Web.Class;
using System.Runtime.InteropServices;
using System.Text;
using System.ComponentModel;

namespace _360_Staff_Survey_Web
{
    public partial class _default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["LoginName"] != null)
            {
                try
                {
                    System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo(Server.MapPath("Charts/"));
                    Empty(directory);
                }
                catch
                {
                }

                string name = Session["LoginName"].ToString();
                if (name != null)
                {
                    staffinfo stf = dbmanager.GetLoginUserId(name);

                    if (stf != null)
                    {
                        string role = stf.Role;
                        Session["UserID"] = stf.Uid;
                        Session["Name"] = stf.Name;
                        Session["Role"] = stf.Role;
                        
                        if (role == "Admin")
                        {
                            Response.Redirect("~/Adminmain.aspx");
                        }
                        
                        else
                        {
                            Response.Redirect("~/Staffmain.aspx");
                        }
                    }
                    else
                    {
                        Response.Redirect("~/accessdenied.aspx");
                    }
                }
                else
                {
                    Response.Redirect("~/accessdenied.aspx");
                }
            }
            else
            {
                Response.Redirect("LoginPage.aspx?timeout");
            }
        }

        protected void Empty(System.IO.DirectoryInfo directory)
        {
            foreach (System.IO.FileInfo file in directory.GetFiles()) file.Delete();
            foreach (System.IO.DirectoryInfo subDirectory in directory.GetDirectories()) subDirectory.Delete(true);
        }
    }
}