using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using teste_webMotorsWeb.Models;

namespace teste_webMotorsWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync("http://localhost:63631/api/Anuncios/listar/1/123").Result;

                response.EnsureSuccessStatusCode();

                string conteudo = response.Content.ReadAsStringAsync().Result;

                List<AnuncioVM> resultado = JsonConvert.DeserializeObject<List<AnuncioVM>>(conteudo);

                return View(resultado);
            }
        }
        [HttpGet]
        public IActionResult Postar(string idMarca = "0", string idModelo = "0")
        {

            DropMarca();
            return View();
        }

        [HttpPost]
        public IActionResult Postar(AnuncioVM anuncio)
        {
            try
            {
                var marca = anuncio.Marca.Split(" ").ToList();
                marca.RemoveAt(0);
                anuncio.Marca = marca.Aggregate((a, b) => a + b);
                var modelo = anuncio.Modelo.Split(" ").ToList();
                modelo.RemoveAt(0);
                anuncio.Modelo = modelo.Aggregate((a, b) => a + b);
                var versao = anuncio.Versao.Split(" ").ToList();
                versao.RemoveAt(0);
                anuncio.Versao = versao.Aggregate((a, b) => a  +" "+ b);



                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpContent content = new StringContent(JsonConvert.SerializeObject(anuncio), Encoding.UTF8, "application/json");

                    HttpResponseMessage response = client.PostAsync("http://localhost:63631/api/Anuncios/", content).Result;

                    response.EnsureSuccessStatusCode();

                    string idAnuncioCriado = response.Content.ReadAsStringAsync().Result;

                    return RedirectToAction("Detalhes", "Home", new { id = idAnuncioCriado });
                }
            }
            catch (Exception ex)
            {
                ViewBag.Erro = ex.Message;
                return View();
            }
        }
        [HttpGet]
        public IActionResult Remover(int id)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.DeleteAsync("http://localhost:63631/api/Anuncios/" + id).Result;

                return RedirectToAction("Index");
            }
        }
        [HttpGet]
        public IActionResult Detalhes(string id)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync("http://localhost:63631/api/Anuncios/" + id).Result;

                response.EnsureSuccessStatusCode();

                string conteudo = response.Content.ReadAsStringAsync().Result;

                AnuncioVM resultado = JsonConvert.DeserializeObject<AnuncioVM>(conteudo);

                if (resultado == null)
                    return RedirectToAction("Index");

                return View(resultado);
            }
        }
        [HttpGet]
        public IActionResult Editar(int id)
        {
            DropMarca();

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync("http://localhost:63631/api/Anuncios/" + id).Result;

                response.EnsureSuccessStatusCode();

                string conteudo = response.Content.ReadAsStringAsync().Result;

                AnuncioVM resultado = JsonConvert.DeserializeObject<AnuncioVM>(conteudo);

                return View(resultado);
            }
        }
        [HttpPost]
        public IActionResult Editar(AnuncioVM anuncio)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpContent content = new StringContent(JsonConvert.SerializeObject(anuncio), Encoding.UTF8, "application/json");

                    HttpResponseMessage response = client.PutAsync("http://localhost:63631/api/Anuncios/", content).Result;

                    response.EnsureSuccessStatusCode();

                    string conteudo = response.Content.ReadAsStringAsync().Result;

                    return RedirectToAction("Detalhes", "Home", new { id = conteudo });
                }
            }
            catch (Exception ex)
            {
                ViewBag.Erro = ex.Message;
                return View();
            }
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public void DropMarca()
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync("http://desafioonline.webmotors.com.br/api/OnlineChallenge/Make").Result;

                response.EnsureSuccessStatusCode();

                string conteudo = response.Content.ReadAsStringAsync().Result;

                List<MarcasVM> resultado = JsonConvert.DeserializeObject<List<MarcasVM>>(conteudo);

                resultado.Add(new MarcasVM() { ID = "0", Name = "Selecione" });

                resultado = resultado.OrderBy(x => x.ID).ToList();


                resultado = resultado.Select(x => new MarcasVM { ID = x.ID + " " + x.Name, Name = x.Name }).ToList();


                ViewBag.Marcas = new SelectList(resultado, "ID", "Name");
            }
        }

        public JsonResult DropModelo(string id)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                id = String.Join("", System.Text.RegularExpressions.Regex.Split(id, @"[^\d]"));

                HttpResponseMessage response = client.GetAsync("http://desafioonline.webmotors.com.br/api/OnlineChallenge/Model?MakeID=" + id).Result;

                response.EnsureSuccessStatusCode();

                string conteudo = response.Content.ReadAsStringAsync().Result;

                List<ModelosVM> resultado = JsonConvert.DeserializeObject<List<ModelosVM>>(conteudo);

                resultado = resultado.Select(x => new ModelosVM { ID = x.ID + " " + x.Name, Name = x.Name }).ToList();


                return Json(new SelectList(resultado, "ID", "Name"));
            }
        }

        public JsonResult DropVersao(string id = "")
        {
            using (var client = new HttpClient())
            {

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                id = String.Join("", System.Text.RegularExpressions.Regex.Split(id, @"[^\d]"));


                HttpResponseMessage response = client.GetAsync("http://desafioonline.webmotors.com.br/api/OnlineChallenge/Version?ModelID=" + id).Result;

                response.EnsureSuccessStatusCode();

                string conteudo = response.Content.ReadAsStringAsync().Result;

                List<VersaoVM> resultado = JsonConvert.DeserializeObject<List<VersaoVM>>(conteudo);

                resultado = resultado.Select(x => new VersaoVM { ID = x.ID + " " + x.Name, Name = x.Name }).ToList();

                return Json(new SelectList(resultado, "ID", "Name"));
            }
        }
    }
}
