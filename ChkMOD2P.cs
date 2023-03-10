using System.Xml;
namespace Plastic_Analizer
{
    public static class ChkMOD2P
    {
        internal static short numtabs;
        public static void check(XmlNode rootPlastic)
        {
            numtabs = 5;
            short chkListen = 0;short chkModbus = 0;

            if (rootPlastic.HasChildNodes)
            {
                foreach (XmlNode node in rootPlastic.ChildNodes)
                {
                    switch (node.Name)
                    {
                        case "listen":
                            Check_Listen(node);
                            chkListen++;
                            break;
                        case "modbus":
                            Check_Modbus(node);
                            chkModbus++;
                            break;
                        default:
                            //No debería estar en ese fichero
                            MainClass.log2.Log($"{MainClass.GetTabs(numtabs)} Tag {node.Name} desconocida ", 1);
                            break;
                    }
                }
                //comprobación de existencia de mínimos
                MainClass.checkExistence(val2Check: chkListen, minimumValue: 1,nameTag: "listen");
                MainClass.checkExistence(val2Check: chkModbus,minimumValue: 1, nameTag: "modbus");

            }
            else
                MainClass.log2.Log($"{MainClass.GetTabs(numtabs)} No hay nodos hijos ", 2);
        }


        internal static void Check_Listen(XmlNode node)
        {
            MainClass.log2.Log($"{MainClass.GetTabs(numtabs)} Tag {node.Name} ");
            numtabs++;
            MainClass.Check_Attribute(node: node, attrb: "ip",  nTabs: numtabs, defaultValue: "ass", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
            MainClass.Check_Attribute(node: node, attrb: "port",  nTabs: numtabs, defaultValue: "2002", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
            numtabs--;
        }
        internal static void Check_Modbus(XmlNode node)
        {
            MainClass.log2.Log($"{MainClass.GetTabs(numtabs)} Tag {node.Name} ");
            numtabs++;
            MainClass.Check_Attribute(node: node, attrb: "ip",  nTabs: numtabs, defaultValue: "*", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
            MainClass.Check_Attribute(node: node, attrb: "port",  nTabs: numtabs, defaultValue: "0", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
            MainClass.Check_Attribute(node: node, attrb: "serial",  nTabs: numtabs, defaultValue: "/dev/ttyS", flags: MainClass.AttributeFlag.Required | MainClass.AttributeFlag.MustHaveValue);
            numtabs--;
        }
    }
}
