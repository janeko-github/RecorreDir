using System;
using System.IO;
using System.Collections.Generic;

namespace RecorreDir
{
    /*
     * Presupongo aplicaciones modularizadas. Cada módulo es "casi" como una aplicación. La diferencia es que una aplicación
     * sería un módulo que no puede tener un módulo padre y debe tener, al menos, un modulo hijo y, además tienen el tag plastic.
     * Un modulo "normal" puede tener, o no, módulos hijos y un tag distinto de plastic.
     * Aún no se contempla varias aplicaciones en un fichero. De momento sólo puede haber una.¿?
     * 
     * 
     * El orden será:
     *      definir los módulos que no tiene módulos hijos
     *      definir los módulos que contienen módulos hijos
     *      definit aplicaciones    
     */   
    public class GlobalConfig
    {
        /*
         * Define los atributos un Tag de qué tipo deben de ser
         */
        public enum TypeOfAttribute
        {
            Undefined,
            NumericInt,
            NumericFloat,
            Bool,
            IP,
            Port,
            File,
            String,
            Address
        }

        public struct Atributo
        {
            public string name;
            public TypeOfAttribute attributeType;
            public string defaultValue;
            public bool required;
            public bool unique;//en esa aplicación no debe de existir otro poseedor con un atributo con igual valor
            public bool isFile;

        }
        public struct TagNames //nombre del módulo a buscar en tag
        {
            public bool required;
            public List<string> tags;
        }
        public struct Tag //módulos 
        {
            public string name;
            public List<Atributo> atributos;
            public bool requiredTags;
            public List<TagNames> tagNames;//una lista de módulos requeridos o no

        }
        public struct Aplicacion //es un módulo especial, tag plastic y no tiene parent
        {
            //public string applicationExtension="pgctrl";//ya veremos
            public bool requiredAttributes;
            public List<Atributo> atributos;//tiene un atributo obligatorio que es application
            public List<TagNames> tagNames;//una lista de módulos requeridos o no. Al menos un modulo
        }
        public static List<Aplicacion> aplicaciones;
        public static List<Tag> tags;
        protected Atributo DefineAtributo(string name = "", bool isFile = false, bool required = false, bool unique = false, TypeOfAttribute attributeType = TypeOfAttribute.Undefined, string defaultValue = "")
        {
            Atributo atributo = new Atributo();
            atributo.name = name;
            atributo.isFile = isFile;
            atributo.required = required;
            atributo.attributeType = attributeType;
            atributo.unique = unique;//mirará que en el parent no exita otro igual ????
            atributo.defaultValue = defaultValue;
            return atributo;
        }
        protected Tag DefineTag(string name = "", bool requiredTags = false,List<Atributo> atributos = null)
        {
            Tag tag = new Tag();
            tag.name = name;
            tag.requiredTags = requiredTags;
            tag.atributos=atributos;
            return tag;
        }
        protected void DefineFileTag()
        {
            List<Atributo> atributos = null;
            atributos.Add(
                DefineAtributo(name:"name",isFile:true,required:true,attributeType: TypeOfAttribute.File)
            );   
            Tag file = DefineTag(name:"file", requiredTags:false,atributos:atributos);

            tags.Add(file);
        }
        protected void DefineIP(string name="",string ipDefault="all",string portDefault="default")
        {
            List<Atributo> atributos = null;
            atributos.Add(
                DefineAtributo(name: "ip", isFile: false, required: true, attributeType: TypeOfAttribute.IP, defaultValue: ipDefault)
            );
            atributos.Add(
                DefineAtributo(name: "port", isFile: false, required: true, attributeType: TypeOfAttribute.Port, defaultValue: portDefault)
            );

            Tag asIp = DefineTag(name: name, requiredTags: false, atributos: atributos);
            tags.Add(asIp);
        }
        protected void DefineHttpmonitor(string defaultIp = "all", string defaultPort = "8091")
        {
            DefineIP(name:"httpmonitor",ipDefault: defaultIp, portDefault:defaultPort);
        }
        protected void DefineHttpcontrol(string defaultIp = "all", string defaultPort = "default")
        {
            DefineIP(name: "httpcontrol", ipDefault: defaultIp, portDefault: defaultPort);
        }
        protected void DefineHttpplasticax(string defaultIp = "all", string defaultPort = "default")
        {
            DefineIP(name: "httpplasticax", ipDefault: defaultIp, portDefault: defaultPort);
        }
        protected void DefineListen(string defaultIp = "all",string defaultPort="2004")
        {
            DefineIP(name: "listen", ipDefault: defaultIp, portDefault: defaultPort);
        }
        protected void DefineModbus(string defaultIp = "all", string defaultPort = "4001")
        {
            DefineIP(name: "modbus", ipDefault: defaultIp, portDefault: "defaultPort");
        }

