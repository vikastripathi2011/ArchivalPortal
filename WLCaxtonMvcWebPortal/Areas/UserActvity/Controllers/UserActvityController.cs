using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using WLCaxtonMvcWebPortal.Models;
using WLCaxtonMvcWebPortal.Services;
using WLCaxtonMvcWebPortal.Util;
using WLCaxtonMvcWebPortal.ViewModel;
using WLCaxtonPortalBusinessEntity;
using WLCaxtonPortalBusinessEntity.Classes;
using WLCaxtonPortalExceptionLogger;
using WLCaxtonMvcWebPortal.Code;

namespace WLCaxtonMvcWebPortal.Areas.UserActvity.Controllers
{
    [CheckCurrentLoggedUserSession]
    public class UserActvityController : Controller
    {
        #region "Object Declration ==>>"
        public readonly string siteroot = System.Configuration.ConfigurationManager.AppSettings["siteroot"] == null ? " " : System.Configuration.ConfigurationManager.AppSettings["siteroot"];
        DocumentViewModel objDocumentViewModel = new DocumentViewModel();
        User loggedUser = SessionClass.loggedUser;
        #endregion

        WLCaxtonPortalServiceClient proxy;
        Common common;
        /// <summary>
        /// This get action will help to render serach page
        /// </summary>

        public UserActvityController()
        {
            common = new Common();
        }
        public ActionResult Search()
        {
            if (SessionClass.loggedUser.LoginDetails != null)
            {
                proxy = new WLCaxtonPortalServiceClient();
                try
                {
                    common.AddMessageHeader(proxy);
                    IList<DocumentType> documentTypes = proxy.GetDocumentTypes();
                    IList<ProductCategory> productCategories = proxy.GetProductCategories();
                   // productCategories.Insert(0, new ProductCategory { ProductCategoryName="",ProductCategoryDescription="Select" });
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

                    LoggerFactory.Logger.Log("Search", LogEventType.Error, ex.Message);

                }
                return View();
            }
            else
            {
                return Redirect(siteroot);

            }
        }

        /// <summary>
        /// This get action will help to render serach view
        /// </summary>
        public ActionResult SearchView()
        {
            return View();
        }

        /// <summary>
        /// This get action will help to render serach result
        /// </summary>
        public ActionResult SearchResult()
        {
            return View();
        }

        /// <summary>
        /// Configure search list partial view
        /// </summary>
        [HttpPost]
        public ActionResult ConfigureSearchList()
        {
            if (SessionClass.loggedUser.LoginDetails != null)
            {
                proxy = new WLCaxtonPortalServiceClient();
                IList<SearchConfigAttribute> searchConfigAttributes = null;
                try
                {
                    common.AddMessageHeader(proxy);
                    User loggedUser = SessionClass.loggedUser;
                    User user = new User { UserId = loggedUser.LoginDetails.RecordId };
                    DocumentSearchParameter docParameter = new DocumentSearchParameter { RecordId = loggedUser.LoginDetails.RecordId, UserDetails = user };
                    searchConfigAttributes = proxy.GetSearchConfigParameters(docParameter);
                    int count = searchConfigAttributes.Where(q => q.IsVisible == true).ToList().Count;
                    List<SelectListItem> diplayOrder = new List<SelectListItem>();
                    for (Int32 i = 0; i < count; i++)
                    {
                        diplayOrder.Add(new SelectListItem { Text = Convert.ToString((i + 1)), Value = Convert.ToString((i + 1)) });
                    }
                    ViewBag.DisplayOrder = diplayOrder;
                   CommonBussinesLogic.EnableDisableGroup(searchConfigAttributes);
                    proxy.Close();


                }
                catch (Exception ex)
                {
                    if (proxy != null)
                    {
                        common.AddMessageHeader(proxy);
                        proxy.Abort();
                    }
                    LoggerFactory.Logger.Log("ConfigureSearchList", LogEventType.Error, ex.Message);
                }

                return PartialView("_ConfigureSearchList", searchConfigAttributes);
            }
            else
            {
                return Redirect(siteroot);

            }
        }

       

