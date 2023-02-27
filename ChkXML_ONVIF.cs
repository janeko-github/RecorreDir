using System.Xml;
namespace Plastic_Analizer
{
    public static class ChkXML_ONVIF
    {
        internal static short numtabs;
        public static void check(XmlNode rootPlastic)
        {
            numtabs = 5;
            short chkONVIF = 0;
            short chkDevice = 0;

            if (rootPlastic.HasChildNodes)
            {
                foreach (XmlNode node in rootPlastic.ChildNodes)
                {
                    switch (node.Name)
                    {
                        case "listen":
                            Check_listen(node);
                            chkONVIF++;
                            break;
                        case "device":
                            Check_device(node);
                            chkDevice++;
                            break;
                        default:
                            //No debería estar en ese fichero
                            MainClass.log2.Log($"{MainClass.GetTabs(numtabs)} Tag {node.Name} desconocida ", 1);
                            break;
                    }
                }
                //comprobación de existencia de mínimos
                MainClass.checkExistence(val2Check: chkONVIF, minimumValue:1, nameTag: "listen");
                MainClass.checkExistence(val2Check: chkDevice, minimumValue: 1, nameTag: "device");

            }
            else
                MainClass.log2.Log($"{MainClass.GetTabs(numtabs)} No hay nodos hijos ", 2);
        }


        internal static void Check_listen(XmlNode node)
        {
            MainClass.log2.Log($"{MainClass.GetTabs(numtabs)} Tag {node.Name} ");
            numtabs++;
            MainClass.Check_Attribute(node: node, attrb: "ip",  nTabs: numtabs, defaultValue: "*", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
            MainClass.Check_Attribute(node: node, attrb: "port",  nTabs: numtabs, defaultValue: "1", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
            numtabs--;
        }
        internal static void Check_device(XmlNode node)
        {
            short nodeCount = 0;
            MainClass.log2.Log($"{MainClass.GetTabs(numtabs)} Tag {node.Name} ");
            numtabs++;
            MainClass.Check_Attribute(node: node, attrb: "ident", nTabs: numtabs, defaultValue: "*", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
            MainClass.Check_Attribute(node: node, attrb: "install_id", nTabs: numtabs, defaultValue: "*", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
            MainClass.Check_Attribute(node: node, attrb: "video_server", nTabs: numtabs, defaultValue: "*", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
            MainClass.Check_Attribute(node: node, attrb: "ip", nTabs: numtabs, defaultValue: "*", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
            MainClass.Check_Attribute(node: node, attrb: "user", nTabs: numtabs, defaultValue: "*", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
            MainClass.Check_Attribute(node: node, attrb: "password", nTabs: numtabs, defaultValue: "*", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
            MainClass.Check_Attribute(node: node, attrb: "bosch", nTabs: numtabs, defaultValue: "*", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
            MainClass.Check_Attribute(node: node, attrb: "pva", nTabs: numtabs, defaultValue: "*", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
            if (node.HasChildNodes)
            {
                foreach (XmlNode aDevice in node.ChildNodes)
                {
                    nodeCount++;

                    if (aDevice.Name == "direction")
                    {
                        MainClass.Check_Attribute(node: aDevice, attrb: "value", nTabs: numtabs, defaultValue: "0", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
                        MainClass.Check_Attribute(node: aDevice, attrb: "counter", nTabs: numtabs, defaultValue: "*", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
                    }
                    else
                    {
                        MainClass.log2.Log($"{MainClass.GetTabs(numtabs)} Tag {node.Name} Existe un tag denominado {aDevice.Name} que no es un tag direction.", 1);
                    }
                }
                MainClass.checkExistence(val2Check: nodeCount, minimumValue: 1, nameTag: "device");

            }
            else
            {
                MainClass.log2.Log($"{MainClass.GetTabs(numtabs)} Tag {node.Name} Debería tener al menos un tag direction", 2);
            }

            numtabs--;
        }
    }


}
