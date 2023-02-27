using System.Xml;
using System.Collections.Generic;
namespace Plastic_Analizer
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
            MainClass.Check_Attribute(node: node, attrb: "width",  nTabs: numtabs, defaultValue: "*", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
            MainClass.Check_Attribute(node: node, attrb: "height",  nTabs: numtabs, defaultValue: "*", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
            MainClass.Check_Attribute(node: node, attrb: "name",  nTabs: numtabs, defaultValue: "*", flags: MainClass.AttributeFlag.NonRequired);
            MainClass.Check_Attribute(node: node, attrb: "description",  nTabs: numtabs, defaultValue: "*", flags: MainClass.AttributeFlag.NonRequired);
            MainClass.Check_Attribute(node: node, attrb: "show_home",  nTabs: numtabs, defaultValue: "1", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
            MainClass.Check_File(node: node, attrb: "background_image",  nTabs: numtabs);
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
                            MainClass.Check_Attribute(node: innerNode, attrb: "layer",  nTabs: numtabs, defaultValue: "*", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
                            MainClass.Check_Attribute(node: innerNode, attrb: "x",  nTabs: numtabs, defaultValue: "0", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
                            MainClass.Check_Attribute(node: innerNode, attrb: "y",  nTabs: numtabs, defaultValue: "30", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
                            MainClass.Check_Attribute(node: innerNode, attrb: "width",  nTabs: numtabs, defaultValue: "468", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
                            MainClass.Check_Attribute(node: innerNode, attrb: "height",  nTabs: numtabs, defaultValue: "30", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
                            MainClass.Check_Attribute(node: innerNode, attrb: "name",  nTabs: numtabs, defaultValue: "", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
                            MainClass.Check_Attribute(node: innerNode, attrb: "hint",  nTabs: numtabs );
                            MainClass.Check_Attribute(node: innerNode, attrb: "group",  nTabs: numtabs);
                            MainClass.Check_Attribute(node: innerNode, attrb: "read_address",  nTabs: numtabs);
                            MainClass.Check_Attribute(node: innerNode, attrb: "write_address",  nTabs: numtabs);
                            MainClass.Check_Attribute(node: innerNode, attrb: "read_symbolic",  nTabs: numtabs);
                            MainClass.Check_Attribute(node: innerNode, attrb: "write_symbolic",  nTabs: numtabs);
                            MainClass.Check_Attribute(node: innerNode, attrb: "text",  nTabs: numtabs, defaultValue: "", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
                            MainClass.Check_Attribute(node: innerNode, attrb: "value_translation",  nTabs: numtabs, defaultValue: "");
                            MainClass.Check_Attribute(node: innerNode, attrb: "font_name",  nTabs: numtabs, defaultValue: "Arial", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
                            MainClass.Check_Attribute(node: innerNode, attrb: "font_size",  nTabs: numtabs, defaultValue: "24", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
                            MainClass.Check_Attribute(node: innerNode, attrb: "font_style",  nTabs: numtabs, defaultValue: "BI");
                            MainClass.Check_Attribute(node: innerNode, attrb: "color",  nTabs: numtabs, defaultValue: "FF000000");
                            MainClass.Check_Attribute(node: innerNode, attrb: "mostrar_marco",  nTabs: numtabs, defaultValue: "0");
                            MainClass.Check_Attribute(node: innerNode, attrb: "padding_marco",  nTabs: numtabs, defaultValue: "0");
                            MainClass.Check_Attribute(node: innerNode, attrb: "color_borde",  nTabs: numtabs, defaultValue: "0");
                            MainClass.Check_Attribute(node: innerNode, attrb: "color_fondo",  nTabs: numtabs, defaultValue: "0");
                            MainClass.Check_Attribute(node: innerNode, attrb: "factor",  nTabs: numtabs);
                            MainClass.Check_Attribute(node: innerNode, attrb: "decimal_places",  nTabs: numtabs);
                            MainClass.Check_Attribute(node: innerNode, attrb: "value_transformation",  nTabs: numtabs);
                            break;
                        case "image":
                            /*
                             *                    
<image image="Volver.png" value_translation="" url="" view="telemetria.scav" view_popup="0" view_params="" graph="" write_value="" alarm_address="" alarm_translation="" popup_empotrado="0"
popup_empotrado_x="0" popup_empotrado_y="0" script_action="" script_arg="" set_manual="0" layer="0" x="20" y="610" width="70" height="30" name="Object49" hint="" group="" read_address="" write_address="" 
read_symbolic="" write_symbolic="" />
                            
                             * */
                            MainClass.Check_File(node: innerNode, attrb: "image",  nTabs: numtabs);
                            MainClass.Check_Attribute(node: innerNode, attrb: "value_translation",  nTabs: numtabs);
                            MainClass.Check_Attribute(node: innerNode, attrb: "url",  nTabs: numtabs);
                            MainClass.Check_File(node: innerNode, attrb: "view",  nTabs: numtabs, required: false);
                            MainClass.Check_Attribute(node: innerNode, attrb: "view_popup",  nTabs: numtabs, defaultValue: "0");
                            MainClass.Check_Attribute(node: innerNode, attrb: "view_params",  nTabs: numtabs);
                            MainClass.Check_Attribute(node: innerNode, attrb: "graph",  nTabs: numtabs);
                            MainClass.Check_Attribute(node: innerNode, attrb: "write_value",  nTabs: numtabs);
                            MainClass.Check_Attribute(node: innerNode, attrb: "alarm_address",  nTabs: numtabs);
                            MainClass.Check_Attribute(node: innerNode, attrb: "alarm_translation",  nTabs: numtabs);
                            MainClass.Check_Attribute(node: innerNode, attrb: "popup_empotrado",  nTabs: numtabs);
                            MainClass.Check_Attribute(node: innerNode, attrb: "popup_empotrado_x",  nTabs: numtabs, defaultValue: "0");
                            MainClass.Check_Attribute(node: innerNode, attrb: "popup_empotrado_y",  nTabs: numtabs, defaultValue: "0");
                            MainClass.Check_Attribute(node: innerNode, attrb: "script_action",  nTabs: numtabs);
                            MainClass.Check_Attribute(node: innerNode, attrb: "script_arg",  nTabs: numtabs);
                            MainClass.Check_Attribute(node: innerNode, attrb: "set_manual",  nTabs: numtabs, defaultValue: "0");
                            MainClass.Check_Attribute(node: innerNode, attrb: "layer",  nTabs: numtabs, defaultValue: "0");
                            MainClass.Check_Attribute(node: innerNode, attrb: "x",  nTabs: numtabs, defaultValue: "20", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
                            MainClass.Check_Attribute(node: innerNode, attrb: "y",  nTabs: numtabs, defaultValue: "610", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
                            MainClass.Check_Attribute(node: innerNode, attrb: "width",  nTabs: numtabs, defaultValue: "70", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
                            MainClass.Check_Attribute(node: innerNode, attrb: "height",  nTabs: numtabs, defaultValue: "30", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
                            MainClass.Check_Attribute(node: innerNode, attrb: "name",  nTabs: numtabs, defaultValue: "*", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
                            MainClass.Check_Attribute(node: innerNode, attrb: "hint",  nTabs: numtabs);
                            MainClass.Check_Attribute(node: innerNode, attrb: "group",  nTabs: numtabs);
                            MainClass.Check_Attribute(node: innerNode, attrb: "read_address",  nTabs: numtabs);
                            MainClass.Check_Attribute(node: innerNode, attrb: "write_address",  nTabs: numtabs);
                            MainClass.Check_Attribute(node: innerNode, attrb: "read_symbolic",  nTabs: numtabs);
                            MainClass.Check_Attribute(node: innerNode, attrb: "write_symbolic",  nTabs: numtabs);
                            break;
                        case "textinput":
                            /*
<textinput layer="2" x="320" y="170" width="100" height="30" name="Object5" hint="" group="" read_address="/direcciones/#101" write_address="" read_symbolic="" write_symbolic="" text="" text_left="" 
text_right="V" font_name="Arial" font_size="12" font_style="B" color="FF000000" factor="" decimal_places="1" />
                             * */
                            MainClass.Check_Attribute(node: innerNode, attrb: "layer",  nTabs: numtabs, defaultValue: "2");
                            MainClass.Check_Attribute(node: innerNode, attrb: "x",  nTabs: numtabs, defaultValue: "10", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
                            MainClass.Check_Attribute(node: innerNode, attrb: "y",  nTabs: numtabs, defaultValue: "10", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
                            MainClass.Check_Attribute(node: innerNode, attrb: "width",  nTabs: numtabs, defaultValue: "100", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
                            MainClass.Check_Attribute(node: innerNode, attrb: "height",  nTabs: numtabs, defaultValue: "30", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
                            MainClass.Check_Attribute(node: innerNode, attrb: "name",  nTabs: numtabs, defaultValue: "*", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
                            MainClass.Check_Attribute(node: innerNode, attrb: "hint",  nTabs: numtabs);
                            MainClass.Check_Attribute(node: innerNode, attrb: "group",  nTabs: numtabs);
                            MainClass.Check_Attribute(node: innerNode, attrb: "read_address",  nTabs: numtabs, defaultValue: "*");
                            MainClass.Check_Attribute(node: innerNode, attrb: "read_symbolic",  nTabs: numtabs);
                            MainClass.Check_Attribute(node: innerNode, attrb: "write_symbolic",  nTabs: numtabs);
                            MainClass.Check_Attribute(node: innerNode, attrb: "text",  nTabs: numtabs);
                            MainClass.Check_Attribute(node: innerNode, attrb: "text_left",  nTabs: numtabs);
                            MainClass.Check_Attribute(node: innerNode, attrb: "text_right",  nTabs: numtabs);
                            MainClass.Check_Attribute(node: innerNode, attrb: "font_name",  nTabs: numtabs, defaultValue: "Arial", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
                            MainClass.Check_Attribute(node: innerNode, attrb: "font_size",  nTabs: numtabs, defaultValue: "24", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
                            MainClass.Check_Attribute(node: innerNode, attrb: "font_style",  nTabs: numtabs, defaultValue: "BI");
                            MainClass.Check_Attribute(node: innerNode, attrb: "color",  nTabs: numtabs, defaultValue: "FF000000");
                            MainClass.Check_Attribute(node: innerNode, attrb: "factor",  nTabs: numtabs);
                            MainClass.Check_Attribute(node: innerNode, attrb: "decimal_places",  nTabs: numtabs);

                            break;
                        case "regulation":
                            /*
<regulation layer="0" x="668" y="561" width="54" height="26" name="Object70" hint="" group="4" blocked="0" display_address="" display_symbolic="" display_value="" read_address="/direcciones/#1800" write_address="" 
read_symbolic="" write_symbolic="" font_name="Arial Narrow" font_size="12" font_style="B" minimo="0" maximo="100" color="FF000000" script_action="" script_arg="" />

                            **/
                            MainClass.Check_Attribute(node: innerNode, attrb: "layer", nTabs: numtabs, defaultValue: "2");
                            MainClass.Check_Attribute(node: innerNode, attrb: "x", nTabs: numtabs, defaultValue: "10", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
                            MainClass.Check_Attribute(node: innerNode, attrb: "y", nTabs: numtabs, defaultValue: "10", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
                            MainClass.Check_Attribute(node: innerNode, attrb: "width", nTabs: numtabs, defaultValue: "100", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
                            MainClass.Check_Attribute(node: innerNode, attrb: "height", nTabs: numtabs, defaultValue: "30", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
                            MainClass.Check_Attribute(node: innerNode, attrb: "name", nTabs: numtabs, defaultValue: "*", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
                            MainClass.Check_Attribute(node: innerNode, attrb: "hint", nTabs: numtabs);
                            MainClass.Check_Attribute(node: innerNode, attrb: "group", nTabs: numtabs);
                            MainClass.Check_Attribute(node: innerNode, attrb: "blocked", nTabs: numtabs);
                            MainClass.Check_Attribute(node: innerNode, attrb: "display_address", nTabs: numtabs, defaultValue: "*");
                            MainClass.Check_Attribute(node: innerNode, attrb: "display_symbolic", nTabs: numtabs, defaultValue: "*");
                            MainClass.Check_Attribute(node: innerNode, attrb: "display_value", nTabs: numtabs, defaultValue: "*");
                            MainClass.Check_Attribute(node: innerNode, attrb: "read_address", nTabs: numtabs, defaultValue: "*");
                            MainClass.Check_Attribute(node: innerNode, attrb: "write_address", nTabs: numtabs, defaultValue: "*");
                            MainClass.Check_Attribute(node: innerNode, attrb: "read_symbolic", nTabs: numtabs);
                            MainClass.Check_Attribute(node: innerNode, attrb: "write_symbolic", nTabs: numtabs);
                            MainClass.Check_Attribute(node: innerNode, attrb: "font_name", nTabs: numtabs, defaultValue: "Arial", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
                            MainClass.Check_Attribute(node: innerNode, attrb: "font_size", nTabs: numtabs, defaultValue: "24", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
                            MainClass.Check_Attribute(node: innerNode, attrb: "font_style", nTabs: numtabs, defaultValue: "BI");
                            MainClass.Check_Attribute(node: innerNode, attrb: "minimo", nTabs: numtabs, defaultValue: "0", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
                            MainClass.Check_Attribute(node: innerNode, attrb: "maximo", nTabs: numtabs, defaultValue: "100", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
                            MainClass.Check_Attribute(node: innerNode, attrb: "color", nTabs: numtabs, defaultValue: "FF000000");
                            MainClass.Check_Attribute(node: innerNode, attrb: "script_action", nTabs: numtabs);
                            MainClass.Check_Attribute(node: innerNode, attrb: "script_arg", nTabs: numtabs);
                            break;
                        case "frame":
                            /*
* <frame layer="0" x="1300" y="508" width="267" height="192" name="Object75" hint="" group="" blocked="0" display_address="" display_symbolic="" display_value="" url="/snapshot.last" type="Normal" />

                            **/
                            MainClass.Check_Attribute(node: innerNode, attrb: "layer", nTabs: numtabs, defaultValue: "2");
                            MainClass.Check_Attribute(node: innerNode, attrb: "x", nTabs: numtabs, defaultValue: "10", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
                            MainClass.Check_Attribute(node: innerNode, attrb: "y", nTabs: numtabs, defaultValue: "10", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
                            MainClass.Check_Attribute(node: innerNode, attrb: "width", nTabs: numtabs, defaultValue: "100", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
                            MainClass.Check_Attribute(node: innerNode, attrb: "height", nTabs: numtabs, defaultValue: "30", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
                            MainClass.Check_Attribute(node: innerNode, attrb: "name", nTabs: numtabs, defaultValue: "*", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
                            MainClass.Check_Attribute(node: innerNode, attrb: "hint", nTabs: numtabs);
                            MainClass.Check_Attribute(node: innerNode, attrb: "group", nTabs: numtabs);
                            MainClass.Check_Attribute(node: innerNode, attrb: "blocked", nTabs: numtabs);
                            MainClass.Check_Attribute(node: innerNode, attrb: "display_address", nTabs: numtabs, defaultValue: "*");
                            MainClass.Check_Attribute(node: innerNode, attrb: "display_symbolic", nTabs: numtabs, defaultValue: "*");
                            MainClass.Check_Attribute(node: innerNode, attrb: "display_value", nTabs: numtabs, defaultValue: "*");
                            MainClass.Check_Attribute(node: innerNode, attrb: "url", nTabs: numtabs, defaultValue: "*", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
                            MainClass.Check_Attribute(node: innerNode, attrb: "type", nTabs: numtabs, defaultValue: "Normal", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);

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
