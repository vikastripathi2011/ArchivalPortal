//==========================================================================================
// Project              : WLCaxtonWebPortal
// File Name            : RenderClient2.cs.cs
// Program Description  : This File will be deleted once we add the .Net API, this is just created to test the rendering of the PDF document and the Image
// Programmed By        : Nadeem Ishrat
// Programmed On        : 26-December-2012 
// Version              : 1.0.0
//==========================================================================================
using System;
using System.IO;
using System.Web;

// This File will be deleted once we add the .Net API, this is just created to test the rendering of the PDF document and the Image

namespace WLCaxtonPortalE2VaultComponent
{
    /// <summary>
    /// Main API Class that renders the document and exposes all the functions
    /// </summary>
    /*public class RenderClient2
    {
        public RenderClient2()
        {
        }

        public RenderClient2(string host, int port)
        {
        }

        public void connect()
        {
        }

        public void close()
        {
        }

        /// <summary>
        /// This is the main function that renders the Document/ File
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public e2RenderPages RenderTransform(e2RenderParameters param)
        {
            // Hardcode the files to similate the Page rendering engine

            e2RenderPages pageData = new e2RenderPages();
            FileStream fsStream =null;
            try
            {
                string path = HttpContext.Current.Server.MapPath("");
                path = String.Format("{0}{1}", path, @"\TestFiles\");

                if (param.outputType == 0) // FOR GIF
                {
                    if (param.documentFile == "D4B02B02-EBF7-4DF7-B4DC-E504D6A65CA9" || param.documentFile == "D4B02B02-EBF7-4DF7-B4DC-E504D6A65CA7" || param.documentFile == "D4B02B02-EBF7-4DF7-B4DC-E504D6A6BCA8" || param.documentFile == "D4B02B02-EBF7-4DF7-B4DC-E504D6A6BCB2")
                    {
                        path = String.Format("{0}{1}", path, "Holiday2013FirstPage.JPG");
                        //fsStream = new FileStream(@"C:\TestFiles\Holiday2013FirstPage.JPG", FileMode.Open);
                        fsStream = new FileStream(path, FileMode.Open);
                        byte[] bytes = new byte[fsStream.Length];
                        fsStream.Read(bytes, 0, Convert.ToInt32(fsStream.Length));
                        pageData.pagesdatabytes = bytes;
                    }
                    else
                    {
                        path = String.Format("{0}{1}", path, "Holiday2012FirstPage.JPG");
                        //fsStream = new FileStream(@"C:\TestFiles\Holiday2012FirstPage.JPG", FileMode.Open);
                        fsStream = new FileStream(path, FileMode.Open);
                        byte[] bytes = new byte[fsStream.Length];
                        fsStream.Read(bytes, 0, Convert.ToInt32(fsStream.Length));
                        pageData.pagesdatabytes = bytes;  
                    }
                }
                else //FOR PDF
                {
                    if (param.documentFile == "D4B02B02-EBF7-4DF7-B4DC-E504D6A65CA9" || param.documentFile == "D4B02B02-EBF7-4DF7-B4DC-E504D6A65CA7" || param.documentFile == "D4B02B02-EBF7-4DF7-B4DC-E504D6A6BCA8" || param.documentFile == "D4B02B02-EBF7-4DF7-B4DC-E504D6A6BCB2")
                    {
                        path = String.Format("{0}{1}", path, "Holiday List_2013.pdf");
                        //fsStream = new FileStream(@"C:\TestFiles\Holiday List_2013.pdf", FileMode.Open);
                        fsStream = new FileStream(path, FileMode.Open);
                        byte[] bytes = new byte[fsStream.Length];
                        fsStream.Read(bytes, 0, Convert.ToInt32(fsStream.Length));
                        pageData.pagesdatabytes = bytes;
                    }
                    else
                    {
                        path = String.Format("{0}{1}", path, "Holiday List-2012-India.pdf");
                       // fsStream = new FileStream(@"C:\TestFiles\Holiday List-2012-India.pdf", FileMode.Open);
                        fsStream = new FileStream(path, FileMode.Open);
                        byte[] bytes = new byte[fsStream.Length];
                        fsStream.Read(bytes, 0, Convert.ToInt32(fsStream.Length));
                        pageData.pagesdatabytes = bytes;
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (fsStream != null)
                {
                    fsStream.Flush();
                    fsStream.Close();
                }
            }

            return pageData;
        }
        public string GetMyMsg()
        {
            return string.Empty;
        }

        public e2IndexList GetIndexList(string dbName)
        {
            return new e2IndexList();
        }
    }

    /// <summary>
    /// Parameter class
    /// </summary>
    public class e2RenderParameters
    {
        public e2DocParameterType parametertype { get; set; }
        public e2TransformMode transformmode { get; set; } 
        public string dbname { get; set; }
        public int startpage { get; set; }
        public int totalpages { get; set; }
        public int resolution { get; set; }
        public int orientation { get; set; }
        public e2OutputType outputType;
        public string documentFile;
        public string documentGUID;


        public void SetNormalParameters(string account, string docdate, string doctype, string docfile, string docoffset)
        {
            documentFile = docfile;
           
        }

        public void SetOutputType(e2OutputType type)
        {
            outputType = type;


        }

        public void SetGUID(string guid)
        {
            this.documentFile = guid;

        }

       
    }

    /// <summary>
    /// Render page class
    /// </summary>
    public class e2RenderPages
    {
        public byte[] pagesdatabytes { get; set; }
        public int pagesdatasize { get; set; }

    }

    /// <summary>
    /// Exception class
    /// </summary>
    public class e2Exception : SystemException
    {
    }

    /// <summary>
    /// Database class
    /// </summary>
    public class e2Database
    {

    }

    /// <summary>
    /// Index class
    /// </summary>
    public class e2IndexList
    {

    }

    /// <summary>
    /// ParameterType Enum
    /// </summary>
    public enum e2DocParameterType
    {
        NORMAL=1,
        DocumentGUID=2
    }

    /// <summary>
    /// Document transder mode
    /// </summary>
    public enum e2TransformMode
    {
        mem=1
    }

    public enum e2OutputType
    {
        PDF=1
    }
 */   
}
