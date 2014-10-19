Type.registerNamespace('WorkflowTrigger.Ribbon');

WorkflowTrigger.Ribbon.RibbonComponent = function () {
    WorkflowTrigger.Ribbon.RibbonComponent.initializeBase(this);
}

WorkflowTrigger.Ribbon.RibbonComponent.get_instance = function () {
    if (!WorkflowTrigger.Ribbon.RibbonComponent.s_instance) {
        WorkflowTrigger.Ribbon.RibbonComponent.s_instance = new WorkflowTrigger.Ribbon.RibbonComponent();
    }
    return WorkflowTrigger.Ribbon.RibbonComponent.s_instance;
}

WorkflowTrigger.Ribbon.RibbonComponent.prototype = {
    focusedCommands: null,
    globalCommands: null,
    registerWithPageManager: function () {
        SP.Ribbon.PageManager.get_instance().addPageComponent(this);
        SP.Ribbon.PageManager.get_instance().get_focusManager().requestFocusForComponent(this);
    },

    unregisterWithPageManager: function () {
        SP.Ribbon.PageManager.get_instance().removePageComponent(this);
    },

    init: function () { },

    getFocusedCommands: function () {
        return ['SharePoint.TestLab.WorkflowTrigger.PopulateMenus'];
    },

    getGlobalCommands: function () {
        return ['SharePoint.TestLab.WorkflowTrigger.PopulateMenus'];
    },

    canHandleCommand: function (commandId) {
        if (commandId === 'SharePoint.TestLab.WorkflowTrigger.PopulateMenus') {
            return true;
        }
        else { return false; }
    },

    populateMenuXml: null,

    handleCommand: function (commandId, properties, sequence) {
        if (commandId === 'SharePoint.TestLab.WorkflowTrigger.PopulateMenus') {
            //properties.PopulationXML = this.GetDynamicMenuXml();
            if (this.populateMenuXml == null)
            {
                this.GetDynamicMenuXml();
            }
            properties.populateMenuXml = this.populateMenuXml;
        }
        else {
            return handleCommand(commandId, properties, sequence);
        }
    },

    isFocusable: function () { return true; },

    receiveFocus: function () { return true; },

    yieldFocus: function () { return true; },

    currentListWorkflowAssociations: null,

    GetDynamicMenuXml: function () {
        var ctx = SP.ClientContext.get_current();
        var web = ctx.get_web();
        var listId = _spPageContextInfo.pageListId;
        var currentList = web.get_lists().getById(listId);
        this.currentListWorkflowAssociations = currentList.get_workflowAssociations();
        ctx.load(this.currentListWorkflowAssociations);
        ctx.executeQueryAsync(
            Function.createDelegate(this, this.onQuerySucceeded),
            //Function.createDelegate(this, function(){this.onQuerySucceeded(properties)}),
            Function.createDelegate(this, this.onQUeryFailed));
    },

    onQuerySucceeded: function ()
    {
        var looper = this.currentListWorkflowAssociations.getEnumerator();
        var counter = 0;

        var xml = '<Menu Id = "SharePoint.TestLab.WorkflowTrigger.Anchor.Menu">'
        + '<MenuSection Id="SharePoint.TestLab.WorkflowTrigger.Anchor.Menu.MenuSection1" >'
        + '<Controls Id="SharePoint.TestLab.WorkflowTrigger.Anchor.Menu.MenuSection1.Controls">';
        while (looper.moveNext())
        {
            counter = counter + 1;
            var current = looper.get_current();
            var workflowAssociationId = current.get_id();
            var workflowName = current.get_name();
            var workflowDesc = current.get_description();
            var buttonXml = String.format(
                    '<Button Id= "SharePoint.TestLab.WorkflowTrigger.Anchor.Menu.MenuSection1.Menu{0}" '
                    + 'Command="SharePoint.TestLab.WorkflowTrigger.TriggerMenuClick" '
                    + 'MenuItemId="{1}" '
                    + 'LabelText="{2}" '
                    + 'ToolTipTitle="{2}" '
                    + 'ToolTipDescription="{3}" TemplateAlias="o1"/>', counter, workflowAssociationId, workflowName, workflowDesc);
            xml += buttonXml;
        }
        if (counter === 0)
        {
            var msgXml = String.format(
                    '<Button Id= "SharePoint.TestLab.WorkflowTrigger.Anchor.Menu.MenuSection1.Menu{0}" '
                    + 'Command="SharePoint.TestLab.WorkflowTrigger.MessageMenuClick" '
                    + 'MenuItemId="1" '
                    + 'LabelText="{0}" '
                    + 'ToolTipTitle="{0}" '
                    + 'ToolTipDescription="{0}" TemplateAlias="o1"/>', 'No workflow associated on current list');
            xml += buttonXml;
        }
        xml += '</Controls>' + '</MenuSection>' + '</Menu>';
        this.populateMenuXml = xml;
    },

    onQUeryFailed: function (sender, args)
    {
        alert('Failed to get workflow on this list');
    }

}

WorkflowTrigger.Ribbon.RibbonComponent.registerClass('WorkflowTrigger.Ribbon.RibbonComponent', CUI.Page.PageComponent);
WorkflowTrigger.Ribbon.RibbonComponent.get_instance().registerWithPageManager();
NotifyScriptLoadedAndExecuteWaitingJobs("WorkflowTriggerPageComponent.js");
