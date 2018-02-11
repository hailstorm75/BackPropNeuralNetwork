using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Input;

namespace NetworkBenchmarking
{
  public class BenchmarkerViewModel : BaseViewModel, ICloseable
  {
    #region Properties

    public object OperationType { get; set; }
    public IEnumerable OperationTypes { get; set; } = Enum.GetValues(typeof(DataGenerator.Operation)).Cast<DataGenerator.Operation>();

    public bool UIEnabled { get; set; } = true;
    public bool ButtonStartEnabled { get { return UIEnabled; } }
    public bool ButtonCancelEnabled { get { return !UIEnabled; } }

    public double Progress { get; set; } = 0;
    public double ProgressTaskBar { get { return Progress / 100; } }
    public double InputIterations { get; set; } = 5;

    public string OutputLog { get; set; }
    public string InputMinimum { get; set; } = "1";
    public string InputMaximum { get; set; } = "8";
    public string InputInputs { get; set; } = "4";
    public string InputOutputs { get; set; } = "3";

    #endregion

    #region Commands

    public ICommand RunBenchmarkCommand { get; set; }
    public ICommand CancelOperationCommand { get; set; }
    public ICommand CloseCommand { get; set; }

    #endregion

    #region Fields

    private bool _benchmarkRunning = false;
    private CancellationTokenSource _ct;
    private List<BenchmarkData> _benchmarkData;
    private enum ExitCodes
    {
      Normal = 0,
      Cancelled = 1
    }

    #endregion

    #region Constructor

    //--------------------------------------------------
    public BenchmarkerViewModel()
    //--------------------------------------------------
    {
      RunBenchmarkCommand = new RelayCommand(RunBenchmark);
      CancelOperationCommand = new RelayCommand(CancelOperation);
    }

    #endregion

    #region Methods

    //--------------------------------------------------
    private async void RunBenchmark()
    //--------------------------------------------------
    {
      UIEnabled = false;
      Progress = 0;

      if (int.Parse(InputMinimum) <= int.Parse(InputMaximum))
      {
        _ct = new CancellationTokenSource();
        var benchMarker = new Benchmarker(new BenchmarkSettings()
        {
          Inputs = int.Parse(InputInputs),
          Outputs = int.Parse(InputOutputs),
          Minimum = int.Parse(InputMinimum),
          Maximum = int.Parse(InputMaximum),
          Iterations = (int)InputIterations,
          Operation = (DataGenerator.Operation)OperationType
        });

        var prg = new Progress<Tuple<double, bool>>();
        var status = new Progress<BenchmarkData>();

        prg.ProgressChanged += PrgOnProgressChanged;
        status.ProgressChanged += StatusOnProgressChanged;
        benchMarker.BenchmarkStarted += BenchMarker_BenchmarkStarted;
        benchMarker.BenchmarkComplete += BenchMarker_BenchmarkComplete;

        _benchmarkData = new List<BenchmarkData>();
        await benchMarker.RunAsync(prg, status, _ct.Token);

        prg.ProgressChanged -= PrgOnProgressChanged;
        status.ProgressChanged -= StatusOnProgressChanged;
        benchMarker.BenchmarkStarted -= BenchMarker_BenchmarkStarted;
        benchMarker.BenchmarkComplete -= BenchMarker_BenchmarkComplete;
      }
      else OutputLog = "Invalid hidden layer min/max.\n";

      UIEnabled = true;
      Progress = 0;
    }

    //--------------------------------------------------
    private void CancelOperation() => _ct?.Cancel();
    //--------------------------------------------------

    //--------------------------------------------------
    public void CloseWindow(System.ComponentModel.CancelEventArgs e)
    //--------------------------------------------------
    {
      if (_benchmarkRunning)
      {
        e.Cancel = true;
        CancelOperation();
      }
    }

    #endregion

    #region Events

    public event EventHandler RequestClose;

    //--------------------------------------------------
    private void BenchMarker_BenchmarkStarted(object sender, EventArgs e)
    //--------------------------------------------------
    {
      _benchmarkRunning = true;
      OutputLog = "Starting benchmark.\n";
    }

    //--------------------------------------------------
    private void BenchMarker_BenchmarkComplete(object sender, BenchmarkCompleteArgs e)
    //--------------------------------------------------
    {
      _benchmarkRunning = false;

      switch ((ExitCodes)e.ExitCode)
      {
        case ExitCodes.Normal:
          OutputLog += "Benchmark finished.\n";
          if (_benchmarkData.Count != 0)
          {
            var byAccuracy = _benchmarkData.Select(x => x).OrderByDescending(x => x.Error).First();
            OutputLog += $"Most accurate:\n\tNeurons - {string.Join(",", byAccuracy.Neurons)}\n\tError - {byAccuracy.Error}\n";
            var byNeurons = _benchmarkData.Select(x => x).OrderBy(x => x.Neurons.Sum()).First();
            OutputLog += $"Least neurons:\n\tNeurons - {string.Join(",", byNeurons.Neurons)}\n\tError - {byNeurons.Error}";
          }
          else OutputLog += "No stable networks generated.";

          _benchmarkData.Clear();
          break;
        case ExitCodes.Cancelled:
          OutputLog += "Benchmark cancelled.\n";
          RequestClose?.Invoke(this, EventArgs.Empty);
          _benchmarkData.Clear();
          break;
      }
    }

    //--------------------------------------------------
    private void PrgOnProgressChanged(object sender, Tuple<double, bool> b)
    //--------------------------------------------------
    {
      if (b.Item2) Progress += b.Item1;
      else Progress = b.Item1;
    }

    //--------------------------------------------------
    private void StatusOnProgressChanged(object sender, BenchmarkData b)
    //--------------------------------------------------
    {
      if (b.Stable) _benchmarkData.Add(b);
      OutputLog += $"Hidden neurons: {string.Join(",", b.Neurons)}\nStability: {(b.Stable ? "Stable" : "Unstable")}\nError: {b.Error}\n\n";
    }

    #endregion
  }
}
