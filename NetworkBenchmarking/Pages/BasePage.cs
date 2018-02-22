using System.Threading.Tasks;
using System.Windows.Controls;

namespace NetworkBenchmarking
{
  public class BasePage : Page
  {
    #region Properties

    /// <summary>
    /// Set load animation
    /// </summary>
    public PageAnimation PageLoadAnimation { get; set; }
    /// <summary>
    /// Set unload animation
    /// </summary>
    public PageAnimation PageUnloadAnimation { get; set; }
    /// <summary>
    /// Duration of load/unload animation
    /// </summary>
    public float AnimationDuration { get; set; }

    #endregion

    #region Constructor

    /// <summary>
    /// Default constructor
    /// </summary>
    public BasePage()
    {
      if (PageLoadAnimation != PageAnimation.None)
        Visibility = System.Windows.Visibility.Collapsed;

      Loaded += BasePage_Loaded;
      Unloaded += BasePage_Unloaded;
    } 

    #endregion

    #region Events

    private async void BasePage_Unloaded(object sender, System.Windows.RoutedEventArgs e)
    {
      await AnimateOutAsync();
    }

    private async void BasePage_Loaded(object sender, System.Windows.RoutedEventArgs e)
    {
      await AnimateInAsync();
    }

    #endregion

    #region Methods

    /// <summary>
    /// Executes selected load animation
    /// </summary>
    public async Task AnimateInAsync()
    {
      if (PageLoadAnimation == PageAnimation.None) return;

      switch (PageLoadAnimation)
      {
        case PageAnimation.SlideAndFadeInFromRight:
          await this.SlideAndFadeInFromRight(AnimationDuration);
          break;
        case PageAnimation.SlideAndFadeInFromLeft:
          await this.SlideAndFadeInFromLeft(AnimationDuration);
          break;
      }
    }

    /// <summary>
    /// Executes selected unload animation
    /// </summary>
    public async Task AnimateOutAsync()
    {
      if (PageUnloadAnimation == PageAnimation.None) return;

      switch (PageUnloadAnimation)
      {
        case PageAnimation.SlideAndFadeOutToLeft:
          await this.SlideAndFadeOutToLeft(AnimationDuration);
          break;
        case PageAnimation.SlideAndFadeOutToRight:
          await this.SlideAndFadeOutToRight(AnimationDuration);
          break;
      }
    }

    #endregion
  }

  public class BasePage<VM> : BasePage 
    where VM : BaseViewModel, new()
  {
    #region Fields

    private VM _viewModel;

    #endregion

    #region Properties

    public VM ViewModel
    {
      get => _viewModel; set
      {
        if (_viewModel == value) return;

        _viewModel = value;

        DataContext = _viewModel;
      }
    }

    #endregion

    #region Constructor

    public BasePage() : base()
    {
      ViewModel = new VM();
    } 

    #endregion
  }
}
