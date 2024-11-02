using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using MonoGame.Extended;
using NeuralNetworks;
using System;

namespace PerceptronLinearClassification
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch spriteBatch;
        private List<ClassifiedPoint> points;
        private MouseState previousMs;
        private KeyboardState previousKs;
        private Perceptron perceptron;
        private Random random;
        private Func<double, double, double> errorFunc;
        private double currentError;
        private double[][] inputs;
        private double[] outputs;
        private double[] classes;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }
        private double ErrorFunc(double actual, double desired)
        {
            return Math.Pow(desired - actual, 2);
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferHeight = 800;
            _graphics.PreferredBackBufferWidth = 1000;
            _graphics.ApplyChanges();

            errorFunc = ErrorFunc;
            random = new Random();
            points = new List<ClassifiedPoint>();
            perceptron = new Perceptron(2, 1, random, errorFunc);
            currentError = double.MaxValue;
            inputs = [];
            outputs = [];
            classes = [];

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
            KeyboardState ks = Keyboard.GetState();
            if (ms.LeftButton == ButtonState.Pressed && previousMs.LeftButton == ButtonState.Released && IsActive)
            {
                points.Add(new ClassifiedPoint(ms.X, ms.Y, 0));
            }

            if (ms.RightButton == ButtonState.Pressed && previousMs.RightButton == ButtonState.Released && IsActive)
            {
                points.Add(new ClassifiedPoint(ms.X, ms.Y, 1));
            }

            if (ks.IsKeyDown(Keys.Space) && previousKs.IsKeyUp(Keys.Space))
            {
                inputs = new double[points.Count][];
                outputs = new double[points.Count];
                classes = new double[points.Count];

                for (int i = 0; i < points.Count; i++)
                {
                    //add inputs when adding points so training works
                    inputs[i] = new double[2];
                    outputs[i] = points[i].Class;
                    inputs[i][0] = points[i].Point.X;
                    inputs[i][1] = points[i].Point.Y;
                }
            }
            if (points.Count > 0)
            {
                currentError = perceptron.TrainWithHillClimbing(inputs, outputs, currentError);
                classes = perceptron.Compute(inputs);
            }




            previousMs = ms;
            previousKs = ks;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin();
            foreach (var point in points)
            {
                if (point.Class == 0)
                {
                    spriteBatch.DrawEllipse(point.Point, new(5, 5), 20, Color.Blue, 10f);
                }
                else
                {
                    spriteBatch.DrawEllipse(point.Point, new(5, 5), 20, Color.Red, 10f);
                }
            }

            for (int i = 0; i < inputs.Length; i++)
            {
                if (classes[i] == 0)
                {
                    spriteBatch.DrawEllipse(new((float)inputs[i][0], (float)inputs[i][1]), new(8, 8), 20, Color.Blue);
                }
                else
                {
                    spriteBatch.DrawEllipse(new((float)inputs[i][0], (float)inputs[i][1]), new(8, 8), 20, Color.Red);
                }
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
