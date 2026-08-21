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

namespace Statuen_aufstellen
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public class Item
    {
        public string name { get; set; }
    }
    public partial class MainWindow : Window
    {

        // mit eigener klasse und binding machen, irgendwann

        static int N = 100;
        static int[] queenPos = new int[N];
        static bool first = true;


        public MainWindow()
        {
            InitializeComponent();

            //for (int i = 0; i < N; i++)
            //    queenPos[i] = -1;

            //if (NextSolution())
            //    DrawBoard();

            int[] state = new int[N];
            int[,] board = new int[N, N];

            ConfigureRandomly(board, state);
            HillClimbing(board, state);

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    TextBlock tb = new TextBlock();
                    tb.TextAlignment = TextAlignment.Center;
                    tb.VerticalAlignment = VerticalAlignment.Center;
                    tb.FontSize = 20;

                    if (board[i, j] == 1)
                        tb.Text = "S";
                    else
                        tb.Text = "";

                    Border border = new Border();
                    border.BorderBrush = Brushes.Black;
                    border.BorderThickness = new Thickness(1);
                    border.Child = tb;

                    ug.Children.Add(border);
                }
            }



        }

        #region 8queensproblemwithnext

        static bool IsSafe(int row, int col)
        {
            for (int i = 0; i < row; i++)
            {
                int qCol = queenPos[i];

                if (qCol == col)
                    return false;

                if (Math.Abs(qCol - col) == Math.Abs(i - row))
                    return false;
            }

            return true;
        }

        static bool NextSolution()
        {
            int row = 0;

            while (row >= 0)
            {
                queenPos[row]++;

                while (queenPos[row] < N && !IsSafe(row, queenPos[row]))
                    queenPos[row]++;

                if (queenPos[row] < N)
                {
                    if (row == N - 1)
                        return true;

                    row++;
                    queenPos[row] = -1;
                }
                else
                {
                    queenPos[row] = -1;
                    row--;
                }
            }

            return false;
        }

        int[,] GetBoard()
        {
            int[,] board = new int[N, N];

            for (int r = 0; r < N; r++)
            {
                if (queenPos[r] >= 0)
                    board[r, queenPos[r]] = 1;
            }

            return board;
        }

        void DrawBoard()
        {
            ug.Children.Clear();

            int[,] res = GetBoard();

            foreach (int i in res)
            {
                Label l = new Label();

                if (i == 1)
                    l.Content = "Statue";
                else
                    l.Content = "[_]";

                ug.Children.Add(l);
            }
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (NextSolution())
                DrawBoard();
            else
            {
                MessageBox.Show("Das waren alle Lösungen", "INFO", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        #endregion

        #region 8queensproblemwithhillclimbing

        static void ConfigureRandomly(int[,] board, int[] state)
        {
            Random rand = new Random();

            for (int i = 0; i < N; i++)
            {
                state[i] = rand.Next(N);
                board[state[i], i] = 1;
            }
        }

        static void PrintBoard(int[,] board)
        {
            for (int i = 0; i < N; i++)
            {
                Console.Write(" ");
                for (int j = 0; j < N; j++)
                {
                    Console.Write(board[i, j] + " ");
                }
                Console.WriteLine();
            }
        }

        static void PrintState(int[] state)
        {
            for (int i = 0; i < N; i++)
            {
                Console.Write(" " + state[i] + " ");
            }
            Console.WriteLine();
        }

        static bool CompareStates(int[] state1, int[] state2)
        {
            for (int i = 0; i < N; i++)
            {
                if (state1[i] != state2[i])
                {
                    return false;
                }
            }
            return true;
        }

        static void Fill(int[,] board, int value)
        {
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    board[i, j] = value;
                }
            }
        }

        static int CalculateObjective(int[,] board, int[] state)
        {
            int attacking = 0;
            int row, col;

            for (int i = 0; i < N; i++)
            {
                row = state[i];
                col = i - 1;
                while (col >= 0 && board[row, col] != 1)
                {
                    col--;
                }
                if (col >= 0 && board[row, col] == 1)
                {
                    attacking++;
                }

                row = state[i];
                col = i + 1;
                while (col < N && board[row, col] != 1)
                {
                    col++;
                }
                if (col < N && board[row, col] == 1)
                {
                    attacking++;
                }

                row = state[i] - 1;
                col = i - 1;
                while (col >= 0 && row >= 0 && board[row, col] != 1)
                {
                    col--;
                    row--;
                }
                if (col >= 0 && row >= 0 && board[row, col] == 1)
                {
                    attacking++;
                }

                row = state[i] + 1;
                col = i + 1;
                while (col < N && row < N && board[row, col] != 1)
                {
                    col++;
                    row++;
                }
                if (col < N && row < N && board[row, col] == 1)
                {
                    attacking++;
                }

                row = state[i] + 1;
                col = i - 1;
                while (col >= 0 && row < N && board[row, col] != 1)
                {
                    col--;
                    row++;
                }
                if (col >= 0 && row < N && board[row, col] == 1)
                {
                    attacking++;
                }

                row = state[i] - 1;
                col = i + 1;
                while (col < N && row >= 0 && board[row, col] != 1)
                {
                    col++;
                    row--;
                }
                if (col < N && row >= 0 && board[row, col] == 1)
                {
                    attacking++;
                }
            }

            return attacking / 2;
        }

        static void GenerateBoard(int[,] board, int[] state)
        {
            Fill(board, 0);
            for (int i = 0; i < N; i++)
            {
                board[state[i], i] = 1;
            }
        }

        static void CopyState(int[] state1, int[] state2)
        {
            Array.Copy(state2, state1, N);
        }

        static void GetNeighbour(int[,] board, int[] state)
        {
            int[,] opBoard = new int[N, N];
            int[] opState = new int[N];
            CopyState(opState, state);
            GenerateBoard(opBoard, opState);
            int opObjective = CalculateObjective(opBoard, opState);

            int[,] neighbourBoard = new int[N, N];
            int[] neighbourState = new int[N];
            CopyState(neighbourState, state);
            GenerateBoard(neighbourBoard, neighbourState);

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (j != state[i])
                    {
                        neighbourState[i] = j;
                        neighbourBoard[neighbourState[i], i] = 1;
                        neighbourBoard[state[i], i] = 0;
                        int temp = CalculateObjective(neighbourBoard, neighbourState);

                        if (temp <= opObjective)
                        {
                            opObjective = temp;
                            CopyState(opState, neighbourState);
                            GenerateBoard(opBoard, opState);
                        }

                        neighbourBoard[neighbourState[i], i] = 0;
                        neighbourState[i] = state[i];
                        neighbourBoard[state[i], i] = 1;
                    }
                }
            }

            CopyState(state, opState);
            Fill(board, 0);
            GenerateBoard(board, state);
        }

        static void HillClimbing(int[,] board, int[] state)
        {
            int[,] neighbourBoard = new int[N, N];
            int[] neighbourState = new int[N];

            CopyState(neighbourState, state);
            GenerateBoard(neighbourBoard, neighbourState);

            for (int i = 0; i < 100; i++)
            {
                CopyState(state, neighbourState);
                GenerateBoard(board, state);
                GetNeighbour(neighbourBoard, neighbourState);

                if (CompareStates(state, neighbourState))
                {
                    if (CalculateObjective(board, state) == 0)
                    {
                        PrintBoard(board);
                    }

                    break;
                }
                else if (CalculateObjective(board, state) == CalculateObjective(neighbourBoard, neighbourState))
                {
                    neighbourState[new Random().Next(N)] = new Random().Next(N);
                    GenerateBoard(neighbourBoard, neighbourState);
                }
            }

            //do
            //{
            //    CopyState(state, neighbourState);
            //    GenerateBoard(board, state);
            //    GetNeighbour(neighbourBoard, neighbourState);
            //
            //    if (CompareStates(state, neighbourState))
            //    {
            //        PrintBoard(board);
            //        break;
            //    }
            //    else if (CalculateObjective(board, state) == CalculateObjective(neighbourBoard, neighbourState))
            //    {
            //        neighbourState[new Random().Next(N)] = new Random().Next(N);
            //        GenerateBoard(neighbourBoard, neighbourState);
            //    }
            //
            //} while (true);
        }

        #endregion


    }
}