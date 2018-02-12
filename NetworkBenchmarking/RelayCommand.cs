using System.Windows.Input;
using System;
using System.Diagnostics;

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

  public class RelayCommand<T> : ICommand
  {
    #region Fields

    readonly Action<T> _execute = null;
    readonly Predicate<T> _canExecute = null;

    #endregion

    #region Constructor

    public RelayCommand(Action<T> execute)
        : this(execute, null)
    {
    }

    /// <summary>
    /// Creates a new command.
    /// </summary>
    /// <param name="execute">The execution logic.</param>
    /// <param name="canExecute">The execution status logic.</param>
    public RelayCommand(Action<T> execute, Predicate<T> canExecute)
    {
      _execute = execute ?? throw new ArgumentNullException("execute");
      _canExecute = canExecute;
    }

    #endregion

    #region ICommand Members

    [DebuggerStepThrough]
    public bool CanExecute(object parameter)
    {
      return _canExecute == null ? true : _canExecute((T)parameter);
    }

    public event EventHandler CanExecuteChanged
    {
      add { CommandManager.RequerySuggested += value; }
      remove { CommandManager.RequerySuggested -= value; }
    }

    public void Execute(object parameter)
    {
      _execute((T)parameter);
    }

    #endregion
  }
}
