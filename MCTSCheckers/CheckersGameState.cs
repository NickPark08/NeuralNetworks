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

        public Piece[,] board;
        int redCount;
        int redKingCount;
        int blackCount;
        int blackKingCount;
        public bool redTurn;
        List<CheckersGameState> children;


        public CheckersGameState(Piece[,] b, bool turn)
        {
            redTurn = turn;
            board = b;
            children = new List<CheckersGameState>();

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

        public override string ToString()
        {
            string result = "\n";
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    var curr = board[j, i];
                    result += (" " + (curr == Piece.Red ? 'R' : curr == Piece.Black ? 'B' : '_'));
                }
                result += "\n";
            }
            return result;
        }



        public int Value => (redCount + redKingCount) - (blackCount + blackKingCount); //red pieces - black pieces (count for kings)

        public bool IsTerminal => blackCount == 0 || redCount == 0 || GetChildren().Length == 0; //game over, no pieces

        public override bool Equals(object obj)
        {
            return base.Equals(obj); // 2d equals
        }

        public CheckersGameState[] GetChildren()
        {
            var possibleBoards = PossibleBoards();

            for (int i = 0; i < possibleBoards.Length; i++)
            {
                children.Add(new(possibleBoards[i], !redTurn));
            }

            return children.ToArray();
        }

        // capturing / "local" function https://sharplab.io/#v2:EYLgHgbALANALiATgVwHYwCYgNQB8ACATAAwCwAUEQIwUX4DMABEYwMIUDeFjPzVEfQozgBTALYAHANzde+fowCWqOIwCSAEUYBeRsRnlejWT3kD8URhoD2AZTjIAZo4AUAShOMuho73uJlAHNGAC8dRgAiAAtFCINfXmVVAENw+niEpRUlcM1sbAyEi0YACUV3TyNvTN95AE4XABIIzRAvRQBfLzDsXQjYxmxGZI6ItyljHxqeZPzC3w7K3iWeUUlB3TKJ5anGRd3PIkJPDmnfFas7B2d3bcmai7WJW8fxZ/GLm3snV3GznZqX2uv3mRnqLgiAFpoTDIWNQbwni9dkYkR9yJ4upkKB0gA==

        private Piece[][,] PossibleBoards()
        {
            List<Piece[,]> possibleBoards = [];
            var player = redTurn ? Piece.Red : Piece.Black;

            for (int i = 0; i < board.GetLength(1); i++)
            {
                for (int j = 0; j < board.GetLength(0); j++)
                {
                    if (board[j, i] != player) continue;

                    int y = redTurn ? i + 1 : i - 1;

                    if (j == board.GetLength(0) - 1)
                    {
                        ;
                    }

                    void AddPossibleBoard(int x, int y)
                    {
                        var tempBoard = (Piece[,])board.Clone();
                        tempBoard[x, y] = player;
                        tempBoard[j, i] = Piece.None;
                        possibleBoards.Add(tempBoard);
                    }
                    if (y >= 0 && y < board.GetLength(1))
                    {
                        if (j + 1 <= board.GetLength(0) - 1 && board[j + 1, y] == Piece.None)
                        {
                            AddPossibleBoard(j + 1, y);
                        }
                        if (j - 1 >= 0 && board[j - 1, y] == Piece.None)
                        {
                            AddPossibleBoard(j - 1, y);
                        }
                    }
                }
            }
            return possibleBoards.ToArray();
        }

        private bool HasForcedMove(List<Piece[,]> possibleBoards)
        {
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    if (!board[j, i].HasFlag(Piece.Red)) continue;

                    var player = board[j, i];

                    void AddPossibleBoard(int x, int y)
                    {
                        var tempBoard = (Piece[,])board.Clone();
                        tempBoard[x, y] = player;
                        tempBoard[j, i] = Piece.None;
                        possibleBoards.Add(tempBoard);
                    }
                        if (j + 1 <= board.GetLength(0) - 1 && board[j + 1, i + 1].HasFlag(Piece.Black))
                        {
                            //AddPossibleBoard(j + 1, y);

                        }
                        if (j - 1 >= 0 && board[j - 1, i + 1].HasFlag(Piece.Black))
                        {
                            //AddPossibleBoard(j - 1, y);
                        }
                        if (board[j, i] == Piece.RedKing)
                        {
                            //king logic
                        }
                    
                }
            }
        }


    }
}
