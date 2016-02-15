using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using System.Collections;
using _360_Staff_Survey_Web.Class;
using ASC_FeedbackSystem.classes;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.Web.Security;

namespace _360_Staff_Survey_Web
{
    public partial class ImportExportStaffList : System.Web.UI.Page
    {
        public static string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Appraisal_System"].ConnectionString.ToString();

        protected void Page_Load(object sender, EventArgs e)
        {
            StaffErrorMsgLbl.Text = "";
            if (Session["Role"] != null)
            {
                string role = Session["Role"].ToString();
                if (role != "Admin")
                {
                    Response.Redirect("accessdenied.aspx");
                }
                else
                {
                    exportStaffInfo();
                    lblNote.Text = "*only applicable to .csv format";
                }
            }
            else
            {
                Response.Redirect("accessdenied.aspx");
            }
        }

        private ArrayList ReadListOfDataFromCSVStaffInfo(DataTable listofStaffInformation)
        {
            ArrayList listofStaff = new ArrayList();
            try
            {
                int emptyvalue = 0;
                int invalidrelation = 0;
                int invaliddataformat = 0;
                StringBuilder SqlQuery = new StringBuilder();
                SqlQuery.Append("");

                if (listofStaffInformation.Rows.Count != 0)
                {
                    foreach (DataRow dr in listofStaffInformation.Rows)
                    {
                        if (dr["UserID"].ToString() != "")
                        {
                            string designationstring = "";
                            bool checksec = false;
                            string staffname = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(dr["Name"].ToString().Trim());
                            string designation = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(dr["Designation"].ToString().Trim());

                            string[] designarray = designation.Split('/');

                            if (designarray.LongLength > 0)
                            {
                                int counter = 0;
                                foreach (string de in designarray)
                                {
                                    if (de != "")
                                    {
                                        if (counter > 0)
                                        {
                                            designationstring += "/" + CultureInfo.CurrentCulture.TextInfo.ToTitleCase(de.Trim());
                                        }
                                        else
                                        {
                                            designationstring += CultureInfo.CurrentCulture.TextInfo.ToTitleCase(de.Trim());
                                        }
                                        counter++;
                                    }
                                }
                            }
                            else
                            {
                                designationstring = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(designation);
                            }
                            string section = dr["Section"].ToString().Trim();
                            string sectionstring = "";
                            string[] secinfo = section.Split(',');
                            string[] secarray = RemoveDuplicates(secinfo);
                            int countstring = 0;
                            foreach (string se in secarray)
                            {
                                if (se != "")
                                {

                                    checksec = dbmanager.CheckSectionExist(se.Trim());
                                    if (checksec == true)
                                    {
                                        if (countstring > 0)
                                        {
                                            sectionstring += "," + se.Trim();
                                        }
                                        else
                                        {
                                            sectionstring += se.Trim();
                                        }
                                        countstring++;
                                    }
                                    if (checksec == false)
                                    {
                                        break;
                                    }
                                }
                            }

                            string function = dr["Functions"].ToString().Trim();

                            bool checkfunc = dbmanager.CheckFunctionExist(function);

                            string staffuid = dr["UserID"].ToString().ToLower().Trim();
                            string role = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(dr["Role"].ToString().Trim());

                            bool checkrole = dbmanager.CheckRoleExist(role);

                            if (staffname != "" && staffuid != "" && checksec == true && checkfunc == true && checkrole == true)
                            {
                                if (sectionstring.Contains("ALL"))
                                {
                                    sectionstring = "ALL";
                                }
                                staffinfo staffinfo = new staffinfo(staffname, designationstring, sectionstring, function, staffuid, role);
                                listofStaff.Add(staffinfo);
                            }
                            else if (checksec == false || checkfunc == false || checkrole == false)
                            {
                                invalidrelation++;
                            }
                            if (staffname.Length > 100 || staffuid.Length > 30)
                            {
                                invaliddataformat++;
                            }
                        }
                        else
                        {
                            emptyvalue++;
                        }
                    }
                    if (invalidrelation > 0)
                    {
                        listofStaff.Clear();
                        MessageBoxShow(invalidrelation + " record(s) found with invalid data. might need relation data from other tables.");
                    }
                    if (invaliddataformat > 0)
                    {
                        listofStaff.Clear();
                        MessageBoxShow(invaliddataformat + " record(s) found with invalid data format.");
                    }
                    if (emptyvalue > 0)
                    {
                        listofStaff.Clear();
                        MessageBoxShow(emptyvalue + " record(s) found with empty data field.");
                    }
                }
                else
                {
                    StaffErrorMsgLbl.ForeColor = System.Drawing.Color.Red;
                    StaffErrorMsgLbl.Text = "Fail to read file.";
                }
            }
            catch
            {
                StaffErrorMsgLbl.ForeColor = System.Drawing.Color.Red;
                StaffErrorMsgLbl.Text = "Invalid data format found.";
            }
            return listofStaff;
        }

