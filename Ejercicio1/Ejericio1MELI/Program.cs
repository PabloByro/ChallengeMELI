using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json.Linq;
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
                if (snameField == "") //Si no recibi por parametros el nombre del archivo, es 1era vez y voy a crearlo, caso contrario seguire agregando datos.
                {

                    snameField = sSite_id + sSeller_id + "_" + DateTime.Now.ToString("yyyyMMddhhmmss");
                }

                string sMeliApi = (ConfigurationManager.AppSettings["urlMELI"]); //url API MELI
                
              

                //obtenemos el status de la cnx, cabecera, y cuerpo
                // var Req = sMeliApi + sSite_id + "/search?seller_id=" + sSeller_id + "&logistic_type=fulfillment&offset =" + iCant.ToString(); //+ "&limit =3";
                var sURL = sMeliApi + sSite_id + "/search?seller_id=" + sSeller_id + "&logistic_type=fulfillment&offset=" + iCant.ToString();
                var request = await client.GetAsync(sURL);
                var requestContent = request.Content;
                string jsonContent = requestContent.ReadAsStringAsync().Result;
                JObject rss = JObject.Parse(jsonContent);

                if (request.IsSuccessStatusCode)
                {
                    //Recuperar cantidad de productos
                    string rssTotal = (string)rss["paging"]["total"];
                    if (Convert.ToInt32(rssTotal) > 0)
                    {
                        //Recupero los valores de cada indicados
                        string site_id = (string)rss["site_id"];
                        string seller_id = (string)rss["seller"]["id"];
                        string nickname = (string)rss["seller"]["nickname"];




                        int ipos = 0;
                        var postId =
                                    from p in rss["results"]
                                    select (string)p["id"];

                        foreach (var item in postId)
                        {
                            string title_item = (string)rss["results"][ipos]["title"];
                            string id_item = (string)rss["results"][ipos]["id"];
                            string category_id = (string)rss["results"][ipos]["category_id"];
                            string currency_Id = (string)rss["results"][ipos]["installments"]["currency_id"];
                            string amount = (string)rss["results"][ipos]["installments"]["amount"];

                            string sMeliApiCN = (ConfigurationManager.AppSettings["urlCategoryName"]); //url API MELI, para obtener las categorias

                            //obtenemos el status de la cnx, cabecera, y cuerpo                
                            var sURLCN = sMeliApi + category_id;
                            var requestCN = await client.GetAsync(sURLCN);
                            var requestContentCN = requestCN.Content;
                            string jsonContentCN = requestContentCN.ReadAsStringAsync().Result;
                            JObject rssCN = JObject.Parse(jsonContentCN);
                            string categoryName = "";
                            if (requestCN.IsSuccessStatusCode) //no logro recuperar los datos solicitados, siempre obtengo site not found
                            {
                                //Recuperar cantidad de productos
                                categoryName = (string)rssCN["name"];
                            }


                            //string sMessage = site_id + "," + seller_id + "," + nickname + "," + id_item + "," + item + "," + title_item + "," + category_id  + "," + currency_Id + amount;
                            string sMessage = site_id + "," + seller_id + "," + nickname + "," + id_item + "," + item + "," + title_item + "," + category_id + "," + categoryName + "," + currency_Id + amount;




                            ClsLog.WriteLog(sMessage, snameField); //Verifico que la carpeta donde se grabara el log exista y luego lo grabo
                            ClsCSV.GrabarCSV(snameField, sMessage); //Verifico que la carpeta donde se grabara el CSV exista y luego lo grabo

                            ipos++;

                        }

                        if (Convert.ToInt32(rssTotal) > iCant)
                        {
                            iCant = iCant + Convert.ToInt32(ConfigurationManager.AppSettings["CantPaginacion"]);
                            await ProcessRepositories(sSite_id, sSeller_id, iCant, snameField);
                        }
                    }
                    else
                    {
                        ClsLog.WriteLog("Sin productos", snameField); //Verifico que la carpeta donde se grabara el log exista y luego lo grabo
                    }
                    

                }
                else {
                    ClsLog.WriteLog("Estado Error NO hay registros", snameField); //Verifico que la carpeta donde se grabara el log exista y luego lo grabo
                    
                       };


            }
            catch (Exception ex)
            {
                ClsLog.WriteLog("Estado Error " + ex.Message.ToString(), snameField); ; //Verifico que la carpeta donde se grabara el log exista y luego lo grabo
                throw;
            }

        }
        private  static  async Task ObtenerCategoryName(string id)
        {
            try
            {
                string sMeliApi = (ConfigurationManager.AppSettings["urlCategoryName"]); //url API MELI, para obtener las categorias

                //obtenemos el status de la cnx, cabecera, y cuerpo                
                var sURL = sMeliApi + id;
                var request = await client.GetAsync(sURL);
                var requestContent = request.Content;
                string jsonContent = requestContent.ReadAsStringAsync().Result;
                JObject rss = JObject.Parse(jsonContent);

                if (request.IsSuccessStatusCode)
                {
                    //recupero el nombre de la categoria de cada producto
                    string rsCategoryName = (string)rss["name"];
                }
            }
            catch (Exception)
            {
                 throw;
             
                
            }
        }
     
        static async Task Main(string[] args)
        {
            //Los datos se podrian recuperar de un archivo txt, excel, ingreso manual  o base de datos SQL                        
            await ProcessRepositories("MLA", "326659539", 0, "");
            await ProcessRepositories("MLB", "244066576",0,"");            
            Environment.Exit(0);
        }

    }

}
