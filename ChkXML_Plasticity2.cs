using System.Xml;
using System.Collections.Generic;

namespace Plastic_Analizer
{
    public static class ChkXML_Plasticity2
    {
        internal static short numtabs;
        internal static List<string> origins = new List<string>();
        internal static List<string> destinations = new List<string>();
        internal static List<string> blocks = new List<string>();
        internal static List<string> config_var_val = new List<string>();
        public static void check(XmlNode rootPlastic)
        {
            numtabs = 5;
            short chkBlock = 0;
            short chkLink = 0;

            if (rootPlastic.HasChildNodes)
            {
                foreach (XmlNode node in rootPlastic.ChildNodes)
                {
                    switch (node.Name)
                    {

                        case "link":
                            Check_link(node);
                            chkLink++;
                            break;
                        case "block":
                            Check_block(node);  
                            chkBlock++;
                            break;
                        default:
                            //No debería estar en ese fichero
                            MainClass.log2.Log($"{MainClass.GetTabs(numtabs)} Tag {node.Name} desconocida ", 1);
                            break;
                    }
                }
                //comprobación de existencia de mínimos
                MainClass.checkExistence(val2Check: chkBlock, minimumValue:1, nameTag: "block");
                MainClass.checkExistence(val2Check: chkLink, minimumValue: 1, nameTag: "link");

            }
            else
                MainClass.log2.Log($"{MainClass.GetTabs(numtabs)} No hay nodos hijos ", 2);
        }


        internal static void Check_link(XmlNode node)
        {
            MainClass.log2.Log($"{MainClass.GetTabs(numtabs)} Tag {node.Name} ");
            numtabs++;
            string origin = node.Attributes.GetNamedItem("origin").Value;
            string destination = node.Attributes.GetNamedItem("destination").Value;
            if (origins.Contains(origin))
            {
                MainClass.log2.Log($"{MainClass.GetTabs(numtabs)} Tag {node.Name} origin {origin} repetido ", 2);
            }
            else
                origins.Add(origin);
            if (destinations.Contains(destination))
            {
                MainClass.log2.Log($"{MainClass.GetTabs(numtabs)} Tag {node.Name} destination {destination} repetido ", 2);
            }
            else
                destinations.Add(destination);

            MainClass.Check_Attribute(node: node, attrb: "origin",  nTabs: numtabs, defaultValue: "*", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
            MainClass.Check_Attribute(node: node, attrb: "destination",  nTabs: numtabs, defaultValue: "*", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
            MainClass.Check_Attribute(node: node, attrb: "output", nTabs: numtabs, defaultValue: "OUT_?", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
            MainClass.Check_Attribute(node: node, attrb: "parameter", nTabs: numtabs, defaultValue: "IN_?", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
            numtabs--;
        }
        internal static void Check_block(XmlNode node)
        {
            short nodeCount = 0;
            List<string> blocks1 = new List<string>();
            List<string> config_var_val1 = new List<string>();

            MainClass.log2.Log($"{MainClass.GetTabs(numtabs)} Tag {node.Name} ");

            numtabs++;
            string block_type = node.Attributes.GetNamedItem("block_description").Value;
            string datapoint = "";
            string val = "";
            MainClass.Check_Attribute(node: node, attrb: "block_description", nTabs: numtabs, defaultValue: "DATAPOINT", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
            MainClass.Check_Attribute(node: node, attrb: "x", nTabs: numtabs, defaultValue: "*", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
            MainClass.Check_Attribute(node: node, attrb: "y", nTabs: numtabs, defaultValue: "*", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
            MainClass.Check_Attribute(node: node, attrb: "name", nTabs: numtabs, defaultValue: "*", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
            MainClass.Check_Attribute(node: node, attrb: "label", nTabs: numtabs, defaultValue: "*", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
            MainClass.Check_Attribute(node: node, attrb: "ident", nTabs: numtabs, defaultValue: "*", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
            MainClass.Check_Attribute(node: node, attrb: "width", nTabs: numtabs, defaultValue: "100", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
            MainClass.Check_Attribute(node: node, attrb: "height", nTabs: numtabs, defaultValue: "100", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
            if (node.HasChildNodes)
            {

                foreach (XmlNode aTag in node.ChildNodes)
                {
                    nodeCount++;
                    MainClass.Check_Attribute(node: aTag, attrb: "name", nTabs: numtabs, defaultValue: "0", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
                    MainClass.Check_Attribute(node: aTag, attrb: "val", nTabs: numtabs, defaultValue: "*", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
                    datapoint = node.Attributes.GetNamedItem("name").Value;
                    val = node.Attributes.GetNamedItem("val").Value;

                    switch (block_type)
                    {
                        case "DATAPOINT":
                            //config_var que no existan direcciones repetidas
                            if(datapoint== "DATAPOINT")
                            {

                                if (config_var_val.Contains(val))
                                {
                                    MainClass.log2.Log($"{MainClass.GetTabs(numtabs)} Tag {aTag.Name} val {val} repetido ", 2);
                                }
                                else
                                    config_var_val.Add(val);
                            }
                            break;
                        case "FORMULA":
                            //ya veremos que hhacer con esto
                            break;
                        default:
                            MainClass.log2.Log($"{MainClass.GetTabs(numtabs)} Tag {node.Name} Existe un tag denominado {aTag.Name} desconocido.", 1);
                            break;
                    }

                }
                MainClass.checkExistence(val2Check: nodeCount, minimumValue: 1, nameTag: "config_var");

            }
            else
            {
                MainClass.log2.Log($"{MainClass.GetTabs(numtabs)} Tag {node.Name} Debería tener al menos un tag config_var", 2);
            }

            numtabs--;
        }
    }


}
