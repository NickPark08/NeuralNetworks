using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks
{
    public class MiniMax<T> where T : IGameState<T>
    {
        public bool isMax = true;
        public class Node
        {
            public T State { get; set; }
            public Node[] Children;
            public int Value;

            public Node(T state)
            {
                State = state;
                Value = state.Value;
                Children = new Node[state.GetChildren().Length];
                //for(int i = 0; i < Children.Length; i++)
                //{
                //    Children[i] = new Node(state.GetChildren()[i]);
                //}
            }
        }

        public void BuildTree(Node node)
        {
            Node startNode = new Node(node.State);
            for (int i = 0; i < node.Children.Length; i++)
            {
                node.Children[i] = new Node(node.State.GetChildren()[i]);
                BuildTree(node.Children[i]);
            }
        }
        public Node FindBestMove(Node node)
        {
            Minimax(node);

            node.Children = node.Children.OrderBy(n => n.Value).ToArray();

            if (isMax)
            {
                return node.Children[node.Children.Length - 1];
            }
            else
            {
                return node.Children[0];
            }
        }
        public void Minimax(Node node)
        {
            node.Value = Minimax(node, isMax);
        }
        private int Minimax(Node node, bool max)
        {
            if (node.Children.Length == 0) return node.Value;

            if (max)
            {
                int bestValue = int.MinValue;
                foreach (var child in node.Children)
                {
                    int val = Minimax(child, !max);
                    bestValue = Math.Max(bestValue, val);
                }
                node.Value = bestValue;
                return bestValue;
            }
            else
            {
                int bestValue = int.MaxValue;
                foreach (var child in node.Children)
                {
                    int val = Minimax(child, !max);
                    bestValue = Math.Min(bestValue, val);
                }
                node.Value = bestValue;
                return bestValue;
            }
        }

    }
}
