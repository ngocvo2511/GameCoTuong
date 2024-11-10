using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLogic
{
    public class ValuePiece
    {
        public ValuePiece()
        {
            rAdvisor = ReverseMatrix(bSoldier);
            rCannon = ReverseMatrix(bCannon);
            rChariot = ReverseMatrix(rChariot);
            rElephant = ReverseMatrix(rElephant);
            rGeneral = ReverseMatrix(rGeneral);
            rSoldier = ReverseMatrix(rSoldier);
            rHorse = ReverseMatrix(rHorse);
        }
        private readonly int[,] bSoldier = new int[10, 9]
        {
            {0,  0,  0,  0,  0,  0,  0,  0,  0 },
            {0,  0,  0,  0,  0,  0,  0,  0,  0 },
            {0,  0,  0,  0,  0,  0,  0,  0,  0},
            {0,  0, -2,  0,  4,  0, -2,  0,  0},
            {2,  0,  8,  0,  8,  0,  8,  0,  2 },
            {6,  12, 18, 18, 20, 18, 18, 12, 6 },
            {10, 20, 30, 34, 40, 34, 30, 20, 10 },
            {14, 26, 42, 60, 80, 60, 42, 26, 14 },
            {18, 36, 56, 80, 120, 80, 56, 36, 18 },
            {0,  3,  6,  9,  12,  9,  6,  3,  0}
        };
        private readonly int[,] bHorse = new int[10, 9]
        {
            {0, -4, 0, 0, 0, 0, 0, -4, 0 },
            {0, 2, 4, 4, -2, 4, 4, 2, 0 },
            {4, 2, 8, 8, 4, 8, 8, 2, 4 },
            {2, 6, 8, 6, 10, 6, 8, 6, 2 },
            {4, 12, 16, 14, 12, 14, 16, 12, 4 },
            {6, 16, 14, 18, 16, 18, 14, 16, 6 },
            {8, 24, 18, 24, 20, 24, 18, 24, 8 },
            {12, 14, 16, 20, 18, 20, 16, 14, 12 },
            {4, 10, 28, 16, 8, 16, 28, 10, 4 },
            {4, 8, 16, 12, 4, 12, 16, 8, 4 }
        };
        private readonly int[,] bChariot = new int[10, 9]
        {
            {-2, 10, 6, 14, 12, 14, 6, 10, -2 },
            {8, 4, 8, 16, 8, 16, 8, 4, 8 },
            {4, 8, 6, 14, 12, 14, 6, 8, 4 },
            {6, 10, 8, 14, 14, 14, 8, 10, 6 },
            {12, 16, 14, 20, 20, 20, 14, 16, 12 },
            {12, 14, 12, 18, 18, 18, 12, 14, 12 },
            {12, 18, 16, 22, 22, 22, 16, 18, 12 },
            {12, 12, 12, 18, 18, 18, 12, 12, 12 },
            {16, 20, 18, 24, 26, 24, 18, 20, 16 },
            {14, 14, 12, 18, 16, 18, 12, 14, 14 }
        };
        private readonly int[,] bCannon = new int[10, 9]
        {
            {0, 0, 2, 6, 6, 6, 2, 0, 0 },
            { 0, 2, 4, 6, 6, 6, 4, 2, 0 },
            {4, 0, 8, 6, 10, 6, 8, 0, 4 },
            {0, 0, 0, 2, 4, 2, 0, 0, 0 },
            {-2, 0, 4, 2, 6, 2, 4, 0, -2 },
            {0, 0, 0, 2, 8, 2, 0, 0, 0 },
            {0, 0, -2, 4, 10, 4, -2, 0, 0 },
            {2, 2, 0, -10, -8, -10, 0, 2, 2 },
            {2, 2, 0, -4, -14, -4, 0, 2, 2 },
            {6, 4, 0, -10, -12, -10, 0, 4, 6 }
        };
        private readonly int[,] bElephant = new int[10, 9]
        {
            {0, 0, 0, 0, 0, 0, 0, 0, 0 },
            {0, 0, 0, 0, 0, 0, 0, 0, 0 },
            {0, 0, 0, 0, 2, 0, 0, 0, 0 },
            {0, 0, 0, 0, 0, 0, 0, 0, 0 },
            {0, 0, 0, 0, 0, 0, 0, 0, 0 },
            {0, 0, 0, 0, 0, 0, 0, 0, 0 },
            {0, 0, 0, 0, 0, 0, 0, 0, 0 },
            {0, 0, 0, 0, 0, 0, 0, 0, 0 },
            {0, 0, 0, 0, 0, 0, 0, 0, 0 },
            {0, 0, 0, 0, 0, 0, 0, 0, 0 }
        };
        private readonly int[,] bAdvisor = new int[10, 9]
        {
            {0, 0, 0, 0, 0, 0, 0, 0, 0 },
            {0, 0, 0, 0, 2, 0, 0, 0, 0 },
            {0, 0, 0, 0, 0, 0, 0, 0, 0 },
            {0, 0, 0, 0, 0, 0, 0, 0, 0 },
            {0, 0, 0, 0, 0, 0, 0, 0, 0 },
            {0, 0, 0, 0, 0, 0, 0, 0, 0 },
            {0, 0, 0, 0, 0, 0, 0, 0, 0 },
            {0, 0, 0, 0, 0, 0, 0, 0, 0 },
            {0, 0, 0, 0, 0, 0, 0, 0, 0 },
            {0, 0, 0, 0, 0, 0, 0, 0, 0 }
        };
        private readonly int[,] bGeneral = new int[10, 9]
        {
            {0, 0, 0, 0, 2, 0, 0, 0, 0 },
            {0, 0, 0, 0, 2, 0, 0, 0, 0 },
            {0, 0, 0, 0, 2, 0, 0, 0, 0 },
            {0, 0, 0, 0, 0, 0, 0, 0, 0 },
            {0, 0, 0, 0, 0, 0, 0, 0, 0 },
            {0, 0, 0, 0, 0, 0, 0, 0, 0 },
            {0, 0, 0, 0, 0, 0, 0, 0, 0 },
            {0, 0, 0, 0, 0, 0, 0, 0, 0 },
            {0, 0, 0, 0, 0, 0, 0, 0, 0 },
            {0, 0, 0, 0, 0, 0, 0, 0, 0 }
        };
        private readonly int[,] rSoldier;
        private readonly int[,] rChariot;
        private readonly int[,] rCannon;
        private readonly int[,] rHorse;
        private readonly int[,] rElephant;
        private readonly int[,] rAdvisor;
        private readonly int[,] rGeneral;
        private int[,] ReverseMatrix(int[,] matrix)
        {
            int row = matrix.GetLength(0);
            int col = matrix.GetLength(1);
            int[,] newMatrix = new int[row, col];
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    newMatrix[i, j] = matrix[row - i - 1, col - j - 1];
                }
            }
            return newMatrix;
        }
        public int GetValueBoard(Board board)
        {
            int totalvalue = 0;
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (board[i, j] != null)
                    {
                        switch (board[i, j].Type)
                        {
                            case PieceType.Cannon:
                                totalvalue += (board[i, j].Color == Player.Red) ? -(45 + rCannon[i, j]) : (45 + bCannon[i, j]);
                                break;
                            case PieceType.Advisor:
                                totalvalue += (board[i, j].Color == Player.Red) ? -(20 + rAdvisor[i, j]) : (20 + bAdvisor[i, j]);
                                break;
                            case PieceType.Horse:
                                totalvalue += (board[i, j].Color == Player.Red) ? -(40 + rHorse[i, j]) : (40 + bHorse[i, j]);
                                break;
                            case PieceType.Elephant:
                                totalvalue += (board[i, j].Color == Player.Red) ? -(20 + rCannon[i, j]) : (20 + bCannon[i, j]);
                                break;
                            case PieceType.Chariot:
                                totalvalue += (board[i, j].Color == Player.Red) ? -(90 + rChariot[i, j]) : (90 + bCannon[i, j]);
                                break;
                            case PieceType.Soldier:
                                totalvalue += (board[i, j].Color == Player.Red) ? -(15 + rCannon[i, j]) : (15 + bCannon[i, j]);
                                break;
                            case PieceType.General:
                                totalvalue += (board[i, j].Color == Player.Red) ? -(900 + rCannon[i, j]) : (900 + bCannon[i, j]);
                                break;
                        }
                    }
                }
            }
            return totalvalue;
        }

    }
}
