using SimpleMessenger.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMessenger.Messages;

public class ChangeViewModelMessage
{
    public ChangeViewModelMessage(BaseViewModel viewModel)
    {
        ViewModel = viewModel;
    }
    public BaseViewModel ViewModel { get; set; }
}
