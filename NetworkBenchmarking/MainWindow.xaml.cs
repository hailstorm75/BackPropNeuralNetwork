using System.Windows;
using System.Windows.Controls;

namespace NetworkBenchmarking
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    #region Properties

    private BenchmarkerViewModel ViewModel { get; set; }
    private BenchmarkPage CurrentPage { get; set; }

    #endregion

    //--------------------------------------------------
    public MainWindow()
    //--------------------------------------------------
    {
      InitializeComponent();

      CurrentPage = new BenchmarkPage();
      this.Content = CurrentPage;
    }

    #region Events

    //--------------------------------------------------
    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) => CurrentPage.CloseWindow(e);
    //-------------------------------------------------- 
    
    #endregion
  }
}

