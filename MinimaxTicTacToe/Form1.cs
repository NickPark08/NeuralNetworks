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
            minimax.Opossum = new TicTacToeGameState(new int[,] { { 0, 2, 1 }, { 0, 2, 1 }, { 0, 0, 1 } }, false);
            minimax.BuildTree(root, !circleTurn);
            label1.Size = new(100, 100);
            label1.Font = new("Times New Roman", 50);
            label1.Text = "";
            label2.Size = new(100, 100);
            label2.Font = new("Times New Roman", 50);
            label2.Text = "";
            int x = 225;
            int y = 60;
            for (int row = 0; row < buttons.GetLength(1); row++)
            {
                for (int col = 0; col < buttons.GetLength(0); col++)
                {
                    var newButton = buttons[row, col] = new Button()
                    {
                        Location = new Point(x, y),
                        Size = new Size(100, 100),
                        BackColor = Color.White,
                        Font = new Font("Times New Roman", 40),
                        Tag = new Point(row, col),
                    };
                    newButton.Click += OnClick;
                    Controls.Add(newButton);
                    x += 110;
                }
                y += 110;
                x = 225;
            }
        }

        private void OnClick(object? sender, EventArgs e)
        {
            if (sender is not Button button) return;

            if (buttons == null || button.Text != "") return;

            minimax.isMax = !circleTurn;

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
            }
            minimax.isMax = !circleTurn;

            if(circleTurn)
            {
                if (root.Children.Length == 0)
                {
                    label1.Text = " T\n  I\n E";
                    label2.Text = " G\n A\n M\n E";
                    return;
                }

                var node = minimax.FindBestMove(root);
                MiniMax<TicTacToeGameState>.Node bestMove = null;

                foreach (var child in root.Children)
                {
                    if (child == node)
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

            if (root.State.IsTerminal)
            {
                buttons = null;
                string winner = !circleTurn ? "OS" : "XS";
                label1.Text = winner;
                label2.Text = " W\n  I\n N";
            }
        }

        private void DisplayBoard(int[,] board)
        {
            for (int row = 0; row < board.GetLength(1); row++)
            {
                for (int col = 0; col < board.GetLength(0); col++)
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
