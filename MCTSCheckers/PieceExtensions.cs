using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static MCTSCheckers.CheckersGameState;

namespace MCTSCheckers
{
    public static class PieceExtensions
    {
        public static int[][] GetPossibleMoves(this Piece piece, Piece[,] board, int x, int y)
        {
            //check if x / y is in bounds
            //think of smart way to check forced moves
            var enemy = piece.HasFlag(Piece.Red) ? Piece.BlackPiece : Piece.RedPiece;
            List<int[]> moves = new List<int[]>();
            var forced = false; //(board[x + 1, y + 1].HasFlag(enemy) && board[x + 2, y + 2].HasFlag(Piece.None)) || (board[x - 1, y + 1].HasFlag(enemy) && board[x - 2, y + 2].HasFlag(Piece.None)) || (board[x + 1, y - 1].HasFlag(enemy) && board[x + 2, y - 2].HasFlag(Piece.None)) || (board[x - 1, y - 1].HasFlag(enemy) && board[x - 2, y - 2].HasFlag(Piece.None));

            if (y - 1 >= 0 && y + 1 < board.GetLength(1) && x - 1 >= 0 && x + 1 < board.GetLength(0))
            {

                if (piece.HasFlag(Piece.MoveDown))
                {
                    if (board[x + 1, y + 1].HasFlag(enemy) && board[x + 2, y + 2].HasFlag(Piece.None))
                    {

                    }
                    else
                    {
                        if (board[x + 1, y + 1] == Piece.None)
                        {
                            moves.Add([1, 1]);
                        }
                        if (board[x - 1, y + 1] == Piece.None)
                        {
                            moves.Add([-1, 1]);
                        }
                    }

                    if (board[x - 1, y + 1].HasFlag(enemy) && board[x - 2, y + 2].HasFlag(Piece.None))
                    {

                    }
                }
                if (piece.HasFlag(Piece.MoveUp))
                {
                    if (board[x + 1, y - 1].HasFlag(enemy) && board[x + 2, y - 2].HasFlag(Piece.None))
                    {

                    }
                    if (board[x - 1, y - 1].HasFlag(enemy) && board[x - 2, y - 2].HasFlag(Piece.None))
                    {

                    }
                }
            }


            return new int[][] { };
        }
    }
}
