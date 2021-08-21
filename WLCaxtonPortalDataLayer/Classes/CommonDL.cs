//==========================================================================================
// Project              : WLCaxtonWebPortal
// File Name            : CommonDL.cs
// Program Description  : Contains common fucntionality such as getting productlist,document types,document search 
// Programmed By        : Nadeem Ishrat
// Programmed On        : 10-December-2012 
// Version              : 1.0.0
//==========================================================================================

#region Namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WLCaxtonPortalDataHelper;
using WLCaxtonPortalBusinessEntity;
using System.Data;
using System.Data.SqlClient;
using WLCaxtonPortalBusinessEntity.Classes;
using WLCaxtonPortalDataLayer.Classes;
using System.Data.Common;
using WLCaxtonPortalExceptionLogger;
#endregion

namespace WLCaxtonPortalDataLayer
{
    public class CommonDL : DataLayerBase
    {
        #region private static variables
        private static IDatabase databaseInstance;
        private static IDataReader reader = null;
        private static DbCommand dbCommand = null;
        #endregion

        #region Public methods
        /// <summary>Modified by : VRT: 22-MAR-2018,
        /// Get the product list
        /// </summary>
        /// <returns>List of products</returns>
        public static IList<ProductCategory> GetProductCategories()
        {
            List<ProductCategory> productCategories = new List<ProductCategory>();

            try
            {
                databaseInstance = DataManager.GetDatabaseInstance();

                //dbCommand = databaseInstance.CreateSqlQuery("select ProductCategoryId, ProductCategoryName,ProductCategoryDescription from vw_ProductCategory");
                //reader = dbCommand.ExecuteReader();
                dbCommand = databaseInstance.CreateCommand("Proc_GetProductCategories");
                reader = databaseInstance.ExecuteReader(dbCommand);

                while (reader.Read())
                {
                    productCategories.Add(new ProductCategory
                    {
                        ProductCategoryDescription = GetValue<string>(reader, "ProductCategoryDescription"),
                        ProductCategoryName = GetValue<string>(reader, "ProductCategoryName"),
                        ProductCategoryId = GetValue<int>(reader, "ProductCategoryId")
                    });

                }
            }
            catch (Exception ex)
            {
                LoggerFactory.Logger.Log("CommonDL", LogEventType.Error, ex.Message);
                throw;
            }
            finally
            {
                Clean(dbCommand, dbCommand.Connection, reader);
            }
            return productCategories;
        }