        public static string[] RemoveDuplicates(string[] s)
        {
            HashSet<string> set = new HashSet<string>(s);
            string[] result = new string[set.Count];
            set.CopyTo(result);
            return result;
        }

        protected void exportStaffInfo()
        {
            string query = "select * from StaffInfo order by Name";
            lblNote.Text = "";
            try
            {
                SqlConnection excelConnection = new SqlConnection(connectionString);
                SqlCommand myCommand = new SqlCommand(query, excelConnection);
                try
                {
                    DataTable dataTable = new DataTable();
                    DataTable dataTableRead = new DataTable();
                    //add data into datatable
                    excelConnection.Open();
                    SqlDataAdapter oledbda = new SqlDataAdapter();

                    oledbda.SelectCommand = myCommand;
                    oledbda.Fill(dataTableRead);

                    SqlDataReader dr = (SqlDataReader)myCommand.ExecuteReader();
                    if (!dr.HasRows)
                    {
                        ExportPanel.Visible = false;
                        ExportStaffGrid.Visible = false;
                        excelConnection.Close();
                        oledbda = null;
                        lblNote.Visible = true;
                        lblNote.ForeColor = System.Drawing.Color.Red;
                        lblNote.Text = "No result found.";
                    }
                    else
                    {
                        excelConnection.Close();
                        oledbda = null;
                        ExportStaffGrid.Visible = true;
                        ExportStaffGrid.AllowPaging = true;
                        ExportStaffGrid.HorizontalAlign = HorizontalAlign.Center;
                        ExportStaffGrid.DataSource = dataTableRead;
                        ExportStaffGrid.DataBind();
                        if (dataTableRead.Rows.Count > 10)
                        {
                            viewAllBtn.Text = "view all";
                        }
                        dataTable = dataTableRead;
                        ExportPanel.Visible = true;
                        Session["dataTable"] = dataTable;
                    }
                }
                catch (SqlException ex)
                {
                    ExportPanel.Visible = false;
                    ExportStaffGrid.Visible = false;
                    lblNote.Visible = true;
                    lblNote.ForeColor = System.Drawing.Color.Red;
                    lblNote.Text = ex.Message;
                }
            }
            catch (Exception ee)
            {
                ExportPanel.Visible = false;
                MessageBoxShow(ee.Message.ToString());
            }
        }

        private void MessageBoxShow(string message)
        {
            string strScript = "<script>";
            strScript += "alert('" + message + "');";
            strScript += "window.location='ImportExportStaffList.aspx';";
            strScript += "</script>";
            this.ClientScript.RegisterStartupScript(this.GetType(), "Startup", strScript);
        }

