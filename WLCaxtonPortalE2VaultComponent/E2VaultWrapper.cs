//==========================================================================================
// Project              : WLCaxtonWebPortal
// File Name            : E2VaultWrapper.cs
// Program Description  : This class acts as a wrapper to E2Vault .Net API and exposed only requried functions to return the PDF document and Image
// Programmed By        : Nadeem Ishrat
// Programmed On        : 26-December-2012 
// Version              : 1.0.0
//==========================================================================================

using System;
using WLCaxtonPortalBusinessEntity;
using WLCaxtonPortalDataLayer;
using System.Configuration;
using e2NetRender.render2;
using e2NetRender;
using WLCaxtonPortalExceptionLogger;

namespace WLCaxtonPortalE2VaultComponent
{
    /// <summary>
    /// This class acts as a wrapper to E2Vault .Net API and exposed only requried functions to return the PDF document and Image
    /// </summary>
    public class E2VaultWrapper
    {
        string msg= string.Empty;
        string exmsg = string.Empty;

        #region Document Rendering code
        /// <summary>
        /// This Funtions returns the PDF Document
        /// </summary>
        /// <param name="user"></param>
        /// <param name="documentGUID"></param>
        /// <returns></returns>
        public byte[] GetPdfDocument(User user,string documentGUID, int startPageNumber, int pagesCount)
        {
            return GetPdfDocumentInternal(user, documentGUID, startPageNumber, pagesCount);
        }

        /// <summary>
        /// This function encapsulates the Logic of retrieval of the document.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="documentGUID"></param>
        /// <returns></returns>
        private byte[] GetPdfDocumentInternal(User user, string documentGUID, int startPageNumber, int pagesCount)
        {
            //RenderClient2 myClient = new RenderClient2();
            RenderClient2 client;
            byte[] pdfDocument = null;

            try
            {


                // string guidstring = "284797C36A8D9E2E086CD52B4D945818"; 284797c3-6a8d-9e2e-086c-d52b4d945821
              
                //param.outputfilename = "c:\\" + guidstring + ".pdf";
                //int filesize = client.RenderTransformByFile(dparam);
                //client.close();


                string host = ConfigurationManager.AppSettings["VaultHost"]; //"172.31.49.28";
                int port = int.Parse(ConfigurationManager.AppSettings["VaultPort"]); //8001

                string dbNameVault = ConfigurationManager.AppSettings["VaultDatabase"];// "rsa_test"; // To be provided by Williams lea or we have to loop it through RenderClient2.DatabaseList()

                client = new RenderClient2(host, port);
                
                e2RenderParameters param = new e2RenderParameters();
                param.parametertype = e2DocParameterType.DocumentGUID;
                param.dbname = dbNameVault;//db name from RenderClient2.DatabaseList() if not provided by Williams lea
                //get below parameters fromDatabaseSearch()/DatabaseResolve()/DocumentData()
                param.SetGUID(documentGUID);
                
               
                param.outputtype = e2OutputType.PDF;

                // Setting page count to be retrieved

                param.startpage = startPageNumber;
                    param.totalpages =  pagesCount;
                
        
                param.transformmode = e2TransformMode.mem; //default mode is memory mode
                //
                int testConnection = client.connect();
                e2RenderPages pages = client.RenderTransform(param);
                client.close();
                if (pages == null)
                {

                    msg = "failed to get document pages by memory : " + client.GetMyMsg();
                    pdfDocument = null;

                }
                else
                {
                    //use pages data to do something
                    pdfDocument = pages.pagesdatabytes;
                    int pagessize = pages.pagesdatasize;
                    //...Audit trail...
                }
                if (pdfDocument != null && pdfDocument.Length > 0)
                {
                    user.UserActivity = UserActivityType.ViewCompleteDocument;
                    user.DocumentGuid = documentGUID;
                    AuditTrailDL.LogUserActivity(user);
                }

            }
            catch (e2Exception ex)
            {

               LoggerFactory.Logger.Log("E2VaultWrapper", LogEventType.Error, ex.Message);

                
            }
            catch (Exception ex)
            {
                LoggerFactory.Logger.Log("E2VaultWrapper", LogEventType.Error, ex.Message);
                             
            }
                                    
            return pdfDocument;
        }
        #endregion

        //#region FirstPage Image rendering code

        ///// <summary>
        ///// THis function returns the First page image
        ///// </summary>
        ///// <param name="user"></param>
        ///// <param name="documentGUID"></param>
        ///// <returns></returns>
        //public byte[] GetFirstpage(User user, string documentGUID)
        //{
        //    return GetFirstpageInternal(user, documentGUID);
        //}

        ///// <summary>
        ///// This function encapsulates the Logic of retrieval of First page Image
        ///// </summary>
        ///// <param name="user"></param>
        ///// <param name="documentGUID"></param>
        ///// <returns></returns>
        //private byte[] GetFirstpageInternal(User user, string documentGUID)
        //{
        //    RenderClient2 myClient = new RenderClient2();
        //    byte[] pdfDocument = null;

        //    try
        //    {
        //        string dbName = "WilliamsLea"; // To be provided by Williams lea or we have to loop it through RenderClient2.DatabaseList()


        //        e2RenderParameters param = new e2RenderParameters();
        //        param.parametertype = e2DocParameterType.DocumentGUID;
        //        param.dbname = dbName;//db name from RenderClient2.DatabaseList() if not provided by Williams lea

        //        //get below parameters fromDatabaseSearch()/DatabaseResolve()/DocumentData()
        //        string account = "12345";
        //        string docdate = "2007/01/01";
        //        string doctype = "afp";
        //        string docfile = documentGUID;
        //        string docoffset = "00000C00";

        //        param.SetNormalParameters(account, docdate, doctype, docfile, docoffset);
        //        param.startpage = 1;
        //        param.totalpages = 1;
        //        param.SetOutputType(0);//0=gif
        //        param.resolution = 800;
        //        param.orientation = 0;
        //        param.transformmode = e2TransformMode.mem; //default mode is memory mode
        //        //
        //        myClient.connect();
        //        e2RenderPages pages = myClient.RenderTransform(param);
        //        myClient.close();
        //        if (pages == null)
        //        {
        //            msg = "failed to get document pages by memory : " + myClient.GetMyMsg();
        //        }

        //        //use pages data to do something
        //        pdfDocument = pages.pagesdatabytes;
        //        int pagessize = pages.pagesdatasize;

        //        //...Audit trail
        //        if (pdfDocument != null && pdfDocument.Length > 0)
        //        {
        //            user.UserActivity = UserActivityType.ViewDocumentFirstPage;
        //            user.DocumentGuid = documentGUID;
        //            AuditTrailDL.LogUserActivity(user);
        //        }
        //    }
        //    catch (e2Exception ex)
        //    {
        //        exmsg = ex.Message;
        //        msg = myClient.GetMyMsg();
        //        //do something
        //        //...
        //    }
        //    catch (Exception ex)
        //    {
        //        exmsg = ex.Message;
        //        //do something
        //        //...
        //    }
        //    finally
        //    {
        //        //free resources
        //        myClient.close();
        //    }

        //    return pdfDocument;
        //}
        //#endregion
    }

   
}
