using Microsoft.SharePoint.WebControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace SharePoint.TestLab.WorkflowTrigger
{
    public class RibbonLoaderControl : Control
    {
        protected override void OnPreRender(EventArgs e)
        {
            SPRibbon ribbon = SPRibbon.GetCurrent(this.Page);
            if (ribbon != null)
            {
                RegisterWorkflowTriggerRibbon(ribbon);
            }

            base.OnPreRender(e);
        }

        private void RegisterWorkflowTriggerRibbon(SPRibbon ribbon)
        {
            ScriptLink.RegisterScriptAfterUI(this.Page, "CUI.js", false, true);
            ScriptLink.RegisterScriptAfterUI(this.Page, "SP.Ribbon.js", false, true);
            ScriptLink.RegisterScriptAfterUI(this.Page, "/_layouts/15/SharePoint.TestLab.WorkflowTrigger/js/WorkflowTriggerPageComponent.js", false, true);
        }
    }
}
