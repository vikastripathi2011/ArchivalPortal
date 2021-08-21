using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WLCaxtonMvcWebPortal.Services;
using System.IO;
using WLCaxtonPortalBusinessEntity;
using WLCaxtonPortalBusinessEntity.Classes;
using WLCaxtonPortalExceptionLogger;
using WLCaxtonMvcWebPortal.Models;
using System.Text;
using System.Configuration;
using WLCaxtonMvcWebPortal.Util;
using System.Web.UI.WebControls;
using System.Data;
using WLCaxtonMvcWebPortal.ViewModel;
using Newtonsoft.Json;
using System.ServiceModel;
//using AWSS3ServiceLayer;
using WLCaxtonMvcWebPortal.HCPServiceReference;
using System.Web.Hosting;
using System.Threading;
using WLCaxtonMvcWebPortal.Code;
using System.Collections;


namespace WLCaxtonMvcWebPortal.Areas.UserActvity.Controllers
{
    [CheckCurrentLoggedUserSession]
    public class DataController : Controller
    {
        #region "Object Declration ==>>"
        public readonly string siteroot = System.Configuration.ConfigurationManager.AppSettings["siteroot"] == null ? " " : System.Configuration.ConfigurationManager.AppSettings["siteroot"];
        public readonly string _bucketName = System.Configuration.ConfigurationManager.AppSettings["BucketName"] == null ? " " : System.Configuration.ConfigurationManager.AppSettings["BucketName"];
        public readonly string PdfRenderPath = System.Configuration.ConfigurationManager.AppSettings["PdfRenderPath"].ToString();

        DocumentViewModel objDocumentViewModel = new DocumentViewModel();
        User loggedUser = SessionClass.loggedUser;
        // string _fileName = "";
        string chklstUrl = string.Empty;
        static string _endFile = "End Of File";
        #endregion

        WLCaxtonPortalServiceClient proxy;
        AWSS3FileTransferClient awsProxy;
        Common common;

        public DataController()
        {
            common = new Common();
        }

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// This get action will generate Search page
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Search()
        {
            proxy = new WLCaxtonPortalServiceClient();
            try
            {
                common.AddMessageHeader(proxy);
                
                IList<DocumentType> documentTypes = proxy.GetDocumentTypes();
                IList<ProductCategory> productCategories = proxy.GetProductCategories();
                IList<SortCriteria> SortCriterias = proxy.GetSortCriterias();
                ListItem[] operands = ControlUtil.GetOperands();
                ViewBag.docType = documentTypes;
                ViewBag.ProductCategories = productCategories;
                ViewBag.SortCaterories = SortCriterias;
                ViewBag.Operands = operands;
                proxy.Close();
            }
            catch (Exception ex)
            {

                LoggerFactory.Logger.Log("Search Get", LogEventType.Error, ex.Message);

            }
            return View();
        }

