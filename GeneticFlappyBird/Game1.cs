using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System.Collections.Generic;

using NeuralNetworks;
using System;
using System.Linq;
using MonoGame.Extended;

namespace GeneticFlappyBird
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch spriteBatch;
        List<Pipe> pipes;
        //Bird bird;
        double pipeDelay = 5000;
        int score = 0;
        SpriteFont spriteFont;
        bool gameOver = false;
        double totalTime = 0;
        Random random;
        Genetics network;
        Population[] population;
        Bird[] birds;
        bool birdsDead = true;
        float speed = 2;
        double pipeTime = 4000;

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

            pipes = new List<Pipe>();
            //bird = new Bird(new(new Vector2(40, 400), 20));
            random = new Random();
            network = new Genetics(0, 1, 100, random);
            population = new Population[network.NetCount];
            birds = new Bird[network.NetCount];
            for (int i = 0; i < population.Length; i++)
            {
                population[i] = new Population(new NeuralNetwork(ActivationFunctions.BinaryStep, ErrorFunctions.MSE, random, [2, 4, 1]), double.MaxValue, network);
                birds[i] = new Bird(new(new Vector2(40, 400), 20), population[i].Network);

            }

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            spriteFont = Content.Load<SpriteFont>("ScoreFont");

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            KeyboardState ks = Keyboard.GetState();

            speed += .001f;

            if (!gameOver)
            {
                totalTime += gameTime.ElapsedGameTime.Milliseconds;
                pipeDelay += gameTime.ElapsedGameTime.Milliseconds;

                pipeTime -= .5;

                if (pipeDelay >= pipeTime)
                {
                    if (pipes.Count > 0)
                    {
                        pipes.Add(new Pipe(pipes[0].Speed));
                    }
                    else
                    {
                        pipes.Add(new Pipe(2));
                    }
                    pipeDelay = 0;
                }


                for (int i = pipes.Count - 1; i >= 0; i--)
                {
                    pipes[i].Speed += .01f;
                    pipes[i].Move(birds);
                    if (pipes[i].dead)
                    {
                        pipes.Remove(pipes[i]);
                    }
                }

                foreach (var bird in birds)
                {
                    if (pipes.Count > 0)
                    {
                        bird.inputs[0] = pipes[0].Top.X - bird.Hitbox.Center.X;
                        bird.inputs[1] = bird.Hitbox.Center.Y - pipes[0].gap;
                    }

                    if (bird.Network.Compute(bird.inputs)[0] >= 0)//ks.IsKeyDown(Keys.Space) && previousKs.IsKeyUp(Keys.Space)) // change to output of genetic learning
                    {
                        bird.jump = true;
                    }

                    bird.Move(pipes, totalTime);
                }

                birdsDead = true;
                foreach(var bird in birds)
                {
                    if(bird.dead == false)
                    {
                        birdsDead = false;
                    }
                }

                if(birdsDead == true)
                {
                    gameOver = true;
                }
            }
            else
            {
                for (int i = 0; i < population.Length; i++)
                {
                    population[i].Fitness = network.FlappyBirdFitness(birds[i].time, birds[i].score);
                }

                network.Train(population, random, .01);

                pipes.Clear();

                foreach(var bird in birds)
                {
                    bird.Hitbox = new CircleF(new Vector2(40, 400), 20);
                    bird.dead = false;
                }
                gameOver = false;
                birdsDead = false;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.SkyBlue);


            spriteBatch.Begin();

            if (!gameOver)
            {
                foreach (var bird in birds)
                {
                    bird.Draw(spriteBatch);
                }

                foreach (var pipe in pipes)
                {
                    pipe.Draw(spriteBatch);
                }

                spriteBatch.DrawString(spriteFont, $"Score: {score}", new(20, 20), Color.Black);
            }

            else
            {
                GraphicsDevice.Clear(Color.White);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
