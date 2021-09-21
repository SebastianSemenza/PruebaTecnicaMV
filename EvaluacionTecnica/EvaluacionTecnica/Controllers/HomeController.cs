using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AccesoDatos;
using Modelo;
using Newtonsoft.Json;

namespace EvaluacionTecnica.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index(List<RegistroClima> lista=null)
        {
            lista = lista == null ? new List<RegistroClima>() : lista;

            //cargar combo ciudades
            List<Ciudad> listaCiudades = new List<Ciudad>();
            listaCiudades = HistorialClimas.getInstance().listarCiudades();
            ViewBag.listaC = listaCiudades;

            return View(lista);
        }

        [HttpPost]
        public ActionResult Index(FormCollection collection)
        {
            List<RegistroClima> lista = null;

            //parametros obtenidos del collection
            int idCiudad = Convert.ToInt32(collection["Ciudad"]);
            bool checkHistorico = Convert.ToBoolean(collection["checkHistorico"].Split(',')[0]);

            if (idCiudad != -1)
            {
                //busco parametro para concatenarlo en la URL de la API
                string[] arr = HistorialClimas.getInstance().buscarDatosCiudad(idCiudad);
                string parametro = arr[0];
                string NomCiudad = arr[1];

                //Llamada a la api
                string url = "https://api.openweathermap.org/data/2.5/weather?q=" + parametro + "&appid=27e02c167ef115739e259ba848a3216d&units=metric";
                string respuesta = GetHttp(url);
                dynamic jsonObj = JsonConvert.DeserializeObject(respuesta);

                //cargo variables con la respuesta de la api
                string temperatura = jsonObj["main"]["temp"].ToString();
                string termica = jsonObj["main"]["feels_like"].ToString();

                //Viewbag para mostrar info en el view
                ViewBag.temp = temperatura + "°";
                ViewBag.term = termica + "°";
                ViewBag.nomCiudad = NomCiudad;

                RegistroClima reg = new RegistroClima();
                reg.ciudad = new Ciudad();
                reg.ciudad.id = idCiudad;
                reg.temperatura = termica;
                reg.termica = termica;

                //Guardo Registro Clima
                HistorialClimas.getInstance().agregarClima(reg);

                //Busco historial
                if (checkHistorico == true)
                {
                    try
                    {
                        lista = HistorialClimas.getInstance().mostrarHistorial(idCiudad);
                    }
                    catch (Exception ex)
                    {
                        lista = null;
                        throw ex;
                    }
                }
            }

            return Index(lista);
        }

        //request
        public string GetHttp (string url)
        {
            WebRequest _request = WebRequest.Create(url);
            WebResponse _Response = _request.GetResponse();
            StreamReader sr = new StreamReader(_Response.GetResponseStream());
            return sr.ReadToEnd().Trim();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}