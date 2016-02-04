using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using _360_Staff_Survey_Web.Class;
using System.Collections;
using System.Data;

namespace _360_Staff_Survey_Web
{
    public partial class ManageUser : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            #region SelectedLinkBtn
            if (Session["AddUser"] != null)
            {
                lbAddUser.Style.Add("color", "Purple");
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
                        BindGrid();
                    }
                }
                else
                {
                    Response.Redirect("accessdenied.aspx");
                }
            }

        }

        protected void BindGrid()
        {
            ArrayList listofStaff = dbmanager.GetAllStaffDetails();
            if (listofStaff.Count != 0)
            {
                ManageStaffGrid.DataSource = listofStaff;
                ManageStaffGrid.DataBind();

                Session["StaffGrid"] = ManageStaffGrid.DataSource;

                if (ManageStaffGrid.PageCount > 10)
                {
                    lbViewAll.Text = "View all";
                }
            }
            else
            {
                Session["ManageUserPage"] = "ManageUserAdd.aspx?return";
                Response.Redirect("~/ManageUserAdd.aspx?adduser");
            }
        }

        protected void AddUser_Click(object sender, EventArgs e)
        {
            //lbAddUser.Style.Add("color", "Purple");
            Session["AddUser"] = true;
            Response.Redirect("~/ManageUserAdd.aspx");
        }

        private void MessageBoxShow(string message)
        {
            string strScript = "<script>";
            strScript += "alert('" + message + "');";
            strScript += "window.location='ManageUser.aspx';";
            strScript += "</script>";
            this.ClientScript.RegisterStartupScript(this.GetType(), "Startup", strScript);
        }

        private void MessageBoxShowWithOutRedirect(string message)
        {
            string strScript = "<script>";
            strScript += "alert('" + message + "');";
            strScript += "</script>";
            this.ClientScript.RegisterStartupScript(this.GetType(), "Startup", strScript);
        }

        protected void ConfirmDelete_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            int i = Convert.ToInt32(row.RowIndex);

            string name = (string)ManageStaffGrid.Rows[i].Cells[0].Text;
            string designation = (string)ManageStaffGrid.Rows[i].Cells[1].Text;
            Label sectionlbl = ((Label)ManageStaffGrid.Rows[i].Cells[2].FindControl("lblSection"));
            string section = sectionlbl.Text;
            Label functions = ((Label)ManageStaffGrid.Rows[i].Cells[3].FindControl("lblFunction"));
            string uid = (string)ManageStaffGrid.Rows[i].Cells[4].Text;
            Label rolee = ((Label)ManageStaffGrid.Rows[i].Cells[5].FindControl("lblDropdown"));

            //bind panel
            #region bind panel
            try
            {
                string legend = "";
                legend += "<table align='left'><tr><td>";
                legend += "<b>Delete system user</b><br>";
                legend += "Please confirm staff information before deletion is processed.<br>";
                legend += "</td></tr></table>";
                LegendLbl.Text = legend;

                NameLbl.Text = name;
                DesignationLbl.Text = designation;
                SectionLbl.Text = section;
                FunctionLbl.Text = functions.Text;
                UserIDLbl.Text = uid;
                RoleLbl.Text = rolee.Text;
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message);
            }
            #endregion

            this.ModalPopupExtender2.Show();
        }

        protected void DeleteBtn_Click(object sender, EventArgs e)
        {
            try
            {
                string uid = UserIDLbl.Text;
                bool deleted = dbmanager.DeleteStaffviaUserId(uid);
                if (deleted == true)
                {
                    //delete all where no staff info found
                    dbmanager.DeleteAllAppraisalFromNonStaff();
                    dbmanager.DeleteAllAppraisalLoginFromNonStaff();
                    MessageBoxShow("User deleted successfully.");
                }
                else
                {
                    MessageBoxShow("Fail to delete, might have data relationship with other tables.");
                }
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message);
            }
        }

        protected void UpdateBtn_Click(object sender, EventArgs e)
        {
            try
            {
                #region update fields
                try
                {

                    bool chkresult = true;
                    int count = 1;
                    string sect = "";
                    ArrayList listofSectionItem = new ArrayList();

                    for (int i = 0; i < listSection.Items.Count; i++)
                    {
                        if (listSection.Items[i].Selected == true)
                        {
                            listofSectionItem.Add(listSection.Items[i].Text);
                        }
                    }
                    foreach (string s in listofSectionItem)
                    {
                        if (count == listofSectionItem.Count)
                        {
                            sect += s;
                        }
                        else
                        {
                            sect += s + ",";
                        }
                        count++;
                    }
                    if (listofSectionItem.Count == 0)
                    {
                        MessageBoxShowWithOutRedirect("Please select at least one section.");
                        chkresult = false;
                    }
                    if (chkresult == true)
                    {
                        if (sect.Contains("ALL"))
                        {
                            sect = "ALL";
                        }
                        string name = lblStaffName.Text;
                        string designation = lblStaffDesignation.Text;
                        string function = ddlFunction.Text;
                        string role = ddlRole.Text;
                        string uid = lblUid.Text;
                        staffinfo stf = new staffinfo(name, designation, sect, function, uid, role);
                        bool result = dbmanager.UpdateUserInformation(stf);
                        {
                            if (result == true)
                            {
                                MessageBoxShow("Updated successfully.");
                            }
                            else
                            {
                                MessageBoxShow("Fail to update.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBoxShow(ex.Message);
                }
                #endregion

            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message);
            }
        }

        protected void EditBtn_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            int i = Convert.ToInt32(row.RowIndex);

            string name = (string)ManageStaffGrid.Rows[i].Cells[0].Text;
            string designation = (string)ManageStaffGrid.Rows[i].Cells[1].Text;
            Label sectionlbl = ((Label)ManageStaffGrid.Rows[i].Cells[2].FindControl("lblSection"));
            string section = sectionlbl.Text;
            Label functions = ((Label)ManageStaffGrid.Rows[i].Cells[3].FindControl("lblFunction"));
            string uid = (string)ManageStaffGrid.Rows[i].Cells[4].Text;
            Label rolee = ((Label)ManageStaffGrid.Rows[i].Cells[5].FindControl("lblDropdown"));


            //bind panel
            #region bind panel
            try
            {
                string legend = "";
                legend += "<table align='left'><tr><td>";
                legend += "<b>Legend of Managing User</b><br>";
                legend += "Managing on staff: Only Section, Function and Role columns are editable fields.<br>";
                legend += "</td></tr></table>";
                lblLegend.Text = legend;

                ArrayList listofSection = dbmanager.GetAllSection();
                ArrayList listofFunction = dbmanager.GetAllFunction();
                ArrayList listofRole = dbmanager.GetAllRole();
                string[] sectionlist = section.ToString().Split(',');

                if (listofSection.Count != 0 && listofFunction.Count != 0 && listofRole.Count != 0)
                {
                    lblStaffName.Text = name;
                    lblStaffDesignation.Text = designation;
                    lblUid.Text = uid;
                    listSection.DataSource = listofSection;
                    listSection.DataBind();
                    foreach (ListItem item in listSection.Items)
                    {
                        foreach (string sect in sectionlist)
                        {
                            if (item.Text == sect)
                            {
                                item.Selected = true;
                                break;
                            }
                        }
                    }
                    ddlFunction.DataSource = listofFunction;
                    ddlFunction.DataBind();
                    ddlFunction.SelectedValue = functions.Text;

                    ddlRole.DataSource = listofRole;
                    ddlRole.DataBind();
                    ddlRole.SelectedValue = rolee.Text;
                }
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message);
            }
            #endregion

            this.ModalPopupExtender.Show();
        }

        protected void BtnEdit_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            int i = Convert.ToInt32(row.RowIndex);

            string name = (string)ManageStaffGrid.Rows[i].Cells[0].Text;
            string designation = (string)ManageStaffGrid.Rows[i].Cells[1].Text;
            Label sectionlbl = ((Label)ManageStaffGrid.Rows[i].Cells[2].FindControl("lblSection"));
            string section = sectionlbl.Text;
            Label functions = ((Label)ManageStaffGrid.Rows[i].Cells[3].FindControl("lblFunction"));
            string uid = (string)ManageStaffGrid.Rows[i].Cells[4].Text;
            Label rolee = ((Label)ManageStaffGrid.Rows[i].Cells[5].FindControl("lblDropdown"));


            //bind panel
            #region bind panel
            try
            {
                string legend = "";
                legend += "<table align='left'><tr><td>";
                legend += "<b>Legend of Managing User</b><br>";
                legend += "Managing on staff: Only Section, Function and Role columns are editable fields.<br>";
                legend += "</td></tr></table>";
                lblLegend.Text = legend;

                ArrayList listofSection = dbmanager.GetAllSection();
                ArrayList listofFunction = dbmanager.GetAllFunction();
                ArrayList listofRole = dbmanager.GetAllRole();
                string[] sectionlist = section.ToString().Split(',');

                if (listofSection.Count != 0 && listofFunction.Count != 0 && listofRole.Count != 0)
                {
                    lblStaffName.Text = name;
                    lblStaffDesignation.Text = designation;
                    lblUid.Text = uid;
                    listSection.DataSource = listofSection;
                    listSection.DataBind();
                    foreach (ListItem item in listSection.Items)
                    {
                        foreach (string sect in sectionlist)
                        {
                            if (item.Text == sect)
                            {
                                item.Selected = true;
                                break;
                            }
                        }
                    }
                    ddlFunction.DataSource = listofFunction;
                    ddlFunction.DataBind();
                    ddlFunction.SelectedValue = functions.Text;

                    ddlRole.DataSource = listofRole;
                    ddlRole.DataBind();
                    ddlRole.SelectedValue = rolee.Text;
                }
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message);
            }
            #endregion

            this.ModalPopupExtender.Show();
        }

        protected void ManageStaffGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ManageStaffGrid.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        //protected void viewAllBtn_Click(object sender, EventArgs e)
        //{
        //    if (Session["StaffGrid"] != null)
        //    {
        //        ArrayList arrayTable = (ArrayList)Session["StaffGrid"];

        //        if (Session["ViewALLExportUser"] == null)
        //        {
        //            ManageStaffGrid.AllowPaging = false;
        //            ManageStaffGrid.DataSource = arrayTable;
        //            ManageStaffGrid.DataBind();
        //            lbViewAll.Text = "view with paging";
        //            Session["ViewALLExportUser"] = true;
        //        }
        //        else
        //        {
        //            ManageStaffGrid.AllowPaging = true;
        //            ManageStaffGrid.DataSource = arrayTable;
        //            ManageStaffGrid.DataBind();
        //            lbViewAll.Text = "view all";
        //            Session["ViewALLExportUser"] = null;
        //        }
        //    }
        //}

        protected void lbViewAll_Click(object sender, EventArgs e)
        {
            if (Session["StaffGrid"] != null)
            {
                ArrayList arrayTable = (ArrayList)Session["StaffGrid"];

                if (Session["ViewALLExportUser"] == null)
                {
                    ManageStaffGrid.AllowPaging = false;
                    ManageStaffGrid.DataSource = arrayTable;
                    ManageStaffGrid.DataBind();
                    lbViewAll.Text = "view with paging";
                    Session["ViewALLExportUser"] = true;
                }
                else
                {
                    ManageStaffGrid.AllowPaging = true;
                    ManageStaffGrid.DataSource = arrayTable;
                    ManageStaffGrid.DataBind();
                    lbViewAll.Text = "view all";
                    Session["ViewALLExportUser"] = null;
                }
            }
        }
      

    }
}