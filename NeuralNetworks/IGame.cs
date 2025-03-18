using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks
{
    public interface IGameState<T> where T : IGameState<T>
    {
        int Value { get; }
        bool IsTerminal { get; }
        T[] GetChildren();
    }
}
