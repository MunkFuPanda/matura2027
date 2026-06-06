using LINQtoCSV;
using System;

namespace WPF_Einkaufsliste
{
    public class Product
    {
        [CsvColumn(FieldIndex = 1)]
        public string Kategorie { get; set; }

        [CsvColumn(FieldIndex = 2)]
        public String Name { get; set; }
    }
}