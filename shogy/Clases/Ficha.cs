using System;
using System.Collections.Generic;
using System.Text;

namespace shogy.Clases
{
    class Ficha
    {
        public string Posicion { get; set; }
        public string Nombre { get; set; }

        public string Dibujo { get; set; } // Cv  C^

        public Jugador Duenio { get; set; }

        public Ficha(string posicion, string nombre, string dibujo ) {
            Posicion = posicion;
            Nombre = nombre;
            Dibujo = dibujo;
        }


    }
}
