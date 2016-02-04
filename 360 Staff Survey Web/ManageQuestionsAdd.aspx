<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ManageQuestionsAdd.aspx.cs" Inherits="_360_Staff_Survey_Web.ManageQuestionsAdd" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div style="text-align: left">
        &nbsp; &nbsp;&nbsp;&nbsp;<asp:Label ID="bracket1" runat="server" Text="["></asp:Label>
        &nbsp;<asp:LinkButton ID="BackBtnLink" runat="server" CssClass="hereLink" OnClick="BackBtnLink_Click">Back</asp:LinkButton>
        &nbsp;<asp:Label ID="bracket2" runat="server" Text="]"></asp:Label>
        <br />
    </div>
    <div style="text-align: center">
        <asp:Label ID="AddQuestionLbl" runat="server" CssClass="label" Text="Add Peer Feedback Question"
            Font-Bold="True"></asp:Label>
    </div>
    <br />
    <br />
    <div style="text-align: center">
        <asp:GridView ID="AddQuestionGrid" runat="server" AutoGenerateColumns="False" CssClass="otherstandard"
            GridLines="None" HorizontalAlign="Center" ShowFooter="True" OnRowCreated="AddQuestionGrid_RowCreated">
            <Columns>
                <asp:BoundField DataField="RowNumber" HeaderText="Entry">
                    <HeaderStyle HorizontalAlign="Left" Width="30px" />
                    <ItemStyle Width="30px" VerticalAlign="Top" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="Questions">
                    <ItemTemplate>
                        <asp:TextBox ID="QuestionTbx" runat="server" CssClass="textboxesquestions" onkeypress="return check(event)"
                            onMouseDown="return DisableControlKey(event)" onpaste="return false" TextMode="MultiLine"
                            Height="80px" Width="400px"></asp:TextBox>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Rate 1 Comment">
                    <ItemTemplate>
                        <asp:TextBox ID="RateOneTbx" runat="server" CssClass="textboxesquestions" onkeypress="return check(event)"
                            onMouseDown="return DisableControlKey(event)" onpaste="return false" TextMode="MultiLine"
                            Height="80px" Width="320px"></asp:TextBox>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Rate 7 Comment">
                    <ItemTemplate>
                        <asp:TextBox ID="RateSevenTbx" runat="server" CssClass="textboxesquestions" onkeypress="return check(event)"
                            onMouseDown="return DisableControlKey(event)" onpaste="return false" TextMode="MultiLine"
                            Height="80px" Width="320px"></asp:TextBox>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Include Qn">
                    <ItemTemplate>
                        <asp:CheckBox ID="QnInclude" runat="server" Checked="true" />
                    </ItemTemplate>
                    <HeaderStyle CssClass="checkbox" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText=" ">
                    <ItemTemplate>
                        <asp:LinkButton ID="RemoveBtn" runat="server" OnClick="RemoveBtnLink_Click">Remove</asp:LinkButton>
                    </ItemTemplate>
                    <ControlStyle Width="100px" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <div style="text-align: right; width: 80%">
        <td>
            <asp:Button ID="AddBtn" runat="server" CssClass="buttonquestions" Text="Add New Row"
                OnClick="AddBtn_Click" />
        </td>
        <td>
            &nbsp;<asp:Button ID="AddQuestionBtn" runat="server" CssClass="buttonquestions" Text="Add Question"
                OnClick="AddQuestionBtn_Click" />
        </td>
    </div>
    <div style="width: 100%; height: 100%;">
        <br />
        &nbsp;&nbsp;
        <asp:Label ID="IncludeLbl" runat="server" CssClass="label"></asp:Label>
        <br />
        <br />
    </div>
</asp:Content>
