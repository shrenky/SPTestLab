$(document).ready(function () {
    //alert('$ is loaded!');
    //ExecuteOrDelayUntilScriptLoaded(GetTasks, "sp.js");
    SP.SOD.executeFunc('sp.js', 'SP.ClientContext', GetTasks);

    $('#addTaskButton').click(function () {
        var title = $('#newTaskTitle').val();
        if (title == '') {
            alert('Please enter a value.');
            $('#newTaskTitle').focus();
        } else {
            AddNewTask(title);
        }
    });

    $('#addTaskTitle').keyup(function (event) {
        switch (event.keyCode) {
            case 13:
                $('#addTaskButton').click();
                break;
            case 27:
                $(this).val('');
                break;
        }
    });

    $('#notStartedTasksDiv>ul').droppable({
        activeClass: "task-active",
        hoverClass: "task-hover",
        accept: function (d) {
            // Check if item is coming from valid state
            // if it is then accept to drop
            if (d.data('status') == "In Progress") {
                return true;
            }
        },
        drop: function (ev, ui) {
            // Get the element being dragged
            var taskItem = $(ui.draggable);
            // Update the item
            ChangeStatus(taskItem, direction.Left);
        }
    });
    // Add droppable to the 'In Progress' list
    // Will accept items which are not 'In Progress'
    $('#inProgressTasksDiv>ul').droppable({
        activeClass: "task-active",
        hoverClass: "task-hover",
        accept: function (d) {
            if (d.data('status') != "In Progress") {
                return true;
            }
        },
        drop: function (ev, ui) {
            var taskItem = $(ui.draggable);
            if (taskItem.data('status') == "Completed") {
                ChangeStatus(taskItem, direction.Left);
            } else {
                ChangeStatus(taskItem, direction.Right);
            }
        }
    });

    $('#completedTasksDiv>ul').droppable({
        activeClass: "task-active",
        hoverClass: "task-hover",
        accept: function (d) {
            if (d.data('status') == "In Progress") {
                return true;
            }
        },
        drop: function (ev, ui) {
            var taskItem = $(ui.draggable);
            ChangeStatus(taskItem, direction.Right);
        }
    });
});

var tasks;
var taskListName = "Task";

function GetTasks() {
    var context = new SP.ClientContext.get_current();
    var web = context.get_web();
    var list = web.get_lists().getByTitle(taskListName);
    tasks = list.getItems('');
    context.load(tasks, 'Include(ID,Title,Body,Status)');
    context.executeQueryAsync(OnGetTasksSucceeded, OnGetTasksFailed);
}

function OnGetTasksSucceeded() {
    $('.taskDiv>ul').empty();
    var taskCollection = tasks.getEnumerator();
    while (taskCollection.moveNext()) {
        var task = taskCollection.get_current();
        var taskList = GetTaskListElementByTaskStatus(task.get_item('Status'));
        if (taskList != null) {
            addTaskToList(taskList, task.get_item('ID'), task.get_item('Title'), task.get_item('Body'), task.get_item('Status'));
        }
    }
}

function OnGetTasksFailed(sender, args) {
    alert('Request failed. ' + args.get_message() + '\n' + args.get_stackTrace());
}

function GetTaskListElementByTaskStatus(status) {
    var taskList;
    switch (status) {
        case 'Not Started':
            taskList = $('#notStartedTasksDiv>ul');
            break;
        case 'In Progress':
            taskList = $('#inProgressTasksDiv>ul');
            break;
        case 'Completed':
            taskList = $('#completedTasksDiv>ul');
            break;
        default:
            taskList = null;
    }
    return taskList;
}

function addTaskToList(list, id, title, body, status) {
    var itemTemplate = $('#TemplateListItem').clone();
    itemTemplate.attr('id', null);
    itemTemplate.find('.taskTitle').text(title);
    itemTemplate.find('.taskDescription').html(body);
    itemTemplate.find('.moveLeftButton').click(function () { ChangeStatus(this, direction.Left); });
    itemTemplate.find('.moveRightButton').click(function () { ChangeStatus(this, direction.Right); });
    itemTemplate.find('.deleteButton').click(function () {
        $(this).siblings('.confirmDelete').show();
        $(this).hide();
    });
    itemTemplate.find('.noDeleteButton').click(function () {
        $(this).parent('.confirmDelete').hide();
        $(this).parent().siblings('.deleteButton').show();
    });
    itemTemplate.find('.yesDeleteButton').click(function () {
        deleteTask(this);
    });
    itemTemplate.dblclick(function () {
        var taskElement = $(this);
        taskElement.children('div.taskDetailDiv').hide();
        taskElement.children('div.editDiv').show();
        var taskTitle = taskElement.find('p.taskTitle').text();
        var description = taskElement.find('p.taskDescription').text();
        taskElement.find('input.editTaskTitle').val(taskTitle);
        taskElement.find('textarea.editTaskDescription').text(description);
    });
    itemTemplate.find('.editTaskCancelButton').click(function () {
        var taskListItemElement = $(this).parents('li:first');
        taskListItemElement.children('div.editDiv').hide();
        taskListItemElement.children('div.taskDetailDiv').show();
    });
    itemTemplate.find('.editTaskSaveButton').click(function () {
        UpdateTask(this);
    });
    itemTemplate.draggable({
        revert: true,
        helper: 'clone',
        start: function (event, ui) {
            ui.helper.width($(this).width());
        }

    });
    itemTemplate.data('status', status);
    itemTemplate.data('taskId', id);
    itemTemplate.data('title', title);
    switch (status) {
        case "Not Started":
            itemTemplate.find('.moveLeftButton').hide();
            break;
        case "Completed":
            itemTemplate.find('.moveRightButton').hide();
            break;
    }
    list.append(itemTemplate);
}

