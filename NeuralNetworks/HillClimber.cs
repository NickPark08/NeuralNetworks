using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks
{
    internal class HillClimber
    {
        string Target;
        string randomString;
        Random random;
        //convert ints to chars
        public HillClimber(string target, Random ran)
        {
            Target = target;
            random = ran;
            for(int i = 0; i < Target.Length; i++)
            {
                randomString += random.Next(32, 127);
            }
        }

        public string Run()
        {
            while(GetError() != 0)
            {
                Mutate();
            }

            return randomString;
        }

        private double GetError()
        {
            int error = 0;
            for(int i = 0; i < Target.Length; i++)
            {
                error += Math.Abs(Target[i] - randomString[i]);
            }
            return error / Target.Length;
        }

        private void Mutate()
        {
            StringBuilder sb = new StringBuilder(randomString, Target.Length);
            int ranIndex = random.Next(Target.Length);
            if (randomString[ranIndex] == 32)
            {
                sb[random.Next(Target.Length)]++;
            }
            else if (randomString[ranIndex] == 126)
            {
                sb[random.Next(Target.Length)]--;
            }
            else
            {
                if(random.Next(2) == 1)
                {
                    sb[random.Next(Target.Length)]++;
                }
                else
                {
                    sb[random.Next(Target.Length)]--;
                }
            }

        }
    }
}
