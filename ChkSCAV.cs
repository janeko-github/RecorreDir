using System.Xml;
using System.Collections.Generic;
namespace RecorreDir
{
    public static class ChkSCAV
    {
        internal static short numtabs;
        internal static List<string> viewNames = new List<string>();
        public static void check(XmlNode rootPlastic)
        {
            numtabs = 5;
            short chkView = 0;
            string nameOfView = "";

            if (rootPlastic.HasChildNodes)
            {
                foreach (XmlNode viewNode in rootPlastic.ChildNodes)
                {
                    switch (viewNode.Name)
                    {
                        case "view":
                            nameOfView = viewNode.Attributes.GetNamedItem("name").Value;
                            Check_View(viewNode);
                            if (viewNames.Contains(nameOfView))
                            {
                                MainClass.log2.Log($"{MainClass.GetTabs(numtabs)} Tag {viewNode.Name} View {nameOfView} repetido ", 2);
                            }
                            else
                                viewNames.Add(nameOfView);
                            chkView++;
                            break;
                        default:
                            //No debería estar en ese fichero
                            MainClass.log2.Log($"{MainClass.GetTabs(numtabs)} Tag {viewNode.Name} desconocida ", 1);
                            break;
                    }
                }
                //comprobación de existencia de mínimos
                MainClass.checkExistence(val2Check: chkView, minimumValue: 1, maxValue: 200, nameTag: "folder");


            }
            else
                MainClass.log2.Log($"{MainClass.GetTabs(numtabs)} No hay nodos hijos ", 2);
        }

