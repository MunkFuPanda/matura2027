using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WPF_Rechenquiz
{
    public enum ArithOperators
    {
        Add,
        Sub,
        Mul,
        Div,
        Empty

    }

    internal class Calculation : INotifyPropertyChanged
    {
        private int _number1;
        private int _number2;
        private ArithOperators _arithOperator;

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public int Number1
        {
            get => _number1;
            set
            {
                if (_number1 != value)
                {
                    _number1 = value;
                    OnPropertyChanged();
                }
            }
        }

        public int Number2
        {
            get => _number2;
            set
            {
                if (_number2 != value)
                {
                    _number2 = value;
                    OnPropertyChanged();
                }
            }
        }

        public ArithOperators ArithOperator
        {
            get => _arithOperator;
            set
            {
                if (_arithOperator != value)
                {
                    _arithOperator = value;
                    OnPropertyChanged();
                }
            }
        }

        public Calculation()
        {

        }

        public Calculation(int number1, int number2, ArithOperators arithOperator)
        {
            _number1 = number1;
            _number2 = number2;
            _arithOperator = arithOperator;
        }

        public double? CalcResult() {
            if (Number1 == null || Number2 == null || _arithOperator == null)
            {
                return null; 
            } 
            
            switch (_arithOperator) 
            { 
                case ArithOperators.Add: 
                    return Number1 + Number2; 
                    break; 
                case ArithOperators.Sub: 
                    return Number1 - Number2; 
                    break; 
                case ArithOperators.Mul: 
                    return Number1 * Number2; 
                    break; 
                case ArithOperators.Div: 
                    return Number1 / Number2; 
                    break; 
                default: 
                    return null; 
                    break; 
            } 
        }
    }


    public class StatusSymbolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return value switch
            {
                ArithOperators.Add => "+",
                ArithOperators.Sub => "-",
                ArithOperators.Mul => "*",
                ArithOperators.Div => "/",
                ArithOperators.Empty => "",
                _ => ""
            };
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return value switch
            {
                "+" => ArithOperators.Add,
                "-" => ArithOperators.Sub,
                "*" => ArithOperators.Mul,
                "/" => ArithOperators.Div,
                "" => ArithOperators.Empty,
                _ => ArithOperators.Empty
            };
        } 
    }
}
