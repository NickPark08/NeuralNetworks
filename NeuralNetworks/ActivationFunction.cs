using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks
{
    public class ActivationFunction
    {
        Func<double, double> function;
        Func<double, double> derivative;

        public ActivationFunction(Func<double, double> func, Func<double, double> deriv)
        {
            function = func;
            derivative = deriv;

            int a = Extensions.Add(5, 10);
        }

        public double Function(double input)
        {
            return function.Invoke(input);
        }

        public double Derivative(double input) 
        {
            return derivative.Invoke(input);
        }
    }
}
