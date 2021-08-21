using System;
using System.IO;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using WLCaxtonPortalExceptionLogger;

namespace WLCaxtonMvcWebPortal.Models
{

    /// <summary>Author: VRT:12/6/2018
    /// Delete  all File after Result Executed 
    /// </summary>
    public class DeleteFileAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            filterContext.HttpContext.Response.Flush();
            if ((filterContext.Result as FilePathResult).FileName != null)
            {
                try
                {
                    //convert the current filter context to file and get the file path
                    string filePath = (filterContext.Result as FilePathResult).FileName;
                    LoggerFactory.Logger.Log("DeleteFileAttribute ", LogEventType.Error, "file Downloaded on  ==> " + DateTime.Now.ToString("MM-dd-yyyy hh:mm:ss") + " ; " + filePath);

                    //Thread.Sleep(100000); //                   await Task.Delay(10000);
                    using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Write, FileShare.Read))
                    {
                       // if (File.Exists(filePath))
                        {
                            stream.Dispose();
                            System.IO.File.Delete(filePath);//delete the file after download
                        }
                    }

                    LoggerFactory.Logger.Log("DeleteFileAttribute ", LogEventType.Error, "File Deleted on  ==> " + DateTime.Now.ToString("MM-dd-yyyy hh:mm:ss"));
                }
                catch (Exception ex)
                {
                    LoggerFactory.Logger.Log("DeleteFileAttribute ", LogEventType.Error, ex.Message);
                }
            }

        }
    }
}