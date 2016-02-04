using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using _360_Staff_Survey_Web.Class;
using System.Collections;
using System.IO;
using System.Globalization;
using System.Text;
using System.Data;
using System.Web.UI.DataVisualization.Charting;

namespace _360_Staff_Survey_Web
{
    public partial class ViewAppraisal : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            if (Session["IndividualBackOp"] != null)
            {
                IndividualBack.Style.Add("color", "Purple");
            }
            if (Session["historylineLinkOp"] != null)
            {
                historylineLink.Style.Add("color", "Purple");
            }
            if (Session["historychartbarLinkOp"] != null)
            {
                historychartbarLink.Style.Add("color", "Purple");
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session["Role"] != null)
                {
                    string role = Session["Role"].ToString();
                    string userid = Session["UserID"].ToString();

                    if (Session["OperatorCheck"] != null)
                    {
                        SearchPanel.Visible = true;
                        staffinfo stf = dbmanager.GetStaffDetailsViaUid(userid);
                        if (stf != null)
                        {
                            if (stf.Section != "ALL")
                            {
                                BindHistoryReportViaSection(stf.Section);
                            }
                            else
                            {
                                BindHistoryReportAll();
                                Session["OperatorAll"] = true;
                            }
                        }
                        BindDropDownList();
                        Session["AverageGrading"] = true;
                        ViewOverallBtn.Text = "View Average Grading";
                        lblSearchIndividual.Text = "View individual report by user name: ";
                        lblFilterBySection.Text = "View individual report by section: ";
                        lblFilterByFunction.Text = "View individual report by function: ";
                    }
                    else
                    {
                        int countapp = dbmanager.GetCountYourAppraisal(userid);
                        if (countapp != 0)
                        {
                            SearchPanel.Visible = false;
                            Session["AverageGrading"] = null;
                            Session["CountApp"] = countapp;
                            BindHistoryReport();
                            ViewOverallBtn.Text = "View School's Grading";
                        }
                        else
                        {
                            Response.Redirect("default.aspx");
                        }
                    }
                }
                else
                {
                    Response.Redirect("accessdenied.aspx");
                }
            }
        }

        protected void BindDropDownList()
        {
            if (Session["ListofSectionOp"] != null && Session["ListofFunctionOp"] != null)
            {
                ArrayList listofsection = (ArrayList)Session["ListofSectionOp"];
                ArrayList listsection = RemoveDups(listofsection);
                ArrayList listoffunction = (ArrayList)Session["ListofFunctionOp"];
                ArrayList listfunction = RemoveDups(listoffunction);


                if (listsection.Count != 0)
                {
                    listsection.Insert(0, "<----Please select one---->");
                    listsection.Insert(1, "<-----------Default----------->");
                    listsection.Insert(2, "All section");
                    ddlFilterSection.DataSource = listsection;
                    ddlFilterSection.DataBind();
                }
                if (listfunction.Count != 0)
                {
                    listfunction.Insert(0, "<----Please select one---->");
                    listfunction.Insert(1, "<-----------Default----------->");
                    listfunction.Insert(2, "All function");
                    ddlFilterFunction.DataSource = listfunction;
                    ddlFilterFunction.DataBind();
                }
            }
        }

        protected void exportWord_Click(object sender, ImageClickEventArgs e)
        {
            ExportWord(ViewAllHistory);
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }

        private void MessageBoxShow(string message)
        {
            string strScript = "<script>";
            strScript += "alert('" + message + "');";
            strScript += "</script>";
            this.ClientScript.RegisterStartupScript(this.GetType(), "Startup", strScript);
        }

        private void MessageBoxShowWithRedirect(string message)
        {
            string strScript = "<script>";
            strScript += "alert('" + message + "');";
            strScript += "window.location='ViewAppraisal.aspx';";
            strScript += "</script>";
            this.ClientScript.RegisterStartupScript(this.GetType(), "Startup", strScript);
        }

        protected void ExportExcel(GridView gridView)
        {
            /*string username = Session["Name"].ToString();
            if (username.Contains(" "))
            {
                username = username.Replace(" ", "_");
            }**/

            HttpContext.Current.Response.Clear();
            System.Web.HttpContext curContext = System.Web.HttpContext.Current;
            curContext.Response.AddHeader("content-disposition", "attachment; filename=" + System.Web.HttpUtility.UrlEncode("StaffAppraisal", System.Text.Encoding.UTF8) + ".xls");
            curContext.Response.ContentType = "application/vnd.ms-excel";
            curContext.Response.Write("<meta http-equiv=Content-Type content=text/html;charset=UTF-8>");

            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";

            //  Create a table to contain the grid
            Table table = new Table();

            //  include the gridline settings
            table.GridLines = gridView.GridLines;

            //  add the header row to the table
            if (gridView.HeaderRow != null)
            {
                PrepareControlForExport(gridView.HeaderRow);
                table.Rows.Add(gridView.HeaderRow);
            }

            //  add each of the data rows to the table

            foreach (GridViewRow row in gridView.Rows)
            {
                PrepareControlForExport(row);
                table.Rows.Add(row);
            }

            //  add the footer row to the table
            if (gridView.FooterRow != null)
            {
                PrepareControlForExport(gridView.FooterRow);
                table.Rows.Add(gridView.FooterRow);
            }

            for (int i = 0; i <= gridView.Rows.Count; i++)
            {
                table.Rows[i].Cells[0].Visible = false;
            }

            using (StringWriter stringWriter = new StringWriter())
            {
                using (HtmlTextWriter htmlWriter = new HtmlTextWriter(stringWriter))
                {
                    //  render the table into the htmlwriter
                    table.RenderControl(htmlWriter);

                    //  render the htmlwriter into the response
                    curContext.Response.Write(stringWriter.ToString());
                    curContext.Response.End();
                }
            }
        }

        protected void ExportWord(GridView gridView)
        {
            /*string username = Session["Name"].ToString();
            if (username.Contains(" "))
            {
                username = username.Replace(" ", "_");
            }**/

            HttpContext.Current.Response.Clear();
            System.Web.HttpContext curContext = System.Web.HttpContext.Current;
            curContext.Response.AddHeader("content-disposition", "attachment; filename=" + System.Web.HttpUtility.UrlEncode("StaffAppraisal.doc"));

            HttpContext.Current.Response.ContentType = "application/ms-word";

            //  Create a table to contain the grid
            Table table = new Table();

            //  include the gridline settings
            table.GridLines = gridView.GridLines;

            //  add the header row to the table
            if (gridView.HeaderRow != null)
            {
                PrepareControlForExport(gridView.HeaderRow);
                gridView.HeaderRow.Style.Add("background", "black");
                table.Rows.Add(gridView.HeaderRow);
            }

            //  add each of the data rows to the table

            foreach (GridViewRow row in gridView.Rows)
            {
                PrepareControlForExport(row);
                table.Rows.Add(row);
            }

            //  add the footer row to the table
            if (gridView.FooterRow != null)
            {
                PrepareControlForExport(gridView.FooterRow);
                table.Rows.Add(gridView.FooterRow);
            }

            for (int i = 0; i <= gridView.Rows.Count; i++)
            {
                table.Rows[i].Cells[0].Visible = false;
            }

            using (StringWriter stringWriter = new StringWriter())
            {
                using (HtmlTextWriter htmlWriter = new HtmlTextWriter(stringWriter))
                {
                    //  render the table into the htmlwriter
                    table.RenderControl(htmlWriter);

                    //  render the htmlwriter into the response
                    curContext.Response.Write(stringWriter.ToString());
                    curContext.Response.End();
                }
            }
        }

        protected void BindHistoryReport()
        {
            ArrayList listofavgrating = new ArrayList();
            ArrayList listofdouble = new ArrayList();
            ArrayList listofquestion = dbmanager.GetAllQuestion();
            ArrayList listofstaff = dbmanager.GetAllStaffDetails();
            DataTable dt = new DataTable();
            DataRow dr = null;
            int count = 0;
            string uid = Session["UserID"].ToString();
            staffinfo stf = dbmanager.GetStaffDetailsViaUid(uid);

            ArrayList listofhistorydates = dbmanager.GetTotalHistoryDates(stf.Uid);
            if (count == 0)
            {
                dt.Columns.Add(new DataColumn("UserID", typeof(string)));
                dt.Columns.Add(new DataColumn("Name", typeof(string)));
                dt.Columns.Add(new DataColumn("Section", typeof(string)));
                dt.Columns.Add(new DataColumn("Function", typeof(string)));
                count++;
            }

            if (listofhistorydates.Count != 0 && listofquestion.Count != 0 && count > 0)
            {
                dr = dt.NewRow();
                dr["UserID"] = stf.Uid;
                dr["Name"] = stf.Name;
                dr["Section"] = stf.Section;
                dr["Function"] = stf.Function;
                dt.Rows.Add(dr);
            }

            if (count > 0)
            {
                ViewState["HistoryGrid"] = dt;
                Session["ListofQuestion"] = listofquestion;
                ViewAllHistory.DataSource = dt;
                ViewAllHistory.DataBind();
                BindInsideGradeGrid();
                LegendMessage();
                MultiView1.ActiveViewIndex = 0;
            }
            else
            {
                Response.Redirect("default.aspx");
            }
        }

        protected void BindHistoryReportAll()
        {
            ArrayList listofsection = new ArrayList();
            ArrayList listoffunction = new ArrayList();
            ArrayList listofavgrating = new ArrayList();
            ArrayList listofdouble = new ArrayList();
            ArrayList listofquestion = dbmanager.GetAllQuestion();
            ArrayList listofstaff = dbmanager.GetAllStaffDetails();
            DataTable dt = new DataTable();
            DataRow dr = null;
            int count = 0;

            foreach (staffinfo stf in listofstaff)
            {
                ArrayList listofhistorydates = dbmanager.GetTotalHistoryDates(stf.Uid);
                if (count == 0)
                {
                    dt.Columns.Add(new DataColumn("UserID", typeof(string)));
                    dt.Columns.Add(new DataColumn("Name", typeof(string)));
                    dt.Columns.Add(new DataColumn("Section", typeof(string)));
                    dt.Columns.Add(new DataColumn("Function", typeof(string)));
                    count++;
                }

                if (listofhistorydates.Count != 0 && listofquestion.Count != 0 && count > 0 && stf.Section != "ALL")
                {
                    dr = dt.NewRow();
                    dr["UserID"] = stf.Uid;
                    dr["Name"] = stf.Name;
                    dr["Section"] = stf.Section;
                    string[] stsec = stf.Section.Split(',');
                    if (stsec.LongLength > 0)
                    {
                        foreach (string sec in stsec)
                        {
                            listofsection.Add(sec);
                        }
                    }
                    dr["Function"] = stf.Function;
                    listoffunction.Add(stf.Function);
                    dt.Rows.Add(dr);
                }
            }
            if (count > 0)
            {
                ViewState["HistoryGrid"] = dt;
                Session["ListofQuestion"] = listofquestion;
                Session["ListofSectionOp"] = RemoveDups(listofsection);
                Session["ListofFunctionOp"] = listoffunction;
                ViewAllHistory.DataSource = dt;
                ViewAllHistory.DataBind();
                BindInsideGradeGrid();
                LegendMessage();
                MultiView1.ActiveViewIndex = 0;
            }
            else
            {
                Response.Redirect("default.aspx");
            }
        }

        protected void BindHistoryReportViaSectionIndividual(string section)
        {
            ArrayList listofavgrating = new ArrayList();
            ArrayList listofdouble = new ArrayList();
            ArrayList listofquestion = dbmanager.GetAllQuestion();
            ArrayList listofstaff = dbmanager.GetAllStaffDetails();
            DataTable dt = new DataTable();
            DataRow dr = null;
            int count = 0;
            int functcount = 0;

            foreach (staffinfo stf in listofstaff)
            {
                ArrayList listofhistorydates = dbmanager.GetTotalHistoryDates(stf.Uid);
                if (count == 0)
                {
                    dt.Columns.Add(new DataColumn("UserID", typeof(string)));
                    dt.Columns.Add(new DataColumn("Name", typeof(string)));
                    dt.Columns.Add(new DataColumn("Section", typeof(string)));
                    dt.Columns.Add(new DataColumn("Function", typeof(string)));
                    count++;
                }

                if (listofhistorydates.Count != 0 && listofquestion.Count != 0 && count > 0 && stf.Section.Contains(section))
                {
                    string[] listofsection = stf.Section.Split(',');
                    if (listofsection.LongLength > 0)
                    {
                        foreach (string s in listofsection)
                        {
                            if (s.Equals(section))
                            {
                                dr = dt.NewRow();
                                dr["UserID"] = stf.Uid;
                                dr["Name"] = stf.Name;
                                dr["Section"] = stf.Section;
                                dr["Function"] = stf.Function;
                                dt.Rows.Add(dr);
                                functcount++;
                            }
                        }
                    }
                }
            }
            if (count > 0 && functcount > 0)
            {
                ViewState["HistoryGrid"] = dt;
                Session["ListofQuestion"] = listofquestion;
                ViewAllHistory.DataSource = dt;
                ViewAllHistory.DataBind();
                BindInsideGradeGrid();
                LegendMessage();
                MultiView1.ActiveViewIndex = 0;
            }
            else
            {
                MessageBoxShow("No record found for this section.");
            }
        }

        protected void BindHistoryReportViaSection(string section)
        {
            string[] finalsec = section.Split(',');
            ArrayList listofsection = new ArrayList();
            ArrayList listoffunction = new ArrayList();
            ArrayList listofavgrating = new ArrayList();
            ArrayList listofdouble = new ArrayList();
            ArrayList listofquestion = dbmanager.GetAllQuestion();
            ArrayList listofstaff = dbmanager.GetAllStaffDetails();
            DataTable dt = new DataTable();
            DataRow dr = null;
            int count = 0;
            int sectcount = 0;
            foreach (string sec in finalsec)
            {
                foreach (staffinfo stf in listofstaff)
                {
                    ArrayList listofhistorydates = dbmanager.GetTotalHistoryDates(stf.Uid);
                    if (count == 0 && listofhistorydates.Count > 0)
                    {
                        dt.Columns.Add(new DataColumn("UserID", typeof(string)));
                        dt.Columns.Add(new DataColumn("Name", typeof(string)));
                        dt.Columns.Add(new DataColumn("Section", typeof(string)));
                        dt.Columns.Add(new DataColumn("Function", typeof(string)));
                        count++;
                    }
                    if (listofhistorydates.Count != 0 && listofquestion.Count != 0 && count > 0 && stf.Section == sec.Trim())
                    {
                        dr = dt.NewRow();
                        dr["UserID"] = stf.Uid;
                        dr["Name"] = stf.Name;
                        dr["Section"] = stf.Section;
                        listofsection.Add(stf.Section);
                        dr["Function"] = stf.Function;
                        listoffunction.Add(stf.Function);
                        dt.Rows.Add(dr);
                        sectcount++;
                    }
                }
            }
            if (count > 0 && sectcount > 0)
            {
                ViewState["HistoryGrid"] = dt;
                Session["ListofQuestion"] = listofquestion;
                Session["ListofSectionOp"] = listofsection;
                Session["ListofFunctionOp"] = listoffunction;
                ViewAllHistory.DataSource = dt;
                ViewAllHistory.DataBind();
                BindInsideGradeGrid();
                LegendMessage();
                MultiView1.ActiveViewIndex = 0;
            }
            else
            {
                MessageBoxShow("No record found for this section.");
            }
        }

        protected void BindHistoryReportViaUserID(string name, string sec)
        {
            ArrayList listofsection = (ArrayList)Session["ListofSectionOp"];
            ArrayList filterlist = RemoveDups(listofsection);
            ArrayList listofstafflikename = dbmanager.GetAllUserIDViaLikeNameSection(name, filterlist);
            ArrayList listofavgrating = new ArrayList();
            ArrayList listofdouble = new ArrayList();
            ArrayList listofquestion = dbmanager.GetAllQuestion();
            ArrayList listofstaff = dbmanager.GetAllStaffDetails();
            DataTable dt = new DataTable();
            DataRow dr = null;
            int count = 0;

            if (listofstafflikename.Count != 0)
            {
                foreach (staffinfo stf in listofstafflikename)
                {
                    ArrayList listofhistorydates = dbmanager.GetTotalHistoryDates(stf.Uid);
                    if (count == 0 && listofhistorydates.Count > 0)
                    {
                        dt.Columns.Add(new DataColumn("UserID", typeof(string)));
                        dt.Columns.Add(new DataColumn("Name", typeof(string)));
                        dt.Columns.Add(new DataColumn("Section", typeof(string)));
                        dt.Columns.Add(new DataColumn("Function", typeof(string)));
                        count++;
                    }

                    if (listofhistorydates.Count != 0 && listofquestion.Count != 0 && count > 0)
                    {
                        dr = dt.NewRow();
                        dr["UserID"] = stf.Uid;
                        dr["Name"] = stf.Name;
                        dr["Section"] = stf.Section;
                        dr["Function"] = stf.Function;
                        dt.Rows.Add(dr);
                    }
                }
                if (count > 0)
                {
                    ViewState["HistoryGrid"] = dt;
                    Session["ListofQuestion"] = listofquestion;
                    ViewAllHistory.DataSource = dt;
                    ViewAllHistory.DataBind();
                    BindInsideGradeGrid();
                    LegendMessage();
                    MultiView1.ActiveViewIndex = 0;
                }
                else
                {
                    MessageBoxShow("No record found for this user.");
                }
            }
            else
            {
                MessageBoxShow("No record found for this user within your section.");
            }

        }

        protected void BindHistoryReportViaFunction(string function)
        {
            ArrayList listofavgrating = new ArrayList();
            ArrayList listofdouble = new ArrayList();
            ArrayList listofquestion = dbmanager.GetAllQuestion();
            ArrayList listofstaff = dbmanager.GetAllStaffDetails();
            DataTable dt = new DataTable();
            DataRow dr = null;
            int count = 0;
            int functcount = 0;

            foreach (staffinfo stf in listofstaff)
            {
                ArrayList listofhistorydates = dbmanager.GetTotalHistoryDates(stf.Uid);
                if (count == 0)
                {
                    dt.Columns.Add(new DataColumn("UserID", typeof(string)));
                    dt.Columns.Add(new DataColumn("Name", typeof(string)));
                    dt.Columns.Add(new DataColumn("Section", typeof(string)));
                    dt.Columns.Add(new DataColumn("Function", typeof(string)));
                    count++;
                }

                if (listofhistorydates.Count != 0 && listofquestion.Count != 0 && count > 0 && stf.Function == function)
                {
                    dr = dt.NewRow();
                    dr["UserID"] = stf.Uid;
                    dr["Name"] = stf.Name;
                    dr["Section"] = stf.Section;
                    dr["Function"] = stf.Function;
                    dt.Rows.Add(dr);
                    functcount++;
                }
            }
            if (count > 0 && functcount > 0)
            {
                ViewState["HistoryGrid"] = dt;
                Session["ListofQuestion"] = listofquestion;
                ViewAllHistory.DataSource = dt;
                ViewAllHistory.DataBind();
                BindInsideGradeGrid();
                LegendMessage();
                MultiView1.ActiveViewIndex = 0;
            }
            else
            {
                MessageBoxShow("No record found for this function.");
            }
        }

        protected void BindHistoryReportViaSectionAndFunction(string section, string function)
        {
            ArrayList listofavgrating = new ArrayList();
            ArrayList listofdouble = new ArrayList();
            ArrayList listofquestion = dbmanager.GetAllQuestion();
            ArrayList listofstaff = dbmanager.GetAllStaffDetails();
            DataTable dt = new DataTable();
            DataRow dr = null;
            int count = 0;
            int functcount = 0;

            foreach (staffinfo stf in listofstaff)
            {
                ArrayList listofhistorydates = dbmanager.GetTotalHistoryDates(stf.Uid);
                if (count == 0)
                {
                    dt.Columns.Add(new DataColumn("UserID", typeof(string)));
                    dt.Columns.Add(new DataColumn("Name", typeof(string)));
                    dt.Columns.Add(new DataColumn("Section", typeof(string)));
                    dt.Columns.Add(new DataColumn("Function", typeof(string)));
                    count++;
                }

                if (listofhistorydates.Count != 0 && listofquestion.Count != 0 && count > 0 && stf.Section.Contains(section) && stf.Function == function)
                {
                    string[] listofsection = stf.Section.Split(',');
                    if (listofsection.LongLength > 0)
                    {
                        foreach (string s in listofsection)
                        {
                            if (s.Equals(section))
                            {
                                dr = dt.NewRow();
                                dr["UserID"] = stf.Uid;
                                dr["Name"] = stf.Name;
                                dr["Section"] = stf.Section;
                                dr["Function"] = stf.Function;
                                dt.Rows.Add(dr);
                                functcount++;
                            }
                        }
                    }
                }
            }

            if (count > 0 && functcount > 0)
            {
                ViewState["HistoryGrid"] = dt;
                Session["ListofQuestion"] = listofquestion;
                ViewAllHistory.DataSource = dt;
                ViewAllHistory.DataBind();
                BindInsideGradeGrid();
                LegendMessage();
                MultiView1.ActiveViewIndex = 0;
            }
            else
            {
                MessageBoxShow("No record found for this combination.");
            }
        }

        protected void BindHistoryReportViaSectionAndFunctionWithRegion(string section, string function)
        {
            ArrayList listofavgrating = new ArrayList();
            ArrayList listofdouble = new ArrayList();
            ArrayList listofquestion = dbmanager.GetAllQuestion();
            ArrayList listofstaff = dbmanager.GetAllStaffDetails();
            DataTable dt = new DataTable();
            DataRow dr = null;
            int count = 0;
            int functcount = 0;

            foreach (staffinfo stf in listofstaff)
            {
                ArrayList listofhistorydates = dbmanager.GetTotalHistoryDates(stf.Uid);
                if (count == 0)
                {
                    dt.Columns.Add(new DataColumn("UserID", typeof(string)));
                    dt.Columns.Add(new DataColumn("Name", typeof(string)));
                    dt.Columns.Add(new DataColumn("Section", typeof(string)));
                    dt.Columns.Add(new DataColumn("Function", typeof(string)));
                    count++;
                }

                if (listofhistorydates.Count != 0 && listofquestion.Count != 0 && count > 0 && stf.Function == function)
                {
                    string[] listofsection = stf.Section.Split(',');
                    if (listofsection.LongLength > 0)
                    {
                        foreach (string s in listofsection)
                        {
                            if (section.Contains(s))
                            {
                                dr = dt.NewRow();
                                dr["UserID"] = stf.Uid;
                                dr["Name"] = stf.Name;
                                dr["Section"] = stf.Section;
                                dr["Function"] = stf.Function;
                                dt.Rows.Add(dr);
                                functcount++;
                            }
                        }
                    }
                }
            }

            if (count > 0 && functcount > 0)
            {
                ViewState["HistoryGrid"] = dt;
                Session["ListofQuestion"] = listofquestion;
                ViewAllHistory.DataSource = dt;
                ViewAllHistory.DataBind();
                BindInsideGradeGrid();
                LegendMessage();
                MultiView1.ActiveViewIndex = 0;
            }
            else
            {
                MessageBoxShow("No report found for this combination.");
            }
        }

        protected void BindInsideGradeGrid()
        {
            ArrayList listofquestion = (ArrayList)Session["ListofQuestion"];
            DataTable maintable = (DataTable)ViewState["HistoryGrid"];
            int indexgrid = 0;

            foreach (DataRow row in maintable.Rows)
            {
                DataTable dt2 = new DataTable();
                DataRow dr2 = null;
                string userid = row["UserID"].ToString();
                ArrayList listofhistorydates = dbmanager.GetTotalHistoryDates(userid);

                for (int i = 0; i < listofhistorydates.Count; i++)
                {
                    DateTime toshortdate = ((DateTime)listofhistorydates[i]);

                    string monthname = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(toshortdate.Month);
                    dt2.Columns.Add(new DataColumn(monthname.Substring(0, 3) + "/" + toshortdate.Year.ToString().Substring(2, 2), typeof(double)));
                }

                int index = 0;
                double result = 0.0;
                dr2 = dt2.NewRow();
                foreach (DateTime date in listofhistorydates)
                {
                    foreach (Question qn in listofquestion)
                    {
                        result += dbmanager.GetAvgRating(userid, date, qn.QuestionID);
                    }
                    int countquestion = dbmanager.GetTotalCountQuestionInPeriod(userid, date);
                    result = Math.Round((result / countquestion), 1);
                    DateTime toshortdate = ((DateTime)listofhistorydates[index]);

                    string monthname = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(toshortdate.Month);
                    dr2[monthname.Substring(0, 3) + "/" + toshortdate.Year.ToString().Substring(2, 2)] = result;
                    index++;
                    result = 0.0;
                }
                dt2.Rows.Add(dr2);
                GridView gv = (GridView)ViewAllHistory.Rows[indexgrid].FindControl("GridView1");
                gv.DataSource = dt2;
                gv.DataBind();
                indexgrid++;
            }
        }

        protected void BindOverAllSchoolGrid()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            dt.Columns.Add(new DataColumn("Name", typeof(string)));

            ArrayList listofdates = dbmanager.GetListofDatesViaAll();

            if (listofdates.Count > 0)
            {
                dr = dt.NewRow();
                dr["Name"] = "ASC School";
                dt.Rows.Add(dr);
            }
            else
            {
                MessageBoxShowWithRedirect("No record found.");
            }
            ViewState["OverSchoolGrid"] = dt;
            ViewOverAllSchoolGrid.DataSource = dt;
            ViewOverAllSchoolGrid.DataBind();
            BindOverAllInsideGrid();
        }

        protected void BindOverAllSchoolGridByFunction(string function)
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            dt.Columns.Add(new DataColumn("Name", typeof(string)));

            ArrayList listofdates = dbmanager.GetListofDatesViaAllByFunction(function);

            if (listofdates.Count > 0)
            {
                dr = dt.NewRow();
                dr["Name"] = "ASC School";
                dt.Rows.Add(dr);
            }
            else
            {
                MessageBoxShowWithRedirect("No record found.");
            }
            ViewState["OverSchoolGrid"] = dt;
            ViewOverAllSchoolGrid.DataSource = dt;
            ViewOverAllSchoolGrid.DataBind();
            BindOverAllInsideGridByFunction(function);
        }

        protected void BindOverAllSchoolGridBySection(string section, string function)
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            dt.Columns.Add(new DataColumn("Name", typeof(string)));

            ArrayList listofdates = dbmanager.GetListofDatesViaAllBySection(section);

            if (listofdates.Count > 0)
            {
                dr = dt.NewRow();
                dr["Name"] = "ASC School";
                dt.Rows.Add(dr);
            }
            else
            {
                MessageBoxShowWithRedirect("No record found.");
            }
            ViewState["OverSchoolGrid"] = dt;
            ViewOverAllSchoolGrid.DataSource = dt;
            ViewOverAllSchoolGrid.DataBind();
            BindOverAllInsideGridBySection(section, function);
        }

        protected void BindOverAllSchoolGridByBothSectionFunction(string section, string function)
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            dt.Columns.Add(new DataColumn("Name", typeof(string)));

            ArrayList listofdates = dbmanager.GetListofDatesViaAllBySectionFunction(section, function);

            if (listofdates.Count > 0)
            {
                dr = dt.NewRow();
                dr["Name"] = "ASC School";
                dt.Rows.Add(dr);
            }
            else
            {
                MessageBoxShowWithRedirect("No record found.");
            }
            ViewState["OverSchoolGrid"] = dt;
            ViewOverAllSchoolGrid.DataSource = dt;
            ViewOverAllSchoolGrid.DataBind();
            BindOverAllInsideGridBySection(section, function);
        }

        protected void BindOverAllSchoolGridByLIKEName(string name)
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            dt.Columns.Add(new DataColumn("Name", typeof(string)));

            ArrayList listofdates = dbmanager.GetListofDatesViaAllLIKEName(name);

            if (listofdates.Count > 0)
            {
                dr = dt.NewRow();
                dr["Name"] = "ASC School";
                dt.Rows.Add(dr);
            }
            else
            {
                MessageBoxShowWithRedirect("No record found.");
            }
            ViewState["OverSchoolGrid"] = dt;
            ViewOverAllSchoolGrid.DataSource = dt;
            ViewOverAllSchoolGrid.DataBind();
            BindOverAllInsideGridByLIKEName(name);
        }

        protected void BindOverAllInsideGrid()
        {
            if (ViewOverAllSchoolGrid.Rows.Count > 0)
            {
                DataTable overallschool = (DataTable)ViewState["OverSchoolGrid"];
                foreach (DataRow row in overallschool.Rows)
                {
                    ArrayList listofdates = dbmanager.GetListofDatesViaAll();
                    DataTable dt2 = new DataTable();
                    DataRow dr2 = null;

                    for (int i = 0; i < listofdates.Count; i++)
                    {
                        DateTime toshortdate = ((DateTime)listofdates[i]);

                        string monthname = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(toshortdate.Month);
                        dt2.Columns.Add(new DataColumn(monthname.Substring(0, 3) + "/" + toshortdate.Year.ToString().Substring(2, 2), typeof(double)));
                    }

                    int index = 0;
                    double result = 0.0;
                    dr2 = dt2.NewRow();
                    foreach (DateTime date in listofdates)
                    {
                        result = dbmanager.GetAvgRatingAllSchool(date);
                        result = Math.Round((result), 1);
                        DateTime toshortdate = ((DateTime)listofdates[index]);

                        string monthname = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(toshortdate.Month);
                        dr2[monthname.Substring(0, 3) + "/" + toshortdate.Year.ToString().Substring(2, 2)] = result;
                        index++;
                        result = 0.0;
                    }
                    dt2.Rows.Add(dr2);
                    GridView gv = (GridView)ViewOverAllSchoolGrid.Rows[0].FindControl("InsideGridOverAll");
                    gv.DataSource = dt2;
                    gv.DataBind();
                }
            }
        }

        protected void BindOverAllInsideGridByFunction(string function)
        {
            if (ViewOverAllSchoolGrid.Rows.Count > 0)
            {
                DataTable overallschool = (DataTable)ViewState["OverSchoolGrid"];
                foreach (DataRow row in overallschool.Rows)
                {
                    ArrayList listofdates = dbmanager.GetListofDatesViaAllByFunction(function);
                    DataTable dt2 = new DataTable();
                    DataRow dr2 = null;

                    for (int i = 0; i < listofdates.Count; i++)
                    {
                        DateTime toshortdate = ((DateTime)listofdates[i]);

                        string monthname = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(toshortdate.Month);
                        dt2.Columns.Add(new DataColumn(monthname.Substring(0, 3) + "/" + toshortdate.Year.ToString().Substring(2, 2), typeof(double)));
                    }

                    int index = 0;
                    double result = 0.0;
                    dr2 = dt2.NewRow();
                    foreach (DateTime date in listofdates)
                    {
                        result = dbmanager.GetAvgRatingByFunction(function, date);
                        result = Math.Round((result), 1);
                        DateTime toshortdate = ((DateTime)listofdates[index]);

                        string monthname = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(toshortdate.Month);
                        dr2[monthname.Substring(0, 3) + "/" + toshortdate.Year.ToString().Substring(2, 2)] = result;
                        index++;
                        result = 0.0;
                    }
                    dt2.Rows.Add(dr2);
                    GridView gv = (GridView)ViewOverAllSchoolGrid.Rows[0].FindControl("InsideGridOverAll");
                    gv.DataSource = dt2;
                    gv.DataBind();
                }
            }
        }

        protected void BindOverAllInsideGridBySection(string section, string function)
        {
            if (ViewOverAllSchoolGrid.Rows.Count > 0)
            {
                DataTable overallschool = (DataTable)ViewState["OverSchoolGrid"];
                foreach (DataRow row in overallschool.Rows)
                {
                    ArrayList listofdates = dbmanager.GetListofDatesViaAllBySection(section);
                    DataTable dt2 = new DataTable();
                    DataRow dr2 = null;

                    for (int i = 0; i < listofdates.Count; i++)
                    {
                        DateTime toshortdate = ((DateTime)listofdates[i]);

                        string monthname = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(toshortdate.Month);
                        dt2.Columns.Add(new DataColumn(monthname.Substring(0, 3) + "/" + toshortdate.Year.ToString().Substring(2, 2), typeof(double)));
                    }

                    int index = 0;
                    double result = 0.0;
                    dr2 = dt2.NewRow();
                    foreach (DateTime date in listofdates)
                    {
                        result = dbmanager.GetAvgRatingBySection(section, date, function);
                        result = Math.Round((result), 1);
                        DateTime toshortdate = ((DateTime)listofdates[index]);

                        string monthname = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(toshortdate.Month);
                        dr2[monthname.Substring(0, 3) + "/" + toshortdate.Year.ToString().Substring(2, 2)] = result;
                        index++;
                        result = 0.0;
                    }
                    dt2.Rows.Add(dr2);
                    GridView gv = (GridView)ViewOverAllSchoolGrid.Rows[0].FindControl("InsideGridOverAll");
                    gv.DataSource = dt2;
                    gv.DataBind();
                }
            }
        }

        protected void BindOverAllInsideGridBySectionFunction(string section, string function)
        {
            if (ViewOverAllSchoolGrid.Rows.Count > 0)
            {
                DataTable overallschool = (DataTable)ViewState["OverSchoolGrid"];
                foreach (DataRow row in overallschool.Rows)
                {
                    ArrayList listofdates = dbmanager.GetListofDatesViaAllBySectionFunction(section, function);
                    DataTable dt2 = new DataTable();
                    DataRow dr2 = null;

                    for (int i = 0; i < listofdates.Count; i++)
                    {
                        DateTime toshortdate = ((DateTime)listofdates[i]);

                        string monthname = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(toshortdate.Month);
                        dt2.Columns.Add(new DataColumn(monthname.Substring(0, 3) + "/" + toshortdate.Year.ToString().Substring(2, 2), typeof(double)));
                    }

                    int index = 0;
                    double result = 0.0;
                    dr2 = dt2.NewRow();
                    foreach (DateTime date in listofdates)
                    {
                        result = dbmanager.GetAvgRatingBySectionFunction(section, function, date);
                        result = Math.Round((result), 1);
                        DateTime toshortdate = ((DateTime)listofdates[index]);

                        string monthname = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(toshortdate.Month);
                        dr2[monthname.Substring(0, 3) + "/" + toshortdate.Year.ToString().Substring(2, 2)] = result;
                        index++;
                        result = 0.0;
                    }
                    dt2.Rows.Add(dr2);
                    GridView gv = (GridView)ViewOverAllSchoolGrid.Rows[0].FindControl("InsideGridOverAll");
                    gv.DataSource = dt2;
                    gv.DataBind();
                }
            }
        }

        protected void BindOverAllInsideGridByLIKEName(string name)
        {
            if (ViewOverAllSchoolGrid.Rows.Count > 0)
            {
                DataTable overallschool = (DataTable)ViewState["OverSchoolGrid"];
                foreach (DataRow row in overallschool.Rows)
                {
                    ArrayList listofdates = dbmanager.GetListofDatesViaAllLIKEName(name);
                    DataTable dt2 = new DataTable();
                    DataRow dr2 = null;

                    for (int i = 0; i < listofdates.Count; i++)
                    {
                        DateTime toshortdate = ((DateTime)listofdates[i]);

                        string monthname = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(toshortdate.Month);
                        dt2.Columns.Add(new DataColumn(monthname.Substring(0, 3) + "/" + toshortdate.Year.ToString().Substring(2, 2), typeof(double)));
                    }

                    int index = 0;
                    double result = 0.0;
                    dr2 = dt2.NewRow();
                    foreach (DateTime date in listofdates)
                    {
                        result = dbmanager.GetAvgRatingByLIKEName(name, date);
                        result = Math.Round((result), 1);
                        DateTime toshortdate = ((DateTime)listofdates[index]);

                        string monthname = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(toshortdate.Month);
                        dr2[monthname.Substring(0, 3) + "/" + toshortdate.Year.ToString().Substring(2, 2)] = result;
                        index++;
                        result = 0.0;
                    }
                    dt2.Rows.Add(dr2);
                    GridView gv = (GridView)ViewOverAllSchoolGrid.Rows[0].FindControl("InsideGridOverAll");
                    gv.DataSource = dt2;
                    gv.DataBind();
                }
            }
        }
        //others

        protected void LegendMessage()
        {
            string legend = "";
            legend += "<table align='left'><tr><td>";
            legend += "<b>Legend on Viewing Overall Feedback</b><br><br>";
            legend += "Grading by date refers to overall final grading for the feedback period.<br><br>";
            legend += "Select 'View' at left corner of each row for individual feedback grading and comments.<br><br>";
            legend += "Please note that the format of date shown is: MONTH/YEAR.<br>";
            legend += "</td></tr></table>";
            lbllegendHistory.Text = legend;
        }

        protected void SearchIndividualBtn_Click(object sender, EventArgs e)
        {
            Session["SearchClickName"] = null;
            if (SearchTbx.Text.Length != 0)
            {
                string uid = Session["UserID"].ToString();
                staffinfo stf = dbmanager.GetStaffDetailsViaUid(uid);
                if (stf != null)
                {
                    BindHistoryReportViaUserID(SearchTbx.Text.Trim(), stf.Section);
                    Session["SearchClickName"] = true;
                }
            }
        }

        public static ArrayList listofquestion = new ArrayList();

        protected void ViewBtn_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            int i = Convert.ToInt32(row.RowIndex);

            string uid = ViewAllHistory.Rows[i].Cells[1].Text;
            Session["IndividualOp"] = uid;
            Session["PreviousPageOp"] = System.IO.Path.GetFileName(System.Web.HttpContext.Current.Request.FilePath);
            SearchPanel.Visible = false;
            ArrayList listofhistorydates = dbmanager.GetTotalHistoryDates(uid);
            ArrayList listofdates = new ArrayList();
            foreach (DateTime date in listofhistorydates)
            {
                string monthname = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(date.Month);
                listofdates.Add(monthname + ", " + date.Year);
            }

            ddlOverDates.DataSource = listofdates;
            ddlOverDates.DataBind();

            DateTime select = Convert.ToDateTime(listofdates[listofdates.Count - 1].ToString());
            DateTime getlikedate = dbmanager.CheckDateTimeLike(select);
            ddlOverDates.SelectedIndex = listofdates.Count - 1;
            string name = dbmanager.GetNameViaUserID(uid);

            // populate the question ddl
            ArrayList listofQuestionIDs = dbmanager.GetAllQuestionListViaPerson(getlikedate, uid);

            if (listofQuestionIDs.Count != 0)
            {
                for (int j = 0; j < listofQuestionIDs.Count; j++)
                {
                    int id = Convert.ToInt32(listofQuestionIDs[j].ToString());
                    string questionDetails = dbmanager.GetQuestionViaQuestionId(id);
                    listofquestion.Add(questionDetails);
                }
            }

            ArrayList removeDupListQuestion = new ArrayList();
            removeDupListQuestion = RemoveDups(listofquestion);

            if (removeDupListQuestion.Count > 1)
            {
                removeDupListQuestion.Insert(0, "<--------All Question(s)------->");
                ddlFilterQuestion.DataSource = removeDupListQuestion;
                ddlFilterQuestion.DataBind();
            }
            else
            {
                ddlFilterQuestion.DataSource = removeDupListQuestion;
                ddlFilterQuestion.DataBind();

            }

            Session["dupQuestion"] = (ArrayList)removeDupListQuestion;
            //last
            ViewIndividualBothRateAndComment(uid, getlikedate, 0, "");
            MultiView1.ActiveViewIndex = 1;



        }

        protected void ddlOverDates_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblYourStat.Text = "";
            if (Session["IndividualOp"] != null)
            {
                string uid = Session["IndividualOp"].ToString();
                DateTime select = Convert.ToDateTime(ddlOverDates.Text);
                DateTime getdatelike = dbmanager.CheckDateTimeLike(select);

                listofquestion = new ArrayList();
                int questionID = ddlFilterQuestion.SelectedIndex;
                string questionDetail = ddlFilterQuestion.SelectedValue;

                ArrayList listofQuestionIDs = dbmanager.GetAllQuestionListViaPerson(getdatelike, uid);

                if (listofQuestionIDs.Count != 0)
                {
                    for (int i = 0; i < listofQuestionIDs.Count; i++)
                    {
                        int id = Convert.ToInt32(listofQuestionIDs[i].ToString());
                        string questionDetails = dbmanager.GetQuestionViaQuestionId(id);
                        listofquestion.Add(questionDetails);
                    }
                }

                ArrayList listQuestion = RemoveDups(listofquestion);

                if (listQuestion.Count > 1)
                {
                    listQuestion.Insert(0, "<--------All Question(s)------->");
                    ddlFilterQuestion.DataSource = listQuestion;
                    ddlFilterQuestion.DataBind();
                }
                else
                {
                    ddlFilterQuestion.DataSource = listQuestion;
                    ddlFilterQuestion.DataBind();
                }

                Session["dupQuestion"] = (ArrayList)listQuestion;

                if (ddlFilterOutPut.SelectedIndex == 0)
                {
                    ViewIndividualBothRateAndComment(uid, getdatelike, questionID, questionDetail);
                }
                else
                {
                    ViewIndividualComment(uid, getdatelike, questionID, questionDetail);
                }
            }
        }

        protected void ddlFilterOutPut_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblYourStat.Text = "";
            if (Session["IndividualOp"] != null)
            {
                string uid = Session["IndividualOp"].ToString();
                int questionId = ddlFilterQuestion.SelectedIndex;
                string questionDetails = ddlFilterQuestion.SelectedValue;
                DateTime select = Convert.ToDateTime(ddlOverDates.Text);
                DateTime getdatelike = dbmanager.CheckDateTimeLike(select);
                if (ddlFilterOutPut.SelectedIndex == 0)
                {
                    ViewIndividualBothRateAndComment(uid, getdatelike, questionId, questionDetails);
                }
                else
                {
                    ViewIndividualComment(uid, getdatelike, questionId, questionDetails);
                }
            }
        }

        protected void ddlFilterQuestion_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblYourStat.Text = "";
            if (Session["IndividualOp"] != null)
            {
                string uid = Session["IndividualOp"].ToString();
                int questionId = ddlFilterQuestion.SelectedIndex;
                string questionDetails = ddlFilterQuestion.SelectedValue;
                DateTime select = Convert.ToDateTime(ddlOverDates.Text);
                DateTime getdatelike = dbmanager.CheckDateTimeLike(select);
                if (ddlFilterQuestion.SelectedIndex >= 0 && ddlFilterOutPut.SelectedIndex == 0)
                {
                    ViewIndividualBothRateAndComment(uid, getdatelike, questionId, questionDetails);
                }
                else
                {
                    ViewIndividualComment(uid, getdatelike, questionId, questionDetails);
                }
            }
        }

        //protected double GetAverageRating(string uid, int qid, int count, DateTime date)
        //{
        //    double averagefinal = 0.00;




        //    //try
        //    //{
        //    //    ArrayList listofchoice = dbmanager.GetAllChoice();
        //    //    listofchoice.Remove("N/A");
        //    //    listofchoice.Add("N/A");

        //    //    string last = listofchoice[listofchoice.Count - 1].ToString();
        //    //    double final = 0.00;
        //    //    foreach (string rate in listofchoice)
        //    //    {
        //    //        if (rate != last)
        //    //        {
        //    //            int rating = Convert.ToInt32(rate);
        //    //            double apprate = dbmanager.GetCountYourAppraisalViaRate(uid, rate, qid, date);
        //    //            if (apprate > 0.0)
        //    //            {
        //    //                final += apprate * rating;
        //    //            }
        //    //        }
        //    //    }
        //    //    averagefinal = final / Convert.ToDouble(count);
        //    //    averagefinal = (Math.Round(averagefinal, 1));
        //    //    return averagefinal;
        //    //}
        //    //catch
        //    //{
        //    //    return averagefinal;
        //    //}




        //}
        protected double GetStandardDeviation(string uid, int qid, DateTime date)
        {
            ArrayList appraisalresults = dbmanager.GetAppraisalResults(uid, date, qid);
            double standarddeviationfinal = 0.00;
            double averagefinal = 0.00;
            try
            {
                ArrayList listofchoice = dbmanager.GetAllChoice();
                listofchoice.Remove("N/A");
                listofchoice.Add("N/A");

                string last = listofchoice[listofchoice.Count - 1].ToString();
                double final = 0.00;
                double std = 0.00;
                ArrayList standarddevlist = new ArrayList();
                foreach (string rate in listofchoice)
                {
                    if (rate != last)
                    {
                        int rating = Convert.ToInt32(rate);
                        double apprate = dbmanager.GetCountYourAppraisalViaRate(uid, rate, qid, date);
                        if (apprate > 0.0)
                        {
                            for (int i = 1; i <= Convert.ToInt32(apprate); i++)
                            {

                                std = rating;
                                standarddevlist.Add(std);
                            }
                            final += apprate * rating;
                        }
                    }
                }
                ArrayList listofstandarddev = new ArrayList();
                
                for (int i = 0; i < standarddevlist.Count; i++)
                {
                    double stdd = Convert.ToDouble(standarddevlist[i].ToString());
                    double stddd = stdd - averagefinal;
                    double stds = stddd * stddd;
                    listofstandarddev.Add(stds);
                }
                double standarddev = 0.00;
                for (int k = 0; k < listofstandarddev.Count; k++)
                {
                    standarddev += Convert.ToDouble(listofstandarddev[k].ToString());

                }
                double standarddevs = standarddev;

                standarddeviationfinal = (Math.Round(Math.Sqrt(standarddevs), 1));

                //standarddeviationfinal = (Math.Round(standarddeviationfinal, 1));
                return standarddeviationfinal;


            }
            catch
            {
                return standarddeviationfinal;
            }
        }

        protected double GetMedian(string uid, int qid, DateTime date)
        {
            double standarddeviationfinal = 0.00;
            double averagefinal = 0.00;
            double median = 0.00;
            int position = 0;
            int pos2 = 0;
            try
            {
                ArrayList listofchoice = dbmanager.GetAllChoice();
                listofchoice.Remove("N/A");
                listofchoice.Add("N/A");

                string last = listofchoice[listofchoice.Count - 1].ToString();
                double final = 0.00;
                double std = 0.00;

                ArrayList standarddevlist = new ArrayList();


                ArrayList medians = new ArrayList();
                int count = 0;
                foreach (string rate in listofchoice)
                {
                    if (rate != last)
                    {
                        int rating = Convert.ToInt32(rate);
                        double apprate = dbmanager.GetCountYourAppraisalViaRate(uid, rate, qid, date);
                        count = Convert.ToInt32(apprate);
                        if (apprate > 0.0)
                        {
                            
                            for (int i = 1; i <= Convert.ToInt32(apprate); i++)
                            {
                                if (count % 2 != 0)
                                {

                                    final = rating;
                                    position = ((count + 1) / 2) - 1;
                                    medians.Add(final);
                                }
                                else if (count % 2 == 0)
                                {
                                    final += rating;
                                    position = (count / 2) - 1;
                                    pos2 = (((count / 2) + 1)) - 1;
                                    medians.Add(final);

                                }
                            }


                        }
                    }
                }
                if (count % 2 != 0)
                {

                    median = (Math.Round(Convert.ToDouble(medians[position].ToString()), 1));
                }
                else if (count % 2 == 0)
                {
                    median = (Math.Round((Convert.ToDouble(medians[position].ToString()) + Convert.ToDouble(medians[pos2].ToString())) / 2, 1));


                }

                return median;


            }
            catch
            {
                return median;
            }
        }

        protected void ViewIndividualBothRateAndComment(string uid, DateTime date, int questionID, string questionDetails)
        {
            lblYourStat.Text = "";
            #region view individual both rate and comment
            int countapp2 = dbmanager.GetCountYourAppraisalViaDate(uid, date);
            ArrayList ratecount2 = dbmanager.GetAllChoice();
            ratecount2.Remove("N/A");
            ratecount2.Add("N/A");
            ArrayList listofquestion2 = dbmanager.GetAllQuestion();
            int qncount2 = 1;
            string content2 = "";
            double result2 = 0.0;
            double avgGrade = 0.0; // added

            #region overall grading result

            foreach (Question qn in listofquestion2)
            {
                result2 += dbmanager.GetAvgRating(uid, date, qn.QuestionID);
            }
            int countquestion = dbmanager.GetTotalCountQuestionInPeriod(uid, date);
            avgGrade = Math.Round((result2 / countquestion), 1);

            //content2 += "<table width='99.3%' border='0' style=padding-left:10px><tr><td>";
            //content2 += "<table width='99.3%' border='1' style='border-color:#000080' cellspacing='0'>";
            //content2 += "<tr style='border-color:#000080'><td style='border-color:#000080'>";
            if (avgGrade > 0.0)// change
            {
                content2 += "<font size='4'>Final grading received for period:&nbsp;" + avgGrade + "</font>";
            }
            else
            {
                content2 += "<font size='4'>Final grading received for period:&nbsp;" + "N/A" + "</font>";
            }
            //content2 += "<tr style='border-color:#000080'><td style='border-color:#000080'>";
            content2 += "<br>";
            content2 += "<font size='4'>Total number of peer feedback(s) you received:&nbsp;" + countapp2 + "</font>";
            content2 += "<br>";
            content2 += "<br>";
            content2 += "__________________________________________________________________________________________________________________________________________";
            content2 += "<br>";
            content2 += "<br>";
           
            #endregion

            if (questionID == 0)
            {
                #region view all questions
                //view by question number
                foreach (Question qn in listofquestion2) // for number of questions
                {
                    #region
                    if (qncount2 > 1)
                    {
                        content2 += "<br>";
                    }
                    double standarddeviation = GetStandardDeviation(uid, qn.QuestionID, date);
                    double median = GetMedian(uid, qn.QuestionID, date);
                    
                    //content2 += "<table width='99.3%' border='1' style='border-color:#000080' cellspacing='0' padding-left:'10px'>";
                    //content2 += "<tr style='border-color:#000080'><td style='border-color:#000080'>";
                    content2 += "<font color='blue' size = '4.5'><b>Q" + qncount2 + ")" + " " + qn.QuestionDetails; // question details
                    content2 += "</b></font><br>";
                    content2 += "<br>";

                    double averagefinal = dbmanager.GetAvgRating(uid, date, qn.QuestionID); // added
                    content2 += "<table width='800' border='1' style='border-color:#000080' cellspacing='0' padding-left:'10px'>";
                    content2 += "<tr style='border-color:#000080' width ='600'><td style='border-color:#000080' width='100'>";
                    if (averagefinal > 0.0)
                    {
                        content2 += "&nbsp;&nbsp;&nbsp;&nbsp;Average grade:&nbsp;" + averagefinal; // for avg grade on a question
                    }
                    else
                    {
                        content2 += "&nbsp;&nbsp;&nbsp;&nbsp;Average grade:&nbsp;N/A"; // for avg grade on a question
                    }
                    content2 += "</td>";
                    content2 += "<td style='border-color:#000080' width='100'>";
                    content2 += "&nbsp;&nbsp;&nbsp;&nbsp;Median Grade: " + median;
                    content2 += "</td>";
                    //content2 += "<br>";
                    //content2 += "<br>";
                    content2 += "<td style='border-color:#000080' width='100'>";
                    content2 += "&nbsp;&nbsp;&nbsp;&nbsp;Standard Deviation of grade: " + standarddeviation;
                    content2 += "</td>";
                    //content2 += "<br>";
                    //content2 += "<br>";

                    //content2 += "</td></tr>";
                    content2 += "</tr></table><br>";
                    //content2 += "</td></tr>";
                    //content2 += "</table><br>";

                    // end for question result
                    content2 += "<b>Table 1: Summary of grading received</b>";
                    content2 += "<table width='99.3%' border='1' style='border-color:#000080' cellspacing='0' padding-left:'10px'>";
                    content2 += "<tr style='border-color:#000080' align='left'>";
                    content2 += "<td style='border-color:#000080' width='20%' colspan=" + ratecount2.Count + "><b>Grading (Highest to Lowest)</b></td>";

                    foreach (string rate in ratecount2)
                    {
                        content2 += "<td width='8%' align='center' style='border-color:#000080'><b>" + rate + "</b></td>";
                    }

                    content2 += "</tr>";
                    content2 += "<tr style='border-color:#000080' align='left'>";
                    content2 += "<td style='border-color:#000080' width='20%' colspan=" + ratecount2.Count + "><b>Total number of feedback(s) on each grade</b></td>";

                    foreach (string rating in ratecount2)
                    {
                        int apprate = dbmanager.GetCountYourAppraisalViaRate(uid, rating, qn.QuestionID, date);
                        content2 += "<td width='8%' align='center' style='border-color:#000080'>" + apprate + "</td>";
                    }
                    content2 += "</tr></table><br>";
                    content2 += "<b>Table 2: Summary of remarks you received</b><br>";

                    ArrayList listoffunction = dbmanager.GetAllFunction();
                    ArrayList listoftotalnofunction = dbmanager.GetCountUserIDAppraisalNoFunction(uid, qn.QuestionID, date);
                    int remarksfunction = 0;
                    int remarks = 0;
                    int counter = 0;
                    int index = 1;
                    if (listoftotalnofunction.Count != 0)
                    {
                        if (listoftotalnofunction.Count > 0)
                        {
                            foreach (string name in listoftotalnofunction)
                            {
                                staffappraisal stffapp = dbmanager.GetAllRemarksViaUserNameNoFunction(uid, name, qn.QuestionID, date);

                                if (stffapp != null)
                                {
                                    if (counter == 0)
                                    {
                                        content2 += "<table width='99.3%' border='1' style='border-color:#000080' cellspacing='0'>";
                                        content2 += "<tr style='border-color:#000080' align='left'>";
                                        content2 += "<td style='border-color:#000080' width='9%'><b>No.</b></td><td style='border-color:#000080' width='91%'><b>Remarks</b></td></tr>";
                                        content2 += "<tr style='border-color:#000080' align='left'>";
                                        counter++;
                                    }
                                    content2 += "<td width='9%' align='left' style='border-color:#000080'>" + index + "</td>";
                                    content2 += "<td width='91%' align='left' style='border-color:#000080'>" + stffapp.Appremarks + "</td>";
                                    content2 += "</tr>";
                                    content2 += "<br>";
                                    content2 += "<br>";
                                    content2 += "__________________________________________________________________________________________________________________________________________";
                                    index++;
                                    remarksfunction++;
                                }
                            }
                        }
                    }

                    foreach (string function in listoffunction)
                    {
                        ArrayList totalappraisal = dbmanager.GetCountUserIDAppraisalFunction(uid, function, qn.QuestionID, date);

                        if (totalappraisal.Count != 0)
                        {
                            foreach (string userappraisal in totalappraisal)
                            {
                                staffappraisal stffapp = dbmanager.GetAllRemarksViaUserNameNoFunction(uid, userappraisal, qn.QuestionID, date);
                                if (stffapp != null)
                                {
                                    if (counter == 0)
                                    {
                                        content2 += "<table width='99.3%' border='1' style='border-color:#000080' cellspacing='0'>";
                                        content2 += "<tr style='border-color:#000080' align='left'>";
                                        content2 += "<td style='border-color:#000080' width='9%'><b>No.</b></td><td style='border-color:#000080' width='91%'><b>Remarks</b></td></tr>";
                                        content2 += "<tr style='border-color:#000080' align='left'>";
                                        counter++;
                                    }

                                    content2 += "<td width='9%' align='left' style='border-color:#000080'>" + index + "</td>";
                                    content2 += "<td width='91%' align='left' style='border-color:#000080'>" + stffapp.Appremarks + "</td>";
                                    content2 += "</tr>";
                                    content2 += "<br>";
                                    content2 += "<br>";
                                    content2 += "__________________________________________________________________________________________________________________________________________";
                                    index++;
                                    remarks++;
                                }
                            }
                        }
                    }
                    if (remarksfunction == 0 && remarks == 0)
                    {
                        content2 += "<b><font color='red'>No remarks found</font></b>";
                    }
                    if (counter > 0)
                    {
                        content2 += "</table>";
                    }
                    else
                    {
                        content2 += "<br>";
                    }
                    qncount2++;
                    #endregion
                }
                content2 += "</td></tr></table>";
                lblYourStat.Text += content2;
                #endregion
            }
            else
            {
                #region view per questions
                //view by question number
                foreach (Question qn in listofquestion2) // for number of questions
                {
                    if (qn.QuestionDetails == questionDetails)
                    {
                        //int countappqid = dbmanager.GetCountYourAppraisalViaQidDate(uid, qn.QuestionID, date);

                        if (qncount2 > 1)
                        {
                            content2 += "<br>";
                        }
                        double standarddeviation = GetStandardDeviation(uid, qn.QuestionID, date);
                        double median = GetMedian(uid, qn.QuestionID, date);

                        //double averagefinal = GetAverageRating(uid, qn.QuestionID, countappqid, date);
                        //content2 += "<table width='99.3%' border='1' style='border-color:#000080' cellspacing='0' padding-left:10px'>";
                        //content2 += "<tr style='border-color:#000080'><td style='border-color:#000080'>";
                        content2 += "<font color='blue' size = '4.5'><b> Q" + qncount2 + ") " + qn.QuestionDetails; // question details
                        content2 += "</b></font><br>";
                        content2 += "<br>";
                        content2 += "<table width='800' border='1' style='border-color:#000080' cellspacing='0' padding-left:'10px'>";
                        content2 += "<tr style='border-color:#000080' width ='600'><td style='border-color:#000080' width='100'>";

                        double averagefinal = dbmanager.GetAvgRating(uid, date, qn.QuestionID); // added

                        if (averagefinal > 0.0)
                        {
                            content2 += "&nbsp;&nbsp;&nbsp;&nbsp;Average grade:&nbsp;" + averagefinal; // for avg grade on a question
                        }
                        else
                        {
                            content2 += "&nbsp;&nbsp;&nbsp;&nbsp;Average grade:&nbsp;N/A"; // for avg grade on a question
                        }

                        content2 += "</td>";
                        content2 += "<td style='border-color:#000080' width='100'>";
                        content2 += "&nbsp;&nbsp;&nbsp;&nbsp;Median Grade: " + median;
                        content2 += "</td>";
                        //content2 += "<br>";
                        //content2 += "<br>";
                        content2 += "<td style='border-color:#000080' width='100'>";
                        content2 += "&nbsp;&nbsp;&nbsp;&nbsp;Standard Deviation of grade: " + standarddeviation;
                        content2 += "</td>";
                        //content2 += "<br>";
                        //content2 += "<br>";

                        //content2 += "</td></tr>";
                        content2 += "</tr></table><br>";

                        // end for question result
                        content2 += "<b>Table 1: Summary of grading received</b>";
                        content2 += "<table width='99.3%' border='1' style='border-color:#000080' cellspacing='0' padding-left:10px'>";
                        content2 += "<tr style='border-color:#000080' align='left'>";
                        content2 += "<td style='border-color:#000080' width='20%' colspan=" + ratecount2.Count + "><b>Grading (Highest to Lowest)</b></td>";

                        foreach (string rate in ratecount2)
                        {
                            content2 += "<td width='8%' align='center' style='border-color:#000080'><b>" + rate + "</b></td>";
                        }

                        content2 += "</tr>";
                        content2 += "<tr style='border-color:#000080' align='left'>";
                        content2 += "<td style='border-color:#000080' width='20%' colspan=" + ratecount2.Count + "><b>Total number of feedback(s) on each grade</b></td>";

                        foreach (string rating in ratecount2)
                        {
                            int apprate = dbmanager.GetCountYourAppraisalViaRate(uid, rating, qn.QuestionID, date);
                            content2 += "<td width='8%' align='center' style='border-color:#000080'>" + apprate + "</td>";
                        }
                        content2 += "</tr></table><br>";
                        content2 += "<b>Table 2: Summary of remarks you received</b><br>";

                        ArrayList listoffunction = dbmanager.GetAllFunction();
                        ArrayList listoftotalnofunction = dbmanager.GetCountUserIDAppraisalNoFunction(uid, qn.QuestionID, date);
                        int remarksfunction = 0;
                        int remarks = 0;
                        int counter = 0;
                        int index = 1;
                        if (listoftotalnofunction.Count != 0)
                        {
                            if (listoftotalnofunction.Count > 0)
                            {
                                foreach (string name in listoftotalnofunction)
                                {
                                    staffappraisal stffapp = dbmanager.GetAllRemarksViaUserNameNoFunction(uid, name, qn.QuestionID, date);

                                    if (stffapp != null)
                                    {
                                        if (counter == 0)
                                        {
                                            content2 += "<table width='99.3%' border='1' style='border-color:#000080' cellspacing='0'>";
                                            content2 += "<tr style='border-color:#000080' align='left'>";
                                            content2 += "<td style='border-color:#000080' width='9%'><b>No.</b></td><td style='border-color:#000080' width='91%'><b>Remarks</b></td></tr>";
                                            content2 += "<tr style='border-color:#000080' align='left'>";
                                            counter++;
                                        }
                                        content2 += "<td width='9%' align='left' style='border-color:#000080'>" + index + "</td>";
                                        content2 += "<td width='91%' align='left' style='border-color:#000080'>" + stffapp.Appremarks + "</td>";
                                        content2 += "</tr>";
                                        content2 += "<br>";
                                        content2 += "<br>";
                                        content2 += "__________________________________________________________________________________________________________________________________________";
                                        index++;
                                        remarksfunction++;
                                    }
                                }
                            }
                        }

                        foreach (string function in listoffunction)
                        {
                            ArrayList totalappraisal = dbmanager.GetCountUserIDAppraisalFunction(uid, function, qn.QuestionID, date);

                            if (totalappraisal.Count != 0)
                            {
                                foreach (string userappraisal in totalappraisal)
                                {
                                    staffappraisal stffapp = dbmanager.GetAllRemarksViaUserNameNoFunction(uid, userappraisal, qn.QuestionID, date);
                                    if (stffapp != null)
                                    {
                                        if (counter == 0)
                                        {
                                            content2 += "<table width='99.3%' border='1' style='border-color:#000080' cellspacing='0'>";
                                            content2 += "<tr style='border-color:#000080' align='left'>";
                                            content2 += "<td style='border-color:#000080' width='9%'><b>No.</b></td><td style='border-color:#000080' width='91%'><b>Remarks</b></td></tr>";
                                            content2 += "<tr style='border-color:#000080' align='left'>";
                                            counter++;
                                        }

                                        content2 += "<td width='9%' align='left' style='border-color:#000080'>" + index + "</td>";
                                        content2 += "<td width='91%' align='left' style='border-color:#000080'>" + stffapp.Appremarks + "</td>";
                                        content2 += "</tr>";
                                        content2 += "<br>";
                                        content2 += "<br>";
                                        content2 += "__________________________________________________________________________________________________________________________________________";
                                        index++;
                                        remarks++;
                                    }
                                }
                            }
                        }
                        if (remarksfunction == 0 && remarks == 0)
                        {
                            content2 += "<b><font color='red'>No remarks found</font></b>";
                        }
                        if (counter > 0)
                        {
                            content2 += "</table>";
                        }
                        else
                        {
                            content2 += "<br>";
                        }
                        qncount2++;
                    }
                }
                content2 += "</td></tr></table>";
                lblYourStat.Text += content2;
                #endregion
            }
            #endregion
        }

        protected void ViewIndividualComment(string uid, DateTime date, int questionId, string questionDetails)
        {
            #region view individual with comment
            int countapp = dbmanager.GetCountYourAppraisalViaDate(uid, date);
            ArrayList ratecount = dbmanager.GetAllChoice();
            ArrayList listofquestion = dbmanager.GetAllQuestion();
            int qncount = 1;
            string content = "";
            double result = 0.0;

            //when the question id is 0
            if (questionId == 0)
            {
                #region for all questions output
                foreach (Question qn in listofquestion)
                {
                    result += dbmanager.GetAvgRating(uid, date, qn.QuestionID);
                }
                int countquestion = dbmanager.GetTotalCountQuestionInPeriod(uid, date);
                result = Math.Round((result / countquestion), 1);
                //content += "<table width='99.3%' border='0' style=padding-left:10px><tr><td>";
                //content += "<table width='99.3%' border='1' style='border-color:#000080' cellspacing='0'>";
                //content += "<tr style='border-color:#000080'><td style='border-color:#000080'>";
                content += "<font size='4'><b>Final grading received for period:&nbsp;" + result + "</b></font>";
                //content += "<tr style='border-color:#000080'><td style='border-color:#000080'>";
                content += "<font size='4'><b>Total number of peer feedback(s) you received:&nbsp;" + countapp + "</b></font>";
                content += "<br>";
                content += "<br>";
                content += "__________________________________________________________________________________________________________________________________________";
                content += "<br>";
                content += "<br>";
                //content += "</td></tr></table><br>";
                //view by question number
                foreach (Question qn in listofquestion)
                {
                    //int countappqid = dbmanager.GetCountYourAppraisalViaQidDate(uid, qn.QuestionID, date);

                    if (qncount > 1)
                    {
                        content += "<br>";
                    }
                    //double averagefinal = GetAverageRating(uid, qn.QuestionID, countappqid, date);
                    double standarddeviation = GetStandardDeviation(uid, qn.QuestionID, date);
                    double median = GetMedian(uid, qn.QuestionID, date);
                    //content += "<table width='99.3%' border='1' style='border-color:#000080' cellspacing='0'>";

                    ArrayList listofFunction = dbmanager.GetDistinctFunctions();

                    //content += "<tr style='border-color:#000080'><td style='border-color:#000080'>";
                    content += "<font color='blue' size='4.5'><b>Q" + qncount + ")" + " " + qn.QuestionDetails;
                    content += "</b></font>";
                    content += "<br>";
                    content += "<br>";

                    content += "<table width='800' border='1' style='border-color:#000080' cellspacing='0'>";
                    content += "<tr style='border-color:#000080' width ='600'><td style='border-color:#000080' width='100'>";

                    double averagefinal = dbmanager.GetAvgRating(uid, date, qn.QuestionID);

                    if (averagefinal > 0.0)
                    {
                        content += "&nbsp;&nbsp;&nbsp;&nbsp;Average grade:&nbsp;" + averagefinal;

                        content += "</td>";
                    }
                    else
                    {

                        content += "&nbsp;&nbsp;&nbsp;&nbsp;Average grade:&nbsp;N/A";

                        content += "</td>";
                    }
                    content += "<td style='border-color:#000080' width='100'>";
                    content += "&nbsp;&nbsp;&nbsp;&nbsp;Median Grade: " + median;
                    content += "</td>";
                    //content += "<br>";
                    //content += "<br>";
                    content += "<td style='border-color:#000080' width='100'>";
                    content += "&nbsp;&nbsp;&nbsp;&nbsp;Standard Deviation of grade: " + standarddeviation;
                    content += "</td>";
                    //content += "<br>";
                    //content += "<br>";

                    //content += "</td></tr>";
                    content += "</tr></table><br>";

                    content += "<b>Table 1: Summary of remarks you received</b><br>";

                    ArrayList listoffunction = dbmanager.GetAllFunction();
                    ArrayList listoftotalnofunction = dbmanager.GetCountUserIDAppraisalNoFunction(uid, qn.QuestionID, date);
                    int remarksfunction = 0;
                    int remarks = 0;
                    int counter = 0;
                    int index = 1;
                    if (listoftotalnofunction.Count != 0)
                    {
                        foreach (string name in listoftotalnofunction)
                        {
                            staffappraisal stffapp = dbmanager.GetAllRemarksViaUserNameNoFunction(uid, name, qn.QuestionID, date);

                            if (stffapp != null)
                            {
                                if (counter == 0)
                                {
                                    //if admin can see name
                                    content += "<table width='99.3%' border='1' style='border-color:#000080' cellspacing='0'>";
                                    content += "<tr style='border-color:#000080' align='left'>";
                                    content += "<td style='border-color:#000080' width='9%'><b>No.</b></td><td style='border-color:#000080' width='91%'><b>Remarks</b></td></tr>";
                                    content += "<tr style='border-color:#000080' align='left'>";
                                    counter++;
                                }
                                content += "<td width='9%' align='left' style='border-color:#000080'>" + index + "</td>";
                                content += "<td width='91%' align='left' style='border-color:#000080'>" + stffapp.Appremarks + "</td>";
                                content += "</tr>";
                                content += "<br>";
                                content += "<br>";
                                content += "__________________________________________________________________________________________________________________________________________";
                                index++;
                                remarksfunction++;
                            }
                        }
                    }
                    foreach (string function in listoffunction)
                    {
                        ArrayList totalappraisal = dbmanager.GetCountUserIDAppraisalFunction(uid, function, qn.QuestionID, date);

                        if (totalappraisal.Count != 0)
                        {
                            if (counter == 0)
                            {
                                content += "<table width='99.3%' border='1' style='border-color:#000080' cellspacing='0'>";
                                content += "<tr style='border-color:#000080' align='left'>";
                                content += "<td style='border-color:#000080' width='9%'><b>No.</b></td><td style='border-color:#000080' width='91%'><b>Remarks</b></td></tr>";
                                content += "<tr style='border-color:#000080' align='left'>";
                                counter++;
                            }
                            foreach (string userappraisal in totalappraisal)
                            {
                                staffappraisal stffapp = dbmanager.GetAllRemarksViaUserNameNoFunction(uid, userappraisal, qn.QuestionID, date);

                                if (stffapp != null)
                                {
                                    content += "<td width='9%' align='left' style='border-color:#000080'>" + index + "</td>";
                                    content += "<td width='91%' align='left' style='border-color:#000080'>" + stffapp.Appremarks + "</td>";
                                    content += "</tr>";
                                    content += "<br>";
                                    content += "<br>";
                                    content += "__________________________________________________________________________________________________________________________________________";
                                    index++;
                                    remarks++;
                                }
                            }
                        }
                    }

                    if (remarksfunction == 0 && remarks == 0)
                    {
                        content += "<b><font color='red'>No remarks found</font></b>";
                    }
                    if (counter > 0)
                    {
                        content += "</table>";
                    }
                    else
                    {
                        content += "<br>";
                    }
                    qncount++;
                }
                content += "</td></tr></table>";
                lblYourStat.Text += content;
                #endregion
            }
            else
            {
                #region for per questions output
                foreach (Question qn in listofquestion)
                {
                    if (qn.QuestionDetails == questionDetails)
                    {
                        //int countappqid = dbmanager.GetCountYourAppraisalViaQidDate(uid, qn.QuestionID, date);

                        if (qncount > 1)
                        {
                            content += "<br>";
                        }
                        //double averagefinal = GetAverageRating(uid, qn.QuestionID, countappqid, date);
                        double standarddeviation = GetStandardDeviation(uid, qn.QuestionID, date);
                        double median = GetMedian(uid, qn.QuestionID, date);
                        //content += "<table width='99.3%' border='1' style='border-color:#000080' cellspacing='0'>";

                        ArrayList listofFunction = dbmanager.GetDistinctFunctions();

                        //content += "<tr style='border-color:#000080'><td style='border-color:#000080'>";
                        content += "<font color='blue' size='4.5'><b>Q" + qncount + ")" + " " + qn.QuestionDetails;
                        content += "</b></font>";
                        content += "<br>";
                        content += "<br>";

                        content += "<table width='800' border='1' style='border-color:#000080' cellspacing='0'>";
                        content += "<tr style='border-color:#000080' width ='600'><td style='border-color:#000080' width='100'>";


                        double averagefinal = dbmanager.GetAvgRating(uid, date, qn.QuestionID);

                        if (averagefinal > 0.0)
                        {
                            content += "&nbsp;&nbsp;&nbsp;&nbsp;Average grade:&nbsp;" + averagefinal;

                            content += "</td>";
                        }
                        else
                        {

                            content += "&nbsp;&nbsp;&nbsp;&nbsp;Average grade:&nbsp;N/A";

                            content += "</td>";
                        }
                        content += "<td style='border-color:#000080' width='100'>";
                        content += "&nbsp;&nbsp;&nbsp;&nbsp;Median Grade: " + median;
                        content += "</td>";
                        //content += "<br>";
                        //content += "<br>";
                        content += "<td style='border-color:#000080' width='100'>";
                        content += "&nbsp;&nbsp;&nbsp;&nbsp;Standard Deviation of grade: " + standarddeviation;
                        content += "</td>";
                        //content += "<br>";
                        //content += "<br>";

                        //content += "</td></tr>";
                        content += "</tr></table><br>";
                        content += "<b>Table 1: Summary of remarks you received</b><br>";

                        ArrayList listoffunction = dbmanager.GetAllFunction();
                        ArrayList listoftotalnofunction = dbmanager.GetCountUserIDAppraisalNoFunction(uid, qn.QuestionID, date);
                        int remarksfunction = 0;
                        int remarks = 0;
                        int counter = 0;
                        int index = 1;
                        if (listoftotalnofunction.Count != 0)
                        {
                            foreach (string name in listoftotalnofunction)
                            {
                                staffappraisal stffapp = dbmanager.GetAllRemarksViaUserNameNoFunction(uid, name, qn.QuestionID, date);

                                if (stffapp != null)
                                {
                                    if (counter == 0)
                                    {
                                        //if admin can see name
                                        content += "<table width='99.3%' border='1' style='border-color:#000080' cellspacing='0' padding-left:10px'>";
                                        content += "<tr style='border-color:#000080' align='left'>";
                                        content += "<td style='border-color:#000080' width='9%'><b>No.</b></td><td style='border-color:#000080' width='91%'><b>Remarks</b></td></tr>";
                                        content += "<tr style='border-color:#000080' align='left'>";
                                        counter++;
                                    }
                                    content += "<td width='9%' align='left' style='border-color:#000080'>" + index + "</td>";
                                    content += "<td width='91%' align='left' style='border-color:#000080'>" + stffapp.Appremarks + "</td>";
                                    content += "</tr>";
                                    content += "<br>";
                                    content += "<br>";
                                    content += "__________________________________________________________________________________________________________________________________________";
                                    index++;
                                    remarksfunction++;
                                }
                            }
                        }
                        foreach (string function in listoffunction)
                        {
                            ArrayList totalappraisal = dbmanager.GetCountUserIDAppraisalFunction(uid, function, qn.QuestionID, date);

                            if (totalappraisal.Count != 0)
                            {
                                if (counter == 0)
                                {
                                    content += "<table width='99.3%' border='1' style='border-color:#000080' cellspacing='0' padding-left:10px'>";
                                    content += "<tr style='border-color:#000080' align='left'>";
                                    content += "<td style='border-color:#000080' width='9%'><b>No.</b></td><td style='border-color:#000080' width='91%'><b>Remarks</b></td></tr>";
                                    content += "<tr style='border-color:#000080' align='left'>";
                                    counter++;
                                }
                                foreach (string userappraisal in totalappraisal)
                                {
                                    staffappraisal stffapp = dbmanager.GetAllRemarksViaUserNameNoFunction(uid, userappraisal, qn.QuestionID, date);

                                    if (stffapp != null)
                                    {
                                        content += "<td width='9%' align='left' style='border-color:#000080'>" + index + "</td>";
                                        content += "<td width='91%' align='left' style='border-color:#000080'>" + stffapp.Appremarks + "</td>";
                                        content += "</tr>";
                                        content += "<br>";
                                        content += "<br>";
                                        content += "__________________________________________________________________________________________________________________________________________";
                                        index++;
                                        remarks++;
                                    }
                                }
                            }
                        }

                        if (remarksfunction == 0 && remarks == 0)
                        {
                            content += "<b><font color='red'>No remarks found</font></b>";
                        }
                        if (counter > 0)
                        {
                            content += "</table>";
                        }
                        else
                        {
                            content += "<br>";
                        }
                        qncount++;
                    }
                }
                content += "</td></tr></table>";
                lblYourStat.Text += content;
                #endregion
            }
            // end
            string legend = "";
            legend += "<table style=padding-left:10px><tr><td>";
            legend += "</td></tr></table>";
            legend += "<br>";
            legend += "<table style='padding-left:10px'><tr><td></td><td>";
            legend += "Appraisal Period of " + ddlOverDates.SelectedValue;
            legend += "</td></tr></table>";
            lbllegend.Text = legend;

            #endregion
        }

        protected void IndividualBack_Click(object sender, EventArgs e)
        {
            IndividualBack.Style.Add("color", "Purple");
            Session["IndividualBackOp"] = true;
            if (Session["OperatorCheck"] != null)
            {
                SearchPanel.Visible = true;
                MultiView1.ActiveViewIndex = 0;
            }
            else if (Session["PreviousPageOp"] != null)
            {
                MultiView1.ActiveViewIndex = 0;
            }
            else
            {
                Response.Redirect("default.aspx");
            }
        }

        protected void wordExportIndividual_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                string staffName = Session["Name"].ToString();
                string tdate = ddlOverDates.SelectedValue;
                string date = tdate.Replace(",", "-");
                string filename = staffName + "(" + date + ").doc";
                Response.ClearContent();
                Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", filename));
                Response.Charset = "";
                Response.ContentType = "application/ms-word";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                lblYourStat.RenderControl(htw);
                Response.Write(sw.ToString());
                Response.End();
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message.ToString());
            }
        }

        private static void PrepareControlForExport(Control control)
        {
            for (int i = 0; i < control.Controls.Count; i++)
            {
                Control c = control.Controls[i];

                if (c is LinkButton)
                {
                    control.Controls.Remove(c);
                    control.Controls.AddAt(i, new LiteralControl((c as LinkButton).Text));
                }
                else if (c is ImageButton)
                {
                    control.Controls.Remove(c);
                    control.Controls.AddAt(i, new LiteralControl((c as ImageButton).AlternateText));
                }
                else if (c is HyperLink)
                {
                    control.Controls.Remove(c);
                    control.Controls.AddAt(i, new LiteralControl((c as HyperLink).Text));
                }
                else if (c is Image)
                {
                    control.Controls.Remove(c);
                    control.Controls.AddAt(i, new LiteralControl((c as Image).AlternateText));
                }
                else if (c is DropDownList)
                {
                    control.Controls.Remove(c);
                    control.Controls.AddAt(i, new LiteralControl((c as DropDownList).SelectedItem.Text));
                }
                else if (c is CheckBox)
                {
                    control.Controls.Remove(c);
                    if ((c as CheckBox).Checked)
                    {
                        control.Controls.AddAt(i, new LiteralControl("True"));
                    }
                    else
                    {
                        control.Controls.AddAt(i, new LiteralControl("False"));
                    }
                }
                else if (c is HiddenField)
                {
                    control.Controls.Remove(c);
                }

                if (c.HasControls())
                {
                    PrepareControlForExport(c);
                }
            }
        }

        public ArrayList RemoveDups(ArrayList items)
        {
            ArrayList noDups = new ArrayList();

            foreach (string strItem in items)
            {
                if (!noDups.Contains(strItem.Trim()))
                {
                    noDups.Add(strItem.Trim());
                }
            }
            noDups.Sort();
            return noDups;
        }

        protected void SearchBySectionFunction_Click(object sender, EventArgs e)
        {
            Session["SearchClick"] = null;
            SearchTbx.Text = "";
            if (Session["OperatorAll"] == null)
            {
                if (ddlFilterSection.SelectedIndex == 1 || ddlFilterFunction.SelectedIndex == 1)
                {
                    Response.Redirect("ViewAppraisal.aspx");
                }
                else if (ddlFilterSection.Text == "All section" && ddlFilterFunction.Text == "All function")
                {
                    Response.Redirect("ViewAppraisal.aspx");
                }
                else if (ddlFilterSection.SelectedIndex != 0 && ddlFilterFunction.SelectedIndex == 0)
                {
                    BindHistoryReportViaSectionIndividual(ddlFilterSection.Text);
                    Session["SearchClick"] = true;
                }
                else if (ddlFilterSection.SelectedIndex != 0 && ddlFilterFunction.Text == "All function")
                {
                    BindHistoryReportViaSection(ddlFilterSection.Text);
                    Session["SearchClick"] = true;
                }
                else if (ddlFilterFunction.SelectedIndex != 0)
                {
                    if (Session["ListofSectionOp"] != null)
                    {
                        ArrayList listoffiltersection = RemoveDups((ArrayList)Session["ListofSectionOp"]);
                        string fulllist = "";
                        foreach (string sec in listoffiltersection)
                        {
                            fulllist += sec + ",";
                        }
                        if (ddlFilterSection.SelectedIndex == 0 || ddlFilterSection.Text == "All section")
                        {
                            BindHistoryReportViaSectionAndFunctionWithRegion(fulllist, ddlFilterFunction.Text);
                        }
                        else
                        {
                            BindHistoryReportViaSectionAndFunction(ddlFilterSection.Text, ddlFilterFunction.Text);
                        }
                        Session["SearchClick"] = true;
                    }
                }
                else
                {
                    MessageBoxShow("No result found for filter.");
                }
            }
            else
            {
                if (ddlFilterSection.SelectedIndex == 1 || ddlFilterFunction.SelectedIndex == 1)
                {
                    Response.Redirect("ViewAppraisal.aspx");
                }
                else if (ddlFilterSection.Text == "All section" && ddlFilterFunction.Text == "All function")
                {
                    Response.Redirect("ViewAppraisal.aspx");
                }
                else if (ddlFilterSection.SelectedIndex == 0 && ddlFilterFunction.SelectedIndex != 0)
                {
                    BindHistoryReportViaFunction(ddlFilterFunction.Text);
                    Session["SearchClick"] = true;
                }
                else if (ddlFilterSection.SelectedIndex != 0 && ddlFilterFunction.SelectedIndex == 0)
                {
                    BindHistoryReportViaSectionIndividual(ddlFilterSection.Text);
                    Session["SearchClick"] = true;
                }
                else if (ddlFilterSection.Text == "All section" && ddlFilterFunction.SelectedIndex != 0)
                {
                    BindHistoryReportViaFunction(ddlFilterFunction.Text);
                    Session["SearchClick"] = true;
                }
                else if (ddlFilterSection.SelectedIndex != 0 && ddlFilterFunction.Text == "All function")
                {
                    BindHistoryReportViaSection(ddlFilterSection.Text);
                    Session["SearchClick"] = true;
                }
                else if (ddlFilterSection.SelectedIndex != 0 && ddlFilterFunction.SelectedIndex != 0)
                {
                    BindHistoryReportViaSectionAndFunction(ddlFilterSection.Text, ddlFilterFunction.Text);
                    Session["SearchClick"] = true;
                }
                else
                {
                    MessageBoxShow("No result found for filter.");
                }
            }
        }

        public static iTextSharp.text.Document document;
        protected void Exportchart(ArrayList chart)
        {
            string uid = Session["UserID"].ToString();
            MemoryStream msReport = new MemoryStream();

            try
            {
                document = new iTextSharp.text.Document(iTextSharp.text.PageSize.LETTER, 72, 72, 82, 72);
                iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, msReport);
                document.AddAuthor("Test");
                document.AddSubject("Export to PDF");
                document.Open();

                for (int i = 0; i < chart.Count; i++)
                {
                    iTextSharp.text.Chunk c = new iTextSharp.text.Chunk("Export chart to PDF", iTextSharp.text.FontFactory.GetFont("VERDANA", 15));
                    iTextSharp.text.Paragraph p = new iTextSharp.text.Paragraph();
                    p.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                    iTextSharp.text.Image hImage = null;
                    hImage = iTextSharp.text.Image.GetInstance(MapPath(chart[i].ToString()));

                    float NewWidth = 500;
                    float MaxHeight = 400;

                    if (hImage.Width <= NewWidth) { NewWidth = hImage.Width; } float NewHeight = hImage.Height * NewWidth / hImage.Width; if (NewHeight > MaxHeight)
                    {
                        NewWidth = hImage.Width * MaxHeight / hImage.Height;
                        NewHeight = MaxHeight;
                    }

                    float ratio = hImage.Width / hImage.Height;
                    hImage.ScaleAbsolute(NewWidth, NewHeight);
                    document.Add(p);
                    document.Add(hImage);
                }
                // close it
                document.Close();
                string filename = "Feedback Overview for " + Session["Name"].ToString() + ".pdf";
                Response.AddHeader("Content-type", "application/pdf");
                Response.AddHeader("Content-Disposition", "attachment; filename=" + filename);
                Response.OutputStream.Write(msReport.GetBuffer(), 0, msReport.GetBuffer().Length);

            }
            catch (System.Threading.ThreadAbortException ex)
            {
                throw new Exception("Error occured: " + ex);
            }
        }

        protected ArrayList GetChartResult(string uid)
        {
            ArrayList listofresult = new ArrayList();
            ArrayList listofquestion = (ArrayList)Session["ListofQuestion"];
            string userid = uid;
            ArrayList listofhistorydates = dbmanager.GetTotalHistoryDates(userid);

            int index = 0;
            double result = 0.0;
            foreach (DateTime date in listofhistorydates)
            {
                foreach (Question qn in listofquestion)
                {
                    result += dbmanager.GetAvgRating(userid, date, qn.QuestionID);
                }
                int countquestion = dbmanager.GetTotalCountQuestionInPeriod(userid, date);
                result = Math.Round((result / countquestion), 1);
                DateTime toshortdate = ((DateTime)listofhistorydates[index]);
                string monthname = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(toshortdate.Month);

                listofresult.Add(result + ";" + monthname.Substring(0, 3) + "/" + toshortdate.Year.ToString().Substring(2, 2));
                index++;
                result = 0.0;
            }
            return listofresult;
        }

        protected ArrayList GetChartResultViaQuestion(string uid, int questionID)
        {
            ArrayList listofresult = new ArrayList();
            ArrayList listofquestion = (ArrayList)Session["ListofQuestion"]; // note this redentane
            string userid = uid;
            ArrayList listofhistorydates = dbmanager.GetTotalHistoryDates(userid);

            int index = 0;
            double result = 0.0;
            foreach (DateTime date in listofhistorydates)
            {
                result += dbmanager.GetAvgRating(userid, date, questionID);
                int countquestion = dbmanager.GetTotalCountQuestionInPeriod(userid, date);
                DateTime toshortdate = ((DateTime)listofhistorydates[index]);
                string monthname = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(toshortdate.Month);

                listofresult.Add(result + ";" + monthname.Substring(0, 3) + "/" + toshortdate.Year.ToString().Substring(2, 2));
                index++;
                result = 0.0;
            }
            return listofresult;
        }

        protected void historylineLink_Click(object sender, EventArgs e)
        {
            historylineLink.Style.Add("color", "Purple");
            Session["historylineLinkOp"] = true;
            ArrayList chartArray = new ArrayList();
            string individualuid = Session["IndividualOp"].ToString();
            string name = dbmanager.GetNameViaUserID(individualuid);

            #region for overall result

            Chart chart = new Chart();
            ArrayList listofresult = GetChartResult(individualuid);
            if (listofresult.Count > 1)
            {
                ArrayList x = new ArrayList();
                ArrayList y = new ArrayList();
                foreach (string result in listofresult)
                {
                    string[] resultsplit = result.Split(';');
                    x.Add(resultsplit[1].ToString());
                    y.Add(resultsplit[0].ToString());
                }
                chart.Width = 500;
                chart.Height = 500;

                chart.Legends.Add("Legend");
                chart.Legends[0].Enabled = true;
                chart.Legends[0].Docking = Docking.Bottom;
                chart.Legends[0].Alignment = System.Drawing.StringAlignment.Center;
                chart.Titles.Add("Overall feedback report of individual: " + name);
                chart.Titles[0].Font = new System.Drawing.Font("Arial", 10f);

                chart.ChartAreas.Add("");
                chart.ChartAreas[0].AxisX.Title = "Period of feedback";
                chart.ChartAreas[0].AxisY.Title = "Final Grade";
                chart.ChartAreas[0].AxisX.TitleFont = new System.Drawing.Font("Arial", 10f);
                chart.ChartAreas[0].AxisY.TitleFont = new System.Drawing.Font("Arial", 10f);
                chart.ChartAreas[0].AxisX.LabelStyle.Font = new System.Drawing.Font("Arial", 10f);

                chart.Series.Add("Line of average grade");
                chart.Series[0].ChartType = SeriesChartType.Line;
                chart.Series[0].BorderWidth = 2;
                chart.Series[0].BorderColor = System.Drawing.Color.FromArgb(26, 59, 105);
                chart.Series[0].Points.DataBindXY(x, y);
                string path = "Charts/chart_overall(Line).png";
                chart.SaveImage(Server.MapPath(path), ChartImageFormat.Png);
                chartArray.Add(path);

            #endregion

                #region get overall result via questions

                ArrayList listOfQuestions = dbmanager.GetAllQuestionListWithIDs();
                if (listOfQuestions.Count > 0)
                {
                    foreach (Question qu in listOfQuestions)
                    {
                        ArrayList listofresultviaquestion = GetChartResultViaQuestion(individualuid, qu.QuestionID);
                        ArrayList x1 = new ArrayList();
                        ArrayList y1 = new ArrayList();

                        foreach (string result in listofresultviaquestion)
                        {
                            string[] resultsplit = result.Split(';');
                            x1.Add(resultsplit[1].ToString());
                            y1.Add(resultsplit[0].ToString());
                        }
                        Chart chart1 = new Chart();
                        chart1.Width = 500;
                        chart1.Height = 500;

                        chart1.Legends.Add("Legend" + qu.QuestionID);
                        chart1.Legends[0].Enabled = true;
                        chart1.Legends[0].Docking = Docking.Bottom;
                        chart1.Legends[0].Alignment = System.Drawing.StringAlignment.Center;
                        string title = "Q" + qu.QuestionID + ") " + qu.QuestionDetails;
                        chart1.Titles.Add(title);
                        chart1.Titles[0].Font = new System.Drawing.Font("Arial", 10f);
                        chart1.Titles[0].Alignment = System.Drawing.ContentAlignment.TopLeft;

                        chart1.ChartAreas.Add("");
                        chart1.ChartAreas[0].AxisX.Title = "Period of feedback";
                        chart1.ChartAreas[0].AxisY.Title = "Average Grade";
                        chart1.ChartAreas[0].AxisX.TitleFont = new System.Drawing.Font("Arial", 10f);
                        chart1.ChartAreas[0].AxisY.TitleFont = new System.Drawing.Font("Arial", 10f);
                        chart1.ChartAreas[0].AxisX.LabelStyle.Font = new System.Drawing.Font("Arial", 10f);

                        chart1.Series.Add("Area of average grade for question " + qu.QuestionID);
                        chart1.Series[0].ChartType = SeriesChartType.Line;
                        chart1.Series[0].BorderWidth = 2;
                        chart1.Series[0].BorderColor = System.Drawing.Color.FromArgb(26, 59, 105);
                        chart1.Series[0].Points.DataBindXY(x1, y1);



                        string path1 = "Charts/chart_Q" + qu.QuestionID + "(Line).png";
                        chart1.SaveImage(Server.MapPath(path1), ChartImageFormat.Png);
                        chartArray.Add(path1);
                    }
                }
                #endregion
                Exportchart(chartArray);
            }
            else
            {
                MessageBoxShow("Line chart not obvious for viewing, please try again later!");
            }
        }

        protected void historychartbarLink_Click(object sender, EventArgs e)
        {
            historychartbarLink.Style.Add("color", "Purple");
            Session["historychartbarLinkOp"] = true;
            ArrayList chartArray = new ArrayList();
            string individualuid = Session["IndividualOp"].ToString();
            string name = dbmanager.GetNameViaUserID(individualuid);

            #region gets overall result

            Chart chart = new Chart();
            ArrayList listofresult = GetChartResult(individualuid); // gets overall result
            ArrayList x = new ArrayList();
            ArrayList y = new ArrayList();
            foreach (string result in listofresult)
            {
                string[] resultsplit = result.Split(';');
                x.Add(resultsplit[1].ToString());
                y.Add(resultsplit[0].ToString());
            }
            chart.Width = 500;
            chart.Height = 500;

            chart.Legends.Add("Legend");
            chart.Legends[0].Enabled = true;
            chart.Legends[0].Docking = Docking.Bottom;
            chart.Legends[0].Alignment = System.Drawing.StringAlignment.Center;
            chart.Titles.Add("Overall feedback report of individual: " + name);
            chart.Titles[0].Font = new System.Drawing.Font("Arial", 10f);

            chart.ChartAreas.Add("");
            chart.ChartAreas[0].AxisX.Title = "Period of feedback";
            chart.ChartAreas[0].AxisY.Title = "Final Grade";
            chart.ChartAreas[0].AxisX.TitleFont = new System.Drawing.Font("Arial", 10f);
            chart.ChartAreas[0].AxisY.TitleFont = new System.Drawing.Font("Arial", 10f);
            chart.ChartAreas[0].AxisX.LabelStyle.Font = new System.Drawing.Font("Arial", 10f);

            chart.Series.Add("Area of average grade");
            chart.Series[0].ChartType = SeriesChartType.Column;
            chart.Series[0].BorderWidth = 2;
            chart.Series[0].BorderColor = System.Drawing.Color.FromArgb(26, 59, 105);
            chart.Series[0].Points.DataBindXY(x, y);
            string path = "Charts/chart_overall(bar).png";
            chart.SaveImage(Server.MapPath(path), ChartImageFormat.Png);
            chartArray.Add(path);
            #endregion

            #region get overall result via questions

            ArrayList listOfQuestions = dbmanager.GetAllQuestionListWithIDs();
            if (listOfQuestions.Count > 0)
            {
                foreach (Question qu in listOfQuestions)
                {
                    ArrayList listofresultviaquestion = GetChartResultViaQuestion(individualuid, qu.QuestionID);
                    ArrayList x1 = new ArrayList();
                    ArrayList y1 = new ArrayList();

                    foreach (string result in listofresultviaquestion)
                    {
                        string[] resultsplit = result.Split(';');
                        x1.Add(resultsplit[1].ToString());
                        y1.Add(resultsplit[0].ToString());
                    }
                    Chart chart1 = new Chart();
                    chart1.Width = 500;
                    chart1.Height = 500;

                    chart1.Legends.Add("Legend" + qu.QuestionID);
                    chart1.Legends[0].Enabled = true;
                    chart1.Legends[0].Docking = Docking.Bottom;
                    chart1.Legends[0].Alignment = System.Drawing.StringAlignment.Center;
                    string title = "Q" + qu.QuestionID + ") " + qu.QuestionDetails;
                    chart1.Titles.Add(title);
                    chart1.Titles[0].Font = new System.Drawing.Font("Arial", 10f);
                    chart1.Titles[0].Alignment = System.Drawing.ContentAlignment.TopLeft;

                    chart1.ChartAreas.Add("");
                    chart1.ChartAreas[0].AxisX.Title = "Period of feedback";
                    chart1.ChartAreas[0].AxisY.Title = "Average Grade";
                    chart1.ChartAreas[0].AxisX.TitleFont = new System.Drawing.Font("Arial", 10f);
                    chart1.ChartAreas[0].AxisY.TitleFont = new System.Drawing.Font("Arial", 10f);
                    chart1.ChartAreas[0].AxisX.LabelStyle.Font = new System.Drawing.Font("Arial", 10f);

                    chart1.Series.Add("Area of average grade for question " + qu.QuestionID);
                    chart1.Series[0].ChartType = SeriesChartType.Column;
                    chart1.Series[0].BorderWidth = 2;
                    chart1.Series[0].BorderColor = System.Drawing.Color.FromArgb(26, 59, 105);
                    chart1.Series[0].Points.DataBindXY(x1, y1);
                    string path1 = "Charts/chart_Q" + qu.QuestionID + ".png";
                    chart1.SaveImage(Server.MapPath(path1), ChartImageFormat.Png);
                    chartArray.Add(path1);
                }
            }
            #endregion
            Exportchart(chartArray);
        }

        protected void exportExcel_Click(object sender, ImageClickEventArgs e)
        {
            ExportExcel(ViewAllHistory);
        }

        protected void ViewOverallBtn_Click(object sender, EventArgs e)
        {
            if (Session["AverageGrading"] == null)
            {
                BindOverAllSchoolGrid();
                this.ModalPopupExtender.Show();
            }
            else
            {
                if (SearchTbx.Text != "" && Session["SearchClickName"] != null)
                {
                    BindOverAllSchoolGridByLIKEName(SearchTbx.Text);
                    this.ModalPopupExtender.Show();
                }
                else if (ddlFilterSection.SelectedIndex == 0 && ddlFilterFunction.SelectedIndex != 0 && Session["SearchClick"] != null)
                {
                    //filter by function
                    BindOverAllSchoolGridByFunction(ddlFilterFunction.Text);
                    this.ModalPopupExtender.Show();
                }
                else if (ddlFilterSection.Text == "All section" && ddlFilterFunction.SelectedIndex != 0 && Session["SearchClick"] != null)
                {
                    //filter by function
                    BindOverAllSchoolGridByFunction(ddlFilterFunction.Text);
                    this.ModalPopupExtender.Show();
                }
                else if (ddlFilterFunction.SelectedIndex == 0 && ddlFilterSection.SelectedIndex != 0 && Session["SearchClick"] != null)
                {
                    BindOverAllSchoolGridBySection(ddlFilterSection.Text, ddlFilterFunction.Text);
                    this.ModalPopupExtender.Show();
                }
                else if (ddlFilterFunction.Text == "All function" && ddlFilterSection.SelectedIndex != 0 && Session["SearchClick"] != null)
                {
                    BindOverAllSchoolGridBySection(ddlFilterSection.Text, ddlFilterFunction.Text);
                    this.ModalPopupExtender.Show();
                }
                else if (ddlFilterSection.SelectedIndex != 0 && ddlFilterSection.Text != "All section" && ddlFilterFunction.SelectedIndex != 0 && ddlFilterFunction.Text != "All function" && Session["SearchClick"] != null)
                {
                    BindOverAllSchoolGridByBothSectionFunction(ddlFilterSection.Text, ddlFilterFunction.Text);
                    this.ModalPopupExtender.Show();
                }
                else
                {
                    BindOverAllSchoolGrid();
                    this.ModalPopupExtender.Show();
                }
            }
        }

        protected void ViewAllHistory_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
