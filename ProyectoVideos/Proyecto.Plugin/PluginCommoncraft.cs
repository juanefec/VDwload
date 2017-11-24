using System;
using System.Collections.Generic;
using Proyecto.Common;
using System.Net.Http;
using HtmlAgilityPack;


namespace Proyecto.Plugin
{
    public class PluginCommoncraft : IPlugin
    {
        string base_url = "https://www.commoncraft.com";
        public byte[] Bajar(Video video)
        {
            var client = new HttpClient();
            var url = base_url + video.Link;
            var web = new HtmlWeb();
            var htmlVideo = web.Load(url);
            var iframeSource = htmlVideo.DocumentNode.SelectSingleNode($"//iframe[contains(@src,'//')]").Attributes["src"].Value;
            if (!iframeSource.Contains("https:"))
            {
                iframeSource = "https:" + iframeSource;
            }
            var htmlVideoSource = web.Load(iframeSource);
            var htmlScript = htmlVideoSource.DocumentNode.SelectSingleNode("//body").InnerHtml;
            int start = htmlScript.IndexOf("https://embed");
            int end = htmlScript.IndexOf('"', start);
            string videoUrl = htmlScript.Substring(start, end - start);            
            var archivo = Utility.downloadProgress(videoUrl);
            return archivo;
        }

        public List<Video> ListarVideosDisponibles(string urlListado)
        {
            var html = base_url + "/video-library";
            HtmlWeb web = new HtmlWeb();
            var htmlDoc = web.Load(html);
            var nodeColl = htmlDoc.DocumentNode.SelectNodes($"//a[starts-with(@href,'/video/')]");
            var list = new List<Video>();
            int j = 0;
            foreach (HtmlNode node in nodeColl)
            {
                if (node.InnerText != "")
                {
                    var aTag = node.InnerText;
                    var link = node.GetAttributeValue("href", string.Empty);
                    var vid = new Video();
                    vid.Titulo = aTag;
                    vid.Link = link;
                    list.Add(vid);
                    System.Console.WriteLine(j + ")-->  " + vid.Titulo);
                    j++;
                }
            }
            return list;
        }
    }
}
