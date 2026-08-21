using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Rechenquiz
{
    internal class Quiz
    {
        public string Name { get; set; }

        public List<Calculation> calculations = new List<Calculation>();

        public Quiz(string name, List<Calculation> calculations)
        {
            Name = name;
            this.calculations = calculations;
        }

        public void AddCalculation(Calculation calculation)
        {
            calculations.Add(calculation);
        }

        public void RemoveCalculation(Calculation calculation)
        {
            calculations.Remove(calculation);
        }
    }
}
