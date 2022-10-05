/*
Copyright (C) 2019-2022 TARAKHOMYN YURIY IVANOVYCH
All rights reserved.

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

/*
Автор:    Тарахомин Юрій Іванович
Адреса:   Україна, м. Львів
Сайт:     accounting.org.ua
*/

/*

Модуль функцій для звітів

*/

using System.Collections.Generic;
using AccountingSoftware;

using System.IO;
using System.Xml;
using System.Xml.Xsl;

namespace StorageAndTrade_1_0
{
    /// <summary>
    /// Функції для звітів
    /// </summary>
    public class ФункціїДляЗвітів
    {
        /// <summary>
        /// Створює хмл документ та корінну вітку root
        /// </summary>
        /// <returns>хмл документ</returns>
        public static XmlDocument CreateXmlDocument()
        {
            XmlDocument xmlConfDocument = new XmlDocument();
            xmlConfDocument.AppendChild(xmlConfDocument.CreateXmlDeclaration("1.0", "utf-8", ""));

            XmlElement rootNode = xmlConfDocument.CreateElement("root");
            xmlConfDocument.AppendChild(rootNode);

            return xmlConfDocument;
        }

        /// <summary>
        /// Записує блок даних в хмл документ
        /// </summary>
        /// <param name="xmlDoc">Хмл документ</param>
        /// <param name="blockName">Назва блоку даних</param>
        /// <param name="columnsName">Масив стовпчиків</param>
        /// <param name="listRow">Список рядків</param>
        public static void DataToXML(XmlDocument xmlDoc, string blockName, string[] columnsName, List<object[]> listRow)
        {
            XmlNode root = xmlDoc.SelectSingleNode("/root");

            XmlElement rootItemNode = xmlDoc.CreateElement(blockName);
            root.AppendChild(rootItemNode);

            foreach (object[] row in listRow)
            {
                int counter = 0;

                XmlElement nodeRow = xmlDoc.CreateElement("row");
                rootItemNode.AppendChild(nodeRow);

                foreach (string col in columnsName)
                {
                    XmlElement node = xmlDoc.CreateElement(col);
                    if (row[counter].GetType().Name == "UuidAndText")
                        node.InnerXml = ((UuidAndText)row[counter]).ToXml();
                    else
                        node.InnerText = row[counter].ToString();
                    nodeRow.AppendChild(node);

                    counter++;
                }
            }
        }

        /// <summary>
        /// Записує блок даних в ХМЛ документ
        /// </summary>
        /// <param name="xmlDoc">Хмл документ</param>
        /// <param name="blockName">Назва блоку даних</param>
        /// <param name="listRow">Список назва - значення</param>
        public static void DataHeadToXML(XmlDocument xmlDoc, string blockName, List<NameValue<string>> listRow)
        {
            XmlNode root = xmlDoc.SelectSingleNode("/root");

            XmlElement rootItemNode = xmlDoc.CreateElement(blockName);
            root.AppendChild(rootItemNode);

            XmlElement nodeRow = xmlDoc.CreateElement("row");
            rootItemNode.AppendChild(nodeRow);

            foreach (NameValue<string> row in listRow)
            {
                XmlElement node = xmlDoc.CreateElement(row.Name);
                node.InnerText = row.Value;
                nodeRow.AppendChild(node);
            }
        }

        /// <summary>
        /// Записує хмл документ та трансформує його в HTML
        /// </summary>
        /// <param name="xmlDoc">хмл документ</param>
        /// <param name="template">шлях до шаблону XSLT</param>
        /// <param name="openFormReport">Відкрити звіт</param>
        public static void XmlDocumentSaveAndTransform(XmlDocument xmlDoc, string template, bool openFormReport = true, string formReportName = "")
        {
            string pathToFolder = AppContext.BaseDirectory;
            string pathToXmlFile = Path.Combine(pathToFolder, "Report.xml");
            string pathToHtmlFile = Path.Combine(pathToFolder, "Report.html");
            string pathToTemplate = Path.Combine(pathToFolder, template);

            xmlDoc.Save(pathToXmlFile);

            XslCompiledTransform xsltTransform = new XslCompiledTransform();
            xsltTransform.Load(pathToTemplate, new XsltSettings(), null);

            xsltTransform.Transform(pathToXmlFile, pathToHtmlFile);

            //System.Diagnostics.Process.Start("firefox.exe", pathToHtmlFile);

            //if (openFormReport)
             //   OpenFormReport(formReportName, pathToHtmlFile);
        }

        /// <summary>
        /// Відкриває орему форму і виводить звіт
        /// </summary>
        /// <param name="name">Назва форми</param>
        /// <param name="pathToHtmlFile">Шлях до Html File</param>
/*
        public static void OpenFormReport(string name, string pathToHtmlFile)
        {
            Form MdiParent = Application.OpenForms["FormStorageAndTrade"];

            StorageAndTrade.Form_Report form_Report = new StorageAndTrade.Form_Report();
            form_Report.Text += " - " + name;
            form_Report.HtmlDocumentPath = pathToHtmlFile;

            if (MdiParent != null)
                form_Report.MdiParent = MdiParent;

            form_Report.Show();
        }
        */
    }
}

