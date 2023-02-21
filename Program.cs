using System;
using System.IO;
using System.Collections.Generic;
using System.Xml;
using System.Drawing;
using System.Drawing.Imaging;

namespace RecorreDir
{
    class MainClass
    {
        public static string rootdir { get; set; }

        static Save2Log log2;
        static List<string> extensiones = new List<string>();
        public static void Main(string[] args)
        {
            rootdir = Directory.GetCurrentDirectory();
            if (args.Length == 0)
            {
                rootdir = "/media/janeko/Almacen/celofan/copiacelofan/inst_files";
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
            extensiones.Add(".pcond");
            extensiones.Add(".pe2p");
            extensiones.Add(".pligthc");
            extensiones.Add(".mod2p");
            extensiones.Add(".scav");
            extensiones.Add(".scag");
            log2 = new Save2Log(rootdir + "/BuscarDirectorios.log");

            //wLog = File.CreateText(rootdir + "/BuscarDirectorios.log");
            Console.Write("Inicio proceso\n");
            SearchDirectory(rootdir, extension_list: extensiones);
        
            log2.CloseLog();
            Console.Write("\n");
            Console.WriteLine("Finalizado proceso.");
        }
        internal static void SearchDirectory(string rootdir = @"/home/janeko",  List<string> extension_list = null)
        {

            DirectoryInfo dir_info = new DirectoryInfo(rootdir);
            if (dir_info.Name.StartsWith("998"))
            {
                Console.Write("d");
                string cPath = dir_info.FullName + "/data";
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
            log2.Log($"Analizando Proyecto '{rootPlastic.Attributes.GetNamedItem("name").InnerText}' en el fichero '{fFile.Name}' sito en '{fFile.DirectoryName}'");
            if (cAppAttrb != "project")
            {
                log2.Log($" Proyecto sin atributo 'project' en 'plastic.appplication'",1);
            }
             
            if (rootPlastic.HasChildNodes)
            {
                string fileFullName, fileName;
                for (int i = 0; i < rootPlastic.ChildNodes.Count - 1; i++)
                {
                    if (rootPlastic.ChildNodes[i].LocalName == "file")
                    {
                        fileName = rootPlastic.ChildNodes[i].Attributes.GetNamedItem("name").InnerText;
                        fileFullName = fFile.DirectoryName + "/" + fileName;
                        if (!File.Exists(fileFullName))
                        {
                            fileFullName = fileFullName.Replace(rootdir, "");
                            log2.Log($"\tFalta el fichero '{fileFullName}' descrito en el proyecto ", 2);
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
                                log2.Log($"\tFichero del proyecto localizado '{fileName}'");
                                Image image1;
                                switch (cExtension)
                                {
                                    case ".png":
                                        /*
                                        try
                                        {
                                            image1 = Image.FromFile(fileFullName);
                                            if (!image1.RawFormat.Equals(ImageFormat.Png))
                                                log2.Log($"\t\tEl fichero '{fileName}' no es del tipo png");
                                            image1.Dispose();
                                        }
                                        catch (Exception ex)
                                        {
                                            log2.Log($"\t\tEl fichero '{fileName}' excepción {ex.Message}");
                                        }
                                        */
                                        break;
                                    case ".bmp":
                                        image1 = Image.FromFile(fileFullName);
                                        if (!image1.RawFormat.Equals(ImageFormat.Bmp))
                                            log2.Log($"\t\tEl fichero '{fileName}' no es del tipo bmp");
                                        image1.Dispose();
                                        break;
                                    case ".gif":
                                        image1 = Image.FromFile(fileFullName);
                                        if (!image1.RawFormat.Equals(ImageFormat.Gif))
                                            log2.Log($"\t\tEl fichero '{fileName}' no es del tipo gif");
                                        image1.Dispose();
                                        break;
                                    case ".jpg":
                                    case ".jpeg":
                                        image1 = Image.FromFile(fileFullName);
                                        if (!image1.RawFormat.Equals(ImageFormat.Jpeg))
                                            log2.Log($"\t\tEl fichero '{fileName}' no es del tipo Jpeg");
                                        image1.Dispose();
                                        break;
                                    default:
                                        log2.Log($"\t\tEl fichero '{fileName}' no es conocido");
                                        break;
                                }

                            }
                        }
                    }
                    else
                        log2.Log("\tTag {rootPlastic.ChildNodes[i].LocalName} no correspone a fichero de proyecto ",1);



                }
            } else log2.Log($"Sin files definidos en {cFilePath}", 2);
        }
        public static void checkProjectFile(string fileFullName) {
            log2.Log($"\t\tAnalizando '{Path.GetFileName(fileFullName)}' en '{Path.GetFullPath(fileFullName)}' ");
            XmlDocument xDoc = new XmlDocument();
            xDoc.XmlResolver = null;
            try
            {
                xDoc.Load(fileFullName);
            }
            catch (Exception ex)
            {
                log2.Log($"\t\t{ex.Message} al abrir '{Path.GetFileName(fileFullName)}' en '{Path.GetFullPath(fileFullName)}' ", 2);
                return;
            }

            //hace lo mismo que antes
            XmlNode rootPlastic = xDoc.FirstChild;
            if (rootPlastic.Name != "plastic")
            {
                log2.Log($"\t\t No es fichero plastic '{Path.GetFileName(fileFullName)}' en '{Path.GetFullPath(fileFullName)}' ", 2);
                return;
            }
            string typeApplicationPlastic = rootPlastic.Attributes.GetNamedItem("application").InnerText;


            switch (Path.GetExtension(fileFullName))
            {

                case ".padl"://addrlist
                    if(typeApplicationPlastic == "addrlist")
                    {
                        log2.Log($"\t\t\t Comprobando direcciones ");
                    }
                    else
                        log2.Log($"\t\t\t Fichero direcciones sin atributo 'addrlist' '{Path.GetFileName(fileFullName)}' en '{Path.GetFullPath(fileFullName)}' ", 2);
                    //checkPADL();
                    break;
                case ".pctrl"://modbus2plastic
                    if (typeApplicationPlastic == "controller")
                    {
                        log2.Log($"\t\t\t Comprobando Plastic Controller");
                    }
                    else
                        log2.Log($"\t\t\t Fichero Plastic Modbus sin atributo 'controller' '{Path.GetFileName(fileFullName)}' en '{Path.GetFullPath(fileFullName)}' ", 2);
                    ChkCTRL.check(rootPlastic, log2, Path.GetFullPath(fileFullName));
                    break;
                case ".scav"://scadaview
                    if (typeApplicationPlastic == "scadaview")
                    {
                        log2.Log($"\t\t\t Comprobando vista scada ");
                    }
                    else
                        log2.Log($"\t\t\t Fichero vista scada sin atributo 'scadaview' '{Path.GetFileName(fileFullName)}' en '{Path.GetFullPath(fileFullName)}' ", 2);
                    //checkSCAV();
                    break;
                case ".scag"://scadaglobal
                    if (typeApplicationPlastic == "scadaglobal")
                    {
                        log2.Log($"\t\t\t Comprobando scada global");
                    }
                    else
                        log2.Log($"\t\t\t Fichero scada global sin atributo 'scadaview' '{Path.GetFileName(fileFullName)}' en '{Path.GetFullPath(fileFullName)}' ", 2);
                    //checkSCAG();
                    break;
                case ".pgman"://groupmanager
                    if (typeApplicationPlastic == "groupmanager")
                    {
                        log2.Log($"\t\t\t Comprobando Group Manager");
                    }
                    else
                        log2.Log($"\t\t\t Fichero Group Manager sin atributo 'scadaview' '{Path.GetFileName(fileFullName)}' en '{Path.GetFullPath(fileFullName)}' ", 2);
                    //checkPGMAN();
                    break;
                case ".pe2p"://modbus2plastic
                    if (typeApplicationPlastic == "eib2plastic")
                    {
                        log2.Log($"\t\t\t Comprobando Plastic EIB");
                    }
                    else
                        log2.Log($"\t\t\t Fichero Plastic EIB sin atributo 'eib2plastic' '{Path.GetFileName(fileFullName)}' en '{Path.GetFullPath(fileFullName)}' ", 2);
                    //checkEIB2P();
                    break;
                case ".mo2p"://modbus2plastic
                    if (typeApplicationPlastic == "modbus2plastic")
                    {
                        log2.Log($"\t\t\t Comprobando Plastic Modbus");
                    }
                    else
                        log2.Log($"\t\t\t Fichero Plastic Modbus sin atributo 'scadaview' '{Path.GetFileName(fileFullName)}' en '{Path.GetFullPath(fileFullName)}' ", 2);
                    //checkMO2P();
                    break;
                case ".pcond"://modbus2plastic
                    if (typeApplicationPlastic == "conditional")
                    {
                        log2.Log($"\t\t\t Comprobando Plastic Condicional");
                    }
                    else
                        log2.Log($"\t\t\t Fichero Plastic Condicional sin atributo 'conditional' '{Path.GetFileName(fileFullName)}' en '{Path.GetFullPath(fileFullName)}' ", 2);
                    //checkCOND();
                    break;
                case ".xml"://xml
                    switch(typeApplicationPlastic)
                    {
                        case "s7":
                            log2.Log($"\t\t\t Comprobando Plastic S7");
                            //checkXML_();
                            break;
                        case "plasticity2":
                            log2.Log($"\t\t\t Comprobando Plasticity 2 (plasticity2)");
                            //checkXML_();
                            break;
                        case "onvif":
                            log2.Log($"\t\t\t Comprobando Plastic Eventos (onvif)");
                            //checkXML_();
                            break;
                        case "Fronius2Plastic":
                            log2.Log($"\t\t\t Comprobando Fronius to Plastic (Fronius2Plastic)");
                            //checkXML_Fronius2Plastic();
                            break;
                        case "fins2plastic":
                            log2.Log($"\t\t\t Comprobando Fins to Plastic (fins2plastic)");
                            //checkXML_Fins2Plastic();
                            break;
                        case "accum":
                            log2.Log($"\t\t\t Comprobando Direcciones acumuladores (accum)");
                            //checkXML_ACCUM();
                            break;
                        case "lightcontrol_saved_state":
                            log2.Log($"\t\t\t Comprobando Estado persistido de control de luces (lightcontrol_saved_state)");
                            //checkXML_LIGHTSE();
                            break;

                        default:
                            log2.Log($"\t\t\t Fichero XML sin atributo application '{typeApplicationPlastic}' reconocible '{Path.GetFileName(fileFullName)}' en '{Path.GetFullPath(fileFullName)}' ", 2);
                            break;
                    }

                    break;
                default:
                    log2.Log($"\t\t\tNothing to do. I don't know what to do with Application {typeApplicationPlastic}! =>> '{Path.GetFileName(fileFullName)}' en '{Path.GetFullPath(fileFullName)}' ", 2);
                    break;
            }


        }

    }//MainClass
}
