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


                    //int iCantMax = 0;
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
                    
                    // string id_item = (string)rss["results"][0]["id"];
                    // string title_item = (string)rss["results"][0]["title"];
                    //     string cotegory_id = (string)rss["available_filters"][0]["values"][0]["id"];
                    //      string cotegory_name = (string)rss["available_filters"][0]["values"][0]["name"];
                    //   string currency_price = (string)rss["results"][0]["prices"]["currency_id"];

                    int ipos = 0;                    
                    var postId =
                                from p in rss["results"]
                                select (string)p["id"];
                                
                    foreach (var item in postId)
                    {
                            string title_item = (string)rss["results"][ipos]["title"];
                        //string cotegory_id = (string)rss["available_filters"][ipos]["values"][0]["id"];
                        //string cotegory_name = (string)rss["available_filters"][ipos]["values"][0]["name"];


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

    //codigo descartado



    //iCantMax = 110;
    //for (int i = 50; i <= iCantMax - 1; i = (i + 50))
    //{
    //    //obtengo en string el cuerpo de la respuesta
    //   // using var stringTask2 = client.GetStringAsync(sMeliApi + sSite_id + "/search?seller_id=" + sSeller_id + "&logistic_type=fulfillment&offset =" + i.ToString());

    //    JObject rss = JObject.Parse(stringTask);
    //    var msg = await stringTask2;
    //    Console.Write(msg);

    //    if (i + 50 >= iCantMax)
    //    {
    //        iCantMax = i + 51;
    //    }
    //}


    //var stringTask = client.GetStringAsync("https://api.mercadolibre.com/sites/MLA/search?seller_id=326659539"); // type=scan");
    //var stringTask = client.GetStringAsync("https://api.mercadolibre.com/sites/MLB/search?seller_id=244066576&logistic_type=fulfillment");

    //switch (request.StatusCode) //segun el estado podre realizar distintas acciones
    //{
    //    case System.Net.HttpStatusCode.Continue:
    //        break;
    //    case System.Net.HttpStatusCode.SwitchingProtocols:
    //        break;
    //    case System.Net.HttpStatusCode.Processing:
    //        break;
    //    case System.Net.HttpStatusCode.EarlyHints:
    //        break;
    //    case System.Net.HttpStatusCode.OK: //la Api respondio OK
    //        iCantMax = 110;
    //        for (int i = 50; i <= iCantMax - 1; i = (i + 50))
    //        {
    //            //obtengo en string el cuerpo de la respuesta
    //            using var stringTask2 = client.GetStringAsync(sMeliApi + sSite_id + "/search?seller_id=" + sSeller_id + "&logistic_type=fulfillment&offset =" + i.ToString());

    //            JObject rss = JObject.Parse(stringTask);
    //            var msg = await stringTask2;
    //            Console.Write(msg);

    //            if (i + 50 >= iCantMax)
    //            {
    //                iCantMax = i + 51;
    //            }
    //        }
    //        break;
    //    case System.Net.HttpStatusCode.Created:
    //        break;
    //    case System.Net.HttpStatusCode.Accepted:
    //        break;
    //    case System.Net.HttpStatusCode.NonAuthoritativeInformation:
    //        break;
    //    case System.Net.HttpStatusCode.NoContent:
    //        break;
    //    case System.Net.HttpStatusCode.ResetContent:
    //        break;
    //    case System.Net.HttpStatusCode.PartialContent:
    //        break;
    //    case System.Net.HttpStatusCode.MultiStatus:
    //        break;
    //    case System.Net.HttpStatusCode.AlreadyReported:
    //        break;
    //    case System.Net.HttpStatusCode.IMUsed:
    //        break;
    //    case System.Net.HttpStatusCode.Ambiguous:
    //        break;
    //    case System.Net.HttpStatusCode.Moved:
    //        break;
    //    case System.Net.HttpStatusCode.Found:
    //        break;
    //    case System.Net.HttpStatusCode.RedirectMethod:
    //        break;
    //    case System.Net.HttpStatusCode.NotModified:
    //        break;
    //    case System.Net.HttpStatusCode.UseProxy:
    //        break;
    //    case System.Net.HttpStatusCode.Unused:
    //        break;
    //    case System.Net.HttpStatusCode.RedirectKeepVerb:
    //        break;
    //    case System.Net.HttpStatusCode.PermanentRedirect:
    //        break;
    //    case System.Net.HttpStatusCode.BadRequest:
    //        break;
    //    case System.Net.HttpStatusCode.Unauthorized:
    //        break;
    //    case System.Net.HttpStatusCode.PaymentRequired:
    //        break;
    //    case System.Net.HttpStatusCode.Forbidden:
    //        break;
    //    case System.Net.HttpStatusCode.NotFound: // en caso de encontrar nada, puedo hacer una observación
    //        Console.WriteLine("Recurso no encontrado");
    //        break;
    //    case System.Net.HttpStatusCode.MethodNotAllowed:
    //        break;
    //    case System.Net.HttpStatusCode.NotAcceptable:
    //        break;
    //    case System.Net.HttpStatusCode.ProxyAuthenticationRequired:
    //        break;
    //    case System.Net.HttpStatusCode.RequestTimeout:
    //        break;
    //    case System.Net.HttpStatusCode.Conflict:
    //        break;
    //    case System.Net.HttpStatusCode.Gone:
    //        break;
    //    case System.Net.HttpStatusCode.LengthRequired:
    //        break;
    //    case System.Net.HttpStatusCode.PreconditionFailed:
    //        break;
    //    case System.Net.HttpStatusCode.RequestEntityTooLarge:
    //        break;
    //    case System.Net.HttpStatusCode.RequestUriTooLong:
    //        break;
    //    case System.Net.HttpStatusCode.UnsupportedMediaType:
    //        break;
    //    case System.Net.HttpStatusCode.RequestedRangeNotSatisfiable:
    //        break;
    //    case System.Net.HttpStatusCode.ExpectationFailed:
    //        break;
    //    case System.Net.HttpStatusCode.MisdirectedRequest:
    //        break;
    //    case System.Net.HttpStatusCode.UnprocessableEntity:
    //        break;
    //    case System.Net.HttpStatusCode.Locked:
    //        break;
    //    case System.Net.HttpStatusCode.FailedDependency:
    //        break;
    //    case System.Net.HttpStatusCode.UpgradeRequired:
    //        break;
    //    case System.Net.HttpStatusCode.PreconditionRequired:
    //        break;
    //    case System.Net.HttpStatusCode.TooManyRequests:
    //        break;
    //    case System.Net.HttpStatusCode.RequestHeaderFieldsTooLarge:
    //        break;
    //    case System.Net.HttpStatusCode.UnavailableForLegalReasons:
    //        break;
    //    case System.Net.HttpStatusCode.InternalServerError:
    //        break;
    //    case System.Net.HttpStatusCode.NotImplemented:
    //        break;
    //    case System.Net.HttpStatusCode.BadGateway:
    //        break;
    //    case System.Net.HttpStatusCode.ServiceUnavailable:
    //        break;
    //    case System.Net.HttpStatusCode.GatewayTimeout:
    //        break;
    //    case System.Net.HttpStatusCode.HttpVersionNotSupported:
    //        break;
    //    case System.Net.HttpStatusCode.VariantAlsoNegotiates:
    //        break;
    //    case System.Net.HttpStatusCode.InsufficientStorage:
    //        break;
    //    case System.Net.HttpStatusCode.LoopDetected:
    //        break;
    //    case System.Net.HttpStatusCode.NotExtended:
    //        break;
    //    case System.Net.HttpStatusCode.NetworkAuthenticationRequired:
    //        break;
    //    default:
    //        break;
    //}


    //if (stringTask.IsSuccessStatusCode)
    //{
    //    for (int i = 50; i <= iCantMax - 1; i = (i + 50))
    //    {
    //        //obtengo en string el cuerpo de la respuesta
    //        var stringTask2 = client.GetStringAsync(sMeliApi + sSite_id + "/search?seller_id=" + sSeller_id + "&logistic_type=fulfillment&offset =" + i.ToString());
    //        var msg = await stringTask2;
    //        Console.Write(msg);

    //        if (i + 50 >= iCantMax)
    //        {
    //            iCantMax = i + 51;
    //        }
    //    }




    //  var RespuestaString = await client.GetStringAsync(sMeliApi + sSite_id + "/search?seller_id=" + sSeller_id + "&logistic_type=fulfillment&offset =" + iCant.ToString());
    // var RespuestaString2 = await stringTask.Content.ReadAsStringAsync();
    //lo cambio a formato lista, objeto lista, descerealizo
    //var RespuestaJSON = JsonSerializer.Deserialize<List<clvendedor>>(RespuestaString, new JsonSerializerOptions() {PropertyNameCaseInsensitive = true });
    //Console.Write("todo ok");
    //}
    //else
    //{
    //   Console.Write ("Error");
    //}


    //      foreach (var item in postId)
    //                    {
    //                        string title_item = (string)rss["results"][ipos]["title"];
    //    string cotegory_id = (string)rss["available_filters"][ipos]["values"][0]["id"];
    //    string cotegory_name = (string)rss["available_filters"][ipos]["values"][0]["id"];
    //    iposCat = 0;
    //                        var cotegory_id =
    //                               from Pid in rss["available_filters"][ipos]["values"]
    //                               select (string)Pid["id"];


    //                        foreach (var item1 in cotegory_id)
    //                        {
    //                            // string catId = (string)rss["available_filters"][ipos]["values"][iposCat]["id"];
    //                            // string catName = (string)rss["available_filters"][ipos]["values"][iposCat]["name"];
    //                            //Console.WriteLine(item1);
    //                            Console.WriteLine(site_id + "," + seller_id + "," + nickname + "," + item + "," + title_item + "," + item1); // + "," + catName);
    //                          //  iposCat++;
    //                        }


    //ipos++;

}
