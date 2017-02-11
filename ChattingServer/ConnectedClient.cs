using ChattingInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChattingServer
{
    public class ConnectedClient
    {
        public IClient connection;

        public String UserName;
    }
}
