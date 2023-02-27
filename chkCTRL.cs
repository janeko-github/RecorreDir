using System;
using System.IO;
using System.Collections.Generic;
using System.Xml;

namespace Plastic_Analizer
{
    public static class ChkCTRL
    {


        internal static short numtabs;
        internal static List<string> modulos = new List<string>();
        public static void check(XmlNode rootPlastic)
        {
            numtabs = 5;
            short chkHttpmonitor = 0; short chkHttpcontrol = 0;  short chkHttpplasticax = 0; short chkParameters = 0; short chkModule = 0; short chkSource = 0;

            if (rootPlastic.HasChildNodes)
            {
                foreach(XmlNode node in rootPlastic.ChildNodes)
                {
                    switch (node.Name)
                    {
                        case "httpmonitor":
                            Check_httpmonitor(node);
                            chkHttpmonitor++;
                        break;
                        case "httpcontrol":
                            Check_httpcontrol(node);
                            chkHttpcontrol++;
                            break;
                        case "httpplasticax":
                            Check_httpplasticax(node);
                            chkHttpplasticax++;
                            break;
                        case "parameters":
                            //Check_parameters(node);
                            chkParameters++;
                            break;
                        case "module":
                            Check_module(node);
                            chkModule++;
                            //para comprobar que estan todos los modulos necesarios
                            modulos.Add(node.Attributes.GetNamedItem("name").Value);
                            break;
                        case "source":
                             chkSource++;
                            Check_source(node);
                            break;
                        default:
                            //No debería estar en ese fichero
                            MainClass.log2.Log($"{MainClass.GetTabs(numtabs)} Tag {node.Name} desconocida ", 1);
                        break;
                    }
                }
                //comprobación de existencia de mínimos
                MainClass.checkExistence(val2Check: chkHttpmonitor, minimumValue: 1, nameTag: "httpmonitor");
                MainClass.checkExistence(val2Check: chkHttpcontrol, minimumValue: 1, nameTag: "httpcontrol");
                MainClass.checkExistence(val2Check: chkHttpplasticax, minimumValue: 1, nameTag: "httpplasticax");
                MainClass.checkExistence(val2Check: chkParameters, minimumValue: 1, nameTag: "parameters");
                MainClass.checkExistence(val2Check: chkModule , minimumValue: 4, maxValue: 100, nameTag: "algun module");
                MainClass.checkExistence(val2Check: chkSource, minimumValue: 1,maxValue: 100, nameTag: "source");

                if (!modulos.Contains("Polimer")) { MainClass.log2.Log($"{MainClass.GetTabs(numtabs)} Falta el Tag del módulo Polimer ", 2); }
                if (!modulos.Contains("Scada")) { MainClass.log2.Log($"{MainClass.GetTabs(numtabs)} Falta el Tag del módulo Scada ", 2); }
                if (!modulos.Contains("Plasticity")) { MainClass.log2.Log($"{MainClass.GetTabs(numtabs)} Falta el Tag del módulo Platicity ", 2); }
                if (!modulos.Contains("SimpleView")) { MainClass.log2.Log($"{MainClass.GetTabs(numtabs)} Falta el Tag del módulo Platicity ", 2); }
            }
            else
                MainClass.log2.Log($"{MainClass.GetTabs(numtabs)} No hay nodos hijos ", 2);


        }

