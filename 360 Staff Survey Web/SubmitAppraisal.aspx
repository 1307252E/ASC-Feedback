<%@ Page Title="Submit Appraisal" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="SubmitAppraisal.aspx.cs" Inherits="_360_Staff_Survey_Web.SubmitAppraisal" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .verticalLine
        {
            border-left: thick solid #ff0000;
        }
        .style2
        {
            height: 23px;
        }
        .style3
        {
            width: 12px;
            height: 23px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <asp:MultiView ID="mainView" runat="server">
            <asp:View ID="View1" runat="server">
                <table width="100%">
                    <tr>
                        <td style="width: 12px">
                        </td>
                        <td>
                            <%--<asp:Button ID="ViewBtn" runat="server" CssClass="standardManage" OnClick="ViewBtn_Click"
                                Font-Size="12pt" Height="25px" Text="here" Width="60px" />--%>
                            <asp:Label ID="lblLegend2" runat="server" CssClass="label"></asp:Label>
                            <asp:Button ID="InvisBtn" runat="server" Style="display: none" />
                             <asp:ModalPopupExtender ID="ModalPopupExtender" runat="server" BackgroundCssClass="ModalPopupBG"
                                CancelControlID="CancelBtn" Enabled="True" PopupControlID="ViewDescription" TargetControlID="InvisBtn">
                            </asp:ModalPopupExtender>
                            <asp:LinkButton ID="ViewBtn" runat="server" CssClass="label" 
                                OnClick="ViewBtn_Click">here</asp:LinkButton>
                            <asp:Label ID="lblLegend" runat="server" CssClass="label"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <br />
                            <div style="width: 100%; height: 1px; background-color: gray; float: left;">
                            </div>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td valign="top">

                            <asp:Label ID="lblQuestion" runat="server" Text="Label"></asp:Label>
                            <div style="height: 600px; width: 100%; overflow: scroll;">
                                <asp:GridView ID="SubmitAppraisalGrid" runat="server" AutoGenerateColumns="False"
                                    BorderWidth="0px" ShowHeader="False" Style="margin-right: 0px; width: 100%" PageSize="1"
                                    OnPageIndexChanging="SubmitAppraisalGrid_PageIndexChanging" AllowPaging="True"
                                    GridLines="None">
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="QuestionLbl" runat="server" Text='<%# Bind("Question")%>' Visible=false></asp:Label>
                                                <br />
                                                <asp:GridView ID="StaffAppraisalGrid" runat="server" GridLines="Horizontal" AutoGenerateColumns="False"
                                                    BorderWidth="0px" CellPadding="4" OnRowCommand="StaffAppraisalGrid_RowCommand"
                                                    OnRowDataBound="StaffAppraisalGrid_RowDataBound" Style="margin-right: 0px; width: 100%">
                                                    <Columns>
                                                        <asp:BoundField DataField="No" HeaderText="No.">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:HyperLinkField DataTextField="StaffName" HeaderText="Name" Target="_blank">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:HyperLinkField>
                                                        <asp:TemplateField HeaderText="Rate">
                                                            <HeaderTemplate>
                                                                <a href="javascript:var popup = window.open('RateInfo.aspx','Popup','width=800,height=500');">
                                                                    Rate</a>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:RadioButtonList ID="RadioList" runat="server" CellPadding="8" DataSource='<%# Bind("RadioList")%>'
                                                                    RepeatDirection="Horizontal">
                                                                </asp:RadioButtonList>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Remarks">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="RemarksTbx" runat="server" CssClass="remarkTbx" Text='<%# Bind("RemarkTbx")%>'
                                                                    TextMode="MultiLine" onKeyDown="limitText(this,500);" onKeyUp="limitText(this,500);"
                                                                    onkeypress="return check(event)" onMouseDown="return DisableControlKey(event)"
                                                                    Height="30px" Width="400px"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerSettings Position="TopAndBottom" />
                                    <PagerStyle HorizontalAlign="Center" />
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                    <tr style="height: 15px">
                        <td style="width: 12px">
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 12px; vertical-align: top">
                        </td>
                        <td>
                            <asp:Button ID="SaveLtrBtn" runat="server" OnClick="SaveLtrBtn_Click" Text="Save draft"
                                CssClass="buttonquestions" />
                            <asp:Button ID="NextBtn" runat="server" CssClass="standardButtons" OnClick="NextBtn_Click"
                                Text="Next" />
                            <br />
                        </td>
                    </tr>
                </table>
                <br />
            </asp:View>
            <asp:View ID="View2" runat="server">
                <div style="text-align: left">
                    &nbsp;&nbsp;&nbsp;<asp:Label ID="bracket1" runat="server" Text="["></asp:Label>
                    &nbsp;<asp:LinkButton ID="BackBtnLink" runat="server" CssClass="hereLink" OnClick="BackBtnLink_Click">Back</asp:LinkButton>
                    &nbsp;<asp:Label ID="bracket2" runat="server" Text="]"></asp:Label><br />
                    <br />
                    <table width="100%">
                        <tr>
                            <td class="style2">
                                &nbsp;
                            </td>
                            <td class="style2">
                                <asp:Label ID="InformationLbl" runat="server" CssClass="label"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 12px">
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                                <asp:ImageButton ID="wordExportIndividual" runat="server" 
                                    AlternateText="Export to word" CssClass="csvwordpdficon" 
                                    ImageUrl="~/Image/WORD.png" OnClick="wordExportIndividual_Click" />
                                &nbsp;(Export to Word)</td>
                        </tr>
                        <tr>
                            <td class="style3">
                                &nbsp;
                            </td>
                            <td class="style2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:Label ID="SummaryLbl" runat="server" CssClass="label"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:Button ID="SubmitBtn" runat="server" CssClass="standardButtons" OnClick="SubmitBtn_Click"
                                    OnClientClick="return confirm('You can only submit this appraisal once. Once you submit you can no longer make any changes. Are you sure you want to submit now?');"
                                    Text="Submit" />
                            </td>
                        </tr>
                    </table>
                </div>
                <br />
            </asp:View>
        </asp:MultiView>
    </div>
    <asp:Panel ID="ViewDescription" runat="server" BackColor="White" Style="background-image: url(./images/header_bg.gif);
        display: none">
        <table>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    <div id="d1" runat="server" style="border: none; padding-bottom: 10px;
                        width: auto; height: auto;">
                        <asp:Label ID="lblDetails" runat="server" Text="Label"></asp:Label>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <asp:Button ID="CancelBtn" CssClass="standardButtons" runat="server" Text="Start"
                        Width="76px" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    </asp:Content>
