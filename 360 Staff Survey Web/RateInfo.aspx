<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RateInfo.aspx.cs" Inherits="_360_Staff_Survey_Web.RateInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
            height: 23px;
        }
        .CommentStyle
        {
            width: 15%;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:Label ID="lblRate" runat="server"></asp:Label>
    <div>
        <asp:Label ID="lblTableRate" runat="server"></asp:Label>
        <table width="100%">
            <tr>
                <td colspan="3" class="CommentStyle">
                    <asp:TextBox ID="tbxRateOne" runat="server" Width="248px" TextMode="MultiLine" 
                        Height="200px" ReadOnly="True"></asp:TextBox>
                </td>
                <td colspan="1">
                </td>
                <td colspan="3" class="CommentStyle">
                    <asp:TextBox ID="tbxRateSeven" runat="server" Width="248px" TextMode="MultiLine" 
                        Height="200px" ReadOnly="True"></asp:TextBox>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
