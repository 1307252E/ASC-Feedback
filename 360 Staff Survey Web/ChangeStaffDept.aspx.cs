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
using System.Data.SqlClient;
using System.IO;
using System.Globalization;

/* using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Globalization;
using System.Web.Security;
using System.Security.Cryptography; */



namespace _360_Staff_Survey_Web
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        private List<string> myList = new List<string>();
        private List<string> mySecondList = new List<string>();
        private List<string> myThirdList = new List<string>();




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

                }
                else
                {
                    Response.Redirect("accessdenied.aspx");
                }
            }


            ListBox1.Visible = false;
            ListBox2.Visible = false;
            ListBox3.Visible = false;
            ListBox4.Visible = false;
            ListBox5.Visible = false;
            ListBox6.Visible = false;
            ListBox7.Visible = false;
            ListBox8.Visible = false;
            ListBox9.Visible = false;
            ListBox10.Visible = false;
            ListBox11.Visible = false;
            ListBox12.Visible = false;
            ListBox13.Visible = false;
            ListBox14.Visible = false;
            ListBox15.Visible = false;
            ListBox16.Visible = false;
            ListBox17.Visible = false;
            ListBox18.Visible = false;
            ListBox19.Visible = false;
            ListBox20.Visible = false;

            Panel2.Visible = false;
            Panel3.Visible = false;
            Panel4.Visible = false;
            Panel5.Visible = false;
            Panel6.Visible = false;
            Panel7.Visible = false;
            Panel8.Visible = false;
            Panel9.Visible = false;
            Panel10.Visible = false;
            Panel11.Visible = false;
            Panel12.Visible = false;
            Panel13.Visible = false;
            Panel14.Visible = false;
            Panel15.Visible = false;
            Panel16.Visible = false;
            Panel17.Visible = false;
            Panel18.Visible = false;
            Panel19.Visible = false;
            Panel20.Visible = false;
            Panel21.Visible = false;


            if (ddlChooseFunct.SelectedValue == "Update User's section")
            {

                ArrayList listofsection = dbmanager.GetAllSection();

                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Appraisal_System"].ConnectionString.ToString();
                SqlConnection myconn = null;

                staffinfo staff = null;

                for (int i = 0; i < listofsection.Count; i++)
                {

                    if (i == 0)
                    {
                        Panel2.GroupingText = listofsection[i].ToString();
                        Panel2.Visible = true;

                    }
                    else if (i == 1)
                    {
                        Panel3.GroupingText = listofsection[i].ToString();
                        Panel3.Visible = true;
                    }
                    else if (i == 2)
                    {
                        Panel4.GroupingText = listofsection[i].ToString();
                        Panel4.Visible = true;
                    }

                    //TextBox abc = new TextBox();
                    //abc.Visible = true;

                    //Panel a = new Panel();
                    //a.GroupingText = listofsection[i].ToString();
                    //a.CssClass = "defaultPanel";
                    //a.Enabled = true;
                    //a.Visible = true;
                    //a.Height = 150;
                    //a.Width = 400;
                    //Panel1.Controls.Add(a);

                    //ListBox stafflist = new ListBox();
                    //stafflist.Enabled = true;
                    //stafflist.Visible = true;
                    //stafflist.ID = "stafflist" + i;
                    //stafflist.Width = 300;


                    //a.Controls.Add(stafflist);


                    ArrayList listofstaff = new ArrayList();

                    ArrayList listboxlist = new ArrayList();

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
                            string[] split = section.Split(',');
                            ArrayList splitted = new ArrayList();
                            foreach (string word in split)
                            {
                                splitted.Add(word);
                            }

                            for (int p = 0; p < splitted.Count; p++)
                            {
                                if (splitted[p].ToString() == listofsection[i].ToString())
                                {
                                    listboxlist.Add(staffname + " " + uid);

                                }
                            }

                            listofstaff.Add(staff);

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
                    if (i == 0)
                    {
                        ListBox1.Visible = true;
                        ListBox1.DataSource = listboxlist;
                        ListBox1.DataBind();
                    }
                    else if (i == 1)
                    {
                        ListBox2.Visible = true;
                        ListBox2.DataSource = listboxlist;
                        ListBox2.DataBind();
                    }
                    else if (i == 2)
                    {
                        ListBox3.Visible = true;
                        ListBox3.DataSource = listboxlist;
                        ListBox3.DataBind();
                    }

                    //stafflist.AutoPostBack = true;
                    //stafflist.DataSource = listboxlist;
                    //stafflist.DataBind();


                    //stafflist.SelectedIndexChanged += new EventHandler(this.ListBox1_SelectedIndexChanged);
                    //Label1.Text = stafflist.SelectedValue;


                    //Label name = new Label();
                    //name.Visible = true;
                    //name.Enabled = true;
                    //name.Text = listofsection[i].ToString();
                    //name.ID = "section" + i;


                    //a.Controls.Add(name);
                    //AjaxControlToolkit.DragPanelExtender ab = new AjaxControlToolkit.DragPanelExtender();
                    //ab.Enabled = true;
                    //ab.ID = "dragpanel"+i;
                    //ab.DragHandleID = name.ID;

                    //ab.TargetControlID = name.ID;

                    btnChange.Text = "Change user's section";

                }
            }
            else if (ddlChooseFunct.SelectedValue == "Update User's Role")
            {
                ArrayList listofsection = dbmanager.GetAllRole();

                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Appraisal_System"].ConnectionString.ToString();
                SqlConnection myconn = null;


                staffinfo staff = null;

                for (int i = 0; i < listofsection.Count; i++)
                {

                    if (i == 0)
                    {
                        Panel2.GroupingText = listofsection[i].ToString();
                        Panel2.Visible = true;
                    }
                    else if (i == 1)
                    {
                        Panel3.GroupingText = listofsection[i].ToString();
                        Panel3.Visible = true;
                    }
                    else if (i == 2)
                    {
                        Panel4.GroupingText = listofsection[i].ToString();
                        Panel4.Visible = true;
                    }
                    else if (i == 3)
                    {
                        Panel5.GroupingText = listofsection[i].ToString();
                        Panel5.Visible = true;
                    }
                    else if (i == 4)
                    {
                        Panel6.GroupingText = listofsection[i].ToString();
                        Panel6.Visible = true;
                    }

                    //TextBox abc = new TextBox();
                    //abc.Visible = true;

                    //Panel a = new Panel();
                    //a.GroupingText = listofsection[i].ToString();
                    //a.CssClass = "defaultPanel";
                    //a.Enabled = true;
                    //a.Visible = true;
                    //a.Height = 150;
                    //a.Width = 400;
                    //Panel1.Controls.Add(a);

                    //ListBox stafflist = new ListBox();
                    //stafflist.Enabled = true;
                    //stafflist.Visible = true;
                    //stafflist.ID = "stafflist" + i;
                    //stafflist.Width = 300;


                    //a.Controls.Add(stafflist);


                    ArrayList listofstaff = new ArrayList();

                    ArrayList listboxlist = new ArrayList();

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
                            if (role == listofsection[i].ToString())
                            {
                                listboxlist.Add(staffname + " " + uid);

                            }

                            listofstaff.Add(staff);

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

                    if (i == 0)
                    {
                        ListBox1.Visible = true;
                        ListBox1.DataSource = listboxlist;
                        ListBox1.DataBind();
                    }
                    else if (i == 1)
                    {
                        ListBox2.Visible = true;
                        ListBox2.DataSource = listboxlist;
                        ListBox2.DataBind();
                    }
                    else if (i == 2)
                    {
                        ListBox3.Visible = true;
                        ListBox3.DataSource = listboxlist;
                        ListBox3.DataBind();
                    }
                    else if (i == 3)
                    {
                        ListBox4.Visible = true;
                        ListBox4.DataSource = listboxlist;
                        ListBox4.DataBind();
                    }
                    else if (i == 4)
                    {
                        ListBox5.Visible = true;
                        ListBox5.DataSource = listboxlist;
                        ListBox5.DataBind();
                    }

                }
                btnChange.Text = "Change user's role";


            }
            else if (ddlChooseFunct.SelectedValue == "Update User's Function")
            {
                ArrayList listofsection = dbmanager.GetAllFunctionName();

                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Appraisal_System"].ConnectionString.ToString();
                SqlConnection myconn = null;

                staffinfo staff = null;

                for (int i = 0; i < listofsection.Count; i++)
                {

                    if (i == 0)
                    {
                        Panel2.GroupingText = listofsection[i].ToString();
                        Panel2.Visible = true;

                    }
                    else if (i == 1)
                    {
                        Panel3.GroupingText = listofsection[i].ToString();
                        Panel3.Visible = true;
                    }
                    else if (i == 2)
                    {
                        Panel4.GroupingText = listofsection[i].ToString();
                        Panel4.Visible = true;
                    }
                    else if (i == 3)
                    {
                        Panel5.GroupingText = listofsection[i].ToString();
                        Panel5.Visible = true;
                    }
                    else if (i == 4)
                    {
                        Panel6.GroupingText = listofsection[i].ToString();
                        Panel6.Visible = true;
                    }
                    else if (i == 5)
                    {
                        Panel7.GroupingText = listofsection[i].ToString();
                        Panel7.Visible = true;
                    }
                    else if (i == 6)
                    {
                        Panel8.GroupingText = listofsection[i].ToString();
                        Panel8.Visible = true;
                    }
                    
                    //TextBox abc = new TextBox();
                    //abc.Visible = true;

                    //Panel a = new Panel();
                    //a.GroupingText = listofsection[i].ToString();
                    //a.CssClass = "defaultPanel";
                    //a.Enabled = true;
                    //a.Visible = true;
                    //a.Height = 150;
                    //a.Width = 400;
                    //Panel1.Controls.Add(a);

                    //ListBox stafflist = new ListBox();
                    //stafflist.Enabled = true;
                    //stafflist.Visible = true;
                    //stafflist.ID = "stafflist" + i;
                    //stafflist.Width = 300;


                    //a.Controls.Add(stafflist);


                    ArrayList listofstaff = new ArrayList();

                    ArrayList listboxlist = new ArrayList();

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
                            if (function == listofsection[i].ToString())
                            {
                                listboxlist.Add(staffname + " " + uid);

                            }

                            listofstaff.Add(staff);

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
                    if (i == 0)
                    {
                        ListBox1.Visible = true;
                        ListBox1.DataSource = listboxlist;
                        ListBox1.DataBind();
                    }
                    else if (i == 1)
                    {
                        ListBox2.Visible = true;
                        ListBox2.DataSource = listboxlist;
                        ListBox2.DataBind();
                    }
                    else if (i == 2)
                    {
                        ListBox3.Visible = true;
                        ListBox3.DataSource = listboxlist;
                        ListBox3.DataBind();
                    }
                    else if (i == 3)
                    {
                        ListBox4.Visible = true;
                        ListBox4.DataSource = listboxlist;
                        ListBox4.DataBind();
                    }
                    else if (i == 4)
                    {
                        ListBox5.Visible = true;
                        ListBox5.DataSource = listboxlist;
                        ListBox5.DataBind();
                    }
                    else if (i == 5)
                    {
                        ListBox6.Visible = true;
                        ListBox6.DataSource = listboxlist;
                        ListBox6.DataBind();
                    }
                    else if (i == 6)
                    {
                        ListBox7.Visible = true;
                        ListBox7.DataSource = listboxlist;
                        ListBox7.DataBind();
                    }
                    
                }
                btnChange.Text = "Change user's Function";


            }
            else if (ddlChooseFunct.SelectedValue == "Update User's Designation")
            {
                ArrayList listofsection = dbmanager.GetAllDesignation();

                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Appraisal_System"].ConnectionString.ToString();
                SqlConnection myconn = null;

                staffinfo staff = null;

                for (int i = 0; i < listofsection.Count; i++)
                {

                    if (i == 0)
                    {
                        Panel2.GroupingText = listofsection[i].ToString();
                        Panel2.Visible = true;

                    }
                    else if (i == 1)
                    {
                        Panel3.GroupingText = listofsection[i].ToString();
                        Panel3.Visible = true;
                    }
                    else if (i == 2)
                    {
                        Panel4.GroupingText = listofsection[i].ToString();
                        Panel4.Visible = true;
                    }
                    else if (i == 3)
                    {
                        Panel5.GroupingText = listofsection[i].ToString();
                        Panel5.Visible = true;
                    }
                    else if (i == 4)
                    {
                        Panel6.GroupingText = listofsection[i].ToString();
                        Panel6.Visible = true;
                    }
                    else if (i == 5)
                    {
                        Panel7.GroupingText = listofsection[i].ToString();
                        Panel7.Visible = true;
                    }
                    
                    //TextBox abc = new TextBox();
                    //abc.Visible = true;

                    //Panel a = new Panel();
                    //a.GroupingText = listofsection[i].ToString();
                    //a.CssClass = "defaultPanel";
                    //a.Enabled = true;
                    //a.Visible = true;
                    //a.Height = 150;
                    //a.Width = 400;
                    //Panel1.Controls.Add(a);

                    //ListBox stafflist = new ListBox();
                    //stafflist.Enabled = true;
                    //stafflist.Visible = true;
                    //stafflist.ID = "stafflist" + i;
                    //stafflist.Width = 300;


                    //a.Controls.Add(stafflist);


                    ArrayList listofstaff = new ArrayList();

                    ArrayList listboxlist = new ArrayList();

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
                            if (designation == listofsection[i].ToString())
                            {
                                listboxlist.Add(staffname + " " + uid);

                            }

                            listofstaff.Add(staff);

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
                    if (i == 0)
                    {
                        ListBox1.Visible = true;
                        ListBox1.DataSource = listboxlist;
                        ListBox1.DataBind();
                    }
                    else if (i == 1)
                    {
                        ListBox2.Visible = true;
                        ListBox2.DataSource = listboxlist;
                        ListBox2.DataBind();
                    }
                    else if (i == 2)
                    {
                        ListBox3.Visible = true;
                        ListBox3.DataSource = listboxlist;
                        ListBox3.DataBind();
                    }
                    else if (i == 3)
                    {
                        ListBox4.Visible = true;
                        ListBox4.DataSource = listboxlist;
                        ListBox4.DataBind();
                    }
                    else if (i == 4)
                    {
                        ListBox5.Visible = true;
                        ListBox5.DataSource = listboxlist;
                        ListBox5.DataBind();
                    }
                    else if (i == 5)
                    {
                        ListBox6.Visible = true;
                        ListBox6.DataSource = listboxlist;
                        ListBox6.DataBind();
                    }
                    
                }
                btnChange.Text = "Change user's Designation";


            }
        }
        private void MessageBoxShow(string message)
        {
            string strScript = "<script>";
            strScript += "alert('" + message + "');";
            strScript += "</script>";
            this.ClientScript.RegisterStartupScript(this.GetType(), "Startup", strScript);
        }

        protected void btnSearchName_Click(object sender, EventArgs e)
        {

            tbxID.Text = "";
            if (tbxName.Text == "")
            {
                MessageBoxShow("Please input name of staff into the Search By Name textbox.");
            }
            else if (tbxName.Text != "")
            {
                try
                {
                    staffinfo staff = dbmanager.GetStaffDetailsViaName(tbxName.Text);

                    lblID.Text = staff.Uid;
                    lblName.Text = staff.Name;
                    lblSection.Text = staff.Section;
                    lblDesignation.Text = staff.Designation;
                    lblRole.Text = staff.Role;
                    lblDesignation.Text = staff.Designation;
                    lblFunctions.Text = staff.Function;

                    lbluser.Visible = true;
                    Label1.Visible = true;
                    Label2.Visible = true;
                    Label3.Visible = true;
                    lblID.Visible = true;
                    lblName.Visible = true;
                    lblSection.Visible = true;
                    lblDesignation.Visible = true;
                    btnChange.Visible = true;
                    lblRole.Visible = true;
                    Label4.Visible = true;
                    ddlRole.Visible = false;
                    listSection.Visible = false;
                    btnSubmit.Visible = false;
                    ddlFunctions.Visible = false;
                    ddlDesignation.Visible = false;
                    Label6.Visible = true;
                    lblFunctions.Visible = true;
                    lblDesignation.Visible = true;


                    lblSuccess.Text = "";
                }
                catch
                {
                    MessageBoxShow("Failed to find the staff. Please enter the FULL NAME of the staff.");
                    lbluser.Visible = false;
                    Label1.Visible = false;
                    Label2.Visible = false;
                    Label3.Visible = false;
                    lblID.Visible = false;
                    lblName.Visible = false;
                    lblSection.Visible = false;
                    lblDesignation.Visible = false;
                    btnChange.Visible = false;
                    lblRole.Visible = false;
                    Label4.Visible = false;
                    ddlRole.Visible = false;
                    listSection.Visible = false;
                    btnSubmit.Visible = false;
                    Label6.Visible = false;
                    lblFunctions.Visible = false;
                    lblDesignation.Visible = false;
                    ddlDesignation.Visible = false;
                    ddlFunctions.Visible = false;


                }
            }


        }

        protected void btnSearchUID_Click(object sender, EventArgs e)
        {

            tbxName.Text = "";
            if (tbxID.Text == "")
            {
                MessageBoxShow("Please input ID of staff into the Search By ID textbox.");
            }
            else if (tbxID.Text != "")
            {
                try
                {
                    staffinfo staff = dbmanager.GetStaffDetailsViaUid(tbxID.Text);

                    lblID.Text = staff.Uid;
                    lblName.Text = staff.Name;
                    lblSection.Text = staff.Section;
                    lblDesignation.Text = staff.Designation;
                    lblRole.Text = staff.Role;
                    lblDesignation.Text = staff.Designation;
                    lblFunctions.Text = staff.Function;

                    lbluser.Visible = true;
                    Label1.Visible = true;
                    Label2.Visible = true;
                    Label3.Visible = true;
                    lblID.Visible = true;
                    lblName.Visible = true;
                    lblSection.Visible = true;
                    lblDesignation.Visible = true;
                    btnChange.Visible = true;
                    lblRole.Visible = true;
                    Label4.Visible = true;
                    ddlRole.Visible = false;
                    listSection.Visible = false;
                    btnSubmit.Visible = false;
                    ddlFunctions.Visible = false;
                    ddlDesignation.Visible = false;
                    Label6.Visible = true;
                    lblFunctions.Visible = true;
                    lblDesignation.Visible = true;
                    lblSuccess.Text = "";
                }
                catch
                {
                    MessageBoxShow("Failed to find the User ID in the database. Please enter a valid ID.");
                    lbluser.Visible = false;
                    Label1.Visible = false;
                    Label2.Visible = false;
                    Label3.Visible = false;
                    lblID.Visible = false;
                    lblName.Visible = false;
                    lblSection.Visible = false;
                    lblDesignation.Visible = false;
                    btnChange.Visible = false;
                    lblRole.Visible = false;
                    Label4.Visible = false;
                    ddlRole.Visible = false;
                    listSection.Visible = false;
                    btnSubmit.Visible = false;
                    lblFunctions.Visible = false;
                    Label6.Visible = false;
                    lblDesignation.Visible = false;
                    ddlDesignation.Visible = false;
                    ddlFunctions.Visible = false;

                }
            }

        }

        protected void btnChange_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlChooseFunct.SelectedValue == "Update User's section")
                {

                    lblSection.Visible = false;
                    listSection.Visible = true;
                    ArrayList listofsection = dbmanager.GetAllSection();
                    listSection.DataSource = listofsection;
                    listSection.DataBind();
                    btnSubmit.Visible = true;

                    btnChange.Visible = false;
                }
                else if (ddlChooseFunct.SelectedValue == "Update User's Role")
                {

                    lblRole.Visible = false;
                    ddlRole.Visible = true;
                    ArrayList listofRole = dbmanager.GetAllRole();
                    ddlRole.DataSource = listofRole;
                    ddlRole.DataBind();
                    btnSubmit.Visible = true;

                    btnChange.Visible = false;
                }
                else if (ddlChooseFunct.SelectedValue == "Update User's Function")
                {

                    lblFunctions.Visible = false;
                    ddlFunctions.Visible = true;
                    ArrayList listofRole = dbmanager.GetAllFunctionName();
                    ddlFunctions.DataSource = listofRole;
                    ddlFunctions.DataBind();
                    btnSubmit.Visible = true;

                    btnChange.Visible = false;
                }
                else if (ddlChooseFunct.SelectedValue == "Update User's Designation")
                {

                    lblDesignation.Visible = false;
                    ddlDesignation.Visible = true;
                    ArrayList listofRole = dbmanager.GetAllDesignation();
                    ddlDesignation.DataSource = listofRole;
                    ddlDesignation.DataBind();
                    btnSubmit.Visible = true;

                    btnChange.Visible = false;
                }
            }
            catch
            {
                MessageBoxShow("An error occurred!");
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlChooseFunct.SelectedValue == "Update User's section")
                {
                    ArrayList listofSectionItems = new ArrayList();

                    string sect = "";
                    int count = 1;

                    foreach (ListItem listItem in listSection.Items)
                    {
                        if (listItem.Selected == true)
                        {
                            listofSectionItems.Add(listItem.Text);
                        }
                    }
                    foreach (string s in listofSectionItems)
                    {
                        if (count == listofSectionItems.Count)
                        {
                            sect += s;
                        }
                        else
                        {
                            sect += s + ",";
                        }
                        count++;
                    }
                    staffinfo staff = dbmanager.GetStaffDetailsViaUid(lblID.Text);
                    staffinfo updatestaff = new staffinfo(staff.Name, staff.Designation, sect, staff.Function, staff.Uid, staff.Role);
                    bool passfail = dbmanager.UpdateUserInformation(updatestaff);
                    if (passfail == true)
                    {

                        MessageBoxShow("Successfully updated user's section.");
                        lbluser.Visible = true;
                        Label1.Visible = true;
                        Label2.Visible = true;
                        Label3.Visible = true;
                        lblID.Visible = true;
                        lblName.Visible = true;
                        lblSection.Visible = true;
                        lblDesignation.Visible = true;
                        Label4.Visible = true;
                        lblRole.Visible = true;
                        btnChange.Visible = true;
                        btnSubmit.Visible = true;
                        listSection.Visible = true;
                        ddlRole.Visible = false;
                        Label6.Visible = true;
                        lblDesignation.Visible = true;
                        ddlDesignation.Visible = false;
                        ddlFunctions.Visible = false;
                        lblFunctions.Visible = true;
                        lblSuccess.Text = "Successfully updated user's section.";
                        Page_Load(null, EventArgs.Empty);

                    }

                }
                else if (ddlChooseFunct.SelectedValue == "Update User's Role")
                {
                    staffinfo staff = dbmanager.GetStaffDetailsViaUid(lblID.Text);
                    staffinfo updatestaff = new staffinfo(staff.Name, staff.Designation, staff.Section, staff.Function, staff.Uid, ddlRole.SelectedValue);
                    bool passfail = dbmanager.UpdateUserInformation(updatestaff);
                    if (passfail == true)
                    {
                        MessageBoxShow("Successfully updated user's Role.");
                        lbluser.Visible = true;
                        Label1.Visible = true;
                        Label2.Visible = true;
                        Label3.Visible = true;
                        lblID.Visible = true;
                        lblName.Visible = true;
                        lblSection.Visible = true;
                        lblDesignation.Visible = true;
                        Label4.Visible = true;
                        lblRole.Visible = true;
                        btnChange.Visible = true;
                        btnSubmit.Visible = true;
                        listSection.Visible = false;
                        ddlRole.Visible = true;
                        Label6.Visible = true;
                        lblDesignation.Visible = true;
                        ddlDesignation.Visible = false;
                        ddlFunctions.Visible = false;
                        lblFunctions.Visible = true;
                        lblSuccess.Text = "Successfully updated user's Role.";
                        Page_Load(null, EventArgs.Empty);

                    }
                }
                else if (ddlChooseFunct.SelectedValue == "Update User's Function")
                {
                    staffinfo staff = dbmanager.GetStaffDetailsViaUid(lblID.Text);
                    staffinfo updatestaff = new staffinfo(staff.Name, staff.Designation, staff.Section, ddlFunctions.SelectedValue, staff.Uid, staff.Role);
                    bool passfail = dbmanager.UpdateUserInformation(updatestaff);
                    if (passfail == true)
                    {
                        MessageBoxShow("Successfully updated user's Function.");
                        lbluser.Visible = true;
                        Label1.Visible = true;
                        Label2.Visible = true;
                        Label3.Visible = true;
                        lblID.Visible = true;
                        lblName.Visible = true;
                        lblSection.Visible = true;
                        lblDesignation.Visible = true;
                        Label4.Visible = true;
                        lblRole.Visible = true;
                        btnChange.Visible = true;
                        btnSubmit.Visible = true;
                        listSection.Visible = false;
                        ddlRole.Visible = false;
                        Label6.Visible = true;
                        lblDesignation.Visible = true;
                        ddlDesignation.Visible = false;
                        ddlFunctions.Visible = true;
                        lblFunctions.Visible = true;
                        lblSuccess.Text = "Successfully updated user's Function.";
                        Page_Load(null, EventArgs.Empty);

                    }
                }
                else if (ddlChooseFunct.SelectedValue == "Update User's Designation")
                {
                    staffinfo staff = dbmanager.GetStaffDetailsViaUid(lblID.Text);
                    staffinfo updatestaff = new staffinfo(staff.Name, ddlDesignation.SelectedValue, staff.Section, staff.Function, staff.Uid, staff.Role);
                    bool passfail = dbmanager.UpdateUserInformation(updatestaff);
                    if (passfail == true)
                    {
                        MessageBoxShow("Successfully updated user's Designation.");
                        lbluser.Visible = true;
                        Label1.Visible = true;
                        Label2.Visible = true;
                        Label3.Visible = true;
                        lblID.Visible = true;
                        lblName.Visible = true;
                        lblSection.Visible = true;
                        lblDesignation.Visible = true;
                        Label4.Visible = true;
                        lblRole.Visible = true;
                        btnChange.Visible = true;
                        btnSubmit.Visible = true;
                        listSection.Visible = false;
                        ddlRole.Visible = false;
                        Label6.Visible = true;
                        lblDesignation.Visible = true;
                        ddlDesignation.Visible = true;
                        ddlFunctions.Visible = false;
                        lblFunctions.Visible = true;
                        lblSuccess.Text = "Successfully updated user's Designation.";
                        Page_Load(null, EventArgs.Empty);

                    }
                }
                else
                {
                    MessageBoxShow("Failed to update.");
                }

            }
            catch
            {
                MessageBoxShow("An error occurred!");
            }

        }

        protected void ddlChooseFunct_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlChooseFunct.SelectedValue == "Update User's section")
            {

                ArrayList listofsection = dbmanager.GetAllSection();

                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Appraisal_System"].ConnectionString.ToString();
                SqlConnection myconn = null;

                staffinfo staff = null;

                for (int i = 0; i < listofsection.Count; i++)
                {

                    if (i == 0)
                    {
                        Panel2.GroupingText = listofsection[i].ToString();
                    }
                    else if (i == 1)
                    {
                        Panel3.GroupingText = listofsection[i].ToString();
                    }
                    else if (i == 2)
                    {
                        Panel4.GroupingText = listofsection[i].ToString();
                    }
                    else if (i == 3)
                    {
                        Panel5.GroupingText = listofsection[i].ToString();
                    }
                    else if (i == 4)
                    {
                        Panel6.GroupingText = listofsection[i].ToString();
                    }
                    else if (i == 5)
                    {
                        Panel7.GroupingText = listofsection[i].ToString();
                    }
                    else if (i == 6)
                    {
                        Panel8.GroupingText = listofsection[i].ToString();
                    }
                    else if (i == 7)
                    {
                        Panel9.GroupingText = listofsection[i].ToString();
                    }
                    else if (i == 8)
                    {
                        Panel10.GroupingText = listofsection[i].ToString();
                    }
                    else if (i == 9)
                    {
                        Panel11.GroupingText = listofsection[i].ToString();
                    }
                    else if (i == 10)
                    {
                        Panel12.GroupingText = listofsection[i].ToString();
                    }
                    else if (i == 11)
                    {
                        Panel13.GroupingText = listofsection[i].ToString();
                    }
                    else if (i == 12)
                    {
                        Panel14.GroupingText = listofsection[i].ToString();
                    }
                    else if (i == 13)
                    {
                        Panel15.GroupingText = listofsection[i].ToString();
                    }
                    else if (i == 14)
                    {
                        Panel16.GroupingText = listofsection[i].ToString();
                    }
                    else if (i == 15)
                    {
                        Panel17.GroupingText = listofsection[i].ToString();
                    }
                    else if (i == 16)
                    {
                        Panel18.GroupingText = listofsection[i].ToString();
                    }
                    else if (i == 17)
                    {
                        Panel19.GroupingText = listofsection[i].ToString();
                    }
                    else if (i == 18)
                    {
                        Panel20.GroupingText = listofsection[i].ToString();
                    }
                    else if (i == 19)
                    {
                        Panel21.GroupingText = listofsection[i].ToString();
                    }

                    //TextBox abc = new TextBox();
                    //abc.Visible = true;

                    //Panel a = new Panel();
                    //a.GroupingText = listofsection[i].ToString();
                    //a.CssClass = "defaultPanel";
                    //a.Enabled = true;
                    //a.Visible = true;
                    //a.Height = 150;
                    //a.Width = 400;
                    //Panel1.Controls.Add(a);

                    //ListBox stafflist = new ListBox();
                    //stafflist.Enabled = true;
                    //stafflist.Visible = true;
                    //stafflist.ID = "stafflist" + i;
                    //stafflist.Width = 300;


                    //a.Controls.Add(stafflist);


                    ArrayList listofstaff = new ArrayList();

                    ArrayList listboxlist = new ArrayList();

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
                            if (section == listofsection[i].ToString())
                            {
                                listboxlist.Add(staffname + " " + uid);

                            }

                            listofstaff.Add(staff);

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
                    if (i == 0)
                    {
                        ListBox1.Visible = true;
                        ListBox1.DataSource = listboxlist;
                        ListBox1.DataBind();
                    }
                    else if (i == 1)
                    {
                        ListBox2.Visible = true;
                        ListBox2.DataSource = listboxlist;
                        ListBox2.DataBind();
                    }
                    else if (i == 2)
                    {
                        ListBox3.Visible = true;
                        ListBox3.DataSource = listboxlist;
                        ListBox3.DataBind();
                    }
                    else if (i == 3)
                    {
                        ListBox4.Visible = true;
                        ListBox4.DataSource = listboxlist;
                        ListBox4.DataBind();
                    }
                    else if (i == 4)
                    {
                        ListBox5.Visible = true;
                        ListBox5.DataSource = listboxlist;
                        ListBox5.DataBind();
                    }
                    else if (i == 5)
                    {
                        ListBox6.Visible = true;
                        ListBox6.DataSource = listboxlist;
                        ListBox6.DataBind();
                    }
                    else if (i == 6)
                    {
                        ListBox7.Visible = true;
                        ListBox7.DataSource = listboxlist;
                        ListBox7.DataBind();
                    }
                    else if (i == 7)
                    {
                        ListBox8.Visible = true;
                        ListBox8.DataSource = listboxlist;
                        ListBox8.DataBind();
                    }
                    else if (i == 8)
                    {
                        ListBox9.Visible = true;
                        ListBox9.DataSource = listboxlist;
                        ListBox9.DataBind();
                    }
                    else if (i == 9)
                    {
                        ListBox10.Visible = true;
                        ListBox10.DataSource = listboxlist;
                        ListBox10.DataBind();
                    }
                    else if (i == 10)
                    {
                        ListBox11.Visible = true;
                        ListBox11.DataSource = listboxlist;
                        ListBox11.DataBind();
                    }
                    else if (i == 11)
                    {
                        ListBox12.Visible = true;
                        ListBox12.DataSource = listboxlist;
                        ListBox12.DataBind();
                    }
                    else if (i == 12)
                    {
                        ListBox13.Visible = true;
                        ListBox13.DataSource = listboxlist;
                        ListBox13.DataBind();
                    }
                    else if (i == 13)
                    {
                        ListBox14.Visible = true;
                        ListBox14.DataSource = listboxlist;
                        ListBox14.DataBind();
                    }
                    else if (i == 14)
                    {
                        ListBox15.Visible = true;
                        ListBox15.DataSource = listboxlist;
                        ListBox15.DataBind();
                    }
                    else if (i == 15)
                    {
                        ListBox16.Visible = true;
                        ListBox16.DataSource = listboxlist;
                        ListBox16.DataBind();
                    }
                    else if (i == 16)
                    {
                        ListBox17.Visible = true;
                        ListBox17.DataSource = listboxlist;
                        ListBox17.DataBind();
                    }
                    else if (i == 17)
                    {
                        ListBox18.Visible = true;
                        ListBox18.DataSource = listboxlist;
                        ListBox18.DataBind();
                    }
                    else if (i == 18)
                    {
                        ListBox19.Visible = true;
                        ListBox19.DataSource = listboxlist;
                        ListBox19.DataBind();
                    }
                    else if (i == 19)
                    {
                        ListBox20.Visible = true;
                        ListBox20.DataSource = listboxlist;
                        ListBox20.DataBind();
                    }


                    //stafflist.AutoPostBack = true;
                    //stafflist.DataSource = listboxlist;
                    //stafflist.DataBind();


                    //stafflist.SelectedIndexChanged += new EventHandler(this.ListBox1_SelectedIndexChanged);
                    //Label1.Text = stafflist.SelectedValue;


                    //Label name = new Label();
                    //name.Visible = true;
                    //name.Enabled = true;
                    //name.Text = listofsection[i].ToString();
                    //name.ID = "section" + i;


                    //a.Controls.Add(name);
                    //AjaxControlToolkit.DragPanelExtender ab = new AjaxControlToolkit.DragPanelExtender();
                    //ab.Enabled = true;
                    //ab.ID = "dragpanel"+i;
                    //ab.DragHandleID = name.ID;

                    //ab.TargetControlID = name.ID;

                    btnChange.Text = "Change user's section";

                }
            }
            else if (ddlChooseFunct.SelectedValue == "Update User's Role")
            {
                ArrayList listofsection = dbmanager.GetAllRole();

                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Appraisal_System"].ConnectionString.ToString();
                SqlConnection myconn = null;

                staffinfo staff = null;

                for (int i = 0; i < listofsection.Count; i++)
                {

                    if (i == 0)
                    {
                        Panel2.GroupingText = listofsection[i].ToString();
                    }
                    else if (i == 1)
                    {
                        Panel3.GroupingText = listofsection[i].ToString();
                    }
                    else if (i == 2)
                    {
                        Panel4.GroupingText = listofsection[i].ToString();
                    }
                    else if (i == 3)
                    {
                        Panel5.GroupingText = listofsection[i].ToString();
                    }
                    else if (i == 4)
                    {
                        Panel6.GroupingText = listofsection[i].ToString();
                    }
                    else if (i == 5)
                    {
                        Panel7.GroupingText = listofsection[i].ToString();
                    }
                    else if (i == 6)
                    {
                        Panel8.GroupingText = listofsection[i].ToString();
                    }
                    else if (i == 7)
                    {
                        Panel9.GroupingText = listofsection[i].ToString();
                    }
                    else if (i == 8)
                    {
                        Panel10.GroupingText = listofsection[i].ToString();
                    }
                    else if (i == 9)
                    {
                        Panel11.GroupingText = listofsection[i].ToString();
                    }
                    else if (i == 10)
                    {
                        Panel12.GroupingText = listofsection[i].ToString();
                    }
                    else if (i == 11)
                    {
                        Panel13.GroupingText = listofsection[i].ToString();
                    }
                    else if (i == 12)
                    {
                        Panel14.GroupingText = listofsection[i].ToString();
                    }
                    else if (i == 13)
                    {
                        Panel15.GroupingText = listofsection[i].ToString();
                    }
                    else if (i == 14)
                    {
                        Panel16.GroupingText = listofsection[i].ToString();
                    }
                    else if (i == 15)
                    {
                        Panel17.GroupingText = listofsection[i].ToString();
                    }
                    else if (i == 16)
                    {
                        Panel18.GroupingText = listofsection[i].ToString();
                    }
                    else if (i == 17)
                    {
                        Panel19.GroupingText = listofsection[i].ToString();
                    }
                    else if (i == 18)
                    {
                        Panel20.GroupingText = listofsection[i].ToString();
                    }
                    else if (i == 19)
                    {
                        Panel21.GroupingText = listofsection[i].ToString();
                    }

                    //TextBox abc = new TextBox();
                    //abc.Visible = true;

                    //Panel a = new Panel();
                    //a.GroupingText = listofsection[i].ToString();
                    //a.CssClass = "defaultPanel";
                    //a.Enabled = true;
                    //a.Visible = true;
                    //a.Height = 150;
                    //a.Width = 400;
                    //Panel1.Controls.Add(a);

                    //ListBox stafflist = new ListBox();
                    //stafflist.Enabled = true;
                    //stafflist.Visible = true;
                    //stafflist.ID = "stafflist" + i;
                    //stafflist.Width = 300;


                    //a.Controls.Add(stafflist);


                    ArrayList listofstaff = new ArrayList();

                    ArrayList listboxlist = new ArrayList();

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
                            if (role == listofsection[i].ToString())
                            {
                                listboxlist.Add(staffname + " " + uid);

                            }

                            listofstaff.Add(staff);

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
                    if (i == 0)
                    {
                        ListBox1.Visible = true;
                        ListBox1.DataSource = listboxlist;
                        ListBox1.DataBind();
                    }
                    else if (i == 1)
                    {
                        ListBox2.Visible = true;
                        ListBox2.DataSource = listboxlist;
                        ListBox2.DataBind();
                    }
                    else if (i == 2)
                    {
                        ListBox3.Visible = true;
                        ListBox3.DataSource = listboxlist;
                        ListBox3.DataBind();
                    }
                    else if (i == 3)
                    {
                        ListBox4.Visible = true;
                        ListBox4.DataSource = listboxlist;
                        ListBox4.DataBind();
                    }
                    else if (i == 4)
                    {
                        ListBox5.Visible = true;
                        ListBox5.DataSource = listboxlist;
                        ListBox5.DataBind();
                    }
                    else if (i == 5)
                    {
                        ListBox6.Visible = true;
                        ListBox6.DataSource = listboxlist;
                        ListBox6.DataBind();
                    }
                    else if (i == 6)
                    {
                        ListBox7.Visible = true;
                        ListBox7.DataSource = listboxlist;
                        ListBox7.DataBind();
                    }
                    else if (i == 7)
                    {
                        ListBox8.Visible = true;
                        ListBox8.DataSource = listboxlist;
                        ListBox8.DataBind();
                    }
                    else if (i == 8)
                    {
                        ListBox9.Visible = true;
                        ListBox9.DataSource = listboxlist;
                        ListBox9.DataBind();
                    }
                    else if (i == 9)
                    {
                        ListBox10.Visible = true;
                        ListBox10.DataSource = listboxlist;
                        ListBox10.DataBind();
                    }
                    else if (i == 10)
                    {
                        ListBox11.Visible = true;
                        ListBox11.DataSource = listboxlist;
                        ListBox11.DataBind();
                    }
                    else if (i == 11)
                    {
                        ListBox12.Visible = true;
                        ListBox12.DataSource = listboxlist;
                        ListBox12.DataBind();
                    }
                    else if (i == 12)
                    {
                        ListBox13.Visible = true;
                        ListBox13.DataSource = listboxlist;
                        ListBox13.DataBind();
                    }
                    else if (i == 13)
                    {
                        ListBox14.Visible = true;
                        ListBox14.DataSource = listboxlist;
                        ListBox14.DataBind();
                    }
                    else if (i == 14)
                    {
                        ListBox15.Visible = true;
                        ListBox15.DataSource = listboxlist;
                        ListBox15.DataBind();
                    }
                    else if (i == 15)
                    {
                        ListBox16.Visible = true;
                        ListBox16.DataSource = listboxlist;
                        ListBox16.DataBind();
                    }
                    else if (i == 16)
                    {
                        ListBox17.Visible = true;
                        ListBox17.DataSource = listboxlist;
                        ListBox17.DataBind();
                    }
                    else if (i == 17)
                    {
                        ListBox18.Visible = true;
                        ListBox18.DataSource = listboxlist;
                        ListBox18.DataBind();
                    }
                    else if (i == 18)
                    {
                        ListBox19.Visible = true;
                        ListBox19.DataSource = listboxlist;
                        ListBox19.DataBind();
                    }
                    else if (i == 19)
                    {
                        ListBox20.Visible = true;
                        ListBox20.DataSource = listboxlist;
                        ListBox20.DataBind();
                    }

                }
                btnChange.Text = "Change user's role";

            }
            else if (ddlChooseFunct.SelectedValue == "Update User's Function")
            {
                ArrayList listofsection = dbmanager.GetAllFunctionName();

                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Appraisal_System"].ConnectionString.ToString();
                SqlConnection myconn = null;

                staffinfo staff = null;

                for (int i = 0; i < listofsection.Count; i++)
                {

                    if (i == 0)
                    {
                        Panel2.GroupingText = listofsection[i].ToString();
                    }
                    else if (i == 1)
                    {
                        Panel3.GroupingText = listofsection[i].ToString();
                    }
                    else if (i == 2)
                    {
                        Panel4.GroupingText = listofsection[i].ToString();
                    }
                    else if (i == 3)
                    {
                        Panel5.GroupingText = listofsection[i].ToString();
                    }
                    else if (i == 4)
                    {
                        Panel6.GroupingText = listofsection[i].ToString();
                    }
                    else if (i == 5)
                    {
                        Panel7.GroupingText = listofsection[i].ToString();
                    }
                    else if (i == 6)
                    {
                        Panel8.GroupingText = listofsection[i].ToString();
                    }
                    else if (i == 7)
                    {
                        Panel9.GroupingText = listofsection[i].ToString();
                    }
                    else if (i == 8)
                    {
                        Panel10.GroupingText = listofsection[i].ToString();
                    }
                    else if (i == 9)
                    {
                        Panel11.GroupingText = listofsection[i].ToString();
                    }
                    else if (i == 10)
                    {
                        Panel12.GroupingText = listofsection[i].ToString();
                    }
                    else if (i == 11)
                    {
                        Panel13.GroupingText = listofsection[i].ToString();
                    }
                    else if (i == 12)
                    {
                        Panel14.GroupingText = listofsection[i].ToString();
                    }
                    else if (i == 13)
                    {
                        Panel15.GroupingText = listofsection[i].ToString();
                    }
                    else if (i == 14)
                    {
                        Panel16.GroupingText = listofsection[i].ToString();
                    }
                    else if (i == 15)
                    {
                        Panel17.GroupingText = listofsection[i].ToString();
                    }
                    else if (i == 16)
                    {
                        Panel18.GroupingText = listofsection[i].ToString();
                    }
                    else if (i == 17)
                    {
                        Panel19.GroupingText = listofsection[i].ToString();
                    }
                    else if (i == 18)
                    {
                        Panel20.GroupingText = listofsection[i].ToString();
                    }
                    else if (i == 19)
                    {
                        Panel21.GroupingText = listofsection[i].ToString();
                    }

                    //TextBox abc = new TextBox();
                    //abc.Visible = true;

                    //Panel a = new Panel();
                    //a.GroupingText = listofsection[i].ToString();
                    //a.CssClass = "defaultPanel";
                    //a.Enabled = true;
                    //a.Visible = true;
                    //a.Height = 150;
                    //a.Width = 400;
                    //Panel1.Controls.Add(a);

                    //ListBox stafflist = new ListBox();
                    //stafflist.Enabled = true;
                    //stafflist.Visible = true;
                    //stafflist.ID = "stafflist" + i;
                    //stafflist.Width = 300;


                    //a.Controls.Add(stafflist);


                    ArrayList listofstaff = new ArrayList();

                    ArrayList listboxlist = new ArrayList();

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
                            if (function == listofsection[i].ToString())
                            {
                                listboxlist.Add(staffname + " " + uid);

                            }

                            listofstaff.Add(staff);

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
                    if (i == 0)
                    {
                        ListBox1.Visible = true;
                        ListBox1.DataSource = listboxlist;
                        ListBox1.DataBind();
                    }
                    else if (i == 1)
                    {
                        ListBox2.Visible = true;
                        ListBox2.DataSource = listboxlist;
                        ListBox2.DataBind();
                    }
                    else if (i == 2)
                    {
                        ListBox3.Visible = true;
                        ListBox3.DataSource = listboxlist;
                        ListBox3.DataBind();
                    }
                    else if (i == 3)
                    {
                        ListBox4.Visible = true;
                        ListBox4.DataSource = listboxlist;
                        ListBox4.DataBind();
                    }
                    else if (i == 4)
                    {
                        ListBox5.Visible = true;
                        ListBox5.DataSource = listboxlist;
                        ListBox5.DataBind();
                    }
                    else if (i == 5)
                    {
                        ListBox6.Visible = true;
                        ListBox6.DataSource = listboxlist;
                        ListBox6.DataBind();
                    }
                    else if (i == 6)
                    {
                        ListBox7.Visible = true;
                        ListBox7.DataSource = listboxlist;
                        ListBox7.DataBind();
                    }
                    else if (i == 7)
                    {
                        ListBox8.Visible = true;
                        ListBox8.DataSource = listboxlist;
                        ListBox8.DataBind();
                    }
                    else if (i == 8)
                    {
                        ListBox9.Visible = true;
                        ListBox9.DataSource = listboxlist;
                        ListBox9.DataBind();
                    }
                    else if (i == 9)
                    {
                        ListBox10.Visible = true;
                        ListBox10.DataSource = listboxlist;
                        ListBox10.DataBind();
                    }
                    else if (i == 10)
                    {
                        ListBox11.Visible = true;
                        ListBox11.DataSource = listboxlist;
                        ListBox11.DataBind();
                    }
                    else if (i == 11)
                    {
                        ListBox12.Visible = true;
                        ListBox12.DataSource = listboxlist;
                        ListBox12.DataBind();
                    }
                    else if (i == 12)
                    {
                        ListBox13.Visible = true;
                        ListBox13.DataSource = listboxlist;
                        ListBox13.DataBind();
                    }
                    else if (i == 13)
                    {
                        ListBox14.Visible = true;
                        ListBox14.DataSource = listboxlist;
                        ListBox14.DataBind();
                    }
                    else if (i == 14)
                    {
                        ListBox15.Visible = true;
                        ListBox15.DataSource = listboxlist;
                        ListBox15.DataBind();
                    }
                    else if (i == 15)
                    {
                        ListBox16.Visible = true;
                        ListBox16.DataSource = listboxlist;
                        ListBox16.DataBind();
                    }
                    else if (i == 16)
                    {
                        ListBox17.Visible = true;
                        ListBox17.DataSource = listboxlist;
                        ListBox17.DataBind();
                    }
                    else if (i == 17)
                    {
                        ListBox18.Visible = true;
                        ListBox18.DataSource = listboxlist;
                        ListBox18.DataBind();
                    }
                    else if (i == 18)
                    {
                        ListBox19.Visible = true;
                        ListBox19.DataSource = listboxlist;
                        ListBox19.DataBind();
                    }
                    else if (i == 19)
                    {
                        ListBox20.Visible = true;
                        ListBox20.DataSource = listboxlist;
                        ListBox20.DataBind();
                    }

                }
                btnChange.Text = "Change user's Function";
            }
            else if (ddlChooseFunct.SelectedValue == "Update User's Designation")
            {
                ArrayList listofsection = dbmanager.GetAllDesignation();
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Appraisal_System"].ConnectionString.ToString();
                SqlConnection myconn = null;

                staffinfo staff = null;

                for (int i = 0; i < listofsection.Count; i++)
                {

                    if (i == 0)
                    {
                        Panel2.GroupingText = listofsection[i].ToString();
                    }
                    else if (i == 1)
                    {
                        Panel3.GroupingText = listofsection[i].ToString();
                    }
                    else if (i == 2)
                    {
                        Panel4.GroupingText = listofsection[i].ToString();
                    }
                    else if (i == 3)
                    {
                        Panel5.GroupingText = listofsection[i].ToString();
                    }
                    else if (i == 4)
                    {
                        Panel6.GroupingText = listofsection[i].ToString();
                    }
                    else if (i == 5)
                    {
                        Panel7.GroupingText = listofsection[i].ToString();
                    }
                    else if (i == 6)
                    {
                        Panel8.GroupingText = listofsection[i].ToString();
                    }
                    else if (i == 7)
                    {
                        Panel9.GroupingText = listofsection[i].ToString();
                    }
                    else if (i == 8)
                    {
                        Panel10.GroupingText = listofsection[i].ToString();
                    }
                    else if (i == 9)
                    {
                        Panel11.GroupingText = listofsection[i].ToString();
                    }
                    else if (i == 10)
                    {
                        Panel12.GroupingText = listofsection[i].ToString();
                    }
                    else if (i == 11)
                    {
                        Panel13.GroupingText = listofsection[i].ToString();
                    }
                    else if (i == 12)
                    {
                        Panel14.GroupingText = listofsection[i].ToString();
                    }
                    else if (i == 13)
                    {
                        Panel15.GroupingText = listofsection[i].ToString();
                    }
                    else if (i == 14)
                    {
                        Panel16.GroupingText = listofsection[i].ToString();
                    }
                    else if (i == 15)
                    {
                        Panel17.GroupingText = listofsection[i].ToString();
                    }
                    else if (i == 16)
                    {
                        Panel18.GroupingText = listofsection[i].ToString();
                    }
                    else if (i == 17)
                    {
                        Panel19.GroupingText = listofsection[i].ToString();
                    }
                    else if (i == 18)
                    {
                        Panel20.GroupingText = listofsection[i].ToString();
                    }
                    else if (i == 19)
                    {
                        Panel21.GroupingText = listofsection[i].ToString();
                    }

                    //TextBox abc = new TextBox();
                    //abc.Visible = true;

                    //Panel a = new Panel();
                    //a.GroupingText = listofsection[i].ToString();
                    //a.CssClass = "defaultPanel";
                    //a.Enabled = true;
                    //a.Visible = true;
                    //a.Height = 150;
                    //a.Width = 400;
                    //Panel1.Controls.Add(a);

                    //ListBox stafflist = new ListBox();
                    //stafflist.Enabled = true;
                    //stafflist.Visible = true;
                    //stafflist.ID = "stafflist" + i;
                    //stafflist.Width = 300;


                    //a.Controls.Add(stafflist);


                    ArrayList listofstaff = new ArrayList();

                    ArrayList listboxlist = new ArrayList();

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
                            if (designation == listofsection[i].ToString())
                            {
                                listboxlist.Add(staffname + " " + uid);

                            }

                            listofstaff.Add(staff);

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
                    if (i == 0)
                    {
                        ListBox1.Visible = true;
                        ListBox1.DataSource = listboxlist;
                        ListBox1.DataBind();
                    }
                    else if (i == 1)
                    {
                        ListBox2.Visible = true;
                        ListBox2.DataSource = listboxlist;
                        ListBox2.DataBind();
                    }
                    else if (i == 2)
                    {
                        ListBox3.Visible = true;
                        ListBox3.DataSource = listboxlist;
                        ListBox3.DataBind();
                    }
                    else if (i == 3)
                    {
                        ListBox4.Visible = true;
                        ListBox4.DataSource = listboxlist;
                        ListBox4.DataBind();
                    }
                    else if (i == 4)
                    {
                        ListBox5.Visible = true;
                        ListBox5.DataSource = listboxlist;
                        ListBox5.DataBind();
                    }
                    else if (i == 5)
                    {
                        ListBox6.Visible = true;
                        ListBox6.DataSource = listboxlist;
                        ListBox6.DataBind();
                    }
                    else if (i == 6)
                    {
                        ListBox7.Visible = true;
                        ListBox7.DataSource = listboxlist;
                        ListBox7.DataBind();
                    }
                    else if (i == 7)
                    {
                        ListBox8.Visible = true;
                        ListBox8.DataSource = listboxlist;
                        ListBox8.DataBind();
                    }
                    else if (i == 8)
                    {
                        ListBox9.Visible = true;
                        ListBox9.DataSource = listboxlist;
                        ListBox9.DataBind();
                    }
                    else if (i == 9)
                    {
                        ListBox10.Visible = true;
                        ListBox10.DataSource = listboxlist;
                        ListBox10.DataBind();
                    }
                    else if (i == 10)
                    {
                        ListBox11.Visible = true;
                        ListBox11.DataSource = listboxlist;
                        ListBox11.DataBind();
                    }
                    else if (i == 11)
                    {
                        ListBox12.Visible = true;
                        ListBox12.DataSource = listboxlist;
                        ListBox12.DataBind();
                    }
                    else if (i == 12)
                    {
                        ListBox13.Visible = true;
                        ListBox13.DataSource = listboxlist;
                        ListBox13.DataBind();
                    }
                    else if (i == 13)
                    {
                        ListBox14.Visible = true;
                        ListBox14.DataSource = listboxlist;
                        ListBox14.DataBind();
                    }
                    else if (i == 14)
                    {
                        ListBox15.Visible = true;
                        ListBox15.DataSource = listboxlist;
                        ListBox15.DataBind();
                    }
                    else if (i == 15)
                    {
                        ListBox16.Visible = true;
                        ListBox16.DataSource = listboxlist;
                        ListBox16.DataBind();
                    }
                    else if (i == 16)
                    {
                        ListBox17.Visible = true;
                        ListBox17.DataSource = listboxlist;
                        ListBox17.DataBind();
                    }
                    else if (i == 17)
                    {
                        ListBox18.Visible = true;
                        ListBox18.DataSource = listboxlist;
                        ListBox18.DataBind();
                    }
                    else if (i == 18)
                    {
                        ListBox19.Visible = true;
                        ListBox19.DataSource = listboxlist;
                        ListBox19.DataBind();
                    }
                    else if (i == 19)
                    {
                        ListBox20.Visible = true;
                        ListBox20.DataSource = listboxlist;
                        ListBox20.DataBind();
                    }

                }
                btnChange.Text = "Change user's Designation";
            }

        }



        //protected void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        //{

        //    // Split string on spaces.
        //    // ... This will separate all the words.
        //    string[] words = s.Split(' ');
        //    string id = "";
        //    foreach (string word in words)
        //    {
        //        id = word;
        //    }
        //    staffinfo staff = dbmanager.GetStaffDetailsViaUid(id);
        //    lblID.Text = id;
        //    lblName.Text = staff.Name;
        //    lblDesignation.Text = staff.Designation;
        //    lblSection.Text = staff.Section;

        //    lbluser.Visible = true;
        //    Label1.Visible = true;
        //    Label2.Visible = true;
        //    Label3.Visible = true;
        //    lblID.Visible = true;
        //    lblName.Visible = true;
        //    lblSection.Visible = true;
        //    lblDesignation.Visible = true;


        //}

        //    protected void ListBox2_SelectedIndexChanged(object sender, EventArgs e)
        //    {
        //        string s = ListBox2.SelectedItem.ToString();

        //        // Split string on spaces.
        //        // ... This will separate all the words.
        //        string[] words = s.Split(' ');
        //        string id = "";
        //        foreach (string word in words)
        //        {
        //            id = word;
        //        }
        //        staffinfo staff = dbmanager.GetStaffDetailsViaUid(id);
        //        lblID.Text = id;
        //        lblName.Text = staff.Name;
        //        lblDesignation.Text = staff.Designation;
        //        lblSection.Text = staff.Section;

        //        lbluser.Visible = true;
        //        Label1.Visible = true;
        //        Label2.Visible = true;
        //        Label3.Visible = true;
        //        lblID.Visible = true;
        //        lblName.Visible = true;
        //        lblSection.Visible = true;
        //        lblDesignation.Visible = true;
        //    }
    }

}