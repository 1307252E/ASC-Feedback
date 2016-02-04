<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ChangePassword.aspx.cs" Inherits="_360_Staff_Survey_Web.ChangePassword" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Panel ID="ChangePasswPanel" runat="server" GroupingText="Change Password">
        <table style="width: 100%; font-family: Arial">
            <tr>
                <td style="width: 15px">&nbsp; </td>
                <td style="width: 155px">Staff ID: </td>
                <td>
                    <asp:Label ID="StfNumlbl" runat="server" CssClass="label" Style="font-size: medium" Text="Label"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width: 15px">&nbsp; </td>
                <td style="width: 155px">&nbsp; </td>
                <td>&nbsp; </td>
            </tr>
            <tr>
                <td style="width: 15px">&nbsp; </td>
                <td style="width: 155px">Current Password: </td>
                <td>
                    <asp:TextBox ID="CurrentPassw" runat="server" CssClass="otherstandard" TextMode="Password" Width="211px"></asp:TextBox>
                    
                    <asp:PasswordStrength ID="CurrentPassw_PasswordStrength" TargetControlID="CurrentPassw"
                        StrengthIndicatorType="Text" PrefixText="Strength:" PreferredPasswordLength="8"
                        MinimumNumericCharacters="1" MinimumSymbolCharacters="1" TextStrengthDescriptions="Poor;Average;Good;Excellent"
                        TextStrengthDescriptionStyles="PoorStrength;AverageStrength;GoodStrength;ExcellentStrength" runat="server">
                    </asp:PasswordStrength>
                    
                    <br />
                </td>
            </tr>
            <tr>
                <td style="width: 15px">&nbsp; </td>
                <td style="width: 155px">&nbsp; </td>
                <td>
                    <asp:RequiredFieldValidator ID="CurrentPasswordValidator" runat="server" ControlToValidate="CurrentPassw" Display="Dynamic" ErrorMessage="Required" ForeColor="Red"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="CurrentPassw" Display="Dynamic" ErrorMessage="Character Limit (8-15)" ForeColor="Red" ValidationExpression="^[a-zA-Z0-9'@&?$\s]{8,15}"></asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td style="width: 15px">&nbsp; </td>
                <td style="width: 155px">New Password: </td>
                <td>
                    <asp:TextBox ID="NewPassw" runat="server" CssClass="otherstandard" TextMode="Password" Width="211px"></asp:TextBox>
                    
                    <asp:PasswordStrength ID="NewPassw_PasswordStrength" runat="server" TargetControlID="NewPassw"
                        StrengthIndicatorType="Text" PrefixText="Strength:" PreferredPasswordLength="8"
                        MinimumNumericCharacters="1" MinimumSymbolCharacters="1" TextStrengthDescriptions="Poor;Average;Good;Excellent"
                        TextStrengthDescriptionStyles="PoorStrength;AverageStrength;GoodStrength;ExcellentStrength">
                    </asp:PasswordStrength>
                    
                    <br />
                </td>
            </tr>
            <tr>
                <td style="width: 15px">&nbsp; </td>
                <td style="width: 155px">&nbsp; </td>
                <td>
                    <asp:RequiredFieldValidator ID="NewPasswordValidator" runat="server" ControlToValidate="NewPassw" Display="Dynamic" ErrorMessage="Required" ForeColor="Red"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="NewPassw" Display="Dynamic" ErrorMessage="Character Limit (8-15)" ForeColor="Red" ValidationExpression="^[a-zA-Z0-9'@&?$\s]{8,15}"></asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td style="width: 15px">&nbsp; </td>
                <td style="width: 155px">Confirm Password: </td>
                <td>
                    <asp:TextBox ID="ConfirmPasw" runat="server" CssClass="otherstandard" TextMode="Password" Width="211px"></asp:TextBox>
                     
                    <asp:PasswordStrength ID="ConfirmPasw_PasswordStrength" TargetControlID="ConfirmPasw"
                        runat="server" StrengthIndicatorType="Text" PrefixText="Strength:" PreferredPasswordLength="8"
                        MinimumNumericCharacters="1" MinimumSymbolCharacters="1" TextStrengthDescriptions="Poor;Average;Good;Excellent"
                        TextStrengthDescriptionStyles="PoorStrength;AverageStrength;GoodStrength;ExcellentStrength">
                    </asp:PasswordStrength>
                     
                    <br />
                </td>
            </tr>
            <tr>
                <td style="width: 15px">&nbsp; </td>
                <td style="width: 155px">&nbsp; </td>
                <td>
                    <asp:RequiredFieldValidator ID="ConfirmValidator" runat="server" ControlToValidate="ConfirmPasw" Display="Dynamic" ErrorMessage="Required" ForeColor="Red"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="ConfirmPasw" Display="Dynamic" ErrorMessage="Character Limit (8-15)" ForeColor="Red" ValidationExpression="^[a-zA-Z0-9'@&?$\s]{8,15}"></asp:RegularExpressionValidator>
                </td>
                
            </tr>
            <tr>
                <td style="width: 15px">&nbsp; </td>
                <td style="width: 155px">&nbsp; </td>
                <td>
                    <asp:Button ID="SubmitBtn" runat="server" CssClass="standardButtons" OnClick="Button1_Click" Text="Submit" />
                    <asp:ConfirmButtonExtender ID="SubmitBtn_ConfirmButtonExtender" runat="server" ConfirmText="Are you sure you want to change your password?"
                        Enabled="True" TargetControlID="SubmitBtn">
                    </asp:ConfirmButtonExtender>
                    
                    &nbsp;<asp:Button ID="CancelBtn" runat="server" CausesValidation="False" CssClass="standardButtons" OnClick="CancelBtn_Click" Text="Cancel" />
                </td>
            </tr>
            <tr>
                <td style="width: 15px">&nbsp; </td>
                <td style="width: 155px">&nbsp; </td>
                <td>&nbsp; </td>
            </tr>
            <tr>
                <td style="width: 15px; height: 23px;">&nbsp; </td>
                <td style="width: 155px; height: 23px;"></td>
                <td style="height: 23px">
                    <asp:CompareValidator ID="CompareTwoPassw" runat="server" ControlToCompare="NewPassw" ControlToValidate="ConfirmPasw" Display="Dynamic" EnableClientScript="False" ErrorMessage="Password does not match with new password" ForeColor="Red"></asp:CompareValidator>
                    <asp:Label ID="Messagelbl" runat="server" Font-Bold="False" ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center" style="width: 15px">&nbsp; </td>
                <td align="center" colspan="2">
                    &nbsp;</td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
