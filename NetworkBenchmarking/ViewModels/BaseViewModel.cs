using PropertyChanged;
using System.ComponentModel;

namespace NetworkBenchmarking
{
  [AddINotifyPropertyChangedInterface]
  public class BaseViewModel : INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

    public virtual void CloseWindow(System.ComponentModel.CancelEventArgs e) { }
  }
}
