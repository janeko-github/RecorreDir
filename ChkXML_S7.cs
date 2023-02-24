using System.Xml;
namespace RecorreDir
{
    public static class ChkXML_S7
    {
        internal static short numtabs;
        public static void check(XmlNode rootPlastic)
        {
            numtabs = 5;
            short chkS7 = 0;

            if (rootPlastic.HasChildNodes)
            {
                foreach (XmlNode node in rootPlastic.ChildNodes)
                {
                    switch (node.Name)
                    {
                        case "s7":
                            Check_S7(node);
                            chkS7++;
                            break;
                        default:
                            //No debería estar en ese fichero
                            MainClass.log2.Log($"{MainClass.GetTabs(numtabs)} Tag {node.Name} desconocida ", 1);
                            break;
                    }
                }
                //comprobación de existencia de mínimos
                MainClass.checkExistence(val2Check:chkS7, minimumValue:1, nameTag: "s7");


            }
            else
                MainClass.log2.Log($"{MainClass.GetTabs(numtabs)} No hay nodos hijos ", 2);
        }


        internal static void Check_S7(XmlNode node)
        {
            short nodeCount = 0;
            MainClass.log2.Log($"{MainClass.GetTabs(numtabs)} Tag {node.Name} ");
            numtabs++;
            MainClass.Check_Attribute(node: node, attrb: "ip", required: true, nTabs: numtabs);
            MainClass.Check_Attribute(node: node, attrb: "slot", required: true, nTabs: numtabs);
            MainClass.Check_Attribute(node: node, attrb: "rack", required: true, nTabs: numtabs);
            MainClass.Check_Attribute(node: node, attrb: "type", required: true, nTabs: numtabs);
            if (node.HasChildNodes)
            {
                foreach (XmlNode aListen in node.ChildNodes)
                {
                    nodeCount++;
                    //TODO En el fichero file debe de existir un folder con name = al atributo path source
                    if (aListen.Name == "listen")
                    {
                        MainClass.Check_Attribute(node: aListen, attrb: "ip", required: true, nTabs: numtabs);
                        MainClass.Check_Attribute(node: aListen, attrb: "port", required: true, nTabs: numtabs);
                    }
                    else
                    {
                        MainClass.log2.Log($"{MainClass.GetTabs(numtabs)} Tag {node.Name} Existe un tag denominado {aListen.Name} que no es un folder.", 1);
                    }
                }
                MainClass.checkExistence(val2Check: nodeCount, minimumValue: 1, nameTag: "listen");

            }
            else
            {
                MainClass.log2.Log($"{MainClass.GetTabs(numtabs)} Tag {node.Name} Debería tener al menos un tag listen", 2);
            }

            numtabs--;
        }

    }


}
