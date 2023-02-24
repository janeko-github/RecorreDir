using System;
using System.IO;

namespace RecorreDir
{
    public class Save2Log
    {
        internal StreamWriter _wLog { get; set; }
        public Save2Log(string fileLog )
        {
            _wLog = File.CreateText(fileLog);
            _wLog.WriteLine("=================================================\n");
            _wLog.WriteLine($"OS      : {Environment.OSVersion.ToString()} ");
            _wLog.WriteLine($"Máquina : {Environment.MachineName}");
            _wLog.WriteLine($"Usuario : {Environment.UserName}");
            //_wLog.WriteLine($"Directorio sistema : {Environment.SystemDirectory}");
            _wLog.WriteLine($"Directorio Analizado : {Path.GetDirectoryName(fileLog)}");

            _wLog.WriteLine("=================================================\n");

        }
        public void CloseLog()
        {
            _wLog.Close();
        }
        public void Log(string logMessage, short isError = 0)
        {
            string cPrep = ":";
            bool print = true;
            switch (isError)
            {

                case 1://warning
                    cPrep = "warning : ";
                    break;
                case 2://Error
                    cPrep = "Error : ";
                    break;
                case 3://jodido estoy
                    cPrep = "Error : ";
                    break;
                case 4://anuncia un fichero
                    cPrep = "";
                    break;
                default:
                    if (MainClass.avoidNormalMessages == 1) print = false;
                        break;
            }
            if(print ) 
                _wLog.WriteLine($"{DateTime.Now.ToLongTimeString()} {cPrep}{logMessage}");

        }
    }
}
