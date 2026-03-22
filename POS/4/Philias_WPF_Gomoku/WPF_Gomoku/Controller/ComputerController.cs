using WPF_Gomoku;
using WPF_Gomoku.Controller;
using WPF_Gomoku.Model;

namespace WPF_Gomoku.Controller
{
    public class ComputerController : IController
    {
        private const int Size = 15;

        public BoardModel Board { get; set; }

        public ComputerController(int boardSize)
        {
            Board = new BoardModel(Size);
        }

        public void Start() { }
        public void Stop() { }

        public bool turn = true; // true for X, false for O

        public void OnCellClicked(Item item)
        {
            if (item.Charater == "" && turn)
            {
                item.Charater = "X";
                ComputerMove();
            }
        }

        private Item GetCell(int row, int col)
        {
            return Board.Cells[row * Size + col];
        }

        private string GetSymbol(int row, int col)
        {
            if (row < 0 || row >= Size || col < 0 || col >= Size)
                return "?";
            return GetCell(row, col).Charater;
        }

        private void ComputerMove()
        {
            // Prüfe ob es überhaupt noch freie Felder gibt
            bool hasEmpty = false;
            foreach (var c in Board.Cells)
            {
                if (c.Charater == "") { hasEmpty = true; break; }
            }
            if (!hasEmpty) return;

            int bestRow = -1, bestCol = -1;
            int bestScore = int.MinValue;

            // 4 Richtungen: horizontal, vertikal, diagonal (\), diagonal (/)
            int[] dr = { 0, 1, 1, 1 };
            int[] dc = { 1, 0, 1, -1 };

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    if (GetSymbol(i, j) != "")
                        continue;

                    int score = 0;

                    for (int d = 0; d < 4; d++)
                    {
                        score += EvaluateDirection(i, j, dr[d], dc[d], "O"); // Angriff
                        score += EvaluateDirection(i, j, dr[d], dc[d], "X"); // Verteidigung
                    }

                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestRow = i;
                        bestCol = j;
                    }
                }
            }

            if (bestRow >= 0)
            {
                GetCell(bestRow, bestCol).Charater = "O";
            }
        }

        /// <summary>
        /// Bewertet, wie gut es wäre, auf (row, col) zu setzen,
        /// bezogen auf eine bestimmte Richtung und ein Symbol (Angriff oder Verteidigung).
        /// </summary>
        private int EvaluateDirection(int row, int col, int dr, int dc, string symbol)
        {
            // Zähle eigene Steine in beide Richtungen entlang der Linie
            int count = 0;
            bool openBefore = false;
            bool openAfter = false;

            // Vorwärts zählen
            for (int step = 1; step <= 4; step++)
            {
                string s = GetSymbol(row + dr * step, col + dc * step);
                if (s == symbol)
                    count++;
                else
                {
                    openAfter = (s == "");
                    break;
                }
            }

            // Rückwärts zählen
            for (int step = 1; step <= 4; step++)
            {
                string s = GetSymbol(row - dr * step, col - dc * step);
                if (s == symbol)
                    count++;
                else
                {
                    openBefore = (s == "");
                    break;
                }
            }

            bool isOwn = (symbol == "O");
            int openEnds = (openBefore ? 1 : 0) + (openAfter ? 1 : 0);

            // Bewertung: je mehr Steine in einer Reihe und je offener, desto besser
            return count switch
            {
                >= 4 => isOwn ? 1_000_000 : 500_000,               // 5 in Reihe (Gewinn / Blockieren)
                3 when openEnds == 2 => isOwn ? 50_000 : 40_000,   // offene 4
                3 when openEnds == 1 => isOwn ? 5_000 : 4_000,     // halb-offene 4
                2 when openEnds == 2 => isOwn ? 3_000 : 2_500,     // offene 3
                2 when openEnds == 1 => isOwn ? 500 : 400,         // halb-offene 3
                1 when openEnds == 2 => isOwn ? 200 : 150,         // offene 2
                1 when openEnds == 1 => isOwn ? 50 : 30,           // halb-offene 2
                _ => 0
            };
        }
    }
}
