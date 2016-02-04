using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using _360_Staff_Survey_Web.Class;

namespace _360_Staff_Survey_Web
{
    public partial class RateInfo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Role"] == null)
            {
                Response.Redirect("accessdenied.aspx");
            }
            else
            {
                lblRate.Text += "<h2>Rate Information</h2>";
                ArrayList rate = dbmanager.GetAllChoice();
                rate.Remove("N/A");
                rate.Add("N/A");

                string rateContent = "";

                if (rate.Count > 0)
                {
                    rateContent += "<br>";
                    rateContent += "<table width='100%'>";
                    rateContent += "<tr>";
                    
                    // change rate to invert
                    for (int i = 0; i < rate.Count; i++)
                    {
                        if (i == 0)
                        {
                            rateContent += "<td align='center' class='style1' width='248px'>";
                            rateContent += rate[i].ToString();
                            rateContent += "</td>";
                        }
                        else if (i == (rate.Count - 2))
                        {
                            rateContent += "<td align='center' class='style1' width='248px'>";
                            rateContent += rate[i].ToString();
                            rateContent += "</td>";
                        }
                        else if (rate[i].ToString()!= "N/A")
                        {
                            rateContent += "<td align='center' class='style1'>";
                            rateContent += rate[i].ToString();
                            rateContent += "</td>";
                        }

                    }
                    rateContent += "</tr>";
                    rateContent += "</table>";
                    rateContent += "<table width='100%'>";
                    rateContent += "<tr>";
                    for (int i = 0; i < rate.Count; i++)
                    {
                        if (i == 0)
                        {
                            rateContent += "<td align='center' class='style1' width='248px'>";
                            rateContent += "<img src='Image/dot.jpg'/>";
                            rateContent += "</td>";
                        }
                        else if (i == rate.Count - 2)
                        {
                            rateContent += "<td align='center' class='style1' width='248px'>";
                            rateContent += "<img src='Image/dot.jpg'/>";
                            rateContent += "</td>";
                        }
                        else if (rate[i].ToString()!= "N/A")
                        {
                            rateContent += "<td align='center' class='style1'>";
                            rateContent += "<img src='Image/dot2.png'/>";
                            rateContent += "</td>";
                        }
                    }
                    rateContent += "</tr>";
                    rateContent += "</table>";
                    lblTableRate.Text = rateContent;
                }
                if (Session["Gridindex"] != null)
                {
                    ArrayList listQuestion = (ArrayList)Session["AllQuestion"];
                    int index = Convert.ToInt32(Session["Gridindex"].ToString());
                    int no = index + 1;
                    Question q = (Question)listQuestion[index];
                    lblRate.Text += "Q" + no + ") " + q.QuestionDetails;
                    lblRate.Text += "<br>";
                    // change between one and seven
                    tbxRateOne.Text = q.QuestionRateSeven;
                    tbxRateSeven.Text = q.QuestionRateOne;
                }
            }
        }
    }
}