using System.Threading.Tasks;
using NeuralNetworkFacadeCS;
using System.Text.RegularExpressions;

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
    }

    #region Methods

    /// <summary>
    /// Benchmarks the NN
    /// </summary>
    /// <returns></returns>
    private async Task Benchmark()
    {
      // Setting up data generator
      var gen = new DataGenerator(4, 3);
      // Getting test iterations count                     
      var iterations = sld_iterations.Value;
      // Test success    
      var stable = true;
      // An average error of all tests combined                    
      var totalError = 0.0;

      await Task.Run(() =>
      {
        var trainingData = gen.GenerateTrainingData();
        var expectedData = gen.GenerateExpectedData(ref trainingData, DataGenerator.Operation.Conjunction);

        // Getting hidden layer definition
        var layers = new[] { trainingData.GetLength(1), 8, 8, expectedData.GetLength(1) };

        // Testing network
        for (var i = 0; i < iterations; i++)
        {
          // Initializing new network
          var net = new FacadeX64(layers, trainingData, expectedData);
          // Training network
          net.TrainNetwork();
          // Evaluating gathered information
          net.OutputTestResult(ref totalError, ref stable);
          // Clearing memory
          net.Clear();
        }
      });

      // Test summary
      await textBox.Dispatcher.InvokeAsync(() =>
      {
        textBox.Text += $"Stability: {(stable ? "Stable" : "Unstable")}\nError: {totalError / iterations}\n";
      });
    }

    private void NumberValidationTextBox(object sender, System.Windows.Input.TextCompositionEventArgs e)
    {
      Regex regex = new Regex("[^0-9]+");
      e.Handled = regex.IsMatch(e.Text);
    }

    #endregion

    #region Events

    private async void Button_Click(object sender, System.Windows.RoutedEventArgs e)
    {
      textBox.Text = "Starting benchmark.\n";
      await Benchmark();
      textBox.Text += "Benchmark finished.";
    }

    #endregion
  }
}
