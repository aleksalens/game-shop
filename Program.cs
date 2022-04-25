using System;

namespace gameshop
{
    class Program
    {
        static void Main()
        {
            double tax;

            Proizvod LegoBlokovi = new Proizvod("Friends Forest House", 41679, 20.25);


            Console.WriteLine("Za izlazak iz aplikacije unesite exit!");

            while (true)
            {
                Console.WriteLine("\nPDV u procentima?");
                string input = Console.ReadLine();
                if (input == "exit") break;
                if (double.TryParse(input, out tax) && tax > 0)
                {
                 
                    Console.WriteLine("Cena " + LegoBlokovi.GetPrice() + " din pre poreza i "
                        + Math.Round(LegoBlokovi.GetPrice() * (1 + tax / 100), 2) + " din nakon " + tax + "% poreza.");

                }
                else
                {
                    Console.WriteLine("Pogresan unos! Pokusajte ponovo!");
                    continue;
                }
            }
        }
    }
}