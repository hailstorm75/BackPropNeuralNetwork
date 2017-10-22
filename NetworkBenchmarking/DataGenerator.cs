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

    /// <summary>
    /// Training data property
    /// </summary>
    public double[,] TrainingData { get; private set; }

    /// <summary>
    /// Expected data property
    /// </summary>
    public double[,] ExpectedData { get; private set; }

    #endregion

    #region Constructor

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="inputCount">Number of elements in <see cref="TrainingData"/></param>
    /// <param name="outputCount">Number of elements in <see cref="ExpectedData"/></param>
    /// <param name="operationType">Logical operation which is to be applied to <see cref="TrainingData"/> values to create <see cref="ExpectedData"/></param>
    public DataGenerator(int inputCount, int outputCount, Operation operationType)
    {
      if (inputCount <= 2 || outputCount < 1)
        throw new ArgumentException("Invalid input or output count.");
      if (inputCount <= outputCount)
        throw new ArgumentException("Input count less or equals than output count is prohibited.");

      GenerateTrainingData(ref inputCount);
      GenerateExpectedData(ref outputCount, ref operationType);
    }

    #endregion

    #region Methods

    /// <summary>
    /// Generates a truth table
    /// </summary>
    /// <param name="inputCount">Number of elements in truth table</param>
    private void GenerateTrainingData(ref int inputCount)
    {
      // Initializing array
      TrainingData = new double[(int)Math.Pow(2, inputCount), inputCount];

      for (var i = 0; i < TrainingData.GetLength(1); i++)
      {
        var inc = (int)Math.Pow(2, i);
        var bin = 0;
        var bit = false;

        for (var j = 0; j < TrainingData.GetLength(0); j++)
        {
          if (bin >= inc)
          {
            bit = !bit;
            bin = 0;
          }

          TrainingData[j, i] = bit ? 1 : 0;

          bin++;
        }
      }
    }

    /// <summary>
    /// Generates <see cref="ExpectedData"/> by evaluating <see cref="TrainingData"/>
    /// </summary>
    /// <param name="outputCount">Number of elements in <see cref="ExpectedData"/></param>
    /// <param name="operationType">Logical operation which is to be applied to <see cref="TrainingData"/> values to create <see cref="ExpectedData"/></param>
    private void GenerateExpectedData(ref int outputCount, ref Operation operationType)
    {
      // Initializing array
      ExpectedData = new double[TrainingData.GetLength(0), outputCount];

      for (var i = 0; i < TrainingData.GetLength(0); i++)
      {
        if (TrainingData.GetLength(1) % outputCount == 0)
          for (var j = 0; j < outputCount; j++)
          for (var k = 0; k < TrainingData.GetLength(1) / outputCount - 1; k++)
            ExpectedData[i, j] = TrainingData[i, j * outputCount + k] + TrainingData[i, j * outputCount + k + 1];
        else
          for (var j = 0; j < outputCount; j++)
          for (var k = 0; k < TrainingData.GetLength(1) - outputCount; k++) // inputCount - (outputCount - 1) - 1
            ExpectedData[i, j] = Evaluate(ref TrainingData[i, j + k], ref TrainingData[i, j + k + 1], ref operationType);
      }
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
