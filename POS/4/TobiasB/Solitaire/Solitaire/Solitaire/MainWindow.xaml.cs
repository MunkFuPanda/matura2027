using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Solitaire
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int[,] gridarr = new int[7,7];

        private Grid grid;

        private int PrevRow = 0;
        private int PrevCol = 0;

        // Row und dann Column

        public MainWindow()
        {
            InitializeComponent();

            grid = Create(1);

            grid.PreviewMouseMove += Grid_PreviewMouseMove;
            grid.PreviewDragOver += Grid_PreviewDragOver;

            this.Content = grid;

            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    gridarr[i,j] = 1;
                }
            }

            gridarr[2,3] = 0;

            //[Grid.GetRow(child), Grid.GetColumn(child)] = 1;
            


        }

        Ellipse moving = null;
        private Point clickPosition;

        private void Ellipse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            moving = (Ellipse)sender;
            PrevRow = Grid.GetRow(moving);
            PrevCol = Grid.GetColumn(moving);

            clickPosition = e.GetPosition(this);
            moving.IsHitTestVisible = false;
            DragDrop.DoDragDrop(moving, moving, DragDropEffects.All);
        }

        public void Border_Drop(object sender, DragEventArgs e)
        {
            if (moving != null)
            {
                int col = Grid.GetColumn((UIElement)sender);
                int row = Grid.GetRow((UIElement)sender);

                int dRow = row - PrevRow;
                int dCol = col - PrevCol;

                // nur 2er Schritte erlaubt, damit er nicht weiter hüpft
                if (Math.Abs(dRow) == 2 && dCol == 0 ||
                    Math.Abs(dCol) == 2 && dRow == 0)
                {
                    int midRow = (row + PrevRow) / 2;
                    int midCol = (col + PrevCol) / 2;

                    if (gridarr[midRow, midCol] == 1 && gridarr[row, col] == 0)
                    {
                        FindGridChildAndRemove(midRow, midCol);

                        gridarr[row, col] = 1;
                        gridarr[PrevRow, PrevCol] = 0;

                        Grid.SetColumn(moving, col);
                        Grid.SetRow(moving, row);
                    }
                }

                moving.RenderTransform = null;
                moving.IsHitTestVisible = true;
                moving = null;
            }
        }


        private void FindGridChildAndRemove(int row, int col)
        {
            UIElement temp = new UIElement();

            foreach (UIElement child in grid.Children)
            {
                if (child is Ellipse)
                {
                    if (Grid.GetRow(child) == row && Grid.GetColumn(child) == col)
                    {
                        temp = child;
                    }
                }


            }

            grid.Children.Remove(temp);
            gridarr[row, col] = 0;
            gridarr[PrevRow, PrevCol] = 0;
        }



        private void Grid_PreviewDragOver(object sender, DragEventArgs e)
        {
            if (moving != null)
            {
                Point currentPosition = e.GetPosition(this);

                var transform = moving.RenderTransform as TranslateTransform;
                if (transform == null)
                {
                    transform = new TranslateTransform();
                    moving.RenderTransform = transform;
                }

                transform.X = currentPosition.X - clickPosition.X;
                transform.Y = currentPosition.Y - clickPosition.Y;
            }
        }

        private void Grid_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (moving != null)
            {
                moving.RenderTransform = null;
                moving.IsHitTestVisible = true;
                moving = null;
            }
        }

        public Grid Create(int type)
        {
            List<Border> borderList = new List<Border>();
            List<Ellipse> ellipseList = new List<Ellipse>();


            if (type == 1)
            {
                Grid grid = new Grid();

                for (int i = 0; i < 7; i++)
                {
                    grid.ColumnDefinitions.Add(new ColumnDefinition());
                    grid.RowDefinitions.Add(new RowDefinition());
                }


                for (int i = 2; i < 5; i++)
                {
                    Border border = copyBorder();
                    Ellipse ellipse = copyEllipse();
                    Grid.SetRow(border, 0);
                    Grid.SetRow(ellipse, 0);
                    Grid.SetColumn(border, i);
                    Grid.SetColumn(ellipse, i);

                    borderList.Add(border);
                    ellipseList.Add(ellipse);
                }

                for (int i = 1; i < 6; i++)
                {
                    Border border = copyBorder();
                    Ellipse ellipse = copyEllipse();
                    Grid.SetRow(border, 1);
                    Grid.SetRow(ellipse, 1);
                    Grid.SetColumn(border, i);
                    Grid.SetColumn(ellipse, i);

                    borderList.Add(border);
                    ellipseList.Add(ellipse);
                }

                for (int i = 0; i < 7; i++)
                {
                    Border border = copyBorder();
                    Ellipse ellipse = copyEllipse();
                    Grid.SetRow(border, 2);
                    Grid.SetRow(ellipse, 2);
                    Grid.SetColumn(border, i);
                    Grid.SetColumn(ellipse, i);
                    borderList.Add(border);
                    
                    if (i != 3)
                    {
                        ellipseList.Add(ellipse);
                    }
                }

                for (int i = 0; i < 7; i++)
                {
                    Border border = copyBorder();
                    Ellipse ellipse = copyEllipse();
                    Grid.SetRow(border, 3);
                    Grid.SetRow(ellipse, 3);
                    Grid.SetColumn(border, i);
                    Grid.SetColumn(ellipse, i);

                    borderList.Add(border);
                    ellipseList.Add(ellipse);
                }

                for (int i = 0; i < 7; i++)
                {
                    Border border = copyBorder();
                    Ellipse ellipse = copyEllipse();

                    if (i == 3)
                    {
                        border.Background = Brushes.Gray;
                        border.AllowDrop = false;
                    }

                    Grid.SetRow(border, 4);
                    Grid.SetRow(ellipse, 4);
                    Grid.SetColumn(border, i);
                    Grid.SetColumn(ellipse, i);
                    borderList.Add(border);
                    ellipseList.Add(ellipse);

                }

                for (int i = 1; i < 6; i++)
                {
                    Border border = copyBorder();
                    Ellipse ellipse = copyEllipse();
                    Grid.SetRow(border, 5);
                    Grid.SetRow(ellipse, 5);
                    Grid.SetColumn(border, i);
                    Grid.SetColumn(ellipse, i);

                    borderList.Add(border);
                    ellipseList.Add(ellipse);
                }

                for (int i = 2; i < 5; i++)
                {
                    Border border = copyBorder();
                    Ellipse ellipse = copyEllipse();
                    Grid.SetRow(border, 6);
                    Grid.SetRow(ellipse, 6);
                    Grid.SetColumn(border, i);
                    Grid.SetColumn(ellipse, i);

                    borderList.Add(border);
                    ellipseList.Add(ellipse);
                }

                foreach(Border b in borderList)
                {
                    grid.Children.Add(b);
                }
                foreach(Ellipse ellipse in ellipseList)
                {
                    grid.Children.Add(ellipse);
                }

                return grid;

            }
            return null;
        }


        private Border copyBorder()
        {

            Border b = new Border();

            b.BorderBrush = Brushes.Black;
            b.Background = Brushes.Beige;
            b.BorderThickness = new System.Windows.Thickness(2);
            b.AllowDrop = true;
            b.Drop += Border_Drop;

            return b;

        }

        private Ellipse copyEllipse()
        {

            Ellipse e = new Ellipse();

            e.Stroke = Brushes.Red;
            e.StrokeThickness = 2;
            e.Fill = Brushes.Red;
            e.PreviewMouseLeftButtonDown += Ellipse_MouseLeftButtonDown;

            return e;

        }
    }
}
