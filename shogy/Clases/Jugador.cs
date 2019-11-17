using System;
using System.Collections.Generic;
using System.Text;

namespace shogy.Clases
{
    class Jugador
    {
        public string Nombre { get; set; }
        public List<Ficha> EnMano { get; set; } = new List<Ficha>();

        public Jugador(string nombre) {
            Nombre = nombre;
            
        }

    }
}