        //TODO que no se repitan ni los id ni las address
        internal static void Check_View(XmlNode node)
        {
            short nodeCount = 0;
            MainClass.log2.Log($"{MainClass.GetTabs(numtabs)} Tag {node.Name} ");
            numtabs++;
            MainClass.Check_Attribute(node: node, attrb: "width", required: true, nTabs: numtabs, defaultValue: "*");
            MainClass.Check_Attribute(node: node, attrb: "height", required: true, nTabs: numtabs, defaultValue: "*");
            MainClass.Check_Attribute(node: node, attrb: "name", required: true, nTabs: numtabs, defaultValue: "*");
            MainClass.Check_Attribute(node: node, attrb: "description", required: true, nTabs: numtabs, defaultValue: "*");
            MainClass.Check_Attribute(node: node, attrb: "show_home", required: true, nTabs: numtabs, defaultValue: "0");
            MainClass.Check_File(node: node, attrb: "background_image", required: true, nTabs: numtabs);
            if (node.HasChildNodes)
            {
                foreach (XmlNode innerNode in node.ChildNodes)
                {
                    nodeCount++;
                    //TODO En el fichero file debe de existir un folder con name = al atributo path source
                    switch(innerNode.Name)
                    {
                        case "label":
                            /*
                             *                    
<label layer="0" x="300" y="30" width="468" height="30" name="Object6" hint="" group="" read_address="" write_address="" read_symbolic="" write_symbolic="" text="ESTADIO MONTE ROMERO" value_translation="" 
font_name="Arial" font_size="24" font_style="BI" color="FF000000" mostrar_marco="0" padding_marco="0" color_borde="0" color_fondo="0" factor="" decimal_places="" value_transformation="" />
                             * */
                            MainClass.Check_Attribute(node: innerNode, attrb: "layer", required: true, nTabs: numtabs, defaultValue: "*");
                            MainClass.Check_Attribute(node: innerNode, attrb: "x", required: true, nTabs: numtabs, defaultValue: "0");
                            MainClass.Check_Attribute(node: innerNode, attrb: "y", required: true, nTabs: numtabs, defaultValue: "30");
                            MainClass.Check_Attribute(node: innerNode, attrb: "width", required: true, nTabs: numtabs, defaultValue: "468");
                            MainClass.Check_Attribute(node: innerNode, attrb: "height", required: true, nTabs: numtabs, defaultValue: "30");
                            MainClass.Check_Attribute(node: innerNode, attrb: "name", required: true, nTabs: numtabs, defaultValue: "*");
                            MainClass.Check_Attribute(node: innerNode, attrb: "hint", required: true, nTabs: numtabs );
                            MainClass.Check_Attribute(node: innerNode, attrb: "group", required: true, nTabs: numtabs);
                            MainClass.Check_Attribute(node: innerNode, attrb: "read_address", required: true, nTabs: numtabs);
                            MainClass.Check_Attribute(node: innerNode, attrb: "write_address", required: true, nTabs: numtabs);
                            MainClass.Check_Attribute(node: innerNode, attrb: "read_symbolic", required: true, nTabs: numtabs);
                            MainClass.Check_Attribute(node: innerNode, attrb: "write_symbolic", required: true, nTabs: numtabs);
                            MainClass.Check_Attribute(node: innerNode, attrb: "text", required: true, nTabs: numtabs, defaultValue: "");
                            MainClass.Check_Attribute(node: innerNode, attrb: "value_translation", required: true, nTabs: numtabs, defaultValue: "");
                            MainClass.Check_Attribute(node: innerNode, attrb: "font_name", required: true, nTabs: numtabs, defaultValue: "Arial");
                            MainClass.Check_Attribute(node: innerNode, attrb: "font_size", required: true, nTabs: numtabs, defaultValue: "24");
                            MainClass.Check_Attribute(node: innerNode, attrb: "font_style", required: true, nTabs: numtabs, defaultValue: "BI");
                            MainClass.Check_Attribute(node: innerNode, attrb: "color", required: true, nTabs: numtabs, defaultValue: "FF000000");
                            MainClass.Check_Attribute(node: innerNode, attrb: "mostrar_marco", required: true, nTabs: numtabs, defaultValue: "0");
                            MainClass.Check_Attribute(node: innerNode, attrb: "padding_marco", required: true, nTabs: numtabs, defaultValue: "0");
                            MainClass.Check_Attribute(node: innerNode, attrb: "color_borde", required: true, nTabs: numtabs, defaultValue: "0");
                            MainClass.Check_Attribute(node: innerNode, attrb: "color_fondo", required: true, nTabs: numtabs, defaultValue: "0");
                            MainClass.Check_Attribute(node: innerNode, attrb: "factor", required: true, nTabs: numtabs);
                            MainClass.Check_Attribute(node: innerNode, attrb: "decimal_places", required: true, nTabs: numtabs);
                            MainClass.Check_Attribute(node: innerNode, attrb: "value_transformation", required: true, nTabs: numtabs);
                            break;
                        case "image":
                            /*
                             *                    
<image image="Volver.png" value_translation="" url="" view="telemetria.scav" view_popup="0" view_params="" graph="" write_value="" alarm_address="" alarm_translation="" popup_empotrado="0"
popup_empotrado_x="0" popup_empotrado_y="0" script_action="" script_arg="" set_manual="0" layer="0" x="20" y="610" width="70" height="30" name="Object49" hint="" group="" read_address="" write_address="" 
read_symbolic="" write_symbolic="" />
                            
                             * */
                            MainClass.Check_File(node: innerNode, attrb: "image", required: true, nTabs: numtabs);
                            MainClass.Check_Attribute(node: innerNode, attrb: "value_translation", required: true, nTabs: numtabs);
                            MainClass.Check_Attribute(node: innerNode, attrb: "url", required: true, nTabs: numtabs);
                            MainClass.Check_File(node: innerNode, attrb: "view", required: true, nTabs: numtabs);
                            MainClass.Check_Attribute(node: innerNode, attrb: "view_popup", required: true, nTabs: numtabs, defaultValue: "0");
                            MainClass.Check_Attribute(node: innerNode, attrb: "view_params", required: true, nTabs: numtabs);
                            MainClass.Check_Attribute(node: innerNode, attrb: "graph", required: true, nTabs: numtabs);
                            MainClass.Check_Attribute(node: innerNode, attrb: "write_value", required: true, nTabs: numtabs);
                            MainClass.Check_Attribute(node: innerNode, attrb: "alarm_address", required: true, nTabs: numtabs);
                            MainClass.Check_Attribute(node: innerNode, attrb: "alarm_translation", required: true, nTabs: numtabs);
                            MainClass.Check_Attribute(node: innerNode, attrb: "popup_empotrado", required: true, nTabs: numtabs);
                            MainClass.Check_Attribute(node: innerNode, attrb: "popup_empotrado_x", required: true, nTabs: numtabs, defaultValue: "0");
                            MainClass.Check_Attribute(node: innerNode, attrb: "popup_empotrado_y", required: true, nTabs: numtabs, defaultValue: "0");
                            MainClass.Check_Attribute(node: innerNode, attrb: "script_action", required: true, nTabs: numtabs);
                            MainClass.Check_Attribute(node: innerNode, attrb: "script_arg", required: true, nTabs: numtabs);
                            MainClass.Check_Attribute(node: innerNode, attrb: "set_manual", required: true, nTabs: numtabs, defaultValue: "0");
                            MainClass.Check_Attribute(node: innerNode, attrb: "layer", required: true, nTabs: numtabs, defaultValue: "0");
                            MainClass.Check_Attribute(node: innerNode, attrb: "x", required: true, nTabs: numtabs, defaultValue: "20");
                            MainClass.Check_Attribute(node: innerNode, attrb: "y", required: true, nTabs: numtabs, defaultValue: "610");
                            MainClass.Check_Attribute(node: innerNode, attrb: "width", required: false, nTabs: numtabs, defaultValue: "70");
                            MainClass.Check_Attribute(node: innerNode, attrb: "height", required: true, nTabs: numtabs, defaultValue: "30");
                            MainClass.Check_Attribute(node: innerNode, attrb: "name", required: true, nTabs: numtabs, defaultValue: "*");
                            MainClass.Check_Attribute(node: innerNode, attrb: "hint", required: true, nTabs: numtabs);
                            MainClass.Check_Attribute(node: innerNode, attrb: "group", required: true, nTabs: numtabs);
                            MainClass.Check_Attribute(node: innerNode, attrb: "read_address", required: true, nTabs: numtabs);
                            MainClass.Check_Attribute(node: innerNode, attrb: "write_address", required: true, nTabs: numtabs);
                            MainClass.Check_Attribute(node: innerNode, attrb: "read_symbolic", required: true, nTabs: numtabs);
                            MainClass.Check_Attribute(node: innerNode, attrb: "write_symbolic", required: true, nTabs: numtabs);
                            break;
                        case "textinput":
                            /*
<textinput layer="2" x="320" y="170" width="100" height="30" name="Object5" hint="" group="" read_address="/direcciones/#101" write_address="" read_symbolic="" write_symbolic="" text="" text_left="" 
text_right="V" font_name="Arial" font_size="12" font_style="B" color="FF000000" factor="" decimal_places="1" />
                             * */
                            MainClass.Check_Attribute(node: innerNode, attrb: "layer", required: true, nTabs: numtabs, defaultValue: "2");
                            MainClass.Check_Attribute(node: innerNode, attrb: "x", required: true, nTabs: numtabs, defaultValue: "10");
                            MainClass.Check_Attribute(node: innerNode, attrb: "y", required: true, nTabs: numtabs, defaultValue: "10");
                            MainClass.Check_Attribute(node: innerNode, attrb: "width", required: false, nTabs: numtabs, defaultValue: "100");
                            MainClass.Check_Attribute(node: innerNode, attrb: "height", required: true, nTabs: numtabs, defaultValue: "30");
                            MainClass.Check_Attribute(node: innerNode, attrb: "name", required: true, nTabs: numtabs, defaultValue: "*");
                            MainClass.Check_Attribute(node: innerNode, attrb: "hint", required: true, nTabs: numtabs);
                            MainClass.Check_Attribute(node: innerNode, attrb: "group", required: true, nTabs: numtabs);
                            MainClass.Check_Attribute(node: innerNode, attrb: "read_address", required: true, nTabs: numtabs, defaultValue: "*");
                            MainClass.Check_Attribute(node: innerNode, attrb: "read_symbolic", required: true, nTabs: numtabs);
                            MainClass.Check_Attribute(node: innerNode, attrb: "write_symbolic", required: true, nTabs: numtabs);
                            MainClass.Check_Attribute(node: innerNode, attrb: "text", required: true, nTabs: numtabs);
                            MainClass.Check_Attribute(node: innerNode, attrb: "text_left", required: true, nTabs: numtabs);
                            MainClass.Check_Attribute(node: innerNode, attrb: "text_right", required: true, nTabs: numtabs);
                            MainClass.Check_Attribute(node: innerNode, attrb: "font_name", required: true, nTabs: numtabs, defaultValue: "Arial");
                            MainClass.Check_Attribute(node: innerNode, attrb: "font_size", required: true, nTabs: numtabs, defaultValue: "24");
                            MainClass.Check_Attribute(node: innerNode, attrb: "font_style", required: true, nTabs: numtabs, defaultValue: "BI");
                            MainClass.Check_Attribute(node: innerNode, attrb: "color", required: true, nTabs: numtabs, defaultValue: "FF000000");
                            MainClass.Check_Attribute(node: innerNode, attrb: "factor", required: true, nTabs: numtabs);
                            MainClass.Check_Attribute(node: innerNode, attrb: "decimal_places", required: true, nTabs: numtabs);

                            break;
                        default:
                            MainClass.log2.Log($"{MainClass.GetTabs(numtabs)} Tag {node.Name} Existe un tag denominado {innerNode.Name} que no pertenece a un scadaview.", 1);
                            break;
                    }
 

                }
                MainClass.checkExistence(val2Check: nodeCount, minimumValue: 1, maxValue: 300, nameTag: "address");

            }
            else
            {
                MainClass.log2.Log($"{MainClass.GetTabs(numtabs)} Tag {node.Name} Debería tener al menos un tag view", 2);
            }

            numtabs--;
        }
    }
}
