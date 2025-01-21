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
        public class Node
        {
            public T State { get; set; }
            public T[] Children => State.GetChildren();

            public Node(T state)
            {
                State = state;
            }
        }

        public void BuildTree(IGameState<T> state)
        {
            var children = state.GetChildren();
            foreach (var child in children)
            {
                BuildTree(child);
            }
        }
        public void FindBestMove(IGameState<T> state)
        {
            BuildTree(state);


        }
        public Queue<int> InOrder(Node node)
        {
            Queue<int> queue = new Queue<int>();
            InOrder(node.State, queue);
            return queue;
        }
        private void InOrder(IGameState<T> state, Queue<int> queue)
        {
            if (state != null)
            {
                foreach(var child in state.GetChildren())
                {
                    InOrder(child, queue);
                };
                queue.Enqueue(state.Value);

                //state.Value = 0;
                // set state value to min/max of children
            }

        }
    }
}
