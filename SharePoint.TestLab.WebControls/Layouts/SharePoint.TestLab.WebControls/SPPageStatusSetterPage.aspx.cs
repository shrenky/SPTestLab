using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace SharePoint.TestLab.WebControls.Layouts.SharePoint.TestLab.WebControls
{
    public partial class SPPageStatusSetterPage : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string imgUrl = "http://res2.windows.microsoft.com/resbox/en/windows%207/main/0d8a4985-b5e2-41a6-a1b6-e4bafb517937_92.png";
            SPPageStatusSetter statusSetter = new SPPageStatusSetter();
            statusSetter.AddStatus("Title", string.Format("可以使用html的内容: <img src={0}>", imgUrl), SPPageStatusColor.Yellow);
            this.Controls.Add(statusSetter);
        }
    }
}
