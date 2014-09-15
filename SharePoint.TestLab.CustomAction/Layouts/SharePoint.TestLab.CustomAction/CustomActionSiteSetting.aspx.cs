using System;
using System.Linq;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace SharePoint.TestLab.CustomAction.Layouts.SharePoint.TestLab.CustomAction
{
    public partial class CustomActionSiteSetting : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.MsgLabel.Text = "Show or hide custom action";
            bool show;
            show = IsCustomActionShow();
            if (!this.Page.IsPostBack)
            {
                RefreshStatus(show);
            }
        }

        protected void OnSaveButtonClick(object sender, EventArgs e)
        {
            this.MsgLabel.Text = "Saved.";
            this.Web.Properties["SharePoit.TestLab.ShowCustomAction"] = this.ShowCustomAction.Checked ? true.ToString() : false.ToString();
            this.Web.Properties.Update();
            this.Web.Update();

            bool show = IsCustomActionShow();
            if (show)
            {
                if (!CustomActionExist())
                {
                    Web.AllowUnsafeUpdates = true;
                    SPUserCustomAction action = Web.UserCustomActions.Add();
                    action.Title = "Go to settings page (SharePoint.TestLab.CustomAction)";
                    action.Description = "Go to settings page (SharePoint.TestLab.CustomAction Description)";
                    action.Group = "PersonalActions";
                    action.Location = "Microsoft.SharePoint.StandardMenu";
                    action.Sequence = 1000;
                    action.Url = "~site/_layouts/SharePoint.TestLab.CustomAction/CustomActionSiteSetting.aspx";
                    action.Name = "SharePoint.TestLab.CustomAction_5B02563D269C4CEA83B7952E79C08007";
                    action.Update();
                    Web.AllowUnsafeUpdates = false;
                }
                else
                {
                    var action = Web.UserCustomActions.SingleOrDefault(
                        item =>
                            item.Name == "SharePoint.TestLab.CustomAction_5B02563D269C4CEA83B7952E79C08007");
                    if (!(action.Location == "Microsoft.SharePoint.StandardMenu"))
                    {
                        Web.AllowUnsafeUpdates = true;
                        action.Location = "Microsoft.SharePoint.StandardMenu";
                        action.Update();
                        Web.AllowUnsafeUpdates = false;
                    }
                }
            }
            else
            {
                if (CustomActionExist())
                {
                    var action = Web.UserCustomActions.SingleOrDefault(
                        item =>
                            item.Name == "SharePoint.TestLab.CustomAction_5B02563D269C4CEA83B7952E79C08007");
                    Web.AllowUnsafeUpdates = true;
                    action.Location = "Removed";
                    action.Update();
                    Web.Update();
                    Web.AllowUnsafeUpdates = false;
                }
            }

            RefreshStatus(show);
        }

        #region private 
        
        private void RefreshStatus(bool show)
        {
            if (show)
            {
                this.ShowCustomAction.Checked = true;
                this.HideCustomAction.Checked = false;
            }
            else
            {
                this.ShowCustomAction.Checked = false;
                this.HideCustomAction.Checked = true;
            }
        }

        private bool IsCustomActionShow()
        {
            bool show;
            object value = null;
            using (SPWeb newWeb = Web.Site.OpenWeb(Web.ID))
            {
                value = newWeb.Properties["SharePoit.TestLab.ShowCustomAction"];
            }

            if (value == null)
            {
                show = true;
            }
            else
            {
                if (!string.IsNullOrEmpty(value.ToString()))
                {
                    show = Boolean.Parse(value.ToString());
                }
                else
                {
                    show = true;
                }
            }
            return show;
        }

        private bool CustomActionExist()
        {
            if (Web.UserCustomActions.Count == 0)
            {
                return false;
            }

            bool exist = false;
            foreach (var item in Web.UserCustomActions)
            {
                if (item.Name == "SharePoint.TestLab.CustomAction_5B02563D269C4CEA83B7952E79C08007")
                {
                    exist = true;
                    break;
                }
            }
            return exist;
        }

        #endregion
    }
}
