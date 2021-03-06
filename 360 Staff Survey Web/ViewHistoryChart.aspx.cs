﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using _360_Staff_Survey_Web.Class;
using System.Collections;
using System.Web.UI.DataVisualization.Charting;
using System.IO;
using System.Data;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Drawing.Spreadsheet;
using DocumentFormat.OpenXml.Drawing;
using System.Xml;
using System.Windows.Media.Imaging;
using Xdr = DocumentFormat.OpenXml.Drawing.Spreadsheet;
using A = DocumentFormat.OpenXml.Drawing;


namespace _360_Staff_Survey_Web
{
    public partial class ViewHistoryChart : System.Web.UI.Page
    {
        public static string lbl;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session["Role"] != null)
                {
                    string role = Session["Role"].ToString();
                    if (role == "User")
                    {
                        Response.Redirect("accessdenied.aspx");
                    }
                    else
                    {
                        #region for Section Group Label
                        lbl = "select question:";
                        lblSelectQuestion.Text = lbl;
                        lblSelectSection.Text = "select section group (chart title): ";
                        lblFilterByFunction.Text = "select function (chart components): ";
                        lblSelectedFunction.Text = "display function(s) chart by: ";
                        #endregion

                        bool checkifappraisalsec = dbmanager.CountAllAppraisal();
                        if (checkifappraisalsec == true)
                        {
                            if (role == "Admin")
                            {
                                BindDropDownListAdmin();
                            }
                            else if (role == "Officer")
                            {
                                BindDropDownListOfficer();
                            }
                            else if (role == "Director")
                            {
                                BindDropDownListDirector();
                            }
                            LegendMessage();
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
        protected void LegendMessage()
        {
            string legend = "";
            legend += "<table align='left'><tr><td>";
            legend += "<b>Legend on Viewing Feedback Chart</b><br><br>";
            legend += "A: Average, S: Standard Deviation, M: Median<br><br>";
            legend += "The values above the bar indicate the A, S, & M for the section, function & question chosen.<br><br>";
            legend += "The values below the feedback period date indicate the A, S, & M for the period, regardless of section and function. <br><br>";
            legend += "Please note that the format of date shown is: MONTH/YEAR.<br>";
            legend += "</td></tr></table>";
            lbllegendHistory.Text = legend;
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
        protected void BindDropDownListAdmin()
        {
            try
            {
                ArrayList listofsection = RemoveDups(dbmanager.GetAllSectionnByLimit());
                ArrayList listoffunction = dbmanager.GetAllFunctionByLimit();
                ArrayList listofquestion = dbmanager.GetAllQuestionList();

                ArrayList listofselectfunction = new ArrayList();
                ArrayList listofselectsection = new ArrayList();
                ArrayList listofselectquestion = new ArrayList();
                if (listoffunction.Count > 0 && listofsection.Count > 0 && listofquestion.Count > 0)
                {
                    #region for section group
                    listofsection.Insert(0, "<----Please select one---->");
                    listofsection.Insert(1, "<--------ASC School-------->");
                    ddlSelectSection.DataSource = listofsection;
                    ddlSelectSection.DataBind();

                    listofquestion.Insert(0, "<--------All question(s)------->");
                    ddlSelectQuestion.DataSource = listofquestion;
                    ddlSelectQuestion.DataBind();

                    listofselectfunction.Insert(0, "<--------Autoupdate-------->");
                    ddlFilterFunction.DataSource = listofselectfunction;
                    ddlFilterFunction.DataBind();
                    #endregion
                    #region for function group
                    listoffunction.Insert(0, "<----Please select one---->");
                    listoffunction.Insert(1, "<--------ASC School-------->");
                    ddlSelectFunction.DataSource = listoffunction;
                    ddlSelectFunction.DataBind();

                    //listofquestion.Insert(0, "<---------All question-------->");
                    ddlFilterQuestion.DataSource = listofquestion;
                    ddlFilterQuestion.DataBind();

                    listofselectsection.Insert(0, "<--------Autoupdate-------->");
                    ddlFilterBySection.DataSource = listofselectsection;
                    ddlFilterBySection.DataBind();
                    SearchPanelFunctionViaSection.Visible = false;
                    #endregion
                }
                else
                {
                    MessageBoxShow("Error reading data from database.");
                }
            }
            catch
            {
            }
        }
        protected void BindDropDownListOfficer()
        {
            try
            {
                string uid = Session["UserID"].ToString();
                staffinfo stfinfo = dbmanager.GetStaffDetailsViaUid(uid);
                //if director
                if (stfinfo.Section == "ALL")
                {
                    ArrayList listofsection = RemoveDups(dbmanager.GetAllSectionnByLimit());
                    ArrayList listoffunction = dbmanager.GetAllFunctionByLimit();
                    // ArrayList listofquestion = dbmanager.GetAllQuestionList();
                    ArrayList questionall = new ArrayList();
                    ArrayList listofselectfunction = new ArrayList();
                    ArrayList listofselectsection = new ArrayList();

                    if (listoffunction.Count > 0 && listofsection.Count > 0)
                    {
                        listofsection.Insert(0, "<----Please select one---->");
                        listofsection.Insert(1, "<--------ASC School-------->");
                        ddlSelectSection.DataSource = listofsection;
                        ddlSelectSection.DataBind();
                        //for ddlselectQuestion
                        // listofquestion.Insert(0, "<--------All question(s)------->");
                        // ddlSelectQuestion.DataSource = listofquestion;
                        questionall.Insert(0, "<--------All question(s)------->");
                        ddlSelectQuestion.DataSource = questionall;
                        ddlSelectQuestion.DataBind();
                        // ddlSelectQuestion.Items.Insert(0, new ListItem("<--------All question(s)------->"));
                        // ddlSelectQuestion.DataBind();                  

                        listofselectfunction.Insert(0, "<--------Autoupdate-------->");
                        ddlFilterFunction.DataSource = listofselectfunction;
                        ddlFilterFunction.DataBind();
                        listoffunction.Insert(0, "<----Please select one---->");
                        listoffunction.Insert(1, "<--------ASC School-------->");
                        ddlSelectFunction.DataSource = listoffunction;
                        ddlSelectFunction.DataBind();
                        listofselectsection.Insert(0, "<--------Autoupdate-------->");
                        ddlFilterBySection.DataSource = listofselectsection;
                        ddlFilterBySection.DataBind();
                        // listofquestion.Insert(0, "<--------All question(s)------->");
                        // ddlFilterQuestion.DataSource = listofquestion;
                        // ddlSelectQuestion.Items.Insert(0, new ListItem("<--------All question(s)------->"));
                        // ddlFilterQuestion.DataBind();
                        questionall.Insert(0, "<--------All question(s)------->");
                        ddlSelectQuestion.DataSource = questionall;
                        ddlSelectQuestion.DataBind();
                        SearchPanelFunctionViaSection.Visible = false;
                    }
                    else
                    {
                        MessageBoxShow("Error reading data from database.");
                    }
                }
                else
                {
                    ArrayList listofsection = new ArrayList();
                    string[] arraysection = stfinfo.Section.Split(',');
                    if (arraysection.LongLength > 0)
                    {
                        foreach (string sect in arraysection)
                        {
                            listofsection.Add(sect);
                        }
                    }
                    ArrayList listoffunction = RemoveDups(dbmanager.GetAllFunctionByLimitViaOfficer(stfinfo.Section));
                    // ArrayList listofquestion = dbmanager.GetAllQuestionList();
                    ArrayList questionall = new ArrayList();
                    ArrayList listofselectfunction = new ArrayList();
                    ArrayList listofselectsection = new ArrayList();
                    if (listoffunction.Count > 0 && listofsection.Count > 0)
                    {
                        listofsection.Insert(0, "<----Please select one---->");
                        ddlSelectSection.DataSource = listofsection;
                        ddlSelectSection.DataBind();
                        // for ddlselectQuestion
                        // listofquestion.Insert(0, "<--------All question(s)------->");
                        // ddlSelectQuestion.DataSource = listofquestion;
                        // ddlSelectQuestion.Items.Insert(0, new ListItem("<--------All question(s)------->"));
                        // ddlSelectQuestion.DataBind();
                        questionall.Insert(0, "<--------All question(s)------->");
                        ddlSelectQuestion.DataSource = questionall;
                        ddlSelectQuestion.DataBind();

                        listofselectfunction.Insert(0, "<--------Autoupdate-------->");
                        ddlFilterFunction.DataSource = listofselectfunction;
                        ddlFilterFunction.DataBind();
                        listoffunction.Insert(0, "<----Please select one---->");
                        ddlSelectFunction.DataSource = listoffunction;
                        ddlSelectFunction.DataBind();
                        listofselectsection.Insert(0, "<--------Autoupdate-------->");
                        ddlFilterBySection.DataSource = listofselectsection;
                        ddlFilterBySection.DataBind();
                        // listofquestion.Insert(0, "<--------All question(s)------->");
                        // ddlFilterQuestion.DataSource = listofquestion;
                        // ddlSelectQuestion.Items.Insert(0, new ListItem("<--------All question(s)------->"));
                        // ddlFilterQuestion.DataBind();
                        questionall.Insert(0, "<--------All question(s)------->");
                        ddlSelectQuestion.DataSource = questionall;
                        ddlSelectQuestion.DataBind();
                        SearchPanelFunctionViaSection.Visible = false;
                    }
                    else
                    {
                        MessageBoxShow("Error reading data from database.");
                    }
                }
            }
            catch
            {
            }
        }
        protected void BindDropDownListDirector()
        {
            try
            {
                string uid = Session["UserID"].ToString();
                staffinfo stfinfo = dbmanager.GetStaffDetailsViaUid(uid);
                //if director
                if (stfinfo.Section == "ALL")
                {
                    ArrayList listofsection = RemoveDups(dbmanager.GetAllSectionnByLimit());
                    ArrayList listoffunction = dbmanager.GetAllFunctionByLimit();
                    ArrayList listofquestion = dbmanager.GetAllQuestionList();

                    ArrayList listofselectfunction = new ArrayList();
                    ArrayList listofselectsection = new ArrayList();
                    if (listoffunction.Count > 0 && listofsection.Count > 0 && listofquestion.Count > 0)
                    {
                        listofsection.Insert(0, "<----Please select one---->");
                        listofsection.Insert(1, "<--------ASC School-------->");
                        ddlSelectSection.DataSource = listofsection;
                        ddlSelectSection.DataBind();
                        //for ddlselectQuestion
                        listofquestion.Insert(0, "<--------All question(s)------->");
                        ddlSelectQuestion.DataSource = listofquestion;
                        ddlSelectQuestion.DataBind();

                        listofselectfunction.Insert(0, "<--------Autoupdate-------->");
                        ddlFilterFunction.DataSource = listofselectfunction;
                        ddlFilterFunction.DataBind();
                        listoffunction.Insert(0, "<----Please select one---->");
                        listoffunction.Insert(1, "<--------ASC School-------->");
                        ddlSelectFunction.DataSource = listoffunction;
                        ddlSelectFunction.DataBind();
                        listofselectsection.Insert(0, "<--------Autoupdate-------->");
                        ddlFilterBySection.DataSource = listofselectsection;
                        ddlFilterBySection.DataBind();
                        listofquestion.Insert(0, "<--------All question(s)------->");
                        ddlFilterQuestion.DataSource = listofquestion;
                        ddlFilterQuestion.DataBind();
                        SearchPanelFunctionViaSection.Visible = false;
                    }
                    else
                    {
                        MessageBoxShow("Error reading data from database.");
                    }
                }
                else
                {
                    ArrayList listofsection = new ArrayList();
                    string[] arraysection = stfinfo.Section.Split(',');
                    if (arraysection.LongLength > 0)
                    {
                        foreach (string sect in arraysection)
                        {
                            listofsection.Add(sect);
                        }
                    }
                    ArrayList listoffunction = RemoveDups(dbmanager.GetAllFunctionByLimitViaOfficer(stfinfo.Section));
                    ArrayList listofquestion = dbmanager.GetAllQuestionList();

                    ArrayList listofselectfunction = new ArrayList();
                    ArrayList listofselectsection = new ArrayList();
                    if (listoffunction.Count > 0 && listofsection.Count > 0 && listofquestion.Count > 0)
                    {
                        listofsection.Insert(0, "<----Please select one---->");
                        ddlSelectSection.DataSource = listofsection;
                        ddlSelectSection.DataBind();
                        // for ddlselectQuestion
                        listofquestion.Insert(0, "<--------All question(s)------->");
                        ddlSelectQuestion.DataSource = listofquestion;
                        ddlSelectQuestion.DataBind();

                        listofselectfunction.Insert(0, "<--------Autoupdate-------->");
                        ddlFilterFunction.DataSource = listofselectfunction;
                        ddlFilterFunction.DataBind();
                        listoffunction.Insert(0, "<----Please select one---->");
                        ddlSelectFunction.DataSource = listoffunction;
                        ddlSelectFunction.DataBind();
                        listofselectsection.Insert(0, "<--------Autoupdate-------->");
                        ddlFilterBySection.DataSource = listofselectsection;
                        ddlFilterBySection.DataBind();
                        listofquestion.Insert(0, "<--------All question(s)------->");
                        ddlFilterQuestion.DataSource = listofquestion;
                        ddlFilterQuestion.DataBind();
                        SearchPanelFunctionViaSection.Visible = false;
                    }
                    else
                    {
                        MessageBoxShow("Error reading data from database.");
                    }
                }
            }
            catch
            {
            }
        }
        private void MessageBoxShow(string message)
        {
            string strScript = "<script>";
            strScript += "alert('" + message + "');";
            strScript += "window.location='default.aspx';";
            strScript += "</script>";
            this.ClientScript.RegisterStartupScript(this.GetType(), "Startup", strScript);
        }
        private void MessageBox(string message)
        {
            string strScript = "<script>";
            strScript += "alert('" + message + "');";
            //strcript += "window.location='default.aspx';";
            strScript += "</script>";
            this.ClientScript.RegisterStartupScript(this.GetType(), "Startup", strScript);
        }
        private void MessageBoxShowWithoutredirect(string message)
        {
            string strScript = "<script>";
            strScript += "alert('" + message + "');";
            strScript += "</script>";
            this.ClientScript.RegisterStartupScript(this.GetType(), "Startup", strScript);
        }
        protected void SearchBtn_Click(object sender, EventArgs e)
        {
            SelectedSect.Visible = true;
            SelectedSect.Text = "Selected section: " + ddlSelectSection.Text;
            SelectedQues.Visible = true;
            SelectedQues.Text = "Selected question: " + ddlSelectQuestion.Text;
            SelectedFunc.Visible = true;
            SelectedFunc.Text = "Selected function: " + ddlFilterFunction.Text;

            if (SelectTbxFunction.Text == "")
            {
                MessageBox("Please click add button to insert function(s)!");
            }
            else
            {
                MultiView1.Visible = false;
                if (ddlSelectSection.Text == "<--------ASC School-------->")
                {
                    ddlFilterFunction.Items.Clear();
                    ddlFilterFunction.Items.Add("<--------Autoupdate-------->");
                    ddlFilterFunction.SelectedIndex = 0;
                    populateChartSectionViaFunctionALL();
                }
                else
                {
                    // for question ddl ()
                    int questionID = ddlSelectQuestion.SelectedIndex;
                    string section = ddlSelectSection.Text.Trim();
                    string function = SelectTbxFunction.Text.Trim();
                    string allfunction = "";
                    int counter = 1;

                    if (function.Length > 0)
                    {
                        if (function == "All function")
                        {
                            ArrayList listofitems = new ArrayList();
                            foreach (ListItem funct in ddlFilterFunction.Items)
                            {
                                if (funct.Text != "All function" && funct.Text != "<----Please select one---->")
                                {
                                    listofitems.Add(funct);
                                }
                            }
                            foreach (ListItem functitem in listofitems)
                            {
                                if (counter == listofitems.Count)
                                {
                                    allfunction += functitem.Text;
                                }
                                else
                                {
                                    allfunction += functitem.Text + ",";
                                }
                                counter++;
                            }
                            populateChartFunctionViaSection(section, allfunction, questionID);
                        }
                        else
                        {
                            populateChartFunctionViaSection(section, function, questionID);
                        }
                    }
                }
                ClearText();
            }
        }
        protected List<double> GetIndividualStandardDeviation(string role)
        {
            List<double> standarddeviationfinal = new List<double>();
            try
            {
                ArrayList listofchoice = dbmanager.GetAllChoice();

                listofchoice.Remove("N/A");
                listofchoice.Add("N/A");

                string last = listofchoice[listofchoice.Count - 1].ToString();
                double final = 0.00;

                ArrayList standarddevlist = new ArrayList();
                ArrayList results = dbmanager.GetAllStaffAppraisal();
                for (int i = 0; i < results.Count; i++)
                {
                    final = final + Convert.ToDouble(results[i].ToString());
                }
                double average = 0.00;
                average = final / results.Count;

                double squareroot = 0.00;
                double equation = 0.00;
                double square = 0.00;
                for (int q = 0; q < results.Count; q++)
                {
                    equation = Convert.ToDouble(results[q].ToString()) - average;
                    square = equation * equation;
                    squareroot = squareroot + square;
                }
                double finalequation = squareroot / results.Count;
                //standarddeviationfinal = Math.Sqrt(finalequation);
                return standarddeviationfinal;
            }
            catch
            {
                return standarddeviationfinal;
            }
        }
        protected double GetOverallStandardDeviation()
        {
            double standarddeviationfinal = 0.00;
            try
            {
                ArrayList listofchoice = dbmanager.GetAllChoice();

                listofchoice.Remove("N/A");
                listofchoice.Add("N/A");

                string last = listofchoice[listofchoice.Count - 1].ToString();
                double final = 0.00;

                ArrayList standarddevlist = new ArrayList();
                ArrayList results = dbmanager.GetAllStaffAppraisal();
                for (int i = 0; i < results.Count; i++)
                {
                    final = final + Convert.ToDouble(results[i].ToString());
                }
                double average = 0.00;
                average = final / results.Count;

                double squareroot = 0.00;
                double equation = 0.00;
                double square = 0.00;
                for (int q = 0; q < results.Count; q++)
                {
                    equation = Convert.ToDouble(results[q].ToString()) - average;
                    square = equation * equation;
                    squareroot = squareroot + square;
                }
                double finalequation = squareroot / results.Count;
                standarddeviationfinal = Math.Sqrt(finalequation);

                return standarddeviationfinal;
            }
            catch
            {
                return standarddeviationfinal;
            }
        }
        protected double GetOverallAvg()
        {
            double averagefinal = 0.0;
            try
            {
                ArrayList results = dbmanager.GetAllStaffAppraisal();

                double final = 0.00;
                for (int i = 0; i < results.Count; i++)
                {
                    final = final + Convert.ToDouble(results[i].ToString());
                }
                averagefinal = final / results.Count;

                return averagefinal;
            }
            catch
            {
                return averagefinal;
            }
        }
        protected double GetMedianFunctionViaSectionCount(string section, string function, DateTime date, int questionID)
        {
            double median = 0.0;
            int pos = 0;
            int pos2 = 0;
            try
            {
                ArrayList results = dbmanager.GetAppraisalForFunction(section, function, date, questionID);
                results.Sort();

                int count = results.Count;
                if (count % 2 != 0)
                {
                    pos = ((count + 1) / 2) - 1;
                }
                else if (count % 2 == 0)
                {
                    pos = (count / 2) - 1;
                    pos2 = (((count / 2) + 1)) - 1;
                }
                if (count % 2 != 0)
                {
                    median = Convert.ToDouble(results[pos].ToString());
                }
                else if (count % 2 == 0)
                {
                    median = (Convert.ToDouble(results[pos].ToString()) + Convert.ToDouble(results[pos2].ToString())) / 2;
                }
                return median;
            }
            catch
            {
                return median;
            }
        }
        protected double GetMedianFunctionViaPeriod(DateTime date)
        {
            double median = 0.0;
            int pos = 0;
            int pos2 = 0;
            try
            {
                ArrayList results = dbmanager.GetAppraisalViaPeriod(date);
                results.Sort();

                int count = results.Count;
                if (count % 2 != 0)
                {
                    pos = ((count + 1) / 2) - 1;
                }
                else if (count % 2 == 0)
                {
                    pos = (count / 2) - 1;
                    pos2 = (((count / 2) + 1)) - 1;
                }
                if (count % 2 != 0)
                {
                    median = Convert.ToDouble(results[pos].ToString());
                }
                else if (count % 2 == 0)
                {
                    median = (Convert.ToDouble(results[pos].ToString()) + Convert.ToDouble(results[pos2].ToString())) / 2;
                }
                return median;
            }
            catch
            {
                return median;
            }
        }
        protected void populateChartSectionViaFunctionALL()
        {
            try
            {
                MultiView1.Visible = true;
                Chart1.Series.Clear();
                Chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = System.Drawing.Color.LightGray;
                Chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = System.Drawing.Color.LightGray;
                MultiView1.Visible = true;
                DataTable table = new DataTable();
                table.Columns.Add("AverageGrading");
                table.Columns.Add("Date");
                ArrayList listofdates = dbmanager.GetListofDatesViaAll();

                // decare and get items
                double result = 0.0;

                //ArrayList listofstaff = dbmanager.GetAllStaffDetails();
                //ArrayList listofquestion = dbmanager.GetAllQuestion();
                List<double> stdDeviation = new List<double>();
                foreach (DateTime date in listofdates)
                {
                    /*foreach (staffinfo staff in listofstaff)
                    {
                    foreach (Question qn in listofquestion)
                    {
                    staffAverage += dbmanager.GetAvgRating(staff.Uid, date, qn.QuestionID);
                    staffAverage += dbmanager.GetAverageStaffPeriod(staff.Uid, date);
                    }
                    int countquestion = dbmanager.GetTotalCountQuestionInPeriod(staff.Uid, date);
                    staffAverageResult = Math.Round((staffAverage / countquestion), 1);
                    }
                    result = Math.Round((staffAverageResult / listofstaff.Count), 1); */
                    result = dbmanager.GetAverageAllStaffPeriod(date);
                    string monthname = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(date.Month);
                    table.Rows.Add(result, monthname.Substring(0, 3) + "/" + date.Year.ToString().Substring(2, 2));

                    result = 0.0;
                }
                bool display = false;
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    if (table.Rows[i].ItemArray[1].ToString() == "0")
                    {
                        display = false;
                    }
                    else
                    {
                        display = true;
                        break;
                    }
                }
                if (display == true)
                {
                    double result1 = 0.0;
                    ArrayList date = dbmanager.GetListofDatesViaAll();
                    result1 = dbmanager.GetAverageAllStaffPeriod(date);

                    Chart1.Visible = true;
                    DataTableReader datareader = table.CreateDataReader();
                    Chart1.ChartAreas[0].AxisY.Title = "Average grade";
                    Chart1.DataBindCrossTable(datareader, "Date", "", "AverageGrading", "");
                    Chart1.Legends.Add("Legend");
                    Chart1.Legends[0].Enabled = true;
                    Chart1.Legends[0].BackColor = System.Drawing.Color.Transparent;
                    Chart1.Width = 1000;
                    Chart1.Height = 600;
                    Chart1.Series[0].ToolTip = "populateChartSectionViaFunctionALL";

                    MultiView1.ActiveViewIndex = 0;
                }
                else
                {
                    Chart1.Visible = false;
                    MultiView1.ActiveViewIndex = 1;
                    lbDisplay.Text = "<b>No result found</b>";
                }
            }
            catch (Exception e)
            {
                MultiView1.Visible = false;
                MessageBoxShowWithoutredirect(e.Message);
            }
        }
        protected void populateChartFunctionViaSection(string section, string functionlist, int questionID)
        {
            try
            {
                MultiView1.Visible = true;

                Chart1.Series.Clear();
                Chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = System.Drawing.Color.LightGray;
                Chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = System.Drawing.Color.LightGray;
                MultiView1.Visible = true;
                DataTable table = new DataTable();
                table.Columns.Add("FunctionGroup");
                table.Columns.Add("AverageGrading");
                table.Columns.Add("Date");

                ArrayList graphwidth = new ArrayList();
                ArrayList listofdates = dbmanager.GetListofDatesViaSection(section);
                string[] functionsplit = functionlist.Split(',');

                ArrayList listofquestion = dbmanager.GetAllQuestion();
                ArrayList listofstaff = dbmanager.GetAllStaffDetails();
                foreach (DateTime date in listofdates)
                {
                    foreach (string function in functionsplit)
                    {
                        double result = 0.0;
                        double staffAverage = 0.0;
                        double staffAverageResult = 0.0;
                        int count = 0;
                        foreach (staffinfo staff in listofstaff)
                        {
                            if (staff.Section.Contains(section) && staff.Function.Equals(function))
                            {
                                if (questionID == 0)
                                {
                                    foreach (Question qn in listofquestion)
                                    {
                                        staffAverage += dbmanager.GetAvgRating(staff.Uid, date, qn.QuestionID); // for all question
                                    }
                                    int countquestion = dbmanager.GetTotalCountQuestionInPeriod(staff.Uid, date);
                                    staffAverageResult = Math.Round((staffAverage / countquestion), 1);
                                    count++;
                                }
                                else
                                {
                                    staffAverage += dbmanager.GetAvgRating(staff.Uid, date, questionID); // for per question
                                    int countquestion = dbmanager.GetTotalCountQuestionInPeriod(staff.Uid, date);
                                    staffAverageResult = staffAverage;
                                    count++;
                                }
                            }
                        }
                        try
                        {
                            result = Math.Round((staffAverageResult / count), 1);
                        }
                        catch
                        {
                            result = 0.0;
                        }
                        double avg = dbmanager.GetAvgAppraisalForPeriod(date);
                        double sd = dbmanager.GetStdDevAppraisalForPeriod(date);

                        ArrayList MedList = new ArrayList();
                        MedList.Add(GetMedianFunctionViaPeriod(date));

                        string monthname = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(date.Month);
                        table.Rows.Add(function, result, monthname.Substring(0, 3) + "/" + date.Year.ToString().Substring(2, 2) + "\nA: " + avg.ToString("0.0")
                            + "\nS: " + sd.ToString("F") + "\nM: " + Convert.ToDouble(MedList[0]).ToString("0.0"));

                        result = 0.0;
                        staffAverage = 0.0;
                        staffAverageResult = 0.0;
                        graphwidth.Add(monthname);
                    }
                }
                bool display = false;
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    if (table.Rows[i].ItemArray[1].ToString() == "0")
                    {
                        display = false;
                    }
                    else
                    {
                        display = true;
                        break;
                    }
                }
                if (display == true)
                {
                    int graphwidthx = graphwidth.Count * 300;

                    Chart1.Visible = true;
                    DataTableReader datareader = table.CreateDataReader();
                    Chart1.DataBindCrossTable(datareader, "FunctionGroup", "Date", "AverageGrading", "");
                    Chart1.Legends.Add("Legend");
                    Chart1.ChartAreas[0].AxisX.Title = "Period of feedback";
                    Chart1.ChartAreas[0].AxisY.Title = "Average grade";
                    Chart1.Legends[0].Enabled = true;
                    Chart1.Legends[0].BackColor = System.Drawing.Color.Transparent;
                    Chart1.Width = graphwidthx;
                    Chart1.Height = 600;

                    ArrayList listOfStDev = new ArrayList();
                    ArrayList listOfMed = new ArrayList();
                    foreach (DateTime date in listofdates)
                    {
                        string[] listOfFunctions = functionlist.Split(',');
                        ArrayList listOfSDForFunction = new ArrayList();
                        ArrayList listOfMedForFunction = new ArrayList();

                        for (int x = 0; x < listOfFunctions.Length; x++)
                        {
                            listOfSDForFunction.Add(dbmanager.GetStdDevAppraisalForFunction(section, listOfFunctions[x], date, questionID));
                            listOfMedForFunction.Add(GetMedianFunctionViaSectionCount(section, listOfFunctions[x], date, questionID));
                        }
                        listOfStDev.Add(String.Join(",", listOfSDForFunction.ToArray()));
                        listOfMed.Add(String.Join(",", listOfMedForFunction.ToArray()));
                    }
                    for (int i = 0; i < Chart1.Series.Count; i++)
                    {
                        for (int k = 0; k < Chart1.Series[i].Points.Count; k++)
                        {
                            string[] listOfSDPerSeries = listOfStDev[k].ToString().Split(',');
                            string[] listOfMedPerSeries = listOfMed[k].ToString().Split(',');
                            Chart1.Series[i].Points[k].Label = "A: " + "#VALY\n" + "S: " + Convert.ToDouble(listOfSDPerSeries[i]).ToString("F") + "\nM: " + Convert.ToDouble(listOfMedPerSeries[i]).ToString("0.0");
                        }
                    }
                    MultiView1.ActiveViewIndex = 0;

                    string imgPath = HttpContext.Current.Request.PhysicalApplicationPath + "Charts/ChartImage.png";
                    Chart1.SaveImage(imgPath);
                }
                else
                {
                    Chart1.Visible = false;
                    MultiView1.ActiveViewIndex = 1;
                    lbDisplay.Text = "<b>No result found</b>";
                }
            }
            catch (Exception e)
            {
                throw e;
                MultiView1.Visible = false;
            }
        }
        protected void populateChartSectionViaFunction(string function, string sectionlist, int questionID)
        {
            try
            {
                MultiView1.Visible = true;
                Chart1.Series.Clear();
                Chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = System.Drawing.Color.LightGray;
                Chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = System.Drawing.Color.LightGray;
                MultiView1.Visible = true;
                DataTable table = new DataTable();
                table.Columns.Add("SectionGroup");
                table.Columns.Add("AverageGrading");
                table.Columns.Add("Date");
                ArrayList listofdates = dbmanager.GetListofDatesViaFunction(function);
                string[] sectionsplit = sectionlist.Split(',');

                ArrayList listofquestion = dbmanager.GetAllQuestion();
                ArrayList listofstaff = dbmanager.GetAllStaffDetails();
                foreach (DateTime date in listofdates)
                {
                    foreach (string section in sectionsplit)
                    {
                        double result = 0.0;
                        double staffAverage = 0.0;
                        double staffAverageResult = 0.0;
                        int count = 0;

                        foreach (staffinfo staff in listofstaff)
                        {
                            if (staff.Section.Contains(section) && staff.Function.Equals(function))
                            {
                                if (questionID == 0)
                                {
                                    foreach (Question qn in listofquestion)
                                    {
                                        staffAverage += dbmanager.GetAvgRating(staff.Uid, date, qn.QuestionID); // for all question
                                    }
                                    int countquestion = dbmanager.GetTotalCountQuestionInPeriod(staff.Uid, date);
                                    staffAverageResult = Math.Round((staffAverage / countquestion), 1);
                                    count++;
                                }
                                else
                                {
                                    staffAverage += dbmanager.GetAvgRating(staff.Uid, date, questionID); // for per question
                                    int countquestion = dbmanager.GetTotalCountQuestionInPeriod(staff.Uid, date);
                                    staffAverageResult = staffAverage;
                                    count++;
                                }
                            }
                        }
                        try
                        {
                            result = Math.Round((staffAverageResult / count), 1);
                        }
                        catch
                        {
                            result = 0.0;
                        }
                        string monthname = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(date.Month);
                        table.Rows.Add(section, result, monthname.Substring(0, 3) + "/" + date.Year.ToString().Substring(2, 2));

                        result = 0.0;
                        staffAverage = 0.0;
                        staffAverageResult = 0.0;
                    }
                }
                bool display = false;
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    if (table.Rows[i].ItemArray[1].ToString() == "0")
                    {
                        display = false;
                    }
                    else
                    {
                        display = true;
                        break;
                    }
                }
                if (display == true)
                {
                    Chart1.Visible = true;
                    DataTableReader datareader = table.CreateDataReader();
                    Chart1.Series[0].ChartType = SeriesChartType.Line;
                    Chart1.ChartAreas[0].AxisX.Title = "Period of feedback";
                    Chart1.ChartAreas[0].AxisY.Title = "Average grade";
                    Chart1.DataBindCrossTable(datareader, "SectionGroup", "Date", "AverageGrading", "");
                    Chart1.Legends.Add("Legend");
                    Chart1.Legends[0].Enabled = true;
                    Chart1.Legends[0].BackColor = System.Drawing.Color.Transparent;
                    Chart1.Width = 1300;
                    Chart1.Height = 600;
                    Chart1.Series[0].ToolTip = "populateChartSectionViaFunction";

                    MultiView1.ActiveViewIndex = 0;
                }
                else
                {
                    Chart1.Visible = false;
                    MultiView1.ActiveViewIndex = 1;
                    lbDisplay.Text = "<b>No result found</b>";
                }
            }
            catch (Exception e)
            {
                MultiView1.Visible = false;
                MessageBoxShowWithoutredirect(e.Message);
            }
        }
        protected void SearchSwapBtn_Click(object sender, ImageClickEventArgs e)
        {
            MultiView1.Visible = false;
            string role = Session["Role"].ToString();
            if (Session["FunctionTrue"] != null)
            {
                if (role == "Admin")
                {
                    BindDropDownListAdmin();
                }
                else if (role == "Officer")
                {
                    BindDropDownListOfficer();
                }
                else if (role == "Director")
                {
                    BindDropDownListDirector();
                }
                SelectTbxFunction.Text = "";
                SearchPanelFunctionViaSection.Visible = false;
                SearchPanelSectionViaFunction.Visible = true;
                Session["FunctionTrue"] = null;
            }
            else
            {
                if (role == "Admin")
                {
                    BindDropDownListAdmin();
                }
                else if (role == "Officer")
                {
                    BindDropDownListOfficer();
                }
                else if (role == "Director")
                {
                    BindDropDownListDirector();
                }
                SelectTbxSection.Text = "";
                lblSelectFunction.Text = "select function group (chart title): ";
                lblFilterBySection.Text = "select section (chart components): ";
                lblFilterByQuestion.Text = lbl;
                lblSelectedSection.Text = "selected section(s): ";
                SearchPanelFunctionViaSection.Visible = true;
                SearchPanelSectionViaFunction.Visible = false;
                Session["FunctionTrue"] = true;
            }
        }
        protected void AddBtn_Click(object sender, EventArgs e)
        {
            MultiView1.Visible = false;
            if (SelectTbxFunction.Text != "All function")
            {
                if (ddlFilterFunction.SelectedIndex != 0 && Session["AdditionalSelectFunction"] == null)
                {
                    if (SelectTbxFunction.Text == "")
                    {
                        SelectTbxFunction.Text += ddlFilterFunction.Text;
                    }
                    else
                    {
                        SelectTbxFunction.Text += "," + ddlFilterFunction.Text;
                    }
                    //SelectTbxFunction.Text += ddlFilterFunction.Text;
                    Session["AdditionalSelectFunction"] = true;
                }
                else if (ddlFilterFunction.SelectedIndex != 0 && Session["AdditionalSelectFunction"] != null && ddlFilterFunction.Text != "All function")
                {
                    if (!SelectTbxFunction.Text.Contains("All function"))
                    {
                        if (!SelectTbxFunction.Text.Contains(ddlFilterFunction.Text))
                        {
                            SelectTbxFunction.Text += "," + ddlFilterFunction.Text;
                        }
                    }
                }
                else if (ddlFilterFunction.Text == "All function")
                {
                    SelectTbxFunction.Text = ddlFilterFunction.Text;
                }
            }
            else
            {
                MessageBoxShowWithoutredirect("Please clear search content.");
            }
        }
        protected void AddSectionBtnSection_Click(object sender, EventArgs e)
        {
            MultiView1.Visible = false;
            if (SelectTbxSection.Text != "All section")
            {
                if (ddlFilterBySection.SelectedIndex != 0 && Session["AdditionalSelectSection"] == null)
                {
                    SelectTbxSection.Text += ddlFilterBySection.Text;
                    Session["AdditionalSelectSection"] = true;
                }
                else if (ddlFilterBySection.SelectedIndex != 0 && Session["AdditionalSelectSection"] != null && ddlFilterBySection.Text != "All section")
                {
                    if (!SelectTbxSection.Text.Equals("All section"))
                    {
                        if (!SelectTbxSection.Text.Contains(ddlFilterBySection.Text))
                        {
                            SelectTbxSection.Text += "," + ddlFilterBySection.Text;
                        }
                    }
                }
                else if (ddlFilterBySection.Text == "All section")
                {
                    SelectTbxSection.Text = ddlFilterBySection.Text;
                }
            }
            else
            {
                MessageBoxShowWithoutredirect("Please clear search content.");
            }
        }
        protected void ClearBtn_Click(object sender, EventArgs e)
        {
            MultiView1.Visible = false;
            Session["AdditionalSelectFunction"] = null;
            Session["AdditionalSelectSection"] = null;
            SelectTbxFunction.Text = "";
            ddlFilterFunction.Items.Clear();
            ddlFilterFunction.Items.Add("<--------Autoupdate-------->");
            ddlSelectSection.SelectedIndex = 0;
            SelectTbxSection.Text = "";
            ddlFilterBySection.Items.Clear();
            ddlFilterBySection.Items.Add("<--------Autoupdate-------->");
            ddlSelectFunction.SelectedIndex = 0;
        }
        protected void ClearText()
        {
            //MultiView1.Visible = false;
            Session["AdditionalSelectFunction"] = null;
            Session["AdditionalSelectSection"] = null;
            SelectTbxFunction.Text = "";
            ddlFilterFunction.Items.Clear();
            ddlFilterFunction.Items.Add("<--------Autoupdate-------->");
            ddlSelectSection.SelectedIndex = 0;
            SelectTbxSection.Text = "";
            ddlFilterBySection.Items.Clear();
            ddlFilterBySection.Items.Add("<--------Autoupdate-------->");
            ddlSelectFunction.SelectedIndex = 0;
        }
        protected void ddlSelectSection_SelectedIndexChanged(object sender, EventArgs e)
        {
            MultiView1.Visible = false;
            if (ddlSelectSection.SelectedIndex != 0)
            {
                if (ddlSelectSection.Text == "<--------ASC School-------->")
                {
                    ddlFilterFunction.Items.Clear();
                    ddlFilterFunction.Items.Add("<--------Autoupdate-------->");
                    ddlFilterFunction.SelectedIndex = 0;
                    SelectTbxFunction.Text = "";
                    populateChartSectionViaFunctionALL();
                }
                else
                {
                    ArrayList listoffunctionviasection = RemoveDups(dbmanager.GetDistinctFunctionViaSection(ddlSelectSection.Text));
                    if (listoffunctionviasection.Count > 0)
                    {
                        MultiView1.Visible = false;
                        listoffunctionviasection.Insert(0, "<----Please select one---->");
                        if (listoffunctionviasection.Count > 2)
                        {
                            listoffunctionviasection.Insert(1, "All function");
                        }
                        ddlFilterFunction.DataSource = listoffunctionviasection;
                        ddlFilterFunction.DataBind();
                        SelectTbxFunction.Text = "";
                        Session["AdditionalSelectFunction"] = null;
                    }
                }
            }
        }
        protected void ddlSelectFunction_SelectedIndexChanged(object sender, EventArgs e)
        {
            MultiView1.Visible = false;
            if (ddlSelectFunction.SelectedIndex != 0)
            {
                if (ddlSelectFunction.Text == "<--------ASC School-------->")
                {
                    ddlFilterBySection.Items.Clear();
                    ddlFilterBySection.Items.Add("<--------Autoupdate-------->");
                    ddlFilterBySection.SelectedIndex = 0;
                    SelectTbxSection.Text = "";
                    populateChartSectionViaFunctionALL();
                }
                else
                {
                    ArrayList listofsectionviafunction = RemoveDups(dbmanager.GetDistinctSectionViaFunction(ddlSelectFunction.Text));
                    if (listofsectionviafunction.Count > 0)
                    {
                        MultiView1.Visible = false;
                        listofsectionviafunction.Insert(0, "<----Please select one---->");
                        if (listofsectionviafunction.Count > 2)
                        {
                            listofsectionviafunction.Insert(1, "All section");
                        }
                        ddlFilterBySection.DataSource = listofsectionviafunction;
                        ddlFilterBySection.DataBind();
                        SelectTbxSection.Text = "";
                        Session["AdditionalSelectSection"] = null;
                    }
                }
            }
        }
        protected void SearchBtnSection_Click(object sender, EventArgs e)
        {
            if (SelectTbxSection.Text == "")
            {
                MessageBox("Please click add button to insert section(s)!");
            }
            else
            {
                MultiView1.Visible = false;

                if (ddlSelectFunction.Text == "<--------ASC School-------->")
                {
                    ddlFilterBySection.Items.Clear();
                    ddlFilterBySection.Items.Add("<--------Autoupdate-------->");
                    ddlFilterBySection.SelectedIndex = 0;
                    populateChartSectionViaFunctionALL();
                }
                else
                {
                    // for question ddl ()
                    int questionID = ddlFilterQuestion.SelectedIndex;

                    string function = ddlSelectFunction.Text.Trim();
                    string section = SelectTbxSection.Text.Trim();
                    string allsection = "";
                    int counter = 1;

                    if (section.Length > 0)
                    {
                        if (section == "All section")
                        {
                            ArrayList listofitems = new ArrayList();
                            foreach (ListItem sect in ddlFilterBySection.Items)
                            {
                                if (sect.Text != "All section" && sect.Text != "<----Please select one---->")
                                {
                                    listofitems.Add(sect);
                                }
                            }
                            foreach (ListItem sectitem in listofitems)
                            {
                                if (counter == listofitems.Count)
                                {
                                    allsection += sectitem.Text;
                                }
                                else
                                {
                                    allsection += sectitem.Text + ",";
                                }
                                counter++;
                            }
                            populateChartSectionViaFunction(function, allsection, questionID);
                        }
                        else
                        {
                            populateChartSectionViaFunction(function, section, questionID);
                        }
                    }
                }
                ClearText();
            }
        }
        protected void ExcelDl_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=ViewHistoryChart.xlsx");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            Chart1.RenderControl(hw);

            using (MemoryStream stream = new MemoryStream())
            {
                using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook))
                {
                    var fileName = HttpContext.Current.Request.PhysicalApplicationPath + @"\Charts\ChartImage.png";

                    WorkbookPart wbp = spreadsheetDocument.AddWorkbookPart();
                    WorksheetPart wsp = wbp.AddNewPart<WorksheetPart>();
                    Workbook wb = new Workbook();
                    FileVersion fv = new FileVersion();
                    fv.ApplicationName = "Microsoft Office Excel";
                    Worksheet ws = new Worksheet();
                    SheetData sd = new SheetData();

                    // add contents
                    List<string> values = new List<string>();

                    //values.Add(lblSectionFunctionSelected.Text);

                    uint i = 1;
                    foreach (var value in values)
                    {
                        UInt32Value rowIndex = UInt32Value.FromUInt32(i);
                        var row = new Row { RowIndex = rowIndex }; // add a row at the top of spreadsheet
                        sd.Append(row);

                        var cell = new Cell
                        {
                            CellValue = new CellValue(value),
                            DataType = new EnumValue<CellValues>(CellValues.String)
                        };
                        row.InsertAt(cell, 0);
                        i++;
                    }
                    // add image
                    DrawingsPart dp = wsp.AddNewPart<DrawingsPart>();
                    ImagePart imgp = dp.AddImagePart(ImagePartType.Png, wsp.GetIdOfPart(dp));
                    using (FileStream fs = new FileStream(fileName, FileMode.Open))
                    {
                        imgp.FeedData(fs);
                    }
                    DocumentFormat.OpenXml.Drawing.Spreadsheet.NonVisualDrawingProperties nvdp = new DocumentFormat.OpenXml.Drawing.Spreadsheet.NonVisualDrawingProperties();
                    nvdp.Id = 1025;
                    nvdp.Name = "Picture 1";
                    nvdp.Description = "Chart";
                    DocumentFormat.OpenXml.Drawing.PictureLocks picLocks = new DocumentFormat.OpenXml.Drawing.PictureLocks();
                    picLocks.NoChangeAspect = true;
                    picLocks.NoChangeArrowheads = true;
                    DocumentFormat.OpenXml.Drawing.Spreadsheet.NonVisualPictureDrawingProperties nvpdp = new DocumentFormat.OpenXml.Drawing.Spreadsheet.NonVisualPictureDrawingProperties();
                    nvpdp.PictureLocks = picLocks;
                    DocumentFormat.OpenXml.Drawing.Spreadsheet.NonVisualPictureProperties nvpp = new DocumentFormat.OpenXml.Drawing.Spreadsheet.NonVisualPictureProperties();
                    nvpp.NonVisualDrawingProperties = nvdp;
                    nvpp.NonVisualPictureDrawingProperties = nvpdp;

                    DocumentFormat.OpenXml.Drawing.Stretch stretch = new DocumentFormat.OpenXml.Drawing.Stretch();
                    stretch.FillRectangle = new DocumentFormat.OpenXml.Drawing.FillRectangle();

                    DocumentFormat.OpenXml.Drawing.Spreadsheet.BlipFill blipFill = new DocumentFormat.OpenXml.Drawing.Spreadsheet.BlipFill();
                    DocumentFormat.OpenXml.Drawing.Blip blip = new DocumentFormat.OpenXml.Drawing.Blip();
                    blip.Embed = dp.GetIdOfPart(imgp);
                    blip.CompressionState = DocumentFormat.OpenXml.Drawing.BlipCompressionValues.Print;
                    blipFill.Blip = blip;
                    blipFill.SourceRectangle = new DocumentFormat.OpenXml.Drawing.SourceRectangle();
                    blipFill.Append(stretch);

                    DocumentFormat.OpenXml.Drawing.Transform2D t2d = new DocumentFormat.OpenXml.Drawing.Transform2D();
                    DocumentFormat.OpenXml.Drawing.Offset offset = new DocumentFormat.OpenXml.Drawing.Offset();
                    offset.X = 0;
                    offset.Y = 0;
                    t2d.Offset = offset;
                    Bitmap bm = new Bitmap(fileName);
                    //http://en.wikipedia.org/wiki/English_Metric_Unit#DrawingML
                    //http://stackoverflow.com/questions/1341930/pixel-to-centimeter
                    //http://stackoverflow.com/questions/139655/how-to-convert-pixels-to-points-px-to-pt-in-net-c
                    DocumentFormat.OpenXml.Drawing.Extents extents = new DocumentFormat.OpenXml.Drawing.Extents();
                    extents.Cx = (long)bm.Width * (long)((float)914400 / bm.HorizontalResolution);
                    extents.Cy = (long)bm.Height * (long)((float)914400 / bm.VerticalResolution);
                    bm.Dispose();
                    t2d.Extents = extents;
                    DocumentFormat.OpenXml.Drawing.Spreadsheet.ShapeProperties sp = new DocumentFormat.OpenXml.Drawing.Spreadsheet.ShapeProperties();
                    sp.BlackWhiteMode = DocumentFormat.OpenXml.Drawing.BlackWhiteModeValues.Auto;
                    sp.Transform2D = t2d;
                    DocumentFormat.OpenXml.Drawing.PresetGeometry prstGeom = new DocumentFormat.OpenXml.Drawing.PresetGeometry();
                    prstGeom.Preset = DocumentFormat.OpenXml.Drawing.ShapeTypeValues.Rectangle;
                    prstGeom.AdjustValueList = new DocumentFormat.OpenXml.Drawing.AdjustValueList();
                    sp.Append(prstGeom);
                    sp.Append(new DocumentFormat.OpenXml.Drawing.NoFill());

                    DocumentFormat.OpenXml.Drawing.Spreadsheet.Picture picture = new DocumentFormat.OpenXml.Drawing.Spreadsheet.Picture();
                    picture.NonVisualPictureProperties = nvpp;
                    picture.BlipFill = blipFill;
                    picture.ShapeProperties = sp;

                    DocumentFormat.OpenXml.Drawing.Spreadsheet.Position pos = new DocumentFormat.OpenXml.Drawing.Spreadsheet.Position();
                    pos.X = 0;
                    pos.Y = 10;
                    Extent ext = new Extent();
                    ext.Cx = extents.Cx;
                    ext.Cy = extents.Cy;
                    AbsoluteAnchor anchor = new AbsoluteAnchor();
                    anchor.Position = pos;
                    anchor.Extent = ext;
                    anchor.Append(picture);
                    anchor.Append(new ClientData());
                    WorksheetDrawing wsd = new WorksheetDrawing();
                    wsd.Append(anchor);
                    Drawing drawing = new Drawing();
                    drawing.Id = dp.GetIdOfPart(imgp);

                    wsd.Save(dp);

                    ws.Append(sd);
                    ws.Append(drawing);
                    wsp.Worksheet = ws;
                    wsp.Worksheet.Save();
                    Sheets sheets = new Sheets();
                    Sheet sheet = new Sheet();
                    sheet.Name = "history chart";
                    sheet.SheetId = 1;
                    sheet.Id = wbp.GetIdOfPart(wsp);
                    sheets.Append(sheet);
                    wb.Append(fv);
                    wb.Append(sheets);

                    spreadsheetDocument.WorkbookPart.Workbook = wb;
                    spreadsheetDocument.WorkbookPart.Workbook.Save();
                    spreadsheetDocument.Close();
                }
                File.WriteAllBytes(HttpContext.Current.Request.PhysicalApplicationPath + @"\Charts\temp.xlsx", stream.ToArray());
            }
            Response.WriteFile(HttpContext.Current.Request.PhysicalApplicationPath + @"\Charts\temp.xlsx");
            Response.Flush();
            Response.Close();
            Response.End();
        }
        // Given a worksheet, a column name, and a row index, 
        // gets the cell at the specified column and 
        private static Cell GetCell(Worksheet worksheet,
                  string columnName, uint rowIndex)
        {
            Row row = GetRow(worksheet, rowIndex);

            if (row == null)
                return null;

            return row.Elements<Cell>().Where(c => string.Compare
                   (c.CellReference.Value, columnName +
                   rowIndex, true) == 0).First();
        }
        // Given a worksheet and a row index, return the row.
        private static Row GetRow(Worksheet worksheet, uint rowIndex)
        {
            return worksheet.GetFirstChild<SheetData>().
              Elements<Row>().Where(r => r.RowIndex == rowIndex).First();
        }
    }
}