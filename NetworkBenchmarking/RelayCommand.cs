using System.Windows.Input;
using System;

namespace NetworkBenchmarking
{
  public class RelayCommand : ICommand
  {
    private Action _action;
    public event EventHandler CanExecuteChanged = (sender, e) => { };

    public RelayCommand(Action action)
    {
      _action = action;
    }

    public bool CanExecute(object parameter) => true;

    public void Execute(object parameter)
    {
      _action();
    }
  }
}
