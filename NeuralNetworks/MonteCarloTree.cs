using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NeuralNetworks
{
    public class MonteCarloTree<T> where T : IGameState<T, T[]>
    {
        public class Node
        {
            public T State { get; set; }
            public Node Parent;
            public List<Node> Children;
            public double W;
            public double N;
            public bool isExpanded;
            public bool xTurn;
            private const double C = 1.5;

            public Node(T state, bool isXTurn, Node parent = null)
            {
                State = state;
                Parent = parent;
                Children = new List<Node>();
                W = 0;
                N = 0;
                isExpanded = false;
                xTurn = isXTurn;
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
                    Children.Add(new Node(childState, !xTurn, this));
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

            //make sure root.xTurn is correct
            root = new Node(startingState, root.xTurn);

            for (int i = 0; i < iterations; i++)
            {
                var selectedNode = Select(root);
                var expandedNode = Expansion(selectedNode);
                var testNode = Simulation(expandedNode, random);
                //int value = Simulation(expandedNode, random);
                Backprop(Math.Abs(testNode.State.Value), testNode);
            }


            var orderedKids = root.Children.OrderByDescending(x => x.W / x.N);
            return orderedKids.First().State;


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

            if (currentNode.State.IsTerminal) return currentNode;

            currentNode.GenerateChildren();

            return currentNode.Children.Count > 0 ? currentNode.Children[0] : currentNode;


            //if (!currentNode.State.IsTerminal)
            //{
            //    currentNode.GenerateChildren();
            //    if (currentNode.Children.Count != 0) return currentNode.Children[0];
            //}
            //return currentNode;
        }

        private static Node Simulation(Node currentNode, Random random)
        {
            int depth = 0;
            Debug.Write("Loop Start");
            while (!currentNode.State.IsTerminal)
            {
                depth++;
                //Debug.Write(", " + depth);
                currentNode.GenerateChildren();
                if (currentNode.Children.Count == 0)
                {
                    ;
                }
                currentNode = currentNode.Children[random.Next(0, currentNode.Children.Count)];
            }
            //Debug.WriteLine(depth);
            return currentNode;
            //return currentNode.State.Value;
        }

        private static void Backprop(int value, Node simulatedNode)
        {
            Node currentNode = simulatedNode;
            while (currentNode != null)
            {
                currentNode.N++;
                currentNode.W += value;
                currentNode = currentNode.Parent;
                value = -value;
            }
        }
    }
}
