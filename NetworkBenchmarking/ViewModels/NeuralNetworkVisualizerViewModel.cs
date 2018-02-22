using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace NetworkBenchmarking
{
  public class NeuralNetworkVisualizerViewModel
  {
    #region Properties

    public double Width { get; set; } = 500;
    public double Height { get; set; } = 500;
    public List<VisualizerNode> Items { get; set; }
    public Canvas SelectedShape { get; set; }

    #endregion

    #region Constructor

    public NeuralNetworkVisualizerViewModel()
    {
      Items = new List<VisualizerNode>();

      var node1 = new VisualizerNode(100, 150);
      var node2 = new VisualizerNode(100, 180);
      node1.AddLink(node2);

      Items.Add(node1);
      Items.Add(node2);
    }

    #endregion
  }
}