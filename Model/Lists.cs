using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Model
{
    public static class Lists
    {
        private static readonly string BasePath = AppDomain.CurrentDomain.BaseDirectory + "\\Lists\\";

        public static IEnumerable<string> GetUnits()
        {
            using var fileStream = new FileStream(BasePath + "units.xml", FileMode.Open);
            var xmlReader = new XmlSerializer(typeof(List<string>));
            return (IEnumerable<string>)xmlReader.Deserialize(fileStream);
        }

        public static IEnumerable<string> GetMaterialsAccounts()
        {
            using var fileStream = new FileStream(BasePath + "MaterialsAccounts.xml", FileMode.Open);
            var xmlReader = new XmlSerializer(typeof(List<string>));
            return (IEnumerable<string>)xmlReader.Deserialize(fileStream);
        }

        public static IEnumerable<string> GetLogAccounts()
        {
            using var fileStream = new FileStream(BasePath + "LogAccounts.xml", FileMode.Open);
            var xmlReader = new XmlSerializer(typeof(List<string>));
            return (IEnumerable<string>)xmlReader.Deserialize(fileStream);
        }

        public static IEnumerable<string> GetCountries()
        {
            using var fileStream = new FileStream(BasePath + "countries.xml", FileMode.Open);
            var xmlReader = new XmlSerializer(typeof(List<string>));
            return (IEnumerable<string>)xmlReader.Deserialize(fileStream);
        }
        
        public static IEnumerable<string> GetDocumentTypes()
        {
            using var fileStream = new FileStream(BasePath + "DocumentsTypes.xml", FileMode.Open);
            var xmlReader = new XmlSerializer(typeof(List<string>));
            return (IEnumerable<string>)xmlReader.Deserialize(fileStream);
        }

        public static List<string> GetEducationList()
        {
            using var fileStream = new FileStream(BasePath + "Educations.xml", FileMode.Open);
            var xmlReader = new XmlSerializer(typeof(List<string>));
            return (List<string>)xmlReader.Deserialize(fileStream);
        }

    }
}
