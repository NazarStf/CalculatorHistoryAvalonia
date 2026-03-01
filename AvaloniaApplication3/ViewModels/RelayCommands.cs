using System;
using System.Windows.Input;

public class RelayCommand : ICommand
{
    private readonly Action exe;

    public RelayCommand(Action execute){
        exe = execute;
    }

    public event EventHandler? CanExecuteChanged;
    public bool CanExecute(object? parameter) => true;
    public void Execute(object? parameter){
        exe();
    }
}