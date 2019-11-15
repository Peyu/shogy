using System;
using shogy.Clases;

namespace shogy
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");
            //string val;
            //Console.Write("Enter integer: ");
            //val = Console.ReadLine();
            //Console.WriteLine(val);

            var tab = new Tablero();
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
