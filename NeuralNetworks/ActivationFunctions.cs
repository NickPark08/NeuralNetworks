using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks
{
    public static class ActivationFunctions
    {
        public static ActivationFunction Identity => new ActivationFunction(x => x, x => 1);

        public static ActivationFunction BinaryStep => new ActivationFunction(Math.Round, x => 0);

        public static ActivationFunction Sigmoid => new ActivationFunction(x => 1 / (1 + Math.Pow(Math.E, -x)), x => 1 / (1 + Math.Pow(Math.E, -x) * (1 - (1 / (1 + Math.Pow(Math.E, -x))))));

        public static ActivationFunction TanH => new ActivationFunction(Math.Tanh, x => 1 - Math.Pow(Math.Tanh(x), 2));//(Math.tanh(Math.E, x) - Math.Pow(Math.E, -x)) / (Math.Pow(Math.E, x) + Math.Pow(Math.E, -x)), x => 1 - Math.Pow((Math.Pow(Math.E, x) - Math.Pow(Math.E, -x)) / (Math.Pow(Math.E, x) + Math.Pow(Math.E, -x)), 2));

        public static ActivationFunction ReLU => new ActivationFunction(x => x <= 0 ? 0 : x, x => x <= 0 ? 0 : 1);
    }

    public static class ErrorFunctions
    {
        //public static ErrorFunction Identity => new ErrorFunction((x, y) => x-y, (x, y) => 1);

        public static ErrorFunction MAE => new ErrorFunction((x, y) => Math.Abs(x - y), (x, y) => (x - y) >= 0 ? -1 : 0);
        public static ErrorFunction MSE => new ErrorFunction((x, y) => Math.Pow(x - y, 2), (x, y) => -2 * (x - y));
    }
}
