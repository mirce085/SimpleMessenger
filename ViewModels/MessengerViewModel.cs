using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using SimpleMessenger.Messages;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Net.Sockets;
using System.Net;
using System.Collections.ObjectModel;
using SimpleMessenger.Models;

namespace SimpleMessenger.ViewModels;

[INotifyPropertyChanged]
public partial class MessengerViewModel : BaseViewModel
{
    private object _locker = new object();

    private string _username;

    [ObservableProperty]
    private string? _message;

    private Socket _client;

    private SynchronizationContext _syncContext;

    public ObservableCollection<string> Messages { get; private set; }
    public ObservableCollection<User> Users { get; private set; }

    public MessengerViewModel(string username)
    {
        _username = username;

        _syncContext = SynchronizationContext.Current!;

        Messages = new ObservableCollection<string>();

        Users = new ObservableCollection<User>();

        _client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);

        try
        {
            _client.Connect(IPEndPoint.Parse("127.0.0.1:13350"));

        }
        catch (SocketException ex)
        {
            Console.WriteLine(ex.Message);

            return;
        }

        Task.Run(() =>
        {
            try
            {
                while (true)
                {
                    var buffer = new byte[2048];

                    int len = _client.Receive(buffer);

                    var message = Encoding.UTF8.GetString(buffer);

                    var res = message.Substring(1);

                    res = res.Substring(0, len);

                    if (message[0] == '1')
                    {
                        _syncContext.Send(o =>
                        {
                            Messages.Add(res);
                        }, null);
                    }
                    else if (message[0] == '2')
                    {
                        res = res.Replace("\0", "");
                        _syncContext.Send(o =>
                        {
                            Users.Clear();
                            var tempList = JsonSerializer.Deserialize<List<User>>(res)!;
                            foreach (var user in tempList)
                            {
                                Users.Add(user);
                            }

                        }, null);
                    }
                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine(ex.Message);
            }
        });

        var readyMessage = $"2[{_username}]";

        byte[] bytes = Encoding.UTF8.GetBytes(readyMessage);

        _client.Send(bytes);

    }

    [RelayCommand]
    public void SendMessage()
    {
        if (Message == null || Message == string.Empty)
        {
            return;
        }

        var readyMessage = $"1[{_username}]: {Message}";

        var res = readyMessage.Substring(1);

        byte[] bytes = Encoding.UTF8.GetBytes(readyMessage);

        Messages.Add(res);

        Message = string.Empty;

        _client.Send(bytes);
    }
}
