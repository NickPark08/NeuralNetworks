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
    public partial class MiniMax<T> where T : IGameState<T>
    {
        class MaxComparer : IComparer<Node>
        {
            private MaxComparer() { }
            public static MaxComparer Instance { get; } = new();

            public int Compare(Node x, Node y) => -x.Value.CompareTo(y.Value);
            
        }
        class MinComparer : IComparer<Node>
        {
            private MinComparer() { }

            public static MinComparer Instance { get; } = new();

            public int Compare(Node x, Node y) => x.Value.CompareTo(y.Value);
        }

        //IComparer<Node> comparer;
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

            public override string ToString()
            {
                return Value.ToString() + "\n" + State.ToString(); 
            }
        }

        public void BuildTree(Node node, bool isMax, int a = int.MinValue, int b = int.MaxValue)
        {
            IComparer<Node> comparer = isMax ? MaxComparer.Instance : MinComparer.Instance;
            var childrenStates = node.State.GetChildren();

            if (comparer == MaxComparer.Instance)
            {
                ;
            }

            node.Children = new Node[childrenStates.Length];

            for (int i = 0; i < childrenStates.Length; i++)
            {
                node.Children[i] = new Node(childrenStates[i]);
                BuildTree(node.Children[i], !isMax, a, b);
                //for (int j = i; j > 0; j--)
                //{
                //    if (comparer.Compare(node.Children[j], node.Children[j - 1]) < 0)
                //    {
                //        (node.Children[j], node.Children[j - 1]) = (node.Children[j - 1], node.Children[j]);
                //    }
                //}

                ////pruning
                //if (isMax)
                //{
                //    //a = node.Children[0].Value;
                //    a = Math.Max(a, node.Children[0].Value);
                //    if (b <= a) break;
                //}
                //else
                //{
                //    b = Math.Min(b, node.Children[0].Value);
                //    if (a <= b) break;
                //}
            }

            if (node.Children.Any(m => m.Value == 0))
            {
                ;
            }
            if (comparer == MinComparer.Instance && isMax)
            {
                ;
            }
            if(node == Monkey)
            {
                ;
            }

            if (node.Children.Length > 0 && !node.State.IsTerminal)
            {
                for (int j = node.Children.Length - 1; j > 0; j--)
                {
                    if (comparer.Compare(node.Children[j], node.Children[j - 1]) < 0)
                    {
                        (node.Children[j], node.Children[j - 1]) = (node.Children[j - 1], node.Children[j]);
                    }
                }
                //node.Children = node.Children.OrderBy(n => n.Value).ToArray();
                //node.Value = isMax ? node.Children[^1].Value : node.Children[0].Value;
                node.Value = node.Children[0].Value;
            }
        }

        public Node FindBestMove(Node node) => node.Children.First();
        //{
        //    if (isMax)
        //    {
        //        return node.Children[^1];
        //    }
        //    else
        //    {
        //        return node.Children[0];
        //    }
        //}
    }
}
