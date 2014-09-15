<%@ Assembly Name="SharePoint.TestLab.JQuery, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e3c293e493f96b02" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TaskViewer.aspx.cs" Inherits="SharePoint.TestLab.JQuery.Layouts.TaskViewer.TaskViewer" DynamicMasterPageFile="~masterurl/default.master" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <script src="/_layouts/15/TaskViewer/jquery-1.11.0.min.js" type="text/javascript"></script>
    <script src="/_layouts/15/TaskViewer/jquery-ui.js" type="text/javascript"></script>
    <script src="/_layouts/15/TaskViewer/TaskViewer.js" type="text/javascript"></script>
    <link href="/_layouts/15/TaskViewer/style.css" type="text/css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <div id="newTaskDiv">
        <input id="newTaskTitle" class="newTask" type="text" maxlength="40"/>
        <input id="addTaskButton" class="newTask" type="button" value="Add New Task"/>
    </div>
    <div id="taskContent">
        <div id="notStartedTasksColumn" class="taskColumn">
            <h3>Not Started</h3>
            <div class="taskDiv" id="notStartedTasksDiv">
                <ul />
            </div>
        </div>

        <div id="inProgressTasksColumn" class="taskColumn">
            <h3>In Progress</h3>
            <div class="taskDiv" id="inProgressTasksDiv">
                <ul />
            </div>
        </div>

        <div id="completedTasksColumn" class="taskColumn">
            <h3>Completed</h3>
            <div class="taskDiv" id="completedTasksDiv">
                <ul />
            </div>
        </div>
    </div>

    <ul class="hiddenTemplate">
        <li id="TemplateListItem" class="task">
            <div class="deleteDiv">
                <span class="deleteButton delete">x</span>
                <span class="confirmDelete" style="display:none">Are you sure?<span class="yesDeleteButton delete">Yes</span><span class="noDeleteButton delete">No</span></span>
            </div>
            <div class="taskDetailDiv">
                <p class="taskTitle">Title</p>
                <p class="taskDescription">Description</p>
            </div>
            <div class="editDiv" style="display:none">
                <p>
                    <input class="editTaskTitle edit" type="text" maxlength="40"/>
                </p>
                <p>
                    <textarea class="editTaskDescription edit" rows="4" cols="30"></textarea>
                </p>
                <span class="editTaskSaveButton editButton">Save</span>
                <span class="editTaskCancelButton editButton">Cancel</span>
            </div>
            <div>
                <img class="moveLeftButton moveButton" src="/_layouts/15/images/ARRLEFTA.gif" alt="Move Left" />
                <img class="moveRightButton moveButton" src="/_layouts/15/images/ARRRIGHTA.gif" alt="Move Right" />
            </div>
        </li>
    </ul>
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    View Tasks
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server">
    View Tasks
</asp:Content>
