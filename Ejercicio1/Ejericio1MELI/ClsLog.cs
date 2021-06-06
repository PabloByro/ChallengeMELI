using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace Ejericio1MELI
{
    class ClsLog
    {
        public static void WriteLog(string message, string nameField )
        {
            CreateDirectory(); //1ero verifico que exista el path donde se va a guardar el archivo
            string spath = ConfigurationManager.AppSettings["LogPath"] + nameField + ".txt";
                        
            try
            {
                if (!File.Exists(spath))
                {
                    // Crea el archivo
                    using (StreamWriter sw = File.CreateText(spath))
                    {
                        sw.WriteLine(message);
                        sw.Close();
                    }
                }
                else
                {
                    StreamWriter file = new StreamWriter(spath, append: true) ;
                    file.WriteLine(message);
                    file.Close();
                }
                


            }
            catch (Exception ex)
            {
                throw ex;
            }
          
        }
      
        private static void CreateDirectory()
        {
            try
            {
                string sPath = ConfigurationManager.AppSettings["LogPath"] ;

                if (!Directory.Exists(sPath))
                    Directory.CreateDirectory(sPath);


            }
            catch (DirectoryNotFoundException ex)
            {
                throw new Exception(ex.Message);

            }
        }



    }
}
