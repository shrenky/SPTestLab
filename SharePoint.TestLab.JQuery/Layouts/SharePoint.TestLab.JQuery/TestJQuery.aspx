<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestJQuery.aspx.cs" Inherits="SharePoint.TestLab.JQuery.Layouts.SharePoint.TestLab.JQuery.TestJQuery" DynamicMasterPageFile="~masterurl/default.master" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <script type="text/javascript" src="/_layouts/15/SharePoint.TestLab.JQuery/js/require.js"></script>
    <script type="text/javascript">
        require.config({
            paths: {
                'jquery': 'js/jquery-1.11.0.min'
            }
        });
        require(['jquery'], function (myjquery) {
            myjquery(document).ready(function () {
                myjquery('#myId p:not(.myClass)').css('background-color', '#BED6A3');
                myjquery('ul#people li:first').css('background-color', '#BED6A3');
                myjquery('input:button').css('background-color', '#a52a2a');
                var newPeople = ["sam", "Tony", "Joe", "Eva"];
                myjquery.each(newPeople, function(index, value) {
                    myjquery('<li>' + value +'</li>').appendTo('ul#people');
                });
                myjquery('p').bind('click', {i: "1", j:"2"}, function(event) {
                    var txt = myjquery(this).text();
                    alert(txt);
                    alert(event.data.i);
                    alert(event.data.j);
                });
            });
        });
    </script>
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <div id="myId">
        <p class="myClass">class is set to myClass</p>
        <p>Just a plain paragraph</p>
        <p class="myClass">class is also set to myClass</p>
        <h1>My Title</h1>
        <p class="myClass1">class is also set to myClass</p>
        <ul id="people">
            <li>Phill</li>
            <li>Pip</li>
            <li>Les</li>
            <li>Denise</li>
            <li>Martin</li>
            <li>Helen</li>
            <li>Tony</li>
        </ul>
        <input type="button" value="Input Button" />
        <input type="checkbox" />
        <input type="file" />
        <input type="hidden" />
        <input type="image" />
        <input type="password" />
        <input type="radio" />
        <input type="reset" />
        <input type="submit" />
        <input type="text" />
        <select>
            <option>Option</option>
        </select>
        <textarea cols="10" rows="2">Text area</textarea>
        <button>Button</button>
    </div>
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    Application Page
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server">
    My Application Page
</asp:Content>
