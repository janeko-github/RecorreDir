using System.Xml;
using System.Collections.Generic;

namespace Plastic_Analizer
{
    public static class ChkPADL
    {
        internal static short numtabs;
        internal static List<string> folderNames = new List<string>();
        internal static List<string> ids = new List<string>();
        internal static List<string> addresses = new List<string>();
        public static void check(XmlNode rootPlastic)
        {
            numtabs = 5;
            short chkFolder = 0;
            string nameOfFolder = "";
            folderNames.Clear();
            if (rootPlastic.HasChildNodes)
            {
                foreach (XmlNode node in rootPlastic.ChildNodes)
                {
                    switch (node.Name)
                    {
                        case "folder":
                            if (node.Attributes.GetNamedItem("name") == null)
                            {
                                MainClass.log2.Log($"{MainClass.GetTabs(numtabs)} Tag {node.Name} atributo name  no existe ", 2);
                                continue;
                            }
                            ids.Clear();
                            addresses.Clear();
                            nameOfFolder = node.Attributes.GetNamedItem("name").Value;

                            Check_Folder(node);
                            if (folderNames.Contains(nameOfFolder))
                            {
                                MainClass.log2.Log($"{MainClass.GetTabs(numtabs)} Tag {node.Name} Folder {nameOfFolder} repetido ", 2);
                            }
                            else
                                folderNames.Add(nameOfFolder);
                            chkFolder++;
                            break;
                        default:
                            //No debería estar en ese fichero
                            MainClass.log2.Log($"{MainClass.GetTabs(numtabs)} Tag {node.Name} desconocida ", 1);
                            break;
                    }
                }
                //comprobación de existencia de mínimos
                MainClass.checkExistence(val2Check: chkFolder, minimumValue: 1,maxValue: 200, nameTag: "folder");


            }
            else
                MainClass.log2.Log($"{MainClass.GetTabs(numtabs)} No hay nodos hijos ", 2);
        }

