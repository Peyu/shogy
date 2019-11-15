using System;
using System.Collections.Generic;
using System.Text;

namespace shogy.Clases
{
    class Tablero
    {
        //public List<Ficha> Lugares = new List<Ficha>();
        public Object[,] Lugares = new Object[9, 9];

        public Tablero() {
            Lugares[0,0] = new Ficha("Lv");     //Lancero 
            Lugares[0,1] = new Ficha("Cv");     //Caballo
            Lugares[0,2] = new Ficha("Pv");     //Plata
            Lugares[0,3] = new Ficha("Ov");     //Oro
            Lugares[0,4] = new Ficha("Rv");     //Rey
            Lugares[0,5] = new Ficha("Ov");     
            Lugares[0, 6] = new Ficha("Pv");
            Lugares[0, 7] = new Ficha("Cv");
            Lugares[0, 8] = new Ficha( "Lv");
            Lugares[1,1] = new Ficha("Tv");     //Torre
            Lugares[1, 7] = new Ficha("Av");    //Alfil
            //fila de peones
            for (int columna = 0; columna < 9; columna++)
            {
                Lugares[2, columna] = new Ficha("pv");   //peon
            }
            //filas peones
            for (int columna = 0; columna < 9; columna++)
            {
                Lugares[6, columna] = new Ficha("p^");   //peon
            }
            Lugares[7, 1] = new Ficha("A^");    
            Lugares[7, 7] = new Ficha("T^");    
            Lugares[8, 0] = new Ficha("L^");     
            Lugares[8, 1] = new Ficha("C^");    
            Lugares[8, 2] = new Ficha("P^");    
            Lugares[8, 3] = new Ficha("O^");    
            Lugares[8, 4] = new Ficha("R^");    
            Lugares[8, 5] = new Ficha("O^");
            Lugares[8, 6] = new Ficha("P^");
            Lugares[8, 7] = new Ficha("C^");
            Lugares[8, 8] = new Ficha("L^");

        }

        public void Dibujar() {
            Console.WriteLine("0 1 2 3 4 5 6 7 8 ");
            for (int filas = 0; filas < 9; filas++)
            {
                string linea = "";
                for (int columnas = 0; columnas < 9; columnas++)
                {
                    var f = (Ficha)Lugares[filas, columnas];

                    linea += f?.Dibujo == null? "  " : f.Dibujo;
                }
                Console.WriteLine(linea + " " + filas);
            }
        }

        public void Mover(string origen, string destino) {
            char[] desde = origen.ToCharArray();

            var pp = 0;
        }

    }

}
