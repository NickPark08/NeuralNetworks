using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGame.Extended;

using NeuralNetworks;

using System;
using System.Linq;

namespace GeneticSineWave
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Genetics genetics;
        private Population[] population;
        private Random random;
        private double[][] inputs;
        private double[][] outputs;

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

            genetics = new Genetics(0, 1, 500, random);
            population = new Population[genetics.NetCount];

            int width = GraphicsDevice.Viewport.Width;
            inputs = new double[(int)(width / (Math.PI / 32 * 100))][];
            outputs = new double[(int)(width / (Math.PI / 32 * 100))][];
            for (int i = 0; i < inputs.Length; i++)
            {
                inputs[i] = [Math.PI / 32 * i];
                outputs[i] = [Math.Sin(inputs[i][0])];
            }

            for (int i = 0; i < population.Length; i++)
            {
                population[i] = new Population(
                    new NeuralNetwork(ActivationFunctions.Sigmoid, ErrorFunctions.MSE, random, .01, [ 1, 7, 1 ]),
                    double.MaxValue,
                    genetics
                );
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).board.Back == boardtate.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //check fitness, check only middle
            //if (population.Any(p => p.Fitness != 0))
            //{
            foreach (var pop in population)
            {
                pop.Fitness = genetics.SineWaveFitness(pop.Network, inputs, outputs);
            }
            Console.WriteLine(population[0].Fitness);

            genetics.Train(population, random, 0.3);
            //}

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin();

            for (int i = 0; i < inputs.Length; i++)
            {
                float x = (float)(inputs[i][0] * 100);
                float actualY = (float)(Math.Sin(inputs[i][0]) * 100 + 400);
                float predictedY = (float)(population[0].Network.Compute(inputs[i])[0] * 100 + 400);

                spriteBatch.DrawPoint(new Vector2(x, actualY), Color.Black, 3);

                spriteBatch.DrawPoint(new Vector2(x, predictedY), Color.Red, 3);
                spriteBatch.DrawLine(new Vector2((float)(Math.PI / 2) *100, 0), 800, (float)(Math.PI / 2), Color.Green, 3);

            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
