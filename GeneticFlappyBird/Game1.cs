using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System.Collections.Generic;

using NeuralNetworks;
using System;

namespace GeneticFlappyBird
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch spriteBatch;
        List<Pipe> pipes;
        Bird bird;
        double pipeDelay = 5000;
        int score = 0;
        SpriteFont spriteFont;
        KeyboardState previousKs;
        bool jump = false;
        bool gameOver = false;
        double totalTime = 0;
        Random random;
        Genetics network;
        Population[] population;

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
            bird = new Bird(new(new Vector2(40, 400), 20));
            spriteFont = Content.Load<SpriteFont>("ScoreFont");
            previousKs = new KeyboardState();
            random = new Random();
            network = new Genetics(0, 1, 3000, random);
            population = new Population[network.NetCount];
            for (int i = 0; i < population.Length; i++)
            {
                population[i] = new Population(new NeuralNetwork(ActivationFunctions.BinaryStep, ErrorFunctions.MSE, random, [2, 4, 1]), double.MaxValue, network);
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

            KeyboardState ks = Keyboard.GetState();

            if (!gameOver)
            {
                totalTime += gameTime.ElapsedGameTime.Milliseconds;
                pipeDelay += gameTime.ElapsedGameTime.Milliseconds;

                if (pipeDelay >= 2000)
                {
                    pipes.Add(new Pipe(2));
                    pipeDelay = 0;
                }

                foreach (var pipe in pipes)
                {
                    pipe.Move();
                    if (bird.Hitbox.Center.X >= pipe.Top.X + (pipe.Top.Width / 2) && pipe.passed == false)
                    {
                        score++;
                        pipe.passed = true;
                    }
                    if (pipe.Top.X < -100)
                    {
                        pipe.dead = true;
                    }
                    if (pipe.Top.Intersects((Rectangle)bird.Hitbox) || pipe.Bottom.Intersects((Rectangle)bird.Hitbox))
                    {
                        gameOver = true;
                    }
                }
                for (int i = pipes.Count - 1; i >= 0; i--)
                {
                    if (pipes[i].dead)
                    {
                        pipes.Remove(pipes[i]);
                    }
                }

                if (ks.IsKeyDown(Keys.Space) && previousKs.IsKeyUp(Keys.Space))
                {
                    jump = true;
                }

                bird.Move(ref jump);

                previousKs = ks;
            }
            else
            {
                pipes.Clear();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.SkyBlue);


            spriteBatch.Begin();

            if (!gameOver)
            {
                bird.Draw(spriteBatch);

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
