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
            if (ddlSections.Items.Count == 0 && Session["Role"].ToString().Equals("Director"))
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
                ArrayList listsection = RemoveDups(listofsection);

                listsection.Insert(0, "<----Please select one---->");
                listsection.Insert(1, "All Sections");
                ddlSections.DataSource = listsection;
                ddlSections.DataBind();
            }
            else if (ddlSections.Items.Count == 0 && Session["Role"].ToString().Equals("Officer"))
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
                ArrayList listsection = RemoveDups(listofsection);

                listsection.Insert(0, "<----Please select one---->");
                ddlSections.DataSource = listsection;
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
        protected void btnView_Click(object sender, EventArgs e)
        {
            string question = ddlQuestions.SelectedValue;
            string section = ddlSections.SelectedValue;
            string enddate = ddlPeriod.SelectedValue;
            string name = "";

            ArrayList results = new ArrayList();
            ArrayList appraisalID = new ArrayList();
            ArrayList names = dbmanager.GetAllNames(section);

            //populateChartForStaff(name, section, question);
            //}
            //protected void populateChartForStaff(string staff, string section, int questionID)
            //{
            //string question = ddlQuestions.SelectedValue;
            //string enddate = ddlPeriod.SelectedValue;

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

                    comm.CommandText = "SELECT DISTINCT StaffInfo.Name FROM StaffInfo INNER JOIN StaffAppraisal ON StaffAppraisal.AppraisalStaffUserID = StaffInfo.UserID WHERE StaffAppraisal.AppraisalQuestionID = @qid AND (StaffInfo.Section = @section OR StaffInfo.Section LIKE'%'+ @section OR StaffInfo.Section LIKE'%'+ @section+'%' AND StaffAppraisal.SystemEndDate LIKE '" + enddate + "%')";
                    comm.Parameters.AddWithValue("@qid", qid);
                    comm.Parameters.AddWithValue("@section", section);
                    //comm.Parameters.AddWithValue("@enddate", enddate);

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

                            //comm2.Parameters.AddWithValue("@enddate", enddate);

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

                                /**SqlConnection myconn3 = null;
                                try
                                {
                                    myconn3 = new SqlConnection();
                                    SqlCommand comm3 = new SqlCommand();
                                    myconn3.ConnectionString = connectionString;
                                    myconn3.Open();
                                    comm3.Connection = myconn3;
                                    if (questionID == 0)
                                    {
                                        comm3.CommandText = "select STDEV(stfp.AppraisalResult)as TOTAL from StaffAppraisal stfp, StaffInfo s where stfp.AppraisalStaffUserID=s.UserID and s.Name=@name and s.Section=@section and stfp.SystemEndDateLIKE '" + enddate + "%'";
                                    }
                                    else
                                    {
                                        comm3.CommandText = "select STDEV(stfp.AppraisalResult)as TOTAL from StaffAppraisal stfp, StaffInfo s where stfp.AppraisalStaffUserID=s.UserID and s.Name=@name s.Section=@section and stfp.AppraisalQuestionID=@qID and stfp.SystemEndDateLIKE '" + enddate + "%'";
                                    }
                                    comm3.Parameters.AddWithValue("@name", staff);
                                    comm3.Parameters.AddWithValue("@section", section);
                                    comm3.Parameters.AddWithValue("@qID", questionID);
                                    SqlDataReader dr3 = comm3.ExecuteReader();
                                    while (dr3.Read())
                                    {
                                        string stddev = dr3["StdDeviation"].ToString();

                                        for (int i = 0; i < Chart1.Series[0].Points.Count; i++)
                                        {
                                            Chart1.Series[0].Points[i].Label = "A: #VALY";
                                        }

                                    }
                                    dr3.Close();
                                }
                                catch (SqlException)
                                {
                                }
                                finally
                                {
                                    myconn3.Close();
                                }**/
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

                        else if (role == "Officer" && function == "DD")
                        {
                            comm.CommandText = "SELECT DISTINCT StaffInfo.Name FROM StaffInfo INNER JOIN StaffAppraisal ON StaffAppraisal.AppraisalStaffUserID = StaffInfo.UserID WHERE StaffInfo.Functions != 'Director' AND StaffInfo.Functions != 'DD' AND StaffInfo.Functions != 'AD'";
                        }

                        else if (role == "Officer" && function == "AD")
                        {
                            comm.CommandText = "SELECT DISTINCT StaffInfo.Name FROM StaffInfo INNER JOIN StaffAppraisal ON StaffAppraisal.AppraisalStaffUserID = StaffInfo.UserID WHERE StaffInfo.Functions != 'Director' AND StaffInfo.Functions != 'DD' AND StaffInfo.Functions != 'AD'";
                        }

                        else if (role == "Officer" && function == "Manager")
                        {
                            comm.CommandText = "SELECT DISTINCT StaffInfo.Name FROM StaffInfo INNER JOIN StaffAppraisal ON StaffAppraisal.AppraisalStaffUserID = StaffInfo.UserID WHERE StaffInfo.Functions != 'Director' AND StaffInfo.Functions != 'AD' AND StaffInfo.Functions != 'DD' AND StaffInfo.Functions != 'Manager'";
                        }

                        else if (role == "Officer" && function == "Section Head")
                        {
                            comm.CommandText = "SELECT DISTINCT StaffInfo.Name FROM StaffInfo INNER JOIN StaffAppraisal ON StaffAppraisal.AppraisalStaffUserID = StaffInfo.UserID WHERE StaffInfo.Functions != 'Director' AND StaffInfo.Functions != 'DD' AND StaffInfo.Functions != 'AD' AND StaffInfo.Functions != 'Manager' AND StaffInfo.Functions != 'Section Head'";
                        }
                        else if (role == "Officer")
                        {
                            comm.CommandText = "SELECT DISTINCT StaffInfo.Name FROM StaffInfo INNER JOIN StaffAppraisal ON StaffAppraisal.AppraisalStaffUserID = StaffInfo.UserID WHERE StaffInfo.Functions != 'Director' AND StaffInfo.Functions != 'AD' AND StaffInfo.Functions != 'DD' AND StaffInfo.Functions != 'Manager' AND StaffInfo.Functions != 'Section Head'";
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

                                    /**SqlConnection myconn3 = null;
                                    try
                                    {
                                        myconn3 = new SqlConnection();
                                        SqlCommand comm3 = new SqlCommand();
                                        myconn3.ConnectionString = connectionString;
                                        myconn3.Open();
                                        comm3.Connection = myconn3;
                                        if (questionID == 0)
                                        {
                                            comm3.CommandText = "select STDEV(stfp.AppraisalResult)as TOTAL from StaffAppraisal stfp, StaffInfo s where stfp.AppraisalStaffUserID=s.UserID and s.Name=@name and s.Section=@section and stfp.SystemEndDateLIKE '" + enddate + "%'";
                                        }
                                        else
                                        {
                                            comm3.CommandText = "select STDEV(stfp.AppraisalResult)as TOTAL from StaffAppraisal stfp, StaffInfo s where stfp.AppraisalStaffUserID=s.UserID and s.Name=@name s.Section=@section and stfp.AppraisalQuestionID=@qID and stfp.SystemEndDateLIKE '" + enddate + "%'";
                                        }
                                        comm3.Parameters.AddWithValue("@name", staff);
                                        comm3.Parameters.AddWithValue("@section", section);
                                        comm3.Parameters.AddWithValue("@qID", questionID);
                                        SqlDataReader dr3 = comm3.ExecuteReader();
                                        while (dr3.Read())
                                        {
                                            string stddev = dr3["StdDeviation"].ToString();
                                            for (int i = 0; i < Chart1.Series[0].Points.Count; i++)
                                            {
                                                Chart1.Series[0].Points[i].Label = "A: #VALY";
                                            }

                                        }
                                        dr3.Close();
                                    }
                                    catch (SqlException)
                                    {
                                    }
                                    finally
                                    {
                                        myconn3.Close();
                                    }**/
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
                        else if (role == "Officer" && function == "DD")
                        {
                            comm.CommandText = "SELECT DISTINCT StaffInfo.Name FROM StaffInfo INNER JOIN StaffAppraisal ON StaffAppraisal.AppraisalStaffUserID = StaffInfo.UserID WHERE StaffInfo.Functions != 'Director' AND StaffInfo.Functions != 'DD' AND StaffInfo.Functions != 'AD'";
                        }

                        else if (role == "Officer" && function == "AD")
                        {
                            comm.CommandText = "SELECT DISTINCT StaffInfo.Name FROM StaffInfo INNER JOIN StaffAppraisal ON StaffAppraisal.AppraisalStaffUserID = StaffInfo.UserID WHERE StaffInfo.Functions != 'Director' AND StaffInfo.Functions != 'DD' AND StaffInfo.Functions != 'AD'";
                        }

                        else if (role == "Officer" && function == "Manager")
                        {
                            comm.CommandText = "SELECT DISTINCT StaffInfo.Name FROM StaffInfo INNER JOIN StaffAppraisal ON StaffAppraisal.AppraisalStaffUserID = StaffInfo.UserID WHERE StaffInfo.Functions != 'Director' AND StaffInfo.Functions != 'AD' AND StaffInfo.Functions != 'DD' AND StaffInfo.Functions != 'Manager'";
                        }

                        else if (role == "Officer" && function == "Section Head")
                        {
                            comm.CommandText = "SELECT DISTINCT StaffInfo.Name FROM StaffInfo INNER JOIN StaffAppraisal ON StaffAppraisal.AppraisalStaffUserID = StaffInfo.UserID WHERE StaffInfo.Functions != 'Director' AND StaffInfo.Functions != 'DD' AND StaffInfo.Functions != 'AD' AND StaffInfo.Functions != 'Manager' AND StaffInfo.Functions != 'Section Head'";
                        }
                        else if (role == "Officer")
                        {
                            comm.CommandText = "SELECT DISTINCT StaffInfo.Name FROM StaffInfo INNER JOIN StaffAppraisal ON StaffAppraisal.AppraisalStaffUserID = StaffInfo.UserID WHERE StaffInfo.Functions != 'Director' AND StaffInfo.Functions != 'AD' AND StaffInfo.Functions != 'DD' AND StaffInfo.Functions != 'Manager' AND StaffInfo.Functions != 'Section Head'";
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

                                    /**SqlConnection myconn3 = null;
                                    try
                                    {
                                        myconn3 = new SqlConnection();
                                        SqlCommand comm3 = new SqlCommand();
                                        myconn3.ConnectionString = connectionString;
                                        myconn3.Open();
                                        comm3.Connection = myconn3;
                                        if (questionID == 0)
                                        {
                                            comm3.CommandText = "select STDEV(stfp.AppraisalResult)as TOTAL from StaffAppraisal stfp, StaffInfo s where stfp.AppraisalStaffUserID=s.UserID and s.Name=@name and s.Section=@section and stfp.SystemEndDateLIKE '" + enddate + "%'";
                                        }
                                        else
                                        {
                                            comm3.CommandText = "select STDEV(stfp.AppraisalResult)as TOTAL from StaffAppraisal stfp, StaffInfo s where stfp.AppraisalStaffUserID=s.UserID and s.Name=@name s.Section=@section and stfp.AppraisalQuestionID=@qID and stfp.SystemEndDateLIKE '" + enddate + "%'";
                                        }
                                        comm3.Parameters.AddWithValue("@name", staff);
                                        comm3.Parameters.AddWithValue("@section", section);
                                        comm3.Parameters.AddWithValue("@qID", questionID);
                                        SqlDataReader dr3 = comm3.ExecuteReader();
                                        while (dr3.Read())
                                        {
                                            string stddev = dr3["StdDeviation"].ToString();
                                            for (int i = 0; i < Chart1.Series[0].Points.Count; i++)
                                            {
                                                Chart1.Series[0].Points[i].Label = "A: #VALY";
                                            }

                                        }
                                        dr3.Close();
                                    }
                                    catch (SqlException)
                                    {
                                    }
                                    finally
                                    {
                                        myconn3.Close();
                                    }**/
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
                        else if (role == "Officer" && function == "DD")
                        {
                            comm.CommandText = "SELECT DISTINCT StaffInfo.Name FROM StaffInfo INNER JOIN StaffAppraisal ON StaffAppraisal.AppraisalStaffUserID = StaffInfo.UserID WHERE StaffInfo.Functions != 'Director' AND StaffInfo.Functions != 'DD' AND StaffInfo.Functions != 'AD'";
                        }

                        else if (role == "Officer" && function == "AD")
                        {
                            comm.CommandText = "SELECT DISTINCT StaffInfo.Name FROM StaffInfo INNER JOIN StaffAppraisal ON StaffAppraisal.AppraisalStaffUserID = StaffInfo.UserID WHERE StaffInfo.Functions != 'Director' AND StaffInfo.Functions != 'DD' AND StaffInfo.Functions != 'AD'";
                        }

                        else if (role == "Officer" && function == "Manager")
                        {
                            comm.CommandText = "SELECT DISTINCT StaffInfo.Name FROM StaffInfo INNER JOIN StaffAppraisal ON StaffAppraisal.AppraisalStaffUserID = StaffInfo.UserID WHERE StaffInfo.Functions != 'Director' AND StaffInfo.Functions != 'AD' AND StaffInfo.Functions != 'DD' AND StaffInfo.Functions != 'Manager'";
                        }

                        else if (role == "Officer" && function == "Section Head")
                        {
                            comm.CommandText = "SELECT DISTINCT StaffInfo.Name FROM StaffInfo INNER JOIN StaffAppraisal ON StaffAppraisal.AppraisalStaffUserID = StaffInfo.UserID WHERE StaffInfo.Functions != 'Director' AND StaffInfo.Functions != 'DD' AND StaffInfo.Functions != 'AD' AND StaffInfo.Functions != 'Manager' AND StaffInfo.Functions != 'Section Head'";
                        }
                        else if (role == "Officer")
                        {
                            comm.CommandText = "SELECT DISTINCT StaffInfo.Name FROM StaffInfo INNER JOIN StaffAppraisal ON StaffAppraisal.AppraisalStaffUserID = StaffInfo.UserID WHERE StaffInfo.Functions != 'Director' AND StaffInfo.Functions != 'AD' AND StaffInfo.Functions != 'DD' AND StaffInfo.Functions != 'Manager' AND StaffInfo.Functions != 'Section Head'";
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

                                    /**SqlConnection myconn3 = null;
                                    try
                                    {
                                        myconn3 = new SqlConnection();
                                        SqlCommand comm3 = new SqlCommand();
                                        myconn3.ConnectionString = connectionString;
                                        myconn3.Open();
                                        comm3.Connection = myconn3;
                                        if (questionID == 0)
                                        {
                                            comm3.CommandText = "select STDEV(stfp.AppraisalResult)as TOTAL from StaffAppraisal stfp, StaffInfo s where stfp.AppraisalStaffUserID=s.UserID and s.Name=@name and s.Section=@section and stfp.SystemEndDateLIKE '" + enddate + "%'";
                                        }
                                        else
                                        {
                                            comm3.CommandText = "select STDEV(stfp.AppraisalResult)as TOTAL from StaffAppraisal stfp, StaffInfo s where stfp.AppraisalStaffUserID=s.UserID and s.Name=@name s.Section=@section and stfp.AppraisalQuestionID=@qID and stfp.SystemEndDateLIKE '" + enddate + "%'";
                                        }
                                        comm3.Parameters.AddWithValue("@name", staff);
                                        comm3.Parameters.AddWithValue("@section", section);
                                        comm3.Parameters.AddWithValue("@qID", questionID);
                                        SqlDataReader dr3 = comm3.ExecuteReader();
                                        while (dr3.Read())
                                        {
                                            string stddev = dr3["StdDeviation"].ToString();
                                            for (int i = 0; i < Chart1.Series[0].Points.Count; i++)
                                            {
                                                Chart1.Series[0].Points[i].Label = "A: #VALY";
                                            }

                                        }
                                        dr3.Close();
                                    }
                                    catch (SqlException)
                                    {
                                    }
                                    finally
                                    {
                                        myconn3.Close();
                                    }**/
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
        protected void exportToExcel_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=GraphReports.xls");
            Response.ContentType = "application/vnd.ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            Chart1.RenderControl(hw);

            string tmpChartName = "LineChart.jpg";
            string imgPath = HttpContext.Current.Request.PhysicalApplicationPath + tmpChartName;
            Chart1.SaveImage(imgPath);

            string src = tmpChartName;
            string img = string.Format("<img src = '{0}{1}' />", HttpContext.Current.Request.PhysicalApplicationPath, src);

            Table table = new Table();
            TableRow row = new TableRow();
            Unit width = new Unit(500, UnitType.Pixel);
            row.Cells.Add(new TableCell());
            row.Cells[0].Width = width;
            //row.Cells[0].Controls.Add(new Label { Text = ChartTitle.Text, ForeColor = Color.Red, });
            table.Rows.Add(row);
            row = new TableRow();
            row.Cells.Add(new TableCell());
            row.Cells[0].Controls.Add(new Literal { Text = img });
            table.Rows.Add(row);

            sw = new StringWriter();
            hw = new HtmlTextWriter(sw);
            table.RenderControl(hw);
            Response.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }
        /* else if (CheckBox2.Checked)
         {
             Response.Clear();
             Response.Buffer = true;
             Response.AddHeader("content-disposition", "attachment;filename=ViewHistoryChart.xls");
             Response.ContentType = "application/vnd.ms-excel";
             Response.Charset = "";
             StringWriter sw = new StringWriter();
             HtmlTextWriter hw = new HtmlTextWriter(sw);
             Chart2.RenderControl(hw);

             string tmpChartName = "BarChart.jpg";
             string imgPath = HttpContext.Current.Request.PhysicalApplicationPath + tmpChartName;
             Chart2.SaveImage(imgPath);

             string src = tmpChartName;
             string img = string.Format("<img src = '{0}{1}' />", HttpContext.Current.Request.PhysicalApplicationPath, src);

             Table table = new Table();
             TableRow row = new TableRow();
             Unit width = new Unit(500, UnitType.Pixel);
             row.Cells.Add(new TableCell());
             row.Cells[0].Width = width;
             //row.Cells[0].Controls.Add(new Label { Text = ChartTitle.Text, ForeColor = Color.Red, });
             table.Rows.Add(row);
             row = new TableRow();
             row.Cells.Add(new TableCell());
             //row.Cells[0].Controls.Add(new Literal { Text = lblSectionFunctionSelected.Text });
             table.Rows.Add(row);
             row = new TableRow();
             row.Cells.Add(new TableCell());
             row.Cells[0].Controls.Add(new Literal { Text = img });
             table.Rows.Add(row);
             row.Cells.Add(new TableCell());
             //row.Cells[0].Controls.Add(new Literal { Text = lbllegendHistory.Text });
             table.Rows.Add(row);

             sw = new StringWriter();
             hw = new HtmlTextWriter(sw);
             table.RenderControl(hw);
             Response.Write(sw.ToString());
             Response.Flush();
             Response.End();
         }
         else if (CheckBox1.Checked && CheckBox2.Checked)
         {
             Response.Clear();
             Response.Buffer = true;
             Response.AddHeader("content-disposition", "attachment;filename=ViewHistoryChart.xls");
             Response.ContentType = "application/vnd.ms-excel";
             Response.Charset = "";
             StringWriter sw = new StringWriter();
             HtmlTextWriter hw = new HtmlTextWriter(sw);
             Chart1.RenderControl(hw);
             Chart2.RenderControl(hw);

             string tmpChartName = "LineChart.jpg";
             string imgPath = HttpContext.Current.Request.PhysicalApplicationPath + tmpChartName;
             Chart1.SaveImage(imgPath);

             string ChartName = "BarChart.jpg";
             string path = HttpContext.Current.Request.PhysicalApplicationPath + ChartName;
             Chart2.SaveImage(path);

             string src = tmpChartName;
             string lineimg = string.Format("<img src = '{0}{1}' />", HttpContext.Current.Request.PhysicalApplicationPath, src);
             string source = ChartName;
             string barimg = string.Format("<img src = '{0}{1}' />", HttpContext.Current.Request.PhysicalApplicationPath, source);

             Table table = new Table();
             TableRow row = new TableRow();
             Unit width = new Unit(500, UnitType.Pixel);
             row.Cells.Add(new TableCell());
             row.Cells[0].Width = width;
             //row.Cells[0].Controls.Add(new Label { Text = ChartTitle.Text, ForeColor = Color.Red, });
             table.Rows.Add(row);
             row = new TableRow();
             row.Cells.Add(new TableCell());
             //row.Cells[0].Controls.Add(new Literal { Text = lblSectionFunctionSelected.Text });
             table.Rows.Add(row);
             row = new TableRow();
             row.Cells.Add(new TableCell());
             row.Cells[0].Controls.Add(new Literal { Text = lineimg });
             table.Rows.Add(row);
             row.Cells.Add(new TableCell());
             row.Cells[0].Controls.Add(new Literal { Text = barimg });
             table.Rows.Add(row);

             sw = new StringWriter();
             hw = new HtmlTextWriter(sw);
             table.RenderControl(hw);
             Response.Write(sw.ToString());
             Response.Flush();
             Response.End();
         } */
    }
}
