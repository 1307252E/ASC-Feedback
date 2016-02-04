<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ManageQuestions.aspx.cs" Inherits="_360_Staff_Survey_Web.ManageQuestions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div style="text-align: center">
        <asp:Label ID="ManageQuestionLbl" runat="server" CssClass="label" Text="Manage Peer Feedback Question"
            Font-Bold="True"></asp:Label>
        <br />
        <br />
    </div>
    <div style="text-align: right">
        <asp:LinkButton ID="AddQuestion" runat="server" OnClick="AddQuestion_Click" CssClass="label">Add new question</asp:LinkButton>&nbsp;</div>
    <div style="text-align: center">
        <asp:GridView ID="AppraisalQuestionGrid" runat="server" AutoGenerateColumns="False"
            CssClass="otherstandard" GridLines="None" HorizontalAlign="Center" ShowFooter="True">
            <Columns>
                <asp:TemplateField HeaderText="Qn">
                    <ItemTemplate>
                        <asp:Label ID="QuestionIdLbl" runat="server" Text='<%# Bind("QuestionId")%>'></asp:Label>
                    </ItemTemplate>
                    <ControlStyle Width="30px" />
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" Width="10px" VerticalAlign="Top" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Questions">
                    <ItemTemplate>
                        <asp:TextBox ID="QuestionsTbx" runat="server" CssClass="textboxesquestions" onkeypress="return check(event)"
                            onMouseDown="return DisableControlKey(event)" onpaste="return false" Text='<%# Bind("Question")%>'
                            TextMode="MultiLine" Width="400px" Height="80px"></asp:TextBox>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Comment for Rating 1">
                    <ItemTemplate>
                        <asp:TextBox ID="rateOneTbx" runat="server" CssClass="textboxesquestions" onkeypress="return check(event)"
                            onMouseDown="return DisableControlKey(event)" onpaste="return false" Text='<%# Bind("RateOne")%>'
                            TextMode="MultiLine" Width="320px" Height="80px"></asp:TextBox>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Comment for Rating 7">
                    <ItemTemplate>
                        <asp:TextBox ID="RateSevenTbx" runat="server" CssClass="textboxesquestions" onkeypress="return check(event)"
                            onMouseDown="return DisableControlKey(event)" onpaste="return false" Text='<%# Bind("RateSeven")%>'
                            TextMode="MultiLine" Width="320px" Height="80px"></asp:TextBox>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>



                <asp:TemplateField HeaderText="Include Qn">
                    <ItemTemplate>
                        <asp:CheckBox ID="QnInclude" runat="server" Checked='<%# Bind("QnInclude")%>' />
                    </ItemTemplate>
                    <HeaderStyle CssClass="checkbox" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="DeleteBtn" runat="server" OnClientClick="return confirm('Are you sure you want to delete this question?');"
                            OnClick="DeleteBtn_Click">Delete</asp:LinkButton>
                    </ItemTemplate>
                    <ControlStyle Width="100px" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <div style="text-align: right; width: 80%">
        <asp:Button ID="Save" runat="server" CssClass="buttonquestions" Text="Update" OnClick="Save_Click" />
    </div>
    <div style="width: 100%; height: 100%;">
        <br />
        &nbsp;&nbsp;
        <asp:Label ID="IncludeLbl" runat="server" CssClass="label"></asp:Label>
        <br />
        <br />
    </div>
</asp:Content>
