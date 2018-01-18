using System;
using Wrapper;

namespace NeuralNetworkFacadeCS
{
  public class FacadeX64 : IFacade
  {
    #region Fields

    /// <summary>
    /// Neural network constructed mode
    /// </summary>
    private enum Mode
    {
      WithoutTraining = 0,
      WithTraining = 1
    }

    /// <summary>
    /// The neural network mode
    /// </summary>
    private Mode _mode;

    /// <summary>
    /// The neural network
    /// </summary>
    private NeuralNetworkWrapper _net;

    /// <summary>
    /// Neural network layer definitions
    /// </summary>
    private int[] _layers;

    /// <summary>
    /// Array of data to train the neural network
    /// </summary>
    private double[] _trainingData;

    /// <summary>
    /// Array of data which the network is expected to produce
    /// </summary>
    private double[] _expectedData;

    private int _dataSize;

    #endregion

    #region Constructor

    /// <summary>
    /// Training mode constructor
    /// </summary>
    /// <param name="layers"></param>
    /// <param name="trainingData">Data to feed to the network</param>
    /// <param name="expectedData">Data the network is expected to output</param>
    public FacadeX64(int[] layers, double[,] trainingData, double[,] expectedData)
    {
      Convert2DArrayTo1D(trainingData, out _trainingData);
      Convert2DArrayTo1D(expectedData, out _expectedData);

      _mode = Mode.WithTraining;
      _layers = layers;
      _dataSize = trainingData.GetLength(0);

      Initialize();
    }

    /// <summary>
    /// Training mode constructor
    /// </summary>
    /// <param name="pathToCSV">Path to saved neural network settings</param>
    public FacadeX64(string pathToCSV, EventHandler trainingComplete)
    {
      TrainingComplete = trainingComplete;
      _mode = Mode.WithoutTraining;

      // TODO Load net config

      Initialize();
    }

    #endregion

    #region Methods

    /// <summary>
    /// 
    /// </summary>
    public void ExportToCSV()
    {
      _net.ExportNeuralNetwork(NeuralNetworkWrapper.ExportType.CSV);
    }

    public void TrainNetwork()
    {
      unsafe
      {
        // Training the network
        fixed (double* pTrainingData = &_trainingData[0])
        {
          fixed (double* pExpectedData = &_expectedData[0])
          {
            _net.TrainNetwork(pTrainingData, pExpectedData, _dataSize, 5000);
          }
        }
      }

      OnTrainingComplete(EventArgs.Empty);
    }

    public void Clear() => _net.Clear();

    private void Initialize()
    {
      unsafe
      {
        fixed (int* pLayers = &_layers[0])
        {
          // Initiating the network
          _net = new NeuralNetworkWrapper(pLayers, _layers.Length);
        }
      }
    }

    public void OutputToConsoleTest(ref double totalError, ref bool stability)
    {
      if (_mode == Mode.WithTraining)
      {
        var error = 0.0d;

        unsafe
        {
          for (var i = 0; i < _dataSize; i++)
          {
            fixed (double* pInputs = &_trainingData[i * _layers[0]])
            {
              double* retVal;
              _net.FeedForward(pInputs, &retVal);

              for (var j = 0; j < _layers[_layers.Length - 1]; j++)
                error += CalculateError(_expectedData[i * _layers[_layers.Length - 1] + j], retVal[j]);
            }
          }
        }

        var e = 1 - error / _expectedData.Length;
        var accuracy = 100 * e;        
        var success = accuracy >= 95.0;

        stability &= success;
        totalError += e;
      }
    }

    private static double CalculateError(double expected, double received) => expected == 0 ? Math.Abs(received) : expected - Math.Abs(received);

    private static void Convert2DArrayTo1D(double[,] input, out double[] output)
    {
      output = new double[input.Length];

      Buffer.BlockCopy(input, 0, output, 0, output.Length * sizeof(double));
    }

    #endregion

    #region Events

    public EventHandler TrainingComplete;

    private void OnTrainingComplete(EventArgs e)
    {
      var handler = TrainingComplete;

      handler?.Invoke(this, e);
    }

    #endregion
  }
}