        /// <summary>
        /// This action will render document details
        /// </summary>
        /// <returns></returns>
        public ActionResult DocumentDetails()
        {
            return View();
        }

        /// <summary>
        /// This action will help to render Userlist in popup for print
        /// </summary>
        /// <returns></returns>
        public ActionResult PopupPrintUserList()
        {
            return View();
        }

        /// <summary>
        /// This function will help to bind the result dynamiclly
        /// </summary>
        /// <returns></returns>
        public ActionResult DynamicRwsColBind()
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
                        if (searchResult != null)
                        {
                            DataSet dataset = new DataSet();
                            dataset.Tables.Add(searchResult.SearchRecord);
                            dataset.Tables.Add(searchResult.SearchRecordSubDocument);
                            DataRelation Relation = new DataRelation("DocSubDocRelation", dataset.Tables[0].Columns["DocumentGUID"], dataset.Tables[1].Columns["DocumentGUID"]);
                            dataset.Relations.Add(Relation);
                            DocumentList = dataset;
                            DataTable dt = DocumentList.Tables[0];

                            MasterDocumentDetail objMasterDocumentDetail = new MasterDocumentDetail();
                            List<string> GetColumns = dt.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList();
                            dt.SetColumnsOrder(GetColumns.ToArray());

                            objDocumentViewModel.MasterdocColumnList = (from DataColumn dc in dt.Columns
                                                                        select new MasterDocumentDetail
                                                                        {
                                                                            DyanamicColumn = Convert.ToString(dc)
                                                                        }).ToList();
                            for (Int32 i = 0; i < dt.Rows.Count; i++)
                            {
                                objMasterDocumentDetail.DyanamicRows = Convert.ToString(dt.Rows[i].ItemArray);
                            }

                            WebGridClass objWebGridClass = new WebGridClass();

                            List<WebGridColumn> columns = new List<WebGridColumn>();
                            columns.Add(new WebGridColumn() { ColumnName = Convert.ToString(objDocumentViewModel.MasterdocColumnList[0].DyanamicColumn), Header = Convert.ToString(objDocumentViewModel.MasterdocColumnList[0].DyanamicColumn), CanSort = true });
                            columns.Add(new WebGridColumn() { ColumnName = Convert.ToString(objDocumentViewModel.MasterdocColumnList[1].DyanamicColumn), Header = Convert.ToString(objDocumentViewModel.MasterdocColumnList[1].DyanamicColumn), CanSort = true });
                            columns.Add(new WebGridColumn() { ColumnName = Convert.ToString(objDocumentViewModel.MasterdocColumnList[2].DyanamicColumn), Header = Convert.ToString(objDocumentViewModel.MasterdocColumnList[2].DyanamicColumn), CanSort = true });
                            columns.Add(new WebGridColumn() { ColumnName = Convert.ToString(objDocumentViewModel.MasterdocColumnList[3].DyanamicColumn), Header = Convert.ToString(objDocumentViewModel.MasterdocColumnList[3].DyanamicColumn), CanSort = true });
                            columns.Add(new WebGridColumn() { ColumnName = Convert.ToString(objDocumentViewModel.MasterdocColumnList[4].DyanamicColumn), Header = Convert.ToString(objDocumentViewModel.MasterdocColumnList[4].DyanamicColumn), CanSort = true });
                            columns.Add(new WebGridColumn() { ColumnName = Convert.ToString(objDocumentViewModel.MasterdocColumnList[5].DyanamicColumn), Header = Convert.ToString(objDocumentViewModel.MasterdocColumnList[5].DyanamicColumn), CanSort = true });
                            columns.Add(new WebGridColumn() { ColumnName = Convert.ToString(objDocumentViewModel.MasterdocColumnList[6].DyanamicColumn), Header = Convert.ToString(objDocumentViewModel.MasterdocColumnList[6].DyanamicColumn), CanSort = true });
                            //columns.Add(new WebGridColumn() { Format = (item) => { return new HtmlString(string.Format("<a href= {0}>View</a>", Url.Action("Edit", "Edit", new { Id = item.Id }))); } });
                            ViewBag.Columns = columns;
                            objWebGridClass.ColNames = columns;
                            objDocumentViewModel.WebGridClassModel = objWebGridClass = WebGridClass.HoldWebGridDetails;

                            objDocumentViewModel.MasterdocList = (from DataRow dr in dt.Rows
                                                                  select new MasterDocumentDetail()
                                                                  {
                                                                      AccountNumber = Convert.ToString(dr["AccountNumber"]),
                                                                      DocumentGUID = Convert.ToString(dr["DocumentGUID"]),
                                                                      DocumentDate = Convert.ToString(dr["DocumentDate"]),
                                                                      DocumentType = Convert.ToString(dr["DocumentType"]),
                                                                      PageCount = Convert.ToInt32(dr["PageCount"]),
                                                                      SpoolName = Convert.ToString(dr["SpoolName"]),
                                                                      StreamName = Convert.ToString(dr["StreamName"]),
                                                                      PageCountHidden = Convert.ToInt32(dr["PageCountHidden"]),
                                                                      Postcode = Convert.ToString(dr["Postcode"])
                                                                  }).ToList();

                            foreach (DataRow document in dataset.Tables["SearchResult"].Rows)
                            {
                                objDocumentViewModel.subdocumentDetails = dataset.Tables["SubDocumentSearchResult"]
                                    .AsEnumerable().Where(a => a.Field<string>("DocumentGUID")
                                    .Equals(Convert.ToString(document["DocumentGUID"])))
                                    .Select(a => new SubDocumentDetails
                                    {
                                        DocumentGUID = a.Field<string>("DocumentGUID"),
                                        DocumentType = a.Field<string>("DocumentType"),
                                        PageCount = a.Field<string>("PageCount")
                                    }).ToList();
                                objDocumentViewModel.masterdocumentDetails = new MasterDocumentDetail
                                {
                                    AccountNumber = Convert.ToString(document["AccountNumber"]),
                                    DocumentGUID = Convert.ToString(document["DocumentGUID"]),
                                    DocumentDate = Convert.ToString(document["DocumentDate"]),
                                    DocumentType = Convert.ToString(document["DocumentType"]),
                                    Postcode = Convert.ToString(document["PostCode"]),
                                    SpoolName = Convert.ToString(document["SpoolName"]),
                                    StreamName = Convert.ToString(document["StreamName"]),
                                    PageCount = Convert.ToInt16(document["PageCount"]),
                                    PageCountHidden = Convert.ToInt16(document["PageCountHidden"])
                                };
                                //this will work while  DB document id in AWSS3 document ID and Bucket id are mapped..!!
                            }

                        }

                    }
                }
                catch (Exception ex)
                {
                    if (proxy != null)
                    {
                        common.AddMessageHeader(proxy);
                        proxy.Abort();
                    }

                    LoggerFactory.Logger.Log("DynamicRwsColBindView", LogEventType.Error, ex.Message);
                }

                return View("DynamicRwsColBindView", objDocumentViewModel);
            }
            else
            {
                return Redirect(siteroot);

            }
        }

        /// <summary>
        /// This property will help to return document list in dataset from TempData
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
        /// This action will responsible to generate generic grid
        /// </summary>
        /// <returns></returns>
        public ActionResult GenericGrid()
        {
           
            WebGrid grid = new WebGrid();
            WebGridColumn col1 = grid.Column("BookId", "", (item) => CommonBussinesLogic.GetEditButtons(item), "col1", true);
            WebGridColumn col2 = grid.Column("Book Name", "", (item) => CommonBussinesLogic.GetEditButtons(item), "col2", true);
            WebGridColumn col3 = grid.Column("Subject", "", (item) => CommonBussinesLogic.GetEditButtons(item), "col3", true);
            // ... 2-6 initialized also ...

            List<WebGridColumn> columnSet = new List<WebGridColumn>() { col1, col2, col3, };
            grid.Columns(columnSet.ToArray());
            ViewBag.GridCols = columnSet;
            ViewBag.Grid = grid;

            return View();
        }

       







    }
}