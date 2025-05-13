using NeuralNetworks;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCTSCheckers
{
    public class CheckersGameState : IGameState<CheckersGameState, CheckersGameState[]>
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

            //1111
            RedKing = RedPiece | King,
            //1101
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
        //public List<CheckersGameState> LosNiños
        //{
        //    get
        //    {
        //        if (children.Count != 0)
        //        {
        //            ;
        //        }
        //        return children;
        //    }

        //    set
        //    {
        //        children = value;
        //    }
        //}

        public List<CheckersGameState> Kids;

        public CheckersGameState(Piece[,] b, bool turn, int move)
        {
            Move = move;
            redTurn = turn;
            board = b;
            Kids = new List<CheckersGameState>();

            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    if (board[i, j].HasFlag(Piece.RedPiece))
                    {
                        redCount++;
                        if (board[i, j].HasFlag(Piece.King))
                        {
                            redKingCount++;
                        }
                    }
                    if (board[i, j].HasFlag(Piece.BlackPiece))
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

        public bool IsTerminal => (blackCount == 0 && blackKingCount == 0) || (redCount == 0 && redKingCount == 0) || GetChildren().Length == 0 || IsDraw();

        public int Move;

        public override bool Equals(object obj)
        {
            return base.Equals(obj); // 2d equals
        }

        public CheckersGameState[] GetChildren()
        {
            if (Kids.Count != 0) return Kids.ToArray();

            var possibleBoards = PossibleBoards();


            for (int i = 0; i < possibleBoards.Length; i++)
            {
                Kids.Add(new(possibleBoards[i], !redTurn, Move+1));
            }

            return Kids.ToArray();
        }

        private bool IsDraw()
        {
            return Move >= 20;
        }

        private bool IsChildrenEmpty()
        {
            if (Kids.Count == 0)
            {
                Kids = GetChildren().ToList();
            }

            return Kids.Count == 0;
        }

        // capturing / "local" function https://sharplab.io/#v2:EYLgHgbALANALiATgVwHYwCYgNQB8ACATAAwCwAUEQIwUX4DMABEYwMIUDeFjPzVEfQozgBTALYAHANzde+fowCWqOIwCSAEUYBeRsRnlejWT3kD8URhoD2AZTjIAZo4AUAShOMuho73uJlAHNGAC8dRgAiAAtFCINfXmVVAENw+niEpRUlcM1sbAyEi0YACUV3TyNvTN95AE4XABIIzRAvRQBfLzDsXQjYxmxGZI6ItyljHxqeZPzC3w7K3iWeUUlB3TKJ5anGRd3PIkJPDmnfFas7B2d3bcmai7WJW8fxZ/GLm3snV3GznZqX2uv3mRnqLgiAFpoTDIWNQbwni9dkYkR9yJ4upkKB0gA==

        private Piece[][,] PossibleBoards()
        {
            List<Piece[,]> possibleBoards = [];
            bool forced = false;
            var player = redTurn ? Piece.Red : Piece.Exists; // change -> redking has flag of blackpiece

            for (int i = 0; i < board.GetLength(1); i++)
            {
                for (int j = 0; j < board.GetLength(0); j++)
                {
                    Piece testPlayer = redTurn ? Piece.Red : Piece.None;
                    var temp = ExactMatch(board[j, i]);
                    if (ExactMatch(board[j, i]) != testPlayer) continue;

                    if (i == 2 && j == 7 && board[i, j] == Piece.RedKing)// && board[4, 5] == Piece.BlackPiece) // 2, 7
                    {
                        ;
                    }

                    var moves = board[j, i].GetPossibleMoves(board, j, i);

                    void AddPossibleBoard(int x, int y, Piece[,] tempBoard)
                    {
                        if(y == 0 || y == board.GetLength(1) - 1)
                        {
                            player |= Piece.King;
                        }
                        tempBoard[x, y] = player;
                        tempBoard[j, i] = Piece.None;
                        possibleBoards.Add(tempBoard);
                    }

                    foreach(var move in moves)
                    {
                        player = board[move.Start.X, move.Start.Y];
                        if (!forced && ((move.End.X - move.Start.X) % 2 != 0 || (move.End.Y - move.Start.Y) % 2 != 0))
                        {
                            var tempBoard = (Piece[,])board.Clone();
                            AddPossibleBoard(move.End.X, move.End.Y, tempBoard);
                        }

                        if((move.End.X - move.Start.X) % 2 == 0 && (move.End.Y - move.Start.Y) % 2 == 0)
                        {
                            if (!forced)
                            {
                                forced = true;
                                Move = 0;
                                possibleBoards.Clear();
                            }
                            if (move.End.Y == 0 || move.End.Y == board.GetLength(1) - 1)
                            {
                                player |= Piece.King;
                            }
                            var tempBoard = (Piece[,])board.Clone();
                            AddPossibleBoard(move.End.X, move.End.Y, tempBoard);
                            tempBoard[move.Start.X + ((move.End.X - move.Start.X) / 2), move.Start.Y + ((move.End.Y - move.Start.Y) / 2)] = Piece.None;
                            //var tempState = new CheckersGameState(tempBoard, false, 0);

                        }
                    }

                }
            }
            return possibleBoards.ToArray();
        }

        private Piece ExactMatch (Piece current)
        {
            return current & Piece.Red;
        }

        public bool Equivalent(object other)
        {
            throw new NotImplementedException();
        }
    }
}
