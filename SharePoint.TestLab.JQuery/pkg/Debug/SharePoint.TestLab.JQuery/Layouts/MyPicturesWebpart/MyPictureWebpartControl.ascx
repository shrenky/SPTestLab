<%@ Assembly Name="SharePoint.TestLab.JQuery, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e3c293e493f96b02" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MyPictureWebpartControl.ascx.cs" Inherits="SharePoint.TestLab.JQuery.CONTROLTEMPLATES.MyPicturesWebpart.MyPictureWebpartControl" %>
<script type="text/javascript" src="/_wpresources/SharePoint.TestLab.JQuery/1.0.0.0__e3c293e493f96b02/Scripts/jquery-1.11.0.min.js"></script>
<script type="text/javascript" src="/_wpresources/SharePoint.TestLab.JQuery/1.0.0.0__e3c293e493f96b02/Scripts/MyPictures.js"></script>
<link href="/_wpresources/SharePoint.TestLab.JQuery/1.0.0.0__e3c293e493f96b02/Styles/MyNews.css" rel="stylesheet" type="text/css" />
<img alt="My Pictures" id="MyPicture" src="" />
<div id="News"></div>

<div id="NewsStory-Template" class="NewsStory" style="display: none">
    <h2 class="NewsStoryHeader"></h2>
    <div class="NewsArticle" style="display:none"/>
</div>
