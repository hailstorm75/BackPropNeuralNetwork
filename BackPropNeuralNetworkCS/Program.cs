using Data;
using System;
using System.Collections.Generic;

namespace NeuralNetworkFacadeCS
{
  internal static class Program
  {
    // TODO Automate Training data generation
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

    // TODO Automate Expected data generation
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
        var layers = GetLayers();                             // Getting hidden layer definition
        var iterations = GetInputInt("N times to test: ");    // Getting test iterations count
        var stable = true;                                    // Test success
        var totalError = 0.0;                                 // An average error of all tests combined

        // Testing network
        for (var i = 0; i < iterations; i++)
        {
          // Initializing new network
          var net = new FacadeX64(layers, TrainingData, ExpectedData);
          // Training network
          net.TrainNetwork();
          // Evaluating gathered information
          net.OutputToConsoleTest(ref totalError, ref stable, true);
        }

        // Test summary
        Console.WriteLine($"Stability: {(stable ? "Stable" : "Unstable")}\nError: {totalError / iterations}");

        Console.ReadKey();
        Console.Clear();
      }
    }

    /// <summary>
    /// Request user to provide layer definition
    /// </summary>
    /// <returns>An array of N ints which define each layer size, where N is a layer</returns>
    private static int[] GetLayers()
    {
      var layers = new List<int> { TrainingData.GetLength(1) };
      var hiddenLayers = GetInputInt("Hidden layers count: ");

      for (var layer = 0; layer < hiddenLayers; layer++)
        layers.Add(GetInputInt($"Hidden layer {layer + 1} neuron count: "));

      layers.Add(ExpectedData.GetLength(1));

      return layers.ToArray();
    }

    /// <summary>
    /// Requests integer input from user
    /// </summary>
    /// <param name="message">Message displayed to user</param>
    /// <returns>Requested value</returns>
    private static int GetInputInt(string message)
    {
      string input;     // Input placeholder
      int output;
      do
      {
        Console.Write(message);
        input = Console.ReadLine();
      } while (!int.TryParse(input, out output));

      return output;    // Returning converted value
    }
  }
}
