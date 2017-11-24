using System;
using System.Collections.Generic;
using Proyecto.Common;
using System.Net.Http;
using HtmlAgilityPack;

namespace Proyecto.Plugin
{
    public class PluginGogoanime : IPlugin
    {
        string base_url = "https://ww3.gogoanime.io";
        HtmlWeb web = new HtmlWeb();
        public List<Video> ListarVideosDisponibles(string urlListado)
        {

            var listCap = new List<Video>();
            var listAnime = new List<Video>();
            int j = 0;
            for (var i = 0; i < 7; i++)
            {
                var html = $"{base_url}/anime-list.html?page={i}";
                var htmlDoc = web.Load(html);
                string ToFind = "/category/";
                var nodeColl = htmlDoc.DocumentNode.SelectNodes($"//a[starts-with(@href,'{ToFind}')]");

                foreach (HtmlNode node in nodeColl)
                {
                    var anime = new Video();
                    anime.Link = node.GetAttributeValue("href", string.Empty); ;
                    anime.Titulo = j + ".  " + node.InnerText;
                    listAnime.Add(anime);
                    j++;
                    System.Console.WriteLine(anime.Titulo);
                }
            }

            Console.WriteLine("Seleccione la serie de la cual desea descargar e introduzca el numero:");
            var input = Console.ReadLine();
            int numVid;
            bool esInt = int.TryParse(input, out numVid);
            int serieElegida = numVid
            ;
            if (esInt && serieElegida < listAnime.Count && serieElegida >= 0)
            {
                System.Console.WriteLine("La serie elegida fue: " + listAnime[serieElegida].Titulo);
            }
            else
            {
                System.Console.WriteLine("Input invalido");
                
                
            }

            var urlAnime = base_url + listAnime[serieElegida].Link;
            var htmlAnime = web.Load(urlAnime);
            var caps = "movie_id";
            var movieID = htmlAnime.DocumentNode.SelectSingleNode($"//input[contains(@id,'{caps}')]").Attributes["value"].Value;
            var linkVideos = base_url + "/load-list-episode?ep_start=0&ep_end=10000&id=" + movieID;
            System.Console.WriteLine(linkVideos);
            var htmlListCap = web.Load(linkVideos);
            var classKey = " /";
            var capColl = htmlListCap.DocumentNode.SelectNodes($"//a[starts-with(@href, '{classKey}')]");
            j = 0;
            foreach (HtmlNode n in capColl)
            {
                var vid = new Video();
                var textohtml = n.OuterHtml;
                var elementSynt = "<div class=\"name\">";
                int start = textohtml.IndexOf(elementSynt) + elementSynt.Length;
                int end = textohtml.IndexOf("</div>", start);
                vid.Titulo = textohtml.Substring(start, end - start);
                vid.Link = n.GetAttributeValue("href", string.Empty);
                listCap.Add(vid);
                System.Console.WriteLine(j + ")--> " + vid.Titulo + "  " + vid.Link);
                j++;
            }
            j = 0;
            return listCap;

        }
        public byte[] Bajar(Video video)
        {
            var client = new HttpClient();
            var urlVideo = base_url + video.Link.Replace(" ", "");
            var htmlVideo = web.Load(urlVideo);
            var tipo = "//";

            var videoSource = htmlVideo.DocumentNode.SelectSingleNode(xpath: $"//iframe[contains(@src, '{tipo}')]").Attributes["src"].Value;
            var htmlVideoSource = web.Load("https:" + videoSource);
            var urlSrc = htmlVideoSource.DocumentNode.SelectSingleNode($"//source[contains(@label,'360')]").Attributes["src"].Value;
            byte[] archivo = Utility.downloadProgress(urlSrc);
            return archivo;
        }

    }
}
