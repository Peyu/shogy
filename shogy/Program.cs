using System;
using shogy.Clases;

namespace shogy
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Ingrese nombre jugador 1");
            var nombre1 = Console.ReadLine();
            Jugador j1 = new Jugador(nombre1);
            Console.WriteLine("Ingrese nombre jugador 2");
            var nombre2 = Console.ReadLine();
            Jugador j2 = new Jugador(nombre2);
            Console.Clear();

            var tab = new Tablero(j1,j2);
            
            while (true)
            {
                Console.WriteLine(" ");
                tab.Dibujar();

                Console.WriteLine("Turno de " + tab.Turno.Nombre);
                Console.WriteLine("Casillero Origen");
                string origen = Console.ReadLine();

                Console.WriteLine("Casillero Destino");
                string destino = Console.ReadLine();

                tab.Mover(origen, destino);
                
            }
        }
    }
}
