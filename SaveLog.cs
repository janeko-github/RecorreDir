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
        public void Log(string logMessage, Int16 isError = 0)
        {
            string cPrep = ":";
            switch (isError)
            {

                case 1://warning
                    cPrep = "warning : ";
                    break;
                case 2://warning
                    cPrep = "Error : ";
                    break;
            }

            _wLog.WriteLine($"{DateTime.Now.ToLongTimeString()} {cPrep}{logMessage}");

        }
    }
}
