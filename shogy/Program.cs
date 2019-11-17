using System;
using shogy.Clases;

namespace shogy
{
    class Program
    {
        static void Main(string[] args)
        {
            
            var tab = new Tablero();
            while (true)
            {
                tab.Dibujar();

                Console.WriteLine("");
                Console.WriteLine("Turno jugador 1");
                Console.WriteLine("Casillero Origen");
                string origen = Console.ReadLine();

                Console.WriteLine("Casillero Destino");
                string destino = Console.ReadLine();

                tab.Mover(origen, destino);
                
            }
        }
    }
}
