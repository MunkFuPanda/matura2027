using Microsoft.VisualBasic;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.DirectoryServices.ActiveDirectory;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace PixelDraw_2024
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly int imageSize = 300;
        private static WriteableBitmap _wb;
        private static int _bytesPerPixel;
        private static int _stride;
        private static byte[] _colorArray;

        private static List<Point> pointlist = new List<Point>();

        private static FormsList formslist = new FormsList();
        private static FormsList redo = new FormsList();

        public MainWindow()
        {
            InitializeComponent();
            _wb = new WriteableBitmap(imageSize, imageSize, 96, 96, PixelFormats.Bgra32, null);
            _bytesPerPixel = (_wb.Format.BitsPerPixel + 7) / 8;
            _stride = _wb.PixelWidth * _bytesPerPixel;
            _colorArray = ConvertColor(Colors.Black);
            drawing.Source = _wb;
        }

        #region Hilfsfunktionen

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

        private void setPixel(Color c, int x, int y)
        {
            if (x < _wb.PixelWidth && x > 0 && y < _wb.PixelHeight && y > 0)
            {
                _wb.WritePixels(new Int32Rect(x, y, 1, 1), ConvertColor(c), _stride, 0);
            }
        }

        private void setPixel(int x, int y)
        {
            if (x < _wb.PixelWidth && x > 0 && y < _wb.PixelHeight && y > 0)
            {
                _wb.WritePixels(new Int32Rect(x, y, 1, 1), _colorArray, _stride, 0);
            }
        }


        #endregion

        private void button1_Click(object sender, RoutedEventArgs e)
        {

            Point[] points = new Point[]
    {
            new Point(50, 100),  // Punkt 1
            new Point(100, 150), // Punkt 2
            new Point(150, 100), // Punkt 3
            new Point(200, 200), // Punkt 4
            new Point(250, 50),  // Punkt 5
            new Point(300, 100)  // Punkt 6

    };
            setPixel(50, 100);
            setPixel(100, 150);
            setPixel(150, 100);
            setPixel(200, 200);
            setPixel(250, 50);
            setPixel(300, 100);


    
            for (int i = 10; i <= 290; i++)
            {
                setPixel(i, 10);
                setPixel(i, 290);
                setPixel(10, i);
                setPixel(290, i);
            }
            for (int i = 10; i <= 290; i += 20)
            {
                drawLine(150, 150, 10, i);
                drawLine(150, 150, 290, i);
                drawLine(150, 150, i, 10);
                drawLine(150, 150, i, 290);
            }

            drawCircle(150, 150, 100);
            drawCurves(points, 2000);
        }

        private void drawLine(int x1, int y1, int x2, int y2)
        {

            formslist.forms.Add(new Form(1, new List<Point>() { new Point(x1, y1), new Point(x2, y2) }, -1, -1));

            int dx = Math.Abs(x2 - x1);
            int dy = Math.Abs(y2 - y1);
            int sx = x1 < x2 ? 1 : -1;
            int sy = y1 < y2 ? 1 : -1;
            int err = dx - dy;

            while (true)
            {
                setPixel(x1, y1);

                if (x1 == x2 && y1 == y2)
                    break;

                int e2 = err * 2;

                if (e2 > -dy)
                {
                    err -= dy;
                    x1 += sx;
                }

                if (e2 < dx)
                {
                    err += dx;
                    y1 += sy;
                }
            }
        }

        private void drawCircle(int x1, int y1, int radius)
        {

            formslist.forms.Add(new Form(2, new List<Point>() { new Point(x1, y1)}, radius, -1));

            int x = 0;
            int y = radius;
            int p = 3 - 2 * radius;


            while (x <= y)
            {

                setPixel(x1 + x, y1 - y);
                setPixel(x1 - x, y1 - y);
                setPixel(x1 + x, y1 + y);
                setPixel(x1 - x, y1 + y);
                setPixel(x1 + y, y1 - x);
                setPixel(x1 - y, y1 - x);
                setPixel(x1 + y, y1 + x);
                setPixel(x1 - y, y1 + x);

                x++;

                if (p <= 0)
                {
                    p = p + 4 * x + 6;
                }
                else
                {
                    y--;
                    p = p + 4 * (x - y) + 10;
                }
            }
        }

        public void drawCurves(Point[] points, int count)
        {
            formslist.forms.Add(new Form(3, points.ToList(), -1, count));

            List<double> xs = new List<double>();
            List<double> ys = new List<double>();

            foreach (Point p in points)
            {
                xs.Add(p.X);
                ys.Add(p.Y);
            }

            (double[] xs2, double[] ys2) = Cubic.InterpolateXY(xs.ToArray(), ys.ToArray(), count);

            for (int i = 0; i < xs2.Length; i++)
            {
                setPixel((int)Math.Round(xs2[i]), (int)Math.Round(ys2[i]));
            }
        }

        private void drawing_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (cb_selection.SelectedIndex == 0)
            {
                
                Point p = e.GetPosition(drawing);

                
                double bmpW = _wb.PixelWidth;
                double bmpH = _wb.PixelHeight;

                
                double ctrlW = drawing.ActualWidth;
                double ctrlH = drawing.ActualHeight;

                
                double bmpAspect = bmpW / bmpH;
                double ctrlAspect = ctrlW / ctrlH;

                double scale;
                double offsetX = 0;
                double offsetY = 0;

                if (ctrlAspect > bmpAspect)
                {
                    
                    scale = ctrlH / bmpH;
                    offsetX = (ctrlW - bmpW * scale) / 2.0;
                }
                else
                {
                    
                    scale = ctrlW / bmpW;
                    offsetY = (ctrlH - bmpH * scale) / 2.0;
                }

                int x = (int)Math.Floor((p.X - offsetX) / scale);
                int y = (int)Math.Floor((p.Y - offsetY) / scale);

                
                if (x < 0 || x >= bmpW || y < 0 || y >= bmpH)
                    return;

                
                if (pointlist.Count == 1)
                {
                    drawLine(
                        (int)pointlist[0].X,
                        (int)pointlist[0].Y,
                        x,
                        y
                    );
                    pointlist.Clear();
                }
                else
                {
                    pointlist.Add(new Point(x, y));
                }

            }
            else if (cb_selection.SelectedIndex == 1)
            {
                Point p = e.GetPosition(drawing);


                double bmpW = _wb.PixelWidth;
                double bmpH = _wb.PixelHeight;


                double ctrlW = drawing.ActualWidth;
                double ctrlH = drawing.ActualHeight;


                double bmpAspect = bmpW / bmpH;
                double ctrlAspect = ctrlW / ctrlH;

                double scale;
                double offsetX = 0;
                double offsetY = 0;

                if (ctrlAspect > bmpAspect)
                {

                    scale = ctrlH / bmpH;
                    offsetX = (ctrlW - bmpW * scale) / 2.0;
                }
                else
                {

                    scale = ctrlW / bmpW;
                    offsetY = (ctrlH - bmpH * scale) / 2.0;
                }

                int x = (int)Math.Floor((p.X - offsetX) / scale);
                int y = (int)Math.Floor((p.Y - offsetY) / scale);


                if (x < 0 || x >= bmpW || y < 0 || y >= bmpH)
                    return;


                if (pointlist.Count == 1)
                {
                    int cx = (int)pointlist[0].X;
                    int cy = (int)pointlist[0].Y;

                    int dx = x - cx;
                    int dy = y - cy;

                    int radius = (int)Math.Round(Math.Sqrt(dx * dx + dy * dy));

                    drawCircle(cx, cy, radius);
                    pointlist.Clear();
                }
                else
                {
                    pointlist.Add(new Point(x, y));
                }
            }
            else if (cb_selection.SelectedIndex == 2)
            {
                Point p = e.GetPosition(drawing);


                double bmpW = _wb.PixelWidth;
                double bmpH = _wb.PixelHeight;


                double ctrlW = drawing.ActualWidth;
                double ctrlH = drawing.ActualHeight;


                double bmpAspect = bmpW / bmpH;
                double ctrlAspect = ctrlW / ctrlH;

                double scale;
                double offsetX = 0;
                double offsetY = 0;

                if (ctrlAspect > bmpAspect)
                {

                    scale = ctrlH / bmpH;
                    offsetX = (ctrlW - bmpW * scale) / 2.0;
                }
                else
                {

                    scale = ctrlW / bmpW;
                    offsetY = (ctrlH - bmpH * scale) / 2.0;
                }

                int x = (int)Math.Floor((p.X - offsetX) / scale);
                int y = (int)Math.Floor((p.Y - offsetY) / scale);


                if (x < 0 || x >= bmpW || y < 0 || y >= bmpH)
                    return;

                pointlist.Add(new Point(x, y));

            }
            else
            {
                return;
            }

        }

        private void drawing_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (cb_selection.SelectedIndex == 2)
            {
                if (pointlist.Count == 1)
                {
                    return;
                }
                drawCurves(pointlist.ToArray(), 9999);
                pointlist.Clear();
            }
        }

        private void undoButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void redoButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Open_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "XML-Dateien|*.xml| Alle Dateien | *.* ";
            bool? result = dialog.ShowDialog();
            if (result == true)
            {

                XmlSerializer serializer = new XmlSerializer(typeof(FormsList));
                FileStream stream = new FileStream(dialog.FileName, FileMode.Open,
                                                   FileAccess.ReadWrite);

                FormsList list = (FormsList)serializer.Deserialize(stream);
                stream.Close();
                Draw_At_Open(list);
            }
                

                
        }

        private void ClearBitmap()
        {
            int bufferSize = _stride * imageSize;
            byte[] emptyPixels = new byte[bufferSize]; // Alle Bytes sind 0 = transparent

            _wb.WritePixels(
                new Int32Rect(0, 0, imageSize, imageSize),
                emptyPixels,
                _stride,
                0
            );
        }


        private void Draw_At_Open(FormsList list)
        {
            ClearBitmap();

            try
            {
                foreach (Form f in list.forms)
                {
                    if (f.type == 1)
                    {
                        drawLine((int)f.Points[0].X, (int)f.Points[0].Y, (int)f.Points[1].X, (int)f.Points[1].Y);
                    }
                    else if (f.type == 2)
                    {
                        drawCircle((int)f.Points[0].X, (int)f.Points[0].Y, f.radius);
                    }
                    else if (f.type == 3)
                    {
                        drawCurves(f.Points.ToArray(), f.count);
                    }
                }

                formslist = list;
            }
            catch (Exception ex)
            {
                //TODO
                //Keine Ahnung warum das nicht ohne Try Catch geht "Collection was modified"
                //MessageBox.Show(ex.Message);
            }

            
        }

        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "XML-Dateien|*.xml";
            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                XmlSerializer serializer = new XmlSerializer(typeof(FormsList));

                FileStream stream = new FileStream(dialog.FileName, FileMode.Create,
                                                   FileAccess.ReadWrite);
                serializer.Serialize(stream, formslist);
                stream.Close();
            }
        }

        private void Save_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (formslist.forms.Count > 0)
            {
                e.CanExecute = true;
            }
            else
            {
                e.CanExecute = false;
            }
        }

        private void SaveAs_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "XML-Dateien|*.xml";
            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                XmlSerializer serializer = new XmlSerializer(typeof(FormsList));

                FileStream stream = new FileStream(dialog.FileName, FileMode.Create,
                                                   FileAccess.ReadWrite);
                serializer.Serialize(stream, formslist);
                stream.Close();
            }
        }

        private void Undo_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            FormsList list = formslist;
            redo.forms.Add(list.forms.Last());
            list.forms.Remove(list.forms.Last());
            Draw_At_Open(list);
        }

        private void Undo_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (formslist.forms.Count > 0)
            {
                e.CanExecute = true;
            }
            else
            {
                e.CanExecute = false;
            }
        }

        private void Redo_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            FormsList list = formslist;
            list.forms.Add(redo.forms.Last());
            redo.forms.Remove(list.forms.Last());
            Draw_At_Open(list);
        }

        private void Redo_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (redo.forms.Count > 0)
            {
                e.CanExecute = true;
            }
            else
            {
                e.CanExecute = false;
            }
        }
    }
}