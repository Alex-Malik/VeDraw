using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeDraw.ViewModels
{
    public class ErrorWindowViewModel
    {
        public ErrorWindowViewModel(Exception e)
        {
            Message = e.Message;
            ErrorType = e.GetType().ToString();
            ErrorMessage = e.Message;
            ErrorStackTrace = e.StackTrace;
        }

        public string Message { get; }

        public string ErrorType { get; }

        public string ErrorMessage { get; }

        public string ErrorStackTrace { get; }
    }
}
