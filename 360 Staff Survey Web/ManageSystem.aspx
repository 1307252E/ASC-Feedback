<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ManageSystem.aspx.cs" Inherits="_360_Staff_Survey_Web.ManageSystem" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Panel ID="ManageSystemEndPanel" runat="server" GroupingText="Manage System Previous Dates">
        <br />
        <table style="padding-left: 15px; margin-bottom: 0px;">
            <tr>
                <td style="width: 165px">
                    <asp:Label ID="ppStartDate" runat="server" Text="Previous Start Date:" CssClass="label"></asp:Label>
                    <br />
                </td>
                <td>
                    <asp:Label ID="startlbl" runat="server" ForeColor="#009999" Style="font-weight: 400;
                        font-size: medium" CssClass="label" Font-Bold="False"></asp:Label>
                    <br />
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 165px">
                    <asp:Label ID="ppEndDate" runat="server" Text="Previous Closure Date:" CssClass="label"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="endlbl" runat="server" ForeColor="#009999" Style="font-weight: 400;
                        font-size: medium" CssClass="label"></asp:Label>
                </td>
                <td>
                    &nbsp;
                    <asp:LinkButton ID="ExtensionLink" runat="server" OnClick="ExtensionLink_Click" CssClass="hereLink">extension</asp:LinkButton>
                    <asp:ModalPopupExtender ID="ModalPopupExtender" runat="server" BackgroundCssClass="ModalPopupBG"
                        CancelControlID="CancelChangeBtn" Enabled="True" PopupControlID="ChangeDatePanel"
                        TargetControlID="InvisBtn">
                    </asp:ModalPopupExtender>
                    <asp:Button ID="InvisBtn" runat="server" Style="display: none" />
                    
                </td>
            </tr>
            <tr>
                <td class="standardManage" style="width: 165px">
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Panel ID="ManageSystemPanel" runat="server" GroupingText="Manage System New Dates">
        <br />
        <table style="padding-left: 15px; margin-bottom: 0px;">
            <tr>
                <td style="width: 165px; height: 10px">
                    <asp:Label ID="lblStart" runat="server" Text="New Start Date: " CssClass="label"></asp:Label>
                    <br />
                </td>
                <td style="height: 10px">
                    <asp:TextBox ID="SelectStartTbx" runat="server" AutoPostBack="True" OnTextChanged="SelectEndDateTbx_TextChanged"
                        Width="155px" CssClass="otherstandard"></asp:TextBox>
                  
                    <asp:CalendarExtender ID="SelectStartTbx_CalendarExtender" runat="server" Format="d MMMM, yyyy"
                TargetControlID="SelectStartTbx" />
                  
                </td>
                <td style="height: 10px">
                    &nbsp;</td>
            </tr>
            <tr>
                <td style="width: 165px; height: 10px">
                    <asp:Label ID="lblNewEnd" runat="server" Text="New Closure Date: " CssClass="label"></asp:Label>
                </td>
                <td style="height: 10px">
                    <asp:TextBox ID="SelectEndDateTbx" runat="server" AutoPostBack="True" OnTextChanged="SelectEndDateTbx_TextChanged"
                        Width="155px" CssClass="otherstandard"></asp:TextBox>
                    <asp:CalendarExtender ID="SelectEndDateTbx_CalendarExtender" runat="server" Format="d MMMM, yyyy"
                         TargetControlID="SelectEndDateTbx" />
                </td>
                <td style="height: 10px">
                    &nbsp;
                    <asp:Button ID="Button1" runat="server" Style="display: none" />
                    <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" BackgroundCssClass="ModalPopupBG"
                        CancelControlID="CancelChangeBtn" Enabled="True" PopupControlID="ChangeDatePanel"
                        TargetControlID="InvisBtn">
                    </asp:ModalPopupExtender>
                </td>
            </tr>
        </table>
        <br />
        <table style="padding-left: 15px; margin-bottom: 0px;">
            <tr>
                <td style="width: 165px">
                    <br />
                </td>
                <td>
                    
                    <asp:Button ID="Confirmbtn" runat="server" CssClass="standardButtons" OnClick="Confirmbtn_Click"
                        Text="Confirm" Width="75px" />
                   <asp:ConfirmButtonExtender ID="Confirmbtn_ConfirmButtonExtender" runat="server" ConfirmText="Are you sure you want to set the dates?"
                        Enabled="True" TargetControlID="Confirmbtn">
                    </asp:ConfirmButtonExtender>
                    &nbsp;
                    <asp:Button ID="ClearBtn" runat="server" CssClass="standardButtons" OnClick="ClearBtn_Click"
                        Text="Clear" Width="76px" />
                    <br />
                </td>
            </tr>
            <tr>
                <td style="width: 165px">
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 165px">
                    &nbsp;
                </td>
                <td>
                    <asp:Label ID="Messaglbl" runat="server" CssClass="label" ForeColor="Red" Style="font-size: medium"></asp:Label>
                </td>
            </tr>
        </table>
        <br />
    </asp:Panel>
    <!--------------------extend closure panel-------------------->
    <asp:Panel ID="ChangeDatePanel" runat="server" BackColor="White" GroupingText="Manage System New Closure Dates"
        Style="background-image: url(./Image/bg1.jpg); display: none">
        <br />
        <table style="padding-left: 15px; margin-bottom: 0px;">
            <tr>
                <td style="width: 180px">
                    <asp:Label ID="lblchangePreviousEnd" CssClass="label" runat="server" Text="Previous Closure Date:"></asp:Label>
                    <br />
                </td>
                <td>
                    <asp:Label ID="startlblchange" CssClass="label" runat="server" ForeColor="#33CC33"
                        Style="font-weight: 400; font-size: medium"></asp:Label>
                    <br />
                </td>
            </tr>
            <tr>
                <td style="width: 180px">
                    <asp:Label ID="lblchangeNewEnd" CssClass="label" runat="server" Text="New Closure Date: "></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="closureEndTbx" CssClass="otherstandard" runat="server" Width="155px"></asp:TextBox>
                    &nbsp;<asp:ImageButton ID="closureChangeEnd" runat="Server" AlternateText="Click to show calendar"
                        ImageUrl="~/Image/calendar-schedulehs.jpg" />
                    <asp:CalendarExtender ID="CalendarExtender5" runat="server" Format="d MMMM, yyyy"
                        PopupButtonID="closureChangeEnd" TargetControlID="closureEndTbx" />
                  
                    <br />
                </td>
            </tr>
            <tr>
                <td style="width: 180px">
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 180px">
                    &nbsp;
                </td>
                <td>
                    <asp:Button ID="ConfirmChangeBtn" runat="server" OnClick="ConfirmChangeBtn_Click"
                        Text="Confirm" Width="75px" />
                    &nbsp;
                    <asp:Button ID="CancelChangeBtn" runat="server" Text="Cancel" Width="76px" />
                </td>
            </tr>
            <tr>
                <td style="width: 180px">
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
        </table>
    </asp:Panel>
    </asp:Content>
