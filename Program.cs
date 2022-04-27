using System;

namespace gameshop
{
    class Program
    {
        static void Main()
        {
            double tax;
            const int MAX_CHARACTERS_NAME = 10;
            //const int MIN_DIGITS_UPC = 5;
            //const int MAX_DIGITS_UPC = 10;

            //List<Proizvod> ListOfProducts = new List<Proizvod>();

            End: while (true)
            {
                Console.WriteLine("Unesite ime proizvoda:");
                string name;
                while (true)
                {
                    name = Console.ReadLine();
                    if (name == "")
                    {
                        Console.WriteLine("Greska prilikom unosa naziva proizvoda! Pokusajte ponovo:");
                        continue;
                    }
                    if (name.Length > MAX_CHARACTERS_NAME)
                    {
                        Console.WriteLine("Greska! Ime proizvoda predugacko! Pokusajte ponovo:");
                    }
                    else break;
                }

                Console.WriteLine("Unesite barkod:");
                int barkod;
                while (true)
                {
                    if (int.TryParse(Console.ReadLine(), out barkod) && barkod >= 0) break;
                    else Console.WriteLine("Greska prilikom unosa barkoda! Pokusajte ponovo:");

                }

                Console.WriteLine("Unesite cenu proizvoda:");
                double price;
                while (true)
                {
                    if (double.TryParse(Console.ReadLine(), out price) && price > 0) break;
                    else Console.WriteLine("Greska prilikom unosa cene! Pokusajte ponovo:");
                }

                Proizvod product = new Proizvod(name, barkod, price);

                //ListOfProducts.Add(product);
                
                Console.WriteLine("0. Zatvori program. \n" +
                    "1. Unesite novi proizvod. \n" +
                    "2. Ispisite cenu proizvoda sa uracunatim PDV-om");

                while (true)
                {
                    int option;

                    if (!(int.TryParse(Console.ReadLine(), out option)) || option < 0)
                    {
                        Console.WriteLine("Greska prilikom unosa! Pokusajte ponovo:");
                        continue;
                    }

                    switch (option)
                    {
                        case 0:
                            Environment.Exit(0);
                            break;
                        case 1: goto End;
                            break;
                        case 2:
                            {
                                Console.WriteLine("PDV u procentima?");
                                while (true)
                                {
                                    string input = Console.ReadLine();
                                    if (double.TryParse(input, out tax) && tax > 0)
                                    {
                                        Console.WriteLine("Cena " + product.GetPrice() + " din pre poreza i "
                                            + Math.Round(product.GetPrice() * (1 + tax / 100), 2) + " din nakon " + tax + "% poreza.");
                                        break;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Pogresan unos! Pokusajte ponovo!");
                                    }
                                }
                                Environment.Exit(1);
                                break;
                            }
                        default:
                            Console.WriteLine("Los unos! Pokusajte ponovo:");
                            break;
                    }
                }
                
            }

        }
    }
}