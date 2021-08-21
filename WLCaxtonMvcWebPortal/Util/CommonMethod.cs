using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using WLCaxtonPortalExceptionLogger;

namespace WLCaxtonMvcWebPortal.Util
{
    public class CommonMethods
    {
        public static bool IsDate(object Val)
        {
            return Microsoft.VisualBasic.Information.IsDate(Val);
        }

        public static bool IsNumeric(object Val)
        {
            return Microsoft.VisualBasic.Information.IsNumeric(Val);
        }
        public static bool IsNothing(object Val)
        {
            return Microsoft.VisualBasic.Information.IsNothing(Val);
        }

        public static string FormatDate(DateTime Val, string Format)
        {
            return Val.ToString(Format);
        }

        public static bool IsBoolean(object Val)
        {
            try
            {
                Boolean check = Convert.ToBoolean(Val);
                return true;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>VRT:04/08/2015: Right On Page Global Varriables==>>
        /// 
        /// </summary>
        /// <param name="Val"></param>
        /// <returns></returns>
        #region "Right On Page Global Varriables==>>"


        public static bool IsInsert { get; set; }
        public static bool isUpdate { get; set; }

        public static bool isDelete { get; set; }
        public static bool isView { get; set; }
        public static bool isPrint { get; set; }

        public static string Status { get; set; }
        public static string IsBranch { get; set; }

        #endregion



        /// <summary> using this function for convert 12 hours time format to 24 hours time format
        ///
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string Convert12To24HrTime(string time)
        {

            try
            {
                char[] delimiters = new char[] { ':', ' ' };
                string[] spltTime = time.Split(delimiters);

                int hour = Convert.ToInt32(spltTime[0]);
                int minute = Convert.ToInt32(spltTime[1]);
                int seconds = 0;

                string amORpm = spltTime[2];

                if (amORpm.ToUpper() == "PM")
                {
                    hour = (hour % 12) + 12;
                }

                TimeSpan timespan = new TimeSpan(hour, minute, seconds);


                return timespan.ToString(@"hh\:mm").Replace(':', ' ').Replace(" ", String.Empty);
            }
            catch
            {

                return string.Empty;
            }
        }

        /// <summary>using this function for convert 24 hours time format to 12 hours time format
        /// 
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string Convert24To12HrTime(string time)
        {
            try
            {
                if (!string.IsNullOrEmpty(time))
                {
                    string newTimeSpan = DateTime.ParseExact(time, "HHmm", System.Globalization.CultureInfo.CurrentCulture).ToString("hh:mm tt");
                    return newTimeSpan;
                }
                else
                {
                    return string.Empty;
                }
            }
            catch
            {

                return string.Empty;
            }
        }

        /// <summary>
        /// write log file
        /// </summary>
        /// <param name="ExceptionMessage"></param>
        /// <param name="InnerExceptionMessage"></param>
        /// <param name="LogFilePath"></param>
        public static void LogFileWrite(string ExceptionMessage, string InnerExceptionMessage, string LogFilePath)
        {
            FileStream fileStream = null;
            StreamWriter streamWriter = null;
            try
            {

                if (LogFilePath.Equals("")) return;

                #region Create the Log file directory if it does not exists
                DirectoryInfo logDirInfo = null;
                FileInfo logFileInfo = new FileInfo(LogFilePath);
                logDirInfo = new DirectoryInfo(logFileInfo.DirectoryName);
                if (!logDirInfo.Exists) logDirInfo.Create();
                #endregion Create the Log file directory if it does not exists

                if (!logFileInfo.Exists)
                {
                    fileStream = logFileInfo.Create();
                }
                else
                {
                    fileStream = new FileStream(LogFilePath, FileMode.Append);
                }
                streamWriter = new StreamWriter(fileStream);
                streamWriter.WriteLine("");
                streamWriter.WriteLine("Exception Date:" + DateTime.Now.ToString("MM-dd-yyyy hh:mm:ss"));
                if (!string.IsNullOrEmpty(ExceptionMessage))
                {
                    streamWriter.WriteLine("Exception Message:" + ExceptionMessage);
                }
                if (!string.IsNullOrEmpty(InnerExceptionMessage))
                {
                    streamWriter.WriteLine("Inner Exception Message:" + InnerExceptionMessage);
                }

            }
            finally
            {
                if (streamWriter != null) streamWriter.Close();
                if (fileStream != null) fileStream.Close();
            }

        }

        /// <summary>
        /// Get the data table by page size, Data table must have Index column.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public static DataTable GetDataTableByPazeSize(DataTable dt, Int32 PageIndex, Int32 PageSize)
        {
            DataTable TempTable = null;
            try
            {
                Int32 FromtRecordIndex = PageSize * PageIndex + 1;
                Int32 ToRecordIndex = PageSize * (PageIndex + 1);
                TempTable = dt.Clone();

                DataRow[] dr = dt.Select("Index>=" + FromtRecordIndex + " and Index<=" + ToRecordIndex + "");
                foreach (var item in dr)
                {
                    TempTable.Rows.Add(item.ItemArray);
                }

                return TempTable;
            }
            catch (Exception ex)
            {
                LoggerFactory.Logger.Log("GetDataTableByPazeSize", LogEventType.Error, ex.Message);
                return TempTable;
            }
        }
        public static string ConvertObjectToXMLString(object classObject)
        {
            string xmlString = null;
            XmlSerializer xmlSerializer = new XmlSerializer(classObject.GetType());
            using (MemoryStream memoryStream = new MemoryStream())
            {
                xmlSerializer.Serialize(memoryStream, classObject);
                memoryStream.Position = 0;
                xmlString = new StreamReader(memoryStream).ReadToEnd();
            }
            return xmlString;
        }

        /// <summary>
        /// Get Message By Key
        /// </summary>
        /// <param name="Keys"></param>
        /// <returns></returns>
        //public static string GetMessageByKey(string[] Keys)
        //{
        //    string finalMessage = string.Empty;
        //    string text= string.Empty;
        //    using (var streamReader = new StreamReader(HttpContext.Current.Server.MapPath("~/Content/messages.js"), Encoding.UTF8))
        //    {
        //        text = streamReader.ReadToEnd();
        //    }

        //    string JsonMessages = text.Split('=')[1].TrimStart('\'').TrimEnd('\'');
        //    JArray allMessages = JArray.Parse(JsonMessages);
        //    for (Int32 i = 0; i < Keys.Length; i++)
        //    {
        //        var messageKey = Keys[i];
        //        var messageText = allMessages.Where(x => x["MessageKey"].ToString() == messageKey).Select(x => x["MessageText"].ToString()).FirstOrDefault();
        //        if (messageText != null)
        //        {
        //            finalMessage += messageText;
        //        }

        //    }
        //    return finalMessage;
        //}
        /// <summary>Remove_DataTable_Column BY VRT:24:02:2016
        /// 
        /// </summary>
        /// <param name="_dataTable"></param>
        /// <param name="desiredSize"></param>
        /// <returns></returns>
        public static DataTable Remove_DataTable_Column(DataTable _dataTable, int desiredSize)
        {
            while (_dataTable.Columns.Count > desiredSize)
            {
                _dataTable.Columns.RemoveAt(desiredSize);
            }
            _dataTable.AcceptChanges();
            return _dataTable;
        }

        public static string GetCycleNameByDayRange(int NumberOfDays)
        {
            string CycleName = string.Empty;
            if (NumberOfDays >= 1 && NumberOfDays <= 15)
            {
                CycleName = "Weekly";
            }
            else if (NumberOfDays >= 30 && NumberOfDays <= 40)
            {
                CycleName = "Monthly";
            }
            else if (NumberOfDays >= 90 && NumberOfDays <= 120)
            {
                CycleName = "Quarterly";
            }
            return CycleName;
        }
    }

    /// <summary>VRT:28:02:2016: "ddd, dd MMM yyyy" Extension Methods Global Varriables==>>
    /// 
    /// </summary>
    /// <param name="Val"></param>
    /// <returns></returns>
    public static class CommonExtensionMethods
    {
        public static string CommonDateFormate(this DateTime dt)
        {
            return dt.ToString("ddd, dd MMM yyyy");
        }
        public static string UpdateDateFormate(this DateTime dt)
        {
            return dt.ToString("dd MMM yyyy");
        }

        //public static string DataTableToJson(this DataTable data)
        //{
        //    string JsonString = string.Empty;
        //    JsonString = JsonConvert.SerializeObject(data);
        //    return JsonString;
        //}

        //VRT:31-07-2017: Extension Method for Convert List To DataTable
        public static DataTable ConvertListToDataTable<T>(this List<T> iList)
        {
            DataTable dataTable = new DataTable();
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            for (Int32 i = 0; i < props.Count; i++)
            {
                PropertyDescriptor propertyDescriptor = props[i];
                Type type = propertyDescriptor.PropertyType;

                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    type = Nullable.GetUnderlyingType(type);

                dataTable.Columns.Add(propertyDescriptor.Name, type);
            }
            object[] values = new object[props.Count];
            foreach (T iListItem in iList)
            {
                for (Int32 i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(iListItem);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }


        public static List<T> ConvertToList<T>(this DataTable dt)
        {
            const System.Reflection.BindingFlags flags = System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance;
            var columnNames = dt.Columns.Cast<DataColumn>()
                .Select(c => c.ColumnName)
                .ToList();
            var objectProperties = typeof(T).GetProperties(flags);
            var targetList = dt.AsEnumerable().Select(dataRow =>
            {
                var instanceOfT = Activator.CreateInstance<T>();

                foreach (var properties in objectProperties.Where(properties => columnNames.Contains(properties.Name) && dataRow[properties.Name] != DBNull.Value))
                {
                    properties.SetValue(instanceOfT, dataRow[properties.Name], null);
                }
                return instanceOfT;
            }).ToList();

            return targetList;
        }
    }


    public static class DataTableExtensions
    {
        /// <summary>
        /// SetOrdinal of DataTable columns based on the index of the columnNames array. Removes invalid column names first.
        /// </summary>
        /// <param name="table"></param>
        /// <param name="columnNames"></param>
        /// <remarks> http://stackoverflow.com/questions/3757997/how-to-change-datatable-colums-order</remarks>
        public static void SetColumnsOrder(this DataTable dtbl, params String[] columnNames)
        {
            List<string> listColNames = columnNames.ToList();

            //Remove invalid column names.
            foreach (string colName in columnNames)
            {
                if (!dtbl.Columns.Contains(colName))
                {
                    listColNames.Remove(colName);
                }
            }

            foreach (string colName in listColNames)
            {
                dtbl.Columns[colName].SetOrdinal(listColNames.IndexOf(colName));
            }
        }
    }
}