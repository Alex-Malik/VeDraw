using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace VeDraw.Models
{
    public class Command : ICommand
    {
        private readonly Action<object> ExecuteAction;
        private readonly Func<object, bool> CanExecuteFunc;

        public Command(Action execute, Func<bool> canExecute)
        {
            ExecuteAction = _ => execute();
            CanExecuteFunc = _ => canExecute();
        }

        public Command(Action<object> execute, Func<object, bool> canExecute)
        {
            ExecuteAction = execute;
            CanExecuteFunc = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return CanExecuteFunc(parameter);
        }

        public void Execute(object parameter)
        {
            ExecuteAction(parameter);
        }

        public void Update()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