        /// <summary>
        /// This post action will render serach result
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Search(SearchCriteria model)
        {
            string param0 = string.Empty;
            string param1 = string.Empty;
            string param2 = string.Empty;
            string param3 = string.Empty;
            string param4 = string.Empty;
            string param5 = string.Empty;
            string param6 = string.Empty;
            try
            {
                StringBuilder sbQuery = new StringBuilder();
                //Added By kamal
                StringBuilder sbQueryCount = new StringBuilder();
                sbQuery.Append(Convert.ToString(ConfigurationManager.AppSettings["DocumentSearchFilterCriteria"]));
                //Build query for account number
                if (!string.IsNullOrEmpty(model.policyNo))
                    model.policyNo = model.policyNo.Trim();
                param0 = BuildQuery(Convert.ToString(model.policyNo));
                //Build query for document date
                param1 = BuildDocumentDate(model.docDate.Date_operand, Convert.ToString(model.docDate.FromDate), Convert.ToString(model.docDate.ToDate));
                //Build the query for ProductCategoryName
                if (model.productCategory == "Any")
                    param2 = " like '%%'";
                else
                    param2 = BuildQuery(model.productCategory);
                //Build the query for document Subtypetype
                if (model.docType == "Select")
                    param3 = " like '%%'";
                else
                    param3 = BuildQuery(model.docType);
                //Build the query for post code
                param4 = BuildQuery(Convert.ToString(model.Postcode));
                //Build the query for page count
                param5 = BuildPageCount(model.pageCount.Page_operand, model.pageCount.startPage, model.pageCount.endPage);
                ////Build the query for spool name
                //param6 = BuildQuery(model.spoolName);
                sbQuery.Replace("{0}", param0);
                sbQuery.Replace("{1}", param1);
                sbQuery.Replace("{2}", param2);
                sbQuery.Replace("{3}", param3);
                sbQuery.Replace("{4}", param4);
                sbQuery.Replace("{5}", param5);
                // sbQuery.Replace("{6}", param6);
                SessionClass.SearchQuery = Convert.ToString(sbQuery);

                string qrderByQuery = GetSortCriteria(model.sortExp1, model.sortExp2, model.sortExp3);
                sbQueryCount.Append(Convert.ToString(ConfigurationManager.AppSettings["DocumentSearchFilterCriteriaOrder"]));
                if (!string.IsNullOrEmpty(qrderByQuery))
                {
                    ViewBag.Message = SessionClass.SearchCountQuery = Convert.ToString(sbQueryCount) + " " + " Order by " + qrderByQuery.Remove(qrderByQuery.Length - 1, 1);
                }
                else
                {
                    string defaultOrderBy = Convert.ToString(ConfigurationManager.AppSettings["DefaultDocumentSearchSortCriteria"]);
                    ViewBag.Message = SessionClass.SearchCountQuery = Convert.ToString(sbQueryCount) + " " + "Order by " + defaultOrderBy;
                }

                return Json(new { ResultMsg = ViewBag.Message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LoggerFactory.Logger.Log("Search", LogEventType.Error, ex.Message);
            }
            return Json(new { errorCode = "-2", errorMsg = ViewBag.Message }, JsonRequestBehavior.AllowGet);
        }



        /// <summary>
        /// Search result get action will help to render search results on view
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SearchResult()
        {
            string searchQuery = string.Empty;
            string countQuery = string.Empty;
            SearchResult searchResult = null;
            if (SessionClass.loggedUser.LoginDetails != null)
            {
                try
                {
                    if (SessionClass.SearchQuery != null)
                    {
                        searchQuery = SessionClass.SearchQuery;
                        countQuery = SessionClass.SearchCountQuery;
                        DocumentSearchParameter searchParameter = new DocumentSearchParameter
                        {
                            Query = searchQuery,
                            CountQuery = countQuery,
                            TenantId = loggedUser.TenantDetails.RecordId,
                            UserEmailId = loggedUser.LoginDetails.EmailId,
                            UserDetails = new WLCaxtonPortalBusinessEntity.User { UserId = loggedUser.LoginDetails.RecordId }
                        };

                        proxy = new WLCaxtonPortalServiceClient();
                        common.AddMessageHeader(proxy);
                        searchResult = proxy.GetDocumentsBySearchCriteria(searchParameter, loggedUser.TenantDetails);
                        proxy.Close();
                        if (String.IsNullOrEmpty(searchResult.MessageDetails.MessageId) == false)
                            ViewBag.maxSearchLimit = Convert.ToString(HttpContext.GetGlobalResourceObject("RSAPortalResource", searchResult.MessageDetails.MessageId));

                        if (searchResult != null)
                        {
                            List<SubDocumentDetails> lstSubDocument = new List<SubDocumentDetails>();
                            DataSet dataset = new DataSet();
                            dataset.Tables.Add(searchResult.SearchRecord);
                            dataset.Tables.Add(searchResult.SearchRecordSubDocument);
                         // DataRelation Relation = new DataRelation("DocSubDocRelation", dataset.Tables[0].Columns["DocumentGUID"], dataset.Tables[1].Columns["DocumentGUID"]);
                         //   dataset.Relations.Add(Relation);
                            DocumentList = dataset;
                            DataTable dt = DocumentList.Tables[0];
                            /*******************LOG Write ************************/
                            using (FileStream fs = new FileStream(HttpContext.Server.MapPath("~/Log/ProgramLog" + "-" + DateTime.Today.ToString("yyyyMMdd") + "." + "txt"), FileMode.Append, FileAccess.Write))
                            using (StreamWriter sw = new StreamWriter(fs))
                            {
                                sw.WriteLine("DocumentList.Tables[0]  .==> " + dt.Rows.Count + "(" + DateTime.Today.ToString("dddd, dd MMMM yyyy hh:mm tt") + ")");
                            }
                            /*******************************************/
                            objDocumentViewModel.MasterdocList = (from DataRow dr in dt.Rows
                                                                  select new MasterDocumentDetail()
                                                                  {
                                                                      AccountNumber = Convert.ToString(dr["AccountNumber"]).Trim(),
                                                                      DocumentGUID = Convert.ToString(dr["DocumentGUID"]).Trim(),
                                                                      DocumentDate = !string.IsNullOrEmpty(Convert.ToString(Convert.ToDateTime(dr["DocumentDate"]).ToShortDateString())) ? (Convert.ToString(Convert.ToDateTime(dr["DocumentDate"]).ToShortDateString())) : "NA",
                                                                      //DocumentDate = !string.IsNullOrEmpty(Convert.ToString(dr["DocumentDate"])) ? CommonExtensionMethods.CommonDateFormate(Convert.ToDateTime(dr["DocumentDate"])) : "NA",
                                                                      DocumentType = Convert.ToString(dr["DocumentType"]).Trim(),
                                                                      PageCount = Convert.ToInt32(dr["PageCount"]),
                                                                      SpoolName = Convert.ToString(dr["SpoolName"]).Trim(),
                                                                      StreamName = Convert.ToString(dr["StreamName"]).Trim(),
                                                                      PageCountHidden = Convert.ToInt32(dr["PageCountHidden"]),
                                                                      pageURL = Convert.ToString(dr["DocumentGUID"]).Trim() + ".pdf",
                                                                      Postcode = Convert.ToString(dr["Postcode"]).Trim(),
                                                                      SubDocumentStartPage=Convert.ToInt32(dr["SubDocumentStartPage"])
                                                                      //pageURL = lstKeys.Where(a => a.BucketName == _bucketName).Select(a => new { a.Url }).ToString()
                                                                  }).ToList();

                            //foreach (DataRow document in dataset.Tables["SearchResult"].Rows)
                            //{
                            //    var subDocLst = dataset.Tables["SubDocumentSearchResult"]
                            //        .AsEnumerable().Where(a => a.Field<string>("DocumentGUID")
                            //        .Equals(Convert.ToString(document["DocumentGUID"]).Trim()))
                            //        .Select(a => new SubDocumentDetails
                            //        {
                            //            DocumentGUID = a.Field<string>("DocumentGUID").Trim(),
                            //            DocumentType = a.Field<string>("DocumentType").Trim(),
                            //            PageCount = a.Field<string>("PageCount").Trim()
                            //        }).ToList();
                                //objDocumentViewModel.masterdocumentDetails = new MasterDocumentDetail
                                //{
                                //    AccountNumber = Convert.ToString(document["AccountNumber"]).Trim(),
                                //    DocumentGUID = Convert.ToString(document["DocumentGUID"]).Trim(),
                                //    DocumentDate = !string.IsNullOrEmpty(Convert.ToString(document["DocumentDate"])) ? CommonExtensionMethods.CommonDateFormate(Convert.ToDateTime(document["DocumentDate"])) : "NA",
                                //    DocumentType = Convert.ToString(document["DocumentType"]).Trim(),
                                //    Postcode = Convert.ToString(document["PostCode"]).Trim(),
                                //    SpoolName = Convert.ToString(document["SpoolName"]).Trim(),
                                //    StreamName = Convert.ToString(document["StreamName"]).Trim(),
                                //    PageCount = Convert.ToInt16(document["PageCount"]),
                                //    PageCountHidden = Convert.ToInt16(document["PageCountHidden"]),
                                //    pageURL = Convert.ToString(document["DocumentGUID"]).Trim() + ".pdf",
                                //};
                                //lstSubDocument.AddRange(subDocLst);

                           // }
                            //objDocumentViewModel.subdocumentDetails = lstSubDocument;

                        }
                    }
                }
                catch (Exception ex)
                {
                    if (proxy != null)
                    {
                        //common.AddMessageHeader(proxy);
                        proxy.Abort();
                    }
                    LoggerFactory.Logger.Log("SearchResult", LogEventType.Error, ex.Message);
                }


                return View("SearchResult", objDocumentViewModel);
            }
            else
            {
                return Redirect(siteroot);
            }
        }

        /// <summary>
        /// This post action will help to show document setails
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ShowDocumentDetails(Document model)
        {
            proxy = new WLCaxtonPortalServiceClient();
            Document DocToView = null;
            try
            {
                User loggedUser = SessionClass.loggedUser;
                common.AddMessageHeader(proxy);
                if (loggedUser != null)
                {
                    DocToView = proxy.GetDocumentByDocumentGuid(model.DocumentGuid, loggedUser.TenantDetails);

                    proxy.Close();
                }
            }
            catch (Exception ex)
            {
                if (proxy != null)
                {
                   // common.AddMessageHeader(proxy);
                    proxy.Abort();
                }

                LoggerFactory.Logger.Log("ShowDocumentDetails", LogEventType.Error, ex.Message);
            }
            return PartialView("_DocumentDetails", DocToView);
        }

        /// <summary>
        /// Show pdf file
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        //public ActionResult ShowPDF(string guid, int pgc, string spn)//Document model)//


        public ActionResult ShowPDF(MasterDocumentDetail model)//
        {
            proxy = new WLCaxtonPortalServiceClient();
            awsProxy = new AWSS3FileTransferClient();
            try
            {
                if (model.DocumentGUID != null)
                {
                    User user = new User() { LoginDetails = new WLCaxtonPortalBusinessEntity.Login() { RecordId = 1 }, UserActivity = UserActivityType.ViewDocumentFirstPage };
                    /////var lstKeys = awsProxy.ListingObjects(null, "", _bucketName, _fileName);
                    //var lstKeys = awsProxy.ListBucketItem(_bucketName, model.DocumentGUID);
                    AWSS3FileTransferClient objAWSS3Cls = new AWSS3FileTransferClient();
                    common.AWSAddMessageHeader(objAWSS3Cls);
                   // AWSS3ServiceLayer.AWSS3Cls objAWSS3Cls = new AWSS3ServiceLayer.AWSS3Cls("");
                    // if (lstKeys != null)
                    if (1 == 0)
                    {
                        var lstKeys = objAWSS3Cls.ListBucketItem(_bucketName, model.DocumentGUID);
                        //ViewBag.Message = PdfRenderPath + lstKeys[0].Url.Trim() + "#page=" + model.StartPageNo;
                        ViewBag.Message = PdfRenderPath + model.DocumentGUID.Trim() + ".pdf#page=" + model.StartPageNo;

                        /*******************LOG Write ************************/
                        using (FileStream fs = new FileStream(HttpContext.Server.MapPath("~/Log/ProgramLog" + "-" + DateTime.Today.ToString("yyyyMMdd") + "." + "txt"), FileMode.Append, FileAccess.Write))
                        using (StreamWriter sw = new StreamWriter(fs))
                        {
                            sw.WriteLine("PdfRenderPath  ==> " + ViewBag.Message);
                            // sw.WriteLine("lstKeys[0].Url  ==> " + lstKeys[0].Url + "; ShowPDF==>ListBucketItem ");
                        }
                        /*******************************************/
                        return Json(new { ResultMsg = ViewBag.Message, errorCode = 0 }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        //List is not found
                        if (awsProxy != null)
                            awsProxy.Abort();
                       
                        string NewfileName = string.Empty;
                        String _path =HttpContext.Server.MapPath(@"~/Document/");
                        string fileName = model.DocumentGUID.Trim() + ".pdf";
                        _path = Path.Combine(_path, fileName);
                        
                        byte[] byteArray = objAWSS3Cls.GetHitachiObject(_bucketName, fileName, _path);
                        if (byteArray != null)
                        {
                            System.IO.File.WriteAllBytes(_path, byteArray);
                            if (model.StartPageNo > 0)
                            {
                                /*******************LOG Write ************************/
                                using (FileStream fs = new FileStream(HttpContext.Server.MapPath("~/Log/ProgramLog" + "-" + DateTime.Today.ToString("yyyyMMdd") + "." + "txt"), FileMode.Append, FileAccess.Write))
                                using (StreamWriter sw = new StreamWriter(fs))
                                {
                                    sw.WriteLine("After call Show PDF-NewfileName  ==> my File path " + _path);
                                    sw.WriteLine("StartPageNo  ==> " + model.StartPageNo);
                                }
                                PartialPdfShow(model, out NewfileName);
                                return Json(new { ResultMsg = NewfileName, errorCode = 0 }, JsonRequestBehavior.AllowGet);

                            }
                            else
                            {
                                return Json(new { ResultMsg = fileName, errorCode = 0 }, JsonRequestBehavior.AllowGet);
                                /*******************LOG Write ************************/
                               


                            }

                        }

                    }
                }
                /// return new FileStreamResult(pdfStream, "application/pdf");
            }
            catch (Exception ex)
            {
                if (proxy != null)
                {
                   // common.AddMessageHeader(proxy);
                    proxy.Abort();
                }
                LoggerFactory.Logger.Log("Show PDF", LogEventType.Error, ex.Message);
            }

            return Json(new { ResultMsg = "", errorCode = -2 }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// This Post action will help to configure search list
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ConfigureSearchList(List<SearchConfigAttribute> model)
        {
            return View();
        }
        /// <summary>
        /// This method store the New Search Configuration result into the database.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult SaveSearchConfig(List<SearchConfigAttribute> model)
        {
            try
            {
                proxy = new WLCaxtonPortalServiceClient();
                User loggedUser = SessionClass.loggedUser;

                AuditTrail auditTrail = new AuditTrail { CreatedBy = loggedUser.LoginDetails.RecordId, CreatedOn = DateTime.Now };
                Tenant tenant = loggedUser.TenantDetails;
                User user = new WLCaxtonPortalBusinessEntity.User { UserId = loggedUser.LoginDetails.RecordId, TenantDetails = tenant };

                List<SearchConfigAttribute> searchConfigAttributes = GetSearchConfigAttribute(model);

                SearchParameters searchParameters = new SearchParameters
                {
                    SearchConfigAttributes = searchConfigAttributes
                };
                common.AddMessageHeader(proxy);
                Message message = proxy.SaveConfigureList(searchParameters, auditTrail, user);

                ViewBag.ErrMessage = Convert.ToString(HttpContext.GetGlobalResourceObject("RSAPortalResource", message.MessageId));

                return RedirectToAction("Search");
            }
            catch (FaultException<ServiceException> fault)
            {
                LoggerFactory.Logger.Log("ConfigureSearchList", LogEventType.Error, fault.Message);
                if (proxy != null)
                {

                   // common.AddMessageHeader(proxy);
                    proxy.Abort();
                }
                throw;
            }
            //If service is down or some communication channel exception
            catch (CommunicationException commException)
            {
                string msg = "Service is not running, Error: -" + commException.Message;
                LoggerFactory.Logger.Log("ConfigureSearchList", LogEventType.Error, msg);
                if (proxy != null)
                {
                  //  common.AddMessageHeader(proxy);
                    proxy.Abort();
                }
                throw;
            }
        }

        #region "Private Methods ==> "
        /// <summary>
        /// Build dynamic query
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string BuildQuery(string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return " like '%%'";
            }
            else if (value.EndsWith("*"))
            {
                value = value.Replace("*", "");
                return "like '" + value + "%'";
            }
            else if (value.IndexOf("*") == -1)
            {
                return " = '" + value + "'";
            }
            return String.Empty;
        }

        /// <summary>
        /// Build document data
        /// </summary>
        /// <param name="operand"></param>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <returns></returns>
        private string BuildDocumentDate(string operand, string FromDate, string ToDate)
        {
            if (string.IsNullOrEmpty(operand))
            {
                return " between '" + Extension.DateToDBFormat(DateTime.Now.AddYears(-100)) + "'" + " AND " + "'" + Extension.DateToDBFormat(DateTime.MaxValue) + "'";
            }
            else if (operand.ToLower().Contains("between"))
            {
                //return ddlDateOperands.SelectedValue + "'" + dpFromDate.SelectedDate.DateToDBFormat() + "'" + " AND " + "'" + dpToDate.SelectedDate.DateToDBFormat() + "'";
               // return operand + "'" + Convert.ToDateTime(FromDate).AddDays(1).DateToDBFormat() + "'" + " AND " + "'" + Convert.ToDateTime(ToDate).AddDays(-1).DateToDBFormat() + "'";
             return operand + "'" + Convert.ToDateTime(FromDate).DateToDBFormat() + "'" + " AND " + "'" + Convert.ToDateTime(ToDate).DateToDBFormat() + "'";
            }
            else
            {
                return operand + "'" + Convert.ToDateTime(FromDate).DateToDBFormat() + "'";
            }
        }
        /// <summary>
        /// Build page count
        /// </summary>
        /// <param name="operand"></param>
        /// <param name="startPageCount"></param>
        /// <param name="endPageCount"></param>
        /// <returns></returns>
        private string BuildPageCount(string operand, int startPageCount, int endPageCount)
        {
            if (string.IsNullOrEmpty(operand))
            {
                return " like '%%'";
            }
            else if (operand.ToLower().Contains("between"))
            {
                //return ddlPageCountOprands.SelectedValue + txtPageCountStart.Text.Trim() + " AND " + txtPageCountEnd.Text.Trim();
                return operand + Convert.ToString((Convert.ToInt32(startPageCount) + 1)) + " AND " + Convert.ToString((Convert.ToInt32(endPageCount) - 1));

            }
            else
            {
                return operand + Convert.ToString(startPageCount);
            }
        }
        /// <summary>
        /// Make sorting criteria
        /// </summary>
        /// <param name="ex1"></param>
        /// <param name="ex2"></param>
        /// <param name="ex3"></param>
        /// <returns></returns>
        private string GetSortCriteria(SortExpression ex1, SortExpression ex2, SortExpression ex3)
        {
            string qrderByQuery = string.Empty;
            string SortAsc = string.Empty;
            Dictionary<string, string> sortCriteriaCollection = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(ex1.FieldName))
            {
                SortAsc = ex1.OrderBy;
                qrderByQuery = qrderByQuery + ex1.FieldName + " " + SortAsc + " ,";
                sortCriteriaCollection.Add(ex1.FieldName, SortAsc);
            }
            if (!string.IsNullOrEmpty(ex2.FieldName))
            {
                SortAsc = ex2.OrderBy;
                qrderByQuery = qrderByQuery + ex2.FieldName + " " + SortAsc + " ,";
                sortCriteriaCollection.Add(ex2.FieldName, SortAsc);
            }
            if (!string.IsNullOrEmpty(ex3.FieldName))
            {
                SortAsc = ex3.OrderBy;
                qrderByQuery = qrderByQuery + ex3.FieldName + " " + SortAsc + " ,";
                sortCriteriaCollection.Add(ex3.FieldName, SortAsc);
            }
            SessionClass.SortCriteriaCollection = sortCriteriaCollection;
            return qrderByQuery;
        }
        /// <summary>
        /// This property will help to generate document list from temp data
        /// </summary>
        private DataSet DocumentList
        {
            get
            {
                DataSet docList = null;
                if (TempData["DocList"] != null)
                {
                    docList = (DataSet)TempData["DocList"];
                    TempData.Peek("DocList");
                }

                return docList;
            }
            set { TempData["DocList"] = value; }
        }



        /// <summary>
        /// This method return the search configuration parameters details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private List<SearchConfigAttribute> GetSearchConfigAttribute(List<SearchConfigAttribute> model)
        {
            List<SearchConfigAttribute> searchConfigAttributes = new List<SearchConfigAttribute>();
            foreach (var item in model)
            {
                var lblParameterId = item.ParameterId;
                var chkVisibility = item.IsVisible;
                var hdnPosition = item.DisplayOrder;
                var chkGrouping = item.IsGroupBy;

                SearchConfigAttribute searchConfigAttribute1 = new SearchConfigAttribute
                {
                    ParameterId = Convert.ToInt32(lblParameterId),
                    DisplayOrder = chkVisibility ? Convert.ToInt32(hdnPosition) : -1,
                    IsVisible = chkVisibility,
                    IsGroupBy = chkGrouping
                };
                searchConfigAttributes.Add(searchConfigAttribute1);
            }
            return searchConfigAttributes;
        }


        #endregion

        /// <summary>
        ///  Generate Report Csv File
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GenerateReportinCSV()
        {
            string searchQuery = string.Empty;
            string countQuery = string.Empty;
            SearchResult searchResult = null;
            List<ReportTableList> _objReportTable = null;
            string _dateTime = DateTime.Now.ToString("dd-MMM-yyyy HHmmss");
            try
            {
                if (SessionClass.SearchQuery != null)
                {
                    searchQuery = SessionClass.SearchQuery;
                    countQuery = SessionClass.SearchCountQuery;
                    DocumentSearchParameter searchParameter = new DocumentSearchParameter
                    {
                        Query = searchQuery,
                        CountQuery = countQuery,
                        TenantId = loggedUser.TenantDetails.RecordId,
                        UserEmailId = loggedUser.LoginDetails.EmailId,
                        UserDetails = new WLCaxtonPortalBusinessEntity.User { UserId = loggedUser.LoginDetails.RecordId }
                    };
                    proxy = new WLCaxtonPortalServiceClient();
                    common.AddMessageHeader(proxy);
                    searchResult = proxy.GetDocumentsBySearchCriteria(searchParameter, loggedUser.TenantDetails);
                    if (String.IsNullOrEmpty(searchResult.MessageDetails.MessageId) == false)
                        ViewBag.maxSearchLimit = Convert.ToString(HttpContext.GetGlobalResourceObject("RSAPortalResource", searchResult.MessageDetails.MessageId));

                    proxy.Close();
                    if (searchResult != null)
                    {
                        DataSet dataset = new DataSet();
                        dataset.Tables.Add(searchResult.SearchRecord);
                        dataset.Tables.Add(searchResult.SearchRecordSubDocument);
                        //DataRelation Relation = new DataRelation("DocSubDocRelation", dataset.Tables[0].Columns["DocumentGUID"], dataset.Tables[1].Columns["DocumentGUID"]);
                        //dataset.Relations.Add(Relation);
                        DocumentList = dataset;
                        _objReportTable = new List<ReportTableList>();
                        DataTable _dataTable = DocumentList.Tables[0];

                        if (_dataTable.Rows.Count > 0)
                        {

                            string[] _usr = (loggedUser.LoginDetails.EmailId).Split('@');
                            _objReportTable.Add(new ReportTableList() { ReportTable = _dataTable, ReportName = (_usr[0] + "_REMARKS_NOT_RECEIVED_" + _dateTime).ToUpper() });
                        }
                        objDocumentViewModel.MasterdocList = (from DataRow dr in _dataTable.Rows
                                                              select new MasterDocumentDetail()
                                                              {
                                                                  AccountNumber = Convert.ToString(dr["AccountNumber"]),
                                                                  DocumentGUID = dr["DocumentGUID"].ToString(),
                                                                  DocumentDate = !string.IsNullOrEmpty(Convert.ToString(dr["DocumentDate"])) ? CommonExtensionMethods.CommonDateFormate(Convert.ToDateTime(dr["DocumentDate"])) : "NA",
                                                                  DocumentType = Convert.ToString(dr["DocumentType"]),
                                                                  PageCount = Convert.ToInt32(dr["PageCount"]),
                                                                  SpoolName = Convert.ToString(dr["SpoolName"]),
                                                                  StreamName = Convert.ToString(dr["StreamName"]),
                                                                  PageCountHidden = Convert.ToInt32(dr["PageCountHidden"]),
                                                                  Postcode = Convert.ToString(dr["Postcode"])
                                                              }).ToList();
                    }
                    var jsonConvList = JsonConvert.SerializeObject(_objReportTable, Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore });
                    return Content(jsonConvList, "application/json");
                }
                return Json(new { error = "-2" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                LoggerFactory.Logger.Log("ConfigureSearchList", LogEventType.Error, ex.Message);
                return Json(new { error = "-2" }, JsonRequestBehavior.AllowGet);
            }

        }

        //for test
        [HttpPost]
        public ActionResult Screenreport(MasterDocumentDetail model)
        {
            string NewfileName = string.Empty;
            String _path = HttpContext.Server.MapPath(@"~/Document/");
            string fileName = model.DocumentGUID.Trim() + ".pdf";
            _path = Path.Combine(_path, fileName);
            //split to pDF in page range.
            if (model.StartPageNo > 0)
            {
               PartialPdfShow(model, out NewfileName);
                using (FileStream fs = new FileStream(HttpContext.Server.MapPath("~/Log/ProgramLog" + "-" + DateTime.Today.ToString("yyyyMMdd") + "." + "txt"), FileMode.Append, FileAccess.Write))
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine("NewfileName  ==> my File path " + NewfileName);
                    sw.WriteLine("StartPageNo  ==> " + model.StartPageNo);
                }
                return Json(new { ResultMsg = NewfileName, errorCode = 0 }, JsonRequestBehavior.AllowGet);

            }
            else
            {
                return Json(new { ResultMsg = fileName, errorCode = 0 }, JsonRequestBehavior.AllowGet);

            }
        }


         public string PartialPdfShow(MasterDocumentDetail model, out String fileName)
        {
            string pdfTronKey = System.Configuration.ConfigurationManager.AppSettings["PDFNetLicenceKey"].ToString();
            fileName = string.Empty;
            string filePath = string.Empty;
            String _path = string.Empty;
            //string fileName = string.Empty;
            try
            {
                filePath = _path = HttpContext.Server.MapPath(@"~/Document/");
                fileName = model.DocumentGUID.Trim() + ".pdf";
                _path = Path.Combine(_path, fileName);

                /*******************LOG Write ************************/
                //using (FileStream fs = new FileStream(HttpContext.Server.MapPath("~/Log/ProgramLog" + "-" + DateTime.Today.ToString("yyyyMMdd") + "." + "txt"), FileMode.Append, FileAccess.Write))
                //using (StreamWriter sw = new StreamWriter(fs))
                //{
                //    sw.WriteLine("PartialPdfShow_path  ==> " + _path);
                //}
                /********************PDF Tron ***********************/
                int num = model.StartPageNo + model.PageCount - 1;
                if (num >= 0)
                {


                    using (pdftron.PDF.PDFDoc pDFDoc = new pdftron.PDF.PDFDoc(_path))
                    {
                        pDFDoc.InitSecurityHandler();
                        using (pdftron.PDF.PDFDoc pDFDoc2 = new pdftron.PDF.PDFDoc())
                        {

                            pdftron.PDF.PageIterator pageIterator = pDFDoc.GetPageIterator(model.StartPageNo);

                            ArrayList import_list = new ArrayList();
                            for (int i = 0; i < model.PageCount; i++)
                            {
                                import_list.Add(pageIterator.Current());
                                if (pageIterator.HasNext())
                                {
                                    pageIterator.Next();
                                }
                            }
                            ArrayList imported_pages = pDFDoc2.ImportPages(import_list);
                            for (int i = 0; i != imported_pages.Count; ++i)
                            {
                                pDFDoc2.PagePushBack((pdftron.PDF.Page)imported_pages[i]);
                            }


                            //for (int i = 0; i < model.PageCount; i++)
                            //{
                            //    /*******************LOG Write ************************/
                            //    //using (FileStream fs = new FileStream(HttpContext.Server.MapPath("~/Log/ProgramLog" + "-" + DateTime.Today.ToString("yyyyMMdd") + "." + "txt"), FileMode.Append, FileAccess.Write))
                            //    //using (StreamWriter sw = new StreamWriter(fs))
                            //    //{
                            //    //    sw.WriteLine("pageIterator_model.PageCount  ==> " + model.PageCount);
                            //    //}
                            //    /*******************LOG Write ************************/
                            //    pDFDoc2.PagePushBack(pageIterator.Current());
                            //    if (i < model.PageCount - 1)
                            //    {
                            //        pageIterator.Next();
                            //    }
                            //}



                            fileName = model.DocumentGUID.Trim() + Convert.ToString(model.StartPageNo) + "_" + Convert.ToString(model.StartPageNo + model.PageCount) + ".pdf";
                            pDFDoc2.Save(Path.Combine(filePath, fileName), pdftron.SDF.SDFDoc.SaveOptions.e_linearized);
                            pDFDoc2.Close();
                            pDFDoc2.Dispose();
                        }
                       

                    }
                }



                        /*******************LOG Write ************************/
                    
                    //}
                
            }
            catch (pdftron.Common.PDFNetException e)
            {
                using (FileStream fs = new FileStream(HttpContext.Server.MapPath("~/Log/ProgramLog" + "-" + DateTime.Today.ToString("yyyyMMdd") + "." + "txt"), FileMode.Append, FileAccess.Write))
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine("pdftron _fileName  ==> " + e.Message);
                }
                LoggerFactory.Logger.Log("ShowPDFReport", LogEventType.Error, e.Message);

            }
            catch (Exception ex)
            {
                LoggerFactory.Logger.Log("ShowPDFReport", LogEventType.Error, ex.Message);

            }
            /*******************************************/
            return fileName;

        }

        [DeleteFileAttribute]
        public ActionResult PDFExport(String file)
        {
            String _path = HttpContext.Server.MapPath(@"~/Document/");
            String fileName = String.Empty;
            fileName = file.Trim();
            _path = Path.Combine(_path, fileName);
            try
            {
                var contentDispositionHeader = new System.Net.Mime.ContentDisposition
                {
                    Inline = true,
                    FileName = fileName
                };
                Response.BufferOutput = true;
                Response.Headers.Add("Content-Disposition", contentDispositionHeader.ToString());
               

            }
            catch (Exception ex)
            {
                LoggerFactory.Logger.Log("PDFExport", LogEventType.Error, ex.Message);
            }
            return File(_path, System.Net.Mime.MediaTypeNames.Application.Pdf);
            //  return File(_path, "text/xml", fileName);
        }



    }
}