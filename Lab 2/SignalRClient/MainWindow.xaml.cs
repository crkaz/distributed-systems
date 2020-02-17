using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace SignalRClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        protected HubConnection connection;

        public string Username { get; set; }

        public string Message { get; set; }

        public ObservableCollection<string> Messages { get; set; }

        public MainWindow()
        {
            InitializeComponent();


            Username = "";
            Message = "";
            Messages = new ObservableCollection<string>();

            connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:44361/ChatHub")
                .Build();

            connection.On<string, string>("GetMessage",
            new Action<string, string>((username, message) =>
            GetMessage(username, message)));
         
            DataContext = this;
        }

        private void GetMessage(string username, string message)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                var chat = $"{username}: {message}";
                Messages.Add(chat);
            }));
        }

        private async void Connect(object sender, RoutedEventArgs e)
        {
            try
            {
                await connection.StartAsync();
                Messages.Add("Connection opened...");
            }
            catch
            {
                Messages.Add("Connection failed...");
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
                Messages.Add(ex.Message);
            }
        }
    }
}
