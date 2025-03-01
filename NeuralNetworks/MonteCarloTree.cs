﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks
{
    public class MonteCarloTree<T> where T : IGameState<T>
    {
        public class Node
        {
            public T State { get; set; }
            public Node Parent;
            public Node[] Children;
            public double W;
            public double N;
            public bool isExpanded;
            const double C = 1.5;

            public Node(T state)
            {
                State = state;
                //Value = state.Value;
                Children = new Node[state.GetChildren().Length];
                W = 0;
                N = 0;
                isExpanded = false;
            }

            public double UCT()
            {
                if (N != 0)
                {
                    return (W / N) + C * Math.Sqrt(Math.Log(Parent.N) / N);
                }
                return double.PositiveInfinity;
            }

            public void GenerateChildren()
            {
                for(int i = 0; i < Children.Length; i++)
                {
                    Children[i] = new Node(State.GetChildren()[i]);
                    Children[i].Parent = this;
                }
                isExpanded = true;
            }

            //public override string ToString()
            //{
            //    return Value.ToString() + "\n" + State.ToString();
            //}
        }

        public Node root;

        public T MCTS(int iterations, T startingState, Random random)
        {
            // check bugs with children being null / not expanded

            root = new Node(startingState);
            for(int i = 0; i < iterations; i++)
            {
                var selectedNode = Select(root);
                var expandedNode = Expansion(selectedNode);
                int value = Simulation(expandedNode, random);
                Backprop(value, expandedNode);
            }

            var sortedNodes = root.Children.OrderByDescending(x => x.W);
            return sortedNodes.First().State;
        }


        internal static Node Select(Node rootNode)
        {
            Node currentNode = rootNode;

            while(currentNode.isExpanded)
            {
                Node highestChild = null;
                double highestUCT = double.NegativeInfinity;
                foreach(var child in currentNode.Children)
                {
                    double val = child.UCT();
                    if (val < highestUCT)
                    {
                        highestUCT = val;
                        highestChild = child;
                    }
                }
                if (highestChild == null) break;

                currentNode = highestChild;
            }

            return currentNode;
        }

        internal static Node Expansion(Node currentNode)
        {
            currentNode.GenerateChildren();

            if (currentNode.Children.Length == 0) return currentNode;

            return currentNode.Children[0];
        }

        internal static int Simulation(Node currentNode, Random random)
        {
            while(!currentNode.State.IsTerminal && currentNode.Children.Length != 0)
            {
                currentNode.GenerateChildren();
                int ranIndex = random.Next(currentNode.Children.Length);
                currentNode = currentNode.Children[ranIndex];
            }

            return currentNode.State.Value;
        }

        internal static void Backprop(int value, Node simulatedNode)
        {
            Node currentNode = simulatedNode;

            while(currentNode != null)
            {
                currentNode.N++;
                currentNode.W += -value;
                currentNode = currentNode.Parent;
            }
        }
    }
}
