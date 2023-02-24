using System.Xml;
using System.Collections.Generic;

namespace RecorreDir
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

            if (rootPlastic.HasChildNodes)
            {
                foreach (XmlNode node in rootPlastic.ChildNodes)
                {
                    switch (node.Name)
                    {
                        case "folder":
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
            MainClass.Check_Attribute(node: node, attrb: "name", required: true, nTabs: numtabs, defaultValue: "*");
            MainClass.Check_Attribute(node: node, attrb: "display_name", required: true, nTabs: numtabs);
            MainClass.Check_Attribute(node: node, attrb: "flag_scheduler", required: true, nTabs: numtabs, defaultValue: "0");
            MainClass.Check_Attribute(node: node, attrb: "expiration_seconds", required: true, nTabs: numtabs);
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
                        id = aListen.Attributes.GetNamedItem("id").Value;
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
                        MainClass.Check_Attribute(node: aListen, attrb: "id"               , required: true, nTabs: numtabs, defaultValue: "*");
                        MainClass.Check_Attribute(node: aListen, attrb: "address"          , required: true, nTabs: numtabs, defaultValue: "*");
                        MainClass.Check_Attribute(node: aListen, attrb: "description"      , required: true, nTabs: numtabs, defaultValue: "*");
                        MainClass.Check_Attribute(node: aListen, attrb: "display_name"     , required: false, nTabs: numtabs, defaultValue: "*");
                        MainClass.Check_Attribute(node: aListen, attrb: "export_name", required: true, nTabs: numtabs);
                        MainClass.Check_Attribute(node: aListen, attrb: "physicalvaluetype", required: true, nTabs: numtabs, defaultValue: "*");
                        MainClass.Check_Attribute(node: aListen, attrb: "valuetype", required: true, nTabs: numtabs, defaultValue: "*");
                        MainClass.Check_Attribute(node: aListen, attrb: "flag_interrogate", required: true, nTabs: numtabs, defaultValue: "1");
                        MainClass.Check_Attribute(node: aListen, attrb: "flag_scheduler", required: true, nTabs: numtabs, defaultValue: "0");
                        MainClass.Check_Attribute(node: aListen, attrb: "flag_interrogate_on_write", required: true, nTabs: numtabs);
                        MainClass.Check_Attribute(node: aListen, attrb: "flag_trend_log", required: true, nTabs: numtabs, defaultValue: "0");
                        MainClass.Check_Attribute(node: aListen, attrb: "bac_state_text", required: true, nTabs: numtabs);
                        MainClass.Check_Attribute(node: aListen, attrb: "units", required: true, nTabs: numtabs, defaultValue: "NO_UNITS");
                        MainClass.Check_Attribute(node: aListen, attrb: "trend_log_sample_time", required: true, nTabs: numtabs, defaultValue: "0");
                        MainClass.Check_Attribute(node: aListen, attrb: "factor", required: true, nTabs: numtabs, defaultValue: "1");
                        MainClass.Check_Attribute(node: aListen, attrb: "sumando", required: true, nTabs: numtabs, defaultValue: "0");
                        MainClass.Check_Attribute(node: aListen, attrb: "manualoverridetime", required: true, nTabs: numtabs, defaultValue: "0");
                        MainClass.Check_Attribute(node: aListen, attrb: "acoffset", required: true, nTabs: numtabs);
                        MainClass.Check_Attribute(node: aListen, attrb: "fict", required: true, nTabs: numtabs, defaultValue: "0");
                        MainClass.Check_Attribute(node: aListen, attrb: "persist", required: true, nTabs: numtabs, defaultValue: "1");
                        MainClass.Check_Attribute(node: aListen, attrb: "datalog", required: true, nTabs: numtabs, defaultValue: "0");
                        MainClass.Check_Attribute(node: aListen, attrb: "transform", required: true, nTabs: numtabs);
                        MainClass.Check_Attribute(node: aListen, attrb: "transform_arg", required: true, nTabs: numtabs);
                        MainClass.Check_Attribute(node: aListen, attrb: "changed", required: true, nTabs: numtabs);
                        MainClass.Check_Attribute(node: aListen, attrb: "changed_arg_1", required: true, nTabs: numtabs);
                        MainClass.Check_Attribute(node: aListen, attrb: "changed_arg_2", required: true, nTabs: numtabs);
                        MainClass.Check_Attribute(node: aListen, attrb: "log_min_change"       , required: true, nTabs: numtabs, defaultValue: "0");
                        MainClass.Check_Attribute(node: aListen, attrb: "rt_image_policy"      , required: true, nTabs: numtabs);
                        MainClass.Check_Attribute(node: aListen, attrb: "manual_inhibit_wait"  , required: true, nTabs: numtabs, defaultValue: "0");
                        MainClass.Check_Attribute(node: aListen, attrb: "invert_bytes_on_write", required: true, nTabs: numtabs, defaultValue: "0");
                        MainClass.Check_Attribute(node: aListen, attrb: "bac_object_type"      , required: true, nTabs: numtabs, defaultValue: "Automatic");

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
