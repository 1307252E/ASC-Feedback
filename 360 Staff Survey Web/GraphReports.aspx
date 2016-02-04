<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GraphReports.aspx.cs" Inherits="_360_Staff_Survey_Web.WebForm2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ImageButton ID="exportToExcel" runat="server" ImageUrl="~/Image/EXCEL.png" OnClick="exportToExcel_Click" />
    <br />
    <table class="graph-dropdown">
        <tr>
            <td>
                <asp:Label ID="Label1" runat="server" Text="Choose Question(s):"></asp:Label></td>
            <td>
                <asp:DropDownList ID="ddlQuestions" runat="server"></asp:DropDownList></td>
        </tr>
        <tr><td></td></tr>
        <tr>
            <td>
                <asp:Label ID="Label2" runat="server" Text="Choose Section(s):"></asp:Label></td>
            <td>
                <asp:DropDownList ID="ddlSections" runat="server"></asp:DropDownList></td>
        </tr>
        <tr><td></td></tr>
        <tr>
            <td>
                <asp:Label ID="Label3" runat="server" Text="Choose Period:"></asp:Label></td>
            <td>
                <asp:DropDownList ID="ddlPeriod" runat="server"></asp:DropDownList></td>
        </tr>
    </table>
    <br />
    <asp:CheckBox ID="CheckBox1" runat="server" Checked="True" Text="View by Line Graph" />
    &nbsp;
    <asp:CheckBox ID="CheckBox2" runat="server" Text="View by Bar Graph" />
    <br />
    <br />
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:Button ID="btnView" runat="server" OnClick="btnView_Click" Text="View Graph" />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <asp:Chart ID="Chart1" runat="server" Visible="False">
        <Series>
            <asp:Series ChartType="Line" Name="Series1">
            </asp:Series>
        </Series>
        <ChartAreas>
            <asp:ChartArea Name="ChartArea1">
            </asp:ChartArea>
        </ChartAreas>
        <Titles>
            <asp:Title Font="Microsoft Sans Serif, 9.75pt" Name="Title1" Text="Line Graph of each staff's average rating">
            </asp:Title>
        </Titles>
    </asp:Chart>
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />

    <br />
    <asp:Chart ID="Chart2" runat="server" Visible="False">
        <Series>
            <asp:Series Name="Series1">
            </asp:Series>
        </Series>
        <ChartAreas>
            <asp:ChartArea Name="ChartArea1">
            </asp:ChartArea>
        </ChartAreas>
        <Titles>
            <asp:Title Font="Microsoft Sans Serif, 9.75pt" Name="Title1" Text="Bar Graph of each staff's average rating">
            </asp:Title>
        </Titles>
    </asp:Chart>
</asp:Content>
