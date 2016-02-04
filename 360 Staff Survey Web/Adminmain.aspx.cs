using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using _360_Staff_Survey_Web.Class;
using System.Collections;

namespace _360_Staff_Survey_Web
{
    public partial class Adminmain : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            #region SelectedLinkBtn
            if (Session["SubmitAppraisalLink"] != null)
            {
                SubmitLink.Style.Add("color", "Purple");
            }
            if (Session["ViewAppraisalLink"] != null)
            {
                ViewAppraisalLink.Style.Add("color", "Purple");
            }
            if (Session["ManageQuestionLink"] != null)
            {
                ManageQuestionLink.Style.Add("color", "Purple");
            }
            if (Session["ManageUserLink"] != null)
            {
                ManageUserLink.Style.Add("color", "Purple");
            }
            if (Session["ExportImportStaffLink"] != null)
            {
                ExportImportStaffLink.Style.Add("color", "Purple");
            }
            if (Session["SetOpenCloseLink"] != null)
            {
                SetOpenCloseLink.Style.Add("color", "Purple");
            }
            if (Session["deleteAllLink"] != null)
            {
                deleteAllLink.Style.Add("color", "Purple");
            }
            if (Session["deleteSingleLink"] != null)
            {
                deleteSingleLink.Style.Add("color", "Purple");
            }
            if (Session["ViewNotCompletedLink"] != null)
            {
                ViewNotCompletedLink.Style.Add("color", "Purple");
            }
            if (Session["ViewIndividualAllLink"] != null)
            {
                ViewIndividualAllLink.Style.Add("color", "Purple");
            }
            if (Session["ViewAppraisalChartLink"] != null)
            {
                ViewAppraisalChartLink.Style.Add("color", "Purple");
            }
            if (Session["ResetPasswordLink"] != null)
            {
                ResetPasswordLink.Style.Add("color", "Purple");
            }
            if (Session["AppraisalDisplayDetailsLink"] != null)
            {
                AppraisalDisplayDetailsLink.Style.Add("color", "Purple");
            }
            #endregion
        }

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
                    else
                    {
                        Session["HomeClicked"] = true;
                        string uid = Session["UserID"].ToString();
                        string username = Session["Name"].ToString();
                        //staffName.Text = username;

                        //if appriasal submitted
                        bool result = dbmanager.CheckIfAppraisalSubmitted(uid);

                        //if appraisal started
                        Systemtime st = dbmanager.GetSystemTime();
                        DateTime today = DateTime.Today;
                        if (st != null)
                        {
                            Session["EndTime"] = st.Enddate;
                            int zzp = datediff.dateDiff(st.Enddate);

                            if (st.Startdate > today)
                            {
                                SubmitAppraisalLbl.Text = "System period <b>is closed<b>";
                                SubmitLink.Visible = false;
                            }
                            else if (zzp <= (-1))
                            {
                                SubmitAppraisalLbl.Text = "System period <b>is closed<b>";
                                SubmitLink.Visible = false;
                            }
                            else if (result == true)
                            {
                                Session["Submitted"] = true;
                                SubmitAppraisalLbl.Text = "Your feedback has been <b>submitted<b>";
                                SubmitLink.Visible = false;
                            }
                            else
                            {
                                Session["Submitted"] = null;
                                SubmitAppraisalLbl.Text = "To start peer feedback, click ";
                                SubmitLink.Visible = true;
                            }
                        }
                        else
                        {
                            SubmitLink.Visible = false;
                        }

                        

                        bool countallappraisal = dbmanager.CountAllAppraisal();
                        if (countallappraisal == true)
                        {
                            //ViewIndividualAllLbl.Text = "To view all or individual staff report, click ";
                            ViewIndividualAllLink.Visible = true;
                        }
                        else
                        {
                            ViewIndividualAllLbl.Text = "No report is <b>found<b>";
                            ViewIndividualAllLink.Visible = false;
                        }

                        //view completed appraisal
                        int checkcompleted = dbmanager.CheckNotCompletedAppraisal();
                        if (checkcompleted > 0)
                        {
                            //view individual and all
                            Session["CompletedCount"] = true;
                            //ViewNotCompletedLbl.Text = "To view summary of staff who have not completed evaluation, click ";
                            ViewNotCompletedLink.Visible = true;
                        }
                        else
                        {
                            Session["CompletedCount"] = null;
                            ViewNotCompletedLbl.Text = "All feedback for the period have been <b>completed<b>";
                            ViewNotCompletedLink.Visible = false;
                        }
                        //ManageQuestionPanel.Visible = true;
                        ManageQuestionLink.Visible = true;
            
                        int countapp = dbmanager.GetCountYourAppraisal(uid);

                        if (countapp != 0)
                        {
                            //if appraisal submitted
                            //ViewAppraisalLbl.Text = "To view own evaluation report, click ";
                            ViewAppraisalLbl.Text = "To view own feedback report, click ";
                            ViewAppraisalLink.Visible = true;
                        }
                        else
                        {
                            ViewAppraisalLbl.Text = "You have 0 feedback(s) <b>received<b>";
                            ViewAppraisalLink.Visible = false;

                        }


                        bool checkifappraisalsec = dbmanager.CountAllAppraisal();
                        if (checkifappraisalsec == true)
                        {
                            ViewAppraisalChartLink.Visible = true;
                        }
                        else
                        {
                            ViewAppraisalChart.Text = "No chart is <b>found<b> ";
                            ViewAppraisalChartLink.Visible = false;
                        }

                        DeleteAllLbl.Text = "To delete all reports (period), click ";

                        DeleteSingleUidLbl.Text = "Delete a single report (period): ";


                        ArrayList listofuid = dbmanager.GetAllUid();
                        if (listofuid.Count != 0)
                        {
                            ddlUid.DataSource = listofuid;
                            ddlUid.DataBind();
                        }
                        else
                        {
                            ManageAppraisalSummary.Visible = false;
                        }
                    }
                }
                else
                {
                    Response.Redirect("accessdenied.aspx");
                }

                //ViewAppraisalLbl.Visible = false;
                //ViewAppraisalLink.Visible = false;
            }
        }

        protected void SubmitAppraisalLink_Click(object sender, EventArgs e)
        {
            SubmitLink.Style.Add("color", "Purple");
            Session["SubmitAppraisalLink"] = true;
            Response.Redirect("~/SubmitAppraisal.aspx");
        }

        protected void ViewAppraisalLink_Click(object sender, EventArgs e)
        {
            ViewAppraisalLink.Style.Add("color", "Purple");
            Session["ViewAppraisalLink"] = true;
            Response.Redirect("~/ViewAppraisal.aspx");
        }

        protected void ManageQuestionLink_Click(object sender, EventArgs e)
        {
            ManageQuestionLink.Style.Add("color", "Purple");
            Session["ManageQuestionLink"] = true;
            Response.Redirect("~/ManageQuestions.aspx");
        }

        protected void ManageUserLink_Click(object sender, EventArgs e)
        {
            ManageUserLink.Style.Add("color", "Purple");
            Session["ManageUserLink"] = true;
            Response.Redirect("~/ManageUser.aspx");
        }

        protected void ExportImportStaffLink_Click(object sender, EventArgs e)
        {
            ExportImportStaffLink.Style.Add("color", "Purple");
            Session["ExportImportStaffLink"] = true;
            Response.Redirect("~/ImportExportStaffList.aspx");
        }

        protected void SetOpenCloseLink_Click(object sender, EventArgs e)
        {
            SetOpenCloseLink.Style.Add("color", "Purple");
            Session["SetOpenCloseLink"] = true;
            Response.Redirect("~/ManageSystem.aspx");
        }

        protected void deleteAllLink_Click(object sender, EventArgs e)
        {
            deleteAllLink.Style.Add("color", "Purple");
            Session["deleteAllLink"] = true;
            bool result = dbmanager.DeleteAllAppraisal();
            if (result == true)
            {
                MessageBoxShow("Deleted successfully.");
            }
            else
            {
                MessageBoxShow("No record to delete.");
            }
        }

        protected void deleteSingleLink_Click(object sender, EventArgs e)
        {
            deleteSingleLink.Style.Add("color", "Purple");
            Session["deleteSingleLink"] = true;
            bool result = dbmanager.DeleteAllAppraisalSingle(ddlUid.Text);
            if (result == true)
            {
                MessageBoxShow("Deleted successfully.");
            }
            else
            {
                MessageBoxShow("No record to delete.");
            }
        }

        protected void ViewIndividualAllLink_Click(object sender, EventArgs e)
        {
            ViewIndividualAllLink.Style.Add("color", "Purple");
            Session["ViewIndividualAllLink"] = true;
            Response.Redirect("~/ViewAppraisalAllHistory.aspx");
        }

        private void MessageBoxShow(string message)
        {
            string strScript = "<script>";
            strScript += "alert('" + message + "');";
            strScript += "window.location='default.aspx';";
            strScript += "</script>";
            this.ClientScript.RegisterStartupScript(this.GetType(), "Startup", strScript);
        }

        protected void ViewNotCompletedLink_Click(object sender, EventArgs e)
        {
            ViewNotCompletedLink.Style.Add("color", "Purple");
            Session["ViewNotCompletedLink"] = true;
            Response.Redirect("~/ViewNotCompletedAppraisal.aspx");
        }

        protected void ViewAppraisalChartLink_Click(object sender, EventArgs e)
        {
            ViewAppraisalChartLink.Style.Add("color", "Purple");
            Session["ViewAppraisalChartLink"] = true;
            Response.Redirect("~/ViewHistoryChart.aspx");
        }

        protected void ResetPasswordLink_Click(object sender, EventArgs e)
        {
            ResetPasswordLink.Style.Add("color", "Purple");
            Session["ResetPasswordLink"] = true;
            Response.Redirect("~/ForgetPassword.aspx");
        }

        protected void AppraisalDisplayDetailsLink_Click(object sender, EventArgs e)
        {
            AppraisalDisplayDetailsLink.Style.Add("color", "Purple");
            Session["AppraisalDisplayDetailsLink"] = true;
            Response.Redirect("~/AppraisalDisplay.aspx");
        }

        protected void UpdateStaffLink_Click(object sender, EventArgs e)
        {
            UpdateStaffDept.Style.Add("color", "Purple");
            
            Response.Redirect("~/ChangeStaffDept.aspx");

        }
    }
}