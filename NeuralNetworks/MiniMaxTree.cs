using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks
{
    class MiniMaxTree<T> where T : IGameState<T>
    {
        //build tree "nodes"
        class Node
        {
            T State { get; set; }
            List<Node> Children = new List<Node>();

            public Node(T state)
            {
                State = state;
            }


        }

        public void BuildTree(IGameState<T> state)
        {
        }
        public void FindBestMove(IGameState<T> state)
        {

        }
    }
}
