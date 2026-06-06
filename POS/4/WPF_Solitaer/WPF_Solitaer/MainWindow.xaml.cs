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
using System.Linq;
using System.Collections.Generic;

namespace WPF_Solitaer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartGame_Click(object sender, RoutedEventArgs e)
        {
            MenuOverlay.Visibility = Visibility.Collapsed;
            Button btn = sender as Button;
            int type = int.Parse(btn.Tag.ToString());
            BuildBoard(type);
        }

        private void BuildBoard(int boardType)
        {
            grid.Children.Clear();
            grid.RowDefinitions.Clear();
            grid.ColumnDefinitions.Clear();

            int size = (boardType == 1 || boardType == 4) ? 7 : 9;

            for (int i = 0; i < size; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition());
                grid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    bool isBoard = false;
                    bool isEmpty = false;

                    switch (boardType)
                    {
                        case 1: // Europäisch 7x7 (octagonal)
                            isBoard = !((i == 0 || i == 6) && (j < 2 || j > 4) || (i == 1 || i == 5) && (j < 1 || j > 5));
                            isEmpty = (i == 5 && j == 3);
                            break;
                        case 2: // Großes Kreuz (9x9 with length 3 arms)
                            isBoard = !((i < 3 || i > 5) && (j < 3 || j > 5));
                            isEmpty = (i == 4 && j == 4);
                            break;
                        case 3: // Längliches Kreuz (9x7 / 9x9 style). Image is 9 wide, 7 tall? Let's assume it's like a cross.
                            isBoard = (i >= 3 && i <= 5) || (j >= 1 && j <= 7 && i >= 1 && i <= 7);
                            isBoard = !((i < 3 || i > 5) && (j < 2 || j > 6)); 
                            isEmpty = (i == 4 && j == 4);
                            break;
                        case 4: // Klassisches Kreuz 7x7
                            isBoard = !((i < 2 || i > 4) && (j < 2 || j > 4));
                            isEmpty = (i == 3 && j == 3);
                            break;
                        case 5: // Raute 9x9
                            int mid = 4;
                            isBoard = System.Math.Abs(i - mid) + System.Math.Abs(j - mid) <= 4;
                            isEmpty = (i == 4 && j == 4);
                            break;
                    }

                    Border border = new Border
                    {
                        BorderBrush = Brushes.Black,
                        BorderThickness = new Thickness(2),
                        Background = isBoard ? Brushes.Beige : Brushes.LightGray,
                    };

                    Grid.SetRow(border, i);
                    Grid.SetColumn(border, j);
                    grid.Children.Add(border);

                    if (isBoard)
                    {
                        border.AllowDrop = true;
                        border.Drop += Border_Drop;

                        if (!isEmpty)
                        {
                            Ellipse ellipse = new Ellipse
                            {
                                Fill = Brushes.Red,
                                Width = 50,
                                Height = 50,
                                Margin = new Thickness(5)
                            };
                            Panel.SetZIndex(ellipse, 1);
                            ellipse.PreviewMouseLeftButtonDown += Ellipse_MouseLeftButtonDown;
                            Grid.SetRow(ellipse, i);
                            Grid.SetColumn(ellipse, j);
                            grid.Children.Add(ellipse);
                        }
                    }
                }
            }
        }

        Ellipse moving = null;
        private Point clickPosition;

        private void Ellipse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            moving = (Ellipse)sender;
            clickPosition = e.GetPosition(this);
            moving.IsHitTestVisible = false;
            Panel.SetZIndex(moving, 2); // Bring to very front during drag

            Ellipse currentMoving = moving;
            DragDrop.DoDragDrop(moving, moving, DragDropEffects.All);

            if (currentMoving != null)
            {
                currentMoving.RenderTransform = null;
                currentMoving.IsHitTestVisible = true;
                Panel.SetZIndex(currentMoving, 1);
            }
            moving = null;
        }

        private void Border_Drop(object sender, DragEventArgs e)
        {
            if (moving != null)
            {
                UIElement dropTarget = sender as UIElement;
                if (dropTarget != null)
                {
                    int targetCol = Grid.GetColumn(dropTarget);
                    int targetRow = Grid.GetRow(dropTarget);

                    int sourceCol = Grid.GetColumn(moving);
                    int sourceRow = Grid.GetRow(moving);

                    bool isVerticalJump = (sourceCol == targetCol && Math.Abs(sourceRow - targetRow) == 2);
                    bool isHorizontalJump = (sourceRow == targetRow && Math.Abs(sourceCol - targetCol) == 2);

                    if (isVerticalJump || isHorizontalJump)
                    {
                        int midRow = (sourceRow + targetRow) / 2;
                        int midCol = (sourceCol + targetCol) / 2;

                        Ellipse jumpedOver = null;
                        bool targetOccupied = false;

                        foreach (UIElement child in grid.Children)
                        {
                            if (child is Ellipse el && el != moving && el.Visibility == Visibility.Visible)
                            {
                                int r = Grid.GetRow(el);
                                int c = Grid.GetColumn(el);
                                if (r == midRow && c == midCol)
                                {
                                    jumpedOver = el;
                                }
                                if (r == targetRow && c == targetCol)
                                {
                                    targetOccupied = true;
                                }
                            }
                        }

                        if (jumpedOver != null && !targetOccupied)
                        {
                            jumpedOver.Visibility = Visibility.Hidden;
                            Grid.SetColumn(moving, targetCol);
                            Grid.SetRow(moving, targetRow);

                            // Check game state slightly delayed to ensure visual updates finish.
                            Dispatcher.InvokeAsync(() => CheckGameEndOfGame());
                        }
                    }
                }
            }
        }

        private void CheckGameEndOfGame()
        {
            var ellipses = new List<Ellipse>();
            var validBorders = new List<Border>();

            foreach (UIElement child in grid.Children)
            {
                if (child is Ellipse el && el.Visibility == Visibility.Visible)
                    ellipses.Add(el);
                if (child is Border b && b.AllowDrop)
                    validBorders.Add(b);
            }

            if (ellipses.Count == 1)
            {
                MessageBox.Show("Gewonnen!");
                return;
            }

            bool canMove = false;
            int[][] dirs = new int[][] { new int[] { 0, 2 }, new int[] { 0, -2 }, new int[] { 2, 0 }, new int[] { -2, 0 } };

            foreach (var el in ellipses)
            {
                int r = Grid.GetRow(el);
                int c = Grid.GetColumn(el);

                foreach (var d in dirs)
                {
                    int tr = r + d[0];
                    int tc = c + d[1];
                    int mr = r + d[0] / 2;
                    int mc = c + d[1] / 2;

                    // is it a playable board tile?
                    bool isBoard = validBorders.Any(b => Grid.GetRow(b) == tr && Grid.GetColumn(b) == tc);
                    if (!isBoard) continue;

                    // is target empty?
                    bool targetOccupied = ellipses.Any(x => Grid.GetRow(x) == tr && Grid.GetColumn(x) == tc);
                    if (targetOccupied) continue;

                    // is middle occupied by another peg?
                    bool midOccupied = ellipses.Any(x => Grid.GetRow(x) == mr && Grid.GetColumn(x) == mc);
                    if (midOccupied)
                    {
                        canMove = true;
                        break;
                    }
                }
                if (canMove) break;
            }

            if (!canMove)
            {
                MessageBox.Show("Verloren!");
            }
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
            // Removed erratic reset logic
        }
    }
}