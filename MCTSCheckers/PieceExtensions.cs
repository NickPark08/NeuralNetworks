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
            var enemy = piece.HasFlag(Piece.Red) ? Piece.BlackPiece : Piece.RedPiece;
            List<int[]> moves = new List<int[]>();
            var forced = false;
            if (piece.HasFlag(Piece.MoveDown) && y + 2 < board.GetLength(1))
            {
                if (x + 2 < board.GetLength(0) && board[x + 1, y + 1].HasFlag(enemy) && board[x + 2, y + 2] == Piece.None)
                {
                    moves.Add([x + 2, y + 2, x, y]);
                    forced = true;
                }
                if (x - 2 >= 0 && board[x - 1, y + 1].HasFlag(enemy) && board[x - 2, y + 2] == Piece.None)
                {
                    moves.Add([x - 2, y + 2, x, y]);
                    forced = true;
                }
            }
            if (piece.HasFlag(Piece.MoveUp) && y - 2 >= 0)
            {
                if (x + 2 < board.GetLength(0) && board[x + 1, y - 1].HasFlag(enemy) && board[x + 2, y - 2] == Piece.None)
                {
                    moves.Add([x + 2, y - 2, x, y]);
                    forced = true;
                }
                if (x - 2 >= 0 && board[x - 1, y - 1].HasFlag(enemy) && board[x - 2, y - 2] == Piece.None)
                {
                    moves.Add([x - 2, y - 2, x, y]);
                    forced = true;
                }
            }

            if (!forced)
            {

                if (piece.HasFlag(Piece.MoveDown) && y + 1 < board.GetLength(1))
                {
                    if (x + 1 < board.GetLength(0) && board[x + 1, y + 1] == Piece.None)
                    {
                        moves.Add([x + 1, y + 1, x, y]);
                    }
                    if (x - 1 >= 0 && board[x - 1, y + 1] == Piece.None)
                    {
                        moves.Add([x - 1, y + 1, x, y]);
                    }
                }
                if (piece.HasFlag(Piece.MoveUp) && y - 1 >= 0)
                {
                    if (x + 1 < board.GetLength(0) && board[x + 1, y - 1] == Piece.None)
                    {
                        moves.Add([x + 1, y - 1, x, y]);
                    }
                    if (x - 1 >= 0 && board[x - 1, y - 1] == Piece.None)
                    {
                        moves.Add([x - 1, y - 1, x, y]);
                    }
                }
            }
            return moves.ToArray();
        }
    }
}
