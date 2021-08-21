//==========================================================================================
// Project              : WLCaxtonWebPortal
// File Name            : IWLCaxtonPortalService.cs
// Program Description  : IWLCaxtonPortalService contract implementation
// Programmed By        : Naushad Ali
// Programmed On        : 10-December-2012 
// Version              : 1.0.0
//==========================================================================================

#region Namespaces
using System;
using System.Collections.Generic;
using WLCaxtonPortalBusinessEntity;
using WLCaxtonPortalServiceLayer.Base;
using WLCaxtonPortalBusinessLogicLayer;
using WLCaxtonPortalBusinessEntity.Classes;
//using WLCaxtonPortalE2VaultComponent;
using System.ServiceModel.Activation; 
#endregion

namespace WLCaxtonPortalServiceLayer
{    
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class WLCaxtonPortalService :ServiceBase, IWLCaxtonPortalService
    {
        #region Private variables
        private LoginBL loginBL = null; 
        #endregion

        #region Public methods
       /// <summary>
       /// Authenticate the user 
       /// </summary>
       /// <param name="login">logic details for authentication</param>
       /// <returns>user object contains the authenticated user</returns>
        public User AuthenticateUser(Login login)
        {
            try
            {
                loginBL = new LoginBL();
                return loginBL.AuthenticateUser(login);
            }
            catch (Exception ex)
            {
                throw GetFaultException(ex, ex.Message, "Exception occured in AuthenticateUser()");
            }
        }

        /// <summary>
        /// Use to ChangePassword old password with new one
        /// </summary>
        /// <param name="user">logic details such as emaild,teenantid,old password, new password</param>
        /// <param name="auditTrail">audit trail object.such datetime,created or modified by</param>
        /// <returns></returns>
        public Message ChangePassword(User user, AuditTrail auditTrail)
        {
            try
            {
                UserBL userBL = new UserBL();
                return userBL.ChangePassword(user, auditTrail);
            }
            catch (Exception ex)
            {
                throw GetFaultException(ex, ex.Message, "Exception occured in ChangePassword()");
            }
        }

        /// <summary>
        /// Use to reset the existing password 
        /// </summary>
        /// <param name="user">logic details such as userid,emailid,teenantid, new password</param>
        /// <param name="auditTrail">audit trail object.such datetime,created or modified by</param>
        /// <returns></returns>
        public Message ResetPassword(User user, AuditTrail auditTrail)
        {
            try
            {
                UserBL userBL = new UserBL();
                return userBL.ResetPassword(user, auditTrail);
            }
            catch (Exception ex)
            {
                throw GetFaultException(ex, ex.Message, "Exception occured in ResetPassword()");
            }
        }

        /// <summary>
        /// Use to reset the forget password 
        /// </summary>
        /// <param name="login">logic details such as emaild amd password</param>
        /// <param name="auditTrail">audit trail object.such datetime,created or modified by</param>
        /// <returns></returns>
        public Message ForgetPassword(Login login, AuditTrail auditTrail)
        {
            try
            {
                return UserBL.ForgetPassword(login, auditTrail);
            }
            catch (Exception ex)
            {
                throw GetFaultException(ex, ex.Message, "Exception occured in ForgetPassword()");
            }
        }

        /// <summary>
        /// Get the product list
        /// </summary>
        /// <returns></returns>
        public IList<ProductCategory> GetProductCategories()
        {
            try
            {
                return CommonBL.GetProductCategories();
            }
            catch (Exception ex)
            {
                throw GetFaultException(ex, ex.Message, "Exception occured in GetProductCategories()");
            }
        }

        /// <summary>
        /// Get the document type list
        /// </summary>
        /// <returns></returns>
        public IList<DocumentType> GetDocumentTypes()
        {
            try
            {
                return CommonBL.GetDocumentTypes();
            }
            catch (Exception ex)
            {
                throw GetFaultException(ex, ex.Message, "Exception occured in GetDocumentTypes()");
            }
        }

        /// <summary>
        /// Get the documents based on search criteria
        /// </summary>
        /// <param name="docParameter">object contains search query in xml and teanant code </param>
        /// <returns></returns>
        public SearchResult GetDocumentsBySearchCriteria(DocumentSearchParameter docParameter, Tenant tenant)
        {
            try
            {
                return CommonBL.GetDocumentsBySearchCriteria(docParameter, tenant);
            }
            catch (Exception ex)
            {
                throw GetFaultException(ex, ex.Message, "Exception occured in GetDocumentsBySearchCriteria()");
            }
        }

        /// <summary>
        /// Get the spacific document details 
        /// </summary>
        /// <param name="documentGuid">unique identifier for document</param>
        /// <param name="teenantId">client code</param>
        public Document GetDocumentByDocumentGuid(string documentGuid, Tenant tenant)
        {
            try
            {
                return CommonBL.GetDocumentByDocumentGuid(documentGuid, tenant);
            }
            catch (Exception ex)
            {
                throw GetFaultException(ex, ex.Message, "Exception occured in GetDocumentByDocumentGuid()");
            }
        }

        /// <summary>
        /// Get the sort critearis set by individual user
        /// </summary>
        /// <returns></returns>
        public IList<SortCriteria> GetSortCriterias()
        {
            try
            {
                return CommonBL.GetSortCriterias();
            }
            catch (Exception ex)
            {
                throw GetFaultException(ex, ex.Message, "Exception occured in GetSearchParameters()");
            }
        }
        
        //public byte[] GetPdfDocument(User user, string documentGuid, int startPageNumber, int pageCount)
        //{
        //    try
        //    {
        //        E2VaultWrapper e2VaultWrapper = new E2VaultWrapper();
        //       return e2VaultWrapper.GetPdfDocument(user, documentGuid, startPageNumber, pageCount);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw GetFaultException(ex, ex.Message, "Exception occured in GetPdfDocument()");
        //    }
        //}

        //public byte[] GetFirstpage(User user, string documentGuid)
        //{
        //    try
        //    {
        //        E2VaultWrapper e2VaultWrapper = new E2VaultWrapper();
        //        return e2VaultWrapper.GetFirstpage(user, documentGuid);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw GetFaultException(ex, ex.Message, "Exception occured in GetPdfDocument()");
        //    }
        //}

        /// <summary>
        /// Get the client list
        /// </summary>
        /// <returns></returns>
        public IList<Tenant> GetTenant()
        {
            try
            {
                return new UserBL().GetTenant();
            }
            catch (Exception ex)
            {
                throw GetFaultException(ex, ex.Message, "Exception occured in GetTenant()");
            }
        }

        /// <summary>
        /// Get the user roles such super user,tenneat admin and noram user
        /// </summary>
        /// <returns></returns>
        public IList<Role> GetUserRoles()
        {
            try
            {
                return new UserBL().GetUserRoles();
            }
            catch (Exception ex)
            {
                throw GetFaultException(ex, ex.Message, "Exception occured in GetUserRoles()");
            }
        }

        /// <summary>
        /// Fetch the all user 
        /// </summary>
        /// <returns></returns>
        public IList<User> GetUserList()
        {
            try
            {
                return new UserBL().GetUserList();
            }
            catch (Exception ex)
            {
                throw GetFaultException(ex, ex.Message, "Exception occured in GetUserList()");
            }
        }

        /// <summary>
        /// Add or update the new user
        /// </summary>
        /// <param name="user">user information</param>
        /// <param name="IsAdd">flag for update or add.</param>
        /// <returns></returns>
        public Message AddUpdateUser(User user, bool IsAdd)
        {
            try
            {
                return new UserBL().AddUpdateUser(user, IsAdd);
            }
            catch (Exception ex)
            {
                throw GetFaultException(ex, ex.Message, "Exception occured in AddNewUser()");
            }
        }

        /// <summary>
        /// Update the exsiting user account
        /// </summary>
        /// <param name="user">user information</param>
        /// <param name="auditTrail">audit information</param>
        /// <returns></returns>
        public Message UpdateUserAccount(User user, AuditTrail auditTrail)
        {
            try
            {
                return new UserBL().UpdateUserAccount(user, auditTrail);
            }
            catch (Exception ex)
            {
                throw GetFaultException(ex, ex.Message, "Exception occured in UpdateUserAccount()");
            }
        }

        /// <summary>
        /// Logout the user from the application
        /// </summary>
        /// <param name="user">user information</param>
        /// <returns></returns>
        public Message Logout(User user)
        {
            try
            {
                return new LoginBL().Logout(user);
            }
            catch (Exception ex)
            {
                throw GetFaultException(ex, ex.Message, "Exception occured in Logout()");
            }
        }

        /// <summary>
        /// Get the SearchConfigParameters critearis set by individual user
        /// </summary>
        /// <param name="docParameter"></param>
        /// <returns></returns>
        public IList<SearchConfigAttribute> GetSearchConfigParameters(DocumentSearchParameter docParameter)
        {
            try
            {
                return CommonBL.GetSearchConfigParameters(docParameter);
            }
            catch (Exception ex)
            {
                throw GetFaultException(ex, ex.Message, "Exception occured in GetSearchConfigParameters()");
            }
        }

        /// <summary>
        /// Save the configuration list for individual user
        /// </summary>
        /// <param name="searchConfigXML">collection of config values</param>
        /// <param name="auditTrail">obbject for tracking purpose</param>
        /// <param name="user">User object</param>
        /// <returns>Message object contains message description and issuccess</returns>
        public Message SaveConfigureList(SearchParameters searchParameters, AuditTrail auditTrail, User user)
        {
            try
            {
                return CommonBL.SaveConfigureList(searchParameters, auditTrail, user);
            }
            catch (Exception ex)
            {
                throw GetFaultException(ex, ex.Message, "Exception occured in SaveConfigureList()");
            }
        }

        /// <summary>
        /// Check to see existing user logoged in user session
        /// </summary>
        /// <param name="user">user object</param>
        /// <returns></returns>
        public Message CheckUserSession(User user)
        {
            try
            {
                LoginBL loginBL = new LoginBL();
                return loginBL.CheckUserSession(user);
            }
            catch (Exception ex)
            {
                throw GetFaultException(ex, ex.Message, "Exception occured in CheckUserSession()");
            }
           
        } 
        #endregion
    }
}
