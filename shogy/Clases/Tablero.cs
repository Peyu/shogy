using System;
using System.Collections.Generic;
using System.Text;

namespace shogy.Clases
{
    class Tablero
    {
        //public List<Ficha> Lugares = new List<Ficha>();
        public Ficha[,] Lugares = new Ficha[9, 9];
        public Jugador J1;
        public Jugador J2;
        public Jugador Turno;
        

        public Tablero(Jugador j1, Jugador j2) {
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

            J1 = j1;
            J2 = j2;

            Turno = J1;

        }

        public void Dibujar() {

            Console.WriteLine("    0  1  2  3  4  5  6  7  8 ");
            Console.WriteLine(" ");
            for (int filas = 0; filas < 9; filas++)
            {
                string linea = "";
                for (int columnas = 0; columnas < 9; columnas++)
                {
                    var f = (Ficha)Lugares[filas, columnas];

                    linea += f?.Dibujo == null? "   " : " " + f.Dibujo ;
                }
                Console.WriteLine(filas + "  " + linea);
                Console.WriteLine(" ");
            }
        }

        public void Mover(string origen, string destino) {
            char[] desde = origen.ToCharArray();
            int filaOrigen = int.Parse(desde[0].ToString()); 
            int columnaOrigen = int.Parse(desde[1].ToString());

            char[] hasta = destino.ToCharArray();
            int filaDestino = int.Parse(hasta[0].ToString());
            int columnaDestino = int.Parse(hasta[1].ToString());

            Ficha FichaEnMovimiento = Lugares[filaOrigen, columnaOrigen];
            
            //Agregar logica para comprbar reglas de movimiento
            //vacio el lugar
            Lugares[filaOrigen, columnaOrigen] = null;
            //coloc ficha en nueva posicion
            Lugares[filaDestino, columnaDestino] = FichaEnMovimiento;

            //actualizo tablero
            Console.Clear();
            Dibujar();
        }

        public Jugador getTurno() {
            if (Turno == J1)
            {
                Turno = J2;
                return J1;
            }
            else {
                Turno = J1;
                return J2;
            }
        }

    }

}
