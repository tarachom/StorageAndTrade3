
using System.Xml;
using System.Xml.XPath;

public class ConfigurationParamCollection
{
    public static List<ConfigurationParam>? ListConfigurationParam { get; set; }

    public static string PathToXML { get; set; } = "";

    public static void Init()
    {
        ListConfigurationParam = new List<ConfigurationParam>();
    }

    public static void LoadConfigurationParamFromXML(string pathToXML)
    {
        Init();

        if (File.Exists(pathToXML))
        {
            XPathDocument xPathDoc = new XPathDocument(pathToXML);
            XPathNavigator xPathDocNavigator = xPathDoc.CreateNavigator();

            XPathNodeIterator? ConfigurationParamNodes = xPathDocNavigator.Select("/root/Configuration");
            while (ConfigurationParamNodes!.MoveNext())
            {
                ConfigurationParam ItemConfigurationParam = new ConfigurationParam();

                XPathNavigator? currentNode = ConfigurationParamNodes?.Current;

                string SelectAttribute = currentNode?.GetAttribute("Select", "") ?? "";
                if (!String.IsNullOrEmpty(SelectAttribute))
                    ItemConfigurationParam.Select = bool.Parse(SelectAttribute);

                ItemConfigurationParam.ConfigurationKey = currentNode?.SelectSingleNode("Key")?.Value ?? "";
                ItemConfigurationParam.ConfigurationName = currentNode?.SelectSingleNode("Name")?.Value ?? "";
                ItemConfigurationParam.DataBaseServer = currentNode?.SelectSingleNode("Server")?.Value ?? "";
                ItemConfigurationParam.DataBasePort = int.Parse(currentNode?.SelectSingleNode("Port")?.Value ?? "0");
                ItemConfigurationParam.DataBaseLogin = currentNode?.SelectSingleNode("Login")?.Value ?? "";
                ItemConfigurationParam.DataBasePassword = currentNode?.SelectSingleNode("Password")?.Value ?? "";
                ItemConfigurationParam.DataBaseBaseName = currentNode?.SelectSingleNode("BaseName")?.Value ?? "";

                ListConfigurationParam?.Add(ItemConfigurationParam);
            }
        }
    }

    public static void SaveConfigurationParamFromXML(string pathToXML)
    {
        if (ListConfigurationParam == null)
            throw new Exception("ListConfigurationParam null");

        XmlDocument xmlConfParamDocument = new XmlDocument();
        xmlConfParamDocument.AppendChild(xmlConfParamDocument.CreateXmlDeclaration("1.0", "utf-8", ""));

        XmlElement rootNode = xmlConfParamDocument.CreateElement("root");
        xmlConfParamDocument.AppendChild(rootNode);

        foreach (ConfigurationParam ItemConfigurationParam in ListConfigurationParam)
        {
            XmlElement configurationNode = xmlConfParamDocument.CreateElement("Configuration");
            rootNode.AppendChild(configurationNode);

            XmlAttribute selectAttribute = xmlConfParamDocument.CreateAttribute("Select");
            selectAttribute.Value = ItemConfigurationParam.Select.ToString();
            configurationNode.Attributes.Append(selectAttribute);

            XmlElement nodeKey = xmlConfParamDocument.CreateElement("Key");
            nodeKey.InnerText = ItemConfigurationParam.ConfigurationKey;
            configurationNode.AppendChild(nodeKey);

            XmlElement nodeName = xmlConfParamDocument.CreateElement("Name");
            nodeName.InnerText = ItemConfigurationParam.ConfigurationName;
            configurationNode.AppendChild(nodeName);

            XmlElement nodeServer = xmlConfParamDocument.CreateElement("Server");
            nodeServer.InnerText = ItemConfigurationParam.DataBaseServer;
            configurationNode.AppendChild(nodeServer);

            XmlElement nodePort = xmlConfParamDocument.CreateElement("Port");
            nodePort.InnerText = ItemConfigurationParam.DataBasePort.ToString();
            configurationNode.AppendChild(nodePort);

            XmlElement nodeLogin = xmlConfParamDocument.CreateElement("Login");
            nodeLogin.InnerText = ItemConfigurationParam.DataBaseLogin;
            configurationNode.AppendChild(nodeLogin);

            XmlElement nodePassword = xmlConfParamDocument.CreateElement("Password");
            nodePassword.InnerText = ItemConfigurationParam.DataBasePassword;
            configurationNode.AppendChild(nodePassword);

            XmlElement nodeBaseName = xmlConfParamDocument.CreateElement("BaseName");
            nodeBaseName.InnerText = ItemConfigurationParam.DataBaseBaseName;
            configurationNode.AppendChild(nodeBaseName);
        }

        xmlConfParamDocument.Save(pathToXML);
    }

    public static ConfigurationParam? GetConfigurationParam(string key)
    {
        ConfigurationParam? selectConfigurationParam = null;

        if (ListConfigurationParam != null)
        {
            foreach (ConfigurationParam itemConfigurationParam in ListConfigurationParam)
            {
                if (itemConfigurationParam.ConfigurationKey == key)
                {
                    selectConfigurationParam = itemConfigurationParam;
                    break;
                }
            }
        }

        return selectConfigurationParam;
    }

    public static bool RemoveConfigurationParam(string key)
    {
        if (ListConfigurationParam != null)
        {
            ConfigurationParam? selectConfigurationParam = GetConfigurationParam(key);

            if (selectConfigurationParam != null)
            {
                ListConfigurationParam.Remove(selectConfigurationParam);
                return true;
            }
            else
                return false;
        }
        return false;
    }
}

public class ConfigurationParam
{
    public ConfigurationParam()
    {
        ConfigurationKey = "";
        ConfigurationName = "";
        DataBaseServer = "localhost";
        DataBasePort = 5432;
        DataBaseLogin = "postgres";
        DataBasePassword = "";
        DataBaseBaseName = "";
        Select = false;
    }

    public string ConfigurationKey { get; set; }

    public string ConfigurationName { get; set; }

    public string DataBaseServer { get; set; }

    public int DataBasePort { get; set; }

    public string DataBaseLogin { get; set; }

    public string DataBasePassword { get; set; }

    public string DataBaseBaseName { get; set; }

    public bool Select { get; set; }

    public override string ToString()
    {
        return String.IsNullOrWhiteSpace(ConfigurationName) ? "<>" : ConfigurationName;
    }

    public static ConfigurationParam New()
    {
        ConfigurationParam configurationParam = new ConfigurationParam();
        configurationParam.ConfigurationKey = Guid.NewGuid().ToString();
        configurationParam.ConfigurationName = "<Новий>";

        return configurationParam;
    }

    public ConfigurationParam Clone()
    {
        ConfigurationParam configurationParam = new ConfigurationParam();
        configurationParam.ConfigurationKey = Guid.NewGuid().ToString();
        configurationParam.ConfigurationName = ConfigurationName + " - Копія";
        configurationParam.DataBaseServer = DataBaseServer;
        configurationParam.DataBaseLogin = DataBaseLogin;
        configurationParam.DataBasePassword = DataBasePassword;
        configurationParam.DataBaseBaseName = DataBaseBaseName;
        configurationParam.DataBasePort = DataBasePort;

        return configurationParam;
    }
}