using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ChattingInterfaces
{
    
    [ServiceContract(CallbackContract = typeof(IClient))]
    public interface IChattingService
    {
        [OperationContract]
        void LogOut();
        [OperationContract]
        int Login(string username);
        [OperationContract]
        void SendMessageToAll(string message, string username);
        [OperationContract]
        List<string> GetCurrentUsers();
    }
}
