using System;
using System.IO;

namespace Plastic_Analizer
{
    public class Save2Log
    {
        internal StreamWriter _wLog { get; set; }
        public Save2Log(string fileLog )
        {
            _wLog = File.CreateText(fileLog);
            _wLog.WriteLine("==================================================================================================");
            _wLog.WriteLine($"Fecha   : {DateTime.Now.ToLongDateString()} ");
            _wLog.WriteLine($"OS      : {Environment.OSVersion.ToString()} ");
            _wLog.WriteLine($"Máquina : {Environment.MachineName}");
            _wLog.WriteLine($"Usuario : {Environment.UserName}");
            //_wLog.WriteLine($"Directorio sistema : {Environment.SystemDirectory}");
            _wLog.WriteLine($"Directorio Analizado : {Path.GetDirectoryName(fileLog)}");
            _wLog.WriteLine("==================================================================================================\n");

        }
        public void CloseLog()
        {
            _wLog.Close();
        }
        uint howManyTabs(string s)
        {
            return (uint)s.Split('\t').Length - 1;
        }
        public void Log(string logMessage, short isError = 0)
        {
            string cPrep = ":";
            bool print = true;
            uint nTabs = howManyTabs(logMessage);
            string cTabs = new String(' ', (int)nTabs);
            logMessage = logMessage.Replace("\t", " ");
            switch (isError)
            {

                case 1://warning
                    cPrep = $"warning : {logMessage} ";
                    break;
                case 2://Error
                    cPrep = $"Error : {logMessage}";
                    break;
                case 3://jodido estoy
                    cPrep = $"Fatal Error : {logMessage}";
                    break;
                case 4://anuncia un fichero
                    cPrep = $"{logMessage}";
                    break;
                default:
                    if (MainClass.avoidNormalMessages == 1) print = false;
                    break;
            }
            if (print) { 
                _wLog.WriteLine($"{DateTime.Now.ToLongTimeString()}{cTabs} {cPrep}");
                _wLog.Flush();

            }

        }
    }
}
