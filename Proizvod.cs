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

        public Proizvod(string name, int uPC, double price)
        {
            this.name = name;
            this.UPC = uPC;
            this.price = price;
        }

        public string GetName() { return name;}
        public int GetUPC() { return UPC; }
        public double GetPrice() { return price; }
        public void SetName(string name) { this.name = name; }
        public void SetUPC(int UPC) { this.UPC = UPC; }
        public void SetPrice(double price) { this.price = price; }
    }
}
