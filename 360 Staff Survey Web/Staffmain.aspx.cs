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
    public partial class Staffmain : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            #region SelectedLinkBtn
            if (Session["SubmitAppraisalStaffLink"] != null)
            {
                SubmitLink.Style.Add("color", "Purple");
            }
            if (Session["ViewAppraisalStaffLink"] != null)
            {
                ViewAppraisalLink.Style.Add("color", "Purple");
            }
            if (Session["ViewIndividualAllLink"] != null)
            {
                ViewIndividualAllLink.Style.Add("color", "Purple");
            }
            if (Session["ViewAppraisalChartLink"] != null)
            {
                ViewAppraisalChartLink.Style.Add("color", "Purple");
            }

            #endregion
        }
        //public static string status;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Response.Clear();
                if (Session["Role"] != null)
                {
                    string role = Session["Role"].ToString();
                    if (role == "Admin")
                    {
                        Response.Redirect("accessdenied.aspx");
                    }
                    else
                    {
                        Session["Submitted"] = null;
                        Session["HomeClicked"] = true;
                        string uid = Session["UserID"].ToString();
                        string username = Session["Name"].ToString();

                        if (role != "User")
                        {
                            ArrayList listofsec = new ArrayList();
                            bool checkifappraisalsec = false;
                            ManageAppraisalPanel.Visible = true;
                            staffinfo stfinfo = dbmanager.GetStaffDetailsViaUid(uid);
                            if (stfinfo.Section != "ALL")
                            {
                                string[] arraysec = stfinfo.Section.Split(',');
                                foreach (string sec in arraysec)
                                {
                                    if (sec != "")
                                    {
                                        listofsec.Add(sec);
                                    }
                                }
                                checkifappraisalsec = dbmanager.CheckIfExistSection(listofsec);
                            }
                            else
                            {
                                checkifappraisalsec = dbmanager.CountAllAppraisal();
                            }

                            if (role != "User")
                            {
                                ViewGraphReportLbl.Text = "To view detailed graph report of section, Click ";
                                ViewGraphReportLink.Visible = true;
                            }

                            if (checkifappraisalsec == true)
                            {
                                ViewIndividualAllLbl.Text = "To view by section or individual staff report, click ";
                            }
                            else
                            {
                                ViewIndividualAllLbl.Text = "No staff feedback of the same section is <b>found<b>";
                                ViewIndividualAllLink.Visible = false;
                            }

                            bool checkapp = dbmanager.CountAllAppraisal();
                            if (checkapp == true)
                            {
                                ViewAppraisalChart.Text = "To view peer feedback chart, click ";
                                ViewAppraisalChartLink.Visible = true;
                            }
                            else
                            {
                                ViewAppraisalChart.Text = "No chart is <b>found<b> ";
                                ViewAppraisalChartLink.Visible = false;
                            }
                        }
                        else
                        {
                            ManageAppraisalPanel.Visible = false;
                        }

                        //staffName.Text = username;
                        //if appriasal submitted
                        bool result = dbmanager.CheckIfAppraisalSubmitted(uid);

                        //if appraisal started
                        Systemtime st = dbmanager.GetSystemTime();
                        DateTime today = DateTime.Today;
                        if (st != null)
                        {
                            int zzp = datediff.dateDiff(st.Enddate);
                            Session["EndTime"] = st.Enddate;

                            if (st.Startdate > today)
                            {
                                SubmitAppraisalLbl.Text = "Feedback period is <b>closed<b>";
                                SubmitLink.Visible = false;
                                ViewAppraisalLbl.Text = "To view own feedback report, click ";
                                ViewAppraisalLink.Visible = true;
                            }
                            else if (zzp <= (-1))
                            {
                                SubmitAppraisalLbl.Text = "Feedback period is <b>closed<b>";
                                SubmitLink.Visible = false;
                                ViewAppraisalLbl.Text = "To view own feedback report, click ";
                                ViewAppraisalLink.Visible = true;
                            }
                            else if (result == true)
                            {
                                Session["Submitted"] = true;
                                SubmitAppraisalLbl.Text = "Your feedback has been <b>submitted<b>";
                                SubmitLink.Visible = false;
                                ViewAppraisalLink.Visible = false;
                            }
                            else
                            {
                                Session["Submitted"] = null;
                                SubmitAppraisalLbl.Text = "To start peer feedback, click ";
                                SubmitLink.Visible = true;
                                ViewAppraisalLink.Visible = false;
                            }
                        }
                        else
                        {
                            SubmitLink.Visible = false;
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
            Session["SubmitAppraisalStaffLink"] = true;
            Response.Redirect("~/SubmitAppraisal.aspx");
        }

        protected void ViewAppraisalLink_Click(object sender, EventArgs e)
        {
            //view own appraisal
            Session["OperatorCheck"] = null;
            ViewAppraisalLink.Style.Add("color", "Purple");
            Session["ViewAppraisalStaffLink"] = true;
            Response.Redirect("~/ViewAppraisal.aspx");
        }

        protected void ViewIndividualAllLink_Click(object sender, EventArgs e)
        {
            Session["OperatorCheck"] = true;
            Session["PreviousPageOp"] = null;
            ViewIndividualAllLink.Style.Add("color", "Purple");
            Session["ViewIndividualAllLink"] = true;

            //if (Session["Role"].ToString().Equals("Officer"))
            //{
            //    Response.Redirect("~/ViewAppraisalAllHistory.aspx");
            //}
            //else
            //{
            //    Response.Redirect("~/ViewAppraisal.aspx");
            //}

            if (Session["Role"].ToString() != ("User"))
            {
                Response.Redirect("~/ViewAppraisalAllHistory.aspx");
            }
            else
            {
                Response.Redirect("~/ViewAppraisal.aspx");
            }
        }

        protected void ViewAppraisalChartLink_Click(object sender, EventArgs e)
        {
            Session["OperatorCheck"] = null;
            ViewAppraisalChartLink.Style.Add("color", "Purple");
            Session["ViewAppraisalChartLink"] = true;
            Response.Redirect("~/ViewHistoryChart.aspx");
        }

        protected void ViewGraphReportLink_Click(object sender, EventArgs e)
        {
            Session["OperatorCheck"] = null;
            ViewAppraisalLink.Style.Add("color", "Purple");
            //Session["ViewAppraisalStaffLink"] = true;
            Response.Redirect("~/GraphReports.aspx");
        }
    }
}
