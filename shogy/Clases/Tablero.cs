using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

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
            J1 = j1;
            J2 = j2;

            Lugares[0,0] = new Ficha("Lv", J2);     //Lancero 
            Lugares[0,1] = new Ficha("Cv", J2);     //Caballo
            Lugares[0,2] = new Ficha("Pv", J2);     //Plata
            Lugares[0,3] = new Ficha("Ov", J2);     //Oro
            Lugares[0,4] = new Ficha("Rv", J2);     //Rey
            Lugares[0,5] = new Ficha("Ov", J2);     
            Lugares[0, 6] = new Ficha("Pv", J2);
            Lugares[0, 7] = new Ficha("Cv", J2);
            Lugares[0, 8] = new Ficha( "Lv", J2);
            Lugares[1,1] = new Ficha("Tv", J2);     //Torre
            Lugares[1, 7] = new Ficha("Av", J2);    //Alfil
            //fila de peones
            for (int columna = 0; columna < 9; columna++)
            {
                Lugares[2, columna] = new Ficha("pv", J2);   //peon
            }
            //filas peones
            for (int columna = 0; columna < 9; columna++)
            {
                Lugares[6, columna] = new Ficha("p^", J1);   //peon
            }
            Lugares[7, 1] = new Ficha("A^", J1);    
            Lugares[7, 7] = new Ficha("T^", J1);    
            Lugares[8, 0] = new Ficha("L^", J1);     
            Lugares[8, 1] = new Ficha("C^", J1);    
            Lugares[8, 2] = new Ficha("P^", J1);    
            Lugares[8, 3] = new Ficha("O^", J1);    
            Lugares[8, 4] = new Ficha("R^", J1);    
            Lugares[8, 5] = new Ficha("O^", J1);
            Lugares[8, 6] = new Ficha("P^", J1);
            Lugares[8, 7] = new Ficha("C^", J1);
            Lugares[8, 8] = new Ficha("L^", J1);

            
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

            //agregar regex para verificar formato de entrada y verificar existencia
            string pattern = @"\d{2}";

            if (!(Regex.IsMatch(origen, pattern) && Regex.IsMatch(destino, pattern)))
            {
                Mensaje("Error en coordenadas intentelo denuevo");
            }
            else
            {

                char[] desde = origen.ToCharArray();
                int filaOrigen = int.Parse(desde[0].ToString());
                int columnaOrigen = int.Parse(desde[1].ToString());

                char[] hasta = destino.ToCharArray();
                int filaDestino = int.Parse(hasta[0].ToString());
                int columnaDestino = int.Parse(hasta[1].ToString());

                Ficha FichaEnMovimiento = null;
                try
                {
                    FichaEnMovimiento = Lugares[filaOrigen, columnaOrigen];
                }
                catch (IndexOutOfRangeException e)
                {
                    Mensaje("No se puede mover una fica fuera del casilero, intentenlo nuevamente");
                }

                if (FichaEnMovimiento == null)
                {
                    Mensaje("No hay ninguna ficha en ese casillero");
                }

                else if (FichaEnMovimiento.Duenio.Nombre == Turno.Nombre)
                {

                    if (MovimientoEsValido(filaOrigen, columnaOrigen, filaDestino, columnaDestino, FichaEnMovimiento))
                    {

                        //reviso si hay una ficha enemiga en el lugar de destino y se come
                        if (Lugares[filaDestino, columnaDestino] == null)
                        {
                            //vacio el lugar
                            Lugares[filaOrigen, columnaOrigen] = null;
                            //coloco ficha en nueva coordenada
                            Lugares[filaDestino, columnaDestino] = FichaEnMovimiento;
                            ChequearCoronacion(filaOrigen, filaDestino, FichaEnMovimiento);
                            //actualizo tablero y turno
                            Turno = Turno.Nombre == J1.Nombre ? J2 : J1;
                        }
                        else
                        {
                            if (Lugares[filaDestino, columnaDestino].Duenio.Nombre != Turno.Nombre)
                            {
                                //vacio el lugar
                                Lugares[filaOrigen, columnaOrigen] = null;
                                //ficha pasa al otro jugador
                                Lugares[filaDestino, columnaDestino].Duenio = Turno;
                                Turno.EnMano.Add(Lugares[filaDestino, columnaDestino]);
                                //coloco ficha en nueva coordenada
                                Lugares[filaDestino, columnaDestino] = FichaEnMovimiento;
                                ChequearCoronacion(filaOrigen, filaDestino, FichaEnMovimiento);
                                //actualizo tablero y turno
                                Turno = Turno.Nombre == J1.Nombre ? J2 : J1;
                            }
                            else
                            {
                                Mensaje("No puedes comer tus propias fichas");
                            }
                        }

                        Console.Clear();
                        Dibujar();
                    }
                    else
                    {
                        Mensaje("Movimiento no permitido, intentelo denuevo");
                    }
                }
                else
                {
                    Mensaje("Solo puedes mover tus propias fichas, intentalo denuevo");
                }

                

            }
        }

        private void ChequearCoronacion(int filaOrigen,int filaDestino, Ficha ficha) {
            if (
                !(ficha.Dibujo.Contains("R") || 
                ficha.Dibujo.Contains("O") || 
                ficha.Dibujo.Contains("E") || 
                ficha.Dibujo.Contains("F"))
                )  //Rey Y Oros no se coronan, tampoco los ya coronados.
            {

                string coronar = "";
                if (Turno == J1 && (filaDestino < 3 || filaOrigen < 3))
                {
                    if (filaDestino != 0)
                    {
                        while (coronar != "S" && coronar != "N" && coronar != "s" && coronar != "n")
                        {
                            Console.Clear();
                            Console.WriteLine("Desea coronar la ficha " + ficha.Dibujo + "? (S/N)");
                            coronar = Console.ReadLine();
                            if (coronar != "S" && coronar != "N" && coronar != "s" && coronar != "n")
                            {
                                Console.WriteLine("Por favor responda con S o N, presione una tecla para continuar...");
                            }
                        }
                    }
                    else {
                        coronar = "S";
                    }
                }

                if (Turno == J2 && (filaDestino > 5 || filaOrigen > 5))
                {
                    if (filaDestino != 8)
                    {
                        while (coronar != "S" && coronar != "N" && coronar != "s" && coronar != "n")
                        {
                            Console.Clear();
                            Console.WriteLine("Desea coronar la ficha " + ficha.Dibujo + "? (S/N)");
                            coronar = Console.ReadLine();
                            if (coronar != "S" && coronar != "N" && coronar != "s" && coronar != "n")
                            {
                                Console.WriteLine("Por favor responda con S o N, presione una tecla para continuar...");
                            }
                        }
                    }
                    else {
                        coronar = "S";
                    }
                }

                if (coronar == "s" || coronar == "S")
                {
                    if (ficha.Dibujo.Contains("^"))
                    {
                        if (ficha.Dibujo.Contains("T"))
                            ficha.Dibujo = "E^";
                        else if (ficha.Dibujo.Contains("A"))
                            ficha.Dibujo = "F^";
                        else
                            ficha.Dibujo = "O^";
                    }
                    else {
                        if (ficha.Dibujo.Contains("T"))
                            ficha.Dibujo = "Ev";
                        else if (ficha.Dibujo.Contains("A"))
                            ficha.Dibujo = "Fv";
                        else
                            ficha.Dibujo = "Ov";
                    }

                }
            }
        }

        private void Mensaje(string msg) {
            Console.WriteLine(msg);
            Console.WriteLine("Presione una tecla para continuar");
            Console.ReadLine();
            //System.Threading.Thread.Sleep(3000);
            Console.Clear();
            Dibujar();
        }

        private bool MovimientoEsValido(int filaOrigen, int columnaOrigen, int filaDestino, int columnaDestino, Ficha ficha ) {

            bool EsValido = false;
            switch (ficha.Dibujo) {
                case "pv":
                    if (columnaOrigen == columnaDestino && filaDestino == (filaOrigen + 1))
                        EsValido = true;
                    break;
                case "p^":
                    if (columnaOrigen == columnaDestino && filaDestino == (filaOrigen - 1))
                        EsValido = true;
                    break;
                case "Rv":
                case "R^":
                    if (Math.Abs(filaOrigen - filaDestino) <= 1 && Math.Abs(columnaOrigen - columnaDestino) <= 1)
                        EsValido = true;
                    break;
                case "Tv":
                case "T^":
                    if (
                        ((columnaOrigen == columnaDestino) && (filaOrigen != filaDestino)) ||
                        ((filaOrigen == filaDestino) && (columnaOrigen != columnaDestino))
                      )
                       EsValido = true && RecorridoEsValido(filaOrigen,filaDestino,columnaOrigen,columnaDestino); 
                    break;
                case "Av":
                case "A^":
                    if (  //diagonales izq a der
                          (  (filaOrigen > filaDestino) && (columnaOrigen < columnaDestino) ||
                             (filaOrigen < filaDestino) && (columnaOrigen > columnaDestino)
                          ) &&
                            (filaOrigen + columnaOrigen) == (filaDestino + columnaDestino)
                            &&
                          (
                            (filaOrigen != filaDestino) &&
                            (columnaOrigen != columnaDestino)
                          )
                        )
                        EsValido = true && RecorridoEsValido(filaOrigen, filaDestino, columnaOrigen, columnaDestino);
                    else if( //diagonales der a izq
                        
                            (  ((filaOrigen < filaDestino) && (columnaOrigen < columnaDestino)) ||
                               ((filaOrigen > filaDestino)  && (columnaOrigen > columnaDestino) 
                            ) &&
                                (Math.Abs(filaOrigen - columnaOrigen)) == 
                                (Math.Abs(filaDestino - columnaDestino))                               
                            ) &&
                            (
                                (filaOrigen != filaDestino) &&
                                (columnaOrigen != columnaDestino)
                            )

                        )
                        EsValido = true && RecorridoEsValido(filaOrigen, filaDestino, columnaOrigen, columnaDestino);
                    break;
                case "O^":
                    if (
                        (Math.Abs(filaOrigen - filaDestino) <= 1) && (Math.Abs(columnaOrigen - columnaDestino) <= 1) &&
                        !((filaOrigen == (filaDestino - 1)) && (columnaOrigen != columnaDestino) && (filaOrigen < filaDestino))
                      )
                        EsValido = true;
                    break;
                case "Ov":
                    if (
                        (Math.Abs(filaOrigen - filaDestino) <= 1) && (Math.Abs(columnaOrigen - columnaDestino) <= 1) &&
                        !((filaOrigen == (filaDestino + 1)) && (columnaOrigen != columnaDestino) && (filaOrigen > filaDestino))
                      )
                        EsValido = true;
                    break;
                case "P^":
                    if(
                        (Math.Abs(filaOrigen - filaDestino) <= 1) && (Math.Abs(columnaOrigen - columnaDestino) <= 1) &&
                        ((filaOrigen +1 == filaDestino)&&(Math.Abs(columnaOrigen - columnaDestino) == 1) ) ||
                        (filaOrigen == filaDestino + 1)
                      )
                        EsValido = true;
                    break;
                case "Pv":
                    if (
                        (Math.Abs(filaOrigen - filaDestino) <= 1) && (Math.Abs(columnaOrigen - columnaDestino) <= 1) &&
                        ((filaOrigen - 1 == filaDestino) && (Math.Abs(columnaOrigen - columnaDestino) == 1)) ||
                        (filaOrigen == filaDestino - 1)
                      )
                        EsValido = true;
                    break;
                case "C^":
                    if ((filaOrigen == filaDestino + 2) && (Math.Abs(columnaOrigen - columnaDestino) == 1))
                        EsValido = true;
                    break;
                case "Cv":
                    if ((filaOrigen == filaDestino - 2) && (Math.Abs(columnaOrigen - columnaDestino) == 1))
                        EsValido = true;
                    break;
                case "L^":
                    if ((filaOrigen > filaDestino) && (columnaOrigen == columnaDestino))
                        EsValido = true && RecorridoEsValido(filaOrigen, filaDestino, columnaOrigen, columnaDestino);
                    break;
                case "Lv":
                    if ((filaOrigen < filaDestino) && (columnaOrigen == columnaDestino))
                        EsValido = true && RecorridoEsValido(filaOrigen, filaDestino, columnaOrigen, columnaDestino);
                    break;
                case "E^":  //torre coronada
                case "Ev":
                    if (
                        (
                        ((columnaOrigen == columnaDestino) && (filaOrigen != filaDestino)) ||
                        ((filaOrigen == filaDestino) && (columnaOrigen != columnaDestino))
                        ) || (Math.Abs(filaOrigen - filaDestino) <= 1 && Math.Abs(columnaOrigen - columnaDestino) <= 1)
                      )
                        EsValido = true && RecorridoEsValido(filaOrigen, filaDestino, columnaOrigen, columnaDestino);
                    break;
                case "Fv":
                case "F^": //Alfil coronado
                    if (  
                         (
                            ((filaOrigen > filaDestino) && (columnaOrigen < columnaDestino) ||
                               (filaOrigen < filaDestino) && (columnaOrigen > columnaDestino)
                            ) &&
                              (filaOrigen + columnaOrigen) == (filaDestino + columnaDestino)
                              &&
                            (
                              (filaOrigen != filaDestino) &&
                              (columnaOrigen != columnaDestino)
                            ) ||
                            (Math.Abs(filaOrigen - filaDestino) <= 1 && Math.Abs(columnaOrigen - columnaDestino) <= 1)
                          )
                        )
                        EsValido = true && RecorridoEsValido(filaOrigen, filaDestino, columnaOrigen, columnaDestino);
                    break; 

                default:
                    return false;

            }
            return EsValido;
        }

        public bool RecorridoEsValido(int filaOrigen, int filaDestino, int columnaOrigen,  int columnaDestino)
        {
            bool EsValido = true;
            //hacia 12 en punto

            if ((filaOrigen > filaDestino) && (columnaOrigen == columnaDestino)) {
                for (int i = filaOrigen -1; i > filaDestino; i--)
                {
                    if (Lugares[i, columnaOrigen] != null)
                        EsValido = false;
                }

            }
            //hacia 1:30
            if ((filaOrigen > filaDestino) && (columnaOrigen < columnaDestino)) {
                //fila disminuye columna aumenta
                int cont = 1;
                for (int fila = filaOrigen -1; fila > filaDestino; fila--)
                {
                    var columna = columnaOrigen + cont;
                    if (Lugares[fila, columna] != null)
                        EsValido = false;
                    cont++;
                }

            }
            //hacia las 3 en punto
            if ((filaOrigen == filaDestino) && (columnaOrigen < columnaDestino))
            {
                for (int i = columnaOrigen +1; i < columnaDestino; i++)
                {
                    if (Lugares[filaOrigen, i] != null)
                        EsValido = false;
                }
            }
            //hacia las 4:30
            if ((filaOrigen < filaDestino) && (columnaOrigen < columnaDestino))
            {
                //aumenta fila aumenta columna
                int cont = 1;
                for (int fila = filaOrigen + 1; fila < filaDestino; fila++)
                {
                    var columna = columnaOrigen + cont;
                    if (Lugares[fila, columna] != null)
                        EsValido = false;
                    cont++;
                }

            }
            //hacia las 6 en punto
            if ((filaOrigen < filaDestino) && (columnaOrigen == columnaDestino))
            {
                for (int i = filaOrigen +1; i < filaDestino; i++)
                {
                    if (Lugares[i, columnaOrigen] != null)
                        EsValido = false;
                }

            }
            //hacia las 7:30
            if ((filaOrigen < filaDestino) && (columnaOrigen > columnaDestino))
            {
                //fila aumenta columna disminuye
                int cont = 1;
                for (int fila = filaOrigen + 1; fila < filaDestino; fila++)
                {
                    var columna = columnaOrigen - cont;
                    if (Lugares[fila, columna] != null)
                        EsValido = false;
                    cont++;
                }
            }
            //hacia las 9 en punto
            if ((filaOrigen == filaDestino) && (columnaOrigen > columnaDestino))
            {
                for (int i = columnaOrigen-1; i > columnaDestino; i--)
                {
                    if (Lugares[filaOrigen, i] != null)
                        EsValido = false;
                }
            }
            //jacia las 11:30
            if ((filaOrigen > filaDestino) && (columnaOrigen > columnaDestino))
            {
                //fila disminuye columna disminuye
                int cont = 1;
                for (int fila = filaOrigen - 1; fila > filaDestino; fila--)
                {
                    var columna = columnaOrigen - cont;
                    if (Lugares[fila, columna] != null)
                        EsValido = false;
                    cont++;
                }
            }

            return EsValido;
        }

    }
}
