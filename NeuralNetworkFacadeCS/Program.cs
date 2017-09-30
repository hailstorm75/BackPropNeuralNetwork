using System;

namespace NeuralNetworkFacadeCS
{
  internal static class Program
  {
    public static void Main(string[] args)
    {
      #region Data

      int[] layers = { 3, 8, 1 };

      var trainingData = new double[,]
      {
        { 0, 0, 0 },
        { 0, 0, 1 },
        { 0, 1, 0 },
        { 1, 0, 0 },
        { 0, 1, 1 },
        { 1, 0, 1 },
        { 1, 1, 0 },
        { 1, 1, 1 }
      };

      var expectedData = new double[,] { { 0 }, { 1 }, { 1 }, { 0 }, { 1 }, { 0 }, { 0 }, { 1 } }; 
      
      #endregion

      // Approximately 45ms to create network
      // TODO Optimize
      var net = new FacadeX64(layers, trainingData, expectedData);

      // Approximately 1s to train network
      net.TrainNetwork();

      // Negligible estimate 
      net.OutputToConsoleTest();

      Console.ReadKey();
    }
  }
}
