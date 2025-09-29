using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;

namespace PixelDraw
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly int imageWidth = 390;
        private static readonly int imageHeigth = 500;
        

        public Color Color { get; set; } = Colors.Black;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            Clean();
        }

        #region Hilfsfunktionen

        private static WriteableBitmap _wb;
        private static int _bytesPerPixel;
        private static int _stride;
        private static byte[] _colorArray;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Color = ((SolidColorBrush)((Button)sender).Background).Color;
        }

        private static byte[] ConvertColor(Color color)
        {
            byte[] c = new byte[4];
            c[0] = color.B;
            c[1] = color.G;
            c[2] = color.R;
            c[3] = color.A;
            return c;
        }

        private static Color ConvertColor(byte[] color)
        {
            Color c = new Color();
            c.B = color[0];
            c.G = color[1];
            c.R = color[2];
            c.A = color[3];
            return c;
        }

        private void setPixel(Color c, double x, double y)
        {
            if (x < _wb.PixelWidth && x > 0 && y < _wb.PixelHeight && y > 0)
            {
                _wb.WritePixels(new Int32Rect((int)x, (int)y, 1, 1), ConvertColor(c), _stride, 0);
            }
        }

        private void setPixel(double x, double y)
        {
            if (x < _wb.PixelWidth && x > 0 && y < _wb.PixelHeight && y > 0)
            {
                _wb.WritePixels(new Int32Rect((int)x, (int)y, 1, 1), _colorArray, _stride, 0);
            }
        }

        private static byte[] _readArray = ConvertColor(Colors.Black);

        private void setPixelThreaded(Color c, double x, double y)
        {
            _wb.Dispatcher.Invoke(new Action(() =>
            {
                if (x < _wb.PixelWidth && x > 0 && y < _wb.PixelHeight && y > 0)
                {
                    _wb.WritePixels(new Int32Rect((int)x, (int)y, 1, 1), ConvertColor(c), _stride, 0);
                }
            }));

        }

        private Color getPixelThreaded(double x, double y)
        {
            Color res = Colors.Transparent;
            _wb.Dispatcher.Invoke(new Action(() =>
            {
                if (x < _wb.PixelWidth && x > 0 && y < _wb.PixelHeight && y > 0)
                {
                    _wb.CopyPixels(new Int32Rect((int)x, (int)y, 1, 1), _readArray, _stride, 0);
                    res = ConvertColor(_readArray);
                }
            }));
            return res;
        }

        private Color getPixel(double x, double y)
        {
            Color res = Colors.Transparent;
            if (x < _wb.PixelWidth && x > 0 && y < _wb.PixelHeight && y > 0)
            {
                _wb.CopyPixels(new Int32Rect((int)x, (int)y, 1, 1), _readArray, _stride, 0);
                res = ConvertColor(_readArray);
            }

            return res;
        }


        private void Clean()
        {
            BitmapImage bitmap = new BitmapImage(new Uri("pack://application:,,,/Background.png"));
            //_wb = new WriteableBitmap(imageWidth, imageHeigth, 96, 96, PixelFormats.Bgra32, null);
            _wb = new WriteableBitmap(bitmap);
            _bytesPerPixel = (_wb.Format.BitsPerPixel + 7) / 8;
            _stride = _wb.PixelWidth * _bytesPerPixel;
            _colorArray = ConvertColor(Colors.Black);
            drawing.Source = _wb;
        }

        #endregion


        private void drawing_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //Beispiel LeftButtonDown auf Image mit Umrechnung in Pixelkoordinaten
            Point p = e.GetPosition(drawing);
            p.X = p.X * imageWidth / drawing.ActualWidth;
            p.Y = p.Y * imageHeigth / drawing.ActualHeight;

            Color old = getPixel((int)p.X, (int)p.Y);
            Color _new = Color;
            if (!old.Equals(_new))
            {
                setPixelThreaded(Color, p.X, p.Y);  
            }
        }

        
    }
}
