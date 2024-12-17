using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoGame.Extended;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticFlappyBird
{
    public class Bird
    {
        public CircleF Hitbox;
        float Speed;
        public Bird(CircleF hitbox) 
        {
            Hitbox = hitbox;
            Speed = 3;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawCircle(Hitbox, 15, Color.Yellow, 20);
        }

        public void Move(ref bool jump)
        {
            Hitbox.Center.Y += Speed;
            Speed += 0.15f;

            if(jump)
            {
                Speed = -5;
                jump = false;
            }
        }
    }
}
