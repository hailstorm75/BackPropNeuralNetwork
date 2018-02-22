namespace NetworkBenchmarking
{
  public class VisualizerLink : BaseViewModel
  {
    #region Properties

    public double SourceX { get; set; }
    public double SourceY { get; set; }
    public double DestinationX { get; set; }
    public double DestinationY { get; set; }
    public string ToolTip { get; set; }

    #endregion

    #region Constructor

    public VisualizerLink(double x1, double y1, double x2, double y2, string tooltip = "")
    {
      SourceX = x1;
      SourceY = y1;
      DestinationX = x2;
      DestinationY = y2;
      ToolTip = tooltip;
    }

    #endregion
  }
}
