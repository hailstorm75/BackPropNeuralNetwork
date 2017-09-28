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

      var net = new FacadeX64(layers, trainingData, expectedData);
      net.TrainNetwork();
      net.OutputToConsoleTest();

      Console.ReadKey();
    }
  }
}
