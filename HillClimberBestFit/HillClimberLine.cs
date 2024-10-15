using System;
using System.Collections.Generic;
//using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;


namespace HillClimberBestFit
{
    internal class HillClimberLine
    {
        List<Point> Target;
        Random random;
        public Point intercept;
        public float slope;

        public HillClimberLine(List<Point> target, Random ran)
        {
            Target = target;
            random = ran;
            intercept = new Point();
            slope = 0;
        }

        public void Run()
        {
            while (GetError() <= 0.5f)
            {
                Point tempInt = intercept;
                float tempSlope = slope;
                double tempError = GetError();
                Mutate();

                if (tempError < GetError())
                {
                    slope = tempSlope;
                    intercept = tempInt;
                }
            }

        }

        public void Mutate()
        {
            if (random.Next(2) == 0)
            {
                if(random.Next(2) == 0)
                {
                    slope++;
                }
                else
                {
                    slope--;
                }
            }
            else
            {
                if(random.Next(2) == 0)
                {
                    intercept.Y++;
                }
                else
                {
                    intercept.Y--;
                }
            }
        }

        private double GetError()
        {
            double error = 0;
            foreach(var point in Target)
            {
                error += Math.Abs(point.Y - (slope * point.X + intercept.Y));
            }

            return error / Target.Count();
        }
    }
}
