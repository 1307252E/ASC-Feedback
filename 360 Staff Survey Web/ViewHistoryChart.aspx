<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ViewHistoryChart.aspx.cs" Inherits="_360_Staff_Survey_Web.ViewHistoryChart" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <table align="center" width="100%">
        <tr>
            <td>&nbsp;
                <asp:ImageButton ID="SearchSwapBtn" runat="server" AlternateText="Switch search filter"
                    Height="30px" ImageUrl="~/Image/swap.png" OnClick="SearchSwapBtn_Click" ToolTip="Swap" />
                <asp:Panel ID="SearchPanelSectionViaFunction" runat="server" GroupingText="View Chart By Section Group">
                    <div align="center">
                        <table>
                            <tr align="left">
                                <td>
                                    <asp:Label ID="lblSelectSection" runat="server" CssClass="label"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlSelectSection" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlSelectSection_SelectedIndexChanged"
                                        CssClass="otherstandardDropdown">
                                    </asp:DropDownList>
                                </td>
                                <td>&nbsp;
                                </td>
                            </tr>
                            <tr align="left">
                                <td>
                                    <asp:Label ID="lblSelectQuestion" runat="server" CssClass="label"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlSelectQuestion" runat="server" CssClass="otherstandardDropdown">
                                        <asp:ListItem></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>&nbsp;
                                </td>
                            </tr>
                            <tr align="left">
                                <td>
                                    <asp:Label ID="lblFilterByFunction" runat="server" CssClass="label"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlFilterFunction" runat="server" CssClass="otherstandardDropdown">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Button ID="AddBtn" runat="server" CssClass="standardButtons" Height="25px" Text="Add"
                                        OnClick="AddBtn_Click" />
                                </td>
                            </tr>
                            <tr align="left">
                                <td>
                                    <asp:Label ID="lblSelectedFunction" runat="server" CssClass="label"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="SelectTbxFunction" runat="server" placeholder="Click on add to insert item" CssClass="otherstandard" ReadOnly="True"
                                        Width="205px"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Button ID="SearchBtn" runat="server" CssClass="standardButtons" Text="Display"
                                        OnClick="SearchBtn_Click" />
                                </td>
                                <td>
                                    <asp:Button ID="ClearBtn" runat="server" CssClass="standardButtons" Text="Clear"
                                        OnClick="ClearBtn_Click" Visible="false" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </asp:Panel>
                <asp:Panel ID="SearchPanelFunctionViaSection" runat="server" GroupingText="View Chart By Function Group">
                    <div align="center">
                        <table>
                            <tr align="left">
                                <td>
                                    <asp:Label ID="lblSelectFunction" runat="server" CssClass="label"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlSelectFunction" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlSelectFunction_SelectedIndexChanged"
                                        CssClass="otherstandardDropdown">
                                    </asp:DropDownList>
                                </td>
                                <td>&nbsp;
                                </td>
                            </tr>
                            <tr align="left">
                                <td>
                                    <asp:Label ID="lblFilterByQuestion" runat="server" CssClass="label"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlFilterQuestion" runat="server" CssClass="otherstandardDropdown">
                                    </asp:DropDownList>
                                </td>
                                <td>&nbsp;
                                </td>
                            </tr>
                            <tr align="left">
                                <td>
                                    <asp:Label ID="lblFilterBySection" runat="server" CssClass="label"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlFilterBySection" runat="server" CssClass="otherstandardDropdown">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Button ID="AddSectionBtnSection" runat="server" CssClass="standardButtons" Height="25px"
                                        Text="Add" OnClick="AddSectionBtnSection_Click" />
                                </td>
                            </tr>
                            <tr align="left">
                                <td>
                                    <asp:Label ID="lblSelectedSection" runat="server" CssClass="label"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="SelectTbxSection" runat="server" placeholder="Click on add to insert item" CssClass="otherstandard" ReadOnly="True"
                                        Width="205px"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Button ID="SearchBtnSection" runat="server" CssClass="standardButtons" Text="Display"
                                        OnClick="SearchBtnSection_Click" />
                                </td>
                                <td>
                                    <asp:Button ID="ClearBtnSection" runat="server" CssClass="standardButtons" Text="Clear"
                                        OnClick="ClearBtn_Click" Visible="false" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <br />
    <asp:MultiView ID="MultiView1" runat="server">
        <asp:View ID="View2" runat="server">
            <br />
            <div align="center">
                <table>
                    <tr>
                        <td>
                            <asp:ImageButton ID="ExcelDl" runat="server" ImageUrl="~/Image/EXCEL.png" OnClick="ExcelDl_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label ID="ChartTitle" Text="Peer Evaluation Chart" Font-Names="Microsoft Sans Serif" runat="server" Font-Underline="True" Font-Bold="True"></asp:Label>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <br />
                            <asp:Label ID="SelectedSect" runat="server" Font-Names="Microsoft Sans Serif" Visible="False"></asp:Label>
                            <br />
                            <asp:Label ID="SelectedQues" runat="server" Font-Names="Microsoft Sans Serif" Visible="False"></asp:Label>
                            <br />
                            <asp:Label ID="SelectedFunc" runat="server" Font-Names="Microsoft Sans Serif" Visible="False"></asp:Label>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>

                            <asp:Chart ID="Chart1" runat="server" AntiAliasing="Graphics" BackColor="Transparent">
                                <Series>
                                    <asp:Series Name="Series1" ChartType="Line">
                                    </asp:Series>
                                </Series>
                                <ChartAreas>
                                    <asp:ChartArea Name="ChartArea1">
                                    </asp:ChartArea>
                                </ChartAreas>
                            </asp:Chart>

                        </td>
                    </tr>
                </table>
            </div>
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lbllegendHistory" runat="server" CssClass="label"></asp:Label>
                    </td>
                </tr>
            </table>
            <br />
        </asp:View>
        <asp:View ID="View1" runat="server">
            <br />
            <div>
                <table width="100%">
                    <tr>
                        <td align="center">
                            <asp:Label ID="lbDisplay" runat="server" CssClass="label"></asp:Label></td>
                    </tr>
                </table>
            </div>
            <br />
        </asp:View>
    </asp:MultiView>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .watermarkcss {
            font-size: small;
            color: gray;
        }
    </style>
    <!--[if lt IE 9]>
	<script>
		$(function() {
		
			var el;
			
			$("select.otherstandardDropdown")
				.each(function() {
					el = $(this);
					el.data("origWidth", el.outerWidth()) // IE 8 will take padding on selects
				})
			  .mouseenter(function(){
			    $(this).css("width", "auto");
			  })
			  .bind("blur change", function(){
			  	el = $(this);
			    el.css("width", el.data("origWidth"));
			  });
		
		});
	</script>
	<![endif]-->
</asp:Content>
