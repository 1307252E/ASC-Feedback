<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ImportExportStaffList.aspx.cs" Inherits="_360_Staff_Survey_Web.ImportExportStaffList" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Panel ID="ImportStaffList" runat="server" GroupingText="Import staff data">
        <br />
        <table>
            <tr>
                <td>
                    &nbsp;&nbsp;&nbsp;<asp:Label ID="ImportStaffLbl" runat="server" Font-Bold="False"
                        Text="Import staff data:" CssClass="label"></asp:Label>
                </td>
                <td>
                    <asp:FileUpload ID="StaffDataFileUpload" runat="server" Height="17pt" />
                </td>
                <td>
                    &nbsp;
                    <asp:Button ID="ImportStaffBtn" runat="server" Text="Upload" OnClick="ImportStaffBtn_Click"
                        CssClass="standardButtons" />
                   <asp:ConfirmButtonExtender ID="ImportStaffBtn_ConfirmButtonExtender" runat="server"
                        ConfirmText="Import staff data could affect changes to other related tables, do you really wish to continue?"
                        Enabled="True" TargetControlID="ImportStaffBtn">
                    </asp:ConfirmButtonExtender>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    <asp:Label ID="StaffErrorMsgLbl" runat="server" Font-Bold="False" CssClass="label"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    &nbsp;&nbsp;
                    <asp:Label ID="lblNote" runat="server" CssClass="label"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Panel ID="ExportPanel" runat="server" GroupingText="Export staff data">
        <br />
        <table style="padding-left: 12px; width: 100%;">
            <tr>
                <td>
                    <asp:ImageButton ID="ExcelBtn" runat="server" AlternateText="Export to csv" ImageUrl="~/Image/CSV.png" OnClick="ExcelBtn_Click" />
                    <asp:ImageButton ID="WordBtn" runat="server" AlternateText="Export to word" ImageUrl="~/Image/WORD.png" OnClick="WordBtn_Click" />
                    <asp:ImageButton ID="PdfBtn" runat="server" AlternateText="Export to pdf" ImageUrl="~/Image/PDF.png" OnClick="PdfBtn_Click" />
                </td>
                <td align="right">
                    <%--<asp:LinkButton ID="viewAllBtn" runat="server" OnClick="viewAllBtn_Click" CssClass="label"></asp:LinkButton>--%>
                    <asp:Button ID="viewAllBtn" runat="server" OnClick="viewAllBtn_Click" CssClass="label"
                        Text="view all" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:GridView ID="ExportStaffGrid" runat="server" CellSpacing="2" OnPageIndexChanging="ExportStaffGrid_PageIndexChanging"
                        Width="100%" BackColor="Black" CssClass="otherstandard">
                        <RowStyle BackColor="White" />
                        <PagerSettings Position="TopAndBottom" />
                        <PagerStyle HorizontalAlign="Center" BackColor="White" />
                        <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
</asp:Content>
