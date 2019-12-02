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
        private bool CambiarTurnoFlag = false;
        public bool EnJuego = true;
        private Ficha atacante;

        public Tablero(Jugador j1, Jugador j2) {
            J1 = j1;
            J2 = j2;

            Lugares[0,0] = new Ficha("Lv", J2, "00");     //Lancero 
            Lugares[0,1] = new Ficha("Cv", J2, "01");     //Caballo
            Lugares[0,2] = new Ficha("Pv", J2, "02");     //Plata
            Lugares[0,3] = new Ficha("Ov", J2, "03");     //Oro
            Lugares[0,4] = new Ficha("Rv", J2, "04");     //Rey
            Lugares[0,5] = new Ficha("Ov", J2, "05");     
            Lugares[0, 6] = new Ficha("Pv", J2, "06");
            Lugares[0, 7] = new Ficha("Cv", J2, "07");
            Lugares[0, 8] = new Ficha( "Lv", J2, "08");
            Lugares[1,1] = new Ficha("Tv", J2,"11");     //Torre
            Lugares[1, 7] = new Ficha("Av", J2,"17");    //Alfil
            //fila de peones
            for (int columna = 0; columna < 9; columna++)
            {
                Lugares[2, columna] = new Ficha("pv", J2, "" + 2 + columna);   //peon
            }
            //filas peones
            for (int columna = 0; columna < 9; columna++)
            {
                Lugares[6, columna] = new Ficha("p^", J1, "" + 6 + columna);   //peon
            }
            Lugares[7, 1] = new Ficha("A^", J1, "71");    
            Lugares[7, 7] = new Ficha("T^", J1, "77");    
            Lugares[8, 0] = new Ficha("L^", J1, "80");     
            Lugares[8, 1] = new Ficha("C^", J1, "81");    
            Lugares[8, 2] = new Ficha("P^", J1, "82");    
            Lugares[8, 3] = new Ficha("O^", J1, "83");    
            Lugares[8, 4] = new Ficha("R^", J1, "84");    
            Lugares[8, 5] = new Ficha("O^", J1, "85");
            Lugares[8, 6] = new Ficha("P^", J1, "86");
            Lugares[8, 7] = new Ficha("C^", J1, "87");
            Lugares[8, 8] = new Ficha("L^", J1, "88");

            //for (int i = 0; i < 9; i++)
            //{
            //    for (int j = 0; j < 9; j++)
            //    {
            //        ActualizarAtaquesPosibles(Lugares[i, j]);
            //    }
            //}
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
                    //actualizo posiblesAtaques
                    ActualizarAtaquesPosibles(f);
                    
                    linea += f?.Dibujo == null? "   " : " " + f.Dibujo ;
                }
                Console.WriteLine(filas + "  " + linea);
                Console.WriteLine(" ");
            }
            if(ChequearJaque())
                ChequearJaqueMate();

            if (CambiarTurnoFlag) {
                CambiarTurno();
                CambiarTurnoFlag = false;
            }

        }

        private void ChequearJaqueMate() {
            //primero busco el rey
            string PosicionReyContrincante = "";
            int filaRey = -1;
            int columnaRey = -1;

            for (int fila = 0; fila < 9; fila++)
            {
                for (int columna = 0; columna < 9; columna++)
                {
                    if (Lugares[fila, columna] != null)
                    {
                        if (Lugares[fila, columna].Dibujo.Contains("R") && Lugares[fila, columna].Duenio.Nombre != Turno.Nombre)
                        {
                            PosicionReyContrincante = Lugares[fila, columna].PosicionActual;
                            filaRey = fila;
                            columnaRey = columna;
                        }
                    }
                }
            }
            //busco posibles jugadas del rey
            List<string> posiblesJugadas = new List<string>();
            for (int f = filaRey -1; f <= filaRey +1; f++)
            {
                for (int c = columnaRey -1 ; c <= columnaRey +1; c++)
                {
                    if (f >= 0 && f < 9 && c >= 0 && c < 9) {
                        string posibleJugada = "" + f + c;
                        posiblesJugadas.Add(posibleJugada);
                    }

                }
            }

            //borro las jugadas que se encuentren en jaque
            for (int fila = 0; fila < 9; fila++)
            {
                for (int columna = 0; columna < 9; columna++)
                {
                    if(Lugares[fila, columna] != null) { 
                        foreach (var posicion in Lugares[fila, columna].PosiblesAtaques)
                        {
                            if (posiblesJugadas.Contains(posicion)) {
                                posiblesJugadas.Remove(posicion);
                            }

                        }
                    }
                }
            }

            //Si el atacante puede ser comido no es jaque mate
            bool atacable = false;
            for (int fila = 0; fila < 9; fila++)
            {
                for (int columna = 0; columna < 9; columna++)
                {
                    if (Lugares[fila, columna] != null && Lugares[fila, columna].Duenio != Turno) {
                        foreach (var posicionAtacable in Lugares[fila, columna].PosiblesAtaques)
                        {
                            if (atacante.PosicionActual == posicionAtacable) {
                                atacable = true;
                            }
                        }

                    }

                }
            }

            if (posiblesJugadas.Count == 0 && !atacable) {
                EnJuego = false;
                Mensaje("Jaque Mate!! " + Turno.Nombre + " ha ganado el Juego");
            }

        }

        private void CambiarTurno() {

            Turno = Turno.Nombre == J1.Nombre ? J2 : J1; 
        }

        private bool ChequearJaque() {
            //si el rey ajeno queda en jaque se tiene que anunciar

            //primero busco el rey
            string PosicionReyContrincante = "";
            for (int fila = 0; fila < 9; fila++)
            {
                for (int columna = 0; columna < 9; columna++)
                {
                    if (Lugares[fila, columna] != null)
                    {
                        if (Lugares[fila, columna].Dibujo.Contains("R") && Lugares[fila, columna].Duenio.Nombre != Turno.Nombre)
                        {
                            PosicionReyContrincante = Lugares[fila, columna].PosicionActual;
                        }
                    }
                }
            }

            bool jaque = false;
            for (int fila = 0; fila < 9; fila++)
            {
                for (int columna = 0; columna < 9; columna++)
                {
                    if (Lugares[fila, columna] != null)
                    {
                        if (Lugares[fila, columna].Duenio == Turno)
                        {
                            if (Lugares[fila, columna].PosiblesAtaques.Contains(PosicionReyContrincante))
                            {
                                jaque = true;
                                atacante = Lugares[fila, columna];
                            }

                        }
                    }
                }
            }
            if (jaque) {
                Mensaje(Turno.Nombre + " ha cantado jaque al Rey");
            }

            return jaque;
        }


        public void Mover(string origen, string destino) {

            //agregar regex para verificar formato de entrada y verificar existencia
            string pattern = @"\d{2}";
            string msg = "";
            if (!(Regex.IsMatch(origen, pattern) && Regex.IsMatch(destino, pattern)))
            {
                msg = "Error en coordenadas intentelo denuevo";
            }
            else
            {
                char[] desde = origen.ToCharArray();
                int filaOrigen = int.Parse(desde[0].ToString());
                int columnaOrigen = int.Parse(desde[1].ToString());

                char[] hasta = destino.ToCharArray();
                int filaDestino = int.Parse(hasta[0].ToString());
                int columnaDestino = int.Parse(hasta[1].ToString());

                bool cambioEsNecesario = false;
                Ficha FichaEnMovimiento = null;
                try
                {
                    FichaEnMovimiento = Lugares[filaOrigen, columnaOrigen];
                }
                catch (IndexOutOfRangeException e)
                {
                    msg ="No hay ninguna ficha en el casillero de origen, intentalo denuevo";
                }

                //reviso que el rey propio no quede en jaque
                bool enJaque = false;
                //si estoy moviendo el rey chequeo su nueva posicion
                if (FichaEnMovimiento != null)
                {
                    if (FichaEnMovimiento.Dibujo.Contains("R") && msg == "")
                    {
                        string posicionAChequear = "" + filaDestino + columnaDestino;
                        for (int fila = 0; fila < 9; fila++)
                        {
                            for (int columna = 0; columna < 9; columna++)
                            {
                                if (Lugares[fila, columna] != null)
                                {
                                    if ((Lugares[fila, columna].Duenio.Nombre != Turno.Nombre))
                                    {
                                        if (Lugares[fila, columna].PosiblesAtaques.Contains(posicionAChequear))
                                        {
                                            msg = "No puedes hacer un movimiento que termine en tu propio jaque";
                                            enJaque = true;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    //muevo la ficha
                    if (FichaEnMovimiento.Duenio.Nombre == Turno.Nombre && msg == "")
                    {
                        if (MovimientoEsValido(filaOrigen, columnaOrigen, filaDestino, columnaDestino, FichaEnMovimiento))
                        {
                            //si el casillero destino esta vacio
                            if (Lugares[filaDestino, columnaDestino] == null)
                            {
                                //vacio el lugar
                                Lugares[filaOrigen, columnaOrigen] = null;
                                //coloco ficha en nueva coordenada
                                Lugares[filaDestino, columnaDestino] = FichaEnMovimiento;
                                //Actualizo posicion
                                FichaEnMovimiento.PosicionActual = "" + filaDestino + columnaDestino;
                                //chequeo coronacion
                                ChequearCoronacion(filaOrigen, filaDestino, FichaEnMovimiento);
                                

                            }
                            else  //si hay un enemigo en el casillero enemigo, se come
                            {
                                if (Lugares[filaDestino, columnaDestino].Duenio.Nombre != Turno.Nombre)
                                {
                                     
                                    if (Lugares[filaDestino, columnaDestino].Dibujo.Contains("R"))
                                    {
                                        EnJuego = false;
                                        msg = "El Rey ha muerto, que viva el Rey!!";
                                        Mensaje(Turno.Nombre + " ha comido al rey adversario. " + Turno.Nombre + " es el ganador");
                                    }
                                    else
                                    {
                                        //vacio el lugar
                                        Lugares[filaOrigen, columnaOrigen] = null;
                                        //ficha pasa al otro jugador
                                        Lugares[filaDestino, columnaDestino].Duenio = Turno;
                                        Lugares[filaDestino, columnaDestino].Dibujo = Lugares[filaDestino, columnaDestino].DibujoOriginal;
                                        Turno.EnMano.Add(Lugares[filaDestino, columnaDestino]);
                                        //coloco ficha en nueva coordenada
                                        Lugares[filaDestino, columnaDestino] = FichaEnMovimiento;
                                        ChequearCoronacion(filaOrigen, filaDestino, FichaEnMovimiento);
                                        //Actualizo posicion
                                        FichaEnMovimiento.PosicionActual = "" + filaDestino + columnaDestino;
                                    }
                                }
                                else
                                {
                                    msg = "No puedes comer tus propias fichas";
                                }
                            }

                        }
                        else
                        {
                            msg = "Movimiento no permitido, intentelo denuevo";
                        }
                    }
                    else
                    {
                        if (msg == "")  
                            msg = "Solo puedes mover tus propias fichas, intentalo denuevo";
                    }

                }
                else
                    msg = "Error en coordenadas, intentalo nuevamente";

                //cambio de turno
                if (msg == "") {
                    CambiarTurnoFlag = true;
                    //CambiarTurno();
                }
                else
                    Mensaje(msg);
              
                Console.Clear();
                Dibujar();
            }
        }

        public void ColocarFichaEnMano() {
            
            string msg = "";
            bool posicionEsValida = true;
            Console.Clear();
            Console.WriteLine("Estas son las fichas que posees:");
            Console.WriteLine("");
            foreach (var ficha in Turno.EnMano)
            {
                Console.WriteLine(ficha.DibujoOriginal);
            }
            Console.WriteLine("");
            Console.WriteLine("Cual ficha deseas colocar en el Tablero? (Oprima 'N' si no desea colocar ninguna)");
            var fichaAColocar = Console.ReadLine();
            if (fichaAColocar != "n" && fichaAColocar != "N")
            {
                var EnMano = Turno.EnMano.Find(x => x.Dibujo == fichaAColocar);
                if (EnMano == null)
                {
                    Mensaje("Error al selecionar ficha, intentalo denuevo");
                    ColocarFichaEnMano();
                }
                else
                {
                    Dibujar();
                    Console.WriteLine("Escriba coordinada destino");
                    string destino = Console.ReadLine();
                    string pattern = @"\d{2}";
                    if (!(Regex.IsMatch(destino, pattern)))
                    {
                        msg = "Error en coordenadas intentelo denuevo";
                    }
                    else
                    {
                        char[] hasta = destino.ToCharArray();
                        int filaDestino = int.Parse(hasta[0].ToString());
                        int columnaDestino = int.Parse(hasta[1].ToString());

                        //si es peon reviso 
                        if (EnMano.Dibujo.Contains("p"))
                        {
                            //reviso que no haya otro peon sbre misma linea
                            bool YaExisteUnPeon = false;
                            for (int i = 0; i < 9; i++)
                            {
                                if (Lugares[i,columnaDestino] != null)
                                {
                                    if (Lugares[i, columnaDestino].Dibujo.Contains("p") &&
                                        Lugares[i, columnaDestino].Duenio == Turno
                                        )
                                    {
                                        YaExisteUnPeon = true;
                                    }
                                    
                                }
                            }
                            if (YaExisteUnPeon)
                            {
                                posicionEsValida = false;
                                msg ="Ya existe un peon sobre la misma linea vertical, intentalo nuevamente";
                            }
                            
                            //reviso que no este sobre la ultima linea
                            if ((Turno == J1 && filaDestino == 0) || (Turno == J2 && filaDestino == 8))
                            {
                                msg = "No puedes colocar un peon sobre la ultima fila";
                                posicionEsValida = false;
                            }
                        }
                        //reviso que lanceros no sean colocados sobre ultima linea
                        else if (EnMano.Dibujo.Contains("L") )
                        {
                            if (!((Turno == J1 && filaDestino != 0) || (Turno == J2 && filaDestino != 8))){
                                posicionEsValida = false;
                                msg ="No puedes colocar un Lancero sobre la última fila";
                            }
                        }
                        //reviso que caballos no sean colocados sobre las ultimas dos lineas
                        else if (EnMano.Dibujo.Contains("C") )
                        {
                            if (!((Turno == J1 && filaDestino > 1) || (Turno == J2 && filaDestino < 7))) {
                                posicionEsValida = false;
                                msg ="No puedes colocar un Caballo sobre las dos últimas filas";
                            }
                        }
                        
                        if (posicionEsValida)
                        {
                            //reviso que no se ponga una ficha sobre otra    
                            if (!(Lugares[filaDestino, columnaDestino] == null))
                            {
                                posicionEsValida = false;
                                msg = "Ya hay una ficha en esa coordenada, intentalo nuevamente";
                            }
                            else
                            {
                                //remuevo ficha de la lista en mano
                                Turno.EnMano.Remove(EnMano);

                                //agrego orientacion a la ficha
                                if (Turno == J1)
                                    EnMano.Dibujo += "^";
                                else
                                    EnMano.Dibujo += "v";
                                Lugares[filaDestino, columnaDestino] = EnMano;
                                EnMano.PosicionActual = "" + filaDestino + columnaDestino;

                            }
                        }
                        
                    }
                    if (posicionEsValida) {
                        CambiarTurno();
                    }
                    else
                        Mensaje(msg);
                }
            }
            
        }

        public void ActualizarAtaquesPosibles(Ficha ficha) {
            if (ficha?.PosiblesAtaques != null && ficha?.PosiblesAtaques.Count >0) {
                ficha.PosiblesAtaques.Clear();
            }
            if (ficha != null) {
                switch (ficha.Dibujo)
                {
                    case "p^":
                        
                        if (ficha.getFila() != 0)
                        {
                            string enAtaque = (ficha.getFila() - 1).ToString() + ficha.getColumna();
                            ficha.PosiblesAtaques.Add(enAtaque);
                        }
                        break;
                    case "pv":
                        
                        if (ficha.getFila() != 8)
                        {
                            string enAtaque = (ficha.getFila() + 1).ToString() + ficha.getColumna();
                            ficha.PosiblesAtaques.Add(enAtaque);
                        }
                        break;
                    case "R^":
                        
                        for (int columna = ficha.getColumna() - 1; columna <= ficha.getColumna() + 1; columna++)
                        {
                            string enAtaque = (ficha.getFila().ToString() + columna);
                            ficha.PosiblesAtaques.Add(enAtaque);
                            enAtaque = ((ficha.getFila() + 1).ToString() + columna);
                            ficha.PosiblesAtaques.Add(enAtaque);
                            if (ficha.getFila() != 8)
                            {
                                enAtaque = ((ficha.getFila() - 1).ToString() + columna);
                                ficha.PosiblesAtaques.Add(enAtaque);
                            }
                            
                        }
                        break;
                    case "Rv":
                        
                        for (int columna = ficha.getColumna() - 1; columna <= ficha.getColumna() + 1; columna++)
                        {
                            string enAtaque = (ficha.getFila().ToString() + columna);
                            ficha.PosiblesAtaques.Add(enAtaque);
                            if (ficha.getFila() != 0)
                            {
                                enAtaque = ((ficha.getFila() + 1).ToString() + columna);
                                ficha.PosiblesAtaques.Add(enAtaque);
                            }
                            if (ficha.getFila() != 8)
                            {
                                enAtaque = ((ficha.getFila() - 1).ToString() + columna);
                                ficha.PosiblesAtaques.Add(enAtaque);
                            }

                        }
                        break;
                    case "Tv":
                    case "T^":
                        //el nombre de los metodos se corresponde a las agujas de un reloj
                        Doce(ficha);
                        Seis(ficha);
                        Tres(ficha);
                        Nueve(ficha);
                        break;
                    case "A^":
                    case "Av":
                        //el nombre de los metodos se corresponde a las agujas de un reloj
                        UnaTreinta(ficha);
                        CuatroTreinta(ficha);
                        SieteTreinta(ficha);
                        DiezTreinta(ficha);
                        break;
                    case "O^":
                        for (int columna = ficha.getColumna() - 1; columna <= ficha.getColumna() + 1; columna++)
                        {
                            string enAtaque = (ficha.getFila().ToString() + columna);
                            ficha.PosiblesAtaques.Add(enAtaque);
                            enAtaque = ((ficha.getFila() - 1).ToString() + columna);
                            ficha.PosiblesAtaques.Add(enAtaque);
                            if (ficha.getFila() != 8 && columna == ficha.getColumna())
                            {
                                enAtaque = ((ficha.getFila() + 1).ToString() + columna);
                                ficha.PosiblesAtaques.Add(enAtaque);
                            }

                        }
                        break;
                    case "Ov":
                        for (int columna = ficha.getColumna() - 1; columna <= ficha.getColumna() + 1; columna++)
                        {
                            string enAtaque = (ficha.getFila().ToString() + columna);
                            ficha.PosiblesAtaques.Add(enAtaque);
                            enAtaque = ((ficha.getFila() + 1).ToString() + columna);
                            ficha.PosiblesAtaques.Add(enAtaque);
                            if (ficha.getFila() != 0 && columna == ficha.getColumna())
                            {
                                enAtaque = ((ficha.getFila() - 1).ToString() + columna);
                                ficha.PosiblesAtaques.Add(enAtaque);
                            }

                        }
                        break;
                    case "P^":
                        for (int columna = ficha.getColumna() - 1; columna <= ficha.getColumna() + 1; columna++)
                        {
                            string enAtaque = ((ficha.getFila() - 1).ToString() + columna);
                            ficha.PosiblesAtaques.Add(enAtaque);
                            if (ficha.getFila() != 8 && columna != ficha.getColumna())
                            {
                                enAtaque = ((ficha.getFila() + 1).ToString() + columna);
                                ficha.PosiblesAtaques.Add(enAtaque);
                            }

                        }
                        break;
                    case "Pv":
                        for (int columna = ficha.getColumna() - 1; columna <= ficha.getColumna() + 1; columna++)
                        {
                            string enAtaque = ((ficha.getFila() + 1).ToString() + columna);
                            ficha.PosiblesAtaques.Add(enAtaque);
                            if (ficha.getFila() != 0 && columna != ficha.getColumna())
                            {
                                enAtaque = ((ficha.getFila() - 1).ToString() + columna);
                                ficha.PosiblesAtaques.Add(enAtaque);
                            }

                        }
                        break;
                    case "C^":
                        if (ficha.getFila() > 1) {
                            if (ficha.getColumna() != 0)
                            {
                                string enAtaque = "" + (ficha.getFila() - 2) + (ficha.getColumna() - 1);
                                ficha.PosiblesAtaques.Add(enAtaque);
                            }
                            if (ficha.getColumna() != 8) {
                                string enAtaque = "" + (ficha.getFila() - 2) + (ficha.getColumna() + 1);
                                ficha.PosiblesAtaques.Add(enAtaque);
                            }
                        }
                        break;
                    case "Cv":
                        if (ficha.getFila() < 7)
                        {
                            if (ficha.getColumna() != 0)
                            {
                                string enAtaque = "" + (ficha.getFila() + 2) + (ficha.getColumna() - 1);
                                ficha.PosiblesAtaques.Add(enAtaque);
                            }
                            if (ficha.getColumna() != 8)
                            {
                                string enAtaque = "" + (ficha.getFila() + 2) + (ficha.getColumna() + 1);
                                ficha.PosiblesAtaques.Add(enAtaque);
                            }
                        }
                        break;
                    case "L^":
                        Doce(ficha);
                        break;
                    case "Lv":
                        Seis(ficha);
                        break;
                    case "E^":
                    case "Ev":
                        Doce(ficha);
                        Tres(ficha);
                        Seis(ficha);
                        Nueve(ficha);
                        for (int columna = ficha.getColumna() - 1; columna <= ficha.getColumna() + 1; columna++)
                        {
                            string enAtaque = (ficha.getFila().ToString() + columna);
                            if ( !(enAtaque.Contains("-") || ficha.PosiblesAtaques.Contains(enAtaque)) )
                                ficha.PosiblesAtaques.Add(enAtaque);
                            
                            enAtaque = ((ficha.getFila() + 1).ToString() + columna);
                            if (!(enAtaque.Contains("-") || ficha.PosiblesAtaques.Contains(enAtaque)))
                                ficha.PosiblesAtaques.Add(enAtaque);

                            enAtaque = ((ficha.getFila() - 1).ToString() + columna);
                            if (!(enAtaque.Contains("-") || ficha.PosiblesAtaques.Contains(enAtaque)))
                                ficha.PosiblesAtaques.Add(enAtaque);
                        }
                        break;
                    case "Fv":
                    case "F^":
                        UnaTreinta(ficha);
                        CuatroTreinta(ficha);
                        SieteTreinta(ficha);
                        DiezTreinta(ficha);
                        for (int columna = ficha.getColumna() - 1; columna <= ficha.getColumna() + 1; columna++)
                        {
                            string enAtaque = (ficha.getFila().ToString() + columna);
                            if (!(enAtaque.Contains("-") || ficha.PosiblesAtaques.Contains(enAtaque)))
                                ficha.PosiblesAtaques.Add(enAtaque);

                            enAtaque = ((ficha.getFila() + 1).ToString() + columna);
                            if (!(enAtaque.Contains("-") || ficha.PosiblesAtaques.Contains(enAtaque)))
                                ficha.PosiblesAtaques.Add(enAtaque);

                            enAtaque = ((ficha.getFila() - 1).ToString() + columna);
                            if (!(enAtaque.Contains("-") || ficha.PosiblesAtaques.Contains(enAtaque)))
                                ficha.PosiblesAtaques.Add(enAtaque);
                        }
                        break;
                }
            }
            
        }

        private void Doce(Ficha ficha) {
            for (int i = ficha.getFila() - 1; i >= 0; i--)
            {
                if (i >= 0 && i < 9)
                {
                    string enAtaque = ("" + i + ficha.getColumna());
                    ficha.PosiblesAtaques.Add(enAtaque);

                    if (Lugares[i, ficha.getColumna()] != null)
                    {
                        break;
                    }

                }
            }
        }
        private void Seis(Ficha ficha)
        {
            for (int i = ficha.getFila() + 1; i < 9; i++)
            {
                if (i >= 0 && i < 9)
                {
                    string enAtaque = ("" + i + ficha.getColumna());
                    ficha.PosiblesAtaques.Add(enAtaque);

                    if (Lugares[i, ficha.getColumna()] != null)
                    {
                        break;
                    }
                    
                        
                }
            }
        }
        private void Tres(Ficha ficha)
        {
            for (int i = ficha.getColumna() + 1; i < 9; i++)
            {
                if (i >= 0 && i < 9)
                {
                    string enAtaque = ("" + ficha.getFila() + i);
                    ficha.PosiblesAtaques.Add(enAtaque);
                    if (Lugares[ficha.getFila(), i] != null)
                    {
                        break;
                    }
                    
                        
                }
            }
        }
        private void Nueve(Ficha ficha) {
            for (int i = ficha.getColumna()-1 ; i >= 0; i--)
            {
                if (i >= 0 && i < 9)
                {
                    string enAtaque = ("" + ficha.getFila() + i);
                    ficha.PosiblesAtaques.Add(enAtaque);
                    if (Lugares[ficha.getFila(), i] != null)
                    {
                        break;
                    }
                    
                }
            }
        }
        private void UnaTreinta(Ficha ficha) {
            
            int contador = 1;
            for (int fila = ficha.getFila() -1; fila >= 0; fila--)  //fila disminuye columna aumenta
            {
                int columna = ficha.getColumna();
                columna += contador;
                contador++;
                if (fila >= 0 && fila <= 8 && columna >= 0 && columna <= 8)
                {
                    string enAtaque = ("" + fila + columna);
                    ficha.PosiblesAtaques.Add(enAtaque);
                    if (Lugares[fila, columna] != null)
                    {
                        break;
                    }
                    
                }
                
            }
        }
        private void CuatroTreinta(Ficha ficha) {// todo aumenta
            int contador = 1;
            for (int fila = ficha.getFila() + 1; fila < 9; fila++)  
            {
                int columna = ficha.getColumna();
                columna += contador;
                contador++;
                if (fila >= 0 && fila <= 8 && columna >= 0 && columna <= 8)
                {
                    string enAtaque = ("" + fila + columna);
                    ficha.PosiblesAtaques.Add(enAtaque);
                    if (Lugares[fila, columna] != null)
                    {
                        break;
                    }
                    
                }
               
            }
        }
        private void SieteTreinta(Ficha ficha) { //fila aumenta columna disminuye
            int contador = 1;
            for (int fila = ficha.getFila() + 1; fila < 9; fila++)  
            {
                int columna = ficha.getColumna();
                columna -= contador;
                contador++;
                if (fila >= 0 && fila <= 8 && columna >= 0 && columna <= 8)
                {
                    string enAtaque = ("" + fila + columna);
                    ficha.PosiblesAtaques.Add(enAtaque);
                    if (Lugares[fila, columna] != null)
                    {
                        break;
                    
                    }
                }
            }
        }
        private void DiezTreinta(Ficha ficha) {//todo disminuye
            int contador = 1;
            for (int fila = ficha.getFila() - 1; fila >= 0; fila--)  
            {
                int columna = ficha.getColumna();
                columna -= contador;
                contador++;
                if (fila >= 0 && fila <= 8 && columna >= 0 && columna <= 8)
                {
                    string enAtaque = ("" + fila + columna);
                    ficha.PosiblesAtaques.Add(enAtaque);
                    if (Lugares[fila, columna] != null)
                    {
                        break;
                    }
                    
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
                                Console.WriteLine("Por favor responda con S o N, presione Enter para continuar...");
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
                        ActualizarAtaquesPosibles(ficha);
                    }
                    else {
                        if (ficha.Dibujo.Contains("T"))
                            ficha.Dibujo = "Ev";
                        else if (ficha.Dibujo.Contains("A"))
                            ficha.Dibujo = "Fv";
                        else
                            ficha.Dibujo = "Ov";
                        ActualizarAtaquesPosibles(ficha);
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
            //Dibujar();
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
                    if (  //diagonales izq a der
                          (((filaOrigen > filaDestino) && (columnaOrigen < columnaDestino) ||
                             (filaOrigen < filaDestino) && (columnaOrigen > columnaDestino)
                          ) &&
                            (filaOrigen + columnaOrigen) == (filaDestino + columnaDestino)
                            &&
                          (
                            (filaOrigen != filaDestino) &&
                            (columnaOrigen != columnaDestino)
                          )) || (Math.Abs(filaOrigen - filaDestino) <= 1 && Math.Abs(columnaOrigen - columnaDestino) <= 1)
                        )
                        EsValido = true && RecorridoEsValido(filaOrigen, filaDestino, columnaOrigen, columnaDestino);
                    else if ( //diagonales der a izq

                            ((((filaOrigen < filaDestino) && (columnaOrigen < columnaDestino)) ||
                               ((filaOrigen > filaDestino) && (columnaOrigen > columnaDestino)
                            ) &&
                                (Math.Abs(filaOrigen - columnaOrigen)) ==
                                (Math.Abs(filaDestino - columnaDestino))
                            ) &&
                            (
                                (filaOrigen != filaDestino) &&
                                (columnaOrigen != columnaDestino)
                            )) || (Math.Abs(filaOrigen - filaDestino) <= 1 && Math.Abs(columnaOrigen - columnaDestino) <= 1)

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
