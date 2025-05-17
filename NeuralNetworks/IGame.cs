using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks
{
    public interface IGameState<T, TCollection> where T : IGameState<T, TCollection> where TCollection : IList<T>
    {
        int Value { get; }
        bool IsTerminal { get; }
        TCollection GetChildren(bool expanded);
    }
}