        protected void ExcelBtn_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                DataTable dataTable = (DataTable)Session["dataTable"];
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "StaffDetails.csv"));
                Response.ContentType = "application/text";
                ExportStaffGrid.BorderWidth = 0;
                ExportStaffGrid.AllowPaging = false;
                ExportStaffGrid.DataSource = dataTable;
                ExportStaffGrid.DataBind();
                System.Text.StringBuilder strbldr = new System.Text.StringBuilder();
                for (int i = 0; i < ExportStaffGrid.HeaderRow.Cells.Count; i++)
                {
                    //separting header columns text with comma operator
                    strbldr.Append(ExportStaffGrid.HeaderRow.Cells[i].Text + ',');
                }
                //appending new line for gridview header row
                strbldr.Append("\n");
                for (int j = 0; j < ExportStaffGrid.Rows.Count; j++)
                {
                    for (int i = 0; i < ExportStaffGrid.HeaderRow.Cells.Count; i++)
                    {
                        string stLine = "";
                        string value = HttpUtility.HtmlDecode(ExportStaffGrid.Rows[j].Cells[i].Text);
                        var append = value.Contains(",")
                                 ? string.Format("\"{0}\"", value)
                                 : value;
                        stLine = string.Format("{0}{1},", stLine, append);

                        strbldr.Append(stLine);
                    }
                    //appending new line for gridview rows
                    strbldr.Append("\n");
                }
                Response.Write(strbldr.ToString());
                Response.End();
            }
            catch (Exception)
            {
            }

        }

        protected void WordBtn_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                DataTable dataTable = (DataTable)Session["dataTable"];
                ExportStaffGrid.AllowPaging = false;
                ExportStaffGrid.HeaderStyle.ForeColor = System.Drawing.Color.White;
                ExportStaffGrid.BorderWidth = 0;
                ExportStaffGrid.DataSource = dataTable;
                ExportStaffGrid.DataBind();
                Response.ClearContent();
                Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "StaffDetails.doc"));
                Response.Charset = "";
                Response.ContentType = "application/ms-word";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                //Applying stlye to gridview header cells
                for (int i = 0; i < ExportStaffGrid.HeaderRow.Cells.Count; i++)
                {
                    ExportStaffGrid.HeaderRow.Cells[i].Style.Add("background-color", "Black");
                }
                ExportStaffGrid.RenderControl(htw);
                Response.Write(sw.ToString());
                Response.End();
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message.ToString());
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }

        protected void ImportStaffBtn_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable listOfStaffInformation = new DataTable();
                ArrayList listofStaff = new ArrayList();

                if (StaffDataFileUpload.PostedFile.FileName.Length == 0)
                {
                    StaffErrorMsgLbl.ForeColor = System.Drawing.Color.Red;
                    StaffErrorMsgLbl.Text = "Please select a file to upload.";
                }
                else
                {

                    string extension = System.IO.Path.GetExtension(StaffDataFileUpload.PostedFile.FileName);

                    if (extension != ".csv")
                    {
                        StaffErrorMsgLbl.ForeColor = System.Drawing.Color.Red;
                        StaffErrorMsgLbl.Text = "Invalid extension.";
                    }
                    else
                    {
                        using (CsvReader reader = new CsvReader(StaffDataFileUpload.PostedFile.InputStream))
                        {
                            #region create dt
                            DataTable List = new DataTable();
                            List.Columns.Add("Name");
                            List.Columns.Add("Designation");
                            List.Columns.Add("Section");
                            List.Columns.Add("Functions");
                            List.Columns.Add("UserID");
                            List.Columns.Add("Role");
                            #endregion

                            bool dataformat = false;
                            string checkname = "Name,Designation,Section,Functions,UserID,Role";

                            foreach (string[] values in reader.RowEnumerator)
                            {
                                #region split string
                                string first = values[0].ToString();
                                string second = values[1].ToString();
                                string third = values[2].ToString();
                                string fourth = values[3].ToString();
                                string fivth = values[4].ToString();
                                string sixth = values[5].ToString();
                                #endregion
                                string firststring = first + "," + second + "," + third + "," + fourth + "," + fivth + "," + sixth;
                                if (firststring != ",,,,,")
                                {
                                    List.Rows.Add(first, second, third, fourth, fivth, sixth);
                                    listOfStaffInformation = List;
                                }

                                if (firststring == checkname)
                                {
                                    dataformat = true;
                                }

                                if (dataformat == false)
                                {
                                    break;
                                }
                            }
                            if (List.Rows.Count > 0)
                            {
                                List.Rows.RemoveAt(0);
                            }
                        }
                        listofStaff = ReadListOfDataFromCSVStaffInfo(listOfStaffInformation);

                        if (listofStaff.Count > 0)
                        {
                            int added = 0;
                            //dbmanager.DeleteAllStaffInformation();

                            foreach (staffinfo staffinfo in listofStaff)
                            {
                                bool result = dbmanager.InsertStaffInformation(staffinfo);
                                if (result == true)
                                {
                                    #region send email
                                    bool checkuser = dbmanager.CheckValidUser(staffinfo.Uid);
                                    if (checkuser == false)
                                    {
                                        string password = CreateRandomPassword(8);
                                        string hashpassw = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "sha1");
                                        bool result2 = dbmanager.InsertUserIDandPassw(staffinfo.Uid, hashpassw);

                                        if (result2 == true)
                                        {
                                            //bool sentemail = email.SendMail("Thank you for using the Online 360° leadership system." + Environment.NewLine + Environment.NewLine + "Your new password for login to the system is: " + password + "" + Environment.NewLine + Environment.NewLine + "You are advised to change your password once you have logged in." + Environment.NewLine + "For login, please visit: " + "http://ascapps.tp.edu.sg/360Leadership/LoginPage.aspx" + Environment.NewLine + Environment.NewLine + "For any enquiries to the system, please contact asc webmaster or (x5376)", staffinfo.Uid + "@tp.edu.sg", "360° Leadership System New Password");

                                            bool sentemail = email.SendMail("<p>Thank you for using the Online 360° leadership system.</p>" + Environment.NewLine + Environment.NewLine + "Your new password for login to the system is: " + password + "" + Environment.NewLine + Environment.NewLine + "<br>You are advised to change your password once you have logged in.</br>" + Environment.NewLine + "<br>For login, please visit: " + "<a href= 'http://ascapps.tp.edu.sg/360Leadership/LoginPage.aspx' target='_blank'>http://ascapps.tp.edu.sg/360Leadership/LoginPage.aspx</a></br>" + Environment.NewLine + Environment.NewLine + "<br>For any enquiries to the system, please contact asc webmaster or (x5376)</br>", staffinfo.Uid + "@hotmail.com", "360° Leadership System New Password");

                                            if (sentemail == true)
                                            {
                                                added++;
                                            }
                                        }
                                    }
                                    #endregion
                                }
                            }
                            //delete all where no staff info found
                            //dbmanager.DeleteAllAppraisalFromNonStaff();
                            //dbmanager.DeleteAllAppraisalLoginFromNonStaff();
                            if (added == 0)
                            {
                                MessageBoxShow("No record added.");
                            }
                            else
                            {
                                MessageBoxShow(added + " record(s) added.");
                            }
                        }
                    }
                }
            }
            catch
            {
                MessageBoxShow("No records found.");
            }
        }

        protected void ExportStaffGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ExportStaffGrid.PageIndex = e.NewPageIndex;
            DataTable dt = (DataTable)Session["dataTable"];
            ExportStaffGrid.DataSource = dt;
            ExportStaffGrid.DataBind();
        }

        protected void viewAllBtn_Click(object sender, EventArgs e)
        {
            if (Session["dataTable"] != null)
            {
                DataTable dataTableRead = (DataTable)Session["dataTable"];

                if (Session["ViewALLExport"] == null)
                {
                    ExportStaffGrid.AllowPaging = false;
                    ExportStaffGrid.DataSource = dataTableRead;
                    ExportStaffGrid.DataBind();
                    viewAllBtn.Text = "view with paging";
                    Session["ViewALLExport"] = true;
                }
                else
                {
                    ExportStaffGrid.AllowPaging = true;
                    ExportStaffGrid.DataSource = dataTableRead;
                    ExportStaffGrid.DataBind();
                    viewAllBtn.Text = "view all";
                    Session["ViewALLExport"] = null;
                }
            }
        }

        private static string CreateRandomPassword(int passwordLength)
        {
            string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789@#&";
            char[] chars = new char[passwordLength];
            Random rd = new Random();

            for (int i = 0; i < passwordLength; i++)
            {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }

            return new string(chars);
        }

        protected void PdfBtn_Click(object sender, ImageClickEventArgs e)
        {

            Response.ContentType = "application/pdf";

            Response.AddHeader("content-disposition", "attachment;filename=StaffEvaluation.pdf");

            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            StringWriter sw = new StringWriter();

            HtmlTextWriter hw = new HtmlTextWriter(sw);

            ExportStaffGrid.AllowPaging = false;

            ExportStaffGrid.DataBind();

            ExportStaffGrid.RenderControl(hw);

            ExportStaffGrid.HeaderRow.Style.Add("width", "15%");

            ExportStaffGrid.HeaderRow.Style.Add("font-size", "10px");

            ExportStaffGrid.Style.Add("text-decoration", "none");

            ExportStaffGrid.Style.Add("font-family", "Arial, Helvetica, sans-serif;");

            ExportStaffGrid.Style.Add("font-size", "8px");

            StringReader sr = new StringReader(sw.ToString());

            Document pdfDoc = new Document(PageSize.A2, 7f, 7f, 7f, 0f);

            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);

            PdfWriter.GetInstance(pdfDoc, Response.OutputStream);

            pdfDoc.Open();

            htmlparser.Parse(sr);

            pdfDoc.Close();

            Response.Write(pdfDoc);

            Response.End();
        }
    }
}