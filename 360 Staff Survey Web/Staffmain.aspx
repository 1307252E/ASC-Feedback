<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="Staffmain.aspx.cs"
    Inherits="_360_Staff_Survey_Web.Staffmain" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <%--    <div align="right">
        <asp:Label ID="WelcomeLbl" runat="server" Text="Welcome: " CssClass="label"></asp:Label>
        <asp:Label ID="staffName" runat="server" ForeColor="#009933" CssClass="label"></asp:Label>
    </div>--%>
    <div align="center">
        <asp:Panel ID="DefaultPanel" align="left" CssClass="defaultPanel" runat="server"
            GroupingText="Peer Feedback">
            <div align="center">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="SubmitAppraisalLbl" runat="server" CssClass="label" Font-Names="Times New Roman" Font-Size="Large"></asp:Label>
                            <%--<asp:LinkButton ID="SubmitAppraisalLink" runat="server" CssClass="hereLink" OnClick="SubmitAppraisalLink_Click">here</asp:LinkButton>--%>
                            <%--<asp:HyperLink ID="SubmitLink" runat="server" CssClass="hereLink" OnClick="SubmitAppraisalLink_Click">here</asp:HyperLink>--%>
                            <asp:LinkButton ID="SubmitLink" runat="server" CssClass="hereLink" OnClick="SubmitAppraisalLink_Click">here</asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="ViewAppraisalLbl" runat="server" CssClass="label" Font-Names="Times New Roman" Font-Size="Large"></asp:Label>
                            <asp:LinkButton ID="ViewAppraisalLink" runat="server" CssClass="hereLink" OnClick="ViewAppraisalLink_Click">here</asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </div>
        </asp:Panel>
        <br />
        <asp:Panel ID="ManageAppraisalPanel" align="left" CssClass="defaultPanel" runat="server"
            GroupingText="View Feedback Status">
            <div align="center">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="ViewIndividualAllLbl" runat="server" CssClass="label" Font-Names="Times New Roman" Font-Size="Large"></asp:Label>
                            <asp:LinkButton ID="ViewIndividualAllLink" runat="server" CssClass="hereLink" OnClick="ViewIndividualAllLink_Click">here</asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="ViewAppraisalChart" runat="server" CssClass="label" Font-Names="Times New Roman" Font-Size="Large"></asp:Label>
                            <asp:LinkButton ID="ViewAppraisalChartLink" runat="server" CssClass="hereLink" OnClick="ViewAppraisalChartLink_Click">here</asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td>


                            <asp:Label ID="ViewGraphReportLbl" runat="server" Font-Names="Times New Roman" Font-Size="Large"></asp:Label>
                            <asp:LinkButton ID="ViewGraphReportLink" runat="server" CssClass="hereLink" Visible="False" OnClick="ViewGraphReportLink_Click">here</asp:LinkButton>


                        </td>

                    </tr>
                </table>
            </div>
        </asp:Panel>
        <br />
        <br />
    </div>
    &nbsp;&nbsp;&nbsp; 
</asp:Content>
