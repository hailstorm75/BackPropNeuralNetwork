using System.Collections.Generic;

namespace NetworkBenchmarking
{
  public class BenchmarkSettings
  {
    public int Inputs { get; set; }
    public int Outputs { get; set; }
    public List<HiddenLayerDefinition> HiddenLayerDefinitions { get; set; }
    public int Iterations { get; set; }
    public int ProgressTick { get { return HiddenLayerDefinitions.Count; } }
    public double Tolerance { get; set; }
    public DataGenerator.Operation Operation { get; set; }
  }
}
