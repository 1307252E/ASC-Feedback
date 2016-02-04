using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Collections;

namespace _360_Staff_Survey_Web.Class
{
    public class dbmanager
    {
        public static string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Appraisal_System"].ConnectionString.ToString();
        public static DateTime CheckDateTimeLike(DateTime date)
        {
            SqlConnection myconn = null;
            DateTime result = new DateTime();
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;

                if (date.Month < 10)
                {
                    comm.CommandText = "select Distinct SystemEndDate from StaffAppraisal where SystemEndDate LIKE '%" + date.Year + "-_" + date.Month + "%'";
                }
                else
                {
                    comm.CommandText = "select Distinct SystemEndDate from StaffAppraisal where SystemEndDate LIKE '%" + date.Year + "-" + date.Month + "%'";
                }

                SqlDataReader dr = comm.ExecuteReader();
                if (dr.Read())
                {
                    result = (DateTime)dr["SystemEndDate"];
                }
                dr.Close();
            }
            catch (SqlException)
            {
                return result;
            }
            finally
            {
                myconn.Close();
            }
            return result;
        }
        //Login Module
        public static bool CheckValidCurrentPassword(string userid, string passw)
        {
            SqlConnection myconn = null;
            bool result = false;
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;

                comm.CommandText = "select COUNT(*) AS TOTAL from Login where UserID=@userid and Password=@passw";
                comm.Parameters.AddWithValue("@userid", userid);
                comm.Parameters.AddWithValue("@passw", passw);

                SqlDataReader dr = comm.ExecuteReader();
                if (dr.Read())
                {
                    int total = Convert.ToInt32(dr["TOTAL"]);
                    if (total > 0)
                    {
                        result = true;
                    }
                }
                dr.Close();
            }
            catch (SqlException)
            {
                return result;
            }
            finally
            {
                myconn.Close();
            }
            return result;
        }
        public static bool UpdateAutoEmail(DateTime date, string sendcount)
        {
            bool updateresult = false;

            int rowsAffected = 0;
            SqlConnection myconn = null;
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;

                comm.CommandText = "Update AutoEmail set AutoEmailDate=@date, AutoEmailCount=@count";

                comm.Parameters.AddWithValue("@date", date);
                comm.Parameters.AddWithValue("@count", sendcount);
                rowsAffected = comm.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    updateresult = true;
                }
            }
            catch
            {

            }
            finally
            {
                myconn.Close();
            }
            return updateresult;
        }

        public static bool UpdateAutoEmails(DateTime date, string sendcount)
        {
            bool updateresult = false;
            int rowsAffected = 0;
            SqlConnection myconn = null;
            int k = 20;
            for (int i = 1; i < k; i++)
            {
                try
                {
                    myconn = new SqlConnection();
                    SqlCommand comm = new SqlCommand();
                    myconn.ConnectionString = connectionString;
                    myconn.Open();
                    comm.Connection = myconn;

                    comm.CommandText = "Update AutoEmail set AutoEmailDate=@date, AutoEmailCount=@count where AutoEmailID=@i";
                    comm.Parameters.AddWithValue("@i", i);
                    comm.Parameters.AddWithValue("@date", date);
                    comm.Parameters.AddWithValue("@count", sendcount);
                    rowsAffected = comm.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        updateresult = true;
                    }
                }
                catch
                {

                }

                finally
                {
                    myconn.Close();
                }
            }
            return updateresult;
        }
        public static bool UpdatePassw(string userid, string passw)
        {
            SqlConnection myconn = null;
            bool result = false;
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;

                comm.CommandText = "update Login set Password=@pass where UserID=@userid";
                comm.Parameters.AddWithValue("@userid", userid);
                comm.Parameters.AddWithValue("@pass", passw);
                int rowsAffected = comm.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    result = true;
                }
            }
            catch (SqlException)
            {
                return result;
            }
            finally
            {
                myconn.Close();
            }
            return result;
        }

        public static bool CheckValidUser(string userid)
        {
            SqlConnection myconn = null;
            bool result = false;
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                comm.CommandText = "select COUNT(*) AS TOTAL from Login where UserID=@userid";
                comm.Parameters.AddWithValue("@userid", userid);

                SqlDataReader dr = comm.ExecuteReader();
                if (dr.Read())
                {
                    int total = Convert.ToInt32(dr["TOTAL"]);
                    if (total > 0)
                    {
                        result = true;
                    }
                }
                dr.Close();
            }

            catch (SqlException)
            {
                return result;
            }

            finally
            {
                myconn.Close();
            }
            return result;
        }

        public static bool InsertUserIDandPassw(string userid, string passw)
        {
            bool result = false;
            int rowsAffected = 0;

            try
            {

                SqlConnection myconn = null;
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();

                comm.Connection = myconn;

                comm.CommandText = "insert into dbo.Login" +
               "(UserID,Password) values" +
               "(@uid,@passw)";
                comm.Parameters.AddWithValue("@uid", userid);
                comm.Parameters.AddWithValue("@passw", passw);
                rowsAffected = comm.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    result = true;
                }

                myconn.Close();

                return result;
            }
            catch
            {
                return result;
            }
        }

        public static bool ValidateLogin(string userid, string passw)
        {
            SqlConnection myconn = null;
            bool result = false;
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                comm.CommandText = "select COUNT(*) AS TOTAL from Login where UserID=@userid and Password=@passw and userid in (select userid from StaffInfo)";
                comm.Parameters.AddWithValue("@userid", userid);
                comm.Parameters.AddWithValue("@passw", passw);

                SqlDataReader dr = comm.ExecuteReader();
                if (dr.Read())
                {
                    int total = Convert.ToInt32(dr["TOTAL"]);
                    if (total > 0)
                    {
                        result = true;
                    }
                }
                dr.Close();
            }

            catch (SqlException)
            {
                return result;
            }

            finally
            {
                myconn.Close();
            }
            return result;
        }

        public static ArrayList GetAllSavedAppraisal(string userid, int page)
        {
            SqlConnection myconn = null;
            staffappraisal stf = null;
            ArrayList listofstaff = new ArrayList();
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                //comm.CommandText = "select * from SaveAppraisal sv, StaffInfo s where sv.UserID=s.UserID and sv.userid=@uid and AppraisalQuestionID=@page and AppraisalDate >= (select StartDate from SystemDate) and AppraisalDate <= (select EndDate from SystemDate) order by s.Name, AppraisalQuestionid";
                comm.CommandText = "select * from SaveAppraisal sv where sv.UserID=@uid and sv.userid=@uid and AppraisalQuestionID=@page and AppraisalDate >= (select StartDate from SystemDate) and AppraisalDate <= (select EndDate from SystemDate)";
                comm.Parameters.AddWithValue("@uid", userid);
                comm.Parameters.AddWithValue("@page", page);

                SqlDataReader dr = comm.ExecuteReader();
                while (dr.Read())
                {
                    string uid = dr["UserID"].ToString();
                    int questionid = Convert.ToInt32(dr["AppraisalQuestionID"]);
                    string appstaff = dr["AppraisalStaffUserID"].ToString();
                    double appresult = Convert.ToDouble(dr["AppraisalResult"]);
                    string appremarks = dr["AppraisalRemarks"].ToString();
                    DateTime date = (DateTime)dr["AppraisalDate"];
                    DateTime systemend = (DateTime)dr["SystemEndDate"];
                    stf = new staffappraisal(uid, questionid, appstaff, appresult, appremarks, date, systemend);
                    listofstaff.Add(stf);
                }
                dr.Close();
            }

            catch (SqlException)
            {
                return listofstaff;
            }

            finally
            {
                myconn.Close();
            }
            return listofstaff;
        }

        public static void DeletePreviousSavedByQuestion(string userid, int questionId)
        {
            int rowsAffected = 0;
            SqlConnection myconn = null;
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;

                comm.CommandText = "Delete from SaveAppraisal where UserID=@uid and AppraisalQuestionID = @qid";
                comm.Parameters.AddWithValue("@uid", userid);
                comm.Parameters.AddWithValue("@qid", questionId);
                rowsAffected = comm.ExecuteNonQuery();
            }
            catch
            {

            }
            finally
            {
                myconn.Close();
            }
        }

        public static void DeletePreviousSaved(string userid)
        {
            int rowsAffected = 0;
            SqlConnection myconn = null;
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;

                comm.CommandText = "Delete from SaveAppraisal where UserID=@uid";
                comm.Parameters.AddWithValue("@uid", userid);
                rowsAffected = comm.ExecuteNonQuery();
            }
            catch
            {

            }
            finally
            {
                myconn.Close();
            }
        }

        public static void DeletePreviousAllSaved()
        {
            int rowsAffected = 0;
            SqlConnection myconn = null;
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;

                comm.CommandText = "Delete from SaveAppraisal";
                rowsAffected = comm.ExecuteNonQuery();
            }
            catch
            {

            }
            finally
            {
                myconn.Close();
            }
        }

        public static void InsertSavedData(ArrayList listofappraisal)
        {
            int rowsAffected = 0;

            try
            {
                foreach (staffappraisal app in listofappraisal)
                {
                    SqlConnection myconn = null;

                    myconn = new SqlConnection();
                    SqlCommand comm = new SqlCommand();
                    myconn.ConnectionString = connectionString;
                    myconn.Open();

                    comm.Connection = myconn;

                    comm.CommandText = "insert into dbo.SaveAppraisal" +
                   "(UserID,AppraisalQuestionID,AppraisalStaffUserID,AppraisalResult,AppraisalRemarks,AppraisalDate,SystemEndDate) values" +
                   "(@uid,@qid,@appstfuid,@appresult,@appremarks,@appdate,@enddate)";
                    comm.Parameters.AddWithValue("@uid", app.Uid);
                    comm.Parameters.AddWithValue("@qid", app.Questionid);
                    comm.Parameters.AddWithValue("@appstfuid", app.Appstaffuid);
                    comm.Parameters.AddWithValue("@appresult", app.Appresult);
                    comm.Parameters.AddWithValue("@appremarks", app.Appremarks);
                    comm.Parameters.AddWithValue("@appdate", app.DateTime);
                    comm.Parameters.AddWithValue("@enddate", app.SystemEndTime);
                    rowsAffected = comm.ExecuteNonQuery();

                    myconn.Close();

                }
            }
            catch
            {
            }
        }

        //View History Chart

        public static ArrayList GetDistinctFunctionViaSection(string section)
        {
            SqlConnection myconn = null;
            ArrayList listoffunction = new ArrayList();
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                SqlCommand comm2 = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                comm.CommandText = "select DISTINCT s.Functions from StaffAppraisal stfp, StaffInfo s where stfp.AppraisalStaffUserID=s.UserID and s.Section =@section";
                comm.Parameters.AddWithValue("@section", section);

                SqlDataReader dr = comm.ExecuteReader();
                while (dr.Read())
                {
                    string function = dr["Functions"].ToString();
                    listoffunction.Add(function);
                }
                ArrayList functlist = dbmanager.GetDistinctFunctionLIKESection(section);
                foreach (string function in functlist)
                {
                    listoffunction.Add(function);
                }
                dr.Close();
            }
            catch (SqlException)
            {
                return listoffunction;
            }
            finally
            {
                myconn.Close();
            }
            return listoffunction;
        }

        public static ArrayList GetDistinctFunctionLIKESection(string section)
        {
            SqlConnection myconn = null;
            ArrayList listoffunction = new ArrayList();
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                SqlCommand comm2 = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                comm.CommandText = "select * from StaffInfo where Section Like '%,%' and Section LIKE '%" + section + "%'";
                comm.Parameters.AddWithValue("@section", section);

                SqlDataReader dr = comm.ExecuteReader();
                while (dr.Read())
                {
                    string function = dr["Functions"].ToString();
                    listoffunction.Add(function);
                }
                dr.Close();
            }
            catch (SqlException)
            {
                return listoffunction;
            }
            finally
            {
                myconn.Close();
            }
            return listoffunction;
        }

        public static ArrayList GetDistinctSectionViaFunction(string function)
        {
            SqlConnection myconn = null;
            ArrayList listofsection = new ArrayList();
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                comm.CommandText = "select DISTINCT s.Section from StaffAppraisal stfp, StaffInfo s where stfp.AppraisalStaffUserID=s.UserID and s.Functions = @funct and s.Section!='ALL'";
                comm.Parameters.AddWithValue("@funct", function);

                SqlDataReader dr = comm.ExecuteReader();
                while (dr.Read())
                {
                    string section = dr["Section"].ToString();
                    string[] stsec = section.Split(',');
                    if (stsec.LongLength > 0)
                    {
                        foreach (string sec in stsec)
                        {
                            listofsection.Add(sec);
                        }
                    }
                }
                dr.Close();

            }
            catch (SqlException)
            {
                return listofsection;
            }

            finally
            {
                myconn.Close();
            }
            return listofsection;
        }

        public static ArrayList GetListofDatesViaSection(string section)
        {
            SqlConnection myconn = null;
            ArrayList listofdates = new ArrayList();
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                comm.CommandText = "select DISTINCT SystemEndDate from StaffAppraisal stfp, StaffInfo s where stfp.AppraisalStaffUserID=s.UserID and s.Section=@section order by SystemEndDate";
                comm.Parameters.AddWithValue("@section", section);

                SqlDataReader dr = comm.ExecuteReader();
                while (dr.Read())
                {
                    DateTime dates = Convert.ToDateTime(dr["SystemEndDate"]);
                    listofdates.Add(dates);

                }
                dr.Close();

            }
            catch (SqlException)
            {
                return listofdates;
            }

            finally
            {
                myconn.Close();
            }
            return listofdates;
        }

        public static ArrayList GetListofDatesViaFunction(string function)
        {
            SqlConnection myconn = null;
            ArrayList listofdates = new ArrayList();
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                comm.CommandText = "select DISTINCT SystemEndDate from StaffAppraisal stfp, StaffInfo s where stfp.AppraisalStaffUserID=s.UserID and s.Functions = @funct order by SystemEndDate";
                comm.Parameters.AddWithValue("@funct", function);

                SqlDataReader dr = comm.ExecuteReader();
                while (dr.Read())
                {
                    DateTime dates = Convert.ToDateTime(dr["SystemEndDate"]);
                    listofdates.Add(dates);

                }
                dr.Close();

            }
            catch (SqlException)
            {
                return listofdates;
            }

            finally
            {
                myconn.Close();
            }
            return listofdates;
        }

        public static ArrayList GetListofDatesViaAll()
        {
            SqlConnection myconn = null;
            ArrayList listofdates = new ArrayList();
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                comm.CommandText = "select DISTINCT SystemEndDate from StaffAppraisal stfp, StaffInfo s where stfp.AppraisalStaffUserID=s.UserID order by SystemEndDate";

                SqlDataReader dr = comm.ExecuteReader();
                while (dr.Read())
                {
                    DateTime dates = Convert.ToDateTime(dr["SystemEndDate"]);
                    listofdates.Add(dates);

                }
                dr.Close();

            }
            catch (SqlException)
            {
                return listofdates;
            }

            finally
            {
                myconn.Close();
            }
            return listofdates;
        }

        public static ArrayList GetListofDatesViaAllByFunction(string function)
        {
            SqlConnection myconn = null;
            ArrayList listofdates = new ArrayList();
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                comm.CommandText = "select DISTINCT SystemEndDate from StaffAppraisal stfp, StaffInfo s where stfp.AppraisalStaffUserID=s.UserID and s.Functions=@funct order by SystemEndDate";
                comm.Parameters.AddWithValue("@funct", function);

                SqlDataReader dr = comm.ExecuteReader();
                while (dr.Read())
                {
                    DateTime dates = Convert.ToDateTime(dr["SystemEndDate"]);
                    listofdates.Add(dates);

                }
                dr.Close();

            }
            catch (SqlException)
            {
                return listofdates;
            }

            finally
            {
                myconn.Close();
            }
            return listofdates;
        }

        public static ArrayList GetListofDatesViaAllBySection(string section)
        {
            SqlConnection myconn = null;
            ArrayList listofdates = new ArrayList();
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                comm.CommandText = "select DISTINCT SystemEndDate from StaffAppraisal stfp, StaffInfo s where stfp.AppraisalStaffUserID=s.UserID and s.Section=@sect order by SystemEndDate";
                comm.Parameters.AddWithValue("@sect", section);

                SqlDataReader dr = comm.ExecuteReader();
                while (dr.Read())
                {
                    DateTime dates = Convert.ToDateTime(dr["SystemEndDate"]);
                    listofdates.Add(dates);

                }
                dr.Close();

            }
            catch (SqlException)
            {
                return listofdates;
            }

            finally
            {
                myconn.Close();
            }
            return listofdates;
        }

        public static ArrayList GetListofDatesViaAllBySectionFunction(string section, string function)
        {
            SqlConnection myconn = null;
            ArrayList listofdates = new ArrayList();
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                comm.CommandText = "select DISTINCT SystemEndDate from StaffAppraisal stfp, StaffInfo s where stfp.AppraisalStaffUserID=s.UserID and s.Functions=@funct and s.Section=@sect order by SystemEndDate";
                comm.Parameters.AddWithValue("@sect", section);
                comm.Parameters.AddWithValue("@funct", function);

                SqlDataReader dr = comm.ExecuteReader();
                while (dr.Read())
                {
                    DateTime dates = Convert.ToDateTime(dr["SystemEndDate"]);
                    listofdates.Add(dates);

                }
                dr.Close();

            }
            catch (SqlException)
            {
                return listofdates;
            }

            finally
            {
                myconn.Close();
            }
            return listofdates;
        }

        public static ArrayList GetAllStaffAppraisal()
        {
            SqlConnection myconn = null;
            ArrayList listofappraisal = new ArrayList();
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                comm.CommandText = "select * from StaffAppraisal WHERE AppraisalResult !='0.0'";

                SqlDataReader dr = comm.ExecuteReader();
                while (dr.Read())
                {
                    double result = Convert.ToDouble(dr["AppraisalResult"]);
                    listofappraisal.Add(result);

                }
                dr.Close();

            }
            catch (SqlException)
            {
                return listofappraisal;
            }

            finally
            {
                myconn.Close();
            }
            return listofappraisal;
        }
        public static ArrayList GetAllStaffAppraisalFilterSection(string section)
        {
            SqlConnection myconn = null;
            ArrayList listofappraisal = new ArrayList();
            ArrayList listofSection = new ArrayList();
            ArrayList userID = new ArrayList();
            string counter = "";
            int count = 0;
            string[] strsec = section.Split(',');
            try
            {
                if (strsec.LongLength > 0)
                {

                    foreach (string secti in strsec)
                    {


                        myconn = new SqlConnection();
                        SqlCommand comm = new SqlCommand();
                        myconn.ConnectionString = connectionString;
                        myconn.Open();
                        comm.Connection = myconn;
                        comm.CommandText = "select AppraisalResult,AppraisalStaffUserID from [360DB].[dbo].[StaffAppraisal] stfp, [360DB].[dbo].[StaffInfo] s where stfp.AppraisalStaffUserID = s.UserID AND (s.Section LIKE '%" + secti + "' OR s.Section LIKE '%" + secti + "%' OR s.Section LIKE '" + secti + "%')AND AppraisalResult !=0.0";

                        SqlDataReader dr = comm.ExecuteReader();
                        while (dr.Read())
                        {
                            if (counter == "")
                            {
                                string useridd = Convert.ToString(dr["AppraisalStaffUserID"]);

                                userID.Add(useridd);

                                double result = Convert.ToDouble(dr["AppraisalResult"]);

                                listofappraisal.Add(result);
                            }
                            else
                            {

                                for (int i = 0; i < userID.Count; i++)
                                {
                                    string useridd = Convert.ToString(dr["AppraisalStaffUserID"]);
                                    if (userID[i].ToString() == useridd)
                                    {
                                        count = 1;
                                    }
                                    else
                                    {


                                    }

                                }
                                if (count == 0)
                                {


                                    string useriddd = Convert.ToString(dr["AppraisalStaffUserID"]);
                                    userID.Add(useriddd);

                                    double result = Convert.ToDouble(dr["AppraisalResult"]);

                                    listofappraisal.Add(result);
                                }

                            }

                        }
                        counter = secti;
                        count = 0;
                        dr.Close();

                    }
                }
            }

            catch (SqlException)
            {
                return listofappraisal;
            }

            finally
            {
                myconn.Close();
            }
            return listofappraisal;
        }
        public static ArrayList GetAllStaffAppraisalFilterSectionFunction(string section, string function)
        {
            SqlConnection myconn = null;
            ArrayList listofappraisal = new ArrayList();
            ArrayList listofSection = new ArrayList();
            ArrayList userID = new ArrayList();
            string counter = "";
            int count = 0;
            string[] strsec = section.Split(',');
            try
            {
                if (strsec.LongLength > 0)
                {

                    foreach (string secti in strsec)
                    {


                        myconn = new SqlConnection();
                        SqlCommand comm = new SqlCommand();
                        myconn.ConnectionString = connectionString;
                        myconn.Open();
                        comm.Connection = myconn;
                        comm.CommandText = "select AppraisalResult,AppraisalStaffUserID from [360DB].[dbo].[StaffAppraisal] stfp, [360DB].[dbo].[StaffInfo] s where stfp.AppraisalStaffUserID = s.UserID AND (s.Section LIKE '%" + secti + "' OR s.Section LIKE '%" + secti + "%' OR s.Section LIKE '" + secti + "%') AND s.Functions = '" + function + "' AND AppraisalResult !=0.0";

                        SqlDataReader dr = comm.ExecuteReader();
                        while (dr.Read())
                        {
                            if (counter == "")
                            {
                                string useridd = Convert.ToString(dr["AppraisalStaffUserID"]);

                                userID.Add(useridd);

                                double result = Convert.ToDouble(dr["AppraisalResult"]);

                                listofappraisal.Add(result);
                            }
                            else
                            {

                                for (int i = 0; i < userID.Count; i++)
                                {
                                    string useridd = Convert.ToString(dr["AppraisalStaffUserID"]);
                                    if (userID[i].ToString() == useridd)
                                    {
                                        count = 1;
                                    }
                                    else
                                    {
                                    }

                                }
                                if (count == 0)
                                {
                                    string useriddd = Convert.ToString(dr["AppraisalStaffUserID"]);
                                    userID.Add(useriddd);

                                    double result = Convert.ToDouble(dr["AppraisalResult"]);

                                    listofappraisal.Add(result);
                                }
                            }
                        }
                        counter = secti;
                        count = 0;
                        dr.Close();
                    }
                }
            }
            catch (SqlException)
            {
                return listofappraisal;
            }

            finally
            {
                myconn.Close();
            }
            return listofappraisal;
        }
        public static ArrayList GetListofDatesViaAllLIKEName(string name)
        {
            SqlConnection myconn = null;
            ArrayList listofdates = new ArrayList();
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                comm.CommandText = "select DISTINCT SystemEndDate from StaffAppraisal stfp, StaffInfo s where stfp.AppraisalStaffUserID=s.UserID and s.Name LIKE '%" + name + "%' order by SystemEndDate";

                SqlDataReader dr = comm.ExecuteReader();
                while (dr.Read())
                {
                    DateTime dates = Convert.ToDateTime(dr["SystemEndDate"]);
                    listofdates.Add(dates);
                }
                dr.Close();

            }
            catch (SqlException)
            {
                return listofdates;
            }
            finally
            {
                myconn.Close();
            }
            return listofdates;
        }
        public static double GetAvgAppraisalForPeriod(DateTime date)
        {
            SqlConnection myconn = null;
            double result = 0.0;
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                comm.CommandText = "select AVG(stfp.AppraisalResult)as TOTAL from StaffAppraisal stfp, StaffInfo s where stfp.AppraisalStaffUserID=s.UserID and stfp.SystemEndDate=@date and stfp.AppraisalResult > 0.0";
                comm.Parameters.AddWithValue("@date", date);

                SqlDataReader dr = comm.ExecuteReader();
                if (dr.Read())
                {
                    double total = Convert.ToDouble(Math.Round(Convert.ToDecimal(dr["TOTAL"]), 1));
                    if (total > 0)
                    {
                        result = total;
                    }
                    else
                    {
                        result = total;
                    }
                }
                dr.Close();

            }
            catch (SqlException)
            {
                return result;
            }

            finally
            {
                myconn.Close();
            }
            return result;
        }
        public static double GetStdDevAppraisalForPeriod(DateTime date)
        {
            SqlConnection myconn = null;
            double result = 0.0;
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                comm.CommandText = "select STDEV(stfp.AppraisalResult)as TOTAL from StaffAppraisal stfp, StaffInfo s where stfp.AppraisalStaffUserID=s.UserID and stfp.SystemEndDate=@date";
                comm.Parameters.AddWithValue("@date", date);
                SqlDataReader dr = comm.ExecuteReader();
                if (dr.Read())
                {
                    double total;

                    if (dr["TOTAL"] == DBNull.Value)
                    {
                        total = 0.00;
                    }
                    else
                    {
                        total = Convert.ToDouble(Convert.ToDecimal(dr["TOTAL"]));
                    }
                    result = total;
                }
                dr.Close();
            }
            catch (SqlException)
            {
                return result;
            }
            finally
            {
                myconn.Close();
            }
            return result;
        }
        public static double GetAverageAppraisalForFunction(string section, string function, DateTime date, int questionID)
        {
            SqlConnection myconn = null;
            double result = 0.0;
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                if (questionID == 0)
                {
                    comm.CommandText = "select AVG(stfp.AppraisalResult)as TOTAL from StaffAppraisal stfp, StaffInfo s where stfp.AppraisalStaffUserID=s.UserID and s.Section LIKE '%" + section + "%' and s.Section NOT LIKE '%" + section + "_' and s.Functions=@funct and stfp.SystemEndDate=@date and stfp.AppraisalResult > 0.0";
                }
                else
                {
                    comm.CommandText = "select AVG(stfp.AppraisalResult)as TOTAL from StaffAppraisal stfp, StaffInfo s where stfp.AppraisalStaffUserID=s.UserID and s.Section LIKE '%" + section + "%' and s.Section NOT LIKE '%" + section + "_' and s.Functions=@funct and stfp.SystemEndDate=@date and stfp.AppraisalResult > 0.0 and AppraisalQuestionID=@qID";
                }
                comm.Parameters.AddWithValue("@funct", function);
                comm.Parameters.AddWithValue("@date", date);
                comm.Parameters.AddWithValue("@qID", questionID);

                SqlDataReader dr = comm.ExecuteReader();
                if (dr.Read())
                {
                    double total = Convert.ToDouble(Math.Round(Convert.ToDecimal(dr["TOTAL"]), 1));
                    if (total > 0)
                    {
                        result = total;
                    }
                    else
                    {
                        result = total;
                    }
                }
                dr.Close();

            }
            catch (SqlException)
            {
                return result;
            }

            finally
            {
                myconn.Close();
            }
            return result;
        }
        // new
        public static double GetStdDevAppraisalForFunction(string section, string function, DateTime date, int questionID)
        {
            SqlConnection myconn = null;
            double result = 0.0;
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                if (questionID == 0)
                {
                    comm.CommandText = "select STDEV(stfp.AppraisalResult)as TOTAL from StaffAppraisal stfp, StaffInfo s where stfp.AppraisalStaffUserID=s.UserID and s.Section LIKE '%" + section + "%' and s.Functions=@funct and stfp.SystemEndDate=@date and AppraisalResult > 0.0";
                }
                else
                {
                    comm.CommandText = "select STDEV(stfp.AppraisalResult)as TOTAL from StaffAppraisal stfp, StaffInfo s where stfp.AppraisalStaffUserID=s.UserID and s.Section LIKE '%" + section + "%' and s.Functions=@funct and stfp.SystemEndDate=@date and AppraisalResult > 0.0 and AppraisalQuestionID=@qID";
                }

                comm.Parameters.AddWithValue("@funct", function);
                comm.Parameters.AddWithValue("@date", date);
                comm.Parameters.AddWithValue("@qID", questionID);

                SqlDataReader dr = comm.ExecuteReader();
                if (dr.Read())
                {
                    double total;

                    if (dr["TOTAL"] == DBNull.Value)
                    {
                        total = 0.00;
                    }
                    else
                    {
                        total = Convert.ToDouble(Convert.ToDecimal(dr["TOTAL"]));
                    }
                    result = total;
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                throw ex;
                //return result;
            }
            finally
            {
                myconn.Close();
            }
            return result;
        }
        public static double GetAverageAppraisalForSection(string function, string section, DateTime date, int questionID)
        {
            SqlConnection myconn = null;
            double result = 0.0;
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                if (questionID == 0)
                {
                    comm.CommandText = "select AVG(stfp.AppraisalResult)as TOTAL from StaffAppraisal stfp, StaffInfo s where stfp.AppraisalStaffUserID=s.UserID and s.Section LIKE '%" + section + "%' and s.Functions=@funct and stfp.SystemEndDate=@date and AppraisalResult > 0.0";
                }
                else
                {
                    comm.CommandText = "select AVG(stfp.AppraisalResult)as TOTAL from StaffAppraisal stfp, StaffInfo s where stfp.AppraisalStaffUserID=s.UserID and s.Section LIKE '%" + section + "%' and s.Functions=@funct and stfp.SystemEndDate=@date and AppraisalResult > 0.0 and AppraisalQuestionID=@qID";
                }

                comm.Parameters.AddWithValue("@funct", function);
                comm.Parameters.AddWithValue("@date", date);
                comm.Parameters.AddWithValue("@qID", questionID);

                SqlDataReader dr = comm.ExecuteReader();
                if (dr.Read())
                {
                    double total = Convert.ToDouble(Math.Round(Convert.ToDecimal(dr["TOTAL"]), 1));
                    if (total > 0)
                    {
                        result = total;
                    }
                }
                dr.Close();

            }
            catch (SqlException)
            {
                return result;
            }

            finally
            {
                myconn.Close();
            }
            return result;
        }
        public static double GetIndividualStdDev(string name, string section, int questionID, DateTime date)
        {
            SqlConnection myconn = null;
            double result = 0.0;
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                if (questionID == 0)
                {
                    comm.CommandText = "select STDEV(stfp.AppraisalResult)as TOTAL from StaffAppraisal stfp, StaffInfo s where stfp.AppraisalStaffUserID=s.UserID and s.Name=@name and s.Section=@section and stfp.SystemEndDate=@date ";
                }
                else
                {
                    comm.CommandText = "select STDEV(stfp.AppraisalResult)as TOTAL from StaffAppraisal stfp, StaffInfo s where stfp.AppraisalStaffUserID=s.UserID and s.Name=@name and s.Section=@section and stfp.AppraisalQuestionID=@quesid and stfp.SystemEndDate=@date ";
                }
                comm.Parameters.AddWithValue("@name", name);
                comm.Parameters.AddWithValue("@section", section);
                comm.Parameters.AddWithValue("@qID", questionID);
                comm.Parameters.AddWithValue("@date", date);
                
                SqlDataReader dr = comm.ExecuteReader();
                if (dr.Read())
                {
                    double total;

                    if (dr["TOTAL"] == DBNull.Value)
                    {
                        total = 0.00;
                    }
                    else
                    {
                        total = Convert.ToDouble(Convert.ToDecimal(dr["TOTAL"]));
                    }
                    result = total;
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                throw ex;
                //return result;
            }
            finally
            {
                myconn.Close();
            }
            return result;
        }
        // new
        public static ArrayList GetAppraisalForFunction(string section, string function, DateTime date, int questionID)
        {
            SqlConnection myconn = null;
            ArrayList listofappraisal = new ArrayList();
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                if (questionID == 0)
                {
                    comm.CommandText = "select * from StaffAppraisal stfp, StaffInfo s where stfp.AppraisalStaffUserID=s.UserID and s.Section LIKE '%" + section + "%' and s.Functions=@funct and stfp.SystemEndDate=@date and AppraisalResult >= 0.0";
                }
                else
                {
                    comm.CommandText = "select * from StaffAppraisal stfp, StaffInfo s where stfp.AppraisalStaffUserID=s.UserID and s.Section LIKE '%" + section + "%' and s.Functions=@funct and stfp.SystemEndDate=@date and AppraisalResult >= 0.0 and AppraisalQuestionID=@qID";
                }
                comm.Parameters.AddWithValue("@funct", function);
                comm.Parameters.AddWithValue("@date", date);
                comm.Parameters.AddWithValue("@qID", questionID);

                SqlDataReader dr = comm.ExecuteReader();
                while (dr.Read())
                {
                    double result = Convert.ToDouble(dr["AppraisalResult"]);
                    listofappraisal.Add(result);
                }
                dr.Close();
            }
            catch (SqlException)
            {
                return listofappraisal;
            }
            finally
            {
                myconn.Close();
            }
            return listofappraisal;
        }
        public static ArrayList GetAppraisalViaPeriod(DateTime date)
        {
            SqlConnection myconn = null;
            ArrayList listofappraisal = new ArrayList();
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                comm.CommandText = "select * from StaffAppraisal stfp, StaffInfo s where stfp.AppraisalStaffUserID=s.UserID and stfp.SystemEndDate=@date";
                comm.Parameters.AddWithValue("@date", date);

                SqlDataReader dr = comm.ExecuteReader();
                while (dr.Read())
                {
                    double result = Convert.ToDouble(dr["AppraisalResult"]);
                    listofappraisal.Add(result);
                }
                dr.Close();
            }
            catch (SqlException)
            {
                return listofappraisal;
            }
            finally
            {
                myconn.Close();
            }
            return listofappraisal;
        }
        public static double GetAverageAppraisalForSectionALL(DateTime date)
        {
            SqlConnection myconn = null;
            double result = 0.0;
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                comm.CommandText = "select AVG(stfp.AppraisalResult)as TOTAL from StaffAppraisal stfp, StaffInfo s where stfp.AppraisalStaffUserID=s.UserID and stfp.SystemEndDate=@date and AppraisalResult > 0.0";
                comm.Parameters.AddWithValue("@date", date);

                SqlDataReader dr = comm.ExecuteReader();
                if (dr.Read())
                {
                    double total = Convert.ToDouble(Math.Round(Convert.ToDecimal(dr["TOTAL"]), 1));
                    if (total > 0)
                    {
                        result = total;
                    }
                }
                dr.Close();

            }
            catch (SqlException)
            {
                return result;
            }

            finally
            {
                myconn.Close();
            }
            return result;
        }

        //Report of History Appraisal

        public static bool CheckIfExistSection(ArrayList seclist)
        {
            SqlConnection myconn = null;
            bool result = false;
            try
            {
                foreach (string sect in seclist)
                {
                    myconn = new SqlConnection();
                    SqlCommand comm = new SqlCommand();
                    myconn.ConnectionString = connectionString;
                    myconn.Open();
                    comm.Connection = myconn;
                    comm.CommandText = "  select COUNT(DISTINCT stfapp.UserID) AS TOTAL from StaffAppraisal stfapp, StaffInfo s where AppraisalStaffUserID=s.UserID and s.Section=@sec";
                    comm.Parameters.AddWithValue("@sec", sect);

                    SqlDataReader dr = comm.ExecuteReader();
                    if (dr.Read())
                    {
                        int total = Convert.ToInt32(dr["TOTAL"]);
                        if (total > 0)
                        {
                            result = true;
                        }
                    }
                    dr.Close();
                }
            }

            catch (SqlException)
            {
                return result;
            }

            finally
            {
                myconn.Close();
            }
            return result;
        }

        public static bool CheckQuestionIdExistInAppraisal(int qid)
        {
            SqlConnection myconn = null;
            bool result = false;
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                comm.CommandText = "select COUNT(*) AS TOTAL from StaffAppraisal where AppraisalQuestionID=@qid";
                comm.Parameters.AddWithValue("@qid", qid);

                SqlDataReader dr = comm.ExecuteReader();
                if (dr.Read())
                {
                    int total = Convert.ToInt32(dr["TOTAL"]);
                    if (total > 0)
                    {
                        result = true;
                    }
                }
                dr.Close();
            }

            catch (SqlException)
            {
                return result;
            }

            finally
            {
                myconn.Close();
            }
            return result;
        }

        public static ArrayList GetAllUserIDViaLikeName(string name)
        {
            ArrayList listofstaff = new ArrayList();
            SqlConnection myconn = null;
            staffinfo staff = null;
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                comm.CommandText = "select * from StaffInfo where (Name LIKE '%" + name + "%' OR Name LIKE '" + name + "%' OR Name LIKE '%" + name + "' OR Name='" + name + "')";

                SqlDataReader dr = comm.ExecuteReader();
                while (dr.Read())
                {
                    string staffname = dr["Name"].ToString();
                    string designation = dr["Designation"].ToString();
                    string section = dr["Section"].ToString();
                    string function = dr["Functions"].ToString();
                    string userid = dr["UserID"].ToString();
                    string role = dr["Role"].ToString();

                    staff = new staffinfo(staffname, designation, section, function, userid, role);
                    listofstaff.Add(staff);
                }
                dr.Close();
            }

            catch (SqlException)
            {
                return listofstaff;
            }

            finally
            {
                myconn.Close();
            }
            return listofstaff;
        }
        public static ArrayList GetAllUserIDViaLikeNameLimit(string name, string usersections)
        {
            ArrayList listofstaff = new ArrayList();
            SqlConnection myconn = null;
            staffinfo staff = null;
            try
            {
                string[] listofsection = usersections.Split(',');
                if (listofsection.LongLength > 0)
                {

                    myconn = new SqlConnection();
                    SqlCommand comm = new SqlCommand();
                    myconn.ConnectionString = connectionString;
                    myconn.Open();
                    comm.Connection = myconn;
                    comm.CommandText = "select * from StaffInfo where (Name LIKE '%" + name + "%' OR Name LIKE '" + name + "%' OR Name LIKE '%" + name + "' OR Name='" + name + "')";

                    SqlDataReader dr = comm.ExecuteReader();
                    while (dr.Read())
                    {
                        string staffname = dr["Name"].ToString();
                        string designation = dr["Designation"].ToString();
                        string section = dr["Section"].ToString();
                        string function = dr["Functions"].ToString();
                        string userid = dr["UserID"].ToString();
                        string role = dr["Role"].ToString();

                        staff = new staffinfo(staffname, designation, section, function, userid, role);
                        listofstaff.Add(staff);
                    }
                    dr.Close();

                }
            }

            catch (SqlException)
            {
                return listofstaff;
            }

            finally
            {
                myconn.Close();
            }
            return listofstaff;
        }

        public static ArrayList GetAllUserIDViaLikeNameSection(string name, ArrayList sec)
        {
            ArrayList listofstaff = new ArrayList();
            SqlConnection myconn = null;
            staffinfo staff = null;
            try
            {
                foreach (string sect in sec)
                {
                    myconn = new SqlConnection();
                    SqlCommand comm = new SqlCommand();
                    myconn.ConnectionString = connectionString;
                    myconn.Open();
                    comm.Connection = myconn;

                    comm.CommandText = "select * from StaffInfo where Name LIKE '%" + name + "%' and Section=@sec";
                    comm.Parameters.AddWithValue("@sec", sect);

                    SqlDataReader dr = comm.ExecuteReader();
                    while (dr.Read())
                    {
                        string staffname = dr["Name"].ToString();
                        string designation = dr["Designation"].ToString();
                        string section = dr["Section"].ToString();
                        string function = dr["Functions"].ToString();
                        string userid = dr["UserID"].ToString();
                        string role = dr["Role"].ToString();

                        staff = new staffinfo(staffname, designation, section, function, userid, role);
                        listofstaff.Add(staff);
                    }
                    dr.Close();
                }
            }

            catch (SqlException)
            {
                return listofstaff;
            }

            finally
            {
                myconn.Close();
            }
            return listofstaff;
        }

        public static staffinfo GetStaffDetailsViaName(string name)
        {
            SqlConnection myconn = null;
            staffinfo staff = null;
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                comm.CommandText = "select * from StaffInfo where Name=@name";
                comm.Parameters.AddWithValue("@name", name);

                SqlDataReader dr = comm.ExecuteReader();
                while (dr.Read())
                {
                    string staffname = dr["Name"].ToString();
                    string designation = dr["Designation"].ToString();
                    string section = dr["Section"].ToString();
                    string function = dr["Functions"].ToString();
                    string userid = dr["UserID"].ToString();
                    string role = dr["Role"].ToString();

                    staff = new staffinfo(staffname, designation, section, function, userid, role);
                }
                dr.Close();
            }

            catch (SqlException)
            {
                return staff;
            }

            finally
            {
                myconn.Close();
            }
            return staff;
        }
        public static staffinfo GetStaffDetailsViaUid(string uid)
        {
            SqlConnection myconn = null;
            staffinfo staff = null;
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                comm.CommandText = "select * from StaffInfo where UserID=@uid";
                comm.Parameters.AddWithValue("@uid", uid);

                SqlDataReader dr = comm.ExecuteReader();
                while (dr.Read())
                {
                    string staffname = dr["Name"].ToString();
                    string designation = dr["Designation"].ToString();
                    string section = dr["Section"].ToString();
                    string function = dr["Functions"].ToString();
                    string userid = dr["UserID"].ToString();
                    string role = dr["Role"].ToString();

                    staff = new staffinfo(staffname, designation, section, function, userid, role);
                }
                dr.Close();
            }

            catch (SqlException)
            {
                return staff;
            }

            finally
            {
                myconn.Close();
            }
            return staff;
        }

        public static AutoEmail GetAutoEmailCountandDate()
        {
            SqlConnection myconn = null;
            AutoEmail count = null;
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                comm.CommandText = "select * from AutoEmail";


                SqlDataReader dr = comm.ExecuteReader();
                while (dr.Read())
                {
                    string autoemailcount = dr["AutoEmailCount"].ToString();
                    DateTime autoemaildate = Convert.ToDateTime(dr["AutoEmailDate"].ToString());
                    count = new AutoEmail(autoemaildate, autoemailcount);
                }
                dr.Close();
            }

            catch (SqlException)
            {
                return count;
            }

            finally
            {
                myconn.Close();
            }
            return count;
        }


        public static ArrayList GetTotalHistoryDates(string uid)
        {
            ArrayList listofdates = new ArrayList();

            SqlConnection myconn = null;
            DateTime dates = new DateTime();
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                comm.CommandText = "select Distinct SystemEndDate from StaffAppraisal where AppraisalStaffUserID=@uid order by SystemEndDate";
                comm.Parameters.AddWithValue("@uid", uid);

                SqlDataReader dr = comm.ExecuteReader();
                while (dr.Read())
                {
                    dates = (DateTime)(dr["SystemEndDate"]);
                    listofdates.Add(dates);
                }
                dr.Close();
            }

            catch (SqlException)
            {
                return listofdates;
            }

            finally
            {
                myconn.Close();
            }
            return listofdates;
        }
        public static ArrayList GetAllDates()
        {
            ArrayList listofdates = new ArrayList();

            SqlConnection myconn = null;
            DateTime dates = new DateTime();
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                comm.CommandText = "select Distinct SystemEndDate from StaffAppraisal order by SystemEndDate";

                SqlDataReader dr = comm.ExecuteReader();
                while (dr.Read())
                {
                    dates = (DateTime)(dr["SystemEndDate"]);
                    listofdates.Add(dates);
                }
                dr.Close();
            }
            catch (SqlException)
            {
                return listofdates;
            }
            finally
            {
                myconn.Close();
            }
            return listofdates;
        }
        public static int GetTotalCountQuestionInPeriod(string uid, DateTime date)
        {
            SqlConnection myconn = null;
            int counter = 0;
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                comm.CommandText = "select Count(distinct AppraisalQuestionID) as TOTAL from StaffAppraisal where AppraisalStaffUserID=@uid and SystemEndDate=@date";
                comm.Parameters.AddWithValue("@uid", uid);
                comm.Parameters.AddWithValue("@date", date);

                SqlDataReader dr = comm.ExecuteReader();
                if (dr.Read())
                {
                    counter = Convert.ToInt32(dr["TOTAL"]);
                }
                dr.Close();
            }

            catch (SqlException)
            {
                return counter;
            }

            finally
            {
                myconn.Close();
            }
            return counter;
        }

        public static double GetAvgRating(string uid, DateTime dates, int qid)
        {
            SqlConnection myconn = null;
            double appresult = 0.0;
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                comm.CommandText = "select AVG(AppraisalResult) as TOTAL from StaffAppraisal where AppraisalStaffUserID=@uid and SystemEndDate=@dates and AppraisalQuestionID=@qid  and AppraisalResult != 0.0";
                comm.Parameters.AddWithValue("@uid", uid);
                comm.Parameters.AddWithValue("@dates", dates);
                comm.Parameters.AddWithValue("@qid", qid);

                SqlDataReader dr = comm.ExecuteReader();
                if (dr.Read())
                {
                    if (dr["TOTAL"] != null)
                    {
                        try
                        {
                            appresult = Math.Round((Convert.ToDouble(dr["TOTAL"])), 1);
                        }
                        catch
                        {
                            appresult = 0.0;
                        }
                    }
                }
                dr.Close();
            }

            catch (SqlException)
            {
                return appresult;
            }

            finally
            {
                myconn.Close();
            }
            return appresult;
        }

        public static ArrayList GetAppraisalResults(string uid, DateTime dates, int qid)
        {
            SqlConnection myconn = null;
            ArrayList AppraisalResults = new ArrayList();
            double appresult = 0.0;
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                comm.CommandText = "select * AppraisalResult from StaffAppraisal where AppraisalStaffUserID=@uid and SystemEndDate=@dates and AppraisalQuestionID=@qid  and AppraisalResult != 0.0";
                comm.Parameters.AddWithValue("@uid", uid);
                comm.Parameters.AddWithValue("@dates", dates);
                comm.Parameters.AddWithValue("@qid", qid);

                SqlDataReader dr = comm.ExecuteReader();
                if (dr.Read())
                {
                    double appraisalresult = Convert.ToDouble(dr["AppraisalResult"].ToString());
                    AppraisalResults.Add(appraisalresult);

                }
                dr.Close();
            }

            catch (SqlException)
            {
                return AppraisalResults;
            }

            finally
            {
                myconn.Close();
            }
            return AppraisalResults;
        }

        public static double GetAvgRatingAllSchool(DateTime dates)
        {
            SqlConnection myconn = null;
            double appresult = 0.0;
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                comm.CommandText = "select AVG(AppraisalResult) as TOTAL from StaffAppraisal where SystemEndDate=@dates and AppraisalResult > 0.0";
                comm.Parameters.AddWithValue("@dates", dates);

                SqlDataReader dr = comm.ExecuteReader();
                if (dr.Read())
                {
                    if (dr["TOTAL"] != null)
                    {
                        try
                        {
                            appresult = Math.Round((Convert.ToDouble(dr["TOTAL"])), 1);
                        }
                        catch
                        {
                        }
                    }
                }
                dr.Close();
            }

            catch (SqlException)
            {
                return appresult;
            }

            finally
            {
                myconn.Close();
            }
            return appresult;
        }

        public static double GetAvgRatingByFunction(string function, DateTime dates)
        {
            SqlConnection myconn = null;
            double appresult = 0.0;
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                comm.CommandText = "select AVG(AppraisalResult) as TOTAL from StaffAppraisal stf, StaffInfo s where stf.AppraisalStaffUserID=s.UserID and s.Functions=@funct and SystemEndDate=@dates";
                comm.Parameters.AddWithValue("@funct", function);
                comm.Parameters.AddWithValue("@dates", dates);

                SqlDataReader dr = comm.ExecuteReader();
                if (dr.Read())
                {
                    if (dr["TOTAL"] != null)
                    {
                        try
                        {
                            appresult = Math.Round((Convert.ToDouble(dr["TOTAL"])), 1);
                        }
                        catch
                        {
                        }
                    }
                }
                dr.Close();
            }

            catch (SqlException)
            {
                return appresult;
            }

            finally
            {
                myconn.Close();
            }
            return appresult;
        }

        public static double GetAvgRatingBySection(string section, DateTime dates, string function)
        {
            SqlConnection myconn = null;
            double appresult = 0.0;
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                if (function.Equals("All function"))
                {
                    comm.CommandText = "select AVG(AppraisalResult) as TOTAL from StaffAppraisal stf, StaffInfo s where stf.AppraisalStaffUserID=s.UserID and s.Section=@section and SystemEndDate=@dates";
                }
                else
                {
                    comm.CommandText = "select AVG(AppraisalResult) as TOTAL from StaffAppraisal stf, StaffInfo s where stf.AppraisalStaffUserID=s.UserID and s.Section=@section and SystemEndDate=@dates and s.Functions=@function";
                }
                //comm.CommandText = "select AVG(AppraisalResult) as TOTAL from StaffAppraisal stf, StaffInfo s where stf.AppraisalStaffUserID=s.UserID and s.Section=@section and SystemEndDate=@dates";
                comm.Parameters.AddWithValue("@section", section);
                comm.Parameters.AddWithValue("@dates", dates);
                comm.Parameters.AddWithValue("@function", function);

                SqlDataReader dr = comm.ExecuteReader();
                if (dr.Read())
                {
                    if (dr["TOTAL"] != null)
                    {
                        try
                        {
                            appresult = Math.Round((Convert.ToDouble(dr["TOTAL"])), 1);
                        }
                        catch
                        {
                        }
                    }
                }
                dr.Close();
            }

            catch (SqlException)
            {
                return appresult;
            }

            finally
            {
                myconn.Close();
            }
            return appresult;
        }

        public static double GetAvgRatingBySectionFunction(string section, string function, DateTime dates)
        {
            SqlConnection myconn = null;
            double appresult = 0.0;
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                comm.CommandText = "select AVG(AppraisalResult) as TOTAL from StaffAppraisal stf, StaffInfo s where stf.AppraisalStaffUserID=s.UserID and s.Section=@section and s.Functions=@funct and SystemEndDate=@dates";
                comm.Parameters.AddWithValue("@section", section);
                comm.Parameters.AddWithValue("@function", function);
                comm.Parameters.AddWithValue("@dates", dates);

                SqlDataReader dr = comm.ExecuteReader();
                if (dr.Read())
                {
                    if (dr["TOTAL"] != null)
                    {
                        try
                        {
                            appresult = Math.Round((Convert.ToDouble(dr["TOTAL"])), 1);
                        }
                        catch
                        {
                        }
                    }
                }
                dr.Close();
            }

            catch (SqlException)
            {
                return appresult;
            }

            finally
            {
                myconn.Close();
            }
            return appresult;
        }

        public static double GetAvgRatingByLIKEName(string name, DateTime dates)
        {
            SqlConnection myconn = null;
            double appresult = 0.0;
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                comm.CommandText = "select AVG(AppraisalResult) as TOTAL from StaffAppraisal stf, StaffInfo s where stf.AppraisalStaffUserID=s.UserID and s.Name LIKE '%" + name + "%' and SystemEndDate=@dates";
                comm.Parameters.AddWithValue("@dates", dates);

                SqlDataReader dr = comm.ExecuteReader();
                if (dr.Read())
                {
                    if (dr["TOTAL"] != null)
                    {
                        try
                        {
                            appresult = Math.Round((Convert.ToDouble(dr["TOTAL"])), 1);
                        }
                        catch
                        {
                        }
                    }
                }
                dr.Close();
            }

            catch (SqlException)
            {
                return appresult;
            }

            finally
            {
                myconn.Close();
            }
            return appresult;
        }

        public static string GetUserIDViaName(string name)
        {
            SqlConnection myconn = null;
            string staffname = "";
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                comm.CommandText = "select userid from StaffInfo where Name=@name";
                comm.Parameters.AddWithValue("@name", name);

                SqlDataReader dr = comm.ExecuteReader();
                if (dr.Read())
                {
                    staffname = dr["UserID"].ToString();
                }
                dr.Close();
            }

            catch (SqlException)
            {
                return staffname;
            }

            finally
            {
                myconn.Close();
            }
            return staffname;
        }

        public static string GetUserIDViaImgRowID(int rowid)
        {
            SqlConnection myconn = null;
            string userid = "";
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                comm.CommandText = "SELECT * FROM ( SELECT ROW_NUMBER() OVER (ORDER BY Name) AS rownumber,userid FROM StaffInfo" +
                                    ") AS foo WHERE rownumber = @row";
                comm.Parameters.AddWithValue("@row", rowid);

                SqlDataReader dr = comm.ExecuteReader();
                if (dr.Read())
                {
                    userid = dr["userid"].ToString();
                }
                dr.Close();
            }

            catch (SqlException)
            {
                return userid;
            }

            finally
            {
                myconn.Close();
            }
            return userid;
        }

        //Image of user
        public static string GetImageViaUserID(string userid)
        {
            SqlConnection myconn = null;
            string url = "";
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                comm.CommandText = "select UserImgHtml from UserImage where UserID=@uid";
                comm.Parameters.AddWithValue("@uid", userid);

                SqlDataReader dr = comm.ExecuteReader();
                if (dr.Read())
                {
                    url = dr["UserImgHtml"].ToString();
                }
                dr.Close();
            }

            catch (SqlException)
            {
                return url;
            }

            finally
            {
                myconn.Close();
            }
            return url;
        }

        //Report of Individual Appraisal

        public static int GetCountYourAppraisalViaQidDate(string uid, int qid, DateTime date)
        {
            SqlConnection myconn = null;
            int count = 0;
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                comm.CommandText = "select COUNT(DISTINCT UserID) AS TOTAL from StaffAppraisal where AppraisalStaffUserID=@uid and AppraisalQuestionID=@qid and SystemEndDate=@enddate and AppraisalResult != 0.0";
                comm.Parameters.AddWithValue("@uid", uid);
                comm.Parameters.AddWithValue("@qid", qid);
                comm.Parameters.AddWithValue("@enddate", date);

                SqlDataReader dr = comm.ExecuteReader();
                if (dr.Read())
                {
                    count = Convert.ToInt32(dr["TOTAL"]);
                }
                dr.Close();
            }
            catch (SqlException)
            {
                return count;
            }

            finally
            {
                myconn.Close();
            }
            return count;
        }
        public static int GetCountYourAppraisalViaQidSection(string name, int qid, string section)
        {
            SqlConnection myconn = null;
            int count = 0;
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                comm.CommandText = "select COUNT(DISTINCT UserID) AS TOTAL from [360DB].[dbo].[StaffAppraisal] [sa], [360DB].[dbo].[StaffInfo] [si] where sa.AppraisalStaffUserID=si.UserID and si.Name=@name and sa.AppraisalQuestionID=@qid and sa.SystemEndDate=@enddate and sa.AppraisalResult != 0.0";
                comm.Parameters.AddWithValue("@name", name);
                comm.Parameters.AddWithValue("@qid", qid);
                comm.Parameters.AddWithValue("@section", section);

                SqlDataReader dr = comm.ExecuteReader();
                if (dr.Read())
                {
                    count = Convert.ToInt32(dr["TOTAL"]);
                }
                dr.Close();
            }
            catch (SqlException)
            {
                return count;
            }

            finally
            {
                myconn.Close();
            }
            return count;
        }
        public static int GetCountYourAppraisalViaDate(string uid, DateTime date)
        {
            SqlConnection myconn = null;
            int count = 0;
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                comm.CommandText = "select COUNT(DISTINCT UserID) AS TOTAL from StaffAppraisal where AppraisalStaffUserID=@uid and SystemEndDate=@enddate";
                comm.Parameters.AddWithValue("@uid", uid);
                comm.Parameters.AddWithValue("@enddate", date);

                SqlDataReader dr = comm.ExecuteReader();
                if (dr.Read())
                {
                    count = Convert.ToInt32(dr["TOTAL"]);
                }
                dr.Close();
            }

            catch (SqlException)
            {
                return count;
            }

            finally
            {
                myconn.Close();
            }
            return count;
        }

        public static int GetCountYourAppraisal(string uid)
        {
            SqlConnection myconn = null;
            int count = 0;
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                comm.CommandText = "select COUNT(DISTINCT UserID) AS TOTAL from StaffAppraisal where AppraisalStaffUserID=@uid";
                comm.Parameters.AddWithValue("@uid", uid);

                SqlDataReader dr = comm.ExecuteReader();
                if (dr.Read())
                {
                    count = Convert.ToInt32(dr["TOTAL"]);
                }
                dr.Close();
            }

            catch (SqlException)
            {
                return count;
            }

            finally
            {
                myconn.Close();
            }
            return count;
        }

        public static int GetCountYourAppraisalViaRate(string uid, string rate, int qid, DateTime date)
        {
            SqlConnection myconn = null;
            int count = 0;
            string rating = "";
            try
            {
                if (rate == "N/A")
                {
                    rating = "0.0";
                }
                else
                {
                    rating = rate;
                }
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                comm.CommandText = "select COUNT(DISTINCT UserID) AS TOTAL from StaffAppraisal where AppraisalStaffUserID=@uid and AppraisalResult=@rate and AppraisalQuestionID=@qid and SystemEndDate=@enddate";
                comm.Parameters.AddWithValue("@uid", uid);
                comm.Parameters.AddWithValue("@rate", rating);
                comm.Parameters.AddWithValue("@qid", qid);
                comm.Parameters.AddWithValue("@enddate", date);

                SqlDataReader dr = comm.ExecuteReader();
                if (dr.Read())
                {
                    count = Convert.ToInt32(dr["TOTAL"]);
                }
                dr.Close();
            }

            catch (SqlException)
            {
                return count;
            }

            finally
            {
                myconn.Close();
            }
            return count;
        }

        public static int GetCountYourAppraisalForStandardDev(string uid, string rate, int qid)
        {
            SqlConnection myconn = null;
            int count = 0;
            string rating = "";
            try
            {
                if (rate == "N/A")
                {
                    rating = "0.0";
                }
                else
                {
                    rating = rate;
                }
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                comm.CommandText = "select COUNT(DISTINCT UserID) AS TOTAL from StaffAppraisal where AppraisalStaffUserID=@uid and AppraisalResult=@rate and AppraisalQuestionID=@qid";
                comm.Parameters.AddWithValue("@uid", uid);
                comm.Parameters.AddWithValue("@rate", rating);
                comm.Parameters.AddWithValue("@qid", qid);


                SqlDataReader dr = comm.ExecuteReader();
                if (dr.Read())
                {
                    count = Convert.ToInt32(dr["TOTAL"]);
                }
                dr.Close();
            }

            catch (SqlException)
            {
                return count;
            }

            finally
            {
                myconn.Close();
            }
            return count;
        }

        public static staffappraisal GetAllRemarksViaUserNameNoFunction(string userid, string userappraisal, int qid, DateTime enddate)
        {
            SqlConnection myconn = null;
            staffappraisal stf = null;
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                comm.CommandText = "select * from StaffAppraisal s,StaffInfo p where s.AppraisalStaffUserID=p.UserID and s.AppraisalQuestionID=@qid and s.AppraisalStaffUserID=@uid and s.UserID=@userappraisal and s.AppraisalRemarks!=''" +
                    " and SystemEndDate=@enddate";
                comm.Parameters.AddWithValue("@uid", userid);
                comm.Parameters.AddWithValue("@userappraisal", userappraisal);
                comm.Parameters.AddWithValue("@qid", qid);
                comm.Parameters.AddWithValue("@enddate", enddate);

                SqlDataReader dr = comm.ExecuteReader();
                if (dr.Read())
                {
                    string uid = dr["UserID"].ToString();
                    int questionid = Convert.ToInt32(dr["AppraisalQuestionID"]);
                    string appstaff = dr["AppraisalStaffUserID"].ToString();
                    double appresult = Convert.ToDouble(dr["AppraisalResult"]);
                    string appremarks = dr["AppraisalRemarks"].ToString();
                    DateTime date = (DateTime)dr["AppraisalDate"];
                    DateTime systemend = (DateTime)dr["SystemEndDate"];
                    stf = new staffappraisal(uid, questionid, appstaff, appresult, appremarks, date, systemend);
                }
                dr.Close();
            }

            catch (SqlException)
            {
                return stf;
            }

            finally
            {
                myconn.Close();
            }
            return stf;
        }

        //Manage Appraisal Details
        public static AppraisalDetails GetAppraisalDetails()
        {
            SqlConnection myconn = null;
            AppraisalDetails appraisal = null;
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                comm.CommandText = "select * from AppraisalDetail where AppraisalID = '1'";

                SqlDataReader dr = comm.ExecuteReader();
                while (dr.Read())
                {
                    int appraisalID = Convert.ToInt32(dr["AppraisalID"].ToString());
                    string appraisalDescription = dr["AppraisalDescription"].ToString();

                    appraisal = new AppraisalDetails(appraisalID, appraisalDescription);
                }
                dr.Close();
            }

            catch (SqlException)
            {
                return appraisal;
            }

            finally
            {
                myconn.Close();
            }
            return appraisal;
        }

        public static bool UpdateAppraisalDetails(int appraisalID, string appraisalDetails)
        {
            SqlConnection myconn = null;
            bool result = false;
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;

                comm.CommandText = "update AppraisalDetail set AppraisalDescription=@appraisalDetails where AppraisalID=@appraisalID";
                comm.Parameters.AddWithValue("@appraisalID", appraisalID);
                comm.Parameters.AddWithValue("@appraisalDetails", appraisalDetails);
                int rowsAffected = comm.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    result = true;
                }
            }
            catch (SqlException)
            {
                return result;
            }
            finally
            {
                myconn.Close();
            }
            return result;
        }

        //Manage Completed Appraisal

        public static int CheckNotCompletedAppraisal()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = connectionString;
            int count = 0;
            try
            {
                con.Open();
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandText = "select COUNT(distinct userid) AS TOTAL from StaffInfo where UserID NOT IN (select UserID from StaffAppraisal where  AppraisalDate >= (select StartDate from SystemDate) and AppraisalDate <= (select EndDate from SystemDate))";
                SqlDataReader dr = comm.ExecuteReader();
                if (dr.Read())
                {
                    count = Convert.ToInt32(dr["TOTAL"]);
                }
                dr.Close();
            }

            catch (SqlException)
            {

            }

            finally
            {
                con.Close();
            }
            return count;
        }
        /*public static int CheckStaffHasCompletedAppraisal(string uid, DateTime endDate)
        {
            SqlConnection myconn = null;
            int count = 0;
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                comm.CommandText = "select COUNT(AppraisalStaffUserID) as TOTAL from StaffAppraisal where UserID=@uid AND SystemEndDate =@SystemEndDate";
                comm.Parameters.AddWithValue("@UserID", uid);
                comm.Parameters.AddWithValue("@SystemEndDate", endDate);

                SqlDataReader dr = comm.ExecuteReader();
                while (dr.Read())
                {
                    count = Convert.ToInt32(dr["TOTAL"]);
                }
                dr.Close();
            }
            catch (SqlException)
            {
                return count;
            }
            finally
            {
                myconn.Close();
            }
            return count;
        } */
        public static ArrayList GetDistinctNameUidCompletedAppraisal(string function)
        {
            SqlConnection myconn = null;
            staffappraisal stfapp = null;
            ArrayList listofCompleted = new ArrayList();
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                comm.CommandText = "select UserID,Functions from StaffInfo where Functions=@funct and UserID NOT IN (select UserID from StaffAppraisal where AppraisalDate >= (select StartDate from SystemDate) and AppraisalDate <= (select EndDate from SystemDate)) order by Name";
                comm.Parameters.AddWithValue("@funct", function);

                SqlDataReader dr = comm.ExecuteReader();
                while (dr.Read())
                {
                    string uid = dr["UserID"].ToString();
                    string funct = dr["Functions"].ToString();
                    stfapp = new staffappraisal(uid, 0, funct, 0.0, "", DateTime.Today, DateTime.Today);
                    listofCompleted.Add(stfapp);
                }
                dr.Close();
            }

            catch (SqlException)
            {
                return listofCompleted;
            }

            finally
            {
                myconn.Close();
            }
            return listofCompleted;
        }

        public static ArrayList GetCountUserIDAppraisalFunction(string uid, string function, int questionid, DateTime date)
        {
            SqlConnection myconn = null;
            ArrayList listofCountUID = new ArrayList();
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                comm.CommandText = "select DISTINCT s.UserID from StaffAppraisal s, StaffInfo p where s.UserID=p.UserID and s.AppraisalStaffUserID=@uid and s.AppraisalQuestionID=@qid and s.AppraisalRemarks!='' and p.Functions=@func " +
                    "and s.SystemEndDate=@enddate";
                comm.Parameters.AddWithValue("@uid", uid);
                comm.Parameters.AddWithValue("@qid", questionid);
                comm.Parameters.AddWithValue("@func", function);
                comm.Parameters.AddWithValue("@enddate", date);

                SqlDataReader dr = comm.ExecuteReader();
                while (dr.Read())
                {
                    string id = dr["UserID"].ToString();
                    listofCountUID.Add(id);
                }
                dr.Close();
            }

            catch (SqlException)
            {
                return listofCountUID;
            }

            finally
            {
                myconn.Close();
            }
            return listofCountUID;
        }

        public static ArrayList GetCountUserIDAppraisalNoFunction(string uid, int qid, DateTime date)
        {
            SqlConnection myconn = null;
            ArrayList listofCountUID = new ArrayList();
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                comm.CommandText = "select DISTINCT s.UserID from StaffAppraisal s, StaffInfo p where s.AppraisalStaffUserID=@uid and s.AppraisalQuestionID=@qid and s.SystemEndDate=@date and s.AppraisalRemarks!='' and s.UserID NOT IN(select UserID from StaffInfo)";
                comm.Parameters.AddWithValue("@uid", uid);
                comm.Parameters.AddWithValue("@qid", qid);
                comm.Parameters.AddWithValue("@date", date);

                SqlDataReader dr = comm.ExecuteReader();
                while (dr.Read())
                {
                    string id = dr["UserID"].ToString();
                    listofCountUID.Add(id);
                }
                dr.Close();
            }

            catch (SqlException)
            {
                return listofCountUID;
            }

            finally
            {
                myconn.Close();
            }
            return listofCountUID;
        }

        public static ArrayList GetDistinctFunctions()
        {
            SqlConnection myconn = null;
            ArrayList listofFunction = new ArrayList();
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                comm.CommandText = "select Distinct Functions from StaffInfo";

                SqlDataReader dr = comm.ExecuteReader();
                while (dr.Read())
                {
                    string function = dr["Functions"].ToString();
                    listofFunction.Add(function);
                }
                dr.Close();
            }

            catch (SqlException)
            {
                return listofFunction;
            }

            finally
            {
                myconn.Close();
            }
            return listofFunction;
        }

        //Manage Delete Appraisal

        public static ArrayList GetAllUid()
        {
            SqlConnection myconn = null;
            ArrayList listofUid = new ArrayList();
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                comm.CommandText = "select UserID from StaffInfo order by UserID";

                SqlDataReader dr = comm.ExecuteReader();
                while (dr.Read())
                {
                    string function = dr["UserID"].ToString();
                    listofUid.Add(function);
                }
                dr.Close();
            }

            catch (SqlException)
            {
                return listofUid;
            }

            finally
            {
                myconn.Close();
            }
            return listofUid;
        }

        public static bool DeleteAllAppraisal()
        {
            bool result = false;
            int rowsAffected = 0;
            SqlConnection myconn = null;
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;

                comm.CommandText = "Delete from StaffAppraisal where AppraisalDate >= (select StartDate from SystemDate) and AppraisalDate <= (select EndDate from SystemDate)";
                rowsAffected = comm.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    result = true;
                }
            }
            catch
            {
                return result;
            }
            finally
            {
                myconn.Close();
            }
            return result;
        }

        public static bool DeleteAllAppraisalSingle(string uid)
        {
            bool result = false;
            int rowsAffected = 0;
            SqlConnection myconn = null;
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;

                comm.CommandText = "Delete from StaffAppraisal where UserID=@uid and AppraisalDate >= (select StartDate from SystemDate) and AppraisalDate <= (select EndDate from SystemDate)";
                comm.Parameters.AddWithValue("@uid", uid);
                rowsAffected = comm.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    result = true;
                }
            }
            catch
            {
                return result;
            }
            finally
            {
                myconn.Close();
            }
            return result;
        }

        //Check login
        public static staffinfo GetLoginUserId(string userid)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = connectionString;
            staffinfo stf = null;
            try
            {
                con.Open();
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandText = "Select * from StaffInfo where UserID=@uid";
                comm.Parameters.AddWithValue("@uid", userid);
                SqlDataReader dr = comm.ExecuteReader();
                if (dr.Read())
                {
                    string staffname = dr["Name"].ToString();
                    string designation = dr["Designation"].ToString();
                    string section = dr["Section"].ToString();
                    string function = dr["Functions"].ToString();
                    string uid = dr["UserID"].ToString();
                    string role = dr["Role"].ToString();

                    stf = new staffinfo(staffname, designation, section, function, uid, role);
                }
                dr.Close();
            }

            catch (SqlException)
            {

            }

            finally
            {
                con.Close();
            }
            return stf;
        }

        public static DateTime GetSystemEndTime()
        {
            SqlConnection myconn = null;
            Systemtime st = null;
            DateTime endd = new DateTime();

            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                comm.CommandText = "select EndDate from SystemDate";

                SqlDataReader dr = comm.ExecuteReader();
                if (dr.Read())
                {

                    DateTime end = (DateTime)(dr["EndDate"]);
                    endd = end;

                }
                dr.Close();
            }

            catch (SqlException)
            {
                return endd;
            }

            finally
            {
                myconn.Close();
            }
            return endd;
        }
        //Submit Appraisal Module
        public static ArrayList GetAllChoice()
        {
            SqlConnection myconn = null;
            ArrayList listofchoice = new ArrayList();
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                comm.CommandText = "select ChoiceAns from Choice order by ChoiceID desc";

                SqlDataReader dr = comm.ExecuteReader();
                while (dr.Read())
                {
                    string choice = dr["ChoiceAns"].ToString();
                    listofchoice.Add(choice);
                }
                dr.Close();
            }

            catch (SqlException)
            {
                return listofchoice;
            }

            finally
            {
                myconn.Close();
            }
            return listofchoice;
        }

        public static ArrayList GetAllQuestion()
        {
            SqlConnection myconn = null;
            ArrayList listofquestion = new ArrayList();
            Question quest = null;
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

                    quest = new Question(id, question, qninclude, one, seven);
                    listofquestion.Add(quest);
                }
                dr.Close();
            }

            catch (SqlException)
            {
                return listofquestion;
            }

            finally
            {
                myconn.Close();
            }
            return listofquestion;
        }

        public static ArrayList GetAllQuestionForAppraisal()
        {
            SqlConnection myconn = null;
            ArrayList listofquestion = new ArrayList();
            Question quest = null;
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                comm.CommandText = "select * from Question where QuestionInclude!='N' order by QuestionInclude, QuestionId asc";

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

                    quest = new Question(id, question, qninclude, one, seven);
                    listofquestion.Add(quest);
                }
                dr.Close();
            }

            catch (SqlException)
            {
                return listofquestion;
            }

            finally
            {
                myconn.Close();
            }
            return listofquestion;
        }

        public static string GetQuestionViaQuestionId(int qid)
        {
            SqlConnection myconn = null;
            string question = "";
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                comm.CommandText = "select Question from Question where QuestionID=@qid";
                comm.Parameters.AddWithValue("@qid", qid);

                SqlDataReader dr = comm.ExecuteReader();
                if (dr.Read())
                {
                    question = dr["Question"].ToString();

                }
                dr.Close();
            }

            catch (SqlException)
            {
                return question;
            }

            finally
            {
                myconn.Close();
            }
            return question;
        }

        public static int GetQuestionIDFromQuestion(string question)
        {
            SqlConnection myconn = null;
            int qid = 0;
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                comm.CommandText = "select QuestionID from Question where Question=@question";
                comm.Parameters.AddWithValue("@question", question);

                SqlDataReader dr = comm.ExecuteReader();
                while (dr.Read())
                {
                    qid = Convert.ToInt32(dr["QuestionID"]);

                }
                dr.Close();
            }

            catch (SqlException)
            {
                return qid;
            }

            finally
            {
                myconn.Close();
            }
            return qid;
        }

        public static ArrayList GetAllQuestionID()
        {
            SqlConnection myconn = null;
            ArrayList listofquestion = new ArrayList();
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
                    int qid = Convert.ToInt32(dr["QuestionID"]);
                    listofquestion.Add(qid);
                }
                dr.Close();
            }

            catch (SqlException)
            {
                return listofquestion;
            }

            finally
            {
                myconn.Close();
            }
            return listofquestion;
        }

        public static ArrayList GetAllStaffUserID(string userid)
        {
            SqlConnection myconn = null;
            ArrayList listofuid = new ArrayList();
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                comm.CommandText = "select UserID from StaffInfo where UserID != @uid order by Name";
                comm.Parameters.AddWithValue("@uid", userid);

                SqlDataReader dr = comm.ExecuteReader();
                while (dr.Read())
                {
                    string staffuid = dr["UserID"].ToString();
                    listofuid.Add(staffuid);
                }
                dr.Close();
            }

            catch (SqlException)
            {
                return listofuid;
            }

            finally
            {
                myconn.Close();
            }
            return listofuid;
        }

        public static int InsertAllAppraisalAnswers(ArrayList appraisalList)
        {
            int addednum = 0;
            int rowsAffected = 0;

            try
            {
                foreach (staffappraisal app in appraisalList)
                {
                    SqlConnection myconn = null;

                    double result = 0.0;
                    if (app.Appresult == 1.1)
                    {
                        result = 0.0;
                    }
                    else
                    {
                        result = app.Appresult;
                    }
                    myconn = new SqlConnection();
                    SqlCommand comm = new SqlCommand();
                    myconn.ConnectionString = connectionString;
                    myconn.Open();

                    comm.Connection = myconn;

                    comm.CommandText = "insert into dbo.StaffAppraisal" +
                   "(UserID,AppraisalQuestionID,AppraisalStaffUserID,AppraisalResult,AppraisalRemarks,AppraisalDate,SystemEndDate) values" +
                   "(@uid,@qid,@appstfuid,@appresult,@appremarks,@appdate,@enddate)";
                    comm.Parameters.AddWithValue("@uid", app.Uid);
                    comm.Parameters.AddWithValue("@qid", app.Questionid);
                    comm.Parameters.AddWithValue("@appstfuid", app.Appstaffuid);
                    comm.Parameters.AddWithValue("@appresult", result);
                    comm.Parameters.AddWithValue("@appremarks", app.Appremarks);
                    comm.Parameters.AddWithValue("@appdate", app.DateTime);
                    comm.Parameters.AddWithValue("@enddate", app.SystemEndTime);
                    rowsAffected = comm.ExecuteNonQuery();
                    addednum += rowsAffected;

                    myconn.Close();

                }
                return addednum;
            }
            catch
            {
                return addednum;
            }
        }

        public static void DeleteAllAppraisalAnswerByUserId(string uid)
        {
            int rowsAffected = 0;
            SqlConnection myconn = null;
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;

                comm.CommandText = "Delete from StaffAppraisal where UserID=@uid and AppraisalDate >= (select StartDate from SystemDate) and AppraisalDate <= (select EndDate from SystemDate)";
                comm.Parameters.AddWithValue("@uid", uid);
                rowsAffected = comm.ExecuteNonQuery();
            }
            catch
            {

            }
            finally
            {
                myconn.Close();
            }
        }

        public static bool CheckIfAppraisalSubmitted(string uid)
        {
            SqlConnection myconn = null;
            bool check = false;
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                comm.CommandText = "select * from StaffAppraisal where UserID=@uid and AppraisalDate >= (select StartDate from SystemDate) and AppraisalDate <= (select EndDate from SystemDate)";
                comm.Parameters.AddWithValue("@uid", uid);

                SqlDataReader dr = comm.ExecuteReader();
                if (dr.Read())
                {
                    check = true;
                }
                dr.Close();
                return check;
            }

            catch (SqlException)
            {
                return check;
            }

            finally
            {
                myconn.Close();
            }
        }

        public static bool CheckAppraisalSave(string uid, int qid, DateTime eDate)
        {
            SqlConnection myconn = null;
            bool check = false;
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                comm.CommandText = "select * from SaveAppraisal where UserID=@uid AND AppraisalQuestionID=@qid AND SystemEndDate = @endDate";
                comm.Parameters.AddWithValue("@uid", uid);
                comm.Parameters.AddWithValue("@qid", qid);
                comm.Parameters.AddWithValue("@endDate", eDate);

                SqlDataReader dr = comm.ExecuteReader();
                if (dr.Read())
                {
                    check = true;
                }
                dr.Close();
                return check;
            }

            catch (SqlException)
            {
                return check;
            }

            finally
            {
                myconn.Close();
            }
        }

        public static void DeleteAppraisalSaved(string uid, int qid, DateTime eDate)
        {
            int rowsAffected = 0;
            SqlConnection myconn = null;
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;

                comm.CommandText = "Delete from SaveAppraisal where UserID=@uid and AppraisalQuestionID=@qid and SystemEndDate=@eDate";
                comm.Parameters.AddWithValue("@uid", uid);
                comm.Parameters.AddWithValue("@qid", qid);
                comm.Parameters.AddWithValue("@eDate", eDate);
                rowsAffected = comm.ExecuteNonQuery();
            }
            catch
            {

            }
            finally
            {
                myconn.Close();
            }
        }

        public static bool CountAllAppraisal()
        {
            SqlConnection myconn = null;
            bool check = false;
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                comm.CommandText = "select COUNT(*) AS TOTAL from StaffAppraisal st, StaffInfo s where st.AppraisalStaffUserID=s.UserID";

                SqlDataReader dr = comm.ExecuteReader();
                if (dr.Read())
                {
                    int count = Convert.ToInt32(dr["TOTAL"]);
                    if (count > 0)
                    {
                        check = true;
                    }
                }
                dr.Close();
                return check;
            }

            catch (SqlException)
            {
                return check;
            }

            finally
            {
                myconn.Close();
            }
        }

        //calcuate average grade module
        public static int InsertStaffQuestionGrade(ArrayList appraisalList)
        {
            int addednum = 0;
            int rowsAffected = 0;
            int count = 0;
            try
            {
                foreach (staffappraisal app in appraisalList)
                {
                    SqlConnection myconn = null;

                    double result = 0.0;
                    if (app.Appresult == 1.1)
                    {
                        result = 0.0;
                    }
                    else
                    {
                        result = app.Appresult;
                    }

                    if (result != 0.0)
                    {
                        myconn = new SqlConnection();
                        SqlCommand comm = new SqlCommand();
                        myconn.ConnectionString = connectionString;
                        myconn.Open();

                        comm.Connection = myconn;

                        comm.CommandText = "insert into dbo.StaffQuestionAvg" +
                       "(UserID,QuestionID,StaffUserID,AppraisalGrade,SystemEndDate) values" +
                       "(@uid,@qid,@suid,@grade,@enddate)";
                        comm.Parameters.AddWithValue("@uid", app.Uid);
                        comm.Parameters.AddWithValue("@qid", app.Questionid);
                        comm.Parameters.AddWithValue("@suid", app.Appstaffuid);
                        comm.Parameters.AddWithValue("@grade", result);
                        comm.Parameters.AddWithValue("@enddate", app.SystemEndTime);
                        rowsAffected = comm.ExecuteNonQuery();
                        addednum += rowsAffected;
                        count++;
                        myconn.Close();
                    }
                }
                return addednum;
            }
            catch
            {
                return addednum;
            }
        }

        public static double GetAverageStaffPeriod(string uid, DateTime date)
        {
            SqlConnection myconn = null;
            double result = 0.0;
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                comm.CommandText = "select AverageGrade from StaffPeriodAvg where UserID =@uid AND SystemEndDate=@date";
                comm.Parameters.AddWithValue("@uid", uid);
                comm.Parameters.AddWithValue("@date", date);

                SqlDataReader dr = comm.ExecuteReader();
                if (dr.Read())
                {
                    double total = Convert.ToDouble(Math.Round(Convert.ToDecimal(dr["AverageGrade"]), 1));
                    //if (total > 0)
                    //{
                    //    result = total;
                    //}
                    //else
                    //{
                    result = total;
                    //}
                }
                dr.Close();

            }
            catch (SqlException)
            {
                return result;
            }

            finally
            {
                myconn.Close();
            }
            return result;
        }

        public static double GetAverageAllStaffPeriod(DateTime date)
        {
            SqlConnection myconn = null;
            double result = 0.0;
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                //comm.CommandText = "select AVG(AverageGrade) from StaffPeriodAvg where UserID =@uid AND SystemEndDate=@date";
                comm.CommandText = "select AVG(AverageGrade) as Average from StaffPeriodAvg where SystemEndDate=@date";
                //comm.Parameters.AddWithValue("@uid", uid);
                comm.Parameters.AddWithValue("@date", date);

                SqlDataReader dr = comm.ExecuteReader();
                if (dr.Read())
                {
                    double total = Convert.ToDouble(Math.Round(Convert.ToDecimal(dr["Average"]), 1));
                    //if (total > 0)
                    //{
                    //    result = total;
                    //}
                    //else
                    //{
                    result = total;
                    //}
                }
                dr.Close();

            }
            catch (SqlException)
            {
                return result;
            }

            finally
            {
                myconn.Close();
            }
            return result;
        }

        public static void InsertStaffAverageGradeByPeriod(string uid, double grade, DateTime endDate)
        {
            int rowsAffected = 0;

            try
            {
                SqlConnection myconn = null;

                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();

                comm.Connection = myconn;

                comm.CommandText = "insert into dbo.StaffPeriodAvg" +
               "(UserID,AverageGrade,SystemEndDate) values" +
               "(@uid,@avg,@enddate)";
                comm.Parameters.AddWithValue("@uid", uid);
                comm.Parameters.AddWithValue("@avg", grade);
                comm.Parameters.AddWithValue("@enddate", endDate);
                rowsAffected = comm.ExecuteNonQuery();

                myconn.Close();
            }
            catch
            {
            }
        }

        public static bool CheckStaffPeriodExist(string uid, DateTime date)
        {
            SqlConnection myconn = null;
            bool check = false;
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                comm.CommandText = "select * from StaffPeriodAvg where UserID=@uid and SystemEndDate=@date";
                comm.Parameters.AddWithValue("@uid", uid);
                comm.Parameters.AddWithValue("@date", date);

                SqlDataReader dr = comm.ExecuteReader();
                if (dr.Read())
                {
                    check = true;
                }
                dr.Close();
                return check;
            }

            catch (SqlException)
            {
                return check;
            }

            finally
            {
                myconn.Close();
            }
        }

        public static void UpdateAvgStaffPeriod(string uid, double grade, DateTime endDate)
        {
            int rowsAffected = 0;

            try
            {
                SqlConnection myconn = null;

                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();

                comm.Connection = myconn;

                comm.CommandText = "update StaffPeriodAvg set AverageGrade=@grade where SystemEndDate=@endDate and UserID=@uid";
                comm.Parameters.AddWithValue("@uid", uid);
                comm.Parameters.AddWithValue("@grade", grade);
                comm.Parameters.AddWithValue("@enddate", endDate);
                rowsAffected = comm.ExecuteNonQuery();

                myconn.Close();
            }
            catch
            {
            }
        }

        //new method with updates
        public static void UpdateAvgStaffPeriodIfPeriodIsWithinStartEnd(DateTime endDate)
        {
            int rowsAffected = 0;
            bool check = false;
            try
            {
                SqlConnection myconn = null;

                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();

                comm.Connection = myconn;
                comm.CommandText = "select * from StaffPeriodAvg sp, SystemDate sd where sp.SystemEndDate> sd.StartDate and sp.SystemEndDate<sd.EndDate";
                SqlDataReader dr = comm.ExecuteReader();
                if (dr.Read())
                {
                    check = true;
                }
                dr.Close();
                if (check == true)
                {
                    comm.CommandText = "update StaffPeriodAvg set SystemEndDate=@endDate";
                    comm.Parameters.AddWithValue("@enddate", endDate);
                    rowsAffected = comm.ExecuteNonQuery();
                }
                myconn.Close();
            }
            catch
            {
            }
        }

        //Manage System Module
        public static bool UpdateAppraisalEndTime(DateTime endDate, DateTime newendDate)
        {
            SqlConnection myconn = null;
            bool result = false;
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;

                comm.CommandText = "update StaffAppraisal set SystemEndDate=@endtime where SystemEndDate=@prevendtime";
                comm.Parameters.AddWithValue("@prevendtime", endDate);
                comm.Parameters.AddWithValue("@endtime", newendDate);
                int rowsAffected = comm.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    result = true;
                }
            }
            catch (SqlException)
            {
                return result;
            }
            finally
            {
                myconn.Close();
            }
            return result;
        }

        public static bool UpdateSavedAppraisalEndTime(DateTime endDate, DateTime newendDate)
        {
            SqlConnection myconn = null;
            bool result = false;
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;

                comm.CommandText = "update SaveAppraisal set SystemEndDate=@endtime where SystemEndDate=@prevendtime";
                comm.Parameters.AddWithValue("@prevendtime", endDate);
                comm.Parameters.AddWithValue("@endtime", newendDate);
                int rowsAffected = comm.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    result = true;
                }
            }
            catch (SqlException)
            {
                return result;
            }
            finally
            {
                myconn.Close();
            }
            return result;
        }

        public static bool CheckExistMonth(DateTime enddate)
        {
            SqlConnection myconn = null;
            bool result = false;
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                if (enddate.Month < 10)
                {
                    comm.CommandText = "select COUNT(*) AS TOTAL from StaffAppraisal where SystemEndDate LIKE '%" + enddate.Year + "-_" + enddate.Month + "%'";
                }
                else
                {
                    comm.CommandText = "select COUNT(*) AS TOTAL from StaffAppraisal where SystemEndDate LIKE '%" + enddate.Year + "-" + enddate.Month + "%'";
                }

                SqlDataReader dr = comm.ExecuteReader();
                if (dr.Read())
                {
                    int count = Convert.ToInt32(dr["TOTAL"]);
                    if (count > 0)
                    {
                        result = true;
                    }
                }
                dr.Close();
            }

            catch (SqlException)
            {
                return result;
            }

            finally
            {
                myconn.Close();
            }
            return result;
        }

        public static bool UpdateSystemTime(DateTime endDate, DateTime newendDate)
        {
            SqlConnection myconn = null;
            bool result = false;
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;

                comm.CommandText = "update SystemDate set EndDate=@endtime where EndDate=@prevendtime";
                comm.Parameters.AddWithValue("@prevendtime", endDate);
                comm.Parameters.AddWithValue("@endtime", newendDate);
                int rowsAffected = comm.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    result = true;
                }
            }
            catch (SqlException)
            {
                return result;
            }
            finally
            {
                myconn.Close();
            }
            return result;
        }

        public static bool InsertSystemTime(Systemtime st)
        {
            SqlConnection myconn = null;
            bool result = false;
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;

                comm.CommandText = "insert into SystemDate" +
                    "(StartDate,EndDate) values" +
                    "(@starttime, @endtime)";
                comm.Parameters.AddWithValue("@starttime", st.Startdate);
                comm.Parameters.AddWithValue("@endtime", st.Enddate);
                int rowsAffected = comm.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    result = true;
                }
            }
            catch (SqlException)
            {
                return result;
            }
            finally
            {
                myconn.Close();
            }
            return result;
        }

        public static bool InsertAutoEmailTime(DateTime st)
        {
            SqlConnection myconn = null;

            DateTime autoemailtime = st.AddDays(-7);
            string zero = "N";
            bool result = false;
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;

                comm.CommandText = "insert into AutoEmail" +
                    "(AutoEmailDate,AutoEmailCount) values" +
                    "(@autoemaildate, @zero)";
                comm.Parameters.AddWithValue("@autoemaildate", autoemailtime);
                comm.Parameters.AddWithValue("@zero", zero);
                int rowsAffected = comm.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    result = true;
                }
            }
            catch (SqlException)
            {
                return result;
            }
            finally
            {
                myconn.Close();
            }
            return result;
        }


        public static Systemtime GetSystemTime()
        {
            SqlConnection myconn = null;
            Systemtime st = null;
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                comm.CommandText = "select StartDate, EndDate from SystemDate";

                SqlDataReader dr = comm.ExecuteReader();
                if (dr.Read())
                {
                    DateTime start = (DateTime)(dr["StartDate"]);
                    DateTime end = (DateTime)(dr["EndDate"]);
                    st = new Systemtime(start, end);
                }
                dr.Close();
            }

            catch (SqlException)
            {
                return st;
            }

            finally
            {
                myconn.Close();
            }
            return st;
        }

        public static bool DeleteSystemTime()
        {
            SqlConnection myconn = null;
            bool result = false;
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;

                myconn.Open();
                comm.Connection = myconn;
                comm.CommandText = "Delete from SystemDate";
                int rowsAffected = comm.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    result = true;
                }
            }
            catch (SqlException)
            {
                return result;
            }
            finally
            {
                myconn.Close();
            }
            return result;
        }

        //ImportExport Module
        public static void DeleteAllAppraisalLoginFromNonStaff()
        {
            int rowsAffected = 0;
            SqlConnection myconn = null;
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;

                comm.CommandText = "Delete from Login where UserID NOT IN(select UserID from StaffInfo)";
                rowsAffected = comm.ExecuteNonQuery();
            }
            catch
            {

            }
            finally
            {
                myconn.Close();
            }
        }

        public static void DeleteAllAppraisalFromNonStaff()
        {
            int rowsAffected = 0;
            SqlConnection myconn = null;
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;

                comm.CommandText = "Delete from StaffAppraisal where AppraisalStaffUserID NOT IN(select UserID from StaffInfo)";
                rowsAffected = comm.ExecuteNonQuery();
            }
            catch
            {

            }
            finally
            {
                myconn.Close();
            }
        }

        public static bool CheckSectionExist(string sec)
        {
            SqlConnection myconn = null;
            bool result = false;
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                comm.CommandText = "select COUNT(*) AS TOTAL from Section where Section=@sec";
                comm.Parameters.AddWithValue("@sec", sec);

                SqlDataReader dr = comm.ExecuteReader();
                if (dr.Read())
                {
                    int total = Convert.ToInt32(dr["TOTAL"]);
                    if (total > 0)
                    {
                        result = true;
                    }
                }
                dr.Close();
            }

            catch (SqlException)
            {
                return result;
            }

            finally
            {
                myconn.Close();
            }
            return result;
        }

        public static bool CheckFunctionExist(string func)
        {
            SqlConnection myconn = null;
            bool result = false;
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                comm.CommandText = "select COUNT(*) AS TOTAL from Functions where FunctionID=@func";
                comm.Parameters.AddWithValue("@func", func);

                SqlDataReader dr = comm.ExecuteReader();
                if (dr.Read())
                {
                    int total = Convert.ToInt32(dr["TOTAL"]);
                    if (total > 0)
                    {
                        result = true;
                    }
                }
                dr.Close();
            }

            catch (SqlException)
            {
                return result;
            }

            finally
            {
                myconn.Close();
            }
            return result;
        }

        public static bool CheckRoleExist(string role)
        {
            SqlConnection myconn = null;
            bool result = false;
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                comm.CommandText = "select COUNT(*) AS TOTAL from Role where RoleName=@role";
                comm.Parameters.AddWithValue("@role", role);

                SqlDataReader dr = comm.ExecuteReader();
                if (dr.Read())
                {
                    int total = Convert.ToInt32(dr["TOTAL"]);
                    if (total > 0)
                    {
                        result = true;
                    }
                }
                dr.Close();
            }

            catch (SqlException)
            {
                return result;
            }

            finally
            {
                myconn.Close();
            }
            return result;
        }

        public static void DeleteAllStaffInformation()
        {
            int rowsAffected = 0;
            SqlConnection myconn = null;
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;

                comm.CommandText = "Delete from StaffInfo";
                rowsAffected = comm.ExecuteNonQuery();
            }
            catch
            {

            }
            finally
            {
                myconn.Close();
            }
        }

        public static bool InsertStaffInformation(staffinfo staf)
        {
            bool result = false;
            SqlConnection myconn = null;
            int rowsAffected = 0;

            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;

                comm.CommandText = "insert into StaffInfo" +
                    "(Name,Designation,Section,Functions,UserID,Role) values" +
                    "(@name,@designation,@section,@funct,@uid,@role)";
                comm.Parameters.AddWithValue("@name", staf.Name);
                comm.Parameters.AddWithValue("@designation", staf.Designation);
                comm.Parameters.AddWithValue("@section", staf.Section);
                comm.Parameters.AddWithValue("@funct", staf.Function);
                comm.Parameters.AddWithValue("@uid", staf.Uid);
                comm.Parameters.AddWithValue("@role", staf.Role);
                rowsAffected = comm.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    result = true;
                }
            }
            catch (SqlException)
            {
            }
            finally
            {
                myconn.Close();
            }
            return result;
        }

        //Manage Appraisal Question Module

        public static int GetCountAllQuestion()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = connectionString;
            int count = 0;
            ArrayList result = new ArrayList();
            try
            {
                con.Open();
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandText = "SELECT COUNT(*) AS TOTAL FROM Question";
                SqlDataReader dr = comm.ExecuteReader();
                if (dr.Read())
                {
                    count = Convert.ToInt32(dr["TOTAL"]);
                }
                dr.Close();
            }

            catch (SqlException)
            {

            }

            finally
            {
                con.Close();
            }
            return count;
        }

        public static ArrayList GetAllQuestionListInOrder()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = connectionString;
            int qid = 0;
            ArrayList listofquestion = new ArrayList();
            try
            {
                con.Open();
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandText = "SELECT QuestionID FROM Question order by QuestionInclude, QuestionID asc";
                SqlDataReader dr = comm.ExecuteReader();
                while (dr.Read())
                {
                    qid = Convert.ToInt32(dr["QuestionID"]);
                    listofquestion.Add(qid);
                }
                dr.Close();
            }

            catch (SqlException)
            {

            }

            finally
            {
                con.Close();
            }
            return listofquestion;
        }

        public static int UpdateAllAppraisalQuestionOnlyInclude(ArrayList questionlist)
        {
            int updatenum = 0;
            int rowsAffected = 0;

            try
            {
                foreach (Question qn in questionlist)
                {
                    string qninclude = "N";
                    SqlConnection myconn = null;

                    myconn = new SqlConnection();
                    SqlCommand comm = new SqlCommand();
                    myconn.ConnectionString = connectionString;
                    myconn.Open();

                    comm.Connection = myconn;

                    comm.CommandText = "update Question set Question=@quest,QuestionInclude=@include,OneRate=@one,SevenRate=@seven WHERE QuestionId=@qid";

                    if (qn.QuestionInclude == true)
                    {
                        qninclude = "Y";
                    }
                    comm.Parameters.AddWithValue("@qid", qn.QuestionID);
                    comm.Parameters.AddWithValue("@quest", qn.QuestionDetails);
                    comm.Parameters.AddWithValue("@include", qninclude);
                    comm.Parameters.AddWithValue("@one", qn.QuestionRateOne);
                    comm.Parameters.AddWithValue("@seven", qn.QuestionRateSeven);
                    rowsAffected = comm.ExecuteNonQuery();
                    updatenum += rowsAffected;

                    myconn.Close();
                }
                return updatenum;
            }
            catch
            {
                return updatenum;
            }
        }

        public static int UpdateAllAppraisalQuestion(ArrayList questionlist)
        {
            int updatenum = 0;
            int rowsAffected = 0;

            try
            {
                foreach (Question qn in questionlist)
                {
                    string qninclude = "N";
                    SqlConnection myconn = null;

                    myconn = new SqlConnection();
                    SqlCommand comm = new SqlCommand();
                    myconn.ConnectionString = connectionString;
                    myconn.Open();

                    comm.Connection = myconn;

                    comm.CommandText = "update Question set Question=@quest,QuestionInclude=@include,OneRate=@one,SevenRate=@seven WHERE QuestionId=@qid";

                    if (qn.QuestionInclude == true)
                    {
                        qninclude = "Y";
                    }
                    comm.Parameters.AddWithValue("@qid", qn.QuestionID);
                    comm.Parameters.AddWithValue("@quest", qn.QuestionDetails);
                    comm.Parameters.AddWithValue("@include", qninclude);
                    comm.Parameters.AddWithValue("@one", qn.QuestionRateOne);
                    comm.Parameters.AddWithValue("@seven", qn.QuestionRateSeven);
                    rowsAffected = comm.ExecuteNonQuery();
                    updatenum += rowsAffected;

                    myconn.Close();
                }
                return updatenum;
            }
            catch
            {
                return updatenum;
            }
        }

        public static bool DeleteAppraisalQuestion(int questionid)
        {
            bool deleteresult = false;
            int rowsAffected = 0;
            SqlConnection myconn = null;
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;

                comm.CommandText = "Delete from Question where QuestionID=@qid";
                comm.Parameters.AddWithValue("@qid", questionid);
                rowsAffected = comm.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    deleteresult = true;
                }
            }
            catch
            {

            }
            finally
            {
                myconn.Close();
            }
            return deleteresult;
        }

        public static void UpdateAppraisalQuestionID(int questionid)
        {
            int rowsAffected = 0;
            SqlConnection myconn = null;
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;

                comm.CommandText = "UPDATE Question SET QuestionID = QuestionID - 1 where QuestionID > @qid";
                comm.Parameters.AddWithValue("@qid", questionid);
                rowsAffected = comm.ExecuteNonQuery();
            }
            catch
            {

            }
            finally
            {
                myconn.Close();
            }
        }


        public static bool GetAppraisalQuestionValid(string question)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = connectionString;
            bool result = false;
            try
            {
                con.Open();
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandText = "Select COUNT(*) AS TOTAL from Question where Question=@quest";
                comm.Parameters.AddWithValue("@quest", question);
                SqlDataReader dr = comm.ExecuteReader();
                if (dr.Read())
                {
                    int count = Convert.ToInt32(dr["TOTAL"]);
                    if (count > 0)
                    {
                        result = true;
                    }
                }
                dr.Close();
            }

            catch (SqlException)
            {

            }

            finally
            {
                con.Close();
            }
            return result;
        }

        public static int InsertAllAppraisalQuestions(ArrayList questionlist)
        {
            int updatenum = 0;
            int rowsAffected = 0;

            try
            {
                foreach (Question qn in questionlist)
                {
                    string qninclude = "N";
                    bool result = dbmanager.CheckQuestionIdExist(Convert.ToInt32(qn.QuestionID));

                    if (result == false)
                    {
                        SqlConnection myconn = null;

                        myconn = new SqlConnection();
                        SqlCommand comm = new SqlCommand();
                        myconn.ConnectionString = connectionString;
                        myconn.Open();

                        comm.Connection = myconn;

                        comm.CommandText = "insert into dbo.Question(Question,OneRate,SevenRate,QuestionInclude) values (@quest,@one,@seven,@include)";

                        if (qn.QuestionInclude == true)
                        {
                            qninclude = "Y";
                        }
                        //comm.Parameters.AddWithValue("@qid", qn.QuestionID);
                        comm.Parameters.AddWithValue("@quest", qn.QuestionDetails);
                        comm.Parameters.AddWithValue("@one", qn.QuestionRateOne);
                        comm.Parameters.AddWithValue("@seven", qn.QuestionRateSeven);
                        comm.Parameters.AddWithValue("@include", qninclude);
                        rowsAffected = comm.ExecuteNonQuery();
                        updatenum += rowsAffected;

                        myconn.Close();
                    }
                }
                return updatenum;
            }
            catch
            {
                return updatenum;
            }
        }

        public static bool CheckQuestionIdExist(int qid)
        {
            SqlConnection myconn = null;
            bool check = false;
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                comm.CommandText = "select QuestionID from Question " +
                                   "where QuestionID=@qid";
                comm.Parameters.AddWithValue("@qid", qid);

                SqlDataReader dr = comm.ExecuteReader();
                if (dr.Read())
                {
                    check = true;
                }
                dr.Close();
                return check;
            }

            catch (SqlException)
            {
                return check;
            }

            finally
            {
                myconn.Close();
            }
        }

        //Manage User Module

        public static ArrayList GetAllStaffDetails()
        {
            SqlConnection myconn = null;
            staffinfo staff = null;
            ArrayList listofstaff = new ArrayList();
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                comm.CommandText = "select * from StaffInfo order by Name";

                SqlDataReader dr = comm.ExecuteReader();
                while (dr.Read())
                {
                    string staffname = dr["Name"].ToString();
                    string designation = dr["Designation"].ToString();
                    string section = dr["Section"].ToString();
                    string function = dr["Functions"].ToString();
                    string uid = dr["UserID"].ToString();
                    string role = dr["Role"].ToString();

                    staff = new staffinfo(staffname, designation, section, function, uid, role);
                    listofstaff.Add(staff);
                }
                dr.Close();
            }

            catch (SqlException)
            {
                return listofstaff;
            }

            finally
            {
                myconn.Close();
            }
            return listofstaff;
        }

        public static ArrayList GetAllStaffDetailsByOfficerPermission(staffinfo stafff)
        {
            SqlConnection myconn = null;
            staffinfo staff = null;
            ArrayList listofstaff = new ArrayList();
            try
            {
                if (stafff.Role == "Director")
                {
                    myconn = new SqlConnection();
                    SqlCommand comm = new SqlCommand();
                    myconn.ConnectionString = connectionString;
                    myconn.Open();
                    comm.Connection = myconn;
                    comm.CommandText = "select * from StaffInfo order by Name";

                    SqlDataReader dr = comm.ExecuteReader();
                    while (dr.Read())
                    {
                        string staffname = dr["Name"].ToString();
                        string designation = dr["Designation"].ToString();
                        string section = dr["Section"].ToString();
                        string function = dr["Functions"].ToString();
                        string uid = dr["UserID"].ToString();
                        string role = dr["Role"].ToString();

                        staff = new staffinfo(staffname, designation, section, function, uid, role);
                        listofstaff.Add(staff);
                    }
                    dr.Close();
                }

                else if (stafff.Role == "Officer" && stafff.Function == "Manager")
                {
                    myconn = new SqlConnection();
                    SqlCommand comm = new SqlCommand();
                    myconn.ConnectionString = connectionString;
                    myconn.Open();
                    comm.Connection = myconn;
                    comm.CommandText = "select * from StaffInfo WHERE StaffInfo.Functions != 'Director' AND StaffInfo.Functions != 'AD' AND StaffInfo.Functions != 'DD' AND StaffInfo.Functions != 'Manager' order by Name";

                    SqlDataReader dr = comm.ExecuteReader();
                    while (dr.Read())
                    {
                        string staffname = dr["Name"].ToString();
                        string designation = dr["Designation"].ToString();
                        string section = dr["Section"].ToString();
                        string function = dr["Functions"].ToString();
                        string uid = dr["UserID"].ToString();
                        string role = dr["Role"].ToString();

                        staff = new staffinfo(staffname, designation, section, function, uid, role);
                        listofstaff.Add(staff);
                    }
                    dr.Close();
                }
                else if (stafff.Role == "Officer")
                {
                    myconn = new SqlConnection();
                    SqlCommand comm = new SqlCommand();
                    myconn.ConnectionString = connectionString;
                    myconn.Open();
                    comm.Connection = myconn;
                    comm.CommandText = "select * from StaffInfo WHERE StaffInfo.Functions != 'Director' AND StaffInfo.Functions != 'AD' AND StaffInfo.Functions != 'DD' order by Name";

                    SqlDataReader dr = comm.ExecuteReader();
                    while (dr.Read())
                    {
                        string staffname = dr["Name"].ToString();
                        string designation = dr["Designation"].ToString();
                        string section = dr["Section"].ToString();
                        string function = dr["Functions"].ToString();
                        string uid = dr["UserID"].ToString();
                        string role = dr["Role"].ToString();

                        staff = new staffinfo(staffname, designation, section, function, uid, role);
                        listofstaff.Add(staff);
                    }
                    dr.Close();
                }
            }

            catch (SqlException)
            {
                return listofstaff;
            }

            finally
            {
                myconn.Close();
            }
            return listofstaff;
        }

        public static ArrayList GetAllStaffBySection(string sectionlimit)
        {
            staffinfo staff = null;

            SqlConnection myconn = null;
            ArrayList listofSection = new ArrayList();
            string[] strsec = sectionlimit.Split(',');
            try
            {
                if (strsec.LongLength > 0)
                {
                    foreach (string secti in strsec)
                    {
                        myconn = new SqlConnection();
                        SqlCommand comm = new SqlCommand();
                        myconn.ConnectionString = connectionString;
                        myconn.Open();
                        comm.Connection = myconn;
                        comm.CommandText = "select * from StaffInfo WHERE (StaffInfo.Section LIKE '%'+@sect OR StaffInfo.Section LIKE '%'+@sect + '%' OR StaffInfo.Section LIKE @sect + '%' OR StaffInfo.Section=@sect)";

                        comm.Parameters.AddWithValue("@sect", secti);

                        SqlDataReader dr = comm.ExecuteReader();
                        while (dr.Read())
                        {
                            string staffname = dr["Name"].ToString();
                            string designation = dr["Designation"].ToString();
                            string section = dr["Section"].ToString();
                            string function = dr["Functions"].ToString();
                            string uid = dr["UserID"].ToString();
                            string role = dr["Role"].ToString();

                            staff = new staffinfo(staffname, designation, section, function, uid, role);
                            listofSection.Add(staff);

                        }
                        dr.Close();

                    }
                }
            }

            catch (SqlException)
            {
                return listofSection;
            }

            finally
            {
                myconn.Close();
            }
            return listofSection;
        }
        public static ArrayList GetAllStaffDetailsByFunctionSection(string function, string section)
        {
            SqlConnection myconn = null;
            ArrayList listofStaff = new ArrayList();
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                comm.CommandText = "select * from StaffInfo where (Section LIKE '%'+ @section OR Section LIKE '%'+ @section + '%' OR Section LIKE @section+'%' OR Section=@section) and Functions = @function;";
                comm.Parameters.AddWithValue("@function", function);
                comm.Parameters.AddWithValue("@section", section);
                SqlDataReader dr = comm.ExecuteReader();
                while (dr.Read())
                {
                    //string staffID = dr["UserID"].ToString();
                    ////Question q2 = new Question(qID, question, include);
                    //listofStaff.Add(staffID);
                    string staffname = dr["Name"].ToString();
                    string designation = dr["Designation"].ToString();
                    string sect = dr["Section"].ToString();
                    string funct = dr["Functions"].ToString();
                    string uid = dr["UserID"].ToString();
                    string role = dr["Role"].ToString();

                    staffinfo staff = new staffinfo(staffname, designation, sect, funct, uid, role);
                    listofStaff.Add(staff);
                }
                dr.Close();
            }

            catch (SqlException)
            {
                return listofStaff;
            }

            finally
            {
                myconn.Close();
            }
            return listofStaff;
        }

        public static ArrayList GetAllRole()
        {
            SqlConnection myconn = null;
            ArrayList listofRole = new ArrayList();
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                comm.CommandText = "select * from Role order by RoleName";

                SqlDataReader dr = comm.ExecuteReader();
                while (dr.Read())
                {
                    string rolename = dr["RoleName"].ToString();
                    listofRole.Add(rolename);
                }
                dr.Close();
            }

            catch (SqlException)
            {
                return listofRole;
            }

            finally
            {
                myconn.Close();
            }
            return listofRole;
        }

        public static ArrayList GetAllSectionnByLimit()
        {
            SqlConnection myconn = null;
            ArrayList listofSection = new ArrayList();
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                comm.CommandText = "select DISTINCT s.Section from StaffInfo s, StaffAppraisal stf where s.UserID=stf.AppraisalStaffUserID and s.Section!='ALL'";

                SqlDataReader dr = comm.ExecuteReader();
                while (dr.Read())
                {
                    string section = dr["Section"].ToString();
                    string[] stsec = section.Split(',');
                    if (stsec.LongLength > 0)
                    {
                        foreach (string sec in stsec)
                        {
                            listofSection.Add(sec);
                        }
                    }
                }
                dr.Close();
            }

            catch (SqlException)
            {
                return listofSection;
            }

            finally
            {
                myconn.Close();
            }
            return listofSection;
        }

        public static ArrayList GetAllSectionByUid(string sectionslimit)
        {
            SqlConnection myconn = null;
            ArrayList listofSection = new ArrayList();
            string[] strsec = sectionslimit.Split(',');
            try
            {
                if (strsec.LongLength > 0)
                {
                    foreach (string secti in strsec)
                    {
                        myconn = new SqlConnection();
                        SqlCommand comm = new SqlCommand();
                        myconn.ConnectionString = connectionString;
                        myconn.Open();
                        comm.Connection = myconn;
                        comm.CommandText = "select DISTINCT s.Section from StaffInfo s, StaffAppraisal stf where s.UserID=stf.AppraisalStaffUserID and (s.Section LIKE '%' + @sect OR s.Section LIKE '%' + @sect + '%' OR s.Section LIKE @sect + '%' OR s.Section=@sect) ";

                        comm.Parameters.AddWithValue("@sect", secti);

                        SqlDataReader dr = comm.ExecuteReader();
                        while (dr.Read())
                        {
                            string section = dr["Section"].ToString();
                            string[] stsec = section.Split(',');
                            if (stsec.LongLength > 0)
                            {

                                foreach (string sec in stsec)
                                {
                                    if (sec == secti)
                                    {
                                        listofSection.Add(sec);
                                    }
                                }
                            }
                        }
                        dr.Close();

                    }
                }
            }

            catch (SqlException)
            {
                return listofSection;
            }

            finally
            {
                myconn.Close();
            }
            return listofSection;
        }

        public static ArrayList GetAllFunctionByLimit()
        {
            SqlConnection myconn = null;
            ArrayList listofFunction = new ArrayList();
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                comm.CommandText = "select DISTINCT s.Functions from StaffInfo s, StaffAppraisal stf where s.UserID=stf.AppraisalStaffUserID and s.Section!='ALL'";

                SqlDataReader dr = comm.ExecuteReader();
                while (dr.Read())
                {
                    string function = dr["Functions"].ToString();
                    string[] stfunc = function.Split(',');
                    if (stfunc.LongLength > 0)
                    {
                        foreach (string sec in stfunc)
                        {
                            listofFunction.Add(sec);
                        }
                    }
                }
                dr.Close();
            }

            catch (SqlException)
            {
                return listofFunction;
            }

            finally
            {
                myconn.Close();
            }
            return listofFunction;
        }

        public static ArrayList GetAllFunctionByLimitViaOfficer(string section)
        {
            SqlConnection myconn = null;
            ArrayList listofFunction = new ArrayList();
            try
            {
                string[] arraysection = section.Split(',');
                if (arraysection.LongLength > 0)
                {
                    foreach (string sect in arraysection)
                    {
                        myconn = new SqlConnection();
                        SqlCommand comm = new SqlCommand();
                        myconn.ConnectionString = connectionString;
                        myconn.Open();
                        comm.Connection = myconn;
                        comm.CommandText = "select DISTINCT s.Functions from StaffInfo s, StaffAppraisal stf where s.UserID=stf.AppraisalStaffUserID and s.Section=@sect";
                        comm.Parameters.AddWithValue("@sect", sect);

                        SqlDataReader dr = comm.ExecuteReader();
                        while (dr.Read())
                        {
                            string function = dr["Functions"].ToString();
                            listofFunction.Add(function);
                        }
                        dr.Close();
                    }
                }
            }

            catch (SqlException)
            {
                return listofFunction;
            }

            finally
            {
                myconn.Close();
            }
            return listofFunction;
        }

        public static ArrayList GetAllFunctionByRoleLimit(string role, string section)
        {
            SqlConnection myconn = null;
            ArrayList listofFunction = new ArrayList();
            try
            {
                if (section == "ALL")
                {
                    section = "";

                    ArrayList sections = GetAllSection();
                    for (int i = 0; i < sections.Count; i++)
                    {
                        if (i == (sections.Count - 1))
                        {
                            section += sections[i].ToString();
                        }
                        else
                        {
                            section += sections[i].ToString() + ",";
                        }

                    }

                }
                string[] arraysection = section.Split(',');
                if (arraysection.LongLength > 0)
                {
                    foreach (string sect in arraysection)
                    {
                        myconn = new SqlConnection();
                        SqlCommand comm = new SqlCommand();
                        myconn.ConnectionString = connectionString;
                        myconn.Open();
                        comm.Connection = myconn;
                        if (role == "Officer")
                        {
                            comm.CommandText = "select DISTINCT s.Functions from StaffInfo s, StaffAppraisal stf where s.UserID=stf.AppraisalStaffUserID and (s.Section LIKE '%' + @sect OR s.Section LIKE '%' + @sect + '%' OR s.Section LIKE @sect + '%' OR s.Section=@sect) and s.Role!='Officer'";
                        }
                        else if (role == "User")
                        {
                            comm.CommandText = "select DISTINCT s.Functions from StaffInfo s, StaffAppraisal stf where s.UserID=stf.AppraisalStaffUserID and (s.Section LIKE '%' + @sect OR s.Section LIKE '%' + @sect + '%' OR s.Section LIKE @sect + '%' OR s.Section=@sect) and s.Role !='User'";
                        }
                        else if (role == "Director")
                        {

                            comm.CommandText = "select DISTINCT s.Functions from StaffInfo s, StaffAppraisal stf where s.UserID=stf.AppraisalStaffUserID and (s.Section LIKE '%' + @sect OR s.Section LIKE '%' + @sect + '%' OR s.Section LIKE @sect + '%' OR s.Section=@sect) and s.Role !='Director'";

                        }
                        else if (role == "Admin")
                        {
                            comm.CommandText = "select * from Functions order by Functions";
                        }
                        comm.Parameters.AddWithValue("@sect", sect);

                        SqlDataReader dr = comm.ExecuteReader();
                        while (dr.Read())
                        {
                            string function = dr["Functions"].ToString();
                            listofFunction.Add(function);
                        }
                        dr.Close();
                    }
                }
            }

            catch (SqlException)
            {
                return listofFunction;
            }

            finally
            {
                myconn.Close();
            }
            return listofFunction;
        }
        public static ArrayList GetAllNames()
        {
            SqlConnection myconn = null;
            ArrayList listofNames = new ArrayList();
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                comm.CommandText = "SELECT Name from dbo.StaffInfo";

                SqlDataReader dr = comm.ExecuteReader();
                while (dr.Read())
                {
                    string name = dr["Name"].ToString();
                    listofNames.Add(name);
                }
                dr.Close();
            }
            catch (SqlException)
            {
                return listofNames;
            }
            finally
            {
                myconn.Close();
            }
            return listofNames;
        }
        public static ArrayList GetAllFunction()
        {
            SqlConnection myconn = null;
            ArrayList listofFunction = new ArrayList();
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                comm.CommandText = "select * from Functions order by Functions";

                SqlDataReader dr = comm.ExecuteReader();
                while (dr.Read())
                {
                    string function = dr["Functions"].ToString();
                    listofFunction.Add(function);
                }
                dr.Close();
            }

            catch (SqlException)
            {
                return listofFunction;
            }

            finally
            {
                myconn.Close();
            }
            return listofFunction;
        }
        public static ArrayList GetAllFunctionName()
        {
            SqlConnection myconn = null;
            ArrayList listofFunction = new ArrayList();
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                comm.CommandText = "select * from Functions order by Functions";

                SqlDataReader dr = comm.ExecuteReader();
                while (dr.Read())
                {
                    string function = dr["Functions"].ToString();
                    listofFunction.Add(function);
                }
                dr.Close();
            }

            catch (SqlException)
            {
                return listofFunction;
            }

            finally
            {
                myconn.Close();
            }
            return listofFunction;
        }

        public static ArrayList GetAllDesignation()
        {
            SqlConnection myconn = null;
            ArrayList listofDesignation = new ArrayList();
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                comm.CommandText = "SELECT Distinct Designation FROM StaffInfo";

                SqlDataReader dr = comm.ExecuteReader();
                while (dr.Read())
                {
                    string designation = dr["Designation"].ToString();
                    listofDesignation.Add(designation);
                }
                dr.Close();
            }

            catch (SqlException)
            {
                return listofDesignation;
            }

            finally
            {
                myconn.Close();
            }
            return listofDesignation;
        }

        public static ArrayList GetAllQuestionList()
        {
            SqlConnection myconn = null;
            ArrayList listofquestion = new ArrayList();
            //Question q1 = null;
            //List<Question> questionList = new List<Question>();
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                comm.CommandText = "select Question from Question order by QuestionID asc";

                SqlDataReader dr = comm.ExecuteReader();
                while (dr.Read())
                {
                    string question = dr["Question"].ToString();
                    //Question q2 = new Question(qID, question, include);
                    listofquestion.Add(question);
                }
                dr.Close();
            }

            catch (SqlException)
            {
                return listofquestion;
            }

            finally
            {
                myconn.Close();
            }
            return listofquestion;
        }

        public static ArrayList GetAllQuestionListViaPerson(DateTime date, string staffid)
        {
            SqlConnection myconn = null;
            ArrayList listofquestion = new ArrayList();
            //Question q1 = null;
            //List<Question> questionList = new List<Question>();
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                comm.CommandText = "select DISTINCT AppraisalQuestionID as Total from StaffAppraisal where SystemEndDate=@date and AppraisalStaffUserID = @uid";
                comm.Parameters.AddWithValue("@date", date);
                comm.Parameters.AddWithValue("@uid", staffid);
                SqlDataReader dr = comm.ExecuteReader();
                while (dr.Read())
                {
                    string question = dr["Total"].ToString();
                    //Question q2 = new Question(qID, question, include);
                    listofquestion.Add(question);
                }
                dr.Close();
            }

            catch (SqlException)
            {
                return listofquestion;
            }

            finally
            {
                myconn.Close();
            }
            return listofquestion;
        }

        public static ArrayList GetAllQuestionListWithIDs()
        {
            SqlConnection myconn = null;
            ArrayList listofquestion = new ArrayList();
            Question q1 = null;
            //List<Question> questionList = new List<Question>();
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                comm.CommandText = "select QuestionID, Question from Question order by QuestionID asc";

                SqlDataReader dr = comm.ExecuteReader();
                while (dr.Read())
                {
                    int qID = Convert.ToInt32(dr["QuestionID"].ToString());
                    string question = dr["Question"].ToString();
                    bool include = false;
                    Question q2 = new Question(qID, question, include);
                    listofquestion.Add(q2);
                }
                dr.Close();
            }

            catch (SqlException)
            {
                return listofquestion;
            }

            finally
            {
                myconn.Close();
            }
            return listofquestion;
        }

        public static ArrayList GetAllSection()
        {
            SqlConnection myconn = null;
            ArrayList listofSection = new ArrayList();
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                comm.CommandText = "select * from Section order by Section";

                SqlDataReader dr = comm.ExecuteReader();
                while (dr.Read())
                {
                    string section = dr["Section"].ToString();
                    listofSection.Add(section);
                }
                dr.Close();
            }

            catch (SqlException)
            {
                return listofSection;
            }

            finally
            {
                myconn.Close();
            }
            return listofSection;
        }

        public static bool CheckUserIDExist(string uid)
        {
            SqlConnection myconn = null;
            bool check = false;
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;
                comm.CommandText = "select UserID from StaffInfo " +
                                   "where UserID=@uid";
                comm.Parameters.AddWithValue("@uid", uid);

                SqlDataReader dr = comm.ExecuteReader();
                if (dr.Read())
                {
                    check = true;
                }
                dr.Close();
                return check;
            }

            catch (SqlException)
            {
                return check;
            }

            finally
            {
                myconn.Close();
            }
        }

        public static bool DeleteStaffviaUserId(string uid)
        {
            bool deleteresult = false;
            int rowsAffected = 0;
            SqlConnection myconn = null;
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;

                comm.CommandText = "Delete from StaffInfo where UserID=@uid";
                comm.Parameters.AddWithValue("@uid", uid);
                rowsAffected = comm.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    deleteresult = true;
                }
            }
            catch
            {

            }
            finally
            {
                myconn.Close();
            }
            return deleteresult;
        }

        public static bool UpdateUserInformation(staffinfo stf)
        {
            bool updateresult = false;
            int rowsAffected = 0;
            SqlConnection myconn = null;
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;

                comm.CommandText = "Update StaffInfo set Section=@sect, Designation=@desig, Functions=@funct, Role=@role where UserID=@uid";
                comm.Parameters.AddWithValue("@uid", stf.Uid);
                comm.Parameters.AddWithValue("@sect", stf.Section);
                comm.Parameters.AddWithValue("@desig", stf.Designation);
                comm.Parameters.AddWithValue("@funct", stf.Function);
                comm.Parameters.AddWithValue("@role", stf.Role);
                rowsAffected = comm.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    updateresult = true;
                }
            }
            catch
            {

            }
            finally
            {
                myconn.Close();
            }
            return updateresult;
        }

        //others
        public static string GetNameViaUserID(string uid)
        {
            string userid = "";
            SqlConnection myconn = null;
            try
            {
                myconn = new SqlConnection();
                SqlCommand comm = new SqlCommand();
                myconn.ConnectionString = connectionString;
                myconn.Open();
                comm.Connection = myconn;

                comm.CommandText = "select Name from StaffInfo where UserID=@uid";
                comm.Parameters.AddWithValue("@uid", uid);

                SqlDataReader dr = comm.ExecuteReader();
                if (dr.Read())
                {
                    userid = dr["Name"].ToString();
                }
                dr.Close();
            }
            catch
            {

            }
            finally
            {
                myconn.Close();
            }
            return userid;
        }
        internal static double GetAverageAllStaffPeriod(ArrayList date)
        {
            throw new NotImplementedException();
        }
    }
}
