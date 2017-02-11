using ChattingInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChattingClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static IChattingService Server;
        private static DuplexChannelFactory<IChattingService> _channelFactory;

        public MainWindow()
        {
            InitializeComponent();

            _channelFactory = new DuplexChannelFactory<IChattingService>(new ClientCallback(), "ChattingServiceEndpoint");
            Server = _channelFactory.CreateChannel();

        }

        public void TakeMessage(string message, string username)
        {
            TextDisplayTextBox.Text += username + ": " + message + "\n";
            TextDisplayTextBox.ScrollToEnd();
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageTextBox.Text.Length == 0) return;

            Server.SendMessageToAll(MessageTextBox.Text, UserNameTextBox.Text);
            TakeMessage(MessageTextBox.Text, "You");
            MessageTextBox.Text = "";
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            int returnValue = Server.Login(UserNameTextBox.Text);
            if(returnValue == 1)
            {
                MessageBox.Show("User already logged in.");
            }
            else if(returnValue == 0)
            {
                TextDisplayTextBox.Text += ">>> You are now logged in! \n";
                UserNameTextBox.IsEnabled = false;
                LoginButton.IsEnabled = false;

                AddUsersToList(Server.GetCurrentUsers());
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Server.LogOut();
        }

        public void AddUserToList(string username)
        {
            if (!UserListBox.Items.Contains(username))
            {
                UserListBox.Items.Add(username);
            }
        }

        public void RemoveUserFromList(string username)
        {
            if (UserListBox.Items.Contains(username))
            {
                UserListBox.Items.Remove(username);
            }
        }

        private void AddUsersToList(List<string> users)
        {
            foreach(string user in users)
            {
                AddUserToList(user);
            }
        }
    }
}
