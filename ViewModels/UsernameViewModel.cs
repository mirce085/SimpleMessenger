using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SimpleMessenger.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SimpleMessenger.ViewModels;

[INotifyPropertyChanged]
partial class UsernameViewModel : BaseViewModel
{
    [ObservableProperty]
    private string? _username;


    [RelayCommand]
    public void JoinChat()
    {
        if(Username == null)
        {
            MessageBox.Show("Enter username!");
            return;
        }

        var message = new ChangeViewModelMessage(new MessengerViewModel(Username));

        WeakReferenceMessenger.Default.Send(message);
    }
}
