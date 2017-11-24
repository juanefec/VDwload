using System;
using System.Collections.Generic;
using System.IO;
using Proyecto.Common;
using Proyecto.Plugin;

namespace Proyecto.Consola
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Se requiere un unico parametro 'baseUrl'");
                return;
            }
            string url = args[0];
            IPlugin plugin;
            if (url.Contains("gogoanime"))
            {
                plugin = new PluginGogoanime();
            }
            else if (url.Contains("commoncraft"))
            {
                plugin = new PluginCommoncraft();
            }
            else
            {
                throw new NotSupportedException("URL no soportada");
            }


            System.Console.WriteLine("A countinuacion se listaran las opciones:");

            List<Video> listaVideos = plugin.ListarVideosDisponibles(url);



            if (listaVideos.Count > 0)
            {
                System.Console.WriteLine("Escriba el numero del video que quiere descargar:");
                var input = Console.ReadLine();
                int numVid;
                bool esInt = int.TryParse(input, out numVid);

                if (esInt && numVid < listaVideos.Count && numVid > 0)
                {
                    var _nVid = numVid;
                    var filename = listaVideos[_nVid].Titulo.Replace(" ", "-");
                    System.Console.WriteLine("El video elegido fue: " + listaVideos[_nVid].Titulo);
                    byte[] contenido = plugin.Bajar(listaVideos[_nVid]);
                    File.WriteAllBytes(path: $"descargas/{filename}.mp4", bytes: contenido);
                    Console.WriteLine($"File {filename} saved");
                    Console.WriteLine("Presione enter para salir.");
                    Console.ReadLine(); 
                }
                else
                {
                    System.Console.WriteLine("Input invalido");
                }
            }
        }
    }
}
