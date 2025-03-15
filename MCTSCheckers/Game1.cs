using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using NeuralNetworks;
using MonoGame.Extended;

using System;

namespace MCTSCheckers;

public class Game1 : Game
{
    private GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch;

    Rectangle[,] board;

    const int squareSize = 100;

    public Game1()
    {
        graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

    }

    protected override void Initialize()
    {
        board = new Rectangle[8, 8];
        Size ClientSize = new Size(squareSize * board.GetLength(0),squareSize * board.GetLength(0));

        graphics.PreferredBackBufferWidth = ClientSize.Width;
        graphics.PreferredBackBufferHeight = ClientSize.Height;
        graphics.ApplyChanges();

        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                board[i, j] = new Rectangle(squareSize * i, squareSize * j, squareSize, squareSize);
            }
        }


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
        GraphicsDevice.Clear(Color.CornflowerBlue);

        spriteBatch.Begin();

        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1) - 1; j+= 2)
            {
                if (i % 2 == 0)
                {
                    spriteBatch.FillRectangle(board[i, j], Color.LightGray);
                    spriteBatch.FillRectangle(board[i, j + 1], Color.DarkGray);
                }
                else if (i % 2 == 1)
                {
                    spriteBatch.FillRectangle(board[i, j], Color.DarkGray);
                    spriteBatch.FillRectangle(board[i, j + 1], Color.LightGray);

                }
            }
        }

        spriteBatch.End();

        base.Draw(gameTime);
    }
}
