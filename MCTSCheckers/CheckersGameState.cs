using NeuralNetworks;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCTSCheckers
{
    class CheckersGameState : IGameState<CheckersGameState>
    {
        [Flags]//enum (red, black, nothing, king)
        public enum Piece
        {
            None = 0b000,
            Red = 0b011,
            Black = 0b001,
            King = 0b100,

            //  111 = 011 | 100
            RedKing = Red | King,
            BlackKing = Black | King,
        }

        public Piece [,] board;
        int redCount;
        int redKingCount;
        int blackCount;
        int blackKingCount;

        public CheckersGameState(Piece[,] b)
        {
            board = b;

            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    if (board[i, j] == Piece.Red)
                    {
                        redCount++;
                        if (board[i, j] == Piece.RedKing)
                        {
                            redKingCount++;
                        }
                    }
                    if (board[i, j] == Piece.Black)
                    {
                        blackCount++;
                        if (board[i, j] == Piece.BlackKing)
                        {
                            blackKingCount++;
                        }
                    }
                }
            }
        }





        public int Value => (redCount + redKingCount) - (blackCount + blackKingCount); //red pieces - black pieces (count for kings)

        public bool IsTerminal => blackCount == 0 || redCount == 0; //game over, no pieces

        public bool Equivalent(object other)
        {
            throw new NotImplementedException();
        }

        public CheckersGameState[] GetChildren()
        {
            throw new NotImplementedException();
        }
    }
}
 