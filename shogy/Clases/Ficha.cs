using System;
using System.Collections.Generic;
using System.Text;

namespace shogy.Clases
{
    class Ficha
    {
        public string Dibujo { get; set; } // Cv  C^
        public string DibujoOriginal { get; set; }
        public Jugador Duenio { get; set; }
        public Ficha( string dibujo, Jugador duenio ) {
            Dibujo = dibujo;
            DibujoOriginal = dibujo.Remove(dibujo.Length - 1);
            Duenio = duenio;
        }
    }
}
