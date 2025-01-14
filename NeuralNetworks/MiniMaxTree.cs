using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks
{
    class MiniMaxTree<T> where T : IGameState<T>
    {
        class Node
        {
            int Score;
            T State { get; set; }
            List<Node> Outcomes = new List<Node>();
        }


        //int Minimax(IGameState<T> state, bool isMax)
        //{
        //    if(state.isTerminal)
        //    {
        //    }

        //    return 0;
        //}
    }
}
