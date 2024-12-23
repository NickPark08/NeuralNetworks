using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoGame.Extended;
using NeuralNetworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace GeneticFlappyBird
{
    public class Bird
    {
        public NeuralNetwork Network;
        public CircleF Hitbox;
        float Speed;
        public bool dead;
        public double[] inputs;
        public bool jump;
        public int score;
        public double time;
        public Bird(CircleF hitbox, NeuralNetwork net)
        {
            Hitbox = hitbox;
            Speed = 3;
            Network = net;
            dead = false;
            inputs = new double[2];
            score = 0;
            time = 0;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawCircle(Hitbox, 15, Color.Yellow, 20);
        }

        public void Move(List<Pipe> pipes, double gameTime)
        {
            if (!dead)
            {
                Hitbox.Center.Y += Speed;
                Speed += 0.3f;

                if (jump)
                {
                    Speed = -4;
                    jump = false;
                }

                foreach (var pipe in pipes)
                {
                    if (Hitbox.Center.X >= pipe.Top.X + (pipe.Top.Width / 2) && pipe.passed == false)
                    {
                        pipe.passed = true;
                        score++;
                    }
                    if (pipe.Top.X < -100)
                    {
                        pipe.dead = true;
                    }
                    if (pipe.Top.Intersects((Rectangle)Hitbox) || pipe.Bottom.Intersects((Rectangle)Hitbox))
                    {
                        dead = true;
                        time = gameTime;
                        Hitbox = new(new(-100, -100), 20);
                    }
                }
                if (Hitbox.Center.Y <= -25 || Hitbox.Center.Y >= 800)
                {
                    dead = true;
                    time = gameTime;
                    Hitbox = new(new(-100, -100), 20);
                }
            }
        }
    }
}
