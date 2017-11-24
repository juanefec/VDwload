using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;


namespace Proyecto.Common
{
    public class Utility
    {
              
        public static byte[] downloadProgress(string url)
        {
            using ( var client = new HttpClient())
            {
                var httpResp = client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
                var totalFile = httpResp.Content.Headers.ContentLength;
                var asyncStream = httpResp.Content.ReadAsStreamAsync().Result;

                var moreToRead = true;
                var bytes = new byte[0];
                var buffer = new byte[8192];
                var progress = new Boolean[100];

                while (moreToRead)
                {
                    var dataRead = asyncStream.ReadAsync(buffer, 0, buffer.Length).Result;

                    if (dataRead < buffer.Length)
                    {
                        Array.Resize(ref buffer, dataRead);
                        bytes = bytes.Concat(buffer).ToArray();
                        buffer = new byte[8192];
                    }
                    else
                    {
                        var currentBytes = (long)bytes.Length;
                        bytes = bytes.Concat(buffer).ToArray();
                        var calcPorciento = (currentBytes * 100) / totalFile;
                        int porciento = Math.Abs((int)calcPorciento);
                        if (!progress[porciento])
                        {
                            progress[porciento] = true;
                            System.Console.WriteLine("Descargando: " + porciento + "%  ");
                        }
                    }
                    if (bytes.Length == totalFile)
                    {
                        moreToRead = false;
                    }
                }
                return bytes;
            }
        }


    }




}