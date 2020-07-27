using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

public class SimpleCommand : ICommand
{
    private SimpleCommand distributeCmd;

    public event EventHandler CanExecuteChanged;

    public Action<object> ExecuteAction { get; set; }
    public Predicate<object> CanExecuteAction { get; set; }

    public bool CanExecute(object parameter)
    {
        if (CanExecuteAction != null)
            return CanExecuteAction.Invoke(parameter);
        return true;
    }

    public void Execute(object parameter)
    {
        ExecuteAction?.Invoke(parameter);
    }

    public void RaiseCanExecuteChanged()
    {
        CanExecuteChanged.Invoke(this, EventArgs.Empty);
    }

    public SimpleCommand(Action<object> action)
    {
        this.ExecuteAction = action;
    }

    public SimpleCommand(Action<object> execute, Predicate<object> canExecute) : this(execute)
    {
        this.CanExecuteAction = canExecute;
    }

    public SimpleCommand(SimpleCommand distributeCmd)
    {
        this.distributeCmd = distributeCmd;
    }
}