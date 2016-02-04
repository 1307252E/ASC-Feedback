<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ForgetPassword.aspx.cs"
    Inherits="_360_Staff_Survey_Web.ForgetPassword" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<link href="Styles/Site.css" rel="stylesheet" type="text/css" />
<script type="text/javascript">

    document.attachEvent("onkeydown", my_onkeydown_handler);
    function my_onkeydown_handler() {
        switch (event.keyCode) {

            case 116: // 'F5' 
                event.returnValue = false;
                event.keyCode = 0;
                window.status = "F5 disabled";
                break;
        }
    }
    function check(e) {
        var keynum
        var keychar
        var numcheck
        // For Internet Explorer   
        if (window.event) {
            keynum = e.keyCode
        }
        // For Netscape/Firefox/Opera   
        else if (e.which) {
            keynum = e.which
        }
        keychar = String.fromCharCode(keynum)
        //List of special characters you want to restrict   
        if (keychar == "`" || keychar == "~" || keychar == "#" || keychar == "$" || keychar == "]"
 || keychar == "%" || keychar == "^" || keychar == "*" || keychar == "["
 || keychar == "{" || keychar == "}" || keychar == ";"
 || keychar == "+" || keychar == "=" || keychar == "<" || keychar == ">"
 || keychar == "|" || keychar == "-") {
            return false;
        }
        else {
            return true;
        }
    }

    function DisableControlKey(e) {
        // Message to display
        var message = "Right click option disabled.";
        // Condition to check mouse right click
        if (e.button == 2) {
            alert(message);
            return false;
        }
    }

</script>
<head runat="server">
    <title>Password Recovery</title>
</head>
<body runat="server" style="background-image: url(Image/ASCBackgroundGreen.png)">
    <form id="form1" runat="server">
    <div align="center">
        <div align="center">
            <table>
                <tr>
                    <td>
                        <asp:Image ID="Image1" runat="server" ImageUrl="~/Image/tplogo.png" />
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label6" runat="server" Style="font-size: large; font-weight: 700;"
                            Text="School of Applied Science" CssClass="label"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <br />
        <div align="center">
            <asp:Panel ID="ForgetPasswordPanel" align="left" Width="570px" GroupingText="Forget Password"
                runat="server">
                <br />
                <div align="left" style="margin-bottom: 0px">
                    <table>
                        <tr>
                            <td>
                                <span class="style14">
                                    <asp:Label ID="Label3" runat="server" Style="font-size: medium" Text="Please enter staff email address (@tp.edu.sg), an email will be send to this email account to reset password."
                                        CssClass="label"></asp:Label>
                                </span>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <table align="center">
                        <tr>
                            <td>
                                <asp:Label ID="Label8" runat="server" Text="Email Address:" CssClass="label"></asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="emailTbx" runat="server" Width="284px" onkeypress="return check(event)"
                                    onMouseDown="return DisableControlKey(event)" CssClass="otherstandard"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td colspan="2">
                                <asp:RequiredFieldValidator ID="RequireValidatorName" runat="server" ControlToValidate="emailTbx"
                                    Display="Dynamic" ErrorMessage="Required" ForeColor="Red"></asp:RequiredFieldValidator>
                                <asp:Label ID="messagelbl" runat="server" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td align="right" colspan="2">
                                &nbsp;&nbsp;
                                <asp:Button ID="retrievebtn" runat="server" CssClass="standardButtons" OnClick="retrievebtn_Click"
                                    Text="Submit" />
                                <asp:ConfirmButtonExtender ID="retrievebtn_ConfirmButtonExtender" runat="server"
                                    ConfirmText="Are you sure you want to reset your password?" Enabled="True" TargetControlID="retrievebtn">
                                </asp:ConfirmButtonExtender>
                                &nbsp;<asp:Button ID="backbtn" runat="server" CssClass="standardButtons" OnClick="Button2_Click"
                                    Text="Back" CausesValidation="False" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
                                </asp:ToolkitScriptManager>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </div>
            </asp:Panel>
            <br />
        </div>
    </form>
</body>
</html>
