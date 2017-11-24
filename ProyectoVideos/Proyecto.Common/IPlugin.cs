using System;
using System.Collections.Generic;

namespace Proyecto.Common
{
    public interface IPlugin
    {
        List<Video> ListarVideosDisponibles(string urlListado);
        byte[] Bajar(Video video);
    }
}