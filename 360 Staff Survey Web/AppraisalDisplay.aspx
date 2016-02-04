<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="AppraisalDisplay.aspx.cs" Inherits="_360_Staff_Survey_Web.AppraisalDisplay" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="tiny_mce/tiny_mce_dev.js" type="text/javascript"></script>
    <script type="text/javascript">
        tinyMCE.init({
            // General options
            mode: "textareas",
            theme: "advanced",
            plugins: "pagebreak,style,layer,table,save,advhr,advimage,advlink,emotions,iespell,inlinepopups,insertdatetime,preview,media,searchreplace,print,contextmenu,paste,directionality,fullscreen,noneditable,visualchars,nonbreaking,xhtmlxtras,template,wordcount,advlist,autosave",
            //plugins: "pagebreak,style,layer,table,save,advhr,advimage,advlink,emotions,iespell,inlinepopups,insertdatetime,preview,media,searchreplace,contextmenu,paste,directionality,fullscreen,noneditable,template,wordcount,advlist,autosave",
            theme_advanced_buttons1: "bold,italic,underline,strikethrough,forecolor,|,justifyleft,justifycenter,justifyright,justifyfull,formatselect,fontselect,fontsizeselect",
            //theme_advanced_buttons2: "search,replace,|,bullist,numlist,|,outdent,indent,blockquote,|,undo,redo,|,removeformat,newdocument,|,sub,sup,|,ltr,rtl,|,fullscreen,emotions,|,preview,print",
            theme_advanced_buttons2: "tablecontrols,|,search,replace,|,bullist,numlist,|,outdent,indent,blockquote,|,undo,redo,|,removeformat,newdocument,|,sub,sup,|,ltr,rtl,|,emotions",
            //theme_advanced_buttons3: "link,unlink,",
            theme_advanced_buttons3: null,
            theme_advanced_toolbar_location: "top",
            theme_advanced_toolbar_align: "left",
            theme_advanced_statusbar_location: "bottom",
            theme_advanced_resizing: false,

            // Skin options
            skin: "o2k7",
            skin_variant: "silver",

            // Example content CSS (should be your site CSS)
            content_css: "css/content.css",

            // Drop lists for link/image/media/template dialogs
            template_external_list_url: "lists/template_list.js",
            external_link_list_url: "lists/link_list.js",
            external_image_list_url: "lists/image_list.js",
            media_external_list_url: "lists/media_list.js",

            // Style formats
            style_formats: [
			{ title: 'Bold text', inline: 'b' },
			{ title: 'Red text', inline: 'span', styles: { color: '#ff0000'} },
			{ title: 'Red header', block: 'h1', styles: { color: '#ff0000'} },
			{ title: 'Example 1', inline: 'span', classes: 'example1' },
			{ title: 'Example 2', inline: 'span', classes: 'example2' },
			{ title: 'Table styles' },
			{ title: 'Table row 1', selector: 'tr', classes: 'tablerow1' }
		],

            template_external_list_url: "js/template_list.js",
            external_link_list_url: "js/link_list.js",
            external_image_list_url: "js/image_list.js",
            media_external_list_url: "js/media_list.js",

            // Replace values for the template plugin
            template_replace_values: {
                username: "Some User",
                staffid: "991234"
            }
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <%--    <form method="post" action="somepage">
    <textarea id="tbxDetails" style="width: 100%; height: 500px;" runat="server"></textarea>
    <asp:Button ID="btnSave" runat="server" Text="Save" Width="50px" OnClick="btnSave_Click" />
    </form>--%>
    <div>
        <asp:TextBox ID="tbxDetails" runat="server" TextMode="MultiLine" style="width: 100%; height: 500px;"></asp:TextBox>
        <asp:Button ID="btnSave" runat="server" Text="Save" Width="50px" OnClick="btnSave_Click" />
    </div>
    <br />
    <br />
</asp:Content>
