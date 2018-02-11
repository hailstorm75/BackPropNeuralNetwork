using PropertyChanged;
using System.ComponentModel;

namespace NetworkBenchmarking
{
  [AddINotifyPropertyChangedInterface]
  public class BaseViewModel : INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };
  }
}
