namespace NetworkBenchmarking
{
  public class BenchmarkSettings
  {
    public int Inputs { get; set; }
    public int Outputs { get; set; }
    public int Maximum { get; set; }
    public int Minimum { get; set; }
    public int Iterations { get; set; }
    public int ProgressTick { get { return Maximum - Minimum + 1; } }
    public double Tolerance { get; set; }
    public DataGenerator.Operation Operation { get; set; }
  }
}
