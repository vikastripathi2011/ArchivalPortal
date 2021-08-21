using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WLCaxtonMvcWebPortal.HCPServiceReference;
using WLCaxtonMvcWebPortal.Models;
using WLCaxtonMvcWebPortal.Services;
using WLCaxtonMvcWebPortal.Util;
using WLCaxtonPortalBusinessEntity;
using WLCaxtonPortalBusinessEntity.Classes;
using WLCaxtonMvcWebPortal.Code;

namespace WLCaxtonMvcWebPortal
{
    public class CommonBussinesLogic
    {
        Common common;
        /// <summary>
        /// Bind bucket data
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="fileName"></param>
        /// <param name="userId"></param>

        public CommonBussinesLogic()
        {
            common = new Common();
        }
        public void BindBucketData(string bucketName, string fileName, int userId)
        {
            string guid, spn = string.Empty;
            int pgc = 0;
            User user = new User() { LoginDetails = new WLCaxtonPortalBusinessEntity.Login() { RecordId = userId }, UserActivity = UserActivityType.ViewDocumentFirstPage };
            AWSS3FileTransferClient awsProxy = new AWSS3FileTransferClient();
            List<Key> lstKeys = new List<Key>();
            try
            {
                lstKeys = awsProxy.ListingObjects(null, "", bucketName, fileName);
            }
            catch
            {
                lstKeys = null;
            }
        }

        /// <summary>
        /// This method enable or disable the checkbox's in group
        /// </summary>
        public static void EnableDisableGroup(IList<SearchConfigAttribute> LstModel)
        {
            bool isGroupChecked = false;
            foreach (var item in LstModel)
            {
                var chkGrouping = item.IsGroupBy;
                chkGrouping = false;
                if (chkGrouping)
                {
                    chkGrouping = true;
                    isGroupChecked = true;
                }
            }

            if (!isGroupChecked)
            {
                foreach (var item in LstModel)
                {

                    var chkVisibility = item.IsVisible;
                    var chkGrouping = item.IsGroupBy;

                    if (chkVisibility)
                        chkGrouping = true;
                }
            }
        }

        /// <summary>
        /// This fucnction helps to create edit button dynamically
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static object GetEditButtons(dynamic item)
        {
            int bookID = item.BookId;

            return "<button class='delete-book display-mode' id='" + bookID + "'>Delete</button>" +
          "<button class='edit-book display-mode' id='" + bookID + "'>Edit</button>" +
          "<button class='save-book edit-mode edit-width' id='" + bookID + "'>Save</button>";
        }

        /// <summary>
        /// This function will to get roles in IList
        /// </summary>
        /// <returns></returns>
        public static IList<Role> GetRoles()
        {
            Common common = new Common();
            User loggedUser = SessionClass.loggedUser;
            IList<Role> documentTypes = null;
            WLCaxtonPortalServiceClient proxy = new WLCaxtonPortalServiceClient();
            if (loggedUser.RoleDetails.RecordId == (int)UserType.WLSuperUser)
            {
                common.AddMessageHeader(proxy);
                documentTypes = proxy.GetUserRoles();
            }
            else if (loggedUser.RoleDetails.RecordId == (int)UserType.TenantAdmin)
            {
                common.AddMessageHeader(proxy);
                documentTypes = proxy.GetUserRoles().Where(q => q.RecordId != (int)UserType.WLSuperUser).ToList();
            }
            else if (loggedUser.RoleDetails.RecordId == (int)UserType.User)
            {
                documentTypes = null;
            }
            
            proxy.Close();
            return documentTypes;
        }


    }
}