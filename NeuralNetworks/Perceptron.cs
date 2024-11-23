using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks
{
    public class Perceptron
    {
        double[] weights;
        double bias;
        double mutationRate;
        Random random;
        //Func<double, double, double> errorFunc;
        ErrorFunction errorFunc;
        public ActivationFunction activationFunc;
        public double learningRate { get; set; }
        List<double> adjustments = new List<double>();

        public Perceptron(double[] initialWeightValues, double initialBiasValue, double rate, Random ran, ErrorFunction func, ActivationFunction activation)
        {
            /*initializes the weights array and bias*/
            weights = initialWeightValues;
            bias = initialBiasValue;
            mutationRate = rate;
            random = ran;
            errorFunc = func;
            activationFunc = activation;
        }

        public Perceptron(int amountOfInputs, double rate, Random ran, ErrorFunction func, ActivationFunction activation)
        {
            weights = new double[amountOfInputs];
            mutationRate = rate;
            learningRate = rate;
            random = ran;
            errorFunc = func;
            activationFunc = activation;
            Randomize(random, 0, 1);
        }

        private void Randomize(Random random, double min, double max)
        {
            for (int i = 0; i < weights.Length; i++)
            {
                weights[i] = (random.NextDouble() * (max - min)) + min;
            }
            bias = (random.NextDouble() * (max - min)) + min;
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

        public double GetError(double[][] inputs, double[] desiredOutputs)
        {
            double sum = 0;
            for (int i = 0; i < inputs.Length; i++)
            {
                sum += errorFunc.Function(Compute(inputs[i]), desiredOutputs[i]);
            }
            return sum / desiredOutputs.Length;
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
                //if (weights[index] <= mutationRate)
                //{
                //    weights[index] += mutationRate;
                //}
                //else
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
            if (error >= currentError)
            {
                weights = previousWeights;
                bias = previousBias;
                return currentError;
            }

            return error;
        }

        public double Train(double[] inputs, double desiredOutput)
        {
            double computedOutput = activationFunc.Function(Compute(inputs));

            double error = errorFunc.Function(computedOutput, desiredOutput);
            double gradient = errorFunc.Derivative(computedOutput, desiredOutput) * activationFunc.Derivative(Compute(inputs));

            for (int i = 0; i < weights.Length; i++)
            {
                weights[i] -= learningRate * gradient * inputs[i];
            }
            bias -= learningRate * gradient;

            return error;
        }


        public double BatchTrain(double[][] inputs, double[] desiredOutputs)
        {
            double[] weightGradients = new double[weights.Length];
            double biasGradient = 0;
            double totalError = 0;

            for (int i = 0; i < inputs.Length; i++)
            {
                double computedOutput = activationFunc.Function(Compute(inputs[i]));
                double error = errorFunc.Function(computedOutput, desiredOutputs[i]);
                totalError += error;

                double gradient = errorFunc.Derivative(computedOutput, desiredOutputs[i]) * activationFunc.Derivative(Compute(inputs[i]));

                for (int j = 0; j < weights.Length; j++)
                {
                    weightGradients[j] += gradient * inputs[i][j];
                }
                biasGradient += gradient;
            }

            for (int j = 0; j < weights.Length; j++)
            {
                weights[j] -= learningRate * (weightGradients[j] / inputs.Length);
            }
            bias -= learningRate * (biasGradient / inputs.Length);

            return totalError / inputs.Length;
        }


        public double[][] Normalize(double[][] inputs)
        {
            double max = inputs.Max(m => m.Max());
            double min = inputs.Min(m => m.Min());
            double[][] arr = new double[inputs.Length][];

            for (int j = 0; j < inputs.Length; j++)
            {
                arr[j] = new double[inputs[j].Length];
                for (int i = 0; i < inputs[j].Length; i++)
                {
                    arr[j][i] = ((inputs[j][i] - min) / (max - min)) * (1 - 0) + 0;
                }
            }
            return arr;
        }

        public double[] UnNormalize(double[] inputs, double max, double min)
        {
            double[] arr = new double[inputs.Length];
            for (int i = 0; i < inputs.Length; i++)
            {
                arr[i] = ((inputs[i] - 0) / (1 - 0)) * (max - min) + min;
            }

            return arr;
        }
    }
}
