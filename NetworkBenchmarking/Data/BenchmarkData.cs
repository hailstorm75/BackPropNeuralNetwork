using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkBenchmarking
{
  public class BenchmarkData
  {
    public int[] Neurons { get; set; }
    public bool Stable { get; set; }
    public double Error { get; set; }
  }
}
