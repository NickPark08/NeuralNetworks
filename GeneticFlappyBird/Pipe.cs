using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGame.Extended;

namespace GeneticFlappyBird
{
    public class Pipe
    {
        public Rectangle Top;
        public Rectangle Bottom;
        public int Speed;
        Random random;
        public bool passed;
        public bool dead;

        public Pipe(int speed)
        {
            Top = new Rectangle(1500, 0, 100, 0); //250
            Bottom = new Rectangle(1500, 550, 100, 800); //550
            Speed = speed;
            random = new Random();
            passed = false;
            dead = false;
            Randomize(random);
        }
        void Randomize(Random gen)
        {
            int gap = gen.Next(150, 650);
            Top.Height = gap - 100;
            Bottom.Y = gap + 100;

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.FillRectangle(Top, Color.Green);
            spriteBatch.FillRectangle(Bottom, Color.Green);
        }

        public void Move()
        {
            Top.X -= Speed;
            Bottom.X -= Speed;
        }

    }
}
