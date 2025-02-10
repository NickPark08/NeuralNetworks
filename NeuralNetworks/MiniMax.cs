using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static NeuralNetworks.Zoo;

namespace NeuralNetworks
{
    static class Zoo
    {


    }
    public class MiniMax<T> where T : IGameState<T>
    {

        public Node Monkey;
        public Node Gorilla;

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

        //recursively build tree, and on way back up order children by their values, backpropogate
        public void BuildTree(Node node, bool isMax)
        {
            var childrenStates = node.State.GetChildren();
            node.Children = new Node[childrenStates.Length];

            for (int i = 0; i < childrenStates.Length; i++)
            {
                node.Children[i] = new Node(childrenStates[i]);
                BuildTree(node.Children[i], !isMax);
            }

            if (node.Children.Length > 0)
            {
                node.Children = node.Children.OrderBy(n => n.Value).ToArray();
                node.Value = isMax ? node.Children[^1].Value : node.Children[0].Value;
            }
        }

        public Node FindBestMove(Node node)
        {
            //Minimax(node);

            //node.Children = node.Children.OrderBy(n => n.Value).ToArray();

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
