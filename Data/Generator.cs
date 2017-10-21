using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
  /// <summary>
  /// Generates a dataset
  /// </summary>
  public class Generator
  {
    public enum Operation
    {
      Conjunction = 1,
      Disjunction = 2,
      Implication = 3,
      Equivelance = 4
    }

    /// <summary>
    /// Training data property
    /// </summary>
    public double[,] TrainingData { get; set; }

    /// <summary>
    /// Expected data property
    /// </summary>
    public double[,] ExpectedData { get; set; }

    #region Constructor

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="inputCount">Number of elements in <see cref="TrainingData"/></param>
    /// <param name="outputCount">Number of elements in <see cref="ExpectedData"/></param>
    /// <param name="operationType">Logical operation which is to be applied to <see cref="TrainingData"/> values to create <see cref="ExpectedData"/></param>
    public Generator(int inputCount, int outputCount, Operation operationType)
    {
      if (inputCount <= outputCount)
        throw new ArgumentException("Input count less or equals than output count is prohibited.");

      TrainingData = GenerateTrainingData(ref inputCount);
      ExpectedData = GenerateExpectedData(ref outputCount, ref operationType);
    }

    #endregion

    #region Methods

    private double[,] GenerateTrainingData(ref int inputCount)
    {
      TrainingData = new double[(int)Math.Pow(2, inputCount), inputCount];
      for (int i = 0; i < TrainingData.GetLength(1); i++)
      {
        int incr = (int)Math.Pow(2, i);
        int bin = 0;
        bool bit = false;

        for (int j = 0; j < TrainingData.GetLength(0); j++)
        {
          if (bin + 1 == incr)
          {
            bit = !bit;
            bin = 0;
          }
          else bin++;

          TrainingData[j, i] = bit ? 1 : 0;          
        }
      }
      return null;
    }

    private double[,] GenerateExpectedData(ref int outputCount, ref Operation operationType)
    {
      return null;
    }

    #endregion
  }
}
