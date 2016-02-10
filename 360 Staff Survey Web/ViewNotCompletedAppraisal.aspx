<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ViewNotCompletedAppraisal.aspx.cs" Inherits="_360_Staff_Survey_Web.ViewCompletedAppraisal" %>

<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .cpHeader
        {
            color: white;
            background-color: gray;
            font: bold 11px auto "Trebuchet MS" , Verdana;
            font-size: 12px;
            cursor: pointer;
            width: 450px;
            height: 18px;
            padding: 4px;
        }
        .cpBody
        {
            background-color: #DCE4F9;
            font: normal 11px auto Verdana, Arial;
            border: 1px gray;
            width: 450px;
            padding: 4px;
            padding-top: 7px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div style="padding-left: 10px">
        <asp:ImageButton ID="exportWord" runat="server" AlternateText="Export to word" ImageUrl="~/Image/WORD.png" OnClick="exportWord_Click" /></div>
    <asp:Label ID="lblStaffSummary" runat="server" CssClass="label"></asp:Label>
    <asp:Button ID="emailNotice" runat="server" BorderColor="#FFFFCC" CssClass="label" Font-Names="Arial" Font-Size="Large" OnClick="emailNotice_Click" Text="Send survey reminder e-mail"/>
</asp:Content>
