using System;

namespace gameshop
{
    class Program
    {
        static void Main()
        {
            double tax = 20;
            double discount = 0;
            const int MAX_CHARACTERS_NAME = 10;
            //const int MIN_DIGITS_UPC = 5;
            //const int MAX_DIGITS_UPC = 10;

            List<int> ListOfDiscountedUPCs = new List<int>();

            List<Proizvod> ListOfProducts = new List<Proizvod>();
            Proizvod lego = new Proizvod("Lego Kockice", 12345, 120, 7);
            Proizvod lopta = new Proizvod("lopta za fudbal", 98765, 70);
            Proizvod sveska = new Proizvod("Sveska", 41679, 20.25, 5);

            ListOfProducts.Add(lego);
            ListOfProducts.Add(lopta);
            ListOfProducts.Add(sveska);

            ListOfDiscountedUPCs.Add(lego.GetUPC());
            ListOfDiscountedUPCs.Add(sveska.GetUPC());



            while (true)
            {
                Console.WriteLine("0. Zatvori program.\n" +
                    "1. Dodaj novi proizvod.\n" +
                    "2. Pristupi proizvodu.\n" +
                    "3. Promeni listu UPC za dodatni popust.\n" +
                    "4. Promeni popust na kasi (trenutno: " + discount + "%).\n" +
                    "5. Promeni PDV (trenutno: " + tax + "%).\n" +
                    "6. Ispisi sve proizvode sa dodatnim popustom.");

                int option;
                while(true)
                {
                    if (!int.TryParse(Console.ReadLine(), out option) || option < 0)
                    {
                        Console.WriteLine("Greska prilikom unosa! Pokusajte ponovo:");
                        continue;
                    }
                    else break;
                }
                
                switch (option)
                {
                    case 0:
                        Environment.Exit(0);
                        break;
                    case 1:
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
                                else if (String.IsNullOrWhiteSpace(name))
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
                                else if (!UPCDoesntExist(ListOfProducts, barkod)) Console.WriteLine("Barkod vec postoji u sistemu! Pokusajte ponovo:");
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

                            Console.WriteLine("Unesite dodatni popust na proizvod:");
                            double additionalDiscount;
                            while (true)
                            {
                                string input = Console.ReadLine();
                                input = CheckPercentageInput(input);
                                if (input.Contains(',')) input = input.Replace(',', '.');
                                if (!double.TryParse(input, out additionalDiscount)) Console.WriteLine("Popust mora biti realan broj! Pokusajte ponovo:");
                                else if (additionalDiscount < 0) Console.WriteLine("Popust mora biti broj veci od nule! Pokusajte ponovo:");
                                else if (additionalDiscount >= 100) Console.WriteLine("Greska prilikom unosa popusta! Broj prevelik. Pokusajte ponovo:");
                                else break;
                            }

                            Proizvod product = new Proizvod(name, barkod, price, additionalDiscount);
                            ListOfProducts.Add(product);
                            if(additionalDiscount != 0) ListOfDiscountedUPCs.Add(barkod);

                            IspisProizvoda(product, tax, discount);                         
                        }
                        break;

                    case 2:
                        {
                            if (!ListOfProducts.Any())
                            {
                                Console.WriteLine("Lista proizvoda je prazna. Nema proizvoda za pristupiti. Unesite proizvod pa pokusajte ponovo!");
                                break;
                            }
                            Console.WriteLine("Unesite barkod:");
                            int barkod;
                            while (true)
                            {
                                if (!int.TryParse(Console.ReadLine(), out barkod)) Console.WriteLine("Barkod mora biti ceo broj! Pokusajte ponovo:");
                                else if (barkod < 0) Console.WriteLine("Barkod mora biti ceo broj veci od nule! Pokusajte ponovo:");
                                else if (UPCDoesntExist(ListOfProducts, barkod)) Console.WriteLine("Ne postoji proizvod sa ovim barkodom u bazi podataka! Pokusajte ponovo:");
                                else break;
                            }
                            while(true)
                            {
                                Console.WriteLine("0. Izadji.\n" +
                                "1. Prikazi proizvod.\n" +
                                "2. Promeni dodatni popust.");
                                int flag;
                                while (true)
                                {
                                    if (int.TryParse(Console.ReadLine(), out flag) && (flag == 0 || flag == 1 || flag == 2)) break;
                                    else Console.WriteLine("Greska prilikom unosa! Pokusajte ponovo:");
                                }
                                if (flag == 0) break;
                                else if (flag == 1)
                                {
                                    foreach (Proizvod product in ListOfProducts)
                                    {
                                        if (barkod == product.GetUPC())
                                        {
                                            IspisProizvoda(product, tax, discount);
                                        }
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Dodatni popust?");
                                    double popust;
                                    while (true)
                                    {
                                        string input = Console.ReadLine();
                                        input = CheckPercentageInput(input);
                                        if (input.Contains(',')) input = input.Replace(',', '.');
                                        if (!double.TryParse(input, out popust)) Console.WriteLine("Popust mora biti realan broj! Pokusajte ponovo:");
                                        else if (popust < 0) Console.WriteLine("Popust mora biti broj veci od nule! Pokusajte ponovo:");
                                        else if (popust >= 100) Console.WriteLine("Greska prilikom unosa popusta! Broj prevelik. Pokusajte ponovo:");
                                        else break;
                                    }
                                    foreach (Proizvod proizvod in ListOfProducts)
                                    {
                                        if (barkod == proizvod.GetUPC()) proizvod.SetDiscount(popust);
                                    }

                                    if (popust == 0 && ListOfDiscountedUPCs.Contains(barkod)) ListOfDiscountedUPCs.Remove(barkod);
                                    else ListOfDiscountedUPCs.Add(barkod);

                                }
                            }                              
                        }
                        break;
                    case 3:
                        {
                            if (!ListOfProducts.Any())
                            {
                                Console.WriteLine("Lista proizvoda je prazna. Nema proizvoda za pristupiti. Unesite proizvod pa pokusajte ponovo!");
                                break;
                            }
                            Console.WriteLine("Unesite barkod:");
                            int barkod;
                            while (true)
                            {
                                if (!int.TryParse(Console.ReadLine(), out barkod)) Console.WriteLine("Barkod mora biti ceo broj! Pokusajte ponovo:");
                                else if (barkod < 0) Console.WriteLine("Barkod mora biti ceo broj veci od nule! Pokusajte ponovo:");
                                else if (UPCDoesntExist(ListOfProducts, barkod)) Console.WriteLine("Barkod ne postoji u listi proizvoda! Pokusajte ponovo:");
                                else break;
                            }

                            if(UPCDoesntExist(ListOfDiscountedUPCs, barkod))
                            {
                                Console.WriteLine("Dodatni popust?");
                                double popust;
                                while (true)
                                {
                                    string input = Console.ReadLine();
                                    input = CheckPercentageInput(input);
                                    if (input.Contains(',')) input = input.Replace(',', '.');
                                    if (!double.TryParse(input, out popust)) Console.WriteLine("Popust mora biti realan broj! Pokusajte ponovo:");
                                    else if (popust < 0) Console.WriteLine("Popust mora biti broj veci od nule! Pokusajte ponovo:");
                                    else if (popust >= 100) Console.WriteLine("Greska prilikom unosa popusta! Broj prevelik. Pokusajte ponovo:");
                                    else break;
                                }
                                ListOfDiscountedUPCs.Add(barkod);
                                ChangeDiscount(ListOfProducts, barkod, popust);

                            }
                            else
                            {
                                while (true)
                                {
                                    Console.WriteLine("0. Izadji\n" +
                                   "1. Obrisi UPC iz liste za dodatne popuste.\n" +
                                   "2. Promeni dodatni popust za ovaj UPC (trenutno: " + ListOfProducts.Find(x => x.GetUPC() == barkod).GetDiscount() + "%).");
                                    int flag;
                                    while (true)
                                    {
                                        if (int.TryParse(Console.ReadLine(), out flag) && (flag == 0 || flag == 1 || flag == 2)) break;
                                        else Console.WriteLine("Greska prilikom unosa! Pokusajte ponovo:");
                                    }
                                    if (flag == 0) break;
                                    else if (flag == 1)
                                    {
                                        foreach (Proizvod proizvod in ListOfProducts)
                                        {
                                            if (barkod == proizvod.GetUPC()) proizvod.SetDiscount(0);
                                        }

                                        if (ListOfDiscountedUPCs.Contains(barkod)) ListOfDiscountedUPCs.Remove(barkod);
                                    }
                                    else
                                    {
                                        foreach (Proizvod proizvod in ListOfProducts)
                                        {
                                            if (barkod == proizvod.GetUPC())
                                            {
                                                Console.WriteLine("Dodatni popust?");
                                                double popust;
                                                while (true)
                                                {
                                                    string input = Console.ReadLine();
                                                    input = CheckPercentageInput(input);
                                                    if (input.Contains(',')) input = input.Replace(',', '.');
                                                    if (!double.TryParse(input, out popust)) Console.WriteLine("Popust mora biti realan broj! Pokusajte ponovo:");
                                                    else if (popust < 0) Console.WriteLine("Popust mora biti broj veci od nule! Pokusajte ponovo:");
                                                    else if (popust >= 100) Console.WriteLine("Greska prilikom unosa popusta! Broj prevelik. Pokusajte ponovo:");
                                                    else break;
                                                }
                                                proizvod.SetDiscount(popust);
                                            }
                                        }
                                    }
                                }  
                            }
                        }
                        break;
                    case 4:
                        {
                            Console.WriteLine("Popust?");
                            double temp;
                            while (true)
                            {
                                string input = Console.ReadLine();
                                input = CheckPercentageInput(input);
                                if (input.Contains(',')) input = input.Replace(',', '.');
                                if (!double.TryParse(input, out temp)) Console.WriteLine("Popust mora biti realan broj! Pokusajte ponovo:");
                                else if (temp < 0) Console.WriteLine("Popust mora biti broj veci od nule! Pokusajte ponovo:");
                                else if (temp >= 100) Console.WriteLine("Greska prilikom unosa popusta! Broj prevelik. Pokusajte ponovo:");
                                else
                                {
                                    discount = temp;
                                    break;
                                }
                            }
                        }
                        break;
                    case 5:
                        {
                            Console.WriteLine("PDV?");
                            while (true)
                            {
                                string input = Console.ReadLine();
                                input = CheckPercentageInput(input);
                                if (input.Contains(',')) input = input.Replace(',', '.');
                                if (double.TryParse(input, out tax) && tax > 0)
                                {
                                    //Console.WriteLine("Cena " + product.GetPrice() + " din pre poreza i "
                                    //    + Math.Round(product.GetPrice() * (1 + tax / 100), 2) + " din nakon " + tax + "% poreza.");
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("Pogresan unos! Pokusajte ponovo!");
                                }
                            }
                        }
                        break;
                    case 6:
                        {
                            if (ListOfDiscountedUPCs.Any())
                            {
                                foreach (int i in ListOfDiscountedUPCs)
                                {
                                    Console.WriteLine("Barkod: " + i + " Dodatni popust: " + ListOfProducts.Find(x => x.GetUPC() == i).GetDiscount() + "%");
                                }
                            }
                            else Console.WriteLine("Nema proizvoda sa dodatnim popustom!");
                        }
                        break;
                    default:
                        {
                            Console.WriteLine("Los unos! Pokusajte ponovo:");
                        }
                        break;
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

        static bool UPCDoesntExist(List<Proizvod> lista, int UPC) 
        {
            foreach (Proizvod proizvod in lista)
            {
                if (UPC == proizvod.GetUPC()) return false;
            }
            return true;
        }

        static bool UPCDoesntExist(List<int> lista, int UPC)
        {
            foreach (int barkod in lista)
            {
                if (UPC == barkod) return false;
            }
            return true;
        }
        static void ChangeDiscount(List<Proizvod> lista, int barkod, double popust)
        {
            foreach (Proizvod proizvod in lista)
            {
                if(barkod == proizvod.GetUPC())
                {
                    proizvod.SetDiscount(popust);
                }    
            }    
        
        }
        static  void IspisProizvoda(Proizvod product, double tax, double discount)
        {
            if (discount == 0 && product.GetDiscount() == 0)
            {
                Console.WriteLine("Porez = " + tax + "%, Nema popusta.\n" +
                    "Iznos poreza = " + product.IznosPoreza(tax) + " din\n" +
                    "Osnovna cena = " + product.GetPrice() + " din. Cena nakon poreza = " +
                    (product.GetPrice() + product.IznosPoreza(tax) - product.IznosPopusta(discount)));
            }
            else if (discount != 0 && product.GetDiscount() != 0)
            {
                Console.WriteLine("Porez = " + tax + "%, Popust = " + discount + "%, Dodatni popust = " + product.GetDiscount() + "%\n" +
                "Iznos poreza = " + product.IznosPoreza(tax) + " din; Iznos popusta = " + product.IznosPopusta(discount) + " din; " +
                "Iznos dodatnog popusta = " + product.IznosPopusta(product.GetDiscount()) + " din\n" +
                "Osnovna cena = " + product.GetPrice() + " din. Cena nakon poreza i popusta = "
                + (product.GetPrice() + product.IznosPoreza(tax) - product.IznosPopusta(discount) - product.IznosPopusta(product.GetDiscount())));

            }
            else if (discount == 0 && product.GetDiscount() != 0)
            {
                Console.WriteLine("Porez = " + tax + "%, Dodatni popust = " + product.GetDiscount() + "%\n" +
                  "Iznos poreza = " + product.IznosPoreza(tax) + " din; " +
                  "Iznos dodatnog popusta = " + product.IznosPopusta(product.GetDiscount()) + " din\n" +
                  "Osnovna cena = " + product.GetPrice() + " din. Cena nakon poreza i popusta = "
                  + (product.GetPrice() + product.IznosPoreza(tax) - product.IznosPopusta(product.GetDiscount())));
            }
            else
            {
                Console.WriteLine("Porez = " + tax + "%, Popust = " + discount + "%\n" +
                  "Iznos poreza = " + product.IznosPoreza(tax) + " din; Iznos popusta = " + product.IznosPopusta(discount) + " din\n" +
                  "Osnovna cena = " + product.GetPrice() + " din. Cena nakon poreza i popusta = "
                  + (product.GetPrice() + product.IznosPoreza(tax) - product.IznosPopusta(discount)));

            }
        }
    }
}