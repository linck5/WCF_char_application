using ChattingInterfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ChattingServer
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single)]

    public class ChattingService : IChattingService
    {
        public ConcurrentDictionary<string, ConnectedClient> _connectedClients = 
            new ConcurrentDictionary<string, ConnectedClient>();

        public int Login(string username)
        {
            foreach(var connectedClient in _connectedClients)
            {
                if(connectedClient.Key.ToLower() == username.ToLower())
                {
                    return 1;
                }
            }

            var establishedUserConnection = OperationContext.Current.GetCallbackChannel<IClient>();
            ConnectedClient newClient = new ConnectedClient();
            newClient.connection = establishedUserConnection;
            newClient.UserName = username;

            _connectedClients.TryAdd(username, newClient);

            UpdateHelper(0, username);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("User {0} logged in at {1}", newClient.UserName, System.DateTime.Now);
            Console.ResetColor();

            return 0;
        }

        public void LogOut()
        {
            ConnectedClient client = GetMyClient();
            if(client != null)
            {
                ConnectedClient removedClient;
                _connectedClients.TryRemove(client.UserName, out removedClient);

                UpdateHelper(1, removedClient.UserName);

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("User {0} logged out at {1}", removedClient.UserName, System.DateTime.Now);
                Console.ResetColor();
            }
            else
            {
                //TODO
            }
        }

        private ConnectedClient GetMyClient()
        {
            var establishedUserConnection = OperationContext.Current.GetCallbackChannel<IClient>();
            foreach (var connectedClient in _connectedClients)
            {
                if (connectedClient.Value.connection == establishedUserConnection)
                {
                    return connectedClient.Value;
                }
            }
            return null;


        }

        public void SendMessageToAll(string message, string username)
        {
            foreach(var connectedClient in _connectedClients)
            {
                if(connectedClient.Key.ToLower() != username.ToLower())
                {
                    connectedClient.Value.connection.GetMessage(message, username);
                }
            }
        }

        private void UpdateHelper(int value, string username)
        {
            foreach (var connectedClient in _connectedClients)
            {
                if(connectedClient.Value.UserName.ToLower() != username.ToLower())
                {
                    connectedClient.Value.connection.GetUpdate(value, username);
                }
            }
        }

        public List<string> GetCurrentUsers()
        {
            List<string> currentUsers = new List<string>();
            foreach(var connectedClient in _connectedClients)
            {
                currentUsers.Add(connectedClient.Value.UserName);
            }
            return currentUsers;
        }
    }
}
