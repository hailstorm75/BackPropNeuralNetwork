using System.Collections.Generic;
using System.Windows.Media;

namespace NetworkBenchmarking
{
  public class VisualizerNode : BaseViewModel
  {
    #region Properties

    public double Radius { get; set; } = 40;
    public Brush BackColor { get; set; } = Brushes.Blue;
    public double PosX { get; set; } = 0;
    public double PosY { get; set; } = 0;
    public List<VisualizerLink> Links { get; set; }

    #endregion

    #region Constructors

    public VisualizerNode(double posX, double posY)
    {
      Links = new List<VisualizerLink>();
      PosX = posX;
      PosY = posY;
    }

    public VisualizerNode(double posX, double posY, List<VisualizerLink> links)
    {
      Links = links;
      PosX = posX;
      PosY = posY;
    }

    #endregion

    #region Methods

    public void AddLink(VisualizerNode node) => Links.Add(new VisualizerLink(PosX, PosY, node.PosX, node.PosY));

    #endregion
  }
}
