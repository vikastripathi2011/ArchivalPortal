//==========================================================================================
// Project              : WLCaxtonWebPortal
// File Name            : Util.cs
// Program Description  : This class provide the common utility for bussiness class
// Programmed By        : Naushad Ali.
// Programmed On        : 26-December-2012
// Version              : 1.0.0
//==========================================================================================
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using WLCaxtonPortalExceptionLogger;

namespace WLCaxtonPortalBusinessLogicLayer
{
    /// <summary>
    /// This class provide the common utility for bussiness class
    /// </summary>
    public static class Util
    {
        public static bool Serialize<T>(T value, ref string serializeXml)
        {
            if (value == null)
            {
                return false;
            }
            try
            {
                XmlSerializer xmlserializer = new XmlSerializer(typeof(T));
                StringWriter stringWriter = new StringWriter();

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.OmitXmlDeclaration = true;

                XmlWriter writer = XmlWriter.Create(stringWriter, settings);
                xmlserializer.Serialize(writer, value);
                serializeXml = Convert.ToString(stringWriter);
                writer.Close();
                return true;
            }
            catch (Exception ex)
            {
                LoggerFactory.Logger.Log("Util", LogEventType.Error, ex.Message);
                throw;
            }
        }
    }
}
