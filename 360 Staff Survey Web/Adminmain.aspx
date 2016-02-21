<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Adminmain.aspx.cs" Inherits="_360_Staff_Survey_Web.Adminmain" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%--    <div align="right">
        <asp:Label ID="WelcomeLbl" runat="server" Text="Welcome: " CssClass="label"></asp:Label>
        <asp:Label ID="staffName" runat="server" ForeColor="#009933" CssClass="label"></asp:Label>
    </div>--%>


    <link rel="stylesheet" type="text/css" href="Styles/ddmenu.css">
    <nav id="ddmenu">
        <script src="Scripts/ddmenu.js" type="text/javascript"></script>
    <div class="menu-icon"></div>
    <ul>
         <li></li>
   
         <li>
            <span class="top-heading">Feedback</span><i class="caret"></i>
             <div class="dropdown right-aligned">
                <div class="dd-inner">
                    <div class="column">
                        <h3 style="font-size:medium">Manage Feedback</h3>
                         <asp:Label ID="AppraisalDisplayDetailsLbl" runat="server" CssClass="label"></asp:Label>
                        </td><asp:LinkButton ID="AppraisalDisplayDetailsLink" runat="server" CssClass="hereLink"
                                OnClick="AppraisalDisplayDetailsLink_Click">Customize Feedback Message</asp:LinkButton>
                        <%--<a href="AppraisalDisplay.aspx">View Appraisal Detials</a>--%>
                    </div>
                </div>
            </div>
        </li>
        <li>
            <a class="top-heading">System</a>
			<i class="caret"></i>            
            <div class="dropdown right-aligned">
                <div class="dd-inner">
                    <div class="column">
                        <h3 style="font-size:medium">Manage System</h3>

                        <asp:Label ID="ManageUserLbl" runat="server" CssClass="label"></asp:Label>
                        <asp:LinkButton ID="ManageUserLink" runat="server" CssClass="hereLink" OnClick="ManageUserLink_Click">Manage User</asp:LinkButton>
                        <asp:Label ID="ExportImportStaffListLbl" runat="server" CssClass="label"></asp:Label>
                        <asp:LinkButton ID="ExportImportStaffLink" runat="server" CssClass="hereLink" OnClick="ExportImportStaffLink_Click">Export/Import Staff List</asp:LinkButton>
                        <asp:Label ID="SetOpenCloseLbl" runat="server" CssClass="label"></asp:Label>
                        <asp:LinkButton ID="SetOpenCloseLink" runat="server" CssClass="hereLink" OnClick="SetOpenCloseLink_Click">Set/Close Survey Date</asp:LinkButton>
                    </div>
                </div>
            </div>
        </li>
        <li>
            <span class="top-heading">Questions and Users</span>
			<i class="caret"></i>           
            <div class="dropdown right-aligned">
                <div class="dd-inner">
                    <div class="column">
                        <h3 style="font-size:medium">Questions</h3>
                        <asp:Label ID="ManageQuestionsLbl" runat="server" CssClass="label"></asp:Label>
                        <asp:LinkButton ID="ManageQuestionLink" runat="server" CssClass="hereLink" OnClick="ManageQuestionLink_Click">Manage Questions</asp:LinkButton>
                    </div>
                    <div class="column">
                        <h3 style="font-size:medium">Users</h3>
                        <asp:Label ID="ResetPasswordLbl" runat="server" CssClass="label"></asp:Label>
                            <asp:LinkButton ID="ResetPasswordLink" runat="server" CssClass="hereLink" OnClick="ResetPasswordLink_Click">Reset Staff Password</asp:LinkButton>
                        <asp:Label ID="UpdateStaffDept" runat="server" CssClass="label"></asp:Label>
                            <asp:LinkButton ID="UpdateStaffDeptLink" runat="server" CssClass="hereLink" OnClick="UpdateStaffLink_Click">Update Staff Details</asp:LinkButton>
                    </div>
                    

                    </div>
                </div>
            
        </li>
 <li>
            <a class="top-heading">Manage</a>
			<i class="caret"></i>            
            <div class="dropdown right-aligned">
                <div class="dd-inner">
                    <div class="column">
                        <h3 style="font-size:medium">View Feedback Status</h3>
                            <asp:Label ID="ViewNotCompletedLbl" runat="server" CssClass="label"></asp:Label>
                            <asp:LinkButton ID="ViewNotCompletedLink" runat="server" CssClass="hereLink" OnClick="ViewNotCompletedLink_Click">View Non-Submission List</asp:LinkButton>
                            <asp:Label ID="ViewIndividualAllLbl" runat="server" CssClass="label"></asp:Label>
                            <asp:LinkButton ID="ViewIndividualAllLink" runat="server" CssClass="hereLink" OnClick="ViewIndividualAllLink_Click">View All/Individual Staff Report</asp:LinkButton>
                            <asp:Label ID="ViewAppraisalChart" runat="server" CssClass="label"></asp:Label>
                            <asp:LinkButton ID="ViewAppraisalChartLink" runat="server" CssClass="hereLink" OnClick="ViewAppraisalChartLink_Click">View Historical Chart</asp:LinkButton>
                            <asp:Label ID="ViewIndividualGraphLbl" runat="server" CssClass="label"></asp:Label>
                            <asp:LinkButton ID="ViewIndividualGraphLink" runat="server" CssClass="hereLink" OnClick="ViewIndividualGraph_Click">View Individual Staff's Graph</asp:LinkButton>  
                    </div>

                </div>
            </div>
    </ul>
</nav>


    <div align="center">


        <br />


        <br />


        <asp:Panel ID="DefaultPanel" align="left" CssClass="defaultPanel" runat="server"
            GroupingText="Peer Feedback">
            <div align="center">






                <table>
                    <tr>
                        <td>
                            <asp:Label ID="SubmitAppraisalLbl" runat="server" CssClass="label"></asp:Label>
                            <%--<asp:LinkButton ID="SubmitAppraisalLink" runat="server" CssClass="hereLink" OnClick="SubmitAppraisalLink_Click">here</asp:LinkButton>--%>
                            <%--<asp:HyperLink ID="SubmitLink" runat="server" CssClass="hereLink" OnClick="SubmitAppraisalLink_Click">here</asp:HyperLink>--%>
                            <asp:LinkButton ID="SubmitLink" runat="server" CssClass="hereLink" OnClick="SubmitAppraisalLink_Click">here</asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="ViewAppraisalLbl" runat="server" CssClass="label"></asp:Label>
                            <asp:LinkButton ID="ViewAppraisalLink" runat="server" CssClass="hereLink" OnClick="ViewAppraisalLink_Click">here</asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </div>
        </asp:Panel>

        <br />

        <%--<asp:Panel ID="ManageSystem" align="left" CssClass="defaultPanel" runat="server"
            GroupingText="Manage System">
            <div align="center">
                <table>
                    <tr>
                        
                        <td style="height: 22px">
                            <asp:Label ID="ManageUserLbl" runat="server" CssClass="label"></asp:Label>
                            <asp:LinkButton ID="ManageUserLink" runat="server" CssClass="hereLink" OnClick="ManageUserLink_Click">here</asp:LinkButton>
                        </td>
                        <tr>
                            <td>
                                <asp:Label ID="ExportImportStaffListLbl" runat="server" CssClass="label"></asp:Label>
                                <asp:LinkButton ID="ExportImportStaffLink" runat="server" CssClass="hereLink" OnClick="ExportImportStaffLink_Click">here</asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="SetOpenCloseLbl" runat="server" CssClass="label"></asp:Label>
                                <asp:LinkButton ID="SetOpenCloseLink" runat="server" CssClass="hereLink" OnClick="SetOpenCloseLink_Click">here</asp:LinkButton>
                            </td>
                        </tr>
                        
                    </tr>
                </table>
            </div>
        </asp:Panel>--%>


        <asp:Panel ID="ManageAppraisalSummary" align="left" CssClass="defaultPanel" runat="server"
            GroupingText="Manage Staff Feedback Report">
            <div align="center">
                <table>
                    <tr>
                        <td style="height: 22px">
                            <asp:Label ID="DeleteAllLbl" runat="server" CssClass="label"></asp:Label>
                            <asp:LinkButton ID="deleteAllLink" runat="server" CssClass="hereLink" OnClick="deleteAllLink_Click">here</asp:LinkButton>
                            <asp:ConfirmButtonExtender ID="deleteAllLink_ConfirmButtonExtender" runat="server"
                                ConfirmText="Are you sure to delete all evaluation records?" Enabled="True" TargetControlID="deleteAllLink">
                            </asp:ConfirmButtonExtender>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 22px">
                            <asp:Label ID="DeleteSingleUidLbl" runat="server" CssClass="label"></asp:Label>
                            <asp:DropDownList ID="ddlUid" runat="server" CssClass="otherstandard">
                            </asp:DropDownList>
                            <asp:LinkButton ID="deleteSingleLink" runat="server" CssClass="hereLink" OnClick="deleteSingleLink_Click">delete</asp:LinkButton>
                            <asp:ConfirmButtonExtender ID="deleteSingleLink_ConfirmButtonExtender" runat="server"
                                ConfirmText="Are you sure you want to delete all submitted evaluation records by this user?"
                                Enabled="True" TargetControlID="deleteSingleLink">
                            </asp:ConfirmButtonExtender>
                        </td>
                    </tr>
                </table>
            </div>
        </asp:Panel>
        <br />
    </div>
    &nbsp;&nbsp;&nbsp;
</asp:Content>