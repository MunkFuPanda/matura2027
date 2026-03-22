using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Gomoku.Model;

namespace WPF_Gomoku.Controller
{
    public interface IController
    {
        BoardModel Board { get; set; }
        void OnCellClicked(Item item);
        void Start();
        void Stop();

        /// <summary>
        /// Prüft ob ein Spieler gewonnen hat (5 in einer Reihe).
        /// 0 = keiner hat gewonnen, 1 = X hat gewonnen, 2 = O hat gewonnen
        /// </summary>
        int CheckWin()
        {
            int size = Board.size;
            // Richtungen: horizontal, vertikal, diagonal (\), diagonal (/)
            int[][] dx = { new int[] { 0, 1, 1, 1 } };
            int[][] dy = { new int[] { 1, 0, 1, -1 } };

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    var cell = Board.Cells[i * size + j];
                    if (cell.Charater == "")
                        continue;

                    string symbol = cell.Charater;

                    for (int d = 0; d < 4; d++)
                    {
                        int count = 1;
                        for (int step = 1; step < 5; step++)
                        {
                            int ni = i + dx[0][d] * step;
                            int nj = j + dy[0][d] * step;

                            if (ni < 0 || ni >= size || nj < 0 || nj >= size)
                                break;

                            if (Board.Cells[ni * size + nj].Charater == symbol)
                                count++;
                            else
                                break;
                        }

                        if (count >= 5)
                        {
                            return symbol == "X" ? 1 : 2;
                        }
                    }
                }
            }

            return 0;
        }
    }
}
