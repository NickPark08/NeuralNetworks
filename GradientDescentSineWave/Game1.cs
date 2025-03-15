using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGame.Extended;
using NeuralNetworks;

using System;
using System.Linq;

namespace GradientDescentSineWave
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        NeuralNetwork network;
        Random random;
        private double[][] inputs;
        private double[][] outputs;
        private double[][] newInputs;
        double min;
        double max;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferHeight = 800;
            graphics.PreferredBackBufferWidth = 1500;
            graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            random = new Random();
            network = new NeuralNetwork(ActivationFunctions.TanH, ErrorFunctions.MSE, random, .01, [1, 5, 5, 1]);

            int width = GraphicsDevice.Viewport.Width;
            inputs = new double[(int)(width / (Math.PI / 32 * 100))][];
            outputs = new double[(int)(width / (Math.PI / 32 * 100))][];
            newInputs = new double[inputs.Length / 2][]; 
            for (int i = 0; i < inputs.Length; i++)
            {
                inputs[i] = [Math.PI / 32 * i];
                outputs[i] = [Math.Sin(inputs[i][0])];
            }
            max = inputs.Max(m => m.Max());
            min = inputs.Min(m => m.Min());

            inputs = Normalize(inputs);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).board.Back == boardtate.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            Console.WriteLine(network.BatchTrain(inputs, outputs, 5, .01, .01));

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin();

            for (int i = 0; i < inputs.Length; i++)
            {
                float x = (float)(UnNormalize(inputs[i], max, min)[0] * 100);
                float actualY = (float)(Math.Sin(UnNormalize(inputs[i], max, min)[0]) * 100 + 400);
                float predictedY = (float)network.Compute(inputs[i])[0] * 100 + 400;

                spriteBatch.DrawPoint(new Vector2(x, actualY), Color.Black, 3);

                spriteBatch.DrawPoint(new Vector2(x, predictedY), Color.Red, 3);

            }
            //for (int i = 0; i < newInputs.Length; i++)
            //{
            //    float x = (float)(newInputs[i][0] * 100);
            //    float actualY = (float)(Math.Sin(newInputs[i][0]) * 100 + 400);
            //    float predictedY = (float)network.Compute(newInputs[i])[0] * 100 + 400;

            //    spriteBatch.DrawPoint(new Vector2(x, actualY), Color.Black, 3);

            //    spriteBatch.DrawPoint(new Vector2(x, predictedY), Color.Red, 3);

            //}

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public double[][] Normalize(double[][] inputs)
        {
            double max = inputs.Max(m => m.Max());
            double min = inputs.Min(m => m.Min());
            double[][] arr = new double[inputs.Length][];

            for (int j = 0; j < inputs.Length; j++)
            {
                arr[j] = new double[inputs[j].Length];
                for (int i = 0; i < inputs[j].Length; i++)
                {
                    arr[j][i] = ((inputs[j][i] - min) / (max - min)) * (1 -.1) + .1;
                }
            }
            return arr;
        }

        public double[] UnNormalize(double[] inputs, double max, double min)
        {
            min = 0;
            double[] arr = new double[inputs.Length];
            for (int i = 0; i < inputs.Length; i++)
            {
                arr[i] = ((inputs[i] - .1) / (1 - .1)) * (max - min) + min;
            }

            return arr;
        }
    }
}