        #region Checkers
        internal static void Check_httpmonitor(XmlNode node)
        {
            MainClass.log2.Log($"{MainClass.GetTabs(numtabs)} Tag {node.Name} ");
            numtabs++;
            MainClass.Check_Attribute(node: node,attrb: "ip", required: true, nTabs: numtabs,defaultValue: "all");
            MainClass.Check_Attribute(node:node, attrb: "port", required: true, nTabs: numtabs, defaultValue: "8091");
            numtabs--;

        }
        internal static void Check_httpcontrol(XmlNode node)
        {
            MainClass.log2.Log($"{MainClass.GetTabs(numtabs)} Tag {node.Name} ");
            numtabs++;
            MainClass.Check_Attribute(node: node, attrb: "ip", required: true, nTabs: numtabs, defaultValue: "all", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
            MainClass.Check_Attribute(node: node, attrb: "port", required: true, nTabs: numtabs, defaultValue: "default", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
            numtabs--;
        }
        internal static void Check_httpplasticax(XmlNode node)
        {
            MainClass.log2.Log($"{MainClass.GetTabs(numtabs)} Tag {node.Name} ");
            numtabs++;
            MainClass.Check_Attribute(node: node, attrb: "ip", required: true, nTabs: numtabs, defaultValue: "all", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
            MainClass.Check_Attribute(node: node, attrb: "port", required: true, nTabs: numtabs, defaultValue: "default", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
            numtabs--;
        }
        internal static void Check_parameters(XmlNode node)
        {
            MainClass.log2.Log($"{MainClass.GetTabs(numtabs)} Tag {node.Name} ");
            numtabs++;
            MainClass.Check_Attribute(node: node,attrb: "interrogation_ms", required: true, nTabs: numtabs, defaultValue: "100", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
            MainClass.Check_Attribute(node: node,attrb: "log_interrogation", nTabs: numtabs, defaultValue: "0");
            MainClass.Check_Attribute(node: node,attrb: "log_incoming", nTabs: numtabs, defaultValue: "0");
            MainClass.Check_Attribute(node: node,attrb: "log_separate_security_log", nTabs: numtabs, defaultValue: "0");
            MainClass.Check_Attribute(node: node,attrb: "datalog_enabled", nTabs: numtabs, defaultValue: "1");
            MainClass.Check_Attribute(node: node,attrb: "datalog_filter", nTabs: numtabs, defaultValue: "0");
            MainClass.Check_Attribute(node: node,attrb: "datalog_mininterval", nTabs: numtabs, defaultValue: "0");
            MainClass.Check_Attribute(node: node,attrb: "expiration_seconds", nTabs: numtabs, defaultValue: "600");
            MainClass.Check_Attribute(node: node,attrb: "limit_datalog_days", required: true, nTabs: numtabs, defaultValue: "0");
            MainClass.Check_Attribute(node: node,attrb: "hide_module_control", nTabs: numtabs, defaultValue: "0");
            MainClass.Check_Attribute(node: node,attrb: "system_password", nTabs: numtabs);
            MainClass.Check_Attribute(node: node,attrb: "default_module", nTabs: numtabs);
            MainClass.Check_Attribute(node: node,attrb: "reload_files", nTabs: numtabs, defaultValue: "False");
            numtabs--;
        }
        internal static void Check_module(XmlNode node)
        {
            MainClass.log2.Log($"{MainClass.GetTabs(numtabs)} Tag {node.Name} ");
            numtabs++;
            MainClass.Check_Attribute(node: node,attrb: "name", required: true, nTabs: numtabs, flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
            MainClass.Check_Attribute(node: node,attrb: "runonstart", required: true, nTabs: numtabs, defaultValue: "1", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
            MainClass.Check_Attribute(node: node,attrb: "controladdress", nTabs: numtabs);
            MainClass.Check_Attribute(node: node,attrb: "controlvaluemin", nTabs: numtabs);
            MainClass.Check_Attribute(node: node,attrb: "controlvaluemax", nTabs: numtabs);
            MainClass.Check_Attribute(node: node,attrb: "arg2", nTabs: numtabs);
            MainClass.Check_Attribute(node: node,attrb: "arg3", nTabs: numtabs);
            MainClass.Check_Attribute(node: node,attrb: "inhibit_manual", required: true, nTabs: numtabs);
            switch(node.Attributes.GetNamedItem("name").Value) {
                case "Polimer":
                    MainClass.Check_Attribute(node: node, attrb: "arg1", required: true,nTabs: numtabs, defaultValue: "*", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
                    break;
                case "Plasticity":
                case "SimpleView":
                    MainClass.Check_Attribute(node: node, attrb: "arg1", nTabs: numtabs);
                    break;
                case "Scada":
                    MainClass.Check_Attribute(node: node, attrb: "arg1", nTabs: numtabs);
                    //tiene hijos que son ficheros y han de existir
                    if (node.HasChildNodes)
                    {
                        foreach (XmlNode file in node.ChildNodes)
                        {
                            if (file.Name == "input")
                            {
                                MainClass.Check_File(node: file, attrb: "file", nTabs: numtabs);
                            }
                            else
                            {
                                MainClass.log2.Log($"{MainClass.GetTabs(numtabs)} Tag {node.Name} atributo name scada tiene una tag hijo {file.Name} desconocido", 1);
                            }
                        }
                    }else
                        MainClass.log2.Log($"{MainClass.GetTabs(numtabs)} Tag {node.Name} tag module scada debería tener al menos un tag input", 2);
                    break;
                default:
                    break;
            }



            numtabs--;
        }
        internal static void Check_source(XmlNode node)
        {
            MainClass.log2.Log($"{MainClass.GetTabs(numtabs)} Tag {node.Name} ");
            numtabs++;
            MainClass.Check_Attribute(node: node,attrb: "ip", required: true, nTabs: numtabs, defaultValue: "loopback", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
            MainClass.Check_Attribute(node: node,attrb: "port", required: true, nTabs: numtabs, defaultValue: "*", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
            MainClass.Check_Attribute(node: node,attrb: "address_prefix", nTabs: numtabs);
            if (node.HasChildNodes)
            {
                foreach (XmlNode aFolder in node.ChildNodes)
                {
                    //TODO En el fichero file debe de existir un folder con name = al atributo path source
                    if (aFolder.Name == "folder")
                    {
                        MainClass.Check_File(node: aFolder, attrb: "file", required: true, nTabs: numtabs);
                        MainClass.Check_Attribute(node: aFolder, attrb: "path", required: true, nTabs: numtabs, defaultValue: "*", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
                    }
                    else
                    {
                        MainClass.log2.Log($"{MainClass.GetTabs(numtabs)} Tag {node.Name} Existe un tag denominado {aFolder.Name} que no es un folder.", 1);
                    }
                }
            }
            else
            {
                MainClass.log2.Log($"{MainClass.GetTabs(numtabs)} Tag {node.Name} Debería tener al menos un tag folder", 2);
            }

            numtabs--;
        }
        #endregion Checkers
    }
}
