using NeuralNetworkFacadeCS;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NetworkBenchmarking
{
  public class Benchmarker
  {
    #region Fields

    private static BenchmarkSettings _settings;
    private static Tuple<double[,], double[,]> _data;

    #endregion

    #region Constructor

    //--------------------------------------------------
    public Benchmarker(BenchmarkSettings settings)
    //--------------------------------------------------
    {
      // Saving settings
      _settings = settings;
      // Setting up data generator
      var gen = new DataGenerator(_settings.Inputs, _settings.Outputs);
      // Generating data
      var trainingData = gen.GenerateTrainingData();
      var expectedData = gen.GenerateExpectedData(ref trainingData, _settings.Operation);
      _data = new Tuple<double[,], double[,]>(trainingData, expectedData);
    }

    #endregion

    #region Methods

    /// <summary>
    /// Benchmarks the NN
    /// </summary>
    /// <returns></returns>
    //--------------------------------------------------
    public async Task RunAsync(IProgress<Tuple<double, bool>> prg, IProgress<BenchmarkData> stat, CancellationToken ct)
    //--------------------------------------------------
    {
      OnBenchmarkStarted(EventArgs.Empty);

      try
      {
        for (var i = _settings.Minimum; i <= _settings.Maximum; ++i) await TestNetwork(i, prg, stat, ct);
      }
      catch (OperationCanceledException)
      {
        OnBenchmarkComplete(new BenchmarkCompleteArgs(1));
        return;
      }

      OnBenchmarkComplete(new BenchmarkCompleteArgs(0));
    }

    //--------------------------------------------------
    private static async Task TestNetwork(int hidden, IProgress<Tuple<double, bool>> prg, IProgress<BenchmarkData> stat, CancellationToken ct)
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
        var layers = new[] { _data.Item1.GetLength(1), hidden, _data.Item2.GetLength(1) };
        // Testing network
        for (var j = 0; j < _settings.Iterations; ++j)
        {
          // Initializing new network
          var net = new FacadeX64(layers, _data.Item1, _data.Item2);
          // Check if canceled
          if (ct.IsCancellationRequested)
          {
            net.Clear();
            ct.ThrowIfCancellationRequested();
          }
          // Training network
          net.TrainNetwork();
          // Check if canceled
          if (ct.IsCancellationRequested)
          {
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
          prg.Report(new Tuple<double, bool>(100 / (_settings.ProgressTick * _settings.Iterations), true));
        }
      }, ct);

      // Test summary
      stat.Report(new BenchmarkData
      {
        Neurons = new[] { hidden },
        Error = totalError / _settings.Iterations,
        Stable = stable
      });
      // Update progress
      prg.Report(new Tuple<double, bool>(100 / _settings.ProgressTick * hidden, false));
    }

    #endregion

    #region Events

    public event EventHandler<BenchmarkCompleteArgs> BenchmarkComplete;
    private void OnBenchmarkComplete(BenchmarkCompleteArgs e) => BenchmarkComplete?.Invoke(this, e);

    public event EventHandler BenchmarkStarted;
    private void OnBenchmarkStarted(EventArgs e) => BenchmarkStarted?.Invoke(this, e); 

    #endregion
  }

  public class BenchmarkCompleteArgs : EventArgs
  {
    #region Property

    public int ExitCode { get; private set; }

    #endregion

    #region Constructor

    //--------------------------------------------------
    public BenchmarkCompleteArgs() => ExitCode = 0;
    //--------------------------------------------------

    //--------------------------------------------------
    public BenchmarkCompleteArgs(int exitCode) => ExitCode = exitCode;
    //--------------------------------------------------

    #endregion
  }
}
