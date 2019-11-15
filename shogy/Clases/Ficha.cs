using System;
using System.Collections.Generic;
using System.Text;

namespace shogy.Clases
{
    class Ficha
    {
       
        public string Dibujo { get; set; } // Cv  C^
        public Jugador Duenio { get; set; }
        public Ficha( string dibujo ) {
            Dibujo = dibujo;
        }


    }
}
