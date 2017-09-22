using System;
using Wrapper;

namespace NeuralNetworkFacadeCS
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            int[] layers = { 3, 8, 1 };

            unsafe
            {
                fixed (int* pLayers = &layers[0])
                {                    
                    var net = new NeuralNetworkWrapper(pLayers, layers.Length);
                    net.TrainNetwork(5000, false);
                    Console.ReadKey();
                }
            }
        }
    }
}
