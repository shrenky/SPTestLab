using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace SharePoint.TestLab.JQuery.MyPictures
{
    [ToolboxItemAttribute(false)]
    public class MyPictures : WebPart
    {
        protected override void CreateChildControls()
        {
            Control control = Page.LoadControl(@"/_layouts/15/MyPicturesWebpart/MyPictureWebpartControl.ascx");
            Controls.Add(control);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            SPList list = SPContext.Current.Web.Lists.TryGetList("Pictures");
            if (list == null) return;
            var pics = new List<string>();
            foreach (SPListItem item in list.Items)
            {
                pics.Add(string.Format("'{0}/{1}'", SPContext.Current.Web.ServerRelativeUrl, item.Url));
            }

            var items = string.Join(",", pics.ToArray());
            Page.ClientScript.RegisterArrayDeclaration("MyPictures", items);
        }
    }
}
