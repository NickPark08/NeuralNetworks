using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks
{
    public class Dendrite
    {
        public Neuron Previous { get; set; }
        public Neuron Next { get; set; }
        public double Weight { get; set; }
        public double WeightUpdate { get; set; }
        double previousWeightUpdate = 0;

        public Dendrite(Neuron previous, Neuron next, double weight)
        {
            Previous = previous;
            Next = next;
            Weight = weight;
        }
       
        public double Compute()
        {
            return Previous.Output * Weight;
        }
        public void ApplyUpdate(double momentum)
        {
            WeightUpdate += previousWeightUpdate * momentum;
            Weight += WeightUpdate;
            previousWeightUpdate = WeightUpdate;
            WeightUpdate = 0;
        }
    }
}
