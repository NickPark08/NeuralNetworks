using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace HillClimberBestFit
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch spriteBatch;
        private MouseState previousMs;
        private List<Point> points;
        HillClimberLine line;
        private Random random;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferHeight = 800;
            _graphics.PreferredBackBufferWidth = 1000;
            _graphics.ApplyChanges();

            points = new List<Point>();
            random = new Random();
            line = new HillClimberLine(points, random);


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

            MouseState ms = Mouse.GetState();
            if (ms.LeftButton == ButtonState.Pressed && previousMs.LeftButton == ButtonState.Released)
            {
                points.Add(new Point(ms.X, ms.Y));
            }
                line.Run();


            previousMs = ms;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin();

            //spriteBatch.DrawLine(new(200, 0), new(200, 800), Color.Black, 3);
            //spriteBatch.DrawLine(new(0, 600), new(1000, 600), Color.Black, 3);
            spriteBatch.DrawLine(new(0, line.intercept), 4000f, line.slope, Color.Black);
            Window.Title = $"{line.GetError()}";


            foreach (var point in points)
            {
                spriteBatch.DrawEllipse(point.ToVector2(), new(5, 5), 20, Color.Black, 10f);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
