using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using NeuralNetworkFacadeCS;

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

    // TODO Move to View model

    #region Fields

    private CancellationTokenSource _cs;

    private struct BenchmarkProgress
    {
      public int[] Neurons;
      public bool Stable;
      public double TotalError;
    }

    private List<BenchmarkProgress> _benchmarkData;

    private bool _benchmarkRunning;

    #endregion

    #region Methods

    /// <summary>
    /// Benchmarks the NN
    /// </summary>
    /// <returns></returns>
    //--------------------------------------------------
    private async Task Benchmark(IProgress<Tuple<double, bool>> prg, IProgress<BenchmarkProgress> stat, CancellationToken ct)
    //--------------------------------------------------
    {
      _benchmarkRunning = true;

      // Setting up data generator
      var gen = new DataGenerator(int.Parse(txt_inputs.Text), int.Parse(txt_outputs.Text));
      // 1 Percent of progress
      double progress = int.Parse(txt_max.Text) - int.Parse(txt_min.Text) + 1;
      // Data
      var trainingData = gen.GenerateTrainingData();
      var expectedData = gen.GenerateExpectedData(ref trainingData, (DataGenerator.Operation)cmb_DataType.SelectedItem);

      for (var i = int.Parse(txt_min.Text); i <= int.Parse(txt_max.Text); ++i)
        await TestNetwork(prg, stat, ct, new Tuple<double[,], double[,]>(trainingData, expectedData), i, sld_iterations.Value, progress);

      _benchmarkRunning = false;
    }

    //--------------------------------------------------
    private async Task TestNetwork(IProgress<Tuple<double, bool>> prg, IProgress<BenchmarkProgress> stat, CancellationToken ct, Tuple<double[,], double[,]> data, int hidden, double iterations, double progress)
    //--------------------------------------------------
    {
      // Test success
      var stable = true;
      // An average error of all tests combined
      var totalError = 0.0;
      // Check if canceled
      ct.ThrowIfCancellationRequested();

      await Task.Run(() =>
      {
        // Getting hidden layer definition
        var layers = new[] { data.Item1.GetLength(1), hidden, data.Item2.GetLength(1) };
        // Testing network
        for (var j = 0; j < iterations; ++j)
        {
          // Initializing new network
          var net = new FacadeX64(layers, data.Item1, data.Item2);
          // Check if canceled
          if (ct.IsCancellationRequested)
          {
            _benchmarkRunning = false;
            net.Clear();
            ct.ThrowIfCancellationRequested();
          }
          // Training network
          net.TrainNetwork();
          // Check if canceled
          if (ct.IsCancellationRequested)
          {
            _benchmarkRunning = false;
            net.Clear();
            ct.ThrowIfCancellationRequested();
          }
          // Evaluating gathered information
          net.OutputTestResult(ref totalError, ref stable);
          // Clearing memory
          net.Clear();
          // Check if stable
          if (!stable) break;
          // Update progress
          prg.Report(new Tuple<double, bool>(100 / (progress * iterations), true));
        }
      }, ct);

      // Test summary
      stat.Report(new BenchmarkProgress
      {
        Neurons = new[] { hidden },
        TotalError = totalError / iterations,
        Stable = stable
      });
      // Update progress
      prg.Report(new Tuple<double, bool>(100 / progress * hidden, false));
    }

    //--------------------------------------------------
    private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
    //--------------------------------------------------
    {
      var regex = new Regex("[^0-9]+");
      e.Handled = regex.IsMatch(e.Text);
    }

    #endregion

    #region Events

    //--------------------------------------------------
    private async void Button_Click(object sender, RoutedEventArgs e)
    //--------------------------------------------------
    {
      btn_Start.IsEnabled = false;
      prg_Progress.Value = 0;
      TaskbarItemInfo.ProgressValue = 0;

      if (int.Parse(txt_min.Text) <= int.Parse(txt_max.Text))
      {
        _cs = new CancellationTokenSource();

        try
        {
          textBox.Text = "Starting benchmark.\n";

          var prg = new Progress<Tuple<double, bool>>();
          var status = new Progress<BenchmarkProgress>();
          prg.ProgressChanged += PrgOnProgressChanged;
          status.ProgressChanged += StatusOnProgressChanged;

          _benchmarkData = new List<BenchmarkProgress>();
          await Benchmark(prg, status, _cs.Token);

          textBox.Text += "Benchmark finished.\n";
          if (_benchmarkData.Count != 0)
          {
            var byAccuracy = _benchmarkData.Select(x => x).OrderByDescending(x => x.TotalError).First();
            textBox.Text += $"Most accurate:\n\tNeurons - {string.Join(",", byAccuracy.Neurons)}\n\tError - {byAccuracy.TotalError}\n";
            var byNeurons = _benchmarkData.Select(x => x).OrderBy(x => x.Neurons.Sum()).First();
            textBox.Text += $"Least neurons:\n\tNeurons - {string.Join(",", byNeurons.Neurons)}\n\tError - {byNeurons.TotalError}";
          }
          else textBox.Text += "No stable networks generated.";

          _benchmarkData.Clear();
        }
        catch (OperationCanceledException)
        {
          Application.Current.Shutdown();
        }
      }
      else textBox.Text = "Invalid hidden layer min/max.\n";

      btn_Start.IsEnabled = true;
      prg_Progress.Value = 0;
      TaskbarItemInfo.ProgressValue = 0;
    }

    //--------------------------------------------------
    private void PrgOnProgressChanged(object sender, Tuple<double, bool> b)
    //--------------------------------------------------
    {
      if (b.Item2)
      {
        prg_Progress.Value += b.Item1;
        TaskbarItemInfo.ProgressValue += b.Item1 / 100;
      }
      else
      {
        prg_Progress.Value = b.Item1;
        TaskbarItemInfo.ProgressValue = b.Item1 / 100;
      }
    }

    //--------------------------------------------------
    private void StatusOnProgressChanged(object sender, BenchmarkProgress b)
    //--------------------------------------------------
    {
      if (b.Stable) _benchmarkData.Add(b);
      textBox.Text += $"Hidden neurons: {string.Join(",", b.Neurons)}\nStability: {(b.Stable ? "Stable" : "Unstable")}\nError: {b.TotalError}\n\n";
    }

    //--------------------------------------------------
    private void Window_Closing(object sender, CancelEventArgs e)
    //--------------------------------------------------
    {
      if (_benchmarkRunning)
      {
        _cs.Cancel();
        e.Cancel = true;
      }
    }

    //--------------------------------------------------
    private void Window_Loaded(object sender, RoutedEventArgs e)
    //--------------------------------------------------
    {
      cmb_DataType.ItemsSource = Enum.GetValues(typeof(DataGenerator.Operation)).Cast<DataGenerator.Operation>();
    }

    #endregion
  }
}

