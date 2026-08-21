using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Einkauflistengenerator
{
    public class Einkaufsliste
    {
        List<Product> products;

        public Einkaufsliste() { }

        public Einkaufsliste(List<Product> products)
        {
            this.products = products;
        }


    }
}