        /// <summary>Modified by : VRT : 22-MAR-2018,
        /// Get the document type list
        /// </summary>
        /// <returns>List of document types</returns>
        /// 
        public static IList<DocumentType> GetDocumentTypes()
        {
            List<DocumentType> documentTypes = new List<DocumentType>();
            IDataReader reader = null;
            try
            {
                databaseInstance = DataManager.GetDatabaseInstance();
                if (databaseInstance != null)
                {
                    using (DbCommand command = databaseInstance.CreateCommand("Proc_GetDocumentTypes"))
                    {
                        // dbCommand = databaseInstance.CreateCommand("Proc_GetDocumentTypes");
                        reader = databaseInstance.ExecuteReader(command);
                        while (reader.Read())
                        {
                            documentTypes.Add(new DocumentType
                            {
                                DocumentTypeDescription = GetValue<string>(reader, "DocumentTypeDescription"),
                                DocumentTypeName = GetValue<string>(reader, "DocumentTypeName"),
                                DocumentTypeId = GetValue<int>(reader, "DocumentTypeId")
                            });
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                LoggerFactory.Logger.Log("CommonDL", LogEventType.Error, ex.Message);
                throw;
            }
            finally
            {
                //Clean(dbCommand, dbCommand.Connection, reader);
            }
            return documentTypes;
        }



        //public static IList<DocumentType> GetDocumentTypes()
        //{
        //    List<DocumentType> documentTypes = new List<DocumentType>();
        //    IDataReader reader = null;
        //    try
        //    {
        //        databaseInstance = DataManager.GetDatabaseInstance();
        //        dbCommand = databaseInstance.CreateSqlQuery("select DocumentTypeId, DocumentTypeName, DocumentTypeDescription from vw_DocumentType");
        //        reader = dbCommand.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            documentTypes.Add(new DocumentType
        //            {
        //                DocumentTypeDescription = GetValue<string>(reader, "DocumentTypeDescription"),
        //                DocumentTypeName = GetValue<string>(reader, "DocumentTypeName"),
        //                DocumentTypeId = GetValue<int>(reader, "DocumentTypeId")
        //            });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggerFactory.Logger.Log("CommonDL", LogEventType.Error, ex.Message);
        //        throw;
        //    }
        //    finally
        //    {
        //        Clean(dbCommand, dbCommand.Connection, reader);
        //    }
        //    return documentTypes;
        //}

        /// <summary>
        /// Get the documents based on search criteria
        /// </summary>
        /// <param name="docParameter">object contains search query in xml and teanant code </param>
        /// <returns>Data table for documents returned by search result</returns>
        public static DataTable GetDocumentsBySearchCriteria(DocumentSearchParameter docParameter)
        {
            DataTable dtResults = new DataTable();
            try
            {
                databaseInstance = DataManager.GetDatabaseInstance(docParameter.TenantId);
                dbCommand = databaseInstance.CreateSqlQuery(docParameter.Query);

                DataSet dsResults = databaseInstance.ExecuteDataSet(dbCommand);
                dtResults = dsResults.Tables[0];
            }
            catch (Exception ex)
            {
                LoggerFactory.Logger.Log("CommonDL", LogEventType.Error, ex.Message);
                throw;
            }
            finally
            {
                Clean(dbCommand, dbCommand.Connection);
            }
            return dtResults;
        }



        /// <summary>
        /// Gets the Count of the documents as per the search parameter..
        /// </summary>
        /// <param name="docParameter"></param>
        /// <returns></returns>
        /// VRT
        public static DataTable GetDocumentsCountSearchQuery(DocumentSearchParameter docParameter, Tenant tenant, Int32 MaxSearchLimit, string QueryType, string WhereClouse, string WhereCountClause)
        {
            DataTable dtResults = new DataTable();
            try
            {
                // databaseInstance = DataManager.GetDatabaseInstance();
                databaseInstance = DataManager.GetDatabaseInstance(docParameter.TenantId);
                //dbCommand = databaseInstance.CreateCommand("Proc_BuildSearchDocQuery_Temp");
                if (QueryType == "SearchQuery")
                {
                    dbCommand = databaseInstance.CreateCommand("Proc_SearchDocQuery_TMP");
                }
                else
                {

                    dbCommand = databaseInstance.CreateCommand("Proc_DocumentsSearchCount");
                }
                databaseInstance.AddInParameter(dbCommand, "@IP_tenantMappingId", DbType.Int32, tenant.MetadataDatabaseMappingId);
                databaseInstance.AddInParameter(dbCommand, "@IP_maxSearchLimit", DbType.Int32, MaxSearchLimit);
                databaseInstance.AddInParameter(dbCommand, "@IP_QueryType", DbType.String, QueryType);
                databaseInstance.AddInParameter(dbCommand, "@IP_WhereClouse", DbType.String, WhereClouse);
                databaseInstance.AddInParameter(dbCommand, "@IP_whereCountClause", DbType.String, WhereCountClause);
                databaseInstance.AddOutParameter(dbCommand, "@OP_IsSuccess", DbType.Boolean, 5);
                databaseInstance.AddOutParameter(dbCommand, "@OP_MessageID", DbType.String, 10);
                databaseInstance.AddOutParameter(dbCommand, "@OP_MessageText", DbType.String, 200);

                //reader = databaseInstance.ExecuteReader(dbCommand);

                //databaseInstance = DataManager.GetDatabaseInstance(docParameter.TenantId);
                //dbCommand = databaseInstance.CreateSqlQuery(docParameter.Query);
                dbCommand.CommandTimeout = 0;
                DataSet dsResults = databaseInstance.ExecuteDataSet(dbCommand);
                var message = dbCommand.Parameters["@OP_MessageText"].Value;
                if (dsResults.Tables.Count > 0)
                {
                    dtResults = dsResults.Tables[0];
                }

            }
            catch (Exception ex)
            {
                LoggerFactory.Logger.Log("CommonDL", LogEventType.Error, ex.Message);
                throw;
            }
            finally
            {
                Clean(dbCommand, dbCommand.Connection);
            }
            return dtResults;
        }

        public static DataTable GetDocumentsCountBySearchCriteria(DocumentSearchParameter docParameter)
        {
            DataTable dtResults = new DataTable();
            try
            {
                databaseInstance = DataManager.GetDatabaseInstance(docParameter.TenantId);
                dbCommand = databaseInstance.CreateSqlQuery(docParameter.Query);

                DataSet dsResults = databaseInstance.ExecuteDataSet(dbCommand);
                dtResults = dsResults.Tables[0];
            }
            catch (Exception ex)
            {
                LoggerFactory.Logger.Log("CommonDL", LogEventType.Error, ex.Message);
                throw;
            }
            finally
            {
                Clean(dbCommand, dbCommand.Connection);
            }
            return dtResults;
        }

        /// <summary>
        /// Get the spacific document details 
        /// </summary>
        /// <param name="documentGuid">unique identifier for document</param>
        /// <param name="teenantId">client code</param>
        /// <returns>SelectedDocument</returns>
        /// BY: VRT
        public static Document Get_DocumentByDocGuid(DocumentSearchParameter docParameter, Tenant tenant, Int32 MaxSearchLimit, string QueryType, string WhereClouse, string WhereCountClause)
        {
            DataTable dtResults = new DataTable();
            Document document = new Document();
            List<SubDocument> SubDocuments = new List<SubDocument>();
            try
            {
                databaseInstance = DataManager.GetDatabaseInstance(docParameter.TenantId);
                dbCommand = databaseInstance.CreateCommand("[Proc_Get_DocumentByDocGuid_Temp]");
                databaseInstance.AddInParameter(dbCommand, "@IP_tenantMappingId", DbType.Int32, tenant.MetadataDatabaseMappingId);
                //databaseInstance.AddInParameter(dbCommand, "@IP_maxSearchLimit", DbType.Int32, MaxSearchLimit);
                databaseInstance.AddInParameter(dbCommand, "@IP_QueryType", DbType.String, QueryType);
                databaseInstance.AddInParameter(dbCommand, "@IP_DocGuid", DbType.String, WhereClouse);
                databaseInstance.AddOutParameter(dbCommand, "@OP_IsSuccess", DbType.Boolean, 5);
                databaseInstance.AddOutParameter(dbCommand, "@OP_MessageID", DbType.String, 10);
                databaseInstance.AddOutParameter(dbCommand, "@OP_MessageText", DbType.String, 200);

                DataSet dsResults = databaseInstance.ExecuteDataSet(dbCommand);
                // var message = dbCommand.Parameters["@OP_MessageText"].Value;
                // dtResults = dsResults.Tables[0];
                reader = databaseInstance.ExecuteReader(dbCommand);

                while (reader.Read())
                {
                    document.DocumentGuid = GetValue<string>(reader, "DocumentGUID");
                    document.AccountNumber = GetValue<string>(reader, "AccountNumber");
                    document.PostCode = GetValue<string>(reader, "Postcode");
                    document.PageCount = GetValue<int>(reader, "PageCount");
                    document.DocumentDate = GetValue<DateTime?>(reader, "DocumentDate");
                    document.ProductCategory = new ProductCategory { ProductCategoryName = GetValue<string>(reader, "ProductCategory") };
                    document.DocumentType = new DocumentType { DocumentTypeName = GetValue<string>(reader, "DocumentType") };
                    document.StreamName = GetValue<string>(reader, "StreamName");
                    document.Spoolname = GetValue<string>(reader, "Spoolname");
                    document.Inserts = GetValue<string>(reader, "Insertscodes");
                    document.JobId = Convert.ToInt32(GetValue<string>(reader, "JobID"));
                    document.DocumentGenerationDateTime = GetValue<DateTime?>(reader, "DocumentGenerationDateTime");

                    if (!string.IsNullOrEmpty(GetValue<string>(reader, "SubDocumentName")))
                    {
                        SubDocuments.Add(new SubDocument
                        {
                            ParentDocGuid = GetValue<string>(reader, "DocumentGUID"),
                            ParentDocAccountNumber = GetValue<string>(reader, "AccountNumber"),
                            SubDocumentId = GetValue<int>(reader, "SubDocumentId"),
                            SubDocumentName = GetValue<string>(reader, "SubDocumentName"),
                            SubDocumentStartPage = GetValue<int>(reader, "SubDocumentStartPage"),
                            PageCount = GetValue<int>(reader, "PageCount")
                        });
                    }
                }

                document.SubDocumentDetails = SubDocuments;
            }
            catch (Exception ex)
            {
                LoggerFactory.Logger.Log("CommonDL", LogEventType.Error, ex.Message);
                throw;
            }
            finally
            {
                Clean(dbCommand, dbCommand.Connection, reader);
            }
            return document;

        }
        public static Document GetDocumentByDocumentGuid(string query, int tenantId)
        {
            Document document = new Document();
            List<SubDocument> SubDocuments = new List<SubDocument>();
            try
            {
                databaseInstance = DataManager.GetDatabaseInstance(tenantId);

                dbCommand = databaseInstance.CreateSqlQuery(query);



                //dbCommand = databaseInstance.CreateSqlQuery(string.Format("{0}{1}{2}",
                //    "select * from vw_DocumentSearch where DocumentGUID = '", documentGuid.ToString(), "'"));
                reader = dbCommand.ExecuteReader();

                while (reader.Read())
                {
                    document.DocumentGuid = GetValue<string>(reader, "DocumentGUID");
                    document.AccountNumber = GetValue<string>(reader, "AccountNumber");
                    document.PostCode = GetValue<string>(reader, "Postcode");
                    document.PageCount = GetValue<int>(reader, "PageCount");
                    document.DocumentDate = GetValue<DateTime?>(reader, "DocumentDate");
                    document.ProductCategory = new ProductCategory { ProductCategoryName = GetValue<string>(reader, "ProductCategory") };
                    document.DocumentType = new DocumentType { DocumentTypeName = GetValue<string>(reader, "DocumentType") };
                    document.StreamName = GetValue<string>(reader, "StreamName");
                    document.Spoolname = GetValue<string>(reader, "Spoolname");
                    document.Inserts = GetValue<string>(reader, "Insertscodes");
                    document.JobId = Convert.ToInt32(GetValue<string>(reader, "JobID"));
                    document.DocumentGenerationDateTime = GetValue<DateTime?>(reader, "DocumentGenerationDateTime");

                    if (!string.IsNullOrEmpty(GetValue<string>(reader, "SubDocumentName")))
                    {
                        SubDocuments.Add(new SubDocument
                        {
                            ParentDocGuid = GetValue<string>(reader, "DocumentGUID"),
                            ParentDocAccountNumber = GetValue<string>(reader, "AccountNumber"),
                            SubDocumentId = GetValue<int>(reader, "SubDocumentId"),
                            SubDocumentName = GetValue<string>(reader, "SubDocumentName"),
                            SubDocumentStartPage = GetValue<int>(reader, "SubDocumentStartPage"),
                            PageCount = GetValue<int>(reader, "PageCount")
                        });
                    }
                }

                document.SubDocumentDetails = SubDocuments;
            }
            catch (Exception ex)
            {
                LoggerFactory.Logger.Log("CommonDL", LogEventType.Error, ex.Message);
                throw;
            }
            finally
            {
                Clean(dbCommand, dbCommand.Connection, reader);
            }
            return document;
        }

        /// <summary>Modified by : VRT: 22-MAR-2018,
        /// Get the sort critearis set by individual user
        /// </summary>
        /// <returns>colletion sort criteria</returns>
        public static IList<SortCriteria> GetSortCriterias()
        {
            List<SortCriteria> sortCriterias = new List<SortCriteria>();
            IDataReader reader = null;
            try
            {
                databaseInstance = DataManager.GetDatabaseInstance();
                dbCommand = databaseInstance.CreateCommand("Proc_GetSortCriterias");
                reader = databaseInstance.ExecuteReader(dbCommand);
                while (reader.Read())
                {
                    sortCriterias.Add(new SortCriteria
                    {
                        CriteriaId = GetValue<int>(reader, "CriteriaId"),
                        CriteriaValue = GetValue<string>(reader, "CriteriaDesc"),
                        CriteriaName = GetValue<string>(reader, "CriteriaName")
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerFactory.Logger.Log("CommonDL", LogEventType.Error, ex.Message);
                throw;
            }
            finally
            {
                Clean(dbCommand, dbCommand.Connection, reader);
            }
            return sortCriterias;
        }

        /// <summary>
        /// Get the SearchConfigParameters critearis set by individual user
        /// </summary>
        /// <param name="docParameter"></param>
        /// <returns>Collection search config paratmeters</returns>
        public static IList<SearchConfigAttribute> GetSearchConfigParameters(DocumentSearchParameter docParameter)
        {
            List<SearchConfigAttribute> searchConfigAttributes = new List<SearchConfigAttribute>();
            IDataReader reader = null;
            try
            {
                databaseInstance = DataManager.GetDatabaseInstance();
                dbCommand = databaseInstance.CreateCommand("Proc_GetConfigureList");
                databaseInstance.AddInParameter(dbCommand, "@p_TenantId", DbType.Int32, 1);
                databaseInstance.AddInParameter(dbCommand, "@p_UserId", DbType.Int32, docParameter.UserDetails.UserId);

                reader = databaseInstance.ExecuteReader(dbCommand);

                while (reader.Read())
                {
                    searchConfigAttributes.Add(new SearchConfigAttribute
                    {
                        ParameterId = GetValue<int>(reader, "ParameterId"),
                        ParameterName = GetValue<string>(reader, "ParameterName"),
                        ParameterDescription = GetValue<string>(reader, "ParameterDescription"),
                        ParameterTypeName = GetValue<string>(reader, "ParameterTypeName"),
                        DisplayOrder = GetValue<int>(reader, "DisplayOrder"),
                        IsGroupBy = GetValue<bool>(reader, "IsGroupBy"),
                        IsVisible = GetValue<bool>(reader, "IsVisible")
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerFactory.Logger.Log("CommonDL", LogEventType.Error, ex.Message);
                throw;
            }
            finally
            {
                Clean(dbCommand, dbCommand.Connection, reader);
            }
            return searchConfigAttributes;
        }

        /// <summary>
        /// Save the configuration list for individual user
        /// </summary>
        /// <param name="searchConfigXML">collection of config values</param>
        /// <param name="auditTrail">obbject for tracking purpose</param>
        /// <param name="user">User object</param>
        /// <returns>Message object contains message description and issuccess</returns>
        public static Message SaveConfigureList(string searchConfigXML, AuditTrail auditTrail, User user)
        {
            Message message = new Message();
            try
            {
                databaseInstance = DataManager.GetDatabaseInstance();
                DbCommand dbCommand = databaseInstance.CreateCommand("Proc_SaveConfigureList");
                databaseInstance.AddInParameter(dbCommand, "@p_TenantId", DbType.Int32, user.TenantDetails.RecordId);
                databaseInstance.AddInParameter(dbCommand, "@p_UserId", DbType.Int32, user.UserId);
                databaseInstance.AddInParameter(dbCommand, "@p_ConfigureDetails", DbType.String, searchConfigXML);
                databaseInstance.AddInParameter(dbCommand, "@p_CreatedBy", DbType.Int32, user.UserId);
                databaseInstance.AddInParameter(dbCommand, "@p_CreatedOn", DbType.DateTime, auditTrail.CreatedOn);
                databaseInstance.AddOutParameter(dbCommand, "@p_IsSuccess", DbType.Boolean, 1);
                databaseInstance.AddOutParameter(dbCommand, "@p_MessageID", DbType.String, 10);
                databaseInstance.AddOutParameter(dbCommand, "@p_MessageText", DbType.String, 200);
                databaseInstance.AddInParameter(dbCommand, "@p_DBAction", DbType.Int16, DBAction.Save);

                databaseInstance.ExecuteNonQuery(dbCommand);

                message.IsSuccess = (bool)dbCommand.Parameters["@p_IsSuccess"].Value;
                message.MessageId = (string)dbCommand.Parameters["@p_MessageID"].Value;
                message.Description = (string)dbCommand.Parameters["@p_MessageText"].Value;
            }
            catch (Exception ex)
            {
                message.IsSuccess = false;
                LoggerFactory.Logger.Log("CommonDL", LogEventType.Error, ex.Message);
                throw;
            }

            return message;
        }
        #endregion
    }
}
