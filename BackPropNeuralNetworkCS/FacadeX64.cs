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
      Convert2DArrayTo1D(trainingData, ref _trainingData);
      Convert2DArrayTo1D(expectedData, ref _expectedData); 

      _mode = Mode.WithTraining;
      _layers = layers;
      _dataSize = trainingData.GetLength(0);

      InitializeNetwork();
    }

    /// <summary>
    /// Training mode constructor
    /// </summary>
    /// <param name="pathToCSV">Path to saved neural network settings</param>
    public FacadeX64(string pathToCSV)
    {      
      _mode = Mode.WithoutTraining;

      // TODO Load net config

      InitializeNetwork();
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
            _net.TrainNetwork(pTrainingData, pExpectedData, _dataSize, 5000, false);
          }
        }
      }
    }

    public void OutputToConsoleTest()
    {
      if (_mode == Mode.WithTraining)
      {
        unsafe
        {
          for (var i = 0; i < _dataSize; i++)
          {
            fixed (double* pInputs = &_trainingData[i * _layers[0]])
            {
              double* retVal;
              _net.FeedForward(pInputs, &retVal);

              for (var j = 0; j < _layers[_layers.Length - 1]; j++)
              {
                Console.WriteLine($"Data set #{j}");
                Console.WriteLine($"\tExpected: {_expectedData[j * _dataSize + i]}\tReceived: {Math.Round(retVal[j], 3)}");
              }
            }
          }
        }
      }
      else
      {
        Console.WriteLine("Network is not in training mode.");
      }
    }

    private void InitializeNetwork()
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

    private bool Convert2DArrayTo1D(double[,] input, ref double[] output)
    {
      if (output == null)
        output = new double[input.Length];
      else if (output.Length == input.Length)
        return false;

      try
      {
        Buffer.BlockCopy(input, 0, output, 0, output.Length * sizeof(double));
        return true;
      }
      catch (Exception)
      {
        return false;
      }
    }

    #endregion
  }
}
