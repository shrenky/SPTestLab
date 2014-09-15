<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ClientOM.aspx.cs" Inherits="SharePoint.TestLab.ClientOM.Layouts.SharePoint.TestLab.ClientOM.ClientOM" DynamicMasterPageFile="~masterurl/default.master" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <SharePoint:ScriptLink ID="ScriptLink11" Language="javascript" Localizable="false"
        Name="/_layouts/15/SharePoint.TestLab.ClientOM/jquery-1.10.2.min.js"
        LoadAfterUI="true" runat="server" />
    <script type="text/javascript">
        function getcount2() {

            var ctx = SP.ClientContext.get_current();;//get_current();
            var website = ctx.get_web();
            var list = website.get_lists().getByTitle("My Task");//对应列表名称
            var camlQuery = new SP.CamlQuery();
            var q = "<View><Query><Where><Eq><FieldRef Name='Status'/><Value Type='Text'>In Progress</Value></Eq></Where><OrderBy><FieldRef Name='Modified' Ascending='FALSE'/></OrderBy><GroupBy/></Query></View>";
            camlQuery.set_viewXml(q);
            this.listitems = list.getItems(camlQuery);
            ctx.load(this.listitems);
            ctx.executeQueryAsync(Function.createDelegate(this, this.onUpdate2),
                   Function.createDelegate(this, this.onFail2));


        }
        function onUpdate2(sender, args) {

            var count = 0;
            count = this.listitems.get_count();
            var listItemEnumerator = this.listitems.getEnumerator();
            var listItemInfo = '';
            //  debugger;
            if (count > 0) {

                alert("OK");
                alert(count);
            }
            else {

                alert("0");

            }
            while (listItemEnumerator.moveNext()) {
                var message = "Title=" + listItemEnumerator.get_current().get_item("Title");

                alert(message);
            };

        }

        function onFail2(sender, args) {
            alert('failed on reading list:' + args.get_message());
        }

        $(function () {//$(document).ready(function () {
            SP.SOD.executeFunc('sp.js', 'SP.ClientContext', getcount2);
        });
    </script>
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">

</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
Application Page
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server" >
My Application Page
</asp:Content>
