using System.Xml;
namespace Plastic_Analizer
{
    public static class ChkSCAG
    {
        internal static short numtabs;
        public static void check(XmlNode rootPlastic)
        {
            numtabs = 5;
            short chkConfiguration = 0; 

            if (rootPlastic.HasChildNodes)
            {
                foreach (XmlNode node in rootPlastic.ChildNodes)
                {
                    switch (node.Name)
                    {
                        case "configuration":
                            Check_Configuration(node);
                            chkConfiguration++;
                            break;
                        default:
                            //No debería estar en ese fichero
                            MainClass.log2.Log($"{MainClass.GetTabs(numtabs)} Tag {node.Name} desconocida ", 1);
                            break;
                    }
                }
                //comprobación de existencia de mínimos
                MainClass.checkExistence(val2Check: chkConfiguration,minimumValue: 1,nameTag: "configuration");

            }
            else
                MainClass.log2.Log($"{MainClass.GetTabs(numtabs)} No hay nodos hijos ", 2);
}


        internal static void Check_Configuration(XmlNode node)
        {
            MainClass.log2.Log($"{MainClass.GetTabs(numtabs)} Tag {node.Name} ");
            numtabs++;
            MainClass.Check_Attribute(node: node, attrb: "title",    nTabs: numtabs);
            MainClass.Check_Attribute(node: node, attrb: "link_scheduler",  nTabs: numtabs, defaultValue: "1");
            MainClass.Check_Attribute(node: node, attrb: "use_pin",  nTabs: numtabs, defaultValue: "0");
            MainClass.Check_Attribute(node: node, attrb: "pin_interval",  nTabs: numtabs, defaultValue: "120000");
            numtabs--;
        }
    }
}
