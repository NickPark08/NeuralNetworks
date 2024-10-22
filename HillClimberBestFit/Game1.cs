using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
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
        PerceptronBestFitLine perceptronLine;
        Func<double, double, double> errorFunc;
        int averageX;
        int averageY;
        Point initialPoint;
        float slope;
        float yIntercept;

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
            errorFunc = ErrorFunc;
            perceptronLine = new PerceptronBestFitLine(points.Count(), 0.1, random, errorFunc);


            base.Initialize();
        }

        private double ErrorFunc(double actual, double desired)
        {
            return Math.Pow(desired - actual, 2);
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
            if (ms.LeftButton == ButtonState.Pressed && previousMs.LeftButton == ButtonState.Released && IsActive)
            {
                points.Add(new Point(ms.X, ms.Y));
            }


        if (points.Count() > 1)
            {
                averageY = 0;
                averageX = 0;
                initialPoint = points[0];
                foreach (var point in points)
                {
                    averageX += point.X;
                    averageY += point.Y;
                }
                averageX /= points.Count();
                averageY /= points.Count();
                slope = (float)(averageY - initialPoint.Y) / (averageX - initialPoint.X);
                //y = mx + b -> b = y - mx
                yIntercept = averageY - (slope * averageX);
            }

            previousMs = ms;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            //manual best fit
            //initial point, averaged point, calcultae line


            spriteBatch.Begin();

            if(points.Count() > 1)
            {
                spriteBatch.DrawLine(new(0, yIntercept), 4000f, slope, Color.Black);
            }

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
