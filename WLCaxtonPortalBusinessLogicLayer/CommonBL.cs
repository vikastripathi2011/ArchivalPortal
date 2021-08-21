//==========================================================================================
// Project              : WLCaxtonWebPortal
// File Name            : CommonBL.cs
// Program Description  : All Common functionality of application.
// Programmed By        : Naushad Ali,Nadeem Ishrat.
// Programmed On        : 12-December-2012
// Version              : 1.0.0
//==========================================================================================
#region Namespaces
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using WLCaxtonPortalBusinessEntity;
using WLCaxtonPortalBusinessEntity.Classes;
using WLCaxtonPortalDataLayer;
using WLCaxtonPortalExceptionLogger;
#endregion

namespace WLCaxtonPortalBusinessLogicLayer
{
    /// <summary>
    /// This class is used for all Common functionality of application.
    /// </summary>
    public static class CommonBL
    {
        /// <summary>
        /// To get the Product Categories List
        /// </summary>
        /// <returns></returns>
        public static IList<ProductCategory> GetProductCategories()
        {
            try
            {
                return CommonDL.GetProductCategories();
            }
            catch (Exception ex)
            {
                LoggerFactory.Logger.Log("CommonBL", LogEventType.Error, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// To get the Document Types List
        /// </summary>
        /// <returns></returns>
        public static IList<DocumentType> GetDocumentTypes()
        {
            try
            {
                return CommonDL.GetDocumentTypes();
            }
            catch (Exception ex)
            {
                LoggerFactory.Logger.Log("CommonBL", LogEventType.Error, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// To get the Documents List By Search Criteria
        /// </summary>
        /// <param name="docParameter"></param>
        /// <param name="tenant"></param>
        /// <returns></returns>
        public static SearchResult GetDocumentsBySearchCriteria(DocumentSearchParameter docParameter, Tenant tenant)
        {
            try
            {
                Message message = new Message();
                string whereCountQuery = docParameter.CountQuery;
                string whereClause = docParameter.Query;
                DataTable dtAllResults;
                int maxSearchLimit = int.Parse(System.Configuration.ConfigurationManager.AppSettings["MaxSearchLimit"]);

                //by VRT
                DataTable dtCountResults = CommonDL.GetDocumentsCountSearchQuery(docParameter, tenant, maxSearchLimit, "SearchCountQuery", string.Empty, whereClause);
                if (dtCountResults != null && dtCountResults.Rows.Count > 0)
                {
                    if (Convert.ToInt32(dtCountResults.Rows[0][0]) > 0)
                    {
                        if (Convert.ToInt32(dtCountResults.Rows[0][0]) >= maxSearchLimit)
                        {
                            message.Description = "Your search criteria has crossed the configured limit hence modify the criteria. The result displayed has been trimmed to meet the configured limit.";
                            message.MessageId = "M00069";
                        }

                    }
                }
                dtAllResults = CommonDL.GetDocumentsCountSearchQuery(docParameter, tenant, maxSearchLimit, "SearchQuery", whereClause, whereCountQuery);


                docParameter.Query = "";
                if (dtAllResults != null && dtAllResults.Rows.Count > 0)
                {
                    // if (Convert.ToInt32(dtCountResults.Rows[0][0]) > 0)
                    // {
                    //if (Convert.ToInt32(dtCountResults.Rows[0][0]) > maxSearchLimit)
                    //{
                    //    // By VRT 
                    //    dtAllResults = CommonDL.GetDocumentsCountSearchQuery(docParameter, tenant, maxSearchLimit, "OptimizedSearchQuery", whereClause, whereCountQuery);
                    //    message.Description = "Your search criteria has crossed the configured limit hence modify the criteria. The result displayed has been trimmed to meet the configured limit.";
                    //    message.MessageId = "M00069";
                    //}
                    //else
                    //{
                    //    // By VRT 
                    //    dtAllResults = CommonDL.GetDocumentsCountSearchQuery(docParameter, tenant, maxSearchLimit, "SearchQuery", whereClause, whereCountQuery);
                    //}
                    DataTable dtUniqueResults = dtAllResults.Clone();
                    DataTable dtFinalResults = dtAllResults.Copy();

                    dtUniqueResults = dtAllResults.DefaultView.ToTable(true, "ProductCategoryId");
                    //if (dtUniqueResults.Rows.Count >= maxSearchLimit)
                    //{
                    //    message.Description = "Your search criteria has crossed the configured limit hence modify the criteria. The result displayed has been trimmed to meet the configured limit.";
                    //    message.MessageId = "M00069";
                    //}
                    //Current search limit is set to 1000 in server web.config
                    /**************************** Logically this is not required, Commented BY: VRT ******************************************
                                        if (dtUniqueResults.Rows.Count > maxSearchLimit)
                                        {
                                            dtUniqueResults = dtUniqueResults.AsEnumerable().Take(maxSearchLimit).CopyToDataTable();

                                            var matched = from table1 in dtFinalResults.AsEnumerable()
                                                          join table2 in dtUniqueResults.AsEnumerable() on
                                                          table1.Field<int>("ProductCategoryId") equals table2.Field<int>("ProductCategoryId")
                                                          where table1.Field<int>("ProductCategoryId") == table2.Field<int>("ProductCategoryId")
                                                          select table1;
                                            if (matched.Count() > 0)
                                            {
                                                dtAllResults.Clear();
                                                int i = 0;
                                                foreach (var item in matched)
                                                {
                                                    dtAllResults.ImportRow(item.Table.Rows[i]);
                                                    i++;
                                                }
                                            }
                                            message.Description = "Your search criteria has crossed the configured limit hence modify the criteria. The result displayed has been trimmed to meet the configured limit.";
                                            message.MessageId = "M00069";
                                        }
                                        */
                    IList<SearchConfigAttribute> SearchConfigAttributes = GetSearchConfigParameters(docParameter);
                    DataTable dtSearchResults = new DataTable("SearchResult");

                    //Build the table columns based on search config parameters
                    DataColumn column = null;
                    foreach (SearchConfigAttribute attribute in SearchConfigAttributes)
                    {
                        if (attribute.IsVisible)
                        {
                            column = new DataColumn();
                            column.Caption = attribute.ParameterName;
                            column.ColumnName = attribute.ParameterDescription;

                            if (attribute.ParameterDescription == "PageCount")
                                column.DataType = typeof(Int32);
                            if (attribute.ParameterDescription == "DocumentDate")
                                column.DataType = typeof(DateTime);

                            dtSearchResults.Columns.Add(column);
                        }
                    }

                    column = new DataColumn("DocumentGUID", typeof(string));
                    dtSearchResults.Columns.Add(column);
                    DataColumn columnPageCount = new DataColumn("PageCountHidden", typeof(int));
                    dtSearchResults.Columns.Add(columnPageCount);

                    DataColumn SubDocumentStartPage = new DataColumn("SubDocumentStartPage", typeof(int));
                    dtSearchResults.Columns.Add(SubDocumentStartPage);

                    DataColumn[] dcPrimary = new DataColumn[1];
                    dcPrimary[0] = dtSearchResults.Columns["DocumentGUID"];
                    dtSearchResults.PrimaryKey = dcPrimary;

                    string uniqueGuid = string.Empty;
                    bool IsDuplicateDocGUID = false;
                    //Insert the data into dtSearchResults table
                    for (Int32 rowIndex = 0; rowIndex < dtAllResults.Rows.Count; rowIndex++)
                    {
                        IsDuplicateDocGUID = false;
                        DataRow row = dtSearchResults.NewRow();
                        foreach (SearchConfigAttribute attribute in SearchConfigAttributes)
                        {
                            uniqueGuid = Convert.ToString(dtAllResults.Rows[rowIndex]["DocumentGUID"]);

                            if (dtSearchResults.Rows.Contains(uniqueGuid))
                            {
                                IsDuplicateDocGUID = true;
                                break;
                            }
                            else
                            {
                                if (attribute.IsVisible)
                                {
                                    row[attribute.ParameterDescription] = dtAllResults.Rows[rowIndex][attribute.ParameterDescription];
                                    row["DocumentGUID"] = dtAllResults.Rows[rowIndex]["DocumentGUID"];
                                    row["PageCountHidden"] = dtAllResults.Rows[rowIndex]["PageCount"];
                                    row["SubDocumentStartPage"] = dtAllResults.Rows[rowIndex]["SubDocumentStartPage"];
                                }
                            }
                        }

                        if (!IsDuplicateDocGUID)
                            dtSearchResults.Rows.Add(row);
                    }
                    string groupByColumnName = string.Empty;
                    string GridHeaderText = string.Empty;
                    var grp = from g in SearchConfigAttributes where g.IsGroupBy == true select g;
                    if (grp.Count() > 0)
                    {
                        groupByColumnName = grp.FirstOrDefault().ParameterDescription;
                        GridHeaderText = grp.FirstOrDefault().ParameterName;
                    }

                    // Sub document details
                    DataTable dtSubdocument = new DataTable("SubDocumentSearchResult");
                    //dtSubdocument.Columns.Add("DocumentGUID");
                    //dtSubdocument.Columns.Add("DocumentType");
                    //dtSubdocument.Columns.Add("SubDocumentStartPage");
                    //dtSubdocument.Columns.Add("PageCount");

                    //dcPrimary = new DataColumn[1];
                    //dcPrimary[0] = dtSubdocument.Columns["ID"];
                    //dtSubdocument.PrimaryKey = dcPrimary;

                    //for (Int32 rowCount = 0; rowCount < dtAllResults.Rows.Count; rowCount++)
                    //{
                    //    DataRow drNew = dtSubdocument.NewRow();
                    //    IsDuplicateDocGUID = false;
                    //    if (Convert.ToInt32(dtAllResults.Rows[rowCount]["SubDocumentStartPage"]) > -1)
                    //    {
                    //        IsDuplicateDocGUID = true;
                    //        drNew["DocumentType"] = Convert.ToString(dtAllResults.Rows[rowCount]["SubDocumentName"]);
                    //        drNew["SubDocumentStartPage"] = Convert.ToString(dtAllResults.Rows[rowCount]["SubDocumentStartPage"]);
                    //        drNew["DocumentGUID"] = Convert.ToString(dtAllResults.Rows[rowCount]["DocumentGUID"]);
                    //        drNew["PageCount"] = CalculateSubDocPageCount(dtAllResults, rowCount);
                    //    }
                    //    if (IsDuplicateDocGUID)
                    //        dtSubdocument.Rows.Add(drNew);
                    //}
                    return new SearchResult
                    {
                        SearchRecord = dtSearchResults,
                        SearchRecordSubDocument = dtSubdocument,
                        GroupByColumnName = groupByColumnName,
                        GridHeaderText = GridHeaderText,
                        MessageDetails = message
                    };
                    //}
                    //else
                    //{
                    //    return new SearchResult
                    //    {
                    //        SearchRecord = new DataTable(),
                    //        SearchRecordSubDocument = new DataTable(),
                    //        GroupByColumnName = "",
                    //        GridHeaderText = "",
                    //        MessageDetails = message
                    //    };
                    //}

                }
                else
                {
                    return new SearchResult
                    {
                        SearchRecord = new DataTable(),
                        SearchRecordSubDocument = new DataTable(),
                        GroupByColumnName = "",
                        GridHeaderText = "",
                        MessageDetails = message
                    };
                }
            }
            catch (Exception ex)
            {
                LoggerFactory.Logger.Log("CommonBL", LogEventType.Error, ex.Message);
                throw;
            }
        }

        private static int CalculateSubDocPageCount(DataTable dtAllResults, int rowCount)
        {
            int subDocPageCount = 0;
            DataRow[] foundRow = dtAllResults.Select("DocumentGUID = '" + Convert.ToString(dtAllResults.Rows[rowCount]["DocumentGUID"]) + "'");

            if (foundRow.Count() == 1)
            {
                subDocPageCount = Convert.ToInt32(foundRow[0]["PageCount"]) - Convert.ToInt32(foundRow[0]["SubDocumentStartPage"]) + 1;
            }
            else if (foundRow.Count() > 1)
            {
                foundRow = foundRow.OrderBy(q => q.Field<int>("SubDocumentStartPage")).ToArray();

                for (Int32 i = 0; i < foundRow.Count(); i++)
                {
                    if (Convert.ToInt32(foundRow[i]["SubDocumentId"]) == Convert.ToInt32(dtAllResults.Rows[rowCount]["SubDocumentId"]))
                    {
                        if (i + 1 != foundRow.Count())
                            subDocPageCount = Convert.ToInt32(foundRow[i + 1]["SubDocumentStartPage"]) - Convert.ToInt32(foundRow[i]["SubDocumentStartPage"]);
                        else
                            subDocPageCount = Convert.ToInt32(foundRow[i]["PageCount"]) - Convert.ToInt32(foundRow[i]["SubDocumentStartPage"]) + 1;
                    }
                }
            }

            return subDocPageCount;
        }

        /// <summary>
        /// To get the Search Config Parameters.
        /// </summary>
        /// <param name="docParameter"></param>
        /// <returns></returns>
        public static IList<SearchConfigAttribute> GetSearchConfigParameters(DocumentSearchParameter docParameter)
        {
            try
            {
                return CommonDL.GetSearchConfigParameters(docParameter);
            }
            catch (Exception ex)
            {
                LoggerFactory.Logger.Log("CommonBL", LogEventType.Error, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// To get the Document By DocumentGuid
        /// </summary>
        /// <param name="documentGuid"></param>
        /// <param name="teenantId"></param>
        /// <returns></returns>
        /// 

        public static Document GetDocumentByDocumentGuid(string documentGuid, Tenant tenant)
        {
            DocumentSearchParameter docParameter = new DocumentSearchParameter() { TenantId = tenant.RecordId };
            string whereCountQuery = string.Empty; //docParameter.CountQuery;
            int maxSearchLimit = int.Parse(System.Configuration.ConfigurationManager.AppSettings["MaxSearchLimit"]);
            try
            {
                string whereClause = documentGuid;
                //string query = BuildSearchQuery(whereClause, tenant);
                // Document document_ = CommonDL.GetDocumentByDocumentGuid(query, tenant.RecordId);

                Document document = CommonDL.Get_DocumentByDocGuid(docParameter, tenant, maxSearchLimit, "SearchQuery", whereClause, whereCountQuery);
                if (document.SubDocumentDetails.Count > 0)
                {
                    SetSubdocumentPageCount(ref document);
                }

                return document;
            }
            catch (Exception ex)
            {
                LoggerFactory.Logger.Log("CommonBL", LogEventType.Error, ex.Message);
                throw;
            }
        }

        private static void SetSubdocumentPageCount(ref Document document)
        {
            try
            {
                List<SubDocument> lstSubDocument = document.SubDocumentDetails;
                lstSubDocument = lstSubDocument.OrderBy(q => q.SubDocumentStartPage).ToList();
                int pagecount = 0;
                if (lstSubDocument.Count != 0)
                {
                    if (lstSubDocument.Count == 1)
                    {
                        lstSubDocument[0].PageCount = document.PageCount - lstSubDocument[0].SubDocumentStartPage + 1;
                    }
                    else
                    {
                        for (int i = 0; i < lstSubDocument.Count - 1; i++)
                        {
                            lstSubDocument[i].PageCount = lstSubDocument[i + 1].SubDocumentStartPage - lstSubDocument[i].SubDocumentStartPage;
                            // pagecount =Convert.ToInt32(pagecount +  lstSubDocument[i].PageCount);
                        }
                        lstSubDocument[document.SubDocumentDetails.Count - 1].PageCount = document.PageCount - lstSubDocument[document.SubDocumentDetails.Count - 1].SubDocumentStartPage + 1;
                        //lstSubDocument[document.SubDocumentDetails.Count - 1].PageCount = document.PageCount - pagecount;
                    }
                }




                //if (lstSubDocument.Count == 1)
                //    lstSubDocument[0].PageCount = document.PageCount - lstSubDocument[0].SubDocumentStartPage;
                //else
                //{
                //    for (Int32 count = 0; count < lstSubDocument.Count - 1; count++)
                //    {
                //        lstSubDocument[count].PageCount = lstSubDocument[count + 1].SubDocumentStartPage - lstSubDocument[count].SubDocumentStartPage;
                //    }

                //    lstSubDocument[document.SubDocumentDetails.Count - 1].PageCount = document.PageCount - lstSubDocument[document.SubDocumentDetails.Count - 1].SubDocumentStartPage;
                //}

                document.SubDocumentDetails = lstSubDocument;
            }
            catch (Exception ex)
            {
                LoggerFactory.Logger.Log("CommonBL-SetSubdocumentPageCount", LogEventType.Error, ex.Message);

            }
        }



        /// <summary>
        /// To get the Sortint Criteria's List
        /// </summary>
        /// <returns></returns>
        public static IList<SortCriteria> GetSortCriterias()
        {
            try
            {
                return CommonDL.GetSortCriterias();
            }
            catch (Exception ex)
            {
                LoggerFactory.Logger.Log("CommonBL", LogEventType.Error, ex.Message);
                throw;
            }
        }

        /// <summary>
        ///  To Save Configure List
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="auditTrail"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static Message SaveConfigureList(SearchParameters searchParameters, AuditTrail auditTrail, User user)
        {
            try
            {
                string searchConfigXML = String.Empty;
                Util.Serialize<SearchParameters>(searchParameters, ref searchConfigXML);

                return CommonDL.SaveConfigureList(searchConfigXML, auditTrail, user);
            }
            catch (Exception ex)
            {
                LoggerFactory.Logger.Log("CommonBL", LogEventType.Error, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// To Save Build the Search Query
        /// </summary>
        /// <param name="whereClouse"></param>
        /// <param name="tenant"></param>
        /// <returns></returns>
        private static string BuildSearchQuery(string whereClouse, Tenant tenant)
        {
            System.Text.StringBuilder query = new System.Text.StringBuilder();


            query.Append("SELECT  AccountNumber,DocumentGUID,Postcode, PageCount, DocumentDate, DocumentGenerationDateTime, InsertsCodes,");
            query.Append("ProductCategory, ProductCategoryId, DocumentType, Departments , StreamCode as StreamName, Spoolname, JobID, ");
            query.Append("SubDocumentName, SubDocumentStartPage, ISNull(SubDocumentId, '-1') AS SubDocumentId ");
            query.Append("FROM (SELECT CL.ClientID, DC.AccountNumber AS AccountNumber,DocumentGUID,DC.Postcode AS Postcode, DC.PageCount AS PageCount, ");
            query.Append(" cast(floor(cast(DC.DocumentDate as float)) as datetime) AS DocumentDate, DC.DocumentGenerationDateTime,DC.InsertsCodes, DC.DocumentType AS ProductCategory,");
            query.Append(" DC.DocumentId AS ProductCategoryId, DC.DocumentSubType AS DocumentType,CFC.CustomFieldName  AS CustomFieldName,JB.Stream AS StreamName, ");
            query.Append(" DCF.StringValue   as CustomFieldValue, JB.DUJobID As JobID, ISNull(SD.SubDocumentName, '') As SubDocumentName, ISNull(SD.SubDocumentStartPage, '-1') As SubDocumentStartPage, ISNull(SD.SubDocumentId, '-1') AS SubDocumentId FROM vault.Clients CL INNER JOIN  vault.CustomFieldsClient CFC ON CL.ClientID = CFC.ClientId  ");
            query.Append(" INNER JOIN   vault.DocumentCustomFields DCF ON CFC.CustomFieldsClientID = DCF.CustomFieldsClientID ");
            query.Append(" INNER JOIN  vault.Documents DC ON DC.DocumentID = DCF.DocumentId ");
            query.Append(" INNER JOIN  vault.Jobs JB ON DC.JobID = JB.JobID AND JB.ClientID = " + tenant.MetadataDatabaseMappingId);
            query.Append(" LEFT OUTER JOIN vault.SubDocuments SD on DC.DocumentId= SD.DocumentId )");
            query.Append(" Source PIVOT (MIN(CustomFieldValue) for CustomFieldName  in ([Departments]  , [StreamCode], [SpoolName] )) AS PT ");

            query.Append(whereClouse);


            return query.ToString();
        }

        //private static string BuildSearchQuery(string whereClouse, Tenant tenant)
        //{
        //    System.Text.StringBuilder query = new System.Text.StringBuilder();
        //    query.Append("SELECT  AccountNumber,DocumentGUID,Postcode, PageCount, DocumentDate, DocumentGenerationDateTime, InsertsCodes,");
        //    query.Append("ProductCategory, ProductCategoryId, DocumentType, Departments , StreamCode as StreamName, Spoolname, JobID, ");
        //    query.Append("SubDocumentName, SubDocumentStartPage, ISNull(SubDocumentId, '-1') AS SubDocumentId ");
        //    query.Append("FROM (SELECT CL.ClientID, DC.AccountNumber AS AccountNumber,DocumentGUID,DC.Postcode AS Postcode, DC.PageCount AS PageCount, ");
        //    query.Append(" cast(floor(cast(DC.DocumentDate as float)) as datetime) AS DocumentDate, DC.DocumentGenerationDateTime,DC.InsertsCodes, DC.DocumentType AS ProductCategory,");
        //    query.Append(" DC.DocumentId AS ProductCategoryId, DC.DocumentSubType AS DocumentType,CFC.CustomFieldName  AS CustomFieldName,JB.Stream AS StreamName, ");
        //    query.Append(" DCF.StringValue   as CustomFieldValue, JB.DUJobID As JobID, ISNull(SD.SubDocumentName, '') As SubDocumentName, ISNull(SD.SubDocumentStartPage, '-1') As SubDocumentStartPage, ISNull(SD.SubDocumentId, '-1') AS SubDocumentId FROM Clients CL INNER JOIN  CustomFieldsClient CFC ON CL.ClientID = CFC.ClientId  ");
        //    query.Append(" INNER JOIN   DocumentCustomFields DCF ON CFC.CustomFieldsClientID = DCF.CustomFieldsClientID ");
        //    query.Append(" INNER JOIN  Documents DC ON DC.DocumentID = DCF.DocumentId ");
        //    query.Append(" INNER JOIN  Jobs JB ON DC.JobID = JB.JobID AND JB.ClientID = " + tenant.MetadataDatabaseMappingId);
        //    query.Append(" LEFT OUTER JOIN SubDocuments SD on DC.DocumentId= SD.DocumentId)");
        //    query.Append(" Source PIVOT (MIN(CustomFieldValue) for CustomFieldName  in ([Departments]  , [StreamCode], [SpoolName] )) AS PT ");

        //    query.Append(whereClouse);
        //    return query.ToString();
        //}

        //private static string BuildSearchCountQuery(string whereClouse, Tenant tenant)
        //{
        //    System.Text.StringBuilder query = new System.Text.StringBuilder();
        //    query.Append("SELECT  COUNT(DISTINCT(ProductCategoryId)) ");
        //    query.Append("FROM (SELECT CL.ClientID, DC.AccountNumber AS AccountNumber,DocumentGUID,DC.Postcode AS Postcode, DC.PageCount AS PageCount, ");
        //    query.Append(" cast(floor(cast(DC.DocumentDate as float)) as datetime) AS DocumentDate, DC.DocumentGenerationDateTime,DC.InsertsCodes, DC.DocumentType AS ProductCategory,");
        //    query.Append(" DC.DocumentId AS ProductCategoryId, DC.DocumentSubType AS DocumentType,CFC.CustomFieldName  AS CustomFieldName,JB.Stream AS StreamName, ");
        //    query.Append(" DCF.StringValue   as CustomFieldValue, JB.DUJobID As JobID, ISNull(SD.SubDocumentName, '') As SubDocumentName, ISNull(SD.SubDocumentStartPage, '-1') As SubDocumentStartPage, ISNull(SD.SubDocumentId, '-1') AS SubDocumentId FROM Clients CL INNER JOIN  CustomFieldsClient CFC ON CL.ClientID = CFC.ClientId  ");
        //    query.Append(" INNER JOIN   DocumentCustomFields DCF ON CFC.CustomFieldsClientID = DCF.CustomFieldsClientID ");
        //    query.Append(" INNER JOIN  Documents DC ON DC.DocumentID = DCF.DocumentId ");
        //    query.Append(" INNER JOIN  Jobs JB ON DC.JobID = JB.JobID AND JB.ClientID = " + tenant.MetadataDatabaseMappingId);
        //    query.Append(" LEFT OUTER JOIN SubDocuments SD on DC.DocumentId= SD.DocumentId)");
        //    query.Append(" Source PIVOT (MIN(CustomFieldValue) for CustomFieldName  in ([Departments]  , [StreamCode], [SpoolName] )) AS PT ");

        //    query.Append(whereClouse);
        //    return query.ToString();
        //}

        //private static string BuildOptimizedSearchQuery(string whereClause, string whereCountClause, Tenant tenant)
        //{
        //    int maxSearchLimit = int.Parse(System.Configuration.ConfigurationManager.AppSettings["MaxSearchLimit"]);

        //    System.Text.StringBuilder query = new System.Text.StringBuilder();
        //    query.Append("SELECT AccountNumber,DocumentGUID,Postcode, PageCount, DocumentDate, DocumentGenerationDateTime, InsertsCodes, ");
        //    query.Append(" ProductCategory, A.ProductCategoryId, DocumentType, Departments , StreamName, Spoolname, JobID, ");
        //    query.Append(" SubDocumentName, SubDocumentStartPage, ISNull(SubDocumentId, '-1') AS SubDocumentId ");
        //    query.Append(" FROM  ");
        //    query.Append(" ( ");
        //    query.Append(" SELECT AccountNumber,DocumentGUID,Postcode, PageCount, DocumentDate, DocumentGenerationDateTime, InsertsCodes, ");
        //    query.Append(" ProductCategory, ProductCategoryId, DocumentType, Departments , StreamCode as StreamName, Spoolname, JobID, ");
        //    query.Append(" SubDocumentName, SubDocumentStartPage, ISNull(SubDocumentId, '-1') AS SubDocumentId ");
        //    query.Append(" FROM (SELECT CL.ClientID, DC.AccountNumber AS AccountNumber,DocumentGUID,DC.Postcode AS Postcode, DC.PageCount AS PageCount,  ");
        //    query.Append(" cast(floor(cast(DC.DocumentDate as float)) as datetime) AS DocumentDate, DC.DocumentGenerationDateTime,DC.InsertsCodes, DC.DocumentType AS ProductCategory, ");
        //    query.Append(" DC.DocumentId AS ProductCategoryId, DC.DocumentSubType AS DocumentType,CFC.CustomFieldName  AS CustomFieldName,JB.Stream AS StreamName, ");
        //    query.Append(" DCF.StringValue   as CustomFieldValue, JB.DUJobID As JobID, ISNull(SD.SubDocumentName, '') As SubDocumentName, ISNull(SD.SubDocumentStartPage, '-1') As SubDocumentStartPage, ISNull(SD.SubDocumentId, '-1') AS SubDocumentId FROM Clients CL INNER JOIN  CustomFieldsClient CFC ON CL.ClientID = CFC.ClientId  ");
        //    query.Append(" INNER JOIN   DocumentCustomFields DCF ON CFC.CustomFieldsClientID = DCF.CustomFieldsClientID  ");
        //    query.Append(" INNER JOIN  Documents DC ON DC.DocumentID = DCF.DocumentId  ");
        //    query.Append(" INNER JOIN  Jobs JB ON DC.JobID = JB.JobID AND JB.ClientID = " + tenant.MetadataDatabaseMappingId);
        //    query.Append(" LEFT OUTER JOIN SubDocuments SD on DC.DocumentId= SD.DocumentId)  ");
        //    query.Append(" Source PIVOT (MIN(CustomFieldValue) for CustomFieldName  in ([Departments]  , [StreamCode], [SpoolName] )) AS PT  ");
        //    query.Append(whereCountClause);
        //    query.Append(" ) A ");
        //    query.Append(" INNER JOIN  ");
        //    query.Append(" (SELECT B.ProductCategoryId FROM ");
        //    query.Append(" (  ");
        //    query.Append(" SELECT TOP "  + maxSearchLimit ); 
        //    query.Append(" ProductCategoryId ");
        //    query.Append(" FROM (SELECT CL.ClientID, DC.AccountNumber AS AccountNumber,DocumentGUID,DC.Postcode AS Postcode, DC.PageCount AS PageCount,  ");
        //    query.Append(" cast(floor(cast(DC.DocumentDate as float)) as datetime) AS DocumentDate, DC.DocumentGenerationDateTime,DC.InsertsCodes, DC.DocumentType AS ProductCategory, ");
        //    query.Append(" DC.DocumentId AS ProductCategoryId, DC.DocumentSubType AS DocumentType,CFC.CustomFieldName  AS CustomFieldName,JB.Stream AS StreamName, ");
        //    query.Append(" DCF.StringValue   as CustomFieldValue, JB.DUJobID As JobID FROM Clients CL INNER JOIN  CustomFieldsClient CFC ON CL.ClientID = CFC.ClientId  ");
        //    query.Append(" INNER JOIN   DocumentCustomFields DCF ON CFC.CustomFieldsClientID = DCF.CustomFieldsClientID  ");
        //    query.Append(" INNER JOIN  Documents DC ON DC.DocumentID = DCF.DocumentId  ");
        //    query.Append(" INNER JOIN  Jobs JB ON DC.JobID = JB.JobID AND JB.ClientID = " + tenant.MetadataDatabaseMappingId);
        //    query.Append(" ) ");
        //    query.Append(" Source PIVOT (MIN(CustomFieldValue) for CustomFieldName  in ([Departments]  , [StreamCode], [SpoolName] )) AS PT  ");
        //    query.Append(whereClause);
        //    query.Append(" )B )C ");
        //    query.Append(" ON A.ProductCategoryId = C.ProductCategoryId  ");

        //    query.Append(whereClause);
        //    return query.ToString();
        //}
    }
}
