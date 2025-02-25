namespace MCTSCheckers
{
    public partial class Form1 : Form
    {

        Button[,] buttons;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            buttons = new Button[8, 8];
            const int startX = 0;
            const int startY = 0;

            int x = startX;
            int y = startY;

            const int squareSize = 100;
            const int squareGap = 0;

            for (int row = 0; row < buttons.GetLength(1); row++)
            {
                for (int col = 0; col < buttons.GetLength(0); col++)
                {
                    var newButton = buttons[row, col] = new Button()
                    {
                        Location = new Point(x, y),
                        Size = new Size(squareSize, squareSize),
                        Tag = new Point(row, col),
                        FlatStyle = FlatStyle.Flat,
                    };
                    if(row % 2 == 0)
                    {
                        if(col % 2 == 0)
                        {
                            newButton.BackColor = Color.Black;
                        }
                        else
                        {
                            newButton.BackColor = Color.White;
                        }
                    }
                    else
                    {
                        if(col % 2 == 0)
                        {
                            newButton.BackColor = Color.White;
                        }
                        else
                        {
                            newButton.BackColor = Color.Black;
                        }
                    }
                    newButton.Click += OnClick;
                    Controls.Add(newButton);
                    x += squareSize + squareGap;
                }
                y += squareSize + squareGap;
                x = startX;
            }

            ClientSize = new Size(2 * startX + squareSize * buttons.GetLength(0) + squareGap * (buttons.GetLength(0) - 1), 2 * startY + squareSize * buttons.GetLength(0) + squareGap * (buttons.GetLength(0) - 1));
        }
        private void OnClick(object? sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
