using System;
using System.Collections.Generic;
using System.Dynamic;

namespace NeuralNetworkFacadeCS
{
  internal static class Program
  {
    private static readonly double[,] TrainingData = {
      { 0, 0, 0, 0 },
      { 0, 0, 1, 0 },
      { 0, 1, 0, 0 },
      { 1, 0, 0, 0 },
      { 0, 1, 1, 0 },
      { 1, 0, 1, 0 },
      { 1, 1, 0, 0 },
      { 1, 1, 1, 0 },
      { 0, 0, 0, 1 },
      { 0, 0, 1, 1 },
      { 0, 1, 0, 1 },
      { 1, 0, 0, 1 },
      { 0, 1, 1, 1 },
      { 1, 0, 1, 1 },
      { 1, 1, 0, 1 },
      { 1, 1, 1, 1 },
    };

    private static readonly double[,] ExpectedData = {
      { 0, 0 },
      { 0, 0 },
      { 0, 0 },
      { 0, 0 },
      { 0, 0 },
      { 0, 0 },
      { 1, 0 },
      { 1, 0 },
      { 0, 0 },
      { 0, 1 },
      { 0, 0 },
      { 0, 0 },
      { 0, 1 },
      { 0, 1 },
      { 1, 0 },
      { 1, 1 },
    };

    public static void Main(string[] args)
    {
      while (true)
      {
        var layers = GetLayers();
        var iterations = GetInputInt("N times to test: ");
        var stable = true;
        var totalError = 0.0;

        for (var i = 0; i < iterations; i++)
        {
          var net = new FacadeX64(layers, TrainingData, ExpectedData);

          net.TrainNetwork();

          net.OutputToConsoleTest(ref totalError, ref stable, true);
        }

        Console.WriteLine($"Stability: {(stable ? "Stable" : "Unstable")}\nError: {totalError / iterations}");

        Console.ReadKey();
        Console.Clear();
      }
    }

    private static int[] GetLayers()
    {
      var layers = new List<int> {TrainingData.GetLength(1)};
      var hiddenLayers = GetInputInt("Hidden layers count: ");

      for (var layer = 0; layer < hiddenLayers; layer++)
        layers.Add(GetInputInt($"Hidden layer {layer + 1} neuron count: "));

      layers.Add(ExpectedData.GetLength(1));

      return layers.ToArray();
    }

    private static int GetInputInt(string message)
    {
      string input;
      int output;
      do
	    {
	      Console.Write(message);
	      input = Console.ReadLine();
	    } while (!int.TryParse(input, out output));

      return output;
    }
  }
}
