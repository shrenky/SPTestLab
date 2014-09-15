using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Administration.Claims;

namespace SharePoint.TestLab.Helper.User
{
    public static class UserPropertiesHelper
    {
        public static SPUserToken GetUserTokenByLoginName(string siteUrl, Guid webId, string loginName)
        {
            SPUser spUser = null;

            SPSecurity.RunWithElevatedPrivileges(() =>
            {
                using (SPSite site = new SPSite(siteUrl))
                {
                    using (SPWeb delegateWeb = site.OpenWeb(webId))
                    {
                        SPClaimProviderManager cpm = SPClaimProviderManager.Local;
                        try //check spuser
                        {
                            SPClaim userClaim = cpm.ConvertIdentifierToClaim(loginName,
                                SPIdentifierTypes.WindowsSamAccountName);
                            spUser = delegateWeb.AllUsers[userClaim.ToEncodedString()];
                        }
                        catch (SPException e)
                        {
                            throw e;
                        }

                    }
                }
            });
            
            return spUser.UserToken;
        }

        [System.Runtime.InteropServices.DllImport("Advapi32.DLL", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        public static extern bool ConvertStringSidToSid(string stringSid, out IntPtr pSid);
        [System.Runtime.InteropServices.DllImport("Advapi32.DLL", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        public static extern int GetLengthSid(IntPtr psid);

        public static byte[] ConvertStringSidToSid(string sid)
        {
            IntPtr sidPtr;
            if (ConvertStringSidToSid(sid, out sidPtr))
            {
                int length = GetLengthSid(sidPtr);
                byte[] sidByte = new byte[length];
                System.Runtime.InteropServices.Marshal.Copy(sidPtr, sidByte, 0, length);
                System.Runtime.InteropServices.Marshal.FreeHGlobal(sidPtr);
                return sidByte;
            }
            else
            {
                return null;
            }
        }
    }
}
