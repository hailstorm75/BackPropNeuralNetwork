using System.Windows;

namespace NetworkBenchmarking
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    #region Properties

    private BenchmarkerViewModel ViewModel { get; set; } 
    
    #endregion

    //--------------------------------------------------
    public MainWindow()
    //--------------------------------------------------
    {
      InitializeComponent();

      ViewModel = new BenchmarkerViewModel();
      DataContext = ViewModel;     
    }

    #region Events

    //--------------------------------------------------
    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) => ViewModel.CloseWindow(e);
    //-------------------------------------------------- 
    
    #endregion
  }
}

