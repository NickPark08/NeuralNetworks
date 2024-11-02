using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PerceptronLinearClassification
{
    internal class ClassifiedPoint
    {
        public Vector2 Point;
        public int Class;

        public ClassifiedPoint(float x, float y, int cl)
        {
            Point = new Vector2(x, y);
            Class = cl;
        }
    }
}
