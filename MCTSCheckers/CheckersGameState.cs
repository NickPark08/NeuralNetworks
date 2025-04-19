using NeuralNetworks;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCTSCheckers
{
    public class CheckersGameState : IGameState<CheckersGameState>
    {
        [Flags]//enum (red, black, nothing, king)
        public enum Piece
        {
            Exists = 0b0001,
            Red = 0b0010,
            MoveUp = 0b0100,
            MoveDown = 0b1000,

            //  111 = 011 | 100

            RedPiece = Red | Exists | MoveDown,
            BlackPiece = Exists | MoveUp,
            King = MoveUp | MoveDown,

            RedKing = RedPiece | King,
            BlackKing = BlackPiece | King,

            None = 0b0000,
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

            //change later
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    if (board[i, j] == Piece.RedPiece)
                    {
                        redCount++;
                        if (board[i, j].HasFlag(Piece.King))
                        {
                            redKingCount++;
                        }
                    }
                    if (!board[i, j].HasFlag(Piece.BlackPiece))
                    {
                        blackCount++;
                        if (board[i, j].HasFlag(Piece.King))
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
                    result += (" " + (curr.HasFlag(Piece.RedPiece) ? 'R' : curr.HasFlag(Piece.BlackPiece) ? 'B' : '_'));
                }
                result += "\n";
            }
            return result;
        }



        public int Value => (redCount + redKingCount) - (blackCount + blackKingCount); //red pieces - black pieces (count for kings)

        public bool IsTerminal => blackCount == 0 || redCount == 0 || GetChildren().Length == 0; //game over, no pieces

        //public override bool Equals(object obj)
        //{
        //    return base.Equals(obj); // 2d equals
        //}

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
            List<Piece[,]> possibleBoards = new List<Piece[,]>();
            var player = redTurn ? Piece.RedPiece : Piece.BlackPiece;

            for (int i = 0; i < board.GetLength(1); i++)
            {
                for (int j = 0; j < board.GetLength(0); j++)
                {
                    if (!board[j, i].HasFlag(player)) continue;

                    var moves = board[j, i].GetPossibleMoves(board, j, i);


                    void AddPossibleBoard(int x, int y)
                    {
                        var tempBoard = (Piece[,])board.Clone();
                        if(y == 0 || y == board.GetLength(1))
                        {
                            player = redTurn ? Piece.RedKing : Piece.BlackKing;
                        }
                        tempBoard[x, y] = player;
                        tempBoard[j, i] = Piece.None;
                        possibleBoards.Add(tempBoard);
                    }

                    foreach(var move in moves)
                    {
                        player = board[move[2], move[3]];
                        if ((move[0] - move[2]) % 2 != 0 || (move[1] - move[3]) % 2 != 0)
                        {
                            AddPossibleBoard(move[0], move[1]);
                        }
                        else
                        {
                            var tempBoard = (Piece[,])board.Clone();
                            if (move[1] == 0 || move[1] == board.GetLength(1))
                            {
                                player = redTurn ? Piece.RedKing : Piece.BlackKing;
                            }
                            tempBoard[move[0], move[1]] = player;
                            tempBoard[j, i] = Piece.None;
                            tempBoard[move[2] + ((move[0] - move[2]) / 2), move[3] + ((move[1] - move[3]) / 2)] = Piece.None;
                            possibleBoards.Add(tempBoard);
                        }
                    }

                }
            }
            return possibleBoards.ToArray();
        }

        //private bool HasForcedMove(ref List<Piece[,]> possibleBoards)
        //{ 

        //    for (int i = 0; i < board.GetLength(0); i++)
        //    {
        //        for (int j = 0; j < board.GetLength(1); j++)
        //        {
        //            if (board[j, i] == Piece.None) continue;

        //            var player = board[j, i];

        //            var enemy = player.HasFlag(Piece.Red) ? Piece.BlackPiece : Piece.RedPiece;

        //            if(j == 2 && i == 3)
        //            {
        //                ;
        //            }

        //            if (j + 2 < board.GetLength(0) && j - 2 >= 0 && i + 2 < board.GetLength(1) && i - 2 >= 0)
        //            {

        //                if (player.HasFlag(Piece.MoveDown))
        //                {
        //                    if (board[j + 1, i + 1].HasFlag(enemy) && board[j + 2, i + 2].HasFlag(Piece.None))
        //                    {
        //                        var tempBoard = (Piece[,])board.Clone();
        //                        tempBoard[j + 2, i + 2] = player;
        //                        tempBoard[j + 1, i + 1] = Piece.None;
        //                        tempBoard[j, i] = Piece.None;
        //                        possibleBoards.Add(tempBoard);
        //                    }
        //                    if (board[j - 1, i + 1].HasFlag(enemy) && board[j - 2, i + 2].HasFlag(Piece.None))
        //                    {
        //                        var tempBoard = (Piece[,])board.Clone();
        //                        tempBoard[j - 2, i + 2] = player;
        //                        tempBoard[j - 1, i + 1] = Piece.None;
        //                        tempBoard[j, i] = Piece.None;
        //                        possibleBoards.Add(tempBoard);
        //                    }
        //                }
        //                if (player.HasFlag(Piece.MoveUp))
        //                {
        //                    if (board[j + 1, i - 1].HasFlag(enemy) && board[j + 2, i - 2].HasFlag(Piece.None))
        //                    {
        //                        var tempBoard = (Piece[,])board.Clone();
        //                        tempBoard[j + 2, i - 2] = player;
        //                        tempBoard[j + 1, i - 1] = Piece.None;
        //                        tempBoard[j, i] = Piece.None;
        //                        possibleBoards.Add(tempBoard);
        //                    }
        //                    if (board[j - 1, i - 1].HasFlag(enemy) && board[j - 2, i - 2].HasFlag(Piece.None))
        //                    {
        //                        var tempBoard = (Piece[,])board.Clone();
        //                        tempBoard[j - 2, i - 2] = player;
        //                        tempBoard[j - 1, i - 1] = Piece.None;
        //                        tempBoard[j, i] = Piece.None;
        //                        possibleBoards.Add(tempBoard);
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    return possibleBoards.Count != 0;
        //}


    }
}
