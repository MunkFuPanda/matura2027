using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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

        private static bool ColorMatch(Color replacementColor, Color targetColor)
        {
            return targetColor.Equals(replacementColor);
        }

        private void drawing_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //Beispiel LeftButtonDown auf Image mit Umrechnung in Pixelkoordinaten
            Point p = e.GetPosition(drawing);
            p.X = p.X * imageWidth / drawing.ActualWidth;
            p.Y = p.Y * imageHeigth / drawing.ActualHeight;

            Color old = getPixel((int)p.X, (int)p.Y);
            Color _new = Color;
            if (!ColorMatch(old, _new))
            {

                Queue<Point> queue = new Queue<Point>(); // Warteschlange der Pixel für die Breitensuche
                queue.Enqueue(p); // Fügt das Startpixel der Warteschlange hinzu

                while (queue.Count != 0) // So lange die Warteschlange nicht leer ist
                {
                    Point point1 = queue.Dequeue(); // Entfernt das erste Pixel aus der Warteschlange
                    if (!ColorMatch(getPixel(point1.X, point1.Y), old)) // Wenn die Farbe des aktuellen Pixels gleich dem Startpixel ist, wird die nächste Iteration der äußeren while-Schleife ausgeführt
                    {
                        continue;
                    }
                    Point point2 = new Point(point1.X + 1, point1.Y); // Speichert das Pixel rechts vom aktuellen Pixel
                    while (point1.X >= 0 && ColorMatch(getPixel(point1.X, point1.Y), old)) // So lange das aktuelle Pixel nicht links vom Rand ist und die Farbe des Startpixels hat
                    {
                        setPixel(_new, point1.X, point1.Y); // Setzt das aktuelle Pixel auf die neue Farbe
                        if (point1.Y > 0 && ColorMatch(getPixel(point1.X, point1.Y - 1), old)) // Wenn das aktuelle Pixel nicht am linken Rand ist und die Farbe des Startpixels hat
                        {
                            queue.Enqueue(new Point(point1.X, point1.Y - 1)); // Fügt das Pixel über dem aktuellen Pixel der Warteschlange hinzu
                        }
                        if (point1.Y < imageHeigth - 1 && ColorMatch(getPixel(point1.X, point1.Y + 1), old)) // Wenn das aktuelle Pixel nicht am rechten Rand ist und die Farbe des Startpixels hat
                        {
                            queue.Enqueue(new Point(point1.X, point1.Y + 1)); // Fügt das Pixel unter dem aktuellen Pixel der Warteschlange hinzu
                        }
                        point1.X--; // Verschiebt das aktuelle Pixel um 1 nach links
                    }
                    // Die folgende while-Schleife wiederholt den Ablauf mit dem Pixel rechts vom aktuellen Pixel
                    while (point2.X <= imageWidth - 1 && ColorMatch(getPixel(point2.X, point2.Y), old))
                    {
                        setPixel(_new, point2.X, point2.Y);
                        if (point2.Y > 0 && ColorMatch(getPixel(point2.X, point2.Y - 1), old))
                        {
                            queue.Enqueue(new Point(point2.X, point2.Y - 1));
                        }
                        if (point2.Y < imageHeigth - 1 && ColorMatch(getPixel(point2.X, point2.Y + 1), old))
                        {
                            queue.Enqueue(new Point(point2.X, point2.Y + 1));
                        }
                        point2.X++; // Verschiebt das aktuelle Pixel um 1 nach rechts
                    }
                }
            }
        }
    }
}