        //TODO que no se repitan ni los id ni las address
        internal static void Check_Folder(XmlNode node)
        {
            short nodeCount = 0;
            MainClass.log2.Log($"{MainClass.GetTabs(numtabs)} Tag {node.Name} ");
            numtabs++;
            MainClass.Check_Attribute(node: node, attrb: "name",  nTabs: numtabs, defaultValue: "*", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
            MainClass.Check_Attribute(node: node, attrb: "display_name",  nTabs: numtabs);
            MainClass.Check_Attribute(node: node, attrb: "flag_scheduler",  nTabs: numtabs, defaultValue: "0");
            MainClass.Check_Attribute(node: node, attrb: "expiration_seconds",  nTabs: numtabs);
            if (node.HasChildNodes)
            {
                string id = "";
                string address = "";
                foreach (XmlNode aListen in node.ChildNodes)
                {
                    nodeCount++;
                    //TODO En el fichero file debe de existir un folder con name = al atributo path source
                    if (aListen.Name == "address")
                    {
                        if (aListen.Attributes.GetNamedItem("id") == null)
                        {
                            MainClass.log2.Log($"{MainClass.GetTabs(numtabs)} Tag {aListen.Name} atributo id no existe ", 2);
                            continue;
                        }
                        id = aListen.Attributes.GetNamedItem("id").Value;
                        if (aListen.Attributes.GetNamedItem("address") == null)
                        {
                            MainClass.log2.Log($"{MainClass.GetTabs(numtabs)} Tag {aListen.Name} atributo address no existe ", 2);
                            continue;
                        }
                        address = aListen.Attributes.GetNamedItem("address").Value;
                        if (ids.Contains(id))
                        {
                            MainClass.log2.Log($"{MainClass.GetTabs(numtabs)} Tag {aListen.Name} id {id} repetido ", 2);
                        }
                        else
                            ids.Add(id);
                        if (addresses.Contains(address))
                        {
                            MainClass.log2.Log($"{MainClass.GetTabs(numtabs)} Tag {aListen.Name} address {address} repetido ", 2);
                        }
                        else
                            addresses.Add(address);
                        MainClass.Check_Attribute(node: aListen, attrb: "id"               ,  nTabs: numtabs, defaultValue: "*", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
                        MainClass.Check_Attribute(node: aListen, attrb: "address"          ,  nTabs: numtabs, defaultValue: "*", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
                        MainClass.Check_Attribute(node: aListen, attrb: "description"      ,  nTabs: numtabs, defaultValue: "*");
                        MainClass.Check_Attribute(node: aListen, attrb: "display_name"     ,  nTabs: numtabs, defaultValue: "*");
                        MainClass.Check_Attribute(node: aListen, attrb: "export_name",  nTabs: numtabs);
                        MainClass.Check_Attribute(node: aListen, attrb: "physicalvaluetype",  nTabs: numtabs, defaultValue: "*", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
                        MainClass.Check_Attribute(node: aListen, attrb: "valuetype",  nTabs: numtabs, defaultValue: "*", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
                        MainClass.Check_Attribute(node: aListen, attrb: "flag_interrogate",  nTabs: numtabs, defaultValue: "1");
                        MainClass.Check_Attribute(node: aListen, attrb: "flag_scheduler",  nTabs: numtabs, defaultValue: "0");
                        MainClass.Check_Attribute(node: aListen, attrb: "flag_interrogate_on_write",  nTabs: numtabs);
                        MainClass.Check_Attribute(node: aListen, attrb: "flag_trend_log",  nTabs: numtabs, defaultValue: "0");
                        MainClass.Check_Attribute(node: aListen, attrb: "bac_state_text",  nTabs: numtabs);
                        MainClass.Check_Attribute(node: aListen, attrb: "units",  nTabs: numtabs, defaultValue: "NO_UNITS");
                        MainClass.Check_Attribute(node: aListen, attrb: "trend_log_sample_time",  nTabs: numtabs, defaultValue: "0");
                        MainClass.Check_Attribute(node: aListen, attrb: "factor",  nTabs: numtabs, defaultValue: "1");
                        MainClass.Check_Attribute(node: aListen, attrb: "sumando",  nTabs: numtabs, defaultValue: "0");
                        MainClass.Check_Attribute(node: aListen, attrb: "manualoverridetime",  nTabs: numtabs, defaultValue: "0");
                        MainClass.Check_Attribute(node: aListen, attrb: "acoffset",  nTabs: numtabs);
                        MainClass.Check_Attribute(node: aListen, attrb: "fict",  nTabs: numtabs, defaultValue: "0");
                        MainClass.Check_Attribute(node: aListen, attrb: "persist",  nTabs: numtabs, defaultValue: "1");
                        MainClass.Check_Attribute(node: aListen, attrb: "datalog",  nTabs: numtabs, defaultValue: "0");
                        MainClass.Check_Attribute(node: aListen, attrb: "transform",  nTabs: numtabs);
                        MainClass.Check_Attribute(node: aListen, attrb: "transform_arg",  nTabs: numtabs);
                        MainClass.Check_Attribute(node: aListen, attrb: "changed",  nTabs: numtabs);
                        MainClass.Check_Attribute(node: aListen, attrb: "changed_arg_1",  nTabs: numtabs);
                        MainClass.Check_Attribute(node: aListen, attrb: "changed_arg_2",  nTabs: numtabs);
                        MainClass.Check_Attribute(node: aListen, attrb: "log_min_change"       ,  nTabs: numtabs, defaultValue: "0");
                        MainClass.Check_Attribute(node: aListen, attrb: "rt_image_policy"      ,  nTabs: numtabs);
                        MainClass.Check_Attribute(node: aListen, attrb: "manual_inhibit_wait"  ,  nTabs: numtabs, defaultValue: "0");
                        MainClass.Check_Attribute(node: aListen, attrb: "invert_bytes_on_write",  nTabs: numtabs, defaultValue: "0");
                        MainClass.Check_Attribute(node: aListen, attrb: "bac_object_type"      ,  nTabs: numtabs, defaultValue: "Automatic");
    

                    }
                    else
                    {
                        MainClass.log2.Log($"{MainClass.GetTabs(numtabs)} Tag {node.Name} Existe un tag denominado {aListen.Name} que no es un folder.", 1);
                    }
                }
                MainClass.checkExistence(val2Check: nodeCount, minimumValue: 1,maxValue: 300, nameTag: "address");

            }
            else
            {
                MainClass.log2.Log($"{MainClass.GetTabs(numtabs)} Tag {node.Name} Debería tener al menos un tag listen", 2);
            }

            numtabs--;
        }
    }
}