        protected void DefineFolder(string name="",string displayName="",string flag_sheduler="0",string expiration_seconds="",bool requiredTags=true)
        {
            List<Atributo> atributos = null;
            atributos.Add(
                DefineAtributo(name: "name", isFile: false, required: true, attributeType: TypeOfAttribute.String, defaultValue: name)
            );
            atributos.Add(
                DefineAtributo(name: "display_name", isFile: false, required: true, attributeType: TypeOfAttribute.String, defaultValue:displayName)
            );
            atributos.Add(
                DefineAtributo(name: "flag_scheduler", isFile: false, required: true, attributeType: TypeOfAttribute.NumericInt, defaultValue: flag_sheduler)
            );
            atributos.Add(
                DefineAtributo(name: "expiration_seconds", isFile: false, required: true, attributeType: TypeOfAttribute.NumericInt, defaultValue: expiration_seconds)
            );
            Tag folder = DefineTag(name: "folder", requiredTags: requiredTags, atributos: atributos);
            tags.Add(folder);
        }
        protected void DefineParameters()
        {
            List<Atributo> atributos = null;
            atributos.Add(
                DefineAtributo(name: "interrogation_ms", isFile: false, required: true, attributeType: TypeOfAttribute.NumericInt, defaultValue: "50")
            );
            atributos.Add(
                DefineAtributo(name: "log_interrogation", isFile: false, required: true, attributeType: TypeOfAttribute.NumericInt, defaultValue: "0")
            );
            atributos.Add(
                DefineAtributo(name: "log_sets", isFile: false, required: true, attributeType: TypeOfAttribute.NumericInt, defaultValue: "0")
            );
            atributos.Add(
                DefineAtributo(name: "log_incoming", isFile: false, required: true, attributeType: TypeOfAttribute.NumericInt, defaultValue: "0")
            );
            atributos.Add(
                DefineAtributo(name: "log_separate_security_log", isFile: false, required: true, attributeType: TypeOfAttribute.NumericInt, defaultValue: "0")
            );
            atributos.Add(
                DefineAtributo(name: "datalog_enabled", isFile: false, required: true, attributeType: TypeOfAttribute.NumericInt, defaultValue: "1")
            );
            atributos.Add(
                DefineAtributo(name: "datalog_filter", isFile: false, required: true, attributeType: TypeOfAttribute.NumericInt, defaultValue: "0")
            );
            atributos.Add(
                DefineAtributo(name: "datalog_mininterval", isFile: false, required: true, attributeType: TypeOfAttribute.NumericInt, defaultValue: "180")
            );
            atributos.Add(
                DefineAtributo(name: "expiration_seconds", isFile: false, required: true, attributeType: TypeOfAttribute.NumericInt, defaultValue: "900")
            );

            atributos.Add(
                DefineAtributo(name: "limit_datalog_days", isFile: false, required: true, attributeType: TypeOfAttribute.NumericInt, defaultValue: "3")
            );
            atributos.Add(
                DefineAtributo(name: "hide_module_control", isFile: false, required: true, attributeType: TypeOfAttribute.NumericInt, defaultValue: "0")
            );
            atributos.Add(
                DefineAtributo(name: "system_password", isFile: false, required: true, attributeType: TypeOfAttribute.String, defaultValue: "")
            );
            atributos.Add(
                DefineAtributo(name: "default_module", isFile: false, required: false, attributeType: TypeOfAttribute.NumericInt, defaultValue: "")
            );
            atributos.Add(
                DefineAtributo(name: "reload_files", isFile: false, required: true, attributeType: TypeOfAttribute.Bool, defaultValue: "False")
            );
            atributos.Add(
                DefineAtributo(name: "short_cycle_repetition", isFile: false, required: true, attributeType: TypeOfAttribute.String, defaultValue: "3")
            );

            atributos.Add(
                DefineAtributo(name: "short_cycle_pause", isFile: false, required: true, attributeType: TypeOfAttribute.NumericInt, defaultValue: "25")
            );
            atributos.Add(
                DefineAtributo(name: "target_cycles", isFile: false, required: false, attributeType: TypeOfAttribute.NumericInt, defaultValue: "0")
            );
            atributos.Add(
                DefineAtributo(name: "separate_volatile_database", isFile: false, required: true, attributeType: TypeOfAttribute.NumericInt, defaultValue: "0")
            );
            atributos.Add(
                DefineAtributo(name: "persistent_manual_temporal", isFile: false, required: true, attributeType: TypeOfAttribute.String, defaultValue: "0")
            );

            Tag parameters = DefineTag(name: "parameters", requiredTags: false, atributos: atributos);
            tags.Add(parameters);
        }
        protected void DefineParametersModBus() //????????
        {
            List<Atributo> atributos = null;
            atributos.Add(
                DefineAtributo(name: "modbustcp", isFile: false, required: true, attributeType: TypeOfAttribute.NumericInt, defaultValue: "0")
            );
            atributos.Add(
                DefineAtributo(name: "fast", isFile: false, required: true, attributeType: TypeOfAttribute.NumericInt, defaultValue: "0")
            );
            atributos.Add(
                DefineAtributo(name: "logtimeout_series", isFile: false, required: true, attributeType: TypeOfAttribute.NumericInt, defaultValue: "1000")
            );
            Tag parameters = DefineTag(name: "parameters", requiredTags: false, atributos: atributos);
            tags.Add(parameters);
        }
        public static void LoadApps()
        {
            //leer de un fichero la estructura
            /*
             *  <plastic>
             *      <application>            
             *             <attribute required>name</attribute>
             *             <attribute required default="no description">description</attribute>
             *             <attribute required default=true>requiredAttributes</attribute>
             *             <list attributes>             
             *             </list>            
             *             <attribute required default=true>requiredTags</attribute>            
             *              <list tags>                         
             *              </list>            
             *      </application>            
             *      ........            
             *      <application>            
             *      </application>            
             * </plastic>
             */
            //Aplicacion app = new Aplicacion();


        }

    }
}
