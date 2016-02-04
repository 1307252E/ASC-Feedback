<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ManageUser.aspx.cs" Inherits="_360_Staff_Survey_Web.ManageUser" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div style="text-align: center; height: 21px;">
        <asp:Label ID="ManageSystemLbl" runat="server" CssClass="label" Text="Manage System User"
            Font-Bold="True"></asp:Label>
        <br />
        <br />
    </div>
    <table width="100%">
        <tr>
            <td>
            </td>
            <td>
                <%--<asp:LinkButton ID="viewAllBtn" runat="server" OnClick="viewAllBtn_Click" CssClass="label"></asp:LinkButton>--%>
                <%--<asp:LinkButton ID="lbViewAll" runat="server" onclick="lbViewAll_Click" CssClass="label"></asp:LinkButton>--%>
                <asp:Button ID="lbViewAll" runat="server" OnClick="lbViewAll_Click" Text="View All" CssClass="label"/>
            </td>
            <td align="right">
                <%--<asp:LinkButton ID="AddUser" runat="server" OnClick="AddUser_Click" CssClass="label">Add new system user</asp:LinkButton>--%>
                <%--<asp:LinkButton ID="lbAddUser" runat="server" OnClick="AddUser_Click" CssClass="label">Add new user</asp:LinkButton>--%>
                <asp:Button ID="lbAddUser" runat="server" Text="Add new user" OnClick="AddUser_Click" CssClass="label"/>
            </td>
        </tr>
    </table>
    <br />
    <div align="center" style="width: 100%;">
        <table style="padding-left: 12px; width: 100%;">
            <tr>
                <td>
                    <asp:GridView ID="ManageStaffGrid" Width="100%" runat="server" CellSpacing="2" BackColor="Black"
                        BorderWidth="1px" AutoGenerateColumns="False" AllowPaging="True" OnPageIndexChanging="ManageStaffGrid_PageIndexChanging"
                        Style="margin-right: 0px" CssClass="otherstandard">
                        <AlternatingRowStyle />
                        <Columns>
                            <asp:BoundField DataField="Name" HeaderText="Name" >
                            <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Designation" HeaderText="Designation" >
                            <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Section">
                                <ItemTemplate>
                                    <asp:Label ID="lblSection" runat="server" Text='<%# Bind("Section") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Function">
                                <ItemTemplate>
                                    <asp:Label ID="lblFunction" runat="server" Text='<%# Bind("Function") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="Uid" HeaderText="User ID" >
                            <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Role">
                                <ItemTemplate>
                                    <asp:Label ID="lblDropdown" runat="server" Text='<%# Bind("Role") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbEdit" runat="server" CausesValidation="false" CommandName=""
                                        Text="Edit" OnClick="BtnEdit_Click"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbDelete" runat="server" CausesValidation="false" CommandName=""
                                        Text="Delete" OnClick="ConfirmDelete_Click"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <RowStyle BackColor="White" />
                        <HeaderStyle BackColor="Black" ForeColor="White" />
                        <PagerSettings Position="TopAndBottom" />
                        <PagerStyle HorizontalAlign="Center" BackColor="White" />
                    </asp:GridView>
                </td>
            </tr>
        </table>
        <asp:Button ID="InvisBtn" runat="server" Style="display: none" />
        <asp:ModalPopupExtender ID="ModalPopupExtender" BackgroundCssClass="ModalPopupBG"
            runat="server" CancelControlID="CancelBtn" PopupControlID="UpdatePanel" TargetControlID="InvisBtn">
        </asp:ModalPopupExtender>
        <div align="left">
            <asp:Panel ID="UpdatePanel" runat="server" Style="background-image: url(./Image/bg1.jpg);
                display: none" BackColor="White" GroupingText="Update User">
                <table>
                    <tr>
                        <td style="width: 10px">
                        </td>
                        <td>
                            <asp:Label ID="lblLegend" CssClass="label" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                </table>
                <br />
                <div>
                    <table>
                        <tr>
                            <td style="width: 12px">
                            </td>
                            <td style="width: 76px">
                                <asp:Label ID="lblName" CssClass="label" runat="server" Text="Name:"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblStaffName" CssClass="label" runat="server"></asp:Label>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 12px">
                                &nbsp;
                            </td>
                            <td style="width: 76px">
                                &nbsp;
                            </td>
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
                            <td style="width: 76px">
                                <asp:Label ID="lblDesignationId" CssClass="label" runat="server" Text="Designation:"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblStaffDesignation" CssClass="label" runat="server"></asp:Label>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td style="width: 76px">
                                &nbsp;
                            </td>
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
                            <td valign="top" style="width: 70px">
                                <asp:Label ID="lblSection" CssClass="label" runat="server" Text="Section:"></asp:Label>
                            </td>
                            <td>
                                <asp:ListBox ID="listSection" runat="server" SelectionMode="Multiple" CssClass="standardManage"
                                    onMouseDown="GetCurrentListValues(this);" onchange="FillListValues(this);"></asp:ListBox>
                            </td>
                            <td valign="top">
                                <asp:RequiredFieldValidator ID="RequiredValidatorSection" runat="server" ControlToValidate="listSection"
                                    Display="Dynamic" ErrorMessage="Required" EnableClientScript="false" ForeColor="Red"></asp:RequiredFieldValidator>
                                <asp:Label ID="lblValidatorSection" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td style="width: 76px">
                                &nbsp;
                            </td>
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
                            <td style="width: 76px">
                                <asp:Label ID="lblFunction" CssClass="label" runat="server" Text="Function:"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlFunction" runat="server" CssClass="standardManage">
                                </asp:DropDownList>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td style="width: 76px">
                                &nbsp;
                            </td>
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
                            <td style="width: 76px">
                                <asp:Label ID="lblStaffUid" CssClass="label" runat="server" Text="User ID:"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblUid" CssClass="label" runat="server"></asp:Label>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td style="width: 76px">
                                &nbsp;
                            </td>
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
                            <td style="width: 76px">
                                <asp:Label ID="lblRole" CssClass="label" runat="server" Text="Role:"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlRole" runat="server" CssClass="standardManage">
                                </asp:DropDownList>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td style="width: 76px">
                                &nbsp;
                            </td>
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
                            <td style="width: 70px">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </div>
                <div align="right">
                    <asp:Button ID="UpdateBtn" runat="server" Text="Update" OnClick="UpdateBtn_Click" />&nbsp;
                    <asp:Button ID="CancelBtn" runat="server" Text="Cancel" />
                    &nbsp;</div>
            </asp:Panel>
        </div>
        <asp:Button ID="InvisBtn2" runat="server" Style="display: none" />
       <asp:ModalPopupExtender ID="ModalPopupExtender2" BackgroundCssClass="ModalPopupBG"
            runat="server" CancelControlID="CancelBtn2" PopupControlID="UpdatePanel2" TargetControlID="InvisBtn2">
        </asp:ModalPopupExtender>
        <div align="left">
            <asp:Panel ID="UpdatePanel2" runat="server" Style="background-image: url(./Image/bg1.jpg);
                display: none" BackColor="White" GroupingText="Delete User">
                <table>
                    <tr>
                        <td style="width: 10px">
                        </td>
                        <td>
                            <asp:Label ID="LegendLbl" CssClass="label" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                </table>
                <br />
                <div>
                    <table>
                        <tr>
                            <td style="width: 12px">
                            </td>
                            <td style="width: 76px">
                                <asp:Label ID="Name" CssClass="label" runat="server" Text="Name:"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="NameLbl" CssClass="label" runat="server"></asp:Label>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 12px">
                                &nbsp;
                            </td>
                            <td style="width: 76px">
                                &nbsp;
                            </td>
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
                            <td style="width: 76px">
                                <asp:Label ID="Designation" CssClass="label" runat="server" Text="Designation:"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="DesignationLbl" CssClass="label" runat="server"></asp:Label>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td style="width: 76px">
                                &nbsp;
                            </td>
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
                            <td valign="top" style="width: 70px">
                                <asp:Label ID="Section" CssClass="label" runat="server" Text="Section:"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="SectionLbl" CssClass="label" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td style="width: 76px">
                                &nbsp;
                            </td>
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
                            <td style="width: 76px">
                                <asp:Label ID="Function" CssClass="label" runat="server" Text="Function:"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="FunctionLbl" CssClass="label" runat="server"></asp:Label>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td style="width: 76px">
                                &nbsp;
                            </td>
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
                            <td style="width: 76px">
                                <asp:Label ID="UserID" CssClass="label" runat="server" Text="User ID:"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="UserIDLbl" CssClass="label" runat="server"></asp:Label>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td style="width: 76px">
                                &nbsp;
                            </td>
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
                            <td style="width: 76px">
                                <asp:Label ID="Role" CssClass="label" runat="server" Text="Role:"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="RoleLbl" CssClass="label" runat="server" Text="Role:"></asp:Label>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td style="width: 76px">
                                &nbsp;
                            </td>
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
                            <td style="width: 70px">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </div>
                <div align="right">
                    <asp:Button ID="DeleteBtn" runat="server" Text="Delete" OnClientClick="return confirm('Are you sure you want to delete this user?');"
                        OnClick="DeleteBtn_Click" />&nbsp;
                    <asp:Button ID="CancelBtn2" runat="server" Text="Cancel" />
                    &nbsp;</div>
            </asp:Panel>
        </div>
    </div>
    <br />
    <br />
    <br />
</asp:Content>
