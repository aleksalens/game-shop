using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameshop
{
    class Proizvod
    {
        string name;
        int UPC; //Bar kod
        double price;
        double discount;
        double ambalaza;
        double isporuka;
        double administrativni_troskovi;

        public Proizvod(string name, int uPC, double price, double discount = 0, double ambalaza = 0, double isporuka = 0, double administrativni_troskovi = 0)
        {
            this.name = name;
            this.UPC = uPC;
            this.price = price;
            this.discount = discount;
            this.ambalaza = ambalaza;
            this.isporuka = isporuka;
            this.administrativni_troskovi = administrativni_troskovi;
        }

        public string GetName() { return name; }
        public int GetUPC() { return UPC; }
        public double GetPrice() { return price; }
        public void SetName(string name) { this.name = name; }
        public void SetUPC(int UPC) { this.UPC = UPC; }
        public void SetPrice(double price) { this.price = price; }
        public double GetDiscount() { return discount; }
        public void SetDiscount(double discount) { this.discount = discount; }
        public double GetAmbalaza() { return ambalaza; }
        public double GetIsporuka() { return isporuka; }
        public double GetAdminTroskovi() { return administrativni_troskovi; }
        public void SetAmbalaza(double ambalaza) { this.ambalaza = ambalaza; }
        public void SetIsporuka(double isporuka) { this.isporuka = isporuka; }
        public void SetAdminTroskovi(double drugi_troskovi) { this.administrativni_troskovi = drugi_troskovi; }
        public double IznosPoreza(double tax)
        {
            return Math.Round(price * tax / 100, 2);
        }
        public double IznosPopusta(double discount)
        {
            return Math.Round(price * discount / 100, 2);
        }
    }
}