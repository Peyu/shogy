using System;
using System.Collections.Generic;
using System.Text;

namespace shogy.Clases
{
    class Tablero
    {
        //public List<Ficha> Lugares = new List<Ficha>();
        public Object[,] Lugares = new Object[8, 8];

        public Tablero() {
            var Lancero1 = new Ficha("00", "Lancero", "Lv");
            Lugares[0, 0] = Lancero1;
            var Caballo1 = new Ficha("00", "Caballo", "Cv");
            Lugares[0, 1] = Caballo1;



            var Rey = new Ficha("00", "Rey", "Rv" );
            Lugares[0, 0] = Rey;
            var Torre = new Ficha("01", "Torre", "Tv");
            Lugares[0, 1] = Torre;
        }

        public void Dibujar() {

            for (int filas = 0; filas < 8; filas++)
            {
                string linea = "";
                for (int columnas = 0; columnas < 8; columnas++)
                {
                    var f = (Ficha)Lugares[filas, columnas];
                    linea += f.Dibujo;
                }
                Console.WriteLine(linea);
            }
        }




        //public Jugador j1 { get; set; }
        //public Jugador j2 {get; set;}

    }




}


//Object[] ArrayOfObjects = new Object[] {1,"3"}
//int[][] jagged_arr = new int[4][]
//jagged_arr[0] = new int[2];