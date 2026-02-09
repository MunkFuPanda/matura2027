using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PixelDraw_2024 {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private static readonly int imageSize = 300;
        private static WriteableBitmap _wb;
        private static int _bytesPerPixel;
        private static int _stride;
        private static byte[] _colorArray;

        public MainWindow() {
            InitializeComponent();
            _wb = new WriteableBitmap(imageSize, imageSize, 96, 96, PixelFormats.Bgra32, null);
            _bytesPerPixel = (_wb.Format.BitsPerPixel + 7) / 8;
            _stride = _wb.PixelWidth * _bytesPerPixel;
            _colorArray = ConvertColor(Colors.Black);
            drawing.Source = _wb;
        }

        #region Hilfsfunktionen

        private static byte[] ConvertColor(Color color) {
            byte[] c = new byte[4];
            c[0] = color.B;
            c[1] = color.G;
            c[2] = color.R;
            c[3] = color.A;
            return c;
        }

        private static Color ConvertColor(byte[] color) {
            Color c = new Color();
            c.B = color[0];
            c.G = color[1];
            c.R = color[2];
            c.A = color[3];
            return c;
        }

        private void setPixel(Color c, int x, int y) {
            if (x < _wb.PixelWidth && x > 0 && y < _wb.PixelHeight && y > 0) {
                _wb.WritePixels(new Int32Rect(x, y, 1, 1), ConvertColor(c), _stride, 0);
            }
        }

        private void setPixel(int x, int y) {
            if (x < _wb.PixelWidth && x > 0 && y < _wb.PixelHeight && y > 0) {
                _wb.WritePixels(new Int32Rect(x, y, 1, 1), _colorArray, _stride, 0);
            }
        }

        #endregion

        private void button1_Click(object sender, RoutedEventArgs e) {
            for (int i = 10; i <= 290; i++) {
                setPixel(i, 10);
                setPixel(i, 290);
                setPixel(10, i);
                setPixel(290, i);
            }
            for (int i = 10; i <= 290; i += 20) {
                drawLine(150, 150, 10, i);
                drawLine(150, 150, 290, i);
                drawLine(150, 150, i, 10);
                drawLine(150, 150, i, 290);
            }
        }

        private void drawLine(int x1, int y1, int x2, int y2) {
            int dx = Math.Abs(x2 - x1);
            int dy = Math.Abs(y2 - y1);
            int sx = x1 < x2 ? 1 : -1;
            int sy = y1 < y2 ? 1 : -1;
            int err = dx - dy;

            while (true) {
                setPixel(x1, y1);
                if (x1 == x2 && y1 == y2) break;
                int err2 = 2 * err;
                if (err2 > -dy) {
                    err -= dy;
                    x1 += sx;
                }
                if (err2 < dx) {
                    err += dx;
                    y1 += sy;
                }
            }
        }
    }
}