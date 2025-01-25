using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks
{
    public class MiniMax<T> where T : IGameState<T>
    {
        //build tree "nodes"

        bool isMax = true;
        public class Node
        {
            public IGameState<T> State { get; set; }
            public Node[] Children;
            public int Value;

            public Node(IGameState<T> state)
            {
                State = state;
                Value = state.Value;
                Children = new Node[state.GetChildren().Length];
                for(int i = 0; i < Children.Length; i++)
                {
                    Children[i] = new Node(state.GetChildren()[i]);
                }
            }
        }

        public void BuildTree(Node node)
        {
            Node startNode = new Node(node.State);
            foreach (var child in startNode.Children)
            {
                BuildTree(child);
            }
        }
        public Node FindBestMove(Node node)
        {
            //BuildTree(node);
            Minimax(node);

            node.Children.Order();
            if(isMax)
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
            Queue<int> queue = new Queue<int>();
            Minimax(node, queue);
        }
        private int Minimax(Node node, Queue<int> queue)
        {
            if (node != null)
            {
                if (node.Children.Length == 0)
                {
                    return node.Value;
                }
                else
                {
                    foreach (var child in node.Children)
                    {
                        isMax = !isMax;
                        node.Value = Minimax(child, queue);
                    };
                    if(isMax)
                    {
                        return node.Children.Min(x => x.Value);
                    }
                    else
                    {
                        return node.Children.Max(x => x.Value);
                    }
                }
                // set state value to min/max of children
            }
            return 0;
        }
    }
}
