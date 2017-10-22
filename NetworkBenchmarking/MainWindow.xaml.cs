using System.Collections.Generic;
using NeuralNetworkFacadeCS;

namespace NetworkBenchmarking
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow
  {
    public MainWindow()
    {
      InitializeComponent();

      var gen = new DataGenerator(4, 3, DataGenerator.Operation.Conjunction);
      var trainingData = gen.TrainingData;
      var expectedData = gen.ExpectedData;

      while (true)
      {
        // Getting hidden layer definition
        var layers = new[] { trainingData.GetLength(1), 8, 8, expectedData.GetLength(1) };//GetLayers(trainingData.GetLength(1), expectedData.GetLength(1));
        // Getting test iterations count                     
        var iterations = 5; //GetInputInt("N times to test: ");
        // Test success    
        var stable = true;
        // An average error of all tests combined                    
        var totalError = 0.0;

        // Testing network
        for (var i = 0; i < iterations; i++)
        {
          // Initializing new network
          var net = new FacadeX64(layers, trainingData, expectedData);
          // Training network
          net.TrainNetwork();
          // Evaluating gathered information
          net.OutputToConsoleTest(ref totalError, ref stable, true);
        }

        // Test summary
        textBox.Text += $"Stability: {(stable ? "Stable" : "Unstable")}\nError: {totalError / iterations}\n";

        //Console.ReadKey();
        //Console.Clear();
      }
    }

    /// <summary>
    /// Request user to provide layer definition
    /// </summary>
    /// <returns>An array of N integers which define each layer size, where N is a layer</returns>
    private static int[] GetLayers(int inputs, int outputs)
    {
      var layers = new List<int> { inputs };
      var hiddenLayers = GetInputInt("Hidden layers count: ");

      for (var layer = 0; layer < hiddenLayers; layer++)
        layers.Add(GetInputInt($"Hidden layer {layer + 1} neuron count: "));

      layers.Add(outputs);

      return layers.ToArray();
    }

    /// <summary>
    /// Requests integer input from user
    /// </summary>
    /// <param name="message">Message displayed to user</param>
    /// <returns>Requested value</returns>
    private static int GetInputInt(string message)
    {
      string input="";     // Input placeholder
      int output;
      do
      {
        //Console.Write(message);
        //input = Console.ReadLine();
      } while (!int.TryParse(input, out output));

      return output;    // Returning converted value
    }
  }
}
