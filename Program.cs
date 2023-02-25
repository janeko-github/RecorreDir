using System;
using System.IO;
using System.Collections.Generic;
using System.Xml;
using System.Drawing;
using System.Drawing.Imaging;

namespace Plastic_Analizer
{
    class MainClass
    {
        public static string rootdir { get; set; }

        public static Save2Log log2;
        public static List<string> extensiones = new List<string>();
        public static List<string> ficherosProyecto = new List<string>();
        internal static string cPath;
        internal static short nCol;
        internal static short nLin;
        public static short avoidNormalMessages;
        public static void Main(string[] args)
        {
            rootdir = Directory.GetCurrentDirectory();
            if (args.Length == 0)
            {
                rootdir = "/media/janeko/Almacen/celofan/copiacelofan/inst_files/99876515";
                //rootdir = "/media/janeko/Almacen/celofan/copiacelofan/inst_files";
               //rootdir = "/home/janeko/workspace/inst_files/99876321";
                //Console.WriteLine("args is null");
            }
            else
            {
                foreach (string value in args)
                {
                    rootdir = value;
                }
            }
            Console.WriteLine("Inicio en: {0}", rootdir);
            extensiones.Add(".xml");
            extensiones.Add(".yaml");
            extensiones.Add(".toml");
            extensiones.Add(".padl");
            extensiones.Add(".pctrl");
            extensiones.Add(".pprj");
            extensiones.Add(".pgman");
            extensiones.Add(".pcond");
            extensiones.Add(".pe2p");
            extensiones.Add(".pligthc");
            extensiones.Add(".mod2p");
            extensiones.Add(".scav");
            extensiones.Add(".scag");
            log2 = new Save2Log(rootdir + "/Plastic_Análisis.log");
            avoidNormalMessages = 1;
            Console.Write("Inicio proceso de análisis Plastic\n");
            nCol = 0;
            nLin = 2;
        SearchDirectory(rootdir, extension_list: extensiones);
        
            log2.CloseLog();
            Console.Write("\n");
            Console.WriteLine("Finalizado proceso.");

        }
        #region Útiles
        public static string GetTabs(short numtabs=1)
        {
            return new String('\t', numtabs);
        }
        internal static string GetNodePath(XmlNode aNode)
        {
            string cNodePath = aNode.Name;
            while(aNode.ParentNode.Name != "#document")
            {
                aNode = aNode.ParentNode;
                if(aNode.Name == "plastic")
                    cNodePath = $"{aNode.Name}.{aNode.Attributes.GetNamedItem("application").Value}.{cNodePath}";
                else
                    cNodePath = $"{aNode.Name}.{cNodePath}";
            }
            return cNodePath;
        }
        public static string Check_Attribute(XmlNode node, string attrb, bool required = false, short nTabs=1,string defaultValue="")
        {
            string tagName = node.Name;
            /*if (node.Attributes.GetNamedItem("name") != null)
                tagName += "." + node.Attributes.GetNamedItem("name");*/
            if (node.Attributes.GetNamedItem(attrb) == null)
            {

                if (required)
                {
                    log2.Log($"{MainClass.GetTabs(nTabs)} el atributo '{attrb}' No está definido en el tag '{tagName}'. Por defecto podría ser '{defaultValue}' -> {GetNodePath(node)}", 2);
                    return null;
                }
                else
                    return "";
            }
            string value = node.Attributes.GetNamedItem(attrb).Value;
            if (value != "")
            {
                if ((defaultValue != "") && (value != defaultValue) && required)
                    log2.Log($"{MainClass.GetTabs(nTabs)} '{attrb}' = '{value}' ", 1);
                else log2.Log($"{MainClass.GetTabs(nTabs)} '{attrb}' = '{value}' ");
            }
            else
            {
                if (required)
                {
                    if ((defaultValue != "") && (value == ""))
                        log2.Log($"{MainClass.GetTabs(nTabs)} '{attrb}' vacia  -> {GetNodePath(node)}", 2);
                    return value;
                }
                else
                    log2.Log($"{MainClass.GetTabs(nTabs)} '{attrb}' No Necesario en el tag '{tagName}' -> {GetNodePath(node)} ");
            }
            return value;

        }
        public static bool Check_File(XmlNode node, string attrb, bool required = true, short nTabs=1)
        {
            if (node.Attributes.GetNamedItem(attrb) == null)
            {
                if (required)
                {
                    log2.Log($"{MainClass.GetTabs(nTabs)} '{node.Name}'  No está definido el atributo '{attrb}' -> {GetNodePath(node)}", 2);
                    return false;
                }
                else
                    return true;
            }
            string value = node.Attributes.GetNamedItem(attrb).Value;
            if (value != "")
            {

                string fileRequired = $"{ Path.GetFullPath(cPath) }/{ value}";

                if (!File.Exists(fileRequired))
                {
                    log2.Log($"{MainClass.GetTabs(nTabs)} '{attrb}' No existe el fichero '{value}' '{fileRequired}' '{Path.GetFullPath(cPath)}/{value}'  -> {GetNodePath(node)}", 2);
                    return false;
                }
                else
                    log2.Log($"{MainClass.GetTabs(nTabs)} {attrb} = {value} ");
                if (MainClass.ficherosProyecto.Contains(value))
                {
                    log2.Log($"{MainClass.GetTabs(nTabs)} '{attrb}' No existe en el Proyecto el fichero {value}  {fileRequired} -> {GetNodePath(node)}", 2);
                };
            }

            else
            {
                if (required)
                {
                    log2.Log($"{MainClass.GetTabs(nTabs)} No se a definido un fichero en el atributo {attrb} -> {GetNodePath(node)}", 2);
                    return false;
                }
                else
                    log2.Log($"{MainClass.GetTabs(nTabs)} {attrb} Fichero No Necesario ");
            }
            return true;

        }
        public static void checkExistence(short val2Check = 0, short minimumValue = 0, short maxValue = 1, string nameTag = "")
        {
            if (val2Check < minimumValue) { MainClass.log2.Log($"{MainClass.GetTabs(3)} Falta el Tag {nameTag} ", 2); }
            if (val2Check > maxValue) { MainClass.log2.Log($"{MainClass.GetTabs(3)} Demasiados Tags {nameTag} ", 2); }

        }
        #endregion Útiles
        #region Por hacer
        public void LoadApps()
        {
            //TODO GlobalConfig.LoadApps();
        }
        #endregion Por hacer
        #region BuscarFicheros
        internal static void SearchDirectory(string rootdir = @"/home/janeko",  List<string> extension_list = null)
        {

            DirectoryInfo dir_info = new DirectoryInfo(rootdir);
            if (dir_info.Name.StartsWith("998") || (dir_info.Name == "data"))
            {

                Console.SetCursorPosition(nCol,nLin);
                if (nCol == 0)
                    Console.Write("{0,3:D3} ", nLin - 1);
                else
                    Console.SetCursorPosition(nCol+4, nLin);
                Console.Write("d", nLin - 1);
                nCol++;
                if (nCol == 50)
                {
                    nCol = 0;
                    nLin++;
                }


                //Console.WriteLine(rootdir + new String(' ', 200));
                cPath = rootdir;
                if (dir_info.Name != "data")
                    cPath = rootdir + "/data";
                if (Directory.Exists(cPath))
                {
                    DirectoryInfo dir_data = new DirectoryInfo(cPath);

                    bool projectExist = false;
                    foreach (FileInfo file_info in dir_data.GetFiles())
                    {

                        //if (file_info.Extension == ".xml" || file_info.Extension == ".yaml" || file_info.Extension == ".padl" || file_info.Extension == ".pcond" || file_info.Extension == ".pe2p" || file_info.Extension == ".plightc")
                        //Console.WriteLine($"Con datos {file_info.FullName}");
                        if (file_info.Extension == ".pprj")
                        {
                            projectExist = true;
                            AnalizeProject(file_info);

                        }
                    }
                    if(!projectExist)
                    {
                        //Console.WriteLine($"Directorio data sin proyecto {cPath}");
                        log2.Log($"Directorio data sin proyecto {cPath}", 2);
                    }
                }
                else
                {
                    //Console.WriteLine($"Sin directorio data {dir_info.FullName}");
                    log2.Log($"Sin directorio data {dir_info.FullName}",1);
                }

            }
            try
            {
                foreach (DirectoryInfo subdir_info in dir_info.GetDirectories())
                {
                    SearchDirectory(subdir_info.FullName, extension_list);
                }
            }
            catch
            {
            }

        }
        #endregion BuscarFicheros
        #region Analizar Proyecto
        public static void AnalizeProject(FileInfo fFile)
        {


            string cFilePath = fFile.FullName;
            XmlDocument xDoc = new XmlDocument();
            xDoc.XmlResolver = null;

            try
            {
                xDoc.Load(cFilePath);
            }
            catch (Exception ex)
            {
                log2.Log($"Al abrir {cFilePath} {ex.Message}", 2);
                return;
            }
            XmlNode rootPlastic = xDoc.FirstChild;
            string cAppAttrb = rootPlastic.Attributes.GetNamedItem("application").InnerText;
            log2.Log($"Analizando Proyecto '{rootPlastic.Attributes.GetNamedItem("name").InnerText}' en el fichero '{fFile.Name}' sito en '{fFile.DirectoryName}'",4);
            if (cAppAttrb != "project")
            {
                log2.Log($" Proyecto sin atributo 'project' en 'plastic.appplication'",1);
            }
             
            if (rootPlastic.HasChildNodes)
            {
                string fileFullName, fileName;
                for (Int16 i = 0; i < rootPlastic.ChildNodes.Count - 1; i++)
                {
                    if (rootPlastic.ChildNodes[i].LocalName == "file")
                    {
                        fileName = rootPlastic.ChildNodes[i].Attributes.GetNamedItem("name").InnerText;
                        ficherosProyecto.Add(fileName);

                        fileFullName = fFile.DirectoryName + "/" + fileName;
                        if (!File.Exists(fileFullName))
                        {
                            fileFullName = fileFullName.Replace(rootdir, "");
                            log2.Log($"{MainClass.GetTabs(1)}Falta el fichero '{fileFullName}' descrito en el proyecto ", 2);
                            continue;
                        }
                        else
                        {
                            string cExtension = Path.GetExtension(fileName);

                            if (extensiones.Contains(cExtension))
                            {
                                checkProjectFile(fileFullName);
                            }
                            else { 
                                log2.Log($"{MainClass.GetTabs(1)}Fichero del proyecto localizado '{fileName}'");
                                Image image1;
                                switch (cExtension)
                                {
                                    case ".png":
                                        /*
                                        try
                                        {
                                            image1 = Image.FromFile(fileFullName);
                                            if (!image1.RawFormat.Equals(ImageFormat.Png))
                                                log2.Log($"{MainClass.GetTabs(2)}El fichero '{fileName}' no es del tipo png");
                                            image1.Dispose();
                                        }
                                        catch (Exception ex)
                                        {
                                            log2.Log($"{MainClass.GetTabs(2)}El fichero '{fileName}' excepción {ex.Message}");
                                        }
                                        */
                                        break;
                                    case ".bmp":
                                        image1 = Image.FromFile(fileFullName);
                                        if (!image1.RawFormat.Equals(ImageFormat.Bmp))
                                            log2.Log($"{MainClass.GetTabs(2)}El fichero '{fileName}' no es del tipo bmp");
                                        image1.Dispose();
                                        break;
                                    case ".gif":
                                        image1 = Image.FromFile(fileFullName);
                                        if (!image1.RawFormat.Equals(ImageFormat.Gif))
                                            log2.Log($"{MainClass.GetTabs(2)}El fichero '{fileName}' no es del tipo gif");
                                        image1.Dispose();
                                        break;
                                    case ".jpg":
                                    case ".jpeg":
                                        image1 = Image.FromFile(fileFullName);
                                        if (!image1.RawFormat.Equals(ImageFormat.Jpeg))
                                            log2.Log($"{MainClass.GetTabs(2)}El fichero '{fileName}' no es del tipo Jpeg");
                                        image1.Dispose();
                                        break;
                                    default:
                                        log2.Log($"{MainClass.GetTabs(2)}El fichero '{fileName}' no es conocido");
                                        break;
                                }

                            }
                        }
                    }
                    else
                        log2.Log($"{MainClass.GetTabs(1)}Tag {rootPlastic.ChildNodes[i].LocalName} no correspone a fichero de proyecto ",1);



                }
            } else log2.Log($"Sin files definidos en {cFilePath}", 2);
        }
        #endregion Analizar Proyecto
        public static void checkProjectFile(string fileFullName) {
            log2.Log($"{MainClass.GetTabs(2)}Analizando '{Path.GetFileName(fileFullName)}' en '{Path.GetFullPath(fileFullName)}' ",4);
            XmlDocument xDoc = new XmlDocument();
            xDoc.XmlResolver = null;
            try
            {
                xDoc.Load(fileFullName);
            }
            catch (Exception ex)
            {
                log2.Log($"{MainClass.GetTabs(2)}{ex.Message} al abrir '{Path.GetFileName(fileFullName)}' en '{Path.GetFullPath(fileFullName)}' ", 2);
                return;
            }

            //hace lo mismo que antes
            XmlNode rootPlastic = xDoc.FirstChild;
            if (rootPlastic.Name != "plastic")
            {
                log2.Log($"{MainClass.GetTabs(2)} No es fichero plastic '{Path.GetFileName(fileFullName)}' en '{Path.GetFullPath(fileFullName)}' ", 2);
                return;
            }
            string typeApplicationPlastic = rootPlastic.Attributes.GetNamedItem("application").InnerText;

            //cPath = Path.GetFullPath(fileFullName);
            switch (Path.GetExtension(fileFullName))
            {
                case ".pctrl"://modbus2plastic
                    if (typeApplicationPlastic == "controller")
                    {
                        log2.Log($"{MainClass.GetTabs(3)} Comprobando Plastic Controller");
                    }
                    else
                        log2.Log($"{MainClass.GetTabs(3)} Fichero Plastic Modbus sin atributo 'controller' '{Path.GetFileName(fileFullName)}' en '{Path.GetFullPath(fileFullName)}' ", 2);
                    ChkCTRL.check(rootPlastic);
                    break;
                case ".scag"://scadaglobal
                    if (typeApplicationPlastic == "scadaglobal")
                    {
                        log2.Log($"{MainClass.GetTabs(3)} Comprobando scada global");
                    }
                    else
                        log2.Log($"{MainClass.GetTabs(3)} Fichero scada global sin atributo 'scadaglobal' '{Path.GetFileName(fileFullName)}' en '{Path.GetFullPath(fileFullName)}' ", 2);
                    ChkSCAG.check(rootPlastic);
                    break;
                case ".mo2p"://modbus2plastic
                    if (typeApplicationPlastic == "modbus2plastic")
                    {
                        log2.Log($"{MainClass.GetTabs(3)} Comprobando Plastic Modbus");
                    }
                    else
                        log2.Log($"{MainClass.GetTabs(3)} Fichero Plastic Modbus sin atributo 'modbus2plastic' '{Path.GetFileName(fileFullName)}' en '{Path.GetFullPath(fileFullName)}' ", 2);
                    ChkMOD2P.check(rootPlastic);
                    break;
                case ".padl"://addrlist
                    if(typeApplicationPlastic == "addrlist")
                    {
                        log2.Log($"{MainClass.GetTabs(3)} Comprobando direcciones ");
                    }
                    else
                        log2.Log($"{MainClass.GetTabs(3)} Fichero direcciones sin atributo 'addrlist' '{Path.GetFileName(fileFullName)}' en '{Path.GetFullPath(fileFullName)}' ", 2);
                    ChkPADL.check(rootPlastic);
                    break;

                case ".scav"://scadaview
                    if (typeApplicationPlastic == "scadaview")
                    {
                        log2.Log($"{MainClass.GetTabs(3)} Comprobando vista scada ");
                    }
                    else
                        log2.Log($"{MainClass.GetTabs(3)} Fichero vista scada sin atributo 'scadaview' '{Path.GetFileName(fileFullName)}' en '{Path.GetFullPath(fileFullName)}' ", 2);
                    ChkSCAV.check(rootPlastic);
                    break;

                case ".pgman"://groupmanager
                    if (typeApplicationPlastic == "groupmanager")
                    {
                        log2.Log($"{MainClass.GetTabs(3)} Comprobando Group Manager");
                    }
                    else
                        log2.Log($"{MainClass.GetTabs(3)} Fichero Group Manager sin atributo 'groupmanager' '{Path.GetFileName(fileFullName)}' en '{Path.GetFullPath(fileFullName)}' ", 2);
                    //ChkPGMAN.check(rootPlastic);
                    break;
                case ".pe2p"://modbus2plastic
                    if (typeApplicationPlastic == "eib2plastic")
                    {
                        log2.Log($"{MainClass.GetTabs(3)} Comprobando Plastic EIB");
                    }
                    else
                        log2.Log($"{MainClass.GetTabs(3)} Fichero Plastic EIB sin atributo 'eib2plastic' '{Path.GetFileName(fileFullName)}' en '{Path.GetFullPath(fileFullName)}' ", 2);
                    //ChkEIB2P.check(rootPlastic);
                    break;

                case ".pcond"://modbus2plastic
                    if (typeApplicationPlastic == "conditional")
                    {
                        log2.Log($"{MainClass.GetTabs(3)} Comprobando Plastic Condicional");
                    }
                    else
                        log2.Log($"{MainClass.GetTabs(3)} Fichero Plastic Condicional sin atributo 'conditional' '{Path.GetFileName(fileFullName)}' en '{Path.GetFullPath(fileFullName)}' ", 2);
                    //ChkCOND.check(rootPlastic);
                    break;
                case ".xml"://xml
                    switch(typeApplicationPlastic)
                    {
                        case "s7":
                            log2.Log($"{MainClass.GetTabs(3)} Comprobando Plastic S7");
                            ChkXML_S7.check(rootPlastic);
                            break;
                        case "plasticity2":
                            log2.Log($"{MainClass.GetTabs(3)} Comprobando Plasticity 2 (plasticity2)");
                            //ChkXML_Plasticity2.check(rootPlastic);
                            break;
                        case "onvif":
                            log2.Log($"{MainClass.GetTabs(3)} Comprobando Plastic Eventos (onvif)");
                            //ChkXML_ONVIF.check(rootPlastic);
                            break;
                        case "Fronius2Plastic":
                            log2.Log($"{MainClass.GetTabs(3)} Comprobando Fronius to Plastic (Fronius2Plastic)");
                            //ChkXML_Fronius2Plastic.check(rootPlastic);
                            break;
                        case "fins2plastic":
                            log2.Log($"{MainClass.GetTabs(3)} Comprobando Fins to Plastic (fins2plastic)");
                            //ChkXML_Fins2Plastic.check(rootPlastic);
                            break;
                        case "accum":
                            log2.Log($"{MainClass.GetTabs(3)} Comprobando Direcciones acumuladores (accum)");
                            //ChkXML_ACCUM.check(rootPlastic);
                            break;
                        case "lightcontrol_saved_state":
                            log2.Log($"{MainClass.GetTabs(3)} Comprobando Estado persistido de control de luces (lightcontrol_saved_state)");
                            //ChkXML_LIGHTSE.check(rootPlastic);
                            break;

                        default:
                            log2.Log($"{MainClass.GetTabs(3)} Fichero XML sin atributo application '{typeApplicationPlastic}' reconocible '{Path.GetFileName(fileFullName)}' en '{Path.GetFullPath(fileFullName)}' ", 2);
                            break;
                    }

                    break;
                default:
                    log2.Log($"{MainClass.GetTabs(3)}Nothing to do. I don't know what to do with Application {typeApplicationPlastic}! =>> '{Path.GetFileName(fileFullName)}' en '{Path.GetFullPath(fileFullName)}' ", 2);
                    break;
            }


        }

    }//MainClass
}
