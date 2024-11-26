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
        Rectangle Top;
        Rectangle Bottom;

        public Pipe()
        {
            Top = new Rectangle();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.FillRectangle(Top, Color.Green);
            spriteBatch.FillRectangle(Bottom, Color.Green);
        }

        //public void Randomize()
        //{

        //}
    }
}
