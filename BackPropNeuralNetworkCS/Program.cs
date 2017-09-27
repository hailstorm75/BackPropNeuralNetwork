using System;
using Wrapper;

namespace NeuralNetworkFacadeCS
{
  internal static class Program
  {
    public static void Main(string[] args)
    {
      int[] layers = { 3, 8, 1 };
      double[] inputs = { 0, 0, 1 };

      unsafe
      {
        fixed (int* pLayers = &layers[0])
        {
          var net = new NeuralNetworkWrapper(pLayers, layers.Length);
          net.TrainNetwork(5000, false);

          fixed (double* pInputs = &inputs[0])
          {
            double* retVal;
            net.FeedForward(pInputs, &retVal);

            Console.WriteLine(retVal[0].ToString());
          }
        }
      }

      Console.ReadKey();
    }
  }
}
