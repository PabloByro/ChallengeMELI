using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Configuration;
namespace Ejericio1MELI
{
    class ClsCSV
    {
        private static void CreateDirectory()
        {
            try
            {
                string sPath = ConfigurationManager.AppSettings["CSVPath"];

                if (!Directory.Exists(sPath))
                    Directory.CreateDirectory(sPath);
            }
            catch (DirectoryNotFoundException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        
            public static void GrabarCSV(string sNameArch,string sMensaje )
            {
                try
                {

            
                        CreateDirectory();
                        string sPath = ConfigurationManager.AppSettings["CSVPath"];
                        string strFilePath = @sPath + sNameArch + ".csv";
                        string strSeperator = ",";
                        StringBuilder sbOutput = new StringBuilder();

                        sbOutput.AppendLine(string.Join(strSeperator, sMensaje));
                        bool result = File.Exists(strFilePath);
                    if (result == true)
                    {
                        // Agrega lineas al archivo ya existente
                        File.AppendAllText(strFilePath, sbOutput.ToString());
                    }
                    else 
                    {
                        // crea el archivo csv nuevo
                        File.WriteAllText(strFilePath, sbOutput.ToString());
                    }

                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
        }
        
    }
}
