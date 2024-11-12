using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks
{
    public class ErrorFunction
    {
        Func<double, double, double> function;
        Func<double, double, double> derivative;
        public ErrorFunction(Func<double, double, double> func, Func<double, double, double> deriv) 
        {
            function = func;
            derivative = deriv;
        }

        public double Function(double output, double desiredOutput) 
        {
            return function.Invoke(output, desiredOutput);
        }
        public double Derivative(double output, double desiredOutput) 
        {
            return derivative.Invoke(output, desiredOutput);
        }
    }
}
