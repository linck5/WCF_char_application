using ChattingInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ChattingClient
{
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Multiple)]

    public class ClientCallback : IClient
    {
        [OperationContract]
        public void GetMessage(string message, string username)
        {
            ((MainWindow)Application.Current.MainWindow).TakeMessage(message, username);
        }

        public void GetUpdate(int value, string username)
        {
            switch (value)
            {
                case 0:
                    ((MainWindow)Application.Current.MainWindow).AddUserToList(username);
                    ((MainWindow)Application.Current.MainWindow).TextDisplayTextBox.Text +=
                        ">>> " + username + " joined the chat. \n";
                    break;
                case 1:
                    ((MainWindow)Application.Current.MainWindow).RemoveUserFromList(username);
                    ((MainWindow)Application.Current.MainWindow).TextDisplayTextBox.Text +=
                        ">>> " + username + " left the chat. \n";
                    break;
            }
        }
    }
}
