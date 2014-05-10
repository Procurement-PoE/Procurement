using System;
using System.Windows.Input;

namespace Procurement.ViewModel
{
    public class DelegateCommand : ICommand
    {
        private Action<object> run;
        public DelegateCommand(Action<object> run)
        {
            this.run = run;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            run(parameter);
        }
    }
}
