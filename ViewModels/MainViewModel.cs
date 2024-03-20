using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using SimpleMessenger.Messages;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMessenger.ViewModels;

public partial class MainViewModel : ObservableObject
{
    public MainViewModel()
    {
        WeakReferenceMessenger.Default.Register<ChangeViewModelMessage>(this, (sender, message) =>
        {
            CurrentViewModel = message.ViewModel;
        });


        CurrentViewModel = new UsernameViewModel();
    }

    [ObservableProperty]
    public BaseViewModel _currentViewModel = null!;
}