var direction = {
    Left: "Left",
    Right: "Right"
};

function ChangeStatus(taskElement, moveDirection) {
    taskElement = $(taskElement);

    var taskListItemElement;
    if (taskElement.is('li')) {
        taskListItemElement = taskElement;
    } else {
        taskListItemElement = $(taskElement).parents("li:first");
    }
    var updatedStatus = GetNewStatus(taskListItemElement, moveDirection);
    if (updatedStatus == null) {
        return;
    }

    updateTaskStatus(taskListItemElement.data('taskId'), updatedStatus);
    GetTasks();
}

function GetNewStatus(element, moveDirection) {
    var currentStatus = element.data("status");
    var newStatus = null;
    if (moveDirection == direction.Left) {
        newStatus = GetLeftStatus(currentStatus);
    }
    else if (moveDirection == direction.Right) {
        newStatus = GetRightStatus(currentStatus);
    };
    return newStatus;
}

function GetLeftStatus(currentStatus) {
    var newStatus = null;
    switch (currentStatus) {
        case "In Progress": newStatus = "Not Started";
            break;
        case "Completed": newStatus = "In Progress";
            break;
    }
    return newStatus;
}
function GetRightStatus(currentStatus) {
    var newStatus = null;
    switch (currentStatus) {
        case "Not Started": newStatus = "In Progress";
            break;
        case "In Progress": newStatus = "Completed";
            break;
    }
    return newStatus;
}

function updateTaskStatus(id, status) {
    var context = new SP.ClientContext.get_current();
    var web = context.get_web();
    var list = web.get_lists().getByTitle(taskListName);
    var listItem = list.getItemById(id);
    listItem.set_item('Status', status);
    listItem.update();
}

function deleteTask(taskElement) {
    var taskListItemElement = $(taskElement).parents('li:first');
    var context = new SP.ClientContext.get_current();
    var web = context.get_web();
    var list = web.get_lists().getByTitle(taskListName);
    var taskId = taskListItemElement.data('taskId');
    var itemToDelete = list.getItemById(taskId);
    itemToDelete.deleteObject();
    context.executeQueryAsync(DeleteTaskSuccess(taskListItemElement.data('title')), DeleteTaskFail);
}

function DeleteTaskSuccess(title) {
    alert('item ' + title + ' is deleted successully.');
    GetTasks();
}

function DeleteTaskFail(sender, args) {
    alert('Request failed. ' + args.get_message() + '\n' + args.get_stackTrace());
}

function AddNewTask(title) {
    var context = new SP.ClientContext.get_current();
    var web = context.get_web();
    var list = web.get_lists().getByTitle(taskListName);
    var listItemInfo = new SP.ListItemCreationInformation();
    var listItem = list.addItem(listItemInfo);
    listItem.set_item('Title', title);
    listItem.update();
    context.executeQueryAsync(AddTaskSuccess, AddTaskFail);
}

function AddTaskSuccess() {
    alert("new item is added!");
    GetTasks();
    $('#newTaskTitle').val('');
    $('#newTaskTitle').focus();
}

function AddTaskFail(sender, args) {
    alert('Request failed. ' + args.get_message() + '\n' + args.get_stackTrace());
}

function UpdateTask(task) {
    var taskListItemElement = $(task).parents('li:first');
    var taskId = taskListItemElement.data('taskId');
    var taskTitleElement = taskListItemElement.find('input.editTaskTitle');
    var updatedTitle = taskTitleElement.val();
    if (updatedTitle == '') {
        alert('Please enter a value for the task title.');
        taskTitleElement.focus();
        return;
    }

    var updatedDescription = taskListItemElement.find('textarea.editTaskDescription').val();
    var context = new SP.ClientContext.get_current();
    var web = context.get_web();
    var list = web.get_lists().getByTitle(taskListName);
    var listItem = list.getItemById(taskId);
    listItem.set_item('Title', updatedTitle);
    listItem.set_item('Body', updatedDescription);
    listItem.update();
    GetTasks();
}