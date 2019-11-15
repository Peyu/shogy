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

            string val = Console.ReadLine();


        }
    }
}
