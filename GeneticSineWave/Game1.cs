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

            genetics = new Genetics(0, 1, 100, random);
            population = new Population[genetics.NetCount];

            int width = GraphicsDevice.Viewport.Width;
            inputs = new double[width][];
            outputs = new double[width][];
            for (int i = 0; i < width; i++)
            {
                inputs[i] = [ i / 100.0 ];
                outputs[i] = [ Math.Sin(inputs[i][0]) ];
            }

            for (int i = 0; i < population.Length; i++)
            {
                population[i] = new Population(
                    new NeuralNetwork(ActivationFunctions.Sigmoid, ErrorFunctions.MSE, random, [ 1, 3, 1 ]),
                    double.MaxValue,
                    genetics
                );
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //check fitness
            if (population.Any(p => p.Fitness > 0.01))
            {
                foreach (var pop in population)
                {
                    pop.Fitness = genetics.SineWaveFitness(pop.Network, inputs, outputs);
                }

                genetics.Train(population, random, 0.01);
            }

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
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
