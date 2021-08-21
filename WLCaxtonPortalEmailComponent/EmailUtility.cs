//==========================================================================================
// Project              : WLCaxtonWebPortal
// File Name            : EmailUtility.cs
// Program Description  : Utility fucntion for Email stuff                    
// Programmed By        : Naushad Ali.
// Programmed On        : 30-Jan-2013
// Version              : 1.0.0
//==========================================================================================


#region Namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.IO;

#endregion
namespace WLCaxtonPortalEmailComponent
{
    internal class EmailUtility
    {
        #region Internal methods

        /// <summary>
        /// Serialize the object into xml 
        /// </summary>
        /// <typeparam name="T">Object type to be Serialized</typeparam>
        /// <param name="value">value of obeject</param>
        /// <param name="serializeXml">Serialized xml</param>
        /// <returns></returns>
        internal static bool Serialize<T>(T value, ref string serializeXml)
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

                XmlSerializerNamespaces nameSpace = new XmlSerializerNamespaces();
                nameSpace.Add("", "");

                XmlWriter writer = XmlWriter.Create(stringWriter, settings);
                xmlserializer.Serialize(writer, value, nameSpace);
                serializeXml = Convert.ToString(stringWriter);
                writer.Close();
                return true;
            }
            catch
            {
                throw;
            }
        }  
        #endregion
    }
}
