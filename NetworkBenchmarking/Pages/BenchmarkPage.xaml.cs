namespace NetworkBenchmarking
{
  /// <summary>
  /// Interaction logic for BenchmarkPage.xaml
  /// </summary>
  public partial class BenchmarkPage : BasePage
  {
    public BenchmarkerViewModel ViewModel { get; set; }

    public BenchmarkPage()
    {
      InitializeComponent();
      ViewModel = new BenchmarkerViewModel();
    }

    public void CloseWindow(System.ComponentModel.CancelEventArgs e)
    {
      ViewModel.CloseWindow(e);
    }
  }
}
