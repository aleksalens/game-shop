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
                Start:
                Console.WriteLine("0. Zatvori program.\n" +
                    "1. Dodaj novi proizvod.\n" +
                    "2. Pristupi proizvodu.\n" +
                    "3. Promeni listu UPC za dodatni popust.\n" +
                    "4. Promeni popust na kasi (trenutno: " + discount + "%).\n" +
                    "5. Promeni PDV (trenutno: " + tax + "%).\n" +
                    "6. Ispisi sve proizvode sa dodatnim popustom.");

                int option = FlagCheck();

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
                            int barkod = BarcodeCheck(ListOfProducts);


                            Console.WriteLine("Unesite cenu proizvoda:");
                            double price = UnosDinara(Console.ReadLine());

                            Console.WriteLine("Unesite dodatni popust na proizvod:");
                            double additionalDiscount = UnosProcenta(Console.ReadLine());                           

                            Proizvod product = new Proizvod(name, barkod, price, additionalDiscount);
                            ListOfProducts.Add(product);
                            if (additionalDiscount != 0) ListOfDiscountedUPCs.Add(barkod);

                            DodatniTroskovi(product);

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

                                string input = Console.ReadLine();
                                if (input == "exit") goto Start;
                                else if (!int.TryParse(input, out barkod)) Console.WriteLine("Barkod mora biti ceo broj! Pokusajte ponovo:");
                                else if (barkod < 0) Console.WriteLine("Barkod mora biti ceo broj veci od nule! Pokusajte ponovo:");
                                else if (UPCDoesntExist(ListOfProducts, barkod)) Console.WriteLine("Ne postoji proizvod sa ovim barkodom u bazi podataka! Pokusajte ponovo:");
                                else break;
                            }
                            while (true)
                            {
                                Console.WriteLine("0. Izadji.\n" +
                                "1. Prikazi proizvod.\n" +
                                "2. Promeni dodatni popust (trenutno: " + ListOfProducts.Find(x => x.GetUPC() == barkod).GetDiscount() + "%).\n" +
                                "3. Promeni iznos dodatnih troskova.");

                                int flag = FlagCheck(3);

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
                                else if(flag == 2)
                                {
                                    Console.WriteLine("Dodatni popust?");
                                    double popust = UnosProcenta(Console.ReadLine());
                                    
                                    foreach (Proizvod proizvod in ListOfProducts)
                                    {
                                        if (barkod == proizvod.GetUPC()) proizvod.SetDiscount(popust);
                                    }

                                    if (popust == 0 && ListOfDiscountedUPCs.Contains(barkod)) ListOfDiscountedUPCs.Remove(barkod);
                                    else if (popust != 0 && !ListOfDiscountedUPCs.Contains(barkod)) ListOfDiscountedUPCs.Add(barkod);
                                    
                                }
                                else
                                {
                                    DodatniTroskovi(ListOfProducts.Find(x => x.GetUPC() == barkod));
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
                                string input = Console.ReadLine();
                                if (input == "exit") goto Start;
                                else if (!int.TryParse(input, out barkod)) Console.WriteLine("Barkod mora biti ceo broj! Pokusajte ponovo:");
                                else if (barkod < 0) Console.WriteLine("Barkod mora biti ceo broj veci od nule! Pokusajte ponovo:");
                                else if (UPCDoesntExist(ListOfProducts, barkod)) Console.WriteLine("Barkod ne postoji u listi proizvoda! Pokusajte ponovo:");
                                else break;
                            }

                            if (UPCDoesntExist(ListOfDiscountedUPCs, barkod))
                            {
                                Console.WriteLine("Dodatni popust?");
                                double popust = UnosProcenta(Console.ReadLine());
                               
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
                                    int flag = FlagCheck(2);
                                   
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
                                                double popust = UnosProcenta(Console.ReadLine());

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
                            tax = UnosProcenta(Console.ReadLine());
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
                if (barkod == proizvod.GetUPC())
                {
                    proizvod.SetDiscount(popust);
                }
            }

        }
        static void IspisProizvoda(Proizvod product, double tax, double discount)
        {

           

            if(discount == 0 && product.GetDiscount() == 0 && (product.GetAdminTroskovi() == 0 
                && product.GetAmbalaza() == 0 && product.GetIsporuka() == 0))
            {
                Console.WriteLine("Osnovna cena = " + product.GetPrice() + " din\n" +
                     "Porez = " + product.IznosPoreza(tax) + " din\n" +
                     "Nema dodatnih popusta\n" +
                     "Nema dodatnih troskova\n" +
                     "UKUPNO = " + Math.Round(product.GetPrice() + product.IznosPoreza(tax) + product.GetAmbalaza() + product.GetIsporuka() +
                     product.GetAdminTroskovi() - product.IznosPopusta(discount) - product.IznosPopusta(product.GetDiscount()), 2) + " din\n");
            }
            else if (discount == 0 && product.GetDiscount() == 0)
            {
                Console.WriteLine("Osnovna cena = " + product.GetPrice() + " din\n" +
                    "Porez = " + product.IznosPoreza(tax) + " din\n" +
                    "Nema dodatnih popusta\n" +
                    "Ambalaza = " + product.GetAmbalaza() + " din\n" +
                    "Isporuka = " + product.GetIsporuka() + " din\n" +
                    "Administrativni troskovi = " + product.GetAdminTroskovi() + " din\n" +
                    "UKUPNO = " + Math.Round(product.GetPrice() + product.IznosPoreza(tax) + product.GetAmbalaza() + product.GetIsporuka() +
                    product.GetAdminTroskovi() - product.IznosPopusta(discount) - product.IznosPopusta(product.GetDiscount()), 2) + " din\n");
            }
            else if (product.GetAdminTroskovi() == 0 && product.GetAmbalaza() == 0 && product.GetIsporuka() == 0)
            {
                Console.WriteLine("Osnovna cena = " + product.GetPrice() + " din\n" +
                    "Porez = " + product.IznosPoreza(tax) + " din\n" +
                    "Popusti = " + (product.IznosPopusta(discount) + product.IznosPopusta(product.GetDiscount())) + "din\n" +
                    "Nema dodatnih troskova\n" +
                    "UKUPNO = " + Math.Round(product.GetPrice() + product.IznosPoreza(tax) + product.GetAmbalaza() + product.GetIsporuka() +
                    product.GetAdminTroskovi() - product.IznosPopusta(discount) - product.IznosPopusta(product.GetDiscount()), 2) + " din\n");
            }
            else
            {
                Console.WriteLine("Osnovna cena = " + product.GetPrice() + " din\n" +
                    "Porez = " + product.IznosPoreza(tax) + " din\n" +
                    "Popusti = " + (product.IznosPopusta(discount) + product.IznosPopusta(product.GetDiscount())) + "din\n" +
                    "Ambalaza = " + product.GetAmbalaza() + " din\n" +
                    "Isporuka = " + product.GetIsporuka() + " din\n" +
                    "Administrativni troskovi = " + product.GetAdminTroskovi() + " din\n" +
                    "UKUPNO = " + Math.Round(product.GetPrice() + product.IznosPoreza(tax) + product.GetAmbalaza() + product.GetIsporuka() +
                    product.GetAdminTroskovi() - product.IznosPopusta(discount) - product.IznosPopusta(product.GetDiscount()), 2) + " din\n");
            }


            /*
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

            }*/
        }

        static int BarcodeCheck(List<Proizvod> lista)
        {
            int barkod;
            while (true)
            {
                if (!int.TryParse(Console.ReadLine(), out barkod)) Console.WriteLine("Barkod mora biti ceo broj! Pokusajte ponovo:");
                else if (barkod < 0) Console.WriteLine("Barkod mora biti ceo broj veci od nule! Pokusajte ponovo:");
                else if (!UPCDoesntExist(lista, barkod)) Console.WriteLine("Barkod vec postoji u sistemu! Pokusajte ponovo:");
                else break;

            }
            return barkod;
        }

        static double UnosDinara(string input)
        {
            double din;
            while (true)
            {
                if (input.Contains(',')) input = input.Replace(',', '.');
                if (!double.TryParse(input, out din))
                {
                    Console.WriteLine("Unos mora biti realan broj! Pokusajte ponovo:");
                    input = Console.ReadLine();
                }
                else if (din <= 0)
                {
                    Console.WriteLine("Unos mora biti broj veci od nule! Pokusajte ponovo:");
                    input = Console.ReadLine();
                }
                else break;
            }
            return din;
        }
        static double UnosProcenta(string input)
        {
            double percent;
            while (true)
            {
                input = CheckPercentageInput(input);
                if (input.Contains(',')) input = input.Replace(',', '.');
                if (!double.TryParse(input, out percent))
                {
                    Console.WriteLine("Unos mora biti realan broj! Pokusajte ponovo:");
                    input = Console.ReadLine();
                }
                else if (percent < 0)
                {
                    Console.WriteLine("Unos mora biti broj veci od nule! Pokusajte ponovo:");
                    input = Console.ReadLine();
                }
                else if (percent >= 100)
                {
                    Console.WriteLine("Greska prilikom unosa popusta! Broj prevelik. Pokusajte ponovo:");
                    input = Console.ReadLine();
                }
                else break;
            }
            return percent;
        }
        static int FlagCheck(int limit = 99)
        {
            int flag;
            while (true)
            {
                if (!int.TryParse(Console.ReadLine(), out flag) || flag < 0 || flag > limit)
                {
                    Console.WriteLine("Greska prilikom unosa! Pokusajte ponovo:");
                    continue;
                }
                else break;
            }
            return flag;
        }

        static void DodatniTroskovi(Proizvod proizvod)
        {
            string input;
            Console.WriteLine("Troskovi ambalaziranja:");
            double ambalaza;

            input = Console.ReadLine();

            if (input.Contains('%'))
            {
                if ((input == "0%") || (input == "0 %")) ambalaza = 0;
                else ambalaza = UnosProcenta(input);
                proizvod.SetAmbalaza(Math.Round(proizvod.GetPrice() * ambalaza / 100, 2));
            }
            else
            {
                if (input == "0") ambalaza = 0;
                else ambalaza = UnosDinara(input);
                proizvod.SetAmbalaza(ambalaza);
            }

            Console.WriteLine("Troskovi isporuke:");
            double isporuka;

            input = Console.ReadLine();

            if (input.Contains('%'))
            {
                if ((input == "0%") || (input == "0 %")) isporuka = 0;
                else isporuka = UnosProcenta(input);
                proizvod.SetIsporuka(Math.Round(proizvod.GetPrice() * isporuka / 100, 2));
            }
            else
            {
                if (input == "0") isporuka = 0;
                else isporuka = UnosDinara(input);
                proizvod.SetIsporuka(isporuka);
            }

            Console.WriteLine("Administrativni troskovi:");
            double administrativni_troskovi;

            input = Console.ReadLine();

            if (input.Contains('%'))
            {
                if ((input == "0%") || (input == "0 %")) administrativni_troskovi = 0;
                else administrativni_troskovi = UnosProcenta(input);
                proizvod.SetAdminTroskovi(Math.Round(proizvod.GetPrice() * administrativni_troskovi / 100, 2));
            }
            else
            {
                if (input == "0") administrativni_troskovi = 0;
                else administrativni_troskovi = UnosDinara(input);
                proizvod.SetAdminTroskovi(administrativni_troskovi);
            }
        }

    }
}