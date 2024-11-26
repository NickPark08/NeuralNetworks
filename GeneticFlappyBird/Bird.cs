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
        CircleF Hitbox;
        public Bird(CircleF hitbox) 
        {
            Hitbox = hitbox;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawCircle(Hitbox, 15, Color.Yellow, 20);
        }
    }
}
