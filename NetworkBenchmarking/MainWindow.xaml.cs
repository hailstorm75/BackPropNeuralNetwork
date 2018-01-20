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

    // TODO Move to Viewmodel

    #region Fields

    private struct TestData
    {
      public int[] neurons;
      public double totalError;
    }

    private bool closing = false; 

    #endregion

    #region Methods

    /// <summary>
    /// Benchmarks the NN
    /// </summary>
    /// <returns></returns>
    //--------------------------------------------------
    private async Task<List<TestData>> Benchmark()
    //--------------------------------------------------
    {
      // Setting up data generator
      var gen = new DataGenerator(int.Parse(txt_inputs.Text), int.Parse(txt_outputs.Text));
      // Getting test iterations count
      var iterations = sld_iterations.Value;
      // 1 Percent of progress
      double progress = int.Parse(txt_max.Text) - int.Parse(txt_min.Text) + 1;
      // Data
      var trainingData = gen.GenerateTrainingData();
      var expectedData = gen.GenerateExpectedData(ref trainingData, (DataGenerator.Operation)cmb_DataType.SelectedItem);
      // Benchmark result
      List<TestData> result = new List<TestData>();

      for (int i = int.Parse(txt_min.Text); i <= int.Parse(txt_max.Text); ++i)
      {
        // Test success
        var stable = true;
        // An average error of all tests combined
        var totalError = 0.0;

        if (closing) return null;
        
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
            // Check if stable
            if (!stable) break;

            // Update progress
            prg_Progress.Dispatcher.Invoke(callback: () => prg_Progress.Value += 100 / (progress * iterations));
          }
        });

        // Saving stable result
        if (stable) result.Add(new TestData { neurons = new int[] { i }, totalError = totalError / iterations });

        await textBox.Dispatcher.InvokeAsync(() =>
        {
          // Test summary
          textBox.Text += $"Hidden neurons: {i}\nStability: {(stable ? "Stable" : "Unstable")}\nError: {totalError / iterations}\n\n";
          // Update progress
          prg_Progress.Value = (100 / progress) * i;
          // Update progress in taskbar
          TaskbarItemInfo.ProgressValue = prg_Progress.Value / 100;
        });
      }

      return result;
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
      prg_Progress.Value = 0;
      TaskbarItemInfo.ProgressValue = 0;

      if (int.Parse(txt_min.Text) <= int.Parse(txt_max.Text))
      {
        textBox.Text = "Starting benchmark.\n";
        var data = await Benchmark();
        
        textBox.Text += "Benchmark finished.\n";
        if (data.Count != 0)
        {
          var byAccuracy = data.Select(x => x).OrderByDescending(x => x.totalError).First();
          textBox.Text += $"Most accurate:\n\tNeurons - {string.Join(",", byAccuracy.neurons)}\n\tError - {byAccuracy.totalError.ToString()}\n";
          var byNeurons = data.Select(x => x).OrderBy(x => x.neurons.Sum()).First();
          textBox.Text += $"Least neurons:\n\tNeurons - {string.Join(",", byNeurons.neurons)}\n\tError - {byNeurons.totalError.ToString()}";
        }
        else textBox.Text += "No stable networks generated.";
      }
      else textBox.Text = "Invalid hidden layer min/max.\n";

      btn_Start.IsEnabled = true;
      prg_Progress.Value = 0;
      TaskbarItemInfo.ProgressValue = 0;
    }

    //--------------------------------------------------
    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    //--------------------------------------------------
    {
      closing = true;
    }

    //--------------------------------------------------
    private void Window_Loaded(object sender, System.Windows.RoutedEventArgs e)
    //--------------------------------------------------
    {
      cmb_DataType.ItemsSource = Enum.GetValues(typeof(DataGenerator.Operation)).Cast<DataGenerator.Operation>();
    }

    #endregion
  }
}

