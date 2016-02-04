<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoginPage.aspx.cs" Inherits="_360_Staff_Survey_Web.LoginPage" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
    <%----%>
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
    function limitText(limitField, limitNum) {
        if (limitField.value.length > limitNum) {
            limitField.value = limitField.value.substring(0, limitNum);
            alert("Your password has exceeded the limit.");
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
    <link href="Styles/LoginSite.css" rel="stylesheet" type="text/css" />
    <title>Online 360° Leadership System</title>
    <style type="text/css">
        .style40
        {
            width: 152px;
        }
        .label
        {
            font-family: Arial;
            font-size: medium;
        }
        .LoginButton
        {
            width: 90px;
            font-family: Arial;
            margin-bottom: 0px;
        }
        .style41
        {
            width: 213px;
        }
        .logo
        {
            margin: auto;
            padding-left: 20px;
        }
    </style>
    <script type="text/javascript">
        window.history.forward(1); 
    </script>
</head>
<body style="background-image: url(Image/ASCBackgroundGreen.png);">
    <form id="form1" runat="server">
    <div align="center">
        <div align="center">
            <table>
                <tr>
                    <td>
                        <asp:Image ID="Image1" runat="server" CssClass="logo" ImageUrl="~/Image/tplogo.png" />
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
        <table>
            <tr>
                <td>
                    <table>
                        <tr>
                            <td>
                                <asp:Panel ID="Panel1" runat="server" BackColor="#FFFFCC" Style="font-family: Arial;"
                                    BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" Height="190px">
                                    <table style="width: 100%; height: 200px;">
                                        <tr>
                                            <td>
                                                <table style="width: 100%;">
                                                    <tr>
                                                        <td class="style40">
                                                            <asp:Label ID="adminstafflbl" runat="server" CssClass="label" Style="font-weight: 700"
                                                                Text="User ID:"></asp:Label>
                                                        </td>
                                                        <td align="left" class="style41">
                                                            <asp:TextBox ID="staffnumber" runat="server" Width="165px" onkeypress="return check(event)"
                                                                onMouseDown="return DisableControlKey(event)" CssClass="otherstandard"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr align="left">
                                                        <td class="style40">
                                                            &nbsp;
                                                        </td>
                                                        <td class="style41">
                                                            <asp:RequiredFieldValidator ID="adminstaffvalidator0" runat="server" ControlToValidate="staffnumber"
                                                                Display="Dynamic" ErrorMessage="Required" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="passwordlbl0" runat="server" Style="font-weight: 700" Text="Password:"
                                                                CssClass="label"></asp:Label>
                                                        </td>
                                                        <td align="left" class="style41">
                                                            <asp:TextBox ID="password" runat="server" TextMode="Password" Width="165px" Wrap="False"
                                                                onKeyDown="limitText(this,15);" onKeyUp="limitText(this,15);" CssClass="otherstandard"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr align="left">
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td class="style41">
                                                            <asp:RequiredFieldValidator ID="passwordvalidator0" runat="server" ControlToValidate="password"
                                                                Display="Dynamic" ErrorMessage="Required" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td class="style41">
                                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                            <asp:Button ID="LoginBtn" runat="server" CssClass="LoginButton" OnClick="login_button_Click"
                                                                Text="Login" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td align="left" class="style41">
                                                            <asp:Label ID="messagelbl" runat="server" ForeColor="Red" Style="text-align: left"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="left" style="font-family: Arial; font-size: medium">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td align="left" style="font-family: Arial; font-size: medium">
                    <b>For Staff:</b><br />
                    Please use your email id for login.
                </td>
            </tr>
        </table>
        <br />
    </div>
    </form>
</body>
</html>
