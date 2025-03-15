using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using MonoGame.Extended;
using NeuralNetworks;
using System;
using System.Transactions;
using System.Linq;

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
        private List<double[]> inputs;
        private List<double> outputs;
        private double[] classes;

        bool train = false;

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

        //private double[][] Normalize(double[][] inputs)
        //{
        //    foreach (var arr in inputs)
        //    {
        //        for (int i = 0; i < arr.Length; i++)
        //        {
        //            arr[i] = ((arr[i] - min) / (max - min)) * (1 - 0) + 0;
        //        }
        //    }
        //    return inputs;
        //}

        //private double[] UnNormalize(double[] inputs)
        //{
        //    for (int i = 0; i < inputs.Length; i++)
        //    {
        //        inputs[i] = ((inputs[i] - 0) / (1 - 0)) * (max - min) + min;
        //    }

        //    return inputs;
        //}

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferHeight = 800;
            _graphics.PreferredBackBufferWidth = 1000;
            _graphics.ApplyChanges();

            errorFunc = ErrorFunc;
            random = new Random();
            points = new List<ClassifiedPoint>();
            perceptron = new Perceptron(2, .1, random, errorFunc);
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
            if (GamePad.GetState(PlayerIndex.One).board.Back == boardtate.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            MouseState ms = Mouse.GetState();
            KeyboardState ks = Keyboard.GetState();
            if (ms.LeftButton == boardtate.Pressed && previousMs.LeftButton == boardtate.Released && IsActive)
            {
                points.Add(new ClassifiedPoint(ms.X, ms.Y, 0));
                inputs.Add([ms.X, ms.Y]);
                outputs.Add(0);
                currentError = double.MaxValue;
            }

            if (ms.RightButton == boardtate.Pressed && previousMs.RightButton == boardtate.Released && IsActive)
            {
                points.Add(new ClassifiedPoint(ms.X, ms.Y, 1));
                inputs.Add([ms.X, ms.Y]);
                outputs.Add(1);
                currentError = double.MaxValue;

            }

            if (train && points.Count > 0)
            {
                //inputs = perceptron.Normalize(inputs.ToArray()).ToList();
                currentError = perceptron.TrainWithHillClimbing(perceptron.Normalize(inputs.ToArray()), outputs.ToArray(), currentError);
                classes = perceptron.UnNormalize(perceptron.Compute(perceptron.Normalize(inputs.ToArray())), perceptron.Normalize(inputs.ToArray()).Max(m => m.Max()), perceptron.Normalize(inputs.ToArray()).Min(m => m.Min()));
            }
            //classes = UnNormalize(perceptron.Compute(inputs.ToArray()));

            if (ks.IsKeyDown(Keys.Space) && previousKs.IsKeyUp(Keys.Space))
            {
                train = !train;
            }


            previousMs = ms;
            previousKs = ks;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin();

            Window.Title = $"{currentError}";

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

            //if (classes.Length > 0)
            //{
            //    inputs = UnNormalize(inputs.ToArray()).ToList();

            //}

            //unnormalize inputs to draw circles
            

            for (int i = 0; i < classes.Length; i++)
            {
                if (Math.Round(classes[i]) <= 0)
                {
                    spriteBatch.DrawEllipse(new((float)inputs[i][0], (float)inputs[i][1]), new(9, 9), 20, Color.Blue);
                }
                else
                {
                    spriteBatch.DrawEllipse(new((float)inputs[i][0], (float)inputs[i][1]), new(9, 9), 20, Color.Red);
                }
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
