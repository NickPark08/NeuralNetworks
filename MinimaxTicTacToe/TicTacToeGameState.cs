using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuralNetworks;

namespace MinimaxTicTacToe
{
    class TicTacToeGameState : IGameState<TicTacToeGameState>
    {
        int[,] board;
        bool isXTurn = true;
        public int Value => throw new NotImplementedException();

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
                        board[i, j] = 1;
                    }
                    else
                    {
                        board[i, j] = 2;
                    }
                }
            }
        }

        public TicTacToeGameState(int[,] previousBoard)
        {
            board = previousBoard;
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
                    children[i] = new TicTacToeGameState(board);
                    //change one value of each game state to make all different possible outcomes
                }
            }

            return children;
        }
    }
}
