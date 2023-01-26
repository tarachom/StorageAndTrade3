
using StorageAndTrade_1_0.Документи;

using System.Xml;

class Export
{
    public static void ToXmlFile(ПоступленняТоварівТаПослуг_Pointer ПоступленняТоварівТаПослуг, string pathToSave)
    {
        ПоступленняТоварівТаПослуг_Objest ПоступленняТоварівТаПослуг_Objest = ПоступленняТоварівТаПослуг.GetDocumentObject(true);

        XmlWriter xmlWriter = XmlWriter.Create(pathToSave, new XmlWriterSettings() { Indent = true, Encoding = System.Text.Encoding.UTF8 });
        xmlWriter.WriteStartDocument();
        xmlWriter.WriteStartElement("root");

        xmlWriter.WriteStartElement("Constants");

        xmlWriter.WriteEndElement(); //Constants

        xmlWriter.WriteEndElement(); //root
        xmlWriter.WriteEndDocument();
        xmlWriter.Close();
    }
}