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
        public T Opossum;
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
            }
        }

        public void BuildTree(Node node, bool isMax)
        {
            var childrenStates = node.State.GetChildren();
            node.Children = new Node[childrenStates.Length];

            for (int i = 0; i < childrenStates.Length; i++)
            {
                node.Children[i] = new Node(childrenStates[i]);
                BuildTree(node.Children[i], !isMax);
            }

            if (node.Children.Length > 0 && !node.State.IsTerminal)
            {
                node.Children = node.Children.OrderBy(n => n.Value).ToArray();
                node.Value = isMax ? node.Children[^1].Value : node.Children[0].Value;
            }
        }

        public Node FindBestMove(Node node)
        {
            if (isMax)
            {
                return node.Children[^1];
            }
            else
            {
                return node.Children[0];
            }
        }
    }
}
