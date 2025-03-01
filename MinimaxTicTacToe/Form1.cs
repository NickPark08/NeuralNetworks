using NeuralNetworks;

using MonteNode = NeuralNetworks.MonteCarloTree<MinimaxTicTacToe.TicTacToeGameState>.Node;


namespace MinimaxTicTacToe
{
    public partial class Form1 : Form
    {
        Button[,] buttons;
        bool circleTurn;
        //MiniMax<TicTacToeGameState> minimax = new MiniMax<TicTacToeGameState>();
        MonteCarloTree<TicTacToeGameState> monteCarlo = new MonteCarloTree<TicTacToeGameState>();
        MonteNode root;
        TicTacToeGameState originalState;
        Random random = new Random(1);
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object? sender, EventArgs e)
        {
            circleTurn = false;
            buttons = new Button[3, 3];
            originalState = new TicTacToeGameState(buttons);
            //root = new(originalState);
            //minimax.Opossum = new TicTacToeGameState(new int[,] { { 0, 2, 1 }, { 0, 2, 1 }, { 0, 0, 1 } }, false);
            //minimax.BuildTree(root, !circleTurn);
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

            root = new MonteNode(originalState);
            monteCarlo.root = root;
            monteCarlo.MCTS(9, root.State, random);
        }

        private void OnClick(object? sender, EventArgs e)
        {
            if (sender is not Button button) return;

            if (buttons == null || button.Text != "") return;

            //minimax.isMax = !circleTurn;

            if (!circleTurn)
            {
                button.Text = "X";
                TicTacToeGameState tempState = new TicTacToeGameState(buttons);
                foreach (var state in root.Children)
                {
                    if (state != null && state.State.board.SequenceEquals(tempState.board))
                    {
                        monteCarlo.root = state;
                        break;
                    }
                }

                circleTurn = !circleTurn;
            }
            //minimax.isMax = !circleTurn;

            if (circleTurn)
            {
                if (monteCarlo.root.Children.Length == 0)
                {
                    label1.Text = " T\n  I\n E";
                    label2.Text = " G\n A\n M\n E";
                    return;
                }

                //TestDepthFirst(minimax.isMax, root);
                TicTacToeGameState test = monteCarlo.MCTS(100000, root.State, random);
                var node = new MonteNode(test);

                MonteNode bestMove = null;

                foreach (var child in monteCarlo.root.Children)
                {
                    if (child == node)
                    {
                        bestMove = node;
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

        private void TestDepthFirst(bool max, MiniMax<TicTacToeGameState>.Node node)
        {
            if (node == null) return;

            if (max && node.Children.Any(x => x == null) && !node.State.IsTerminal)
            {
                ;
            }

            if (node.Children.Length != 0)
            {

                for (int i = 0; i < node.Children.Length; i++)
                {
                    TestDepthFirst(!max, node.Children[i]);
                }

            }
        }
    }
}
