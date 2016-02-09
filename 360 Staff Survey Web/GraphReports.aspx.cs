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
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Globalization;
using Excel = Microsoft.Office.Interop.Excel;

namespace _360_Staff_Survey_Web
{
    public partial class WebForm2 : System.Web.UI.Page
    {
        public static string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Appraisal_System"].ConnectionString.ToString();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session["Role"] != null)
                {
                    string role = Session["Role"].ToString();
                    if (role != "Officer" && role != "Director")
                    {
                        Response.Redirect("accessdenied.aspx");
                    }
                }
            }
            SqlConnection myconn = null;
            ArrayList listofquestion = new ArrayList();
            ArrayList questionall = new ArrayList();
            ArrayList shortdate = new ArrayList();
            ArrayList listofdates = dbmanager.GetAllDates();
            int index = 0;
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                comm.CommandText = "select * from Question order by QuestionInclude, QuestionId asc";

                SqlDataReader dr = comm.ExecuteReader();
                while (dr.Read())
                {
                    bool qninclude = false;
                    string include = dr["QuestionInclude"].ToString();
                    if (include == "Y")
                    {
                        qninclude = true;
                    }
                    int id = Convert.ToInt32(dr["QuestionID"]);
                    string question = dr["Question"].ToString();
                    string one = dr["OneRate"].ToString();
                    string seven = dr["SevenRate"].ToString();

                    listofquestion.Add(question);
                }
                dr.Close();
            }
            catch (SqlException)
            {
            }
            finally
            {
                myconn.Close();
            }
            if (ddlQuestions.Items.Count == 0 && Session["Role"].ToString().Equals("Director"))
            {
                listofquestion.Insert(0, "<----Please select one---->");
                listofquestion.Insert(1, "All Questions");

                ddlQuestions.DataSource = listofquestion;
                ddlQuestions.DataBind();
            }
            else if (ddlQuestions.Items.Count == 0 && Session["Role"].ToString().Equals("Officer"))
            {
                questionall.Insert(0, "<----Please select one---->");
                questionall.Insert(1, "All Questions");

                ddlQuestions.DataSource = questionall;
                ddlQuestions.DataBind();
            }
            if (ddlSections.Items.Count == 0)
            {
                string uid = Session["UserID"].ToString();
                staffinfo stfinfo = dbmanager.GetStaffDetailsViaUid(uid);

                ArrayList listofsection = new ArrayList();
                string[] arraysection = stfinfo.Section.Split(',');
                if (arraysection.LongLength > 0)
                {
                    foreach (string sect in arraysection)
                    {
                        listofsection.Add(sect);
                    }
                }
                listofsection.Insert(0, "<----Please select one---->");
                listofsection.Insert(1, "All Sections");
                ddlSections.DataSource = listofsection;
                ddlSections.DataBind();
            }
            if (ddlPeriod.Items.Count == 0)
            {
                foreach (DateTime date in listofdates.ToArray())
                {
                    DateTime toshortdate = ((DateTime)listofdates[index]);
                    string monthname = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(toshortdate.Month);
                    shortdate.Add(toshortdate.Year.ToString() + "-" + (toshortdate.Month.ToString().Length == 1 ? "0" + toshortdate.Month.ToString() : toshortdate.Month.ToString()));
                    index++;
                }
                shortdate.Insert(0, "<----Please select one---->");
                ddlPeriod.DataSource = shortdate;
                ddlPeriod.DataBind();
            }
        }
        protected void btnView_Click(object sender, EventArgs e)
        {
            int question = ddlQuestions.SelectedIndex;
            string section = ddlSections.SelectedValue;
            string enddate = ddlPeriod.SelectedValue;
            string name = "";

            ArrayList results = new ArrayList();
            ArrayList appraisalID = new ArrayList();
            ArrayList names = dbmanager.GetAllNames();

            populateChartForStaff(name, section, question);
        }
        protected void populateChartForStaff(string staff, string section, int questionID)
        {
            string question = ddlQuestions.SelectedValue;
            string enddate = ddlPeriod.SelectedValue;

            ArrayList graphwidth = new ArrayList();

            if (question != "All Questions" && section != "All Sections" && enddate != "<----Please select one---->")
            {
                int qid = dbmanager.GetQuestionIDFromQuestion(question);

                SqlConnection myconn = null;
                try
                {
                    myconn = new SqlConnection();
                    SqlCommand comm = new SqlCommand();
                    myconn.ConnectionString = connectionString;
                    myconn.Open();
                    comm.Connection = myconn;
                    comm.CommandText = "SELECT DISTINCT StaffInfo.Name FROM StaffInfo INNER JOIN StaffAppraisal ON StaffAppraisal.AppraisalStaffUserID = StaffInfo.UserID WHERE StaffAppraisal.AppraisalQuestionID = @qid AND (StaffInfo.Section = @section OR StaffInfo.Section LIKE'%'+ @section OR StaffInfo.Section LIKE'%'+ @section+'%')";
                    comm.Parameters.AddWithValue("@qid", qid);
                    comm.Parameters.AddWithValue("@section", section);
                    SqlDataReader dr = comm.ExecuteReader();
                    while (dr.Read())
                    {
                        string staffname = dr["Name"].ToString();

                        SqlConnection myconn2 = null;
                        try
                        {
                            myconn2 = new SqlConnection();
                            SqlCommand comm2 = new SqlCommand();
                            myconn2.ConnectionString = connectionString;
                            myconn2.Open();
                            comm2.Connection = myconn2;
                            comm2.CommandText = "SELECT FORMAT(AVG(StaffAppraisal.AppraisalResult), 'N1') AS AvgResult FROM StaffAppraisal INNER JOIN StaffInfo ON StaffAppraisal.AppraisalStaffUserID = StaffInfo.UserID WHERE StaffAppraisal.AppraisalQuestionID = @qid AND StaffInfo.Name = @name and StaffAppraisal.SystemEndDate LIKE '" + enddate + "%'";
                            comm2.Parameters.AddWithValue("@qid", qid);
                            comm2.Parameters.AddWithValue("@name", staffname);
                            SqlDataReader dr2 = comm2.ExecuteReader();
                            while (dr2.Read())
                            {
                                string avgresult = dr2["AvgResult"].ToString();
                                graphwidth.Add(staffname);

                                Chart1.Series[0].Points.AddXY(staffname, avgresult);
                                Chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = System.Drawing.Color.LightGray;
                                Chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = System.Drawing.Color.LightGray;
                                Chart1.Visible = true;

                                Chart2.Series[0].Points.AddXY(staffname, avgresult);
                                Chart2.ChartAreas[0].AxisX.MajorGrid.LineColor = System.Drawing.Color.LightGray;
                                Chart2.ChartAreas[0].AxisY.MajorGrid.LineColor = System.Drawing.Color.LightGray;
                                Chart2.Visible = true;
                            }
                            dr2.Close();
                        }
                        catch (SqlException)
                        {
                        }
                        finally
                        {
                            myconn2.Close();
                        }
                    }
                    dr.Close();
                }
                catch (SqlException)
                {
                }
                finally
                {
                    myconn.Close();
                }
            }
            else if (question == "All Questions" && section == "All Sections" && enddate != "<----Please select one---->")
            {
                SqlConnection myconn = null;
                try
                {
                    myconn = new SqlConnection();
                    SqlCommand comm = new SqlCommand();
                    myconn.ConnectionString = connectionString;
                    myconn.Open();
                    comm.Connection = myconn;
                    //comm.CommandText = "SELECT DISTINCT StaffInfo.Name FROM StaffInfo INNER JOIN StaffAppraisal ON StaffAppraisal.AppraisalStaffUserID = StaffInfo.UserID WHERE StaffInfo.Functions != 'Director' AND StaffInfo.Functions != 'AD' AND StaffInfo.Functions != 'DD'";
                    staffinfo si = dbmanager.GetStaffDetailsViaUid(Session["LoginName"].ToString());
                    if (true)
                    {
                        string role = si.Role;
                        string function = si.Function;
                        if (role == "Director")
                        {
                            comm.CommandText = "SELECT DISTINCT StaffInfo.Name FROM StaffInfo INNER JOIN StaffAppraisal ON StaffAppraisal.AppraisalStaffUserID = StaffInfo.UserID";
                        }

                        else if (role == "Officer" && function == "Manager")
                        {
                            comm.CommandText = "SELECT DISTINCT StaffInfo.Name FROM StaffInfo INNER JOIN StaffAppraisal ON StaffAppraisal.AppraisalStaffUserID = StaffInfo.UserID WHERE StaffInfo.Functions != 'Director' AND StaffInfo.Functions != 'AD' AND StaffInfo.Functions != 'DD' AND StaffInfo.Functions != 'Manager'";
                        }

                        else if (role == "Officer")
                        {
                            comm.CommandText = "SELECT DISTINCT StaffInfo.Name FROM StaffInfo INNER JOIN StaffAppraisal ON StaffAppraisal.AppraisalStaffUserID = StaffInfo.UserID WHERE StaffInfo.Functions != 'Director' AND StaffInfo.Functions != 'AD' AND StaffInfo.Functions != 'DD'";
                        }
                        SqlDataReader dr = comm.ExecuteReader();
                        while (dr.Read())
                        {
                            string staffname = dr["Name"].ToString();

                            SqlConnection myconn2 = null;
                            try
                            {
                                myconn2 = new SqlConnection();
                                SqlCommand comm2 = new SqlCommand();
                                myconn2.ConnectionString = connectionString;
                                myconn2.Open();
                                comm2.Connection = myconn2;
                                comm2.CommandText = "SELECT FORMAT(AVG(StaffAppraisal.AppraisalResult), 'N1') AS AvgResult FROM StaffAppraisal INNER JOIN StaffInfo ON StaffAppraisal.AppraisalStaffUserID = StaffInfo.UserID WHERE StaffInfo.Name = @name and StaffAppraisal.SystemEndDate LIKE '" + enddate + "%'";
                                comm2.Parameters.AddWithValue("@name", staffname);
                                SqlDataReader dr2 = comm2.ExecuteReader();
                                while (dr2.Read())
                                {
                                    string avgresult = dr2["AvgResult"].ToString();
                                    graphwidth.Add(staffname);

                                    Chart1.Series[0].Points.AddXY(staffname, avgresult);
                                    Chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = System.Drawing.Color.LightGray;
                                    Chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = System.Drawing.Color.LightGray;
                                    Chart1.Visible = true;

                                    Chart2.Series[0].Points.AddXY(staffname, avgresult);
                                    Chart2.ChartAreas[0].AxisX.MajorGrid.LineColor = System.Drawing.Color.LightGray;
                                    Chart2.ChartAreas[0].AxisY.MajorGrid.LineColor = System.Drawing.Color.LightGray;
                                    Chart2.Visible = true;
                                }
                                dr2.Close();
                            }
                            catch (SqlException)
                            {
                            }
                            finally
                            {
                                myconn2.Close();
                            }
                        }
                        dr.Close();
                    }
                }
                catch (SqlException)
                {
                }
                finally
                {
                    myconn.Close();
                }
            }
            else if (question == "All Questions" && section != "All Sections" && enddate != "<----Please select one---->")
            {
                SqlConnection myconn = null;
                try
                {
                    myconn = new SqlConnection();
                    SqlCommand comm = new SqlCommand();
                    myconn.ConnectionString = connectionString;
                    myconn.Open();
                    comm.Connection = myconn;
                    //comm.CommandText = "SELECT DISTINCT StaffInfo.Name FROM StaffInfo INNER JOIN StaffAppraisal ON StaffAppraisal.AppraisalStaffUserID = StaffInfo.UserID WHERE (StaffInfo.Section = @section OR StaffInfo.Section LIKE'%'+ @section OR StaffInfo.Section LIKE'%'+ @section+'%' OR StaffInfo.Section LIKE @section+'%') WHERE StaffInfo.Functions != 'Director' AND StaffInfo.Functions != 'AD' AND StaffInfo.Functions != 'DD'";
                    staffinfo si = dbmanager.GetStaffDetailsViaUid(Session["LoginName"].ToString());
                    if (true)
                    {
                        string role = si.Role;
                        string function = si.Function;
                        if (role == "Director")
                        {
                            comm.CommandText = "SELECT DISTINCT StaffInfo.Name FROM StaffInfo INNER JOIN StaffAppraisal ON StaffAppraisal.AppraisalStaffUserID = StaffInfo.UserID WHERE (StaffInfo.Section = @section OR StaffInfo.Section LIKE'%'+ @section OR StaffInfo.Section LIKE'%'+ @section+'%' OR StaffInfo.Section LIKE @section+'%')";
                        }
                        else if (role == "Officer" && function == "Manager")
                        {
                            comm.CommandText = "SELECT DISTINCT StaffInfo.Name FROM StaffInfo INNER JOIN StaffAppraisal ON StaffAppraisal.AppraisalStaffUserID = StaffInfo.UserID WHERE (StaffInfo.Section = @section OR StaffInfo.Section LIKE'%'+ @section OR StaffInfo.Section LIKE'%'+ @section+'%' OR StaffInfo.Section LIKE @section+'%') AND StaffInfo.Functions != 'Director' AND StaffInfo.Functions != 'AD' AND StaffInfo.Functions != 'DD' AND StaffInfo.Functions != 'Manager'";
                        }
                        else if (role == "Officer")
                        {
                            comm.CommandText = "SELECT DISTINCT StaffInfo.Name FROM StaffInfo INNER JOIN StaffAppraisal ON StaffAppraisal.AppraisalStaffUserID = StaffInfo.UserID WHERE (StaffInfo.Section = @section OR StaffInfo.Section LIKE'%'+ @section OR StaffInfo.Section LIKE'%'+ @section+'%' OR StaffInfo.Section LIKE @section+'%') AND StaffInfo.Functions != 'Director' AND StaffInfo.Functions != 'AD' AND StaffInfo.Functions != 'DD'";
                        }
                        comm.Parameters.AddWithValue("@section", section);
                        SqlDataReader dr = comm.ExecuteReader();
                        while (dr.Read())
                        {
                            string staffname = dr["Name"].ToString();
                            SqlConnection myconn2 = null;
                            try
                            {
                                myconn2 = new SqlConnection();
                                SqlCommand comm2 = new SqlCommand();
                                myconn2.ConnectionString = connectionString;
                                myconn2.Open();
                                comm2.Connection = myconn2;
                                comm2.CommandText = "SELECT FORMAT(AVG(StaffAppraisal.AppraisalResult), 'N1') AS AvgResult FROM StaffAppraisal INNER JOIN StaffInfo ON StaffAppraisal.AppraisalStaffUserID = StaffInfo.UserID WHERE StaffInfo.Name = @name and StaffAppraisal.SystemEndDate LIKE '" + enddate + "%'";
                                comm2.Parameters.AddWithValue("@name", staffname);

                                SqlDataReader dr2 = comm2.ExecuteReader();
                                while (dr2.Read())
                                {
                                    string avgresult = dr2["AvgResult"].ToString();

                                    graphwidth.Add(staffname);
                                    Chart1.Series[0].Points.AddXY(staffname, avgresult);
                                    Chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = System.Drawing.Color.LightGray;
                                    Chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = System.Drawing.Color.LightGray;
                                    Chart1.Visible = true;

                                    Chart2.Series[0].Points.AddXY(staffname, avgresult);
                                    Chart2.ChartAreas[0].AxisX.MajorGrid.LineColor = System.Drawing.Color.LightGray;
                                    Chart2.ChartAreas[0].AxisY.MajorGrid.LineColor = System.Drawing.Color.LightGray;
                                    Chart2.Visible = true;
                                }
                                dr2.Close();
                            }
                            catch (SqlException)
                            {
                            }
                            finally
                            {
                                myconn2.Close();
                            }
                        }
                        dr.Close();
                    }
                }
                catch (SqlException)
                {
                }
                finally
                {
                    myconn.Close();
                }
            }
            else if (question != "All Questions" && section == "All Sections" && enddate != "<----Please select one---->")
            {
                int qid = dbmanager.GetQuestionIDFromQuestion(question);

                SqlConnection myconn = null;
                try
                {
                    myconn = new SqlConnection();
                    SqlCommand comm = new SqlCommand();
                    myconn.ConnectionString = connectionString;
                    myconn.Open();
                    comm.Connection = myconn;
                    //comm.CommandText = "SELECT DISTINCT StaffInfo.Name FROM StaffInfo INNER JOIN StaffAppraisal ON StaffAppraisal.AppraisalStaffUserID = StaffInfo.UserID WHERE StaffAppraisal.AppraisalQuestionID = @qid AND StaffInfo.Functions != 'Director' AND StaffInfo.Functions != 'AD' AND StaffInfo.Functions != 'DD'";
                    staffinfo si = dbmanager.GetStaffDetailsViaUid(Session["LoginName"].ToString());
                    if (true)
                    {
                        string role = si.Role;
                        string function = si.Function;

                        if (role == "Director")
                        {
                            comm.CommandText = "SELECT DISTINCT StaffInfo.Name FROM StaffInfo INNER JOIN StaffAppraisal ON StaffAppraisal.AppraisalStaffUserID = StaffInfo.UserID WHERE StaffAppraisal.AppraisalQuestionID = @qid";
                        }
                        else if (role == "Officer" && function == "Manager")
                        {
                            comm.CommandText = "SELECT DISTINCT StaffInfo.Name FROM StaffInfo INNER JOIN StaffAppraisal ON StaffAppraisal.AppraisalStaffUserID = StaffInfo.UserID WHERE StaffAppraisal.AppraisalQuestionID = @qid AND StaffInfo.Functions != 'Director' AND StaffInfo.Functions != 'AD' AND StaffInfo.Functions != 'DD' AND StaffInfo.Functions != 'Manager'";
                        }
                        else if (role == "Officer")
                        {
                            comm.CommandText = "SELECT DISTINCT StaffInfo.Name FROM StaffInfo INNER JOIN StaffAppraisal ON StaffAppraisal.AppraisalStaffUserID = StaffInfo.UserID WHERE StaffAppraisal.AppraisalQuestionID = @qid AND StaffInfo.Functions != 'Director' AND StaffInfo.Functions != 'AD' AND StaffInfo.Functions != 'DD'";
                        }
                        comm.Parameters.AddWithValue("@qid", qid);

                        SqlDataReader dr = comm.ExecuteReader();
                        while (dr.Read())
                        {
                            string staffname = dr["Name"].ToString();

                            SqlConnection myconn2 = null;
                            try
                            {
                                myconn2 = new SqlConnection();
                                SqlCommand comm2 = new SqlCommand();
                                myconn2.ConnectionString = connectionString;
                                myconn2.Open();
                                comm2.Connection = myconn2;
                                comm2.CommandText = "SELECT FORMAT(AVG(StaffAppraisal.AppraisalResult), 'N1') AS AvgResult FROM StaffAppraisal INNER JOIN StaffInfo ON StaffAppraisal.AppraisalStaffUserID = StaffInfo.UserID WHERE StaffAppraisal.AppraisalQuestionID = @qid AND StaffInfo.Name = @name and StaffAppraisal.SystemEndDate LIKE '" + enddate + "%'";
                                comm2.Parameters.AddWithValue("@qid", qid);
                                comm2.Parameters.AddWithValue("@name", staffname);
                                SqlDataReader dr2 = comm2.ExecuteReader();
                                while (dr2.Read())
                                {
                                    string avgresult = dr2["AvgResult"].ToString();

                                    graphwidth.Add(staffname);

                                    Chart1.Series[0].Points.AddXY(staffname, avgresult);
                                    Chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = System.Drawing.Color.LightGray;
                                    Chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = System.Drawing.Color.LightGray;
                                    Chart1.Visible = true; 

                                    Chart2.Series[0].Points.AddXY(staffname, avgresult);
                                    Chart2.ChartAreas[0].AxisX.MajorGrid.LineColor = System.Drawing.Color.LightGray;
                                    Chart2.ChartAreas[0].AxisY.MajorGrid.LineColor = System.Drawing.Color.LightGray;
                                    Chart2.Visible = true;
                                }
                                dr2.Close();
                            }
                            catch (SqlException)
                            {
                            }
                            finally
                            {
                                myconn2.Close();
                            }
                        }
                        dr.Close();
                    }
                }
                catch (SqlException)
                {
                }
                finally
                {
                    myconn.Close();
                }
            }
            // chart.Series[0].Points.DataBindXY(xAxis, yAxis);
            // where xAxis is List<String> and yAxis is List<Double>
            int qn = ddlQuestions.SelectedIndex;
            string sctn = ddlSections.SelectedValue;

            ArrayList namelist = dbmanager.GetAllNames();
            

            foreach (String stfname in namelist)
            {
                
            }
            

            int graphwidthx = graphwidth.Count * 150;

            Chart1.Width = graphwidthx;
            Chart1.Height = 600;
            Chart1.ChartAreas[0].AxisY.Title = "Average Score Recorded";
            Chart1.ChartAreas[0].AxisX.Title = "Name of Staffs";
            Chart1.ChartAreas[0].AxisX.Interval = 1;
            Chart1.Series[0].Label = "A: #VALY";

            Chart2.Width = graphwidthx;
            Chart2.Height = 600;
            Chart2.ChartAreas[0].AxisY.Title = "Average Score Recorded";
            Chart2.ChartAreas[0].AxisX.Title = "Name of Staffs";
            Chart2.ChartAreas[0].AxisX.Interval = 1;
            Chart2.Series[0].Label = "A: #VALY";

            if (CheckBox1.Checked)
            {
                if (CheckBox2.Checked)
                {
                    Chart1.Visible = true;
                    Chart2.Visible = true;
                }
                else
                {
                    Chart1.Visible = true;
                    Chart2.Visible = false;
                }
            }
            else if (CheckBox2.Checked)
            {
                Chart1.Visible = false;
                Chart2.Visible = true;
            }
            else
            {
                CheckBox1.Checked = true;
                Chart1.Visible = true;
                Chart2.Visible = false;
            }
            if (question == "<----Please select one---->" || section == "<----Please select one---->")
            {
                Chart1.Visible = false;
                Chart2.Visible = false;
            }
        }
        protected void exportToExcel_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            string question = ddlQuestions.SelectedValue;
            string section = ddlSections.SelectedValue;
            string enddate = ddlPeriod.SelectedValue;
            ArrayList results = new ArrayList();
            ArrayList appraisalID = new ArrayList();
            ArrayList names = new ArrayList();
            ArrayList graphwidth = new ArrayList();
            if (question != "All Questions" && section != "All Sections" && enddate != "<----Please select one---->")
            {
                int qid = dbmanager.GetQuestionIDFromQuestion(question);

                SqlConnection myconn = null;
                try
                {
                    myconn = new SqlConnection();
                    SqlCommand comm = new SqlCommand();
                    myconn.ConnectionString = connectionString;
                    myconn.Open();
                    comm.Connection = myconn;
                    comm.CommandText = "SELECT DISTINCT StaffInfo.Name FROM StaffInfo INNER JOIN StaffAppraisal ON StaffAppraisal.AppraisalStaffUserID = StaffInfo.UserID WHERE StaffAppraisal.AppraisalQuestionID = @qid AND (StaffInfo.Section = @section OR StaffInfo.Section LIKE'%'+ @section OR StaffInfo.Section LIKE'%'+ @section+'%' AND StaffAppraisal.SystemEndDate = @enddate)";
                    comm.Parameters.AddWithValue("@qid", qid);
                    comm.Parameters.AddWithValue("@section", section);
                    comm.Parameters.AddWithValue("@enddate", enddate);

                    SqlDataReader dr = comm.ExecuteReader();
                    while (dr.Read())
                    {
                        string staffname = dr["Name"].ToString();

                        SqlConnection myconn2 = null;
                        try
                        {
                            myconn2 = new SqlConnection();
                            SqlCommand comm2 = new SqlCommand();
                            myconn2.ConnectionString = connectionString;
                            myconn2.Open();
                            comm2.Connection = myconn2;
                            comm2.CommandText = "SELECT FORMAT(AVG(StaffAppraisal.AppraisalResult), 'N1') AS AvgResult FROM StaffAppraisal INNER JOIN StaffInfo ON StaffAppraisal.AppraisalStaffUserID = StaffInfo.UserID WHERE StaffAppraisal.AppraisalQuestionID = @qid AND StaffInfo.Name = @name and StaffAppraisal.SystemEndDate LIKE '" + enddate + "%'";
                            comm2.Parameters.AddWithValue("@qid", qid);
                            comm2.Parameters.AddWithValue("@name", staffname);

                            SqlDataReader dr2 = comm2.ExecuteReader();
                            while (dr2.Read())
                            {
                                string avgresult = dr2["AvgResult"].ToString();

                                results.Add(avgresult);
                                names.Add(staffname);
                            }
                            dr2.Close();
                        }
                        catch (SqlException)
                        {
                        }
                        finally
                        {
                            myconn2.Close();
                        }
                    }
                    dr.Close();
                }
                catch (SqlException)
                {
                }
                finally
                {
                    myconn.Close();
                }
            }
            else if (question == "All Questions" && section == "All Sections" && enddate != "<----Please select one---->")
            {
                SqlConnection myconn = null;
                try
                {
                    myconn = new SqlConnection();
                    SqlCommand comm = new SqlCommand();
                    myconn.ConnectionString = connectionString;
                    myconn.Open();
                    comm.Connection = myconn;
                    comm.CommandText = "SELECT DISTINCT StaffInfo.Name FROM StaffInfo INNER JOIN StaffAppraisal ON StaffAppraisal.AppraisalStaffUserID = StaffInfo.UserID";

                    SqlDataReader dr = comm.ExecuteReader();
                    while (dr.Read())
                    {
                        string staffname = dr["Name"].ToString();

                        SqlConnection myconn2 = null;
                        try
                        {
                            myconn2 = new SqlConnection();
                            SqlCommand comm2 = new SqlCommand();
                            myconn2.ConnectionString = connectionString;
                            myconn2.Open();
                            comm2.Connection = myconn2;
                            comm2.CommandText = "SELECT FORMAT(AVG(StaffAppraisal.AppraisalResult), 'N1') AS AvgResult FROM StaffAppraisal INNER JOIN StaffInfo ON StaffAppraisal.AppraisalStaffUserID = StaffInfo.UserID WHERE StaffInfo.Name = @name and StaffAppraisal.SystemEndDate LIKE '" + enddate + "%'";
                            comm2.Parameters.AddWithValue("@name", staffname);

                            SqlDataReader dr2 = comm2.ExecuteReader();
                            while (dr2.Read())
                            {
                                string avgresult = dr2["AvgResult"].ToString();

                                results.Add(avgresult);
                                names.Add(staffname);
                            }
                            dr2.Close();
                        }
                        catch (SqlException)
                        {
                        }
                        finally
                        {
                            myconn2.Close();
                        }
                    }
                    dr.Close();
                }
                catch (SqlException)
                {
                }
                finally
                {
                    myconn.Close();
                }
            }
            else if (question == "All Questions" && section != "All Sections" && enddate != "<----Please select one---->")
            {
                SqlConnection myconn = null;
                try
                {
                    myconn = new SqlConnection();
                    SqlCommand comm = new SqlCommand();
                    myconn.ConnectionString = connectionString;
                    myconn.Open();
                    comm.Connection = myconn;
                    comm.CommandText = "SELECT DISTINCT StaffInfo.Name FROM StaffInfo INNER JOIN StaffAppraisal ON StaffAppraisal.AppraisalStaffUserID = StaffInfo.UserID WHERE StaffAppraisal.AppraisalQuestionID = @qid AND (StaffInfo.Section = @section OR StaffInfo.Section LIKE'%'+ @section OR StaffInfo.Section LIKE'%'+ @section+'%' AND StaffAppraisal.SystemEndDate = @enddate)";
                    comm.Parameters.AddWithValue("@section", section);
                    comm.Parameters.AddWithValue("@enddate", enddate);

                    SqlDataReader dr = comm.ExecuteReader();
                    while (dr.Read())
                    {
                        string staffname = dr["Name"].ToString();

                        SqlConnection myconn2 = null;
                        try
                        {
                            myconn2 = new SqlConnection();
                            SqlCommand comm2 = new SqlCommand();
                            myconn2.ConnectionString = connectionString;
                            myconn2.Open();
                            comm2.Connection = myconn2;
                            comm2.CommandText = "SELECT FORMAT(AVG(StaffAppraisal.AppraisalResult), 'N1') AS AvgResult FROM StaffAppraisal INNER JOIN StaffInfo ON StaffAppraisal.AppraisalStaffUserID = StaffInfo.UserID WHERE StaffInfo.Name = @name and StaffAppraisal.SystemEndDate LIKE '" + enddate + "%'";
                            comm2.Parameters.AddWithValue("@name", staffname);

                            SqlDataReader dr2 = comm2.ExecuteReader();
                            while (dr2.Read())
                            {
                                string avgresult = dr2["AvgResult"].ToString();
                                results.Add(avgresult);
                                names.Add(staffname);
                            }
                            dr2.Close();
                        }
                        catch (SqlException)
                        {
                        }
                        finally
                        {
                            myconn2.Close();
                        }
                    }
                    dr.Close();
                }
                catch (SqlException)
                {
                }
                finally
                {
                    myconn.Close();
                }
            }
            else if (question != "All Questions" && section == "All Sections" && enddate != "<----Please select one---->")
            {
                int qid = dbmanager.GetQuestionIDFromQuestion(question);

                SqlConnection myconn = null;
                try
                {
                    myconn = new SqlConnection();
                    SqlCommand comm = new SqlCommand();
                    myconn.ConnectionString = connectionString;
                    myconn.Open();
                    comm.Connection = myconn;
                    comm.CommandText = "SELECT DISTINCT StaffInfo.Name FROM StaffInfo INNER JOIN StaffAppraisal ON StaffAppraisal.AppraisalStaffUserID = StaffInfo.UserID WHERE StaffAppraisal.AppraisalQuestionID = @qid";
                    comm.Parameters.AddWithValue("@qid", qid);

                    SqlDataReader dr = comm.ExecuteReader();
                    while (dr.Read())
                    {
                        string staffname = dr["Name"].ToString();

                        SqlConnection myconn2 = null;
                        try
                        {
                            myconn2 = new SqlConnection();
                            SqlCommand comm2 = new SqlCommand();
                            myconn2.ConnectionString = connectionString;
                            myconn2.Open();
                            comm2.Connection = myconn2;
                            comm2.CommandText = "SELECT FORMAT(AVG(StaffAppraisal.AppraisalResult), 'N1') AS AvgResult FROM StaffAppraisal INNER JOIN StaffInfo ON StaffAppraisal.AppraisalStaffUserID = StaffInfo.UserID WHERE StaffAppraisal.AppraisalQuestionID = @qid AND StaffInfo.Name = @name and StaffAppraisal.SystemEndDate LIKE '" + enddate + "%'";
                            comm2.Parameters.AddWithValue("@qid", qid);
                            comm2.Parameters.AddWithValue("@name", staffname);

                            SqlDataReader dr2 = comm2.ExecuteReader();
                            while (dr2.Read())
                            {
                                string avgresult = dr2["AvgResult"].ToString();

                                results.Add(avgresult);
                                names.Add(staffname);
                            }
                            dr2.Close();
                        }
                        catch (SqlException)
                        {
                        }
                        finally
                        {
                            myconn2.Close();
                        }
                    }
                    dr.Close();
                }
                catch (SqlException)
                {
                }
                finally
                {
                    myconn.Close();
                }
            }
            try
            {
                var excelApp = new Excel.Application();
                // Make the object visible.
                excelApp.Visible = true;

                // Create a new, empty workbook and add it to the collection returned  
                // by property Workbooks. The new workbook becomes the active workbook. 
                // Add has an optional parameter for specifying a praticular template.  
                // Because no argument is sent in this example, Add creates a new workbook. 
                excelApp.Workbooks.Add();

                Excel._Worksheet workSheet = (Excel.Worksheet)excelApp.ActiveSheet;

                Excel.ChartObjects xlCharts = (Excel.ChartObjects)workSheet.ChartObjects(Type.Missing);
                //2. Set the position of chart where you need to place inside the Excel sheet

                Excel.ChartObject myChart = (Excel.ChartObject)xlCharts.Add(200, 20, 500, 300);

                //3. create a new chart page to display your value
                Excel.Chart chartPage = myChart.Chart;

                //4.Set the X & Y axis Range of data columns   
                //4.1 it takes Excel A Column as as X axis; Data value is from A20-A30
                //4.2 it takes Excel B Column as as Y axis; Data value is from A20-A30
                int rowa = 1;
                int rowb = 1;

                for (int row = 0; row < names.Count; row++)
                {
                    workSheet.Cells[rowa, "A"] = names[row].ToString();
                    rowa++;
                }
                for (int row2 = 0; row2 < results.Count; row2++)
                {
                    workSheet.Cells[rowb, "B"] = results[row2].ToString();
                    rowb++;
                }
                rowa = rowa - 1;
                rowb = rowb - 1;

                //5.Set the chart Source data from your chart range
                chartPage.SetSourceData(workSheet.Range["A1", "B" + rowb], Excel.XlRowCol.xlColumns);

                //6.select the chart type to render your data values
                chartPage.ChartType = Excel.XlChartType.xlLine;

                //7.If you need to declare the chart title please follow the two steps
                myChart.Chart.HasTitle = true;
                Excel.Axis axis = (Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlValue, Excel.XlAxisGroup.xlPrimary);
                axis.HasTitle = true;
                axis.AxisTitle.Text = "Rating";

                Excel.Axis axiss = (Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlCategory, Excel.XlAxisGroup.xlPrimary);
                axiss.HasTitle = true;
                axiss.AxisTitle.Text = "Names";
                chartPage.ChartTitle.Text = "Line Chart";
            }
            catch
            {
            }
        }
    }
}