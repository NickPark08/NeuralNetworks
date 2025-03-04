using System;
using System.Collections.Generic;
using System.Linq;

namespace NeuralNetworks
{
    public class MonteCarloTree<T> where T : IGameState<T>
    {
        public class Node
        {
            public T State { get; set; }
            public Node Parent;
            public List<Node> Children;
            public double W;
            public double N;
            public bool isExpanded;
            private const double C = 1.5;

            public Node(T state, Node parent = null)
            {
                State = state;
                Parent = parent;
                Children = new List<Node>();
                W = 0;
                N = 0;
                isExpanded = false;
            }

            public double UCT()
            {
                if (N == 0) return double.PositiveInfinity;
                return (W / N) + C * Math.Sqrt(Math.Log(Parent.N) / N);
            }

            public void GenerateChildren()
            {
                if (isExpanded) return;

                var childStates = State.GetChildren();
                foreach (var childState in childStates)
                {
                    Children.Add(new Node(childState, this));
                }
                isExpanded = true;
            }

            public override string ToString()
            {
                return $"{isExpanded}";
            }
        }

        public Node root;

        public T MCTS(int iterations, T startingState, Random random)
        {
            root = new Node(startingState);

            for (int i = 0; i < iterations; i++)
            {
                var selectedNode = Select(root);
                var expandedNode = Expansion(selectedNode);
                int value = Simulation(expandedNode, random);
                Backprop(value, expandedNode);
            }

            return root.Children.OrderByDescending(x => (x.W / x.N)).First().State;
        }

        private static Node Select(Node rootNode)
        {
            Node currentNode = rootNode;
            while (currentNode.isExpanded)
            {
                Node highestUCTChild = null;
                double highestUCT = double.NegativeInfinity;
                foreach (var child in currentNode.Children)
                {
                    double val = child.UCT();
                    if (val > highestUCT)
                    {
                        highestUCT = val;
                        highestUCTChild = child;
                    }
                }
                if (highestUCTChild == null) break;
                currentNode = highestUCTChild;
            }
            return currentNode;
        }

        private static Node Expansion(Node currentNode)
        {
            if (!currentNode.State.IsTerminal)
            {
                currentNode.GenerateChildren();
                if (currentNode.Children.Count != 0) return currentNode.Children[0];
            }
            return currentNode;
        }

        private static int Simulation(Node currentNode, Random random)
        {

            while (!currentNode.State.IsTerminal)
            {
                currentNode.GenerateChildren();
                if(currentNode.Children.Count == 0)
                {
                    ;
                }
                int randomIndex = random.Next(0, currentNode.Children.Count);
                currentNode = currentNode.Children[randomIndex];
            }

            return currentNode.State.Value;
        }

        private static void Backprop(int value, Node simulatedNode)
        {
            Node currentNode = simulatedNode;
            while (currentNode != null)
            {
                value = -value;
                currentNode.N++;
                currentNode.W += value;
                currentNode = currentNode.Parent;
            }
        }
    }
}
