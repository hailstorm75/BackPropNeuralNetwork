using System;
using System.Threading;
using System.Threading.Tasks;
using NeuralNetworkFacadeCS;

namespace NetworkBenchmarking
{
  public class Benchmarker
  {
    #region Fields

    private static BenchmarkSettings _settings;
    private static Tuple<double[,], double[,]> _data;
    private static IProgress<Tuple<double, bool>> _prg;
    private static IProgress<BenchmarkData> _stat;
    private static CancellationToken _ct;

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
    /// Initiates neural network benchmarking
    /// </summary>
    /// <param name="prg">Progressbar percantage updater</param>
    /// <param name="stat">Benchmark data updater</param>
    /// <param name="ct">Cancellation token</param>
    //--------------------------------------------------
    public async Task RunAsync(IProgress<Tuple<double, bool>> prg, IProgress<BenchmarkData> stat, CancellationToken ct)
    //--------------------------------------------------
    {
      OnBenchmarkStarted(EventArgs.Empty);

      _prg = prg;
      _stat = stat;
      _ct = ct;

      try
      {
        for (var hiddenSet = _settings.Minimum; hiddenSet <= _settings.Maximum; ++hiddenSet)
          await TestNetwork(hiddenSet);
      }
      catch (OperationCanceledException)
      {
        OnBenchmarkComplete(new BenchmarkCompleteArgs(1));
        return;
      }

      OnBenchmarkComplete(new BenchmarkCompleteArgs(0));
    }

    /// <summary>
    /// Tests a new instance of a neural network based on given settings
    /// </summary>
    /// <param name="hidden">Set of hidden layers</param>
    //--------------------------------------------------
    private static async Task TestNetwork(int hidden)
    //--------------------------------------------------
    {
      // Test success
      var stable = true;
      // An average error of all tests combined
      var totalError = 0.0;
      // Check if canceled
      _ct.ThrowIfCancellationRequested();

      await Task.Run(() =>
      {
        // Getting hidden layer definition
        var layers = new[] { _settings.Inputs, hidden, _settings.Outputs };

        // Testing network
        for (var j = 0; j < _settings.Iterations; ++j)
        {
          // Initializing new network
          var net = new FacadeX64(layers, _data.Item1, _data.Item2);

          // Check if canceled
          if (_ct.IsCancellationRequested)
          {
            net.Clear();
            _ct.ThrowIfCancellationRequested();
          }

          // Training network
          net.TrainNetwork();

          // Check if canceled
          if (_ct.IsCancellationRequested)
          {
            net.Clear();
            _ct.ThrowIfCancellationRequested();
          }

          // Evaluating gathered information
          net.OutputTestResult(ref totalError, ref stable, _settings.Tolerance);
          // Clearing memory
          net.Clear();

          // Check if stable
          if (!stable) break;

          // Update progress
          _prg.Report(new Tuple<double, bool>(100 / (_settings.ProgressTick * _settings.Iterations), true));
        }
      }, _ct);

      // Test summary
      _stat.Report(new BenchmarkData
      {
        Neurons = new[] { hidden },
        Error = totalError / _settings.Iterations,
        Stable = stable
      });

      // Update progress
      _prg.Report(new Tuple<double, bool>(100 / _settings.ProgressTick * hidden, false));
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

    /// <summary>
    /// Determins if the benchmark has finished correctly
    /// </summary>
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
