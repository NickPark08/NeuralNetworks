using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks
{
    public class MonteCarloTree<T> where T : IGameState<T>
    {
        public class Node
        {
            public T State { get; set; }
            public Node[] Children;
            public int Value;
            public bool isExpanded;

            public Node(T state)
            {
                State = state;
                Value = state.Value;
                Children = new Node[state.GetChildren().Length];
            }

            public double UCT()
            {
                return 0;
            }

            //public override string ToString()
            //{
            //    return Value.ToString() + "\n" + State.ToString();
            //}
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

                }
            }

            return null;
        }
    }
}
