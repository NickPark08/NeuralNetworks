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
        private GraphicsDeviceManager _graphics;
        private SpriteBatch spriteBatch;
        Genetics network;
        Population[] population;
        Random random;
        double[][] inputs;
        double[][] outputs;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferHeight = 800;
            _graphics.PreferredBackBufferWidth = 1500;
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            random = new Random();
            network = new Genetics(0, 1, 100, random);
            population = new Population[network.NetCount];
            inputs = new double[GraphicsDevice.Viewport.Width][];
            outputs = new double[inputs.Length][];
            for (int i = 0; i < population.Length; i++)
            {
                population[i] = new Population(new NeuralNetwork(ActivationFunctions.Sigmoid, ErrorFunctions.MSE, random, [1, 3, 1]), double.MaxValue, network);
            }
            for(int i = 0; i < inputs.Length; i++)
            {
                inputs[i] = [i];
                outputs[i] = [inputs[i][0] / 10];
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (population[0].Fitness == 0)
            {
                foreach (var pop in population)
                {
                    pop.Fitness = network.SineWaveFitness(pop.Network, inputs, outputs);
                }

                network.Train(population, random, .01);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin();

            for(int i = 0; i < inputs.Length; i++)
            {
                spriteBatch.DrawPoint(new((float)inputs[i][0], (float)(population[0].Network.Compute(outputs[i])[0] + 400)), Color.Red, 3);
                spriteBatch.DrawPoint(new((float)inputs[i][0], (float)((Math.Sin(outputs[i][0]) * 25) + 400)), Color.Black, 3);

            }

            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
