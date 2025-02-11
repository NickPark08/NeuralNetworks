using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NeuralNetworks;

namespace MinimaxTicTacToe
{
    public class TicTacToeGameState : IGameState<TicTacToeGameState>
    {
        public int[,] board;
        public bool isXTurn = true; // X - Maximizer, O - Minimizer

        public int Value => GetValue();

        public bool IsTerminal => Value == -1 || Value == 1;

        TicTacToeGameState[] children = null;

        public TicTacToeGameState(Button[,] buttons)
        {
            //isXTurn = xTurn;
            board = new int[buttons.GetLength(0), buttons.GetLength(1)];
            for (int i = 0; i < buttons.GetLength(1); i++)
            {
                for (int j = 0; j < buttons.GetLength(0); j++)
                {
                    if (buttons[i, j] != null)
                    {
                        if (buttons[i, j].Text == "")
                        {
                            board[i, j] = 0;
                        }
                        else if (buttons[i, j].Text == "X")
                        {
                            board[i, j] = 1; // X
                        }
                        else
                        {
                            board[i, j] = 2; // O
                        }
                    }
                }
            }
        }

        public TicTacToeGameState(int[,] previousBoard, bool xTurn)
        {
            isXTurn = xTurn;
            board = previousBoard;
        }

        private int GetValue()
        {
            for (int i = 0; i < 3; i++)
            {
                // Rows
                if (board[i, 0] != 0 && board[i, 0] == board[i, 1] && board[i, 1] == board[i, 2]) return board[i, 0] == 1 ? 1 : -1;

                // Columns
                if (board[0, i] != 0 && board[0, i] == board[1, i] && board[1, i] == board[2, i]) return board[0, i] == 1 ? 1 : -1;
            }

            // Diagonals
            if (board[0, 0] != 0 && board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2]) return board[0, 0] == 1 ? 1 : -1;

            if (board[0, 2] != 0 && board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0]) return board[0, 2] == 1 ? 1 : -1;

            // No winner
            return 0;
        }


        public TicTacToeGameState[] GetChildren()
        {
            if (children != null) return children;

            //isXTurn = !isXTurn; // ???
            var possibleBoards = PossibleBoard();
            children = new TicTacToeGameState[possibleBoards.Length];
            for(int i = 0; i < children.Length; i++)
            {
                children[i] = new TicTacToeGameState(possibleBoards[i], !isXTurn);
            }
            return children;
        }
        private int[][,] PossibleBoard()
        {
            //implement each gamestate knowing which player turn

            List<int[,]> possibleBoards = new List<int[,]>();
            int player = isXTurn ? 1 : 2;
            for (int i = 0; i < board.GetLength(1); i++)
            {
                for(int j = 0; j < board.GetLength(0); j++)
                {
                    if (board[i, j] != 0) continue;

                    var tempBoard = (int[,])board.Clone();
                    tempBoard[i, j] = player;
                    possibleBoards.Add(tempBoard);
                }
            }
            return possibleBoards.ToArray();
        }

        public override string ToString()
        {
            string result = "\n";
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    var curr = board[i, j];
                    result += (" " + (curr == 1 ? 'X' : curr == 2 ? 'O' : '_'));
                }
                result += "\n";
            }
            return result;
        }

        public bool Equivalent(object other)
        {

            return board.SequenceEquals(((TicTacToeGameState)other).board);
        }
    }
}
