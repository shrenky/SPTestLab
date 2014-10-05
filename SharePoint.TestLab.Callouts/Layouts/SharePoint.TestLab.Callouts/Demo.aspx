<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Demo.aspx.cs" Inherits="SharePoint.TestLab.Callouts.Layouts.SharePoint.TestLab.Callouts.Demo" DynamicMasterPageFile="~masterurl/default.master" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <script type="text/javascript">
        SP.SOD.executeFunc('callout.js', 'CreateMyCallOut', function () {
            var calloutElement = document.getElementById('CalloutDiv');

            var calloutOptions = new CalloutOptions();
            calloutOptions.ID = 'MyCallout';
            calloutOptions.launchPoint = calloutElement;
            calloutOptions.content = 'This is Callout Test Description';
            calloutOptions.title = 'This is Callout Test Title';
            var callout = CalloutManager.createNew(calloutOptions);

            var calloutAction = new CalloutActionOptions();
            calloutAction.text = 'Click here';
            calloutAction.onClickCallback = function(event, action)
            {
                alert("This is Callout event1");
            };

            var action = new CalloutAction(calloutAction);
            callout.addAction(action);
        
        });
    </script>
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <div id="CalloutDiv" style="width:100px">Callout  <span id="ms-pageDescriptionImage">&nbsp;</span></div>
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
Application Page
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server" >
My Application Page
</asp:Content>
