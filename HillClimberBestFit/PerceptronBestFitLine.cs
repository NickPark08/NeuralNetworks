using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HillClimberBestFit
{
    internal class PerceptronBestFitLine
    {
        double[] weights;
        double bias;
        double mutationRate;
        double biasMutationRate;
        Random random;
        Func<double, double, double> errorFunc;

        public PerceptronBestFitLine(double[] initialWeightValues, double initialBiasValue, double rate, Random ran, Func<double, double, double> func)
        {
            /*initializes the weights array and bias*/
            weights = initialWeightValues;
            bias = initialBiasValue;
            mutationRate = rate;
            random = ran;
            errorFunc = func;
            biasMutationRate = 10;
        }

        public PerceptronBestFitLine(int amountOfInputs, double rate, Random ran, Func<double, double, double> func)
        {
            weights = new double[amountOfInputs];
            mutationRate = rate;
            random = ran;
            errorFunc = func;
            biasMutationRate = 10;
        }

        private void Randomize(Random random, double min, double max)
        {
            for (int i = 0; i < weights.Length; i++)
            {
                weights[i] = (random.NextDouble() * min) + (max - min);
            }
        }

        private double Compute(double[] inputs)
        {
            double total = 0;
            for (int i = 0; i < inputs.Length; i++)
            {
                total += inputs[i] * weights[i];
            }

            return total + bias;
        }

        public double[] Compute(double[][] inputs)
        {
            double[] totals = new double[inputs.Length];
            for (int i = 0; i < inputs.Length; i++)
            {
                totals[i] = Compute(inputs[i]);
            }

            return totals;
        }

        public double GetError(double[][] inputs, double[] desiredOutputs)
        {
            double[] errors = new double[inputs.Length];
            for (int i = 0; i < inputs.Length; i++)
            {
                errors[i] = errorFunc(Compute(inputs[i]), desiredOutputs[i]);
            }

            double totalError = 0;
            foreach (var val in errors)
            {
                totalError += val;
            }

            return totalError / errors.Length;
        }

        public double TrainWithHillClimbing(double[][] inputs, double[] desiredOutputs, double currentError)
        {

            double[] previousWeights = new double[weights.Length];
            for(int i = 0; i < weights.Length; i++)
            {
                previousWeights[i] = weights[i];
            }
            double previousBias = bias;

            if (random.Next(2) == 0)
            {
                if (random.Next(2) == 0)
                {
                    weights[random.Next(weights.Length)] += mutationRate;
                }
                else
                {
                    weights[random.Next(weights.Length)] -= mutationRate;
                }
            }
            else
            {
                if (random.Next(2) == 0)
                {
                    bias += biasMutationRate;
                }
                else
                {
                    bias -= biasMutationRate;
                }
            }
            double error = GetError(inputs, desiredOutputs);
            if (error > currentError)
            {
                weights = previousWeights;
                bias = previousBias;
                return currentError;
            }

            return error;
        }
    }
}