using System;

namespace gameshop
{
    class Program
    {
        static void Main()
        {
            double tax = 20;
            const int MAX_CHARACTERS_NAME = 10;
            //const int MIN_DIGITS_UPC = 5;
            //const int MAX_DIGITS_UPC = 10;

            //List<Proizvod> ListOfProducts = new List<Proizvod>();

        End1: while (true)
            {
                Console.WriteLine("Unesite ime proizvoda:");
                string name;
                while (true)
                {
                    name = Console.ReadLine();
                    if (name == "")
                    {
                        Console.WriteLine("Proizvod mora posedovati ime! Pokusajte ponovo:");
                        continue;
                    }
                    else if (name.Length > MAX_CHARACTERS_NAME)
                    {
                        Console.WriteLine("Greska! Ime proizvoda predugacko! Pokusajte ponovo:");
                        continue;
                    }
                    else if(String.IsNullOrWhiteSpace(name))
                    {
                        Console.WriteLine("Ne mozete uneti space kao ime proizvoda! Pokusajte ponovo:");
                    }
                    else break;
                }
                
                Console.WriteLine("Unesite barkod:");
                int barkod;
                while (true)
                {
                    if (!int.TryParse(Console.ReadLine(), out barkod)) Console.WriteLine("Barkod mora biti ceo broj! Pokusajte ponovo:");
                    else if (barkod < 0) Console.WriteLine("Barkod mora biti ceo broj veci od nule! Pokusajte ponovo:");
                    else break;

                }

                Console.WriteLine("Unesite cenu proizvoda:");
                double price;
                while (true)
                {
                    string input = Console.ReadLine();
                    if (input.Contains(',')) input = input.Replace(',', '.');
                    if (!double.TryParse(input, out price)) Console.WriteLine("Cena mora biti realan broj! Pokusajte ponovo:");
                    else if (price <= 0) Console.WriteLine("Cena mora biti broj veci od nule! Pokusajte ponovo:");
                    else break;
                }

                Proizvod product = new Proizvod(name, barkod, price);

                //ListOfProducts.Add(product);

                Console.WriteLine("0. Zatvori program. \n" +
                    "1. Unesite novi proizvod. \n" +
                    "2. Dodaj popust na proizvod.\n" +
                    "3. Promeni PDV (trenutno: " + tax + "%)");

                while (true)
                {
                    int option;

                    if (!int.TryParse(Console.ReadLine(), out option) || option < 0)
                    {
                        Console.WriteLine("Greska prilikom unosa! Pokusajte ponovo:");
                        continue;
                    }

                    switch (option)
                    {
                        case 0:
                            Environment.Exit(0);
                            break;
                        case 1: goto End1;
                            break;
                        case 2:
                            {
                                Console.WriteLine("Popust?");
                                double discount;
                                while (true)
                                {
                                    string input = Console.ReadLine();
                                    input = CheckPercentageInput(input);
                                    if (input.Contains(',')) input = input.Replace(',', '.');
                                    if (!double.TryParse(input, out discount)) Console.WriteLine("Popust mora biti realan broj! Pokusajte ponovo:");
                                    else if (discount < 0) Console.WriteLine("Popust mora biti broj veci od nule! Pokusajte ponovo:");
                                    else if (discount >= 100) Console.WriteLine("Greska prilikom unosa popusta! Broj prevelik. Pokusajte ponovo:");
                                    else
                                    {
                                        product.SetDiscount(discount);
                                        break;
                                    }
                                }
                                Console.WriteLine("Porez = " + tax + "%, Popust = " + product.GetDiscount() + "%\n" +
                                    "Iznos poreza = " + product.IznosPoreza(tax) + " din; Iznos popusta = " + product.IznosPopusta() + " din\n" +
                                    "Osnovna cena = " + product.GetPrice() + " din. Cena nakon poreza i popusta = "
                                    + (product.GetPrice() + product.IznosPoreza(tax) - product.IznosPopusta()));

                                Console.WriteLine("0. Zatvori program.\n" +
                                    "1. Unesi novi proizvod.");
                                int flag;
                                while (true)
                                {
                                    if (int.TryParse(Console.ReadLine(), out flag) && (flag == 0 || flag == 1)) break;
                                    else Console.WriteLine("Greska prilikom unosa! Pokusajte ponovo:");
                                }
                                if (flag == 0) goto case 0;
                                else goto case 1;
                            }
                            break;
                        case 3:
                            {
                                Console.WriteLine("PDV u procentima?");
                                while (true)
                                {
                                    string input = Console.ReadLine();
                                    input = CheckPercentageInput(input);
                                    if (input.Contains(',')) input = input.Replace(',', '.' );
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
                                Console.WriteLine("0. Zatvori program.\n" +
                                    "1. Unesi novi proizvod.\n" +
                                    "2. Dodaj popust na proizvod.");
                                int flag;
                                while(true)
                                {
                                    if (int.TryParse(Console.ReadLine(), out flag) && (flag == 0 || flag == 1 || flag == 2)) break;
                                    else Console.WriteLine("Greska prilikom unosa! Pokusajte ponovo:");
                                }
                                if (flag == 0) goto case 0;
                                else if(flag == 1) goto case 1;
                                else goto case 2;
                            }
                            break;
                        default:
                            Console.WriteLine("Los unos! Pokusajte ponovo:");
                            break;
                    }
                }              
            }
        }
        static string CheckPercentageInput(string input)
        {
            if (input.Contains('%'))
            {
                for (int i = 0; i < input.Length; i++)
                {
                    if (input[i] == '%' && Char.IsNumber(input[i - 1]) && i == input.Length - 1)
                    {
                        input = input.Substring(0, i);
                        return input;
                    }
                    else if (input[i] == '%' && input[i - 1] == ' ' && Char.IsNumber(input[i - 2]) && i == input.Length - 1)
                    {
                        input = input.Substring(0, i - 1);
                        return input;
                    }                    
                }
                return input;
            }
            else return input;
        }
    }
}