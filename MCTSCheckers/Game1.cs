using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using NeuralNetworks;
using MonoGame.Extended;
using MonteNode = NeuralNetworks.MonteCarloTree<MCTSCheckers.CheckersGameState>.Node;
using Piece = MCTSCheckers.CheckersGameState.Piece;

using System;

namespace MCTSCheckers;

public class Game1 : Game
{
    private GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch;

    Rectangle[,] board;
    Piece[,] originalBoard; 
    MonteCarloTree<CheckersGameState> tree;
    bool redTurn = true;
    const int squareSize = 100;

    public Game1()
    {
        graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

    }

    protected override void Initialize()
    {
        tree = new MonteCarloTree<CheckersGameState>();
        board = new Rectangle[8, 8];
        originalBoard = new Piece[8, 8];
        Size ClientSize = new Size(squareSize * board.GetLength(0),squareSize * board.GetLength(0));

        graphics.PreferredBackBufferWidth = ClientSize.Width;
        graphics.PreferredBackBufferHeight = ClientSize.Height;
        graphics.ApplyChanges();

        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                board[i, j] = new Rectangle(squareSize * i, squareSize * j, squareSize, squareSize);
                if(i % 2 != j % 2)
                {
                    if(j < 3)
                    {
                        originalBoard[i, j] = Piece.Red;
                    }
                    else if(j > 4)
                    {
                        originalBoard[i, j] = Piece.Black;
                    }
                    else
                    {
                        originalBoard[i, j] = Piece.None;
                    }
                }
                else
                {
                    originalBoard[i, j] = Piece.None;
                }
            }
        }

        var originalState = new CheckersGameState(originalBoard, redTurn);
        tree.root = new MonteNode(originalState, redTurn);


        var test = tree.root.State.GetChildren();


        base.Initialize();
    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.LightGray);

        spriteBatch.Begin();

        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                if (i % 2 != j % 2)
                { 
                    spriteBatch.FillRectangle(board[i, j], Color.DarkGray);

                    if (tree.root.State.board[i, j] == Piece.Red)
                    {
                        spriteBatch.DrawCircle(new(new Vector2(board[i, j].X + squareSize/2, board[i, j].Y + squareSize/2), 25), 30, Color.Red, 25);
                    }
                    else if (tree.root.State.board[i, j] == Piece.Black)
                    {
                        spriteBatch.DrawCircle(new(new Vector2(board[i, j].X + squareSize / 2, board[i, j].Y + squareSize / 2), 25), 30, Color.Black, 25);
                    }
                }

            }
        }

        spriteBatch.End();

        base.Draw(gameTime);
    }
}
