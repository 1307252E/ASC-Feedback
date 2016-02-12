using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using _360_Staff_Survey_Web.Class;

namespace _360_Staff_Survey_Web
{
    public partial class ManageSystem : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            #region SelectedLinkBtn
            if (Session["ExtensionLink"] != null)
            {
                ExtensionLink.Style.Add("color", "Purple");
            }

            #endregion
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Session["StartDate"] = null;
                Session["EndDate"] = null;

                if (Session["Role"] != null)
                {
                    string role = Session["Role"].ToString();
                    if (role != "Admin")
                    {
                        Response.Redirect("accessdenied.aspx");
                    }
                    else
                    {
                        Systemtime st = dbmanager.GetSystemTime();

                        if (st != null)
                        {
                            startlbl.Text = st.Startdate.ToLongDateString();
                            endlbl.Text = st.Enddate.ToLongDateString();
                        }
                        else
                        {
                            startlbl.Text = "--";
                            endlbl.Text = "--";
                        }
                    }
                }
                else
                {
                    Response.Redirect("accessdenied.aspx");
                }
            }
        }

        protected void ClearBtn_Click(object sender, EventArgs e)
        {
            SelectStartTbx.Text = "";
            SelectEndDateTbx.Text = "";
            Session["StartDate"] = null;
            Session["EndDate"] = null;
        }

        protected void Confirmbtn_Click(object sender, EventArgs e)
        {
            if (Session["StartDate"] != null && Session["EndDate"] != null)
            {
                if (SelectStartTbx.Text != "" && SelectEndDateTbx.Text != "")
                {
                    Systemtime snull = dbmanager.GetSystemTime();
                    if (snull == null)
                    {
                        if (Session["Invalid"] == null)
                        {
                            DateTime d1 = (DateTime)Session["StartDate"];
                            DateTime d2 = (DateTime)Session["EndDate"];
                            Systemtime st = new Systemtime(d1, d2);

                            bool result = dbmanager.InsertSystemTime(st);

                            if (result == true)
                            {
                                try
                                {
                                    DateTime autoemailtime = d2.AddDays(7);
                                    bool truefalse = dbmanager.UpdateAutoEmail(autoemailtime, "N");
                                    if (truefalse == false)
                                    {
                                        bool falsetrue = dbmanager.InsertAutoEmailTime(d2);
                                        if (falsetrue == false)
                                        {
                                            //Last resort!
                                            dbmanager.UpdateAutoEmails(autoemailtime, "N");
                                        }
                                    }

                                }
                                catch
                                {

                                }
                                dbmanager.DeletePreviousAllSaved();
                                Messaglbl.Text = "Dates updated successfully.";

                                Systemtime sttime = dbmanager.GetSystemTime();

                                if (sttime != null)
                                {
                                    startlbl.Text = sttime.Startdate.ToLongDateString();
                                    endlbl.Text = sttime.Enddate.ToLongDateString();
                                }
                                else
                                {
                                    startlbl.Text = "--";
                                    endlbl.Text = "--";
                                }
                            }
                            else
                            {
                                Messaglbl.Text = "Fail to update date.";
                            }
                        }
                    }
                    else
                    {
                        if (Session["Invalid"] == null)
                        {
                            bool insertresult = false;
                            DateTime d1 = (DateTime)Session["StartDate"];
                            DateTime d2 = (DateTime)Session["EndDate"];
                            Systemtime st = new Systemtime(d1, d2);
                            bool checkexistmonth = dbmanager.CheckExistMonth(d2);
                            if (checkexistmonth == false)
                            {
                                bool result = dbmanager.DeleteSystemTime();

                                if (result == true)
                                {
                                    insertresult = dbmanager.InsertSystemTime(st);
                                }
                            }
                            if (insertresult == true)
                            {
                                try
                                {
                                    DateTime autoemailtime = d2.AddDays(7);
                                   bool truefalse = dbmanager.UpdateAutoEmail(autoemailtime, "N");
                                   if (truefalse == false)
                                   {
                                       bool falsetrue = dbmanager.InsertAutoEmailTime(d2);
                                       if (falsetrue == false)
                                       {
                                           //Last resort!
                                           dbmanager.UpdateAutoEmails(autoemailtime, "N");
                                       }
                                   }
                                    
                                }
                                catch
                                {
                                    
                                }
                                dbmanager.DeletePreviousAllSaved();
                                Messaglbl.Text = "Dates updated successfully.";

                                Systemtime sttime = dbmanager.GetSystemTime();

                                if (sttime != null)
                                {
                                    startlbl.Text = sttime.Startdate.ToLongDateString();
                                    endlbl.Text = sttime.Enddate.ToLongDateString();
                                }
                                else
                                {
                                    startlbl.Text = "--";
                                    endlbl.Text = "--";
                                }
                            }
                            else
                            {
                                Messaglbl.Text = "Fail to update date. Duplicate month might exist for the appraisal.";
                            }
                        }
                    }
                }
                else
                {
                    Messaglbl.Text = "The dates are invalid, please reselect the dates.";
                }
            }
            else
            {
                MessageBoxShow("The dates are invalid, please reselect the dates.");
            }
        }

        protected void SelectEndDateTbx_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DateTime startdate = Convert.ToDateTime(SelectStartTbx.Text.Trim());
                DateTime enddate = Convert.ToDateTime(SelectEndDateTbx.Text.Trim());

                if (startdate != null && enddate != null)
                {
                    DateTime today = DateTime.Today; //today
                    Session["StartDate"] = startdate;
                    Session["EndDate"] = enddate;

                    if (enddate <= startdate || enddate < today)
                    {
                        Session["Invalid"] = true;
                        Messaglbl.Text = "The dates are invalid, please reselect the dates.";
                    }
                    else
                    {
                        Session["Invalid"] = null;
                        Messaglbl.Text = "Note: The starting date for the system is " + startdate.ToLongDateString() + " and the closure date is " + enddate.ToLongDateString();
                    }
                }
                else
                {
                    Messaglbl.Text = "The dates are invalid, please reselect the dates.";
                }
            }
            catch
            {
            }

        }

        protected void ExtensionLink_Click(object sender, EventArgs e)
        {
            ExtensionLink.Style.Add("color", "Purple");
            Session["ExtensionLink"] = true;
            Systemtime st = dbmanager.GetSystemTime();

            if (st != null)
            {
                startlblchange.Text = st.Enddate.ToLongDateString();
                Session["Start"] = startlblchange.Text;
            }
            else
            {
                startlblchange.Text = "--";
            }
            this.ModalPopupExtender.Show();
        }

        protected void ConfirmChangeBtn_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime startDate = Convert.ToDateTime(Session["Start"]);
                DateTime previousenddate = Convert.ToDateTime(startlblchange.Text);
                DateTime extendeddates = Convert.ToDateTime(closureEndTbx.Text.Trim());

                if (extendeddates != null)
                {
                    DateTime today = DateTime.Today; //today

                    if (extendeddates <= startDate || extendeddates < today)
                    {
                        MessageBoxShowWithOutRedirect("The dates are invalid, please reselect the dates.");
                    }
                    else
                    {
                        bool result = dbmanager.UpdateSystemTime(previousenddate, extendeddates);

                        if (result == true)
                        {
                            dbmanager.UpdateSavedAppraisalEndTime(previousenddate, extendeddates);
                            dbmanager.UpdateAppraisalEndTime(previousenddate, extendeddates);
                            MessageBoxShow("New closure dates updated.");
                        }
                        else
                        {
                            MessageBoxShow("Fail to update updated.");
                        }
                    }
                }
                else
                {
                    MessageBoxShowWithOutRedirect("Required field for closure date.");
                }
            }
            catch
            {
                MessageBoxShowWithOutRedirect("The dates are invalid, please reselect the dates.");
            }
            this.ModalPopupExtender.Show();
        }

        private void MessageBoxShowWithOutRedirect(string message)
        {
            string strScript = "<script>";
            strScript += "alert('" + message + "');";
            strScript += "</script>";
            this.ClientScript.RegisterStartupScript(this.GetType(), "Startup", strScript);
        }

        private void MessageBoxShow(string message)
        {
            string strScript = "<script>";
            strScript += "alert('" + message + "');";
            strScript += "window.location='ManageSystem.aspx';";
            strScript += "</script>";
            this.ClientScript.RegisterStartupScript(this.GetType(), "Startup", strScript);
        }
    }
}