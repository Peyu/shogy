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
        public string PosicionActual;
        public List<string> PosiblesAtaques { get; set; } = new List<string>() ;
        public Ficha( string dibujo, Jugador duenio, string posicion ) {
            Dibujo = dibujo;
            DibujoOriginal = dibujo.Remove(dibujo.Length - 1);
            Duenio = duenio;
            PosicionActual = posicion;
        }
        public int getFila() {
            char[] posicion = PosicionActual.ToCharArray();
            int fila = int.Parse(PosicionActual[0].ToString());
            return fila;
        }
        public int getColumna() {
            char[] posicion = PosicionActual.ToCharArray();
            int columna = int.Parse(PosicionActual[1].ToString());
            return columna;
        }



    }
}
