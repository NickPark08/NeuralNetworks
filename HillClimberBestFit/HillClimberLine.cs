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
        public int intercept;
        public float slope;
        public float mRate = 0.001f;
        public int bRate = 1;

        public HillClimberLine(List<Point> target, Random ran)
        {
            Target = target;
            random = ran;
            intercept = 0;
            slope = 0;
        }

        public void Run()
        {
            int count = 0;
            while (count < 10)
            {
                int tempInt = intercept;
                float tempSlope = slope;
                double tempError = GetError();
                Mutate();

                if (tempError < GetError())
                {
                    slope = tempSlope;
                    intercept = tempInt;
                }
                count++;
            }

        }

        private void Mutate()
        {
            if (random.Next(2) == 0)
            {
                if(random.Next(2) == 0)
                {
                    slope += mRate;
                }
                else
                {
                    slope -= mRate;
                }
            }
            else
            {
                if(random.Next(2) == 0)
                {
                    intercept += bRate;
                }
                else
                {
                    intercept -= bRate;
                }
            }
        }

        public double GetError()
        {
            double error = 0;
            foreach(var point in Target)
            {
                error += Math.Pow((slope * point.X + intercept) - point.Y, 2);
            }

            return error / Target.Count();
        }
    }
}
