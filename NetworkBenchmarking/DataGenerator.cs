using System;

namespace NetworkBenchmarking
{
  /// <summary>
  /// Generates a data-set
  /// </summary>
  public class DataGenerator
  {
    #region Properties

    public enum Operation
    {
      Conjunction = 1,
      Disjunction = 2,
      Implication = 3,
      Equivalence = 4
    }

    #endregion

    #region Fields

    private int _inputCount;
    private int _outputCount;

    #endregion

    #region Constructor

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="inputCount">Number of elements in <see cref="TrainingData"/></param>
    /// <param name="outputCount">Number of elements in <see cref="ExpectedData"/></param>
    /// <param name="operationType">Logical operation which is to be applied to <see cref="TrainingData"/> values to create <see cref="ExpectedData"/></param>
    public DataGenerator(int inputCount, int outputCount)
    {
      if (inputCount <= 2 || outputCount < 1)
        throw new ArgumentException("Invalid input or output count.");
      if (inputCount <= outputCount)
        throw new ArgumentException("Input count less or equals than output count is prohibited.");

      _inputCount = inputCount;
      _outputCount = outputCount;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Generates a truth table
    /// </summary>
    /// <param name="inputCount">Number of elements in truth table</param>
    public double[,] GenerateTrainingData()
    {
      // Initializing array
      var trainingData = new double[(int)Math.Pow(2, _inputCount), _inputCount];

      for (var i = 0; i < trainingData.GetLength(1); ++i)
      {
        var inc = (int)Math.Pow(2, i);
        var bin = 0;
        var bit = false;

        for (var j = 0; j < trainingData.GetLength(0); ++j)
        {
          if (bin >= inc)
          {
            bit = !bit;
            bin = 0;
          }

          trainingData[j, i] = bit ? 1 : 0;

          ++bin;
        }
      }

      return trainingData;
    }

    /// <summary>
    /// Generates <see cref="ExpectedData"/> by evaluating <see cref="TrainingData"/>
    /// </summary>
    /// <param name="outputCount">Number of elements in <see cref="ExpectedData"/></param>
    /// <param name="operationType">Logical operation which is to be applied to <see cref="TrainingData"/> values to create <see cref="ExpectedData"/></param>
    public double[,] GenerateExpectedData(ref double[,] trainingData, Operation operationType)
    {
      // Initializing array
      var expectedData = new double[trainingData.GetLength(0), _outputCount];

      for (var i = 0; i < trainingData.GetLength(0); ++i)
      {
        if (trainingData.GetLength(1) % _outputCount == 0)
        {
          for (var j = 0; j < _outputCount; j++)
            for (var k = 0; k < trainingData.GetLength(1) / _outputCount - 1; ++k)
              expectedData[i, j] = trainingData[i, j * _outputCount + k] + trainingData[i, j * _outputCount + k + 1];
        }
        else
        {
          for (var j = 0; j < _outputCount; ++j)
            for (var k = 0; k < trainingData.GetLength(1) - _outputCount; ++k)
              expectedData[i, j] = Evaluate(ref trainingData[i, j + k], ref trainingData[i, j + k + 1], ref operationType);
        }
      }

      return expectedData;
    }

    /// <summary>
    /// Evaluates values using boolean logic
    /// </summary>
    /// <param name="a">First input parameter</param>
    /// <param name="b">Second input parameter</param>
    /// <param name="operation">Boolean operation</param>
    /// <returns>Result</returns>
    private static double Evaluate(ref double a, ref double b, ref Operation operation)
    {
      if (a > 1 || b > 1)
        throw new ArgumentException("Argument values aren't equal to 1 or 0.");

      var result = true;
      var aBool = Convert.ToBoolean(a);
      var bBool = Convert.ToBoolean(b);

      switch (operation)
      {
        case Operation.Conjunction:
          result = aBool & bBool;     // a ʌ b
          break;
        case Operation.Disjunction:
          result = aBool | bBool;     // a v b
          break;
        case Operation.Implication:
          result = !aBool | bBool;    // a => b
          break;
        case Operation.Equivalence:
          result = !(!aBool & bBool); // a <=> b
          break;
      }

      return result ? 1d : 0d;
    }

    #endregion
  }
}
