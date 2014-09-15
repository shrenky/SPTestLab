using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Office.RecordsManagement.RecordsRepository;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.Utilities;

namespace SharePoint.TestLab.RecordsRepository.Layouts.SharePoint.TestLab.RecordsRepository
{
    public partial class RouterTest : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SPWeb web = SPContext.Current.Web;
            SPList dropOffLib = web.Lists["Drop Off Library"];
            EcmDocumentRoutingWeb routingWeb = new EcmDocumentRoutingWeb(web);
            EcmDocumentRouter router = routingWeb.Router;
            string finalDestination;
            bool wasRoutedToOtherWeb;
            if (routingWeb.IsRoutingEnabled) //是否启用了传送 routing
            {
                foreach (SPListItem item in dropOffLib.Items)
                {
                    Response.Write("Item: " + item.DisplayName);
                    SPFile file = item.File;
                    bool enableModeration = item.ParentList.EnableModeration; //是否启用审批
                    bool fileModified = false; //在传送之前需要先checkin和approve
                    if (file.CheckOutType != SPFile.SPCheckOutType.None)
                    {
                        file.CheckIn(string.Empty);
                        fileModified = true;
                    }
                    if (enableModeration && item.DoesUserHavePermissions(SPBasePermissions.ApproveItems))
                    {
                        file.Approve(string.Empty);
                        fileModified = true;
                    }
                    SPListItem newItem = item;
                    if (fileModified)
                    {
                        newItem = web.GetListItem(SPUrlUtility.CombineUrl(web.ServerRelativeUrl, item.Url));
                    }
                    bool isApproved = true;
                    if (enableModeration)
                    {
                        isApproved = (newItem.ModerationInformation != null) && (newItem.ModerationInformation.Status == SPModerationStatusType.Approved);
                    }
                    if (isApproved) //必须是approve的状态才可以传送
                    {
                        bool success = router.RouteFileToFinalDestination(newItem, out finalDestination, out wasRoutedToOtherWeb);
                        //输出传送的结果
                        Response.Write(string.Format("Route result: {0} ", success));
                        Response.Write(string.Format("finalDestination: {0} ", finalDestination));
                        Response.Write(string.Format("wasRoutedToOtherWeb: {0} ", wasRoutedToOtherWeb));
                        Response.Write("</br>");
                    }
                }

            }
        }

        private void RouteItem(SPWeb web, SPListItem item)
        {
            bool routeSucceeded = false;
            EcmDocumentRoutingWeb routingWeb = new EcmDocumentRoutingWeb(web);
            string serverRelativeUrl = "Drop Off Library"; //EcmCommonUrls.GetServerRelativeUrl(SPContext.Current.Web, EcmCommonUrls.Id.DropOffZone, 需要多语言
            bool targetNotFound = false;
            string specifiedTargetUrl = string.Empty;
            if (routingWeb.IsRoutingEnabled)
            {
                SPListItem routeItem = item;
                SPFile file = routeItem.File;
                bool enableModeration = routeItem.ParentList.EnableModeration;
                bool fileOperationClear = false; //flag3
                if (file.CheckOutType != SPFile.SPCheckOutType.None)
                {
                    file.CheckIn(string.Empty);
                    fileOperationClear = true;
                }
                if (enableModeration && routeItem.DoesUserHavePermissions(SPBasePermissions.ApproveItems))
                {
                    file.Approve(string.Empty);
                    fileOperationClear = true;
                }
                if (fileOperationClear)
                {
                    routeItem = web.GetListItem(SPUrlUtility.CombineUrl(web.ServerRelativeUrl, routeItem.Url));
                    file = routeItem.File;
                }

                string displayName = routeItem.DisplayName;
                string finalUrl = web.Site.MakeFullUrl(file.ServerRelativeUrl);
                string docIdFromListItem = routeItem[new Guid("AE3E2A36-125D-45d3-9051-744B513536A6")] as string;
                SPUser currentUser = SPContext.Current.Web.CurrentUser;
                string str5;


                string routeDestination = string.Empty;
                bool routedExternally = false;
                ExternalRoutingResultProperties externalRouteResult = new ExternalRoutingResultProperties();
                bool flag4 = false;

                bool flag5 = true;
                if (enableModeration)
                {
                    flag5 = (routeItem.ModerationInformation != null) &&
                            (routeItem.ModerationInformation.Status == SPModerationStatusType.Approved);
                }
                if (flag5)
                {
                    flag4 = true;
                    SPSecurity.RunWithElevatedPrivileges(() =>
                    {
                        using (SPSite site = new SPSite(web.Site.ID))
                        {
                            using (SPWeb newWeb = site.OpenWeb(web.ID))
                            {
                                EcmDocumentRoutingWeb newRoutingWeb = new EcmDocumentRoutingWeb(newWeb);
                                try
                                {
                                    string finalDestination;
                                    bool wasRoutedToOtherWeb;
                                    routeSucceeded = newRoutingWeb.Router.RouteFileToFinalDestination(routeItem,
                                        out finalDestination, out wasRoutedToOtherWeb);
                                    //newRoutingWeb.Router.RouteFileToFinalLocationNowAsSystem(routeItem, web, currentUser, string.Empty, out routeDestination, out externalRouteResult);
                                    routedExternally = externalRouteResult.IsExternalRouter;
                                }
                                catch (DirectoryNotFoundException exception)
                                {
                                    routeSucceeded = false;
                                    specifiedTargetUrl = exception.Message;
                                    targetNotFound = !string.IsNullOrEmpty(specifiedTargetUrl);
                                }
                            }
                        }
                    });

                }

            }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct ExternalRoutingResultProperties
    {
        internal bool IsExternalRouter;
        internal OfficialFileResult ExternalRouterResult;
    }


}
