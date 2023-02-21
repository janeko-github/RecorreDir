using System;
using System.IO;
using System.Collections.Generic;
using System.Xml;
namespace RecorreDir
{
    public static class ChkCTRL
    {


        internal static Save2Log log;
        internal static Int16 numtabs;
        internal static string cPath;
        internal static string GetTabs()
        {
            return new String('\t', numtabs);
        }
        public static void check(XmlNode rootPlastic, Save2Log log2,string filePath)
        {
            numtabs = 5;
            log = log2;
            cPath = filePath;
            if (rootPlastic.HasChildNodes)
            {
                foreach(XmlNode node in rootPlastic.ChildNodes)
                {
                    switch (node.Name)
                    {
                        case "httpmonitor":
                            Check_httpmonitor(node);
                        break;
                        case "httpcontrol":
                            Check_httpcontrol(node);
                            break;
                        case "httpplasticax":
                            Check_httpplasticax(node);
                            break;
                        case "parameters":
                            //Check_parameters(node);
                            break;
                        case "module":
                            Check_module(node);
                            break;
                        case "source":
                            Check_source(node);
                            break;
                        default:
                            //No debería estar en ese fichero
                            break;
                    }

                }

            }
            else
                log.Log($"{GetTabs()} No hay nodos hijos ", 2);


        }
        internal static bool Check_Attribute(XmlNode node,string attrb,bool required = false)
        {
            if(node.Attributes.GetNamedItem(attrb) == null)
            {
                if (required)
                {
                    log.Log($"{GetTabs()} {attrb} No está definido ", 2);
                    return false;
                }
                else
                    return true;
            }
            string value = node.Attributes.GetNamedItem(attrb).Value;
            if (value != "")
                log.Log($"{GetTabs()} {attrb} = {value} ");
            else
                if (required)
                {
                    log.Log($"{GetTabs()} {attrb} vacia ", 2);
                    return false;
                }
            else
                log.Log($"{GetTabs()} {attrb} No Necesario ");
            return true;

        }
        internal static bool Check_File(XmlNode node, string attrb, bool required = true)
        {
            if (node.Attributes.GetNamedItem(attrb) == null)
            {
                if (required)
                {
                    log.Log($"{GetTabs()} {attrb} No está definido el fichero ", 2);
                    return false;
                }
                else
                    return true;
            }
            string value = node.Attributes.GetNamedItem(attrb).Value;
            if (value != "")
            {
                string fileFullName = $"{cPath}/{value}";
                if (!File.Exists(fileFullName))
                {
                    log.Log($"{GetTabs()} '{attrb}' No existe el fichero {value} {fileFullName}", 2);
                    return false; 
                }
                else
                    log.Log($"{GetTabs()} {attrb} = {value} ");
            }

            else
                if (required)
                {
                    log.Log($"{GetTabs()} {attrb} Sin fichero por definir ", 2);
                    return false;
                }
                else
                    log.Log($"{GetTabs()} {attrb} Fichero No Necesario ");
            return true;

        }
        internal static void Check_httpmonitor(XmlNode node)
        {
            log.Log($"{GetTabs()} Tag {node.Name} ");
            numtabs++;
            Check_Attribute(node, "ip", true);
            Check_Attribute(node, "port", true);
            numtabs--;

        }
        internal static void Check_httpcontrol(XmlNode node)
        {
            log.Log($"{GetTabs()} Tag {node.Name} ");
            numtabs++;
            Check_Attribute(node, "ip", true);
            Check_Attribute(node, "port", true);
            numtabs--;
        }
        internal static void Check_httpplasticax(XmlNode node)
        {
            log.Log($"{GetTabs()} Tag {node.Name} ");
            numtabs++;
            Check_Attribute(node, "ip", true);
            Check_Attribute(node, "port", true);
            numtabs--;
        }
        internal static void Check_parameters(XmlNode node)
        {
            log.Log($"{GetTabs()} Tag {node.Name} ");
            numtabs++;
            Check_Attribute(node, "interrogation_ms", true);
            Check_Attribute(node, "log_interrogation");
            Check_Attribute(node, "log_incoming");
            Check_Attribute(node, "log_separate_security_log");
            Check_Attribute(node, "datalog_enabled");
            Check_Attribute(node, "datalog_filter");
            Check_Attribute(node, "datalog_mininterval");
            Check_Attribute(node, "expiration_seconds");
            Check_Attribute(node, "limit_datalog_days", true);
            Check_Attribute(node, "hide_module_control");
            Check_Attribute(node, "system_password");
            Check_Attribute(node, "default_module");
            Check_Attribute(node, "reload_files");
            numtabs--;
        }
        internal static void Check_module(XmlNode node)
        {
            log.Log($"{GetTabs()} Tag {node.Name} ");
            numtabs++;
            Check_Attribute(node, "name", true);
            Check_Attribute(node, "runonstart", true);
            Check_Attribute(node, "controladdress");
            Check_Attribute(node, "controlvaluemin");
            Check_Attribute(node, "controlvaluemax");
            Check_Attribute(node, "arg1");
            Check_Attribute(node, "arg2");
            Check_Attribute(node, "arg3");
            Check_Attribute(node, "inhibit_manual",true);
            numtabs--;
        }
        internal static void Check_source(XmlNode node)
        {
            log.Log($"{GetTabs()} Tag {node.Name} ");
            numtabs++;
            Check_Attribute(node, "ip", true);
            Check_Attribute(node, "port", true);
            Check_Attribute(node, "address_prefix");
            XmlNode aFolder = node.FirstChild;
            Check_Attribute(node, "path", true);
            Check_File(node, "file");


            numtabs--;
        }
    }
}
