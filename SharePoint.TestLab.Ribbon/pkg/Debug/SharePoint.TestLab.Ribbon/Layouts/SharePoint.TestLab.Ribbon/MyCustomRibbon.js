Type.registerNamespace("MyCustom.Ribbon");

MyCustom.Ribbon.RibbonComponent = function () {
    MyCustom.Ribbon.RibbonComponent.initializeBase(this);
}
MyCustom.Ribbon.RibbonComponent.get_instance = function() {
    if (!MyCustom.Ribbon.RibbonComponent.s_instance) {
        MyCustom.Ribbon.RibbonComponent.s_instance =
        new MyCustom.Ribbon.RibbonComponent();
    }
    return MyCustom.Ribbon.RibbonComponent.s_instance;
}
MyCustom.Ribbon.RibbonComponent.prototype = {
    focusedCommands: null,
    globalCommands: null,
    registerWithPageManager: function () {
        SP.Ribbon.PageManager.get_instance().addPageComponent(this);
        SP.Ribbon.PageManager.get_instance().get_focusManager().requestFocusForComponent(this);
    },
    unregisterWithPageManager: function () {
        SP.Ribbon.PageManager.get_instance().removePageComponent(this);
    },
    init: function () {
    },
    getFocusedCommands: function () {
        return [];
    },
    getGlobalCommands: function () {
        return getGlobalCommands();
    },
    canHandleCommand: function (commandId) {
        return commandEnabled(commandId);
    },
    handleCommand: function (commandId, properties, sequence) {
        return handleCommand(commandId, properties, sequence);
    },
    isFocusable: function () {
        return true;
    },
    receiveFocus: function () {
        return true;
    },
    yieldFocus: function () {
        return true;
    }
}
MyCustom.Ribbon.RibbonComponent.registerClass('MyCustom.Ribbon.RibbonComponent', CUI.Page.PageComponent);
NotifyScriptLoadedAndExecuteWaitingJobs("sp.ui.mycustomribbon.debug.js");