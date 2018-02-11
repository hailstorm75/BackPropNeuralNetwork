using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Input;

namespace NetworkBenchmarking
{
  public class BenchmarkerViewModel : BaseViewModel
  {
    #region Properties

    /// <summary>
    /// Binding property to set selected operation type
    /// </summary>
    public object OperationType { get; set; }
    /// <summary>
    /// Bidning property to set available operation type items
    /// </summary>
    public IEnumerable OperationTypes { get; set; } = Enum.GetValues(typeof(DataGenerator.Operation)).Cast<DataGenerator.Operation>();

    /// <summary>
    /// Binding property to set UI elements enabled state
    /// </summary>
    public bool UIEnabled { get; set; } = true;
    /// <summary>
    /// Binding property to set start button enabled state
    /// </summary>
    public bool ButtonStartEnabled { get { return UIEnabled; } }
    /// <summary>
    /// Binding property to set cancel button enabled state
    /// </summary>
    public bool ButtonCancelEnabled { get { return !UIEnabled; } }
    /// <summary>
    /// Binding property to set popup state
    /// </summary>
    public bool ToggleAddChecked { get; set; }

    /// <summary>
    /// Binding property to display progress
    /// </summary>
    public double Progress { get; set; } = 0;
    /// <summary>
    /// Binding property to display progress in taskbar
    /// </summary>
    public double ProgressTaskBar { get { return Progress / 100; } }
    /// <summary>
    /// Binding property to set number of iterations
    /// </summary>
    public double InputIterations { get; set; } = 5;

    /// <summary>
    /// Binding property to display benchmark logging
    /// </summary>
    public string OutputLog { get; set; }
    /// <summary>
    /// Binding property to set minimum hidden neurons
    /// </summary>
    public string InputMinimum { get; set; } = "1";
    /// <summary>
    /// Binding property to set maximum hidden neurons
    /// </summary>
    public string InputMaximum { get; set; } = "8";
    /// <summary>
    /// Binding property to set number of inputs
    /// </summary>
    public string InputInputs { get; set; } = "4";
    /// <summary>
    /// Bidning property to set number of outputs
    /// </summary>
    public string InputOutputs { get; set; } = "3";

    #endregion

    #region Commands

    /// <summary>
    /// Runs the benchmark
    /// </summary>
    public ICommand RunBenchmarkCommand { get; set; }

    /// <summary>
    /// Aborts the benchmark
    /// </summary>
    public ICommand CancelOperationCommand { get; set; }

    #endregion

    #region Fields

    /// <summary>
    /// Tracks benchmarking state
    /// </summary>
    private bool _benchmarkRunning = false;
    /// <summary>
    /// Cancellation token for benchmarking process
    /// </summary>
    private CancellationTokenSource _ct;
    /// <summary>
    /// Generated data from benchmark
    /// </summary>
    private List<BenchmarkData> _benchmarkData;
    /// <summary>
    /// Benchmarking exit code which is generated upon completion
    /// </summary>
    private enum ExitCodes
    {
      Normal = 0,
      Cancelled = 1
    }

    #endregion

    #region Constructor

    /// <summary>
    /// Default constructor
    /// </summary>
    //--------------------------------------------------
    public BenchmarkerViewModel()
    //--------------------------------------------------
    {
      RunBenchmarkCommand = new RelayCommand(RunBenchmark);
      CancelOperationCommand = new RelayCommand(CancelOperation);
    }

    #endregion

    #region Methods

    /// <summary>
    /// Runs the benchmark based on given settings
    /// </summary>
    //--------------------------------------------------
    private async void RunBenchmark()
    //--------------------------------------------------
    {
      _ct = new CancellationTokenSource();
      UIEnabled = false;
      Progress = 0;

      if (int.Parse(InputMinimum) <= int.Parse(InputMaximum))
      {
        var benchMarker = new Benchmarker(new BenchmarkSettings()
        {
          Inputs = int.Parse(InputInputs),
          Outputs = int.Parse(InputOutputs),
          Minimum = int.Parse(InputMinimum),
          Maximum = int.Parse(InputMaximum),
          Iterations = (int)InputIterations,
          Tolerance = 95,
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

      _ct.Dispose();
    }

    /// <summary>
    /// Cancels running benchmark operation
    /// </summary>
    //--------------------------------------------------
    private void CancelOperation() => _ct?.Cancel();
    //--------------------------------------------------
    
    /// <summary>
    /// Prevents window from closing if benchmark is running
    /// </summary>
    /// <param name="e">Closing event</param>
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
          break;
        case ExitCodes.Cancelled:
          OutputLog += "Benchmark cancelled.\n";
          break;
      }

      _benchmarkData.Clear();
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
