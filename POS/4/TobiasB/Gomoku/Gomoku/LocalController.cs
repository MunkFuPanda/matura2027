using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Runtime.InteropServices;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace Gomoku
{
    public class LocalController : Controller
    {

        // 0 = passt
        // 1 = feld schon belegt
        // 2 = gewonnen

        public override int Input(PlayingField playingField, int x, int y, char player)
        {
            for (int i = 0; i < playingField.size; i++)
            {
                for (int j = 0; j < playingField.size; j++)
                {
                    if (j == x  && i == y)
                    {
                        if (playingField.board[i,j].colour == 'N')
                        {
                            playingField.board[i, j].Colour = player;
                        }
                        else
                        {
                            return 1;
                        }
                    }
                }
            }

            // Win check

            int[][] directions =
            {
                new int[] {1, 0 },
                new int[] {0, 1},
                new int[] {1, 1},
                new int[] {1, -1},
            };

            foreach (int[] dir in directions)
            {
                int count = 1;

                count += CountInDirection(playingField, x, y, dir[0], dir[1], player);
                count += CountInDirection(playingField, x, y, -dir[0], -dir[1], player);

                if (count == 5)
                {
                    return 2;
                }
                else
                {
                    count = 0;

                    for (int i = 0; i < playingField.size; i++)
                    {
                        for (int j = 0; j < playingField.size; j++)
                        {
                            if (playingField.board[j, i].colour == 'N')
                            {
                                count++;
                                break;
                            }
                        }
                    }

                    if (count == 0)
                    {
                        return 3;
                    }
                }
                
            }
            return 0;
        }

        private int CountInDirection(PlayingField pf, int x, int y, int dx, int dy, char player)
        {
            int count = 0;
            int curX = x + dx;
            int curY = y + dy;

            while (curX >= 0 && curX < pf.size && curY >= 0 && curY < pf.size && pf.board[curY, curX].colour == player)
            {
                count++;
                curX += dx;
                curY += dy;
            }

            return count;
        }
    }
}
