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

            algo_type.Items.Add("Iterativ");
            algo_type.Items.Add("Rekursiv");
            algo_type.SelectedItem = "Iterativ";
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
                //setPixelThreaded(Color, p.X, p.Y);

                if (algo_type.SelectedItem.ToString() == "Rekursiv")
                {
                    // man kann auch ein object beim start übergeben, klasse mit allen variablen
                    // drinnen machen
                    ThreadStart ts = new ThreadStart(() =>
                    {
                        fill4_rec((int)p.X, (int)p.Y, old, _new);
                    });
                    Thread fill4_re = new Thread(ts, 50000000);
                    fill4_re.Start();
                }
                else if (algo_type.SelectedItem.ToString() == "Iterativ")
                {
                    fill4_it((int)p.X, (int)p.Y, _new);
                }
                else
                {
                    return;
                }
                    
            }
        }

        private void fill4_rec(int x, int y, Color old, Color neu)
        {
            if (getPixelThreaded(x, y) == old)
            {
                setPixelThreaded(neu, x, y);
                fill4_rec(x, y + 1, old, neu);
                fill4_rec(x, y - 1, old, neu);
                fill4_rec(x + 1, y, old, neu);
                fill4_rec(x - 1, y, old, neu);
            }
            else
            {
                return;
            }
        }

        private bool ColorMatch(Color replacementColor, Color targetColor)
        {
            return replacementColor == targetColor;
        }
        private void fill4_it(int x, int y, Color _new)
        {
            Color targetColor = getPixelThreaded(x,y);
            Queue<Point> queue = new Queue<Point>();
            queue.Enqueue(new Point(x, y));

            while (queue.Count != 0)
            {
                Point point1 = queue.Dequeue();

                if(!ColorMatch(getPixelThreaded(point1.X, point1.Y), targetColor))
                {
                    continue;
                }

                Point point2 = new Point(point1.X + 1, point1.Y);

                while(point1.X >= 0 && ColorMatch(getPixelThreaded(point1.X, point1.Y), targetColor))
                {
                    setPixelThreaded(_new, point1.X, point1.Y);

                    if (point1.Y > 0 && ColorMatch(getPixelThreaded(point1.X, point1.Y -1), targetColor))
                    {
                        queue.Enqueue(new Point(point1.X, point1.Y - 1));
                    }
                    if (point1.Y < imageHeigth - 1 && ColorMatch(getPixelThreaded(point1.X, point1.Y + 1), targetColor))
                    {
                        queue.Enqueue(new Point(point1.X, point1.Y + 1));
                    }
                    point1.X--;
                }

                while(point2.X <= imageWidth - 1 && ColorMatch(getPixelThreaded(point2.X, point2.Y), targetColor))
                {
                    setPixelThreaded(_new, point2.X, point2.Y);
                    if (point2.Y > 0 && ColorMatch(getPixelThreaded(point2.X, point2.Y - 1), targetColor))
                    {
                        queue.Enqueue(new Point(point2.X, point2.Y - 1));

                    }
                    if (point2.Y <  imageHeigth - 1 && ColorMatch(getPixelThreaded(point2.X, point2.Y + 1), targetColor))
                    {
                        queue.Enqueue(new Point(point2.X, point2.Y + 1));
                    }
                    point2.X++;
                }
            }
        }

        
    }
}
