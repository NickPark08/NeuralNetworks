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
            circleTurn = true;
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

            //implement tic tac toe
            minimax.FindBestMove(root);

            if (circleTurn)
            {
                button.Text = "X";
            }
            else
            {
                button.Text = "O";
            }
            //circleTurn = !circleTurn;
        }
    }
}
