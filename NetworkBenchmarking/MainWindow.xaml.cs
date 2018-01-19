using System.Threading.Tasks;
using NeuralNetworkFacadeCS;
using System.Text.RegularExpressions;
using System;
using System.Linq;
using System.Collections.Generic;

namespace NetworkBenchmarking
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow
  {
    //--------------------------------------------------
    public MainWindow()
    //--------------------------------------------------
    {
      InitializeComponent();
    }

    #region Methods

    /// <summary>
    /// Benchmarks the NN
    /// </summary>
    /// <returns></returns>
    //--------------------------------------------------
    private async Task Benchmark()
    //--------------------------------------------------
    {
      // Setting up data generator
      var gen = new DataGenerator(4, 3);
      // Getting test iterations count
      var iterations = sld_iterations.Value;
      // Data
      double[,] trainingData, expectedData;

      trainingData = gen.GenerateTrainingData();
      expectedData = gen.GenerateExpectedData(ref trainingData, (DataGenerator.Operation)cmb_DataType.SelectedItem);


      for (int i = int.Parse(txt_min.Text); i <= int.Parse(txt_max.Text); ++i)
      {
        // Test success
        var stable = true;
        // An average error of all tests combined
        var totalError = 0.0;

        await Task.Run(() =>
        {
          // Getting hidden layer definition
          var layers = new[] { trainingData.GetLength(1), i, expectedData.GetLength(1) };

          // Testing network
          for (var j = 0; j < iterations; ++j)
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
          textBox.Text += $"Hidden neurons: {i}\nStability: {(stable ? "Stable" : "Unstable")}\nError: {totalError / iterations}\n\n";
        });
      }
    }

    //--------------------------------------------------
    private void NumberValidationTextBox(object sender, System.Windows.Input.TextCompositionEventArgs e)
    //--------------------------------------------------
    {
      Regex regex = new Regex("[^0-9]+");
      e.Handled = regex.IsMatch(e.Text);
    }

    #endregion

    #region Events

    //--------------------------------------------------
    private async void Button_Click(object sender, System.Windows.RoutedEventArgs e)
    //--------------------------------------------------
    {
      btn_Start.IsEnabled = false;

      if (int.Parse(txt_min.Text) <= int.Parse(txt_max.Text))
      {
        textBox.Text = "Starting benchmark.\n";
        await Benchmark();
        textBox.Text += "Benchmark finished."; 
      }
      else textBox.Text = "Invalid hidden layer min/max.\n";

      btn_Start.IsEnabled = true;
    }

    private void Window_Loaded(object sender, System.Windows.RoutedEventArgs e)
    {
      cmb_DataType.ItemsSource = Enum.GetValues(typeof(DataGenerator.Operation)).Cast<DataGenerator.Operation>();
    }

    #endregion
  }
}

