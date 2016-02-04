using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using _360_Staff_Survey_Web.Class;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace _360_Staff_Survey_Web
{
    public partial class SubmitAppraisal : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            if (Session["Submitted"] != null)
            {
                Response.Redirect("default.aspx");
            }
            NextBtn.Visible = true;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session["Role"] != null)
                {
                    Session["Gridindex"] = 0;
                    string role = Session["Role"].ToString();
                    NextBtn.Visible = false;
                    BindMasterGrid();
                    BindChildGrid();
                    Session["Page" + 0] = true;

                    // pop up panel.. 
                    AppraisalDetails app = dbmanager.GetAppraisalDetails();
                    string details = app.AppraisalDescription;
                    lblDetails.Text = details;
                    this.ModalPopupExtender.Show();
                }
                else
                {
                    Response.Redirect("accessdenied.aspx");
                }
            }
        }

        protected void BackBtnLink_Click(object sender, EventArgs e)
        {
            mainView.ActiveViewIndex = 0;
        }

        protected void NextBtn_Click(object sender, EventArgs e)
        {
            bool validpage = true;
            ArrayList listofqn = (ArrayList)Session["AllQuestion"];

            for (int page = 0; page < listofqn.Count; page++)
            {
                if (Convert.ToBoolean(Session["Page" + page]) == false)
                {
                    validpage = false;
                }
            }
            if (validpage == true)
            {
                bool result = FindAppraisalValues();

                ArrayList listofallappraisal = (ArrayList)Session["AllAppraisal"];

                if (result == true)
                {
                    string table = "";
                    string remarksdisplay = "";
                    int qncount = 1;
                    int num = 1;
                    InformationLbl.Text = " Please preview your grading for staffs you have appraised. Click 'Back' to change your grading.";
                    table += "<div style='height: 600px; width: 100%; overflow: scroll;'>";
                    table += "<table width='90%' border='0' align='left'><tr><td>";
                    foreach (Question qn in listofqn)
                    {
                        table += "<table width='90%' border='1' style='border-color:#000080' cellspacing='0'><tr style='border-color:#000080'><td style='border-color:#000080'>";
                        table += "<b>Q" + qncount + ")" + " " + qn.QuestionDetails;
                        table += "</b></td></tr></table><br></td></tr><tr><td>";
                        qncount++;

                        table += "<table width='90%' border='1' style='border-color:#000080' cellspacing='0'>";
                        table += "<tr style='border-color:#000080' align='left'>";
                        table += "<td style='border-color:#000080' width='5%'><b>No.</b></td><td style='border-color:#000080' width='35%'><b>Name</b></td><td style='border-color:#000080' width='5%'><b>Grading</b></td><td style='border-color:#000080' width='55%'><b>Remarks</b></td></tr>";
                        foreach (staffappraisal stf in listofallappraisal)
                        {
                            if (qn.QuestionID == stf.Questionid)
                            {
                                if (stf.Appremarks == "")
                                {
                                    remarksdisplay = "NIL";
                                }
                                else
                                {
                                    remarksdisplay = stf.Appremarks;
                                }
                                table += "<tr style='border-color:#000080' align='left'>";
                                table += "<td width='5%' align='left' style='border-color:#000080'>" + num + "</td>";

                                string staffname = dbmanager.GetNameViaUserID(stf.Appstaffuid);
                                table += "<td width='35%' align='left' style='border-color:#000080'>" + staffname + "</td>";
                                string appresult = "";
                                if (stf.Appresult.ToString() == "1.1")
                                {
                                    appresult = "N/A";
                                }
                                else
                                {
                                    appresult = stf.Appresult.ToString();
                                }
                                table += "<td width='5%' align='left' style='border-color:#000080'>" + appresult + "</td>";
                                table += "<td width='55%' align='left' style='border-color:#000080'>" + remarksdisplay + "</td></tr>";
                                num++;
                            }
                        }
                        table += "</table><br>";
                        num = 1;
                    }
                    table += "</td></tr></table>";
                    table += "</div>";
                    SummaryLbl.Text = table;
                    Session["AppraisalReport"] = table;
                    mainView.ActiveViewIndex = 1;
                }
            }
            else
            {
                MessageBoxShow("Please review all questions before going next.");
            }
        }

        private void MessageBoxShow(string message)
        {
            string strScript = "<script>";
            strScript += "alert('" + message + "');";
            strScript += "</script>";
            this.ClientScript.RegisterStartupScript(this.GetType(), "Startup", strScript);
        }

        private void MessageBoxShowReturnHome(string message)
        {
            string strScript = "<script>";
            strScript += "alert('" + message + "');";
            strScript += "window.location='default.aspx';";
            strScript += "</script>";
            this.ClientScript.RegisterStartupScript(this.GetType(), "Startup", strScript);
        }

        private ArrayList FindValues(Control parent)
        {
            ArrayList al = new ArrayList();
            foreach (Control c in parent.Controls)
            {
                if ((c.Controls.Count > 0))
                {
                    ArrayList arr = FindValues(c);
                    for (int i = 0; i < arr.Count; i++)
                    {
                        al.Add(arr[i].ToString());
                    }
                }
                if (c.GetType().ToString().Equals("System.Web.UI.WebControls.TextBox"))
                {
                    al.Add(((TextBox)c).Text.Trim());
                }
                else if (c.GetType().ToString().Equals("System.Web.UI.WebControls.RadioButtonList"))
                {
                    if (((RadioButtonList)c).SelectedValue == "")
                    {
                        al.Add("0.0");
                    }
                    else if (((RadioButtonList)c).SelectedValue == "N/A")
                    {
                        al.Add("N/A");
                    }
                    else
                    {
                        string value = ((RadioButtonList)c).SelectedValue;
                        al.Add(Convert.ToDouble(value));
                    }
                }
            }
            return al;
        }

        protected void SubmitAppraisalGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            SavedAppraisalValues();
            bool result = FindAppraisalValues();
            if (result == true)
            {
                SubmitAppraisalGrid.PageIndex = e.NewPageIndex;
                Session["Gridindex"] = e.NewPageIndex;
                Session["Page" + SubmitAppraisalGrid.PageIndex] = true;

                if (SubmitAppraisalGrid.PageCount - 1 == e.NewPageIndex)
                {
                    NextBtn.Visible = true;
                }
                else
                {
                    NextBtn.Visible = false;
                }
                BindMasterGrid();
                BindChildGrid();
            }
        }

        protected bool FindAppraisalValues()
        {
            int count = 0;
            int index = 0;
            bool result = false;
            ArrayList list = FindValues(this.Page);
            ArrayList listofqn = (ArrayList)Session["AllQuestion"];
            ArrayList listofstf = (ArrayList)Session["AllStaff"];
            ArrayList listofallappraisal = new ArrayList();
            ArrayList listofunans = new ArrayList();
            int countpage = 1;
            foreach (Question qn in listofqn)
            {
                if (countpage == SubmitAppraisalGrid.PageIndex + 1)
                {
                    for (int i = 0; i < list.Count; )
                    {
                        string rate = list[i].ToString();
                        int num = (i / 2) + 1;

                        if (rate == "0.0")
                        {
                            count++;
                        }
                        if (rate == "N/A")
                        {
                            rate = "1.1";
                        }
                        string remarks = list[i + 1].ToString().Trim();

                        string staff = listofstf[index].ToString();
                        string uid = Session["UserID"].ToString();
                        DateTime todaydate = DateTime.Today;
                        DateTime systemend = (DateTime)Session["EndTime"];

                        staffappraisal stfapp = new staffappraisal(uid, qn.QuestionID, staff, Convert.ToDouble(rate), remarks, todaydate, systemend);

                        if (Session["AllAppraisal"] != null)
                        {
                            ArrayList listtodelete = new ArrayList();
                            listofallappraisal = (ArrayList)Session["AllAppraisal"];

                            foreach (staffappraisal allstaf in listofallappraisal)
                            {
                                if (stfapp.Questionid == allstaf.Questionid)
                                {
                                    if (stfapp.Appstaffuid == allstaf.Appstaffuid)
                                    {
                                        listtodelete.Add(allstaf);
                                        //remove past data to prevent stack
                                    }
                                }
                            }
                            //delete all past data
                            foreach (staffappraisal deletestf in listtodelete)
                            {
                                listofallappraisal.Remove(deletestf);
                            }
                            listofallappraisal.Add(stfapp);
                        }
                        else
                        {
                            listofallappraisal.Add(stfapp);
                        }
                        i++;
                        i++;
                        index++;
                    }
                }
                countpage++;
            }
            if (count == 0)
            {
                result = true;
                Session["AllAppraisal"] = listofallappraisal;
                ViewState["answers" + SubmitAppraisalGrid.PageIndex + 1] = list;
            }
            return result;
        }

        protected void SubmitBtn_Click(object sender, EventArgs e)
        {
            if (Session["AllAppraisal"] != null)
            {
                ArrayList listofapplist = (ArrayList)Session["AllAppraisal"];
                int totaladded = dbmanager.InsertAllAppraisalAnswers(listofapplist);

                //int totaladded2 = dbmanager.InsertStaffQuestionGrade(listofapplist); // for question rate filter 0.0

                //if (totaladded == listofapplist.Count && totaladded2 >=0)
                if (totaladded == listofapplist.Count)
                {
                    
                    dbmanager.DeletePreviousSaved(Session["UserID"].ToString());

                    string y = Session["UserID"].ToString() + "@hotmail.com";

                    string passw = "";

                    Systemtime st = dbmanager.GetSystemTime();
                    string enddate = "";
                    if (st != null)
                    {

                        enddate = st.Enddate.ToLongDateString();
                    }

                    if (Session["UserID"].ToString() != "")
                    {



                        bool sentemail = email.SendMail("Dear Staff,<br/><br/>" + Environment.NewLine + Environment.NewLine + "The system has received your 360 Survey feedback.<br/><br/>" + Environment.NewLine + Environment.NewLine + "Thank you for your participation<br/><br/>" + Environment.NewLine + Environment.NewLine + "Together, let us make ASC a school of choice for staff, and a better place to be for all.", y, "Sucessfully completed 360 Staff Feedback survey.");


                        if (sentemail == true)
                        {
                            MessageBoxShowReturnHome("Appraisal submitted successfully. An email has been sent to confirm you have submitted successfully");

                        }

                        else
                        {
                            MessageBoxShow("System Error. One of the emails may not be valid.");
                        }
                    }
                }
                else
                {
                    string uid = Session["UserID"].ToString();
                    dbmanager.DeleteAllAppraisalAnswerByUserId(uid);
                    MessageBoxShowReturnHome("Fail to submit appraisal.");
                }
            }
        }

        protected void BindMasterGrid()
        {
            mainView.ActiveViewIndex = 0;

            ArrayList listofquestion = dbmanager.GetAllQuestionForAppraisal();
            if (Session["AllQuestion"] == null)
            {
                Session["AllQuestion"] = listofquestion;
            }

            if (listofquestion.Count != 0)
            {
                if (listofquestion.Count == 1)
                {
                    NextBtn.Visible = true;
                }
                int index = 1;
                DataTable dt = new DataTable();
                DataRow dr = null;
                dt.Columns.Add(new DataColumn("Question", typeof(string)));

                foreach (Question ques in listofquestion)
                {
                    dr = dt.NewRow();
                    dr["Question"] = "<h1><b>Q" + index + ")</b>  " + ques.QuestionDetails + "</h1>";
                    dt.Rows.Add(dr);
                    index++;
                }
                if (ViewState["QuestionTable"] == null)
                {
                    ViewState["QuestionTable"] = dt;
                    Session["dt"] = (DataTable)dt;
                }
                //before bind
                SubmitAppraisalGrid.DataSource = dt;
                SubmitAppraisalGrid.DataBind();
            }
        }

        protected void BindChildGrid()
        {
            for (int i = 0; i < SubmitAppraisalGrid.Rows.Count; i++)
            {
                string userid = Session["UserID"].ToString();
                ArrayList listofstaffuid = dbmanager.GetAllStaffUserID(userid);
                Session["AllStaff"] = listofstaffuid;

                if (listofstaffuid.Count != 0)
                {
                    DataTable dt = null;
                    if (ViewState["QuestionTable"] != null)
                    {
                        dt = (DataTable)ViewState["QuestionTable"];
                    }
                    else
                    {
                        dt = (DataTable)Session["dt"];
                    }
                    //staffgrid
                    int index = 0;
                    for (int p = 0; p < dt.Rows.Count; p++)
                    {
                        #region populate the grid
                        DataTable staffdt = new DataTable();
                        DataRow staffdr = null;
                        staffdt.Columns.Add(new DataColumn("No", typeof(int)));
                        staffdt.Columns.Add(new DataColumn("StaffName", typeof(string)));
                        staffdt.Columns.Add(new DataColumn("RadioList", typeof(ArrayList)));
                        staffdt.Columns.Add(new DataColumn("RemarkTbx", typeof(string)));
                        ArrayList listofradio = dbmanager.GetAllChoice();
                        listofradio.Remove("N/A");
                        listofradio.Add("N/A");
                        if (listofradio.Count != 0)
                        {
                            string legend = "";
                            string legend2 = "";
                            legend2 += "<b>Note </b> :";
                            legend2 += "<br> - <b>Complete within the given time</b> before session expired.";
                            legend2 += "<br> - Special characters are not <b>allowed.</b>";
                            //legend2 += "<br> - <b>Click</b> on staff's name to view photo.</b>";
                            legend2 += "<br> - <b>Click</b> on 'Rate' for more details.";
                            legend2 += "<br> - <b>Click</b> ";

                            legend += " to view the opening message. <br><br><b>How to rate</b><br>";
                            legend += "- Highest rate is " + listofradio[0].ToString() + " ";
                            legend += ", Lowest rate is " + listofradio[listofradio.Count - 2].ToString() + " ";

                            if (listofradio[listofradio.Count - 1].ToString().Equals("N/A"))
                            {
                                legend += "and " + listofradio[listofradio.Count - 1].ToString() + " for <b>Not Applicable.</b>";
                            }
                            else
                            {
                                legend += "<br>";
                            }
                            lblLegend.Text = legend;
                            lblLegend2.Text = legend2;
                        }
                        int rownum = 1;

                        foreach (string staffuid in listofstaffuid)
                        {
                            staffdr = staffdt.NewRow();
                            staffdr["No"] = rownum;
                            string staffname = dbmanager.GetNameViaUserID(staffuid);
                            staffdr["StaffName"] = staffname;
                            staffdr["RadioList"] = listofradio;
                            staffdr["RemarkTbx"] = string.Empty;
                            staffdt.Rows.Add(staffdr);
                            rownum++;
                        }

                        ViewState["QuestionTable2"] = staffdt;
                        GridView gv = (GridView)SubmitAppraisalGrid.Rows[i].FindControl("StaffAppraisalGrid");
                        gv.DataSource = staffdt;
                        gv.DataBind();

                        GridView grid = (GridView)SubmitAppraisalGrid.Rows[i].FindControl("StaffAppraisalGrid");
                        if (grid != null)
                        {
                            for (int g = 0; g < grid.Rows.Count; g++)
                            {
                                RadioButtonList bblist = (RadioButtonList)grid.Rows[g].Cells[3].FindControl("RadioList");
                                bblist.SelectedValue = "N/A";
                            }
                        }

                        ArrayList listofanswer = (ArrayList)ViewState["answers" + SubmitAppraisalGrid.PageIndex + 1];

                        if (listofanswer != null)
                        {
                            GridView gve = (GridView)SubmitAppraisalGrid.Rows[i].FindControl("StaffAppraisalGrid");
                            int counter = 0;
                            int countertbx = 1;
                            for (int g = 0; g < gve.Rows.Count; g++)
                            {
                                RadioButtonList bblist = (RadioButtonList)gve.Rows[g].Cells[3].FindControl("RadioList");
                                TextBox tbx = (TextBox)gve.Rows[g].Cells[3].FindControl("RemarksTbx");
                                tbx.Text = listofanswer[countertbx].ToString();
                                if (listofanswer[counter].ToString() == "1.1")
                                {
                                    bblist.SelectedValue = "N/A";
                                }
                                else
                                {
                                    bblist.SelectedValue = listofanswer[counter].ToString();
                                }
                                counter++;
                                counter++;
                                countertbx++;
                                countertbx++;
                            }
                        }
                        #endregion
                        Label lbl = (Label)SubmitAppraisalGrid.Rows[i].FindControl("QuestionLbl");
                        string question = lbl.Text;
                        lblQuestion.Text = question;

                        int qid = Convert.ToInt32(question.Substring(8, 1));
                        Session["QuestionID"] = qid;

                        //get saved items
                        ArrayList listofsavedanswer = (ArrayList)dbmanager.GetAllSavedAppraisal(userid, qid);
                        if (listofsavedanswer != null)
                        {
                            try
                            {
                                GridView gve = (GridView)SubmitAppraisalGrid.Rows[i].FindControl("StaffAppraisalGrid");
                                for (int j = 0; j < gve.Rows.Count; j++)
                                {
                                    foreach (staffappraisal stf in listofsavedanswer)
                                    {
                                        HyperLink myLink = (HyperLink)gve.Rows[j].Cells[1].Controls[0];
                                        string name = myLink.Text;
                                        string value = HttpUtility.HtmlDecode(name);
                                        string staffname = dbmanager.GetUserIDViaName(value);
                                        if (staffname == stf.Appstaffuid)
                                        {
                                            RadioButtonList bblist = (RadioButtonList)gve.Rows[j].Cells[3].FindControl("RadioList");
                                            TextBox tbx = (TextBox)gve.Rows[j].Cells[3].FindControl("RemarksTbx");
                                            tbx.Text = stf.Appremarks;
                                            if (stf.Appresult.ToString() == "1.1")
                                            {
                                                bblist.SelectedValue = "N/A";
                                            }
                                            else
                                            {
                                                bblist.SelectedValue = stf.Appresult.ToString();
                                            }
                                        }
                                    }
                                }
                            }
                            catch
                            {
                            }
                        }
                        index++;
                    }
                }
                else
                {
                    if (Session["Role"] != null)
                    {
                        string role = Session["Role"].ToString();

                        if (role == "Admin")
                        {
                            //Response.Redirect("~/Adminmain.aspx?emp");
                            Response.Redirect("~/Adminmain.aspx");
                        }
                        else
                        {
                            //Response.Redirect("~/Staffmain.aspx?emp");
                            Response.Redirect("~/Staffmain.aspx");
                        }
                    }
                }
            }
        }

        protected bool SavedAppraisalValues()
        {
            bool result = false;
            try
            {
                int qid = SubmitAppraisalGrid.PageIndex + 1;
                ArrayList saveAppraisal = new ArrayList();
                string uid = Session["UserID"].ToString();
                DateTime systemend = (DateTime)Session["EndTime"];
                DateTime todaydate = DateTime.Today;

                for (int i = 0; i < SubmitAppraisalGrid.Rows.Count; i++)
                {
                    GridView gve = (GridView)SubmitAppraisalGrid.Rows[i].FindControl("StaffAppraisalGrid"); // for grid
                    if (gve != null)
                    {
                        for (int j = 0; j < gve.Rows.Count; j++)
                        {
                            RadioButtonList bblist = (RadioButtonList)gve.Rows[j].Cells[2].FindControl("RadioList");
                            TextBox tbx = (TextBox)gve.Rows[j].Cells[3].FindControl("RemarksTbx");

                            ArrayList staffList = (ArrayList)Session["AllStaff"];
                            string staff = staffList[j].ToString();
                            string rate = bblist.SelectedValue;
                            double rate2;
                            if (rate.Equals("N/A"))
                            {
                                rate2 = 1.1;
                            }
                            else
                            {
                                rate2 = Convert.ToDouble(rate);
                            }
                            string remarks = tbx.Text;

                            staffappraisal stfapp = new staffappraisal(uid, qid, staff, Convert.ToDouble(rate2), remarks, todaydate, systemend);
                            saveAppraisal.Add(stfapp);
                        }
                    }
                    int count = 0;
                }

                //check if there is exist and save in database
                bool exist = dbmanager.CheckAppraisalSave(uid, qid, systemend);
                if (exist == true)
                {
                    dbmanager.DeleteAppraisalSaved(uid, qid, systemend);
                }
                dbmanager.InsertSavedData(saveAppraisal);
                result = true;
            }
            catch
            {
                result = false;
            }





            return result;
        }

        protected void SaveLtrBtn_Click(object sender, EventArgs e)
        {
            bool result = SavedAppraisalValues();
            if (result == true)
            {
                MessageBoxShow("Feedback Saved!");
            }
            else
            {
                MessageBoxShowReturnHome("Unable to save to draft.");
            }

        }

        protected void StaffAppraisalGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                int rowIndex = int.Parse(e.CommandArgument.ToString());

                GridView gve = (GridView)SubmitAppraisalGrid.Rows[0].FindControl("StaffAppraisalGrid");
                string num = gve.Rows[rowIndex].Cells[0].Text;
                int number = Convert.ToInt32(num);

                string uid = dbmanager.GetUserIDViaImgRowID(number);
                string url = dbmanager.GetImageViaUserID(uid);

                if (url != "")
                {
                    Response.Write("<script>location.href='" + url + "'</script>");
                }
                //else
                //{
                //    MessageBoxShow("No image found for this user.");
                //}
            }
        }

        protected void StaffAppraisalGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HyperLink myLink = (HyperLink)e.Row.Cells[1].Controls[0];

                string uid = dbmanager.GetUserIDViaName(myLink.Text);
                string url = dbmanager.GetImageViaUserID(uid);

                if (url != "")
                {
                    myLink.NavigateUrl = url;
                    myLink.ForeColor = System.Drawing.Color.Blue;
                }
            }
        }

        protected void ViewBtn_Click(object sender, EventArgs e)
        {
            AppraisalDetails app = dbmanager.GetAppraisalDetails();
            string details = app.AppraisalDescription;
            lblDetails.Text = details;
            this.ModalPopupExtender.Show();
        }

        protected void wordExportIndividual_Click(object sender, ImageClickEventArgs e)
        {
            ExportWord(Session["AppraisalReport"].ToString());
        }

        protected void ExportWord(string report)
        {
            HttpContext.Current.Response.Clear();
            System.Web.HttpContext curContext = System.Web.HttpContext.Current;

            DateTime date = DateTime.Today.Date;
            string filename = "Appraisal_Report (" + date.Day + "-" + date.Month + "-" + date.Year + ").doc";
            //curContext.Response.AddHeader("content-disposition", "attachment; filename=" + System.Web.HttpUtility.UrlEncode("Appraisal_Report (" + System.DateTime.Today.Date.ToShortDateString() + ").doc"));
            curContext.Response.AddHeader("content-disposition", "attachment; filename=" + System.Web.HttpUtility.UrlEncode(filename));
            HttpContext.Current.Response.ContentType = "application/ms-word";

            StringBuilder strHTMLContent = new StringBuilder();
            strHTMLContent.Append(report);

            HttpContext.Current.Response.Write(strHTMLContent);
            HttpContext.Current.Response.End();
            HttpContext.Current.Response.Flush();
        }
    }
}