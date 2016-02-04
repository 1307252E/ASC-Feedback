<%@ Page Title="View Appraisal" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ViewAppraisal.aspx.cs" Inherits="_360_Staff_Survey_Web.ViewAppraisal" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Panel ID="SearchPanel" runat="server" GroupingText="Search Feedback Report">
        <div align="center">
            <table>
                <tr align="left">
                    <td>
                        <asp:Label ID="lblFilterBySection" runat="server" CssClass="label"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlFilterSection" runat="server" CssClass="otherstandardDropdown">
                        </asp:DropDownList>
                    </td>
                    <td>
                        &nbsp;
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
                        <asp:Button ID="SearchBySectionFunction" runat="server" OnClick="SearchBySectionFunction_Click"
                            Text="Search" />
                    </td>
                </tr>
                <tr align="left">
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr align="left">
                    <td>
                        <asp:Label ID="lblSearchIndividual" runat="server" CssClass="label"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="SearchTbx" runat="server" CssClass="otherstandard" onkeypress="return check(event)"
                            onMouseDown="return DisableControlKey(event)" Width="205px"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Button ID="SearchIndividualBtn" runat="server" Text="Search" OnClick="SearchIndividualBtn_Click" />
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
    <asp:MultiView ID="MultiView1" runat="server">
        <asp:View ID="View1" runat="server">
            <table style="padding-left: 10px">
                <tr>
                    <td>
                        <asp:Button ID="ViewOverallBtn" runat="server" CssClass="standardManage" OnClick="ViewOverallBtn_Click" />
                        <asp:Button ID="InvisBtn" runat="server" Style="display: none" />
                       <asp:ModalPopupExtender ID="ModalPopupExtender" runat="server" BackgroundCssClass="ModalPopupBG"
                            CancelControlID="CancelBtn" Enabled="True" PopupControlID="ViewOverAllGradePanel"
                            TargetControlID="InvisBtn"></asp:ModalPopupExtender>
                        <br />
                        <br />
                        <asp:ImageButton ID="exportWord" runat="server" ImageUrl="~/Image/WORD.png" OnClick="exportWord_Click" AlternateText="Export to word" />
                        <asp:ImageButton ID="exportExcel" runat="server" AlternateText="Export to excel" ImageUrl="~/Image/EXCEL.png" OnClick="exportExcel_Click" />
                      
                        
                         <link type="text/css" rel="stylesheet" href="Styles For Gridview/style.css" />
                        
                         <asp:GridView ID="ViewAllHistory" runat="server" 
                             GridLines="None" AutoGenerateColumns="false"
            AllowPaging="True"
            CssClass="mGrid"
            PagerStyle-CssClass="pgr"
            AlternatingRowStyle-CssClass="alt">
            <RowStyle HorizontalAlign="center" />
            <AlternatingRowStyle HorizontalAlign="Center" />

                            <Columns>
                                <asp:TemplateField><ItemTemplate><asp:LinkButton ID="ViewBtn" runat="server" OnClick="ViewBtn_Click">View</asp:LinkButton></ItemTemplate><ItemStyle HorizontalAlign="Center" Width="45px" /></asp:TemplateField>
                                <asp:BoundField DataField="UserID" HeaderText="User ID"><ItemStyle HorizontalAlign="Center" Width="65px" /></asp:BoundField>
                                <asp:BoundField DataField="Name" HeaderText="Name"><ItemStyle Width="280px" /></asp:BoundField>
                                <asp:BoundField DataField="Section" HeaderText="Section"><ItemStyle HorizontalAlign="Center" Width="65px" /></asp:BoundField>
                                <asp:BoundField DataField="Function" HeaderText="Function"><ItemStyle HorizontalAlign="Center" Width="75px" /></asp:BoundField>
                                <asp:TemplateField HeaderText="Grade"><ItemTemplate><asp:Panel ID="ScrollPanel" Style="width: 793px" runat="server" ScrollBars="Horizontal"><asp:GridView ID="GridView1" Style="position: static" runat="server" BackColor="Black"><FooterStyle BackColor="#CCCCCC" /><RowStyle BackColor="White" Wrap="true" /><SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" /><HeaderStyle BackColor="Black" Font-Bold="False" ForeColor="White" /></asp:GridView></asp:Panel></ItemTemplate></asp:TemplateField>
                            </Columns>
                            <FooterStyle BackColor="#CCCCCC" />
                            <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="Black" Font-Bold="False" ForeColor="White" />
                        </asp:GridView>
                    </td>
                </tr>
                <tr style="height: 12px">
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lbllegendHistory" runat="server" CssClass="label"></asp:Label>
                    </td>
                </tr>
            </table>
            <br />
        </asp:View>
        <asp:View ID="View2" runat="server">
            <div style="text-align: left">
                &nbsp; &nbsp;&nbsp;&nbsp;<asp:Label ID="bracket1" runat="server" Text="["></asp:Label>
                &nbsp;<asp:LinkButton ID="IndividualBack" runat="server" CssClass="hereLink" OnClick="IndividualBack_Click">Back</asp:LinkButton>
                &nbsp;<asp:Label ID="bracket2" runat="server" Text="]"></asp:Label>
            </div>
            <br />
            <table style="padding-left: 10px; width: 100%;">
                <tr>
                    <td style="font-size: medium;">
                        <b>View PDF (History chart type) : </b>
                    </td>
                    <td style="font-size: medium;">
                        <b>1. Choose Period (Months) : </b>
                    </td>
                    <td style="font-size: medium;">
                        <b>2. Choose question : </b>
                    </td>
                    <td style="font-size: medium;">
                        <b>3. Choose report output : </b>
                    </td>
                    <td style="font-size: medium;">
                        <b>4. Generate report (Below) :</b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table width='100%'>
                            <tr>
                                <td>
                                    <img alt="Line" src="Image/PDF.png" />
                                    <asp:LinkButton ID="historylineLink" runat="server" OnClick="historylineLink_Click"
                                        CssClass="label">(Line)</asp:LinkButton>
                                </td>
                                <td>
                                    <img alt="Bar" src="Image/PDF.png" id="bar" />
                                    <asp:LinkButton ID="historychartbarLink" runat="server" OnClick="historychartbarLink_Click"
                                        CssClass="label">(Bar)</asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlOverDates" runat="server" AutoPostBack="True" CssClass="otherstandardDropdown"
                            OnSelectedIndexChanged="ddlOverDates_SelectedIndexChanged" Width="210px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlFilterQuestion" runat="server" AutoPostBack="True" CssClass="otherstandardDropdown"
                            OnSelectedIndexChanged="ddlFilterQuestion_SelectedIndexChanged" Width="210px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlFilterOutPut" runat="server" AutoPostBack="True" CssClass="otherstandardDropdown"
                            OnSelectedIndexChanged="ddlFilterOutPut_SelectedIndexChanged" Width="210px">
                            <asp:ListItem Selected="True">Both grade and comment</asp:ListItem>
                            <asp:ListItem>Only comment</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:ImageButton ID="wordExportIndividual" runat="server" AlternateText="Export to word" ImageUrl="~/Image/WORD.png" OnClick="wordExportIndividual_Click" />
                    </td>
                </tr>
            </table>
            <br />
            <asp:Label ID="lbllegend" runat="server" CssClass="label"></asp:Label>
            <br />
            <div style="width: 100%; height: 1px; background-color: gray; float: left;">
            </div>
            <br />
            <asp:Label ID="lblYourStat" runat="server" CssClass="label"></asp:Label>
        </asp:View>
    </asp:MultiView>
    <br />
    <br />
    <asp:Panel ID="ViewOverAllGradePanel" runat="server" BackColor="White" GroupingText="Overall Average School's Grade Panel"
        Style="background-image: url(./Image/bg1.jpg); display: none">
        <br />
        <asp:GridView ID="ViewOverAllSchoolGrid" runat="server" BackColor="Black" AutoGenerateColumns="False"
            CssClass="otherstandard" OnSelectedIndexChanged="ddlFilterQuestion_SelectedIndexChanged">
            <RowStyle BackColor="White" />
            <Columns>
                <asp:BoundField DataField="Name" HeaderText="Name" />
                <asp:TemplateField HeaderText="Grade">
                    <ItemTemplate>
                        <asp:Panel ID="ScrollPanel" Style="width: 793px" runat="server" ScrollBars="Horizontal">
                            <asp:GridView ID="InsideGridOverAll" Style="position: static" runat="server" BackColor="Black">
                                <FooterStyle BackColor="#CCCCCC" />
                                <RowStyle BackColor="White" Wrap="true" />
                                <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                                <HeaderStyle BackColor="Black" Font-Bold="False" ForeColor="White" />
                            </asp:GridView>
                        </asp:Panel>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <FooterStyle BackColor="#CCCCCC" />
            <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="Black" Font-Bold="False" ForeColor="White" />
        </asp:GridView>
        <br />
        <table>
            <tr>
                <asp:Button ID="CancelBtn" CssClass="standardButtons" runat="server" Text="Cancel"
                    Width="76px" /></tr>
        </table>
    </asp:Panel>
    </asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="HeadContent">
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
