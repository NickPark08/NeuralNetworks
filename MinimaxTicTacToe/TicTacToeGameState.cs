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
        int[,] board;
        public bool isXTurn = true; // X - Maximizer, O - Minimizer

        public int Value => GetValue();

        public bool IsTerminal => Value == -1 || Value == 1;

        TicTacToeGameState[] children = null;

        public TicTacToeGameState(Button[,] buttons)
        {
            board = new int[buttons.GetLength(0), buttons.GetLength(1)];
            for(int i = 0; i < buttons.GetLength(1); i++)
            {
                for(int j = 0; j < buttons.GetLength(0); j++)
                {
                    if (buttons[i,j].Text == "")
                    {
                        board[i, j] = 0;
                    }
                    else if (buttons[i,j].Text == "X")
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

        public TicTacToeGameState(int[,] previousBoard)
        {
            board = previousBoard;
        }

        private int GetValue()
        {
            for(int i = 0; i <  board.GetLength(1); i++)
            {
                if (board[i, 0] == 1 && board[i, 1] == 1 && board[i, 2] == 1)
                {
                    if(isXTurn)
                    {
                        return -1;
                    }
                    else
                    {
                        return 1;
                    }
                }
            }
            for(int i = 0; i < board.GetLength(0); i++)
            {
                if (board[0, i] == 1 && board[1, i] == 1 && board[2, i] == 1)
                {
                    if(isXTurn)
                    {
                        return -1;
                    }
                    else
                    {
                        return 1;
                    }
                }
            }
            if ((board[0, 0] == 1 && board[1, 1] == 1 && board[2, 2] == 1) || (board[2, 0] == 1 && board[1, 1] == 1 && board[0, 2] == 1))
            {
                if(isXTurn)
                {
                    return -1;
                }
                else
                {
                    return 1;
                }
            }
            return 0;
        }

        public TicTacToeGameState[] GetChildren()
        {
            if(children == null)
            {
                int count = 0;
                foreach(var val in board)
                {
                    if (val == 0)
                    {
                        count++;
                    }
                }
                children = new TicTacToeGameState[count];
                for(int i = 0; i < count; i++)
                {
                    children[i] = new TicTacToeGameState(PossibleBoard(i));
                    //change one value of each game state to make all different possible outcomes
                }
            }

            return children;
        }
        private int[,] PossibleBoard(int count)
        {
            int[,] tempBoard = board;
            int counter = count;
            for(int i = 0; i < tempBoard.GetLength(1); i++)
            {
                for(int j = 0; j < tempBoard.GetLength(0); j++)
                {
                    if (tempBoard[i, j] == 0)
                    {
                        if(counter == 0)
                        {
                            if (isXTurn)
                            {
                                tempBoard[i, j] = 1;
                            }
                            else
                            {
                                tempBoard[i, j] = 2;
                            }
                        }
                        else
                        {
                            counter--;
                        }
                    }
                }
            }
            return tempBoard;
        }
    }
}
