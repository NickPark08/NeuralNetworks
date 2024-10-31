using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks
{
    internal class Perceptron
    {
        double[] weights;
        double bias;
        double mutationRate;
        Random random;
        Func<double, double, double> errorFunc;

        public Perceptron(double[] initialWeightValues, double initialBiasValue, double rate, Random ran, Func<double, double, double> func)
        {
            /*initializes the weights array and bias*/
            weights = initialWeightValues;
            bias = initialBiasValue;
            mutationRate = rate;
            random = ran;
            errorFunc = func;
        }

        public Perceptron(int amountOfInputs, double rate, Random ran, Func<double, double, double> func)
        {
            weights = new double[amountOfInputs];
            mutationRate = rate;
            random = ran;
            errorFunc = func;
            Randomize(random, 0, 1);
        }

        private void Randomize(Random random, double min, double max)
        {
            for (int i = 0; i < weights.Length; i++)
            {
                weights[i] = (random.NextDouble() * max) + min;
            }
        }

        public double Compute(double[] inputs)
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

        private double GetError(double[][] inputs, double[] desiredOutputs)
        {
            double[] errors = new double[inputs.Length];
            for(int i = 0; i < inputs.Length; i++)
            {
                errors[i] = errorFunc(Compute(inputs[i]), Compute(desiredOutputs));
            }

            double totalError = 0;
            foreach(var val in errors)
            {
                totalError += val;
            }

            return totalError / errors.Length;
        }

        public double TrainWithHillClimbing(double[][] inputs, double[] desiredOutputs, double currentError)
        {
            double[] previousWeights = new double[weights.Length];
            for (int i = 0; i < weights.Length; i++)
            {
                previousWeights[i] = weights[i];
            }
            double previousBias = bias;

            if (random.Next(2) == 0)
            {
                int index = random.Next(weights.Length);
                if (weights[index] <= mutationRate)
                {
                    weights[index] += mutationRate;
                }
                else
                {
                    if (random.Next(2) == 0)
                    {
                        weights[index] += mutationRate;
                    }
                    else
                    {
                        weights[index] -= mutationRate;
                    }
                }
            }
            else
            {
                if (random.Next(2) == 0)
                {
                    bias += mutationRate;
                }
                else
                {
                    bias -= mutationRate;
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
