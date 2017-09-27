using System;
using Wrapper;

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

      unsafe
      {    
        // TODO Move to separate method
        // Converting 2D array to 1D    
        var trainingDataConverted = new double[trainingData.GetLength(0) * trainingData.GetLength(1)];
        Buffer.BlockCopy(trainingData, 0, trainingDataConverted, 0, trainingDataConverted.Length * sizeof(double));
        
        var expectedDataConverted = new double[expectedData.GetLength(0) * expectedData.GetLength(1)];
        Buffer.BlockCopy(expectedData, 0, expectedDataConverted, 0, expectedDataConverted.Length * sizeof(double));

        NeuralNetworkWrapper net;

        fixed (int* pLayers = &layers[0])
        {
          // Initiating the network
          net = new NeuralNetworkWrapper(pLayers, layers.Length);

          // Training the network
          fixed (double* pTrainingData = &trainingDataConverted[0])
          {
            fixed (double* pExpectedData = &expectedDataConverted[0])
            {
              net.TrainNetwork(pTrainingData, pExpectedData, trainingData.GetLength(0), 5000, false);
            }            
          }          
        }

        // TODO Create a "test drive" method and a regular "predict" method
        // Outputting expected results vs network predictions
        for (var i = 0; i < trainingData.GetLength(0); i++)
        {
          fixed (double* pInputs = &trainingData[i, 0])
          {
            double* retVal;
            net.FeedForward(pInputs, &retVal);

            Console.WriteLine($"Expected: {expectedData[i, 0]}\tReceived: {Math.Round(retVal[0], 3)}");
          }
        }
      }

      Console.ReadKey();
    }
  }
}
