using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Collections;
using System.Collections.Generic;

namespace SharePoint.TestLab.Ribbon.Layouts.SharePoint.TestLab.Ribbon
{
    public partial class MyRibbonPage : LayoutsPageBase
    {
        protected override void OnPreRender(EventArgs e)
        {
            SPRibbon ribbon = SPRibbon.GetCurrent(this);
            if (ribbon != null)
            {
                ribbon.CommandUIVisible = true;
                ribbon.MakeTabAvailable("Ribbon.Tabs.MyCustomTab");
                ribbon.InitialTabId = "Ribbon.Tabs.MyCustomTab";
                ribbon.Minimized = false;
                ribbon.CommandUIVisible = true;
                ribbon.ServerRendered = true;

                SPRibbonScriptManager manager = new SPRibbonScriptManager();
                List<IRibbonCommand> commands = new List<IRibbonCommand>();
                commands.Add(new SPRibbonCommand("MyCustomTab", ""));
                commands.Add(new SPRibbonCommand("MyCustomTabActions", ""));
                commands.Add(new SPRibbonCommand("MyCustomSave", "alert(commandId)"));
                
                manager.RegisterGetCommandsFunction(this, "getGlobalCommands", commands);
                manager.RegisterCommandEnabledFunction(this, "commandEnabled", commands);
                manager.RegisterHandleCommandFunction(this, "handleCommand", commands);

                ScriptLink.RegisterScriptAfterUI(this.Page, "CUI.js", false, true);
                ScriptLink.RegisterScriptAfterUI(this.Page, "SP.Ribbon.js", false, true);
                ScriptLink.RegisterScriptAfterUI(this.Page, "/_layouts/15/SharePoint.TestLab.Ribbon/MyCustomRibbon.js", false, true);

                //string script = "<script type=\"text/javascript\" defer=\"true\"> //<![CDATA[ \r\n function InitPageComponent(){SP.Ribbon.UsageReportPageComponent.initialize();} \r\n SP.SOD.executeOrDelayUntilScriptLoaded(InitPageComponent, \"SP.Ribbon.js\" ); \r\n //]]\r\n </script>";
                String script = "<script type=\"text/javascript\" defer=\"true\"> //<![CDATA[ \r\n function InitPageComponent() { MyCustom.Ribbon.RibbonComponent.get_instance().registerWithPageManager()} \r\n SP.SOD.executeOrDelayUntilScriptLoaded(InitPageComponent, \"SP.UI.MyCustomRibbon.debug.js\"); \r\n //]]\r\n</script>";
                this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "InitPageComponent", script, false);
            }
            base.OnPreRender(e);
        }
    }
}
