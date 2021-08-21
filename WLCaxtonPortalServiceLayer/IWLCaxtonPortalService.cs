//==========================================================================================
// Project              : WLCaxtonWebPortal
// File Name            : IWLCaxtonPortalService.cs
// Program Description  : Service Contract for WLCaxtonPortalService 
// Programmed By        : Naushad Ali
// Programmed On        : 10-December-2012 
// Version              : 1.0.0
//==========================================================================================

using System;
using System.Collections.Generic;
using System.ServiceModel;
using WLCaxtonPortalBusinessEntity;
using WLCaxtonPortalBusinessEntity.Classes;

namespace WLCaxtonPortalServiceLayer
{
    [ServiceContract]
    public interface IWLCaxtonPortalService
    {
        [OperationContract]
        User AuthenticateUser(Login login);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Message ChangePassword(User user, AuditTrail auditTrail);

        [OperationContract]
        Message ResetPassword(User user, AuditTrail auditTrail);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Message ForgetPassword(Login login, AuditTrail auditTrail);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<ProductCategory> GetProductCategories();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<DocumentType> GetDocumentTypes();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        SearchResult GetDocumentsBySearchCriteria(DocumentSearchParameter docParameter,Tenant tenant);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Document GetDocumentByDocumentGuid(string documentGuid, Tenant tenant);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<SortCriteria> GetSortCriterias();

        //[OperationContract]
        //byte[] GetPdfDocument(User user, string documentGuid, int startPageNumber, int pagesCount);

        //[OperationContract]
        //byte[] GetFirstpage(User user, string documentGuid);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<Tenant> GetTenant();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<Role> GetUserRoles();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<User> GetUserList();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Message AddUpdateUser(User user, bool IsAdd);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Message UpdateUserAccount(User user, AuditTrail auditTrail);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Message Logout(User user);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<SearchConfigAttribute> GetSearchConfigParameters(DocumentSearchParameter docParameter);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Message SaveConfigureList(SearchParameters searchParameters, AuditTrail auditTrail, User user);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Message CheckUserSession(User user);
    }    
}
