using NeuralNetworks;

namespace MinimaxTicTacToe
{
    public partial class Form1 : Form
    {
        Button[,] buttons;
        bool circleTurn;
        MiniMax<TicTacToeGameState> minimax = new MiniMax<TicTacToeGameState>();
        MiniMax<TicTacToeGameState>.Node root;
        TicTacToeGameState originalState;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object? sender, EventArgs e)
        {
            circleTurn = false;
            buttons = new Button[3, 3];
            originalState = new TicTacToeGameState(buttons);
            root = new(originalState);
            minimax.BuildTree(root);
            int x = 225;
            int y = 60;
            for (int row = 0; row < buttons.GetLength(1); row++)
            {
                for (int col = 0; col < buttons.GetLength(0); col++)
                {
                    buttons[row, col] = new Button();
                    buttons[row, col].Location = new Point(x, y);
                    buttons[row, col].Size = new Size(100, 100);
                    buttons[row, col].BackColor = Color.White;
                    buttons[row, col].Click += OnClick;
                    buttons[row, col].Font = new Font("Times New Roman", 40);
                    buttons[row, col].Tag = new Point(row, col);
                    Controls.Add(buttons[row, col]);
                    x += 110;
                }
                y += 110;
                x = 225;
            }
        }

        private void OnClick(object? sender, EventArgs e)
        {
            if (sender is not Button button) return;

            //moves should follow state

            if (!circleTurn)
            {
                
                button.Text = "X";
                TicTacToeGameState tempState = new TicTacToeGameState(buttons);
                foreach (var state in root.Children)
                {                    
                    if (state.State.board.SequenceEquals(tempState.board))
                    {
                        root = state;
                        break;
                    }
                }

                circleTurn = !circleTurn;

                //find which button is pressed
                //move to the state where it is the same
            }
            else
            {

                var node = minimax.FindBestMove(root);
                MiniMax<TicTacToeGameState>.Node bestMove = null;

                foreach (var child in root.Children)
                {
                    if (child.Equals(node))
                    {
                        bestMove = child;
                        break;
                    }
                }

                if (bestMove != null)
                {
                    root = bestMove;
                    DisplayBoard(root.State.board);
                }
                circleTurn = !circleTurn;
            }
        }

        private void DisplayBoard(int[,] board)
        {
            for(int row = 0; row < board.GetLength(1); row++)
            {
                for(int col = 0; col < board.GetLength(0); col++)
                {
                    if (board[row, col] == 1)
                    {
                        buttons[row, col].Text = "X";
                    }
                    else if (board[row, col] == 2)
                    {
                        buttons[row, col].Text = "O";
                    }
                }
            }
        }
    }
}
