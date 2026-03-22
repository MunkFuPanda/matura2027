using WPF_Gomoku;
using WPF_Gomoku.Controller;
using WPF_Gomoku.Model;

namespace WPF_Gomoku.Controller
{
    public class LocalController : IController
    {
        public BoardModel Board { get; set; }

        public LocalController(int boardSize)
        {
            Board = new BoardModel(boardSize);
        }

        public void Start() { }
        public void Stop() { }

        public bool turn = true; // true for X, false for O

        public void OnCellClicked(Item item)
        {
            if (item.Charater == "" && turn)
            {
                item.Charater = "X";
            }
            else if (item.Charater == "" && !turn)
            {
                item.Charater = "O";
            }

            turn = !turn;

            /*int result = ((IController)this).CheckWin();
            if (result == 1)
            {
                System.Windows.MessageBox.Show("X hat gewonnen!");
            }
            else if (result == 2)
            {
                System.Windows.MessageBox.Show("O hat gewonnen!");
            }
            */
        }
    }
}
