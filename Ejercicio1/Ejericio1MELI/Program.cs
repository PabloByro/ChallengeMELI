using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Net;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Text.Json;
using System.Configuration;

namespace Ejericio1MELI
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();
        

        private static async Task ProcessRepositories(string sSite_id, string sSeller_id, int iCant, string snameField)
        {
            try
            {
                string sMeliApi = (ConfigurationManager.AppSettings["urlMELI"]); //url API MELI
                
                if (iCant == 0) //Si la cantidad es cero, pongo el valor defecto que tengo el appconfig
                {
                    iCant = Convert.ToInt32(ConfigurationManager.AppSettings["CantPaginacion"]); //por defecto esta en 50 
                }
                    
                //obtenemos el status de la cnx, cabecera, y cuerpo
                // var Req = sMeliApi + sSite_id + "/search?seller_id=" + sSeller_id + "&logistic_type=fulfillment&offset =" + iCant.ToString(); //+ "&limit =3";
                var request = await client.GetAsync(sMeliApi + sSite_id + "/search?seller_id=" + sSeller_id + "&logistic_type=fulfillment&offset =" + iCant.ToString());
                var requestContent = request.Content;
                string jsonContent = requestContent.ReadAsStringAsync().Result;
                JObject rss = JObject.Parse(jsonContent);

                if (request.IsSuccessStatusCode)
                {
                    //Recuperar cantidad de productos
                    string rssTotal = (string)rss["paging"]["total"];   
                    //Recupero los valores de cada indicados
                    string site_id  =  (string)rss["site_id"];
                    string seller_id = (string)rss["seller"]["id"];
                    string nickname = (string)rss["seller"]["nickname"];

                    
                    string id_item = (string)rss["results"].[0]["id"];
                

                    int ipos = 0;                    
                    var postId =
                                from p in rss["results"]
                                select (string)p["id"];
                                
                    foreach (var item in postId)
                    {
                            string title_item = (string)rss["results"][ipos]["title"];
                        
                   
                        string sMessage = site_id + "," + seller_id + "," + nickname + "," + item + "," + title_item;


                        if (snameField == "") //Si no recibi por parametros el nombre del archivo, es 1era vez y voy a crearlo, caso contrario seguire agregando datos.
                        {
                            
                            snameField = site_id + seller_id + "_" + DateTime.Now.ToString("yyyyMMddhhmmss");
                        }
                        
                        
                        ClsLog.WriteLog(sMessage, snameField); //Verifico que la carpeta donde se grabara el log exista y luego lo grabo
                        ClsCSV.GrabarCSV(snameField, sMessage); //Verifico que la carpeta donde se grabara el CSV exista y luego lo grabo


                        ipos++;                                                                           
                        
                    }

                    if(Convert.ToInt32(rssTotal) > iCant)
                    {
                        iCant = iCant + Convert.ToInt32(ConfigurationManager.AppSettings["CantPaginacion"]);
                        await ProcessRepositories(sSite_id, sSeller_id, iCant, snameField);
                    }

                }
                else {
                    Console.Write("Estado Error");
                       };


            }
            catch (Exception)
            {

                throw;
            }

        }

        static async Task Main(string[] args)
        {
            //Los datos se podrian recuperar de un archivo txt, excel, ingreso manual  o base de datos SQL

            await ProcessRepositories("MLB", "244066576",0,"");
            await ProcessRepositories("MLA", "326659539",0,"");
            Environment.Exit(0);
        }

    }

}
