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

        public Proizvod(string name, int uPC, double price, double discount = 0)
        {
            this.name = name;
            this.UPC = uPC;
            this.price = price;
            this.discount = discount;
        }

        public string GetName() { return name;}
        public int GetUPC() { return UPC; }
        public double GetPrice() { return price; }
        public void SetName(string name) { this.name = name; }
        public void SetUPC(int UPC) { this.UPC = UPC; }
        public void SetPrice(double price) { this.price = price; }
        public double GetDiscount() { return discount; }
        public void SetDiscount(double discount) { this.discount = discount; }
        public double IznosPoreza(double tax)
        {
            return Math.Round(price * tax / 100, 2);
        }
        public double IznosPopusta()
        {
            return Math.Round(price * discount / 100, 2);
        }
    }
}
