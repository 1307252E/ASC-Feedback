using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using _360_Staff_Survey_Web.Class;
using System.Collections;

namespace _360_Staff_Survey_Web
{
    public partial class ManageQuestionsAdd : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            #region SelectedLinkBtn
            if (Session["BackBtnQnLink"] != null)
            {
                BackBtnLink.Style.Add("color", "Purple");
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
                        InitialAddQuestion();
                    }
                }
                else
                {
                    Response.Redirect("accessdenied.aspx");
                }
            }
        }

        private void InitialAddQuestion()
        {
            int temp = 0;
            DataTable dt = new DataTable();
            DataRow dr = null;

            int result = dbmanager.GetCountAllQuestion();
            if (result > 0)
            {
                temp = result + 1;
            }

            dt.Columns.Add(new DataColumn("RowNumber", typeof(string)));
            dt.Columns.Add(new DataColumn("Question", typeof(string)));
            dt.Columns.Add(new DataColumn("QuestionInclude", typeof(bool)));
            dt.Columns.Add(new DataColumn("RateOne", typeof(string)));
            dt.Columns.Add(new DataColumn("RateSeven", typeof(string)));
            dr = dt.NewRow();

            dr["RowNumber"] = temp;
            dr["Question"] = string.Empty;
            dr["QuestionInclude"] = true;
            dr["RateOne"] = string.Empty;
            dr["RateSeven"] = string.Empty;
            dt.Rows.Add(dr);

            //Store the DataTable in ViewState for future reference 

            ViewState["AddTable"] = dt;

            //Bind the Gridview 
            AddQuestionGrid.DataSource = dt;
            AddQuestionGrid.DataBind();
            TextBox box1 = (TextBox)AddQuestionGrid.Rows[0].Cells[1].FindControl("QuestionTbx");
            box1.Focus();

            TextBox box2 = (TextBox)AddQuestionGrid.Rows[0].Cells[2].FindControl("RateOneTbx");
            TextBox box3 = (TextBox)AddQuestionGrid.Rows[0].Cells[3].FindControl("RateSevenTbx");
            box2.Focus();
            box3.Focus();

            Session["temp"] = temp;
            IncludeLbl.Text = "*Include Qn: Checked indicates you would want to include the question to the staff peer evaluation exercise.";
        }

        private void AddNewRowToGrid()
        {
            try
            {
                int temp = (int)Session["temp"];
                int rowIndex = 0;

                if (ViewState["AddTable"] != null)
                {

                    DataTable dtCurrentTable = (DataTable)ViewState["AddTable"];
                    DataRow drCurrentRow = null;

                    if (dtCurrentTable.Rows.Count > 0)
                    {

                        for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                        {

                            //extract the TextBox values

                            TextBox box1 = (TextBox)AddQuestionGrid.Rows[rowIndex].Cells[1].FindControl("QuestionTbx");
                            TextBox box2 = (TextBox)AddQuestionGrid.Rows[0].Cells[2].FindControl("RateOneTbx");
                            TextBox box3 = (TextBox)AddQuestionGrid.Rows[0].Cells[3].FindControl("RateSevenTbx");
                            CheckBox chk1 = (CheckBox)AddQuestionGrid.Rows[rowIndex].Cells[4].FindControl("QnInclude");

                            drCurrentRow = dtCurrentTable.NewRow();
                            drCurrentRow["RowNumber"] = i + temp;

                            dtCurrentTable.Rows[i - 1]["Question"] = box1.Text;
                            dtCurrentTable.Rows[i - 1]["RateOne"] = box2.Text;
                            dtCurrentTable.Rows[i - 1]["RateSeven"] = box3.Text;
                            dtCurrentTable.Rows[i - 1]["QuestionInclude"] = chk1.Checked;

                            rowIndex++;
                        }

                        dtCurrentTable.Rows.Add(drCurrentRow);
                        ViewState["AddTable"] = dtCurrentTable;

                        AddQuestionGrid.DataSource = dtCurrentTable;
                        AddQuestionGrid.DataBind();
                        TextBox bb = (TextBox)AddQuestionGrid.Rows[rowIndex].Cells[1].FindControl("QuestionTbx");
                        bb.Focus();
                        TextBox bb2 = (TextBox)AddQuestionGrid.Rows[rowIndex].Cells[2].FindControl("RateOneTbx");
                        TextBox bb3 = (TextBox)AddQuestionGrid.Rows[rowIndex].Cells[3].FindControl("RateSevenTbx");
                        bb2.Focus();
                        bb3.Focus();
                    }
                }

                else
                {
                    Response.Write("ViewState is null");
                }

                //Set Previous Data on Postbacks 
                SetPreviousData();
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message);
            }
        }

        private void SetPreviousData()
        {
            try
            {
                int rowcount = dbmanager.GetCountAllQuestion() + 1;
                int rowIndex = 0;
                if (ViewState["AddTable"] != null)
                {

                    DataTable dt = (DataTable)ViewState["AddTable"];
                    if (dt.Rows.Count > 0)
                    {

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            AddQuestionGrid.Rows[rowIndex].Cells[0].Text = rowcount.ToString();

                            TextBox box1 = (TextBox)AddQuestionGrid.Rows[rowIndex].Cells[1].FindControl("QuestionTbx");
                            TextBox box2 = (TextBox)AddQuestionGrid.Rows[rowIndex].Cells[2].FindControl("RateOneTbx");
                            TextBox box3 = (TextBox)AddQuestionGrid.Rows[rowIndex].Cells[3].FindControl("RateSevenTbx");
                            CheckBox chk1 = (CheckBox)AddQuestionGrid.Rows[rowIndex].Cells[4].FindControl("QnInclude");

                            if (i <= dt.Rows.Count)
                            {
                                box1.Text = dt.Rows[i]["Question"].ToString();
                                box2.Text = dt.Rows[i]["RateOne"].ToString();
                                box3.Text = dt.Rows[i]["RateSeven"].ToString();
                                chk1.Checked = Convert.ToBoolean(dt.Rows[i]["QuestionInclude"]);
                            }
                            rowIndex++;
                            rowcount++;
                        }
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
            strScript += "window.location='ManageQuestionsAdd.aspx';";
            strScript += "</script>";
            this.ClientScript.RegisterStartupScript(this.GetType(), "Startup", strScript);
        }

        protected void BackBtnLink_Click(object sender, EventArgs e)
        {
            BackBtnLink.Style.Add("color", "Purple");
            Session["BackBtnQnLink"] = true;
            if (Session["QuestionPage"] != null)
            {
                Response.Redirect(Session["QuestionPage"].ToString());
            }
            else
            {
                Response.Redirect("ManageQuestions.aspx");
            }
        }

        protected void AddBtn_Click(object sender, EventArgs e)
        {
            AddNewRowToGrid();
        }

        protected void RemoveBtnLink_Click(object sender, EventArgs e)
        {
            LinkButton lb = (LinkButton)sender;
            GridViewRow gvRow = (GridViewRow)lb.NamingContainer;
            int rowID = gvRow.RowIndex;
            if (ViewState["AddTable"] != null)
            {
                DataTable dt = (DataTable)ViewState["AddTable"];
                if (dt.Rows.Count > 1)
                {
                    if (gvRow.RowIndex < dt.Rows.Count - 1)
                    {
                        //Remove the Selected Row data
                        dt.Rows.Remove(dt.Rows[rowID]);
                    }
                }
                //Store the current data in ViewState for future reference
                ViewState["AddTable"] = dt;
                //Re bind the GridView for the updated data
                AddQuestionGrid.DataSource = dt;
                AddQuestionGrid.DataBind();
            }

            //Set Previous Data on Postbacks
            SetPreviousData();
        }

        protected void AddQuestionGrid_RowCreated(object sender, GridViewRowEventArgs e)
        {
            DataTable dt = (DataTable)ViewState["AddTable"];
            LinkButton lb = (LinkButton)e.Row.FindControl("RemoveBtn");
            if (lb != null)
            {
                if (dt.Rows.Count > 1)
                {
                    if (e.Row.RowIndex == dt.Rows.Count - 1)
                    {
                        lb.Visible = false;
                    }
                }
                else
                {
                    lb.Visible = false;
                }
            }
        }

        protected void AddQuestionBtn_Click(object sender, EventArgs e)
        {
            try
            {
                int rowIndex = 0;
                ArrayList listofQuestionsData = new ArrayList();

                if (ViewState["AddTable"] != null)
                {

                    DataTable dtCurrentTable = (DataTable)ViewState["AddTable"];

                    if (dtCurrentTable.Rows.Count > 0)
                    {

                        for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                        {
                            //extract the TextBox values & Checkbox value
                            string box1 = (string)AddQuestionGrid.Rows[rowIndex].Cells[0].Text;
                            TextBox box2 = (TextBox)AddQuestionGrid.Rows[rowIndex].Cells[1].FindControl("QuestionTbx");
                            TextBox box3 = (TextBox)AddQuestionGrid.Rows[rowIndex].Cells[2].FindControl("RateOneTbx");
                            TextBox box4 = (TextBox)AddQuestionGrid.Rows[rowIndex].Cells[3].FindControl("RateSevenTbx");
                            CheckBox chk2 = (CheckBox)AddQuestionGrid.Rows[rowIndex].Cells[4].FindControl("QnInclude");

                            if (box2.Text.Trim() != "" && box3.Text.Trim() != "" && box4.Text.Trim() != "") 
                            {
                                bool valid = dbmanager.GetAppraisalQuestionValid(box2.Text.Trim());
                                if (valid == false)
                                {
                                    Question qn = new Question(Convert.ToInt32(box1), box2.Text.Trim(), chk2.Checked, box3.Text.Trim(), box4.Text.Trim());
                                    listofQuestionsData.Add(qn);
                                }
                            }
                            rowIndex++;

                        }
                        int returnupdate = dbmanager.InsertAllAppraisalQuestions(listofQuestionsData);
                        if (returnupdate > 0)
                        {
                            MessageBoxShow(returnupdate + " record(s) added.");
                        }
                        else
                        {
                            MessageBoxShow("No record is added.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message);
            }
        }

    }
}