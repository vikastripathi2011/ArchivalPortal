//==========================================================================================
// Project              : WLCaxtonWebPortal
// File Name            : DataLayerBase.cs
// Program Description  : Reusable fucntionalty for DataLayer logic
// Programmed By        : Naushad Ali
// Programmed On        : 10-December-2012 
// Version              : 1.0.0
//==========================================================================================

#region Namespaces

using System;
using System.Collections.Generic;
using System.Configuration;
using WLCaxtonPortalBusinessEntity;
using WLCaxtonPortalBusinessEntity.Classes;
using WLCaxtonPortalServiceLayer;
//using System.ServiceModel.Channels;

//ChannelFactoryBase<IWLCaxtonPortalService>
#endregion

namespace WLCaxtonMvcWebPortal.Services
{
    #region WLCaxtonPortalServiceClient Service Class

    public class WLCaxtonPortalServiceClient :  System.ServiceModel.ClientBase<IWLCaxtonPortalService>, IWLCaxtonPortalService
    {
        #region PagePublicVariables
        
        
        public static string ServiceConfigEndPointName = ConfigurationManager.AppSettings["WLCaxtonPortalService_ActiveEndPoint"].ToString();

        #endregion

        #region PublicMethods

        /// <summary>
        /// This method call the channel factory method
        /// </summary>
        public WLCaxtonPortalServiceClient()
            : base(ServiceConfigEndPointName)
        {
        }

        /// <summary>
        /// This method returns the user authentication result
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public User AuthenticateUser(Login login)
        {
            return base.Channel.AuthenticateUser(login);
        }

        /// <summary>
        /// This method call the Change Passowrd method to Change User Password
        /// </summary>
        /// <param name="user"></param>
        /// <param name="auditTrail"></param>
        /// <returns></returns>
        public Message ChangePassword(User user, AuditTrail auditTrail)
        {
            return base.Channel.ChangePassword(user, auditTrail);
        }

        /// <summary>
        /// This method call the Reset Passowrd method to Reset User Password
        /// </summary>
        /// <param name="user"></param>
        /// <param name="auditTrail"></param>
        /// <returns></returns>
        public Message ResetPassword(User user, AuditTrail auditTrail)
        {
            return base.Channel.ResetPassword(user, auditTrail);
        }

        /// <summary>
        /// This method call the Forget Passowrd method to Reset User Password
        /// </summary>
        /// <param name="login"></param>
        /// <param name="auditTrail"></param>
        /// <returns></returns>
        public Message ForgetPassword(Login login, AuditTrail auditTrail)
        {
            return base.Channel.ForgetPassword(login, auditTrail);
        }

        /// <summary>
        /// This method returns the Product Categories details
        /// </summary>
        /// <returns></returns>
        public IList<ProductCategory> GetProductCategories()
        {
            return base.Channel.GetProductCategories();
        }

        /// <summary>
        /// This method returns the DocumentTypes details
        /// </summary>
        /// <returns></returns>
        public IList<DocumentType> GetDocumentTypes()
        {
            return base.Channel.GetDocumentTypes();
        }

        /// <summary>
        /// This method used for searching the documents details on the basis of search criteria
        /// </summary>
        /// <param name="docParameter"></param>
        /// <param name="tenant"></param>
        /// <returns></returns>
        public SearchResult GetDocumentsBySearchCriteria(DocumentSearchParameter docParameter,Tenant tenant)
        {
            return base.Channel.GetDocumentsBySearchCriteria(docParameter, tenant);
        }

        /// <summary>
        /// This method returns the documents details on the basis of DocumentGUID
        /// </summary>
        /// <param name="documentGuid"></param>
        /// <param name="teenantId"></param>
        /// <returns></returns>
        public Document GetDocumentByDocumentGuid(string documentGuid, Tenant tenant)
        {
            return base.Channel.GetDocumentByDocumentGuid(documentGuid, tenant);
        }

        /// <summary>
        /// This method returns the sort criterias
        /// </summary>
        /// <returns></returns>
        public IList<SortCriteria> GetSortCriterias()
        {
            return base.Channel.GetSortCriterias();
        }

        /// <summary>
        /// This method returns the document details in the PDF format
        /// </summary>
        /// <param name="user"></param>
        /// <param name="documentGuid"></param>
        /// <returns></returns>
        //public byte[] GetPdfDocument(User user, string documentGuid, int startPageNumber, int pagesCount)
        //{
        //    return base.Channel.GetPdfDocument(user, documentGuid, startPageNumber, pagesCount);
        //}

        ///// <summary>
        ///// This method return the First page of the document search result
        ///// </summary>
        ///// <param name="user"></param>
        ///// <param name="documentGuid"></param>
        ///// <returns></returns>
        //public byte[] GetFirstpage(User user, string documentGuid)
        //{
        //    return base.Channel.GetFirstpage(user, documentGuid);
        //}

        /// <summary>
        /// This method return the Tenant List
        /// </summary>
        /// <returns></returns>
        public IList<Tenant> GetTenant()
        {
            return base.Channel.GetTenant();
        }

        /// <summary>
        /// This method returns the UserRole List
        /// </summary>
        /// <returns></returns>
        public IList<Role> GetUserRoles()
        {
            return base.Channel.GetUserRoles();
        }
        
        /// <summary>
        /// This method returns the User List
        /// </summary>
        /// <returns></returns>
        public IList<User> GetUserList()
        {
            return base.Channel.GetUserList();
        }       

        /// <summary>
        /// This method used for adding or updating the user details
        /// </summary>
        /// <param name="user"></param>
        /// <param name="IsAdd"></param>
        /// <returns></returns>
        public Message AddUpdateUser(User user, bool IsAdd)
        {
            return base.Channel.AddUpdateUser(user, IsAdd);
        }

        /// <summary>
        /// This method update the user account
        /// </summary>
        /// <param name="user"></param>
        /// <param name="auditTrail"></param>
        /// <returns></returns>
        public Message UpdateUserAccount(User user, AuditTrail auditTrail)
        {
            return base.Channel.UpdateUserAccount(user, auditTrail);
        }

        /// <summary>
        /// This method call the Logout method to kill current logged user session
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Message Logout(User user)
        {
            return base.Channel.Logout(user);
        }

        /// <summary>
        /// This method returns Search Configuration Parameters List
        /// </summary>
        /// <param name="docParameter"></param>
        /// <returns></returns>
        public IList<SearchConfigAttribute> GetSearchConfigParameters(DocumentSearchParameter docParameter)
        {
            return base.Channel.GetSearchConfigParameters(docParameter);
        }

        /// <summary>
        /// This method call the SaveConfigureList method to save the new configuration list
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="auditTrail"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public Message SaveConfigureList(SearchParameters searchParameters, AuditTrail auditTrail, User user)
        {
            return base.Channel.SaveConfigureList(searchParameters, auditTrail, user);
        }

        /// <summary>
        /// This method call CheckUserSession method to check the current logging user session
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Message CheckUserSession(User user)
        {
            return base.Channel.CheckUserSession(user);
        }

        #endregion
    }

    #endregion
}
