using NeuralNetworks;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static MCTSCheckers.CheckersGameState;

namespace MCTSCheckers
{
    public record Move(Point End, Point Start)
    {
        public Move(int endX, int endY, int startX, int startY)
            : this(new Point(endX, endY), new Point(startX, startY)) { }

    }


    public static class PieceExtensions
    {


        public static Move[] GetPossibleMoves(this Piece piece, Piece[,] board, int x, int y)
        {
            Piece[,] testBoard = new Piece[8, 8];
            for(int row = 0; row < board.GetLength(1); row++)
            {
                for(int col = 0; col < board.GetLength(0); col++)
                {
                    if ((row == 1 && col == 2) || (row == 3 && col == 4) || (row == 6 && col == 5))
                    {
                        testBoard[col, row] = Piece.RedPiece;
                    }
                    else if((row == 4 && col == 5) || (row == 7 && col == 2))
                    {
                        testBoard[col, row] = Piece.BlackPiece;
                    }
                    else
                    {
                        testBoard[col, row] = Piece.None;
                    }

                }
            }
            CheckersGameState test = new(board, false, 0);
            CheckersGameState test2 = new(testBoard, false, 0);


            if (board.SequenceEquals(testBoard))
            {
                ; // breakpoint
            }

            var enemy = piece.HasFlag(Piece.Red) ? Piece.BlackPiece : Piece.RedPiece;

            List<Move> moves = [];
            var forced = false;



            if (!forced)
            {
                if (piece.HasFlag(Piece.MoveDown) && y + 1 < board.GetLength(1))
                {
                    if (x + 1 < board.GetLength(0) && board[x + 1, y + 1] == Piece.None)
                    {
                        moves.Add(new(x + 1, y + 1, x, y));
                    }
                    if (x - 1 >= 0 && board[x - 1, y + 1] == Piece.None)
                    {
                        moves.Add(new(x - 1, y + 1, x, y));
                    }
                }
                if (piece.HasFlag(Piece.MoveUp) && y - 1 >= 0)
                {
                    if (x + 1 < board.GetLength(0) && board[x + 1, y - 1] == Piece.None)
                    {
                        moves.Add(new(x + 1, y - 1, x, y));
                    }
                    if (x - 1 >= 0 && board[x - 1, y - 1] == Piece.None)
                    {
                        moves.Add(new(x - 1, y - 1, x, y));
                    }
                }
            }


            if (piece.HasFlag(Piece.MoveDown) && y + 2 < board.GetLength(1))
            {
                if (x + 2 < board.GetLength(0) && board[x + 1, y + 1].HasFlag(enemy) && board[x + 2, y + 2] == Piece.None)
                {
                    if (!forced)
                    {
                        moves.Clear();
                        forced = true;
                    }
                    moves.Add(new(x + 2, y + 2, x, y));

                }
                if (x - 2 >= 0 && board[x - 1, y + 1].HasFlag(enemy) && board[x - 2, y + 2] == Piece.None)
                {
                    if (!forced)
                    {
                        moves.Clear();
                        forced = true;
                    }
                    moves.Add(new(x - 2, y + 2, x, y));
                }
            }
            if (piece.HasFlag(Piece.MoveUp) && y - 2 >= 0)
            {
                if (x + 2 < board.GetLength(0) && board[x + 1, y - 1].HasFlag(enemy) && board[x + 2, y - 2] == Piece.None)
                {
                    if (!forced)
                    {
                        moves.Clear();
                        forced = true;
                    }
                    moves.Add(new(x + 2, y - 2, x, y));
                }
                if (x - 2 >= 0 && board[x - 1, y - 1].HasFlag(enemy) && board[x - 2, y - 2] == Piece.None)
                {
                    if (!forced)
                    {
                        moves.Clear();
                        forced = true;
                    }
                    moves.Add(new(x - 2, y - 2, x, y));
                }
            }
            return moves.ToArray();
        }
    }
}
