﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks
{
    public class Population
    {
        public NeuralNetwork Network;
        public double Fitness;
        public GeneticGates Genetics;
        public Population(NeuralNetwork net, double fit, GeneticGates genetics) 
        {
            Network = net;
            Fitness = fit;
            Genetics = genetics;
        }

    }
}
