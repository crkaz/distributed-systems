using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;

namespace SignalRClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        protected HubConnection connection;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Username { get; set; }

        public string Message { get; set; }

        private string connectButtonText;
        public string ConnectButtonText
        {
            get
            {
                return connectButtonText;
            }
            set
            {
                connectButtonText = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ConnectButtonText"));
            }
        }

        public bool Connected { get; set; }

        private ObservableCollection<string> messages;
        public ObservableCollection<string> Messages
        {
            get
            {
                return messages;
            }
            set
            {
                messages = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Messages"));
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            Username = "";
            Message = "";
            Messages = new ObservableCollection<string>();
            ConnectButtonText = "Connect";
            Connected = false;

            connection = new HubConnectionBuilder()
                //.WithUrl("https://172.17.186.209:5000/ChatHub")
                .WithUrl("https://localhost:44361/ChatHub")
                .Build();

            connection.On<string, string>("GetMessage",
            new Action<string, string>((username, message) =>
            GetMessage(username, message)));

            connection.Closed += async (exception) => { await ConnectionClosedHandler(exception); };

            DataContext = this;
        }

        private Task ConnectionClosedHandler(Exception exception)
        {
            Console.WriteLine(exception);
            Messages = new ObservableCollection<string>();
            return Task.CompletedTask;
        }

        private void GetMessage(string username, string message)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                var chat = $"{username}: {message}";
                Messages.Add(chat);
            }));
        }

        private void ToggleConnection(object sender, RoutedEventArgs e)
        {
            if (Connected == true)
            {
                Disconnect();
            }
            else
            {
                Connect();
            }
        }

        private async void Connect()
        {
            try
            {
                await connection.StartAsync();
                Messages.Add("Connection opened...");
                ConnectButtonText = "Disconnect";
                Connected = true;
            }
            catch (Exception ex)
            {
                Messages.Add("Connection failed...");
                Messages.Add(ex.Message);
            }
        }

        private async void SendMessage(object sender, RoutedEventArgs e)
        {
            try
            {
                await connection.InvokeAsync("BroadcastMessage", Username,
               Message);
            }
            catch (Exception ex)
            {
                Messages.Add("Failed to send message...");
                Messages.Add(ex.Message);
            }
        }

        private void Disconnect()
        {
            try
            {
                connection.StopAsync();
                ConnectButtonText = "Connect";
                Messages.Add("Disconnected");
                Connected = false;
            }
            catch (Exception ex)
            {
                Messages.Add("Failed to disconnect...");
                Messages.Add(ex.Message);
            }

        }
    }
}
