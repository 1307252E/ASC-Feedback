using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using _360_Staff_Survey_Web.Class;
using System.Collections;
using System.Web.UI.HtmlControls;

namespace _360_Staff_Survey_Web
{
    public partial class ManageQuestions : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            #region SelectedLinkBtn
            if (Session["AddQuestion"] != null)
            {
                AddQuestion.Style.Add("color", "Purple");
            }

            #endregion
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            HtmlGenericControl body = (HtmlGenericControl)Master.FindControl("b1");
            if (body != null)
            {
                body.Attributes.Remove("onkeydown");
            }

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
                        InitailQuestion();
                    }
                }
                else
                {
                    Response.Redirect("accessdenied.aspx");
                }
            }
        }

        private void InitailQuestion()
        {
            ArrayList listofQuestion = dbmanager.GetAllQuestion();

            if (listofQuestion.Count != 0)
            {
                DataTable dt = new DataTable();
                DataRow dr = null;
                dt.Columns.Add(new DataColumn("QuestionId", typeof(string)));
                dt.Columns.Add(new DataColumn("Question", typeof(string)));
                dt.Columns.Add(new DataColumn("QnInclude", typeof(bool)));
                dt.Columns.Add(new DataColumn("RateOne", typeof(string)));
                dt.Columns.Add(new DataColumn("RateSeven", typeof(string)));
                int index = 1;
                foreach (Question qn in listofQuestion)
                {
                    dr = dt.NewRow();
                    dr["QuestionId"] = index;
                    dr["Question"] = qn.QuestionDetails;
                    dr["QnInclude"] = qn.QuestionInclude;
                    dr["RateOne"] = qn.QuestionRateOne;
                    dr["RateSeven"] = qn.QuestionRateSeven;
                    dt.Rows.Add(dr);
                    index++;
                }

                ViewState["CurrentTable"] = dt;
                AppraisalQuestionGrid.DataSource = dt;
                AppraisalQuestionGrid.DataBind();
                IncludeLbl.Text = "*Include Qn: Checked indicates you would want to include the question to the staff peer evaluation exercise.";
            }
            else
            {
                Session["QuestionPage"] = "ManageQuestions.aspx?return";
                Response.Redirect("~/ManageQuestionsAdd.aspx?addqn");
            }
        }

        protected void AddQuestion_Click(object sender, EventArgs e)
        {
            AddQuestion.Style.Add("color", "Purple");
            Session["AddQuestion"] = true;
            Response.Redirect("~/ManageQuestionsAdd.aspx");
        }

        protected void Save_Click(object sender, EventArgs e)
        {
            try
            {
                int rowIndex = 0;
                ArrayList listofQuestionsData = new ArrayList();
                ArrayList listofQuestionOnlyUpdateInclude = new ArrayList();

                if (ViewState["CurrentTable"] != null)
                {

                    DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];

                    if (dtCurrentTable.Rows.Count > 0)
                    {
                        int count = 0;
                        for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                        {
                            //extract the TextBox values & Checkbox value

                            Label lbl1 = (Label)AppraisalQuestionGrid.Rows[rowIndex].Cells[0].FindControl("QuestionIdLbl");
                            TextBox box2 = (TextBox)AppraisalQuestionGrid.Rows[rowIndex].Cells[1].FindControl("QuestionsTbx");

                            TextBox rate1 = (TextBox)AppraisalQuestionGrid.Rows[rowIndex].Cells[2].FindControl("RateOneTbx");
                            TextBox rate2 = (TextBox)AppraisalQuestionGrid.Rows[rowIndex].Cells[3].FindControl("RateSevenTbx");
                            CheckBox chk2 = (CheckBox)AppraisalQuestionGrid.Rows[rowIndex].Cells[4].FindControl("QnInclude");
                            if (box2.Text.Trim() != "")
                            {
                                ArrayList listofquestion = dbmanager.GetAllQuestionListInOrder();

                                Question qn = new Question(Convert.ToInt32(listofquestion[count].ToString()), box2.Text, chk2.Checked, rate1.Text, rate2.Text);
                                //bool checkqninAppraisal = dbmanager.CheckQuestionIdExistInAppraisal(qn.QuestionID);

                                //if (checkqninAppraisal == false)
                                //{
                                //    listofQuestionsData.Add(qn);
                                //}
                                //if (checkqninAppraisal == true)
                                //{
                                    listofQuestionOnlyUpdateInclude.Add(qn);
                                //}
                            }
                            rowIndex++;
                            count++;
                        }
                        //int returnupdate = dbmanager.UpdateAllAppraisalQuestion(listofQuestionsData);
                        int returnupdate = dbmanager.UpdateAllAppraisalQuestion(listofQuestionOnlyUpdateInclude);
                        //int returnupdateInclude = dbmanager.UpdateAllAppraisalQuestionOnlyInclude(listofQuestionOnlyUpdateInclude);
                        //int totalupdate = returnupdate + returnupdateInclude;
                        //if (totalupdate != 0 && totalupdate == listofQuestionsData.Count + listofQuestionOnlyUpdateInclude.Count)
                        if (returnupdate != 0)
                        {
                            MessageBoxShow(returnupdate + " record(s) updated successfully.");
                        }
                        else
                        {
                            MessageBoxShow("No record is updated, might have data relationship with other tables.");
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                MessageBoxShow(ex.Message);
            }
        }

        private void MessageBoxShow(string message)
        {
            string strScript = "<script>";
            strScript += "alert('" + message + "');";
            strScript += "window.location='ManageQuestions.aspx';";
            strScript += "</script>";
            this.ClientScript.RegisterStartupScript(this.GetType(), "Startup", strScript);
        }

        protected void DeleteBtn_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btn = (LinkButton)sender;
                GridViewRow row = (GridViewRow)btn.NamingContainer;
                int i = Convert.ToInt32(row.RowIndex);

                ArrayList listofquestion = dbmanager.GetAllQuestionListInOrder();

                bool result = dbmanager.DeleteAppraisalQuestion(Convert.ToInt32(listofquestion[i].ToString()));
                if (result == true)
                {
                    dbmanager.UpdateAppraisalQuestionID(Convert.ToInt32(listofquestion[i].ToString()));
                    Response.Redirect("ManageQuestions.aspx");
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
    }

}