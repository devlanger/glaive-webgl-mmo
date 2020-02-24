using System;
using System.Net;
using System.Threading;
using WebSocketSharp.Server;

namespace GlaiveServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ZonesManager zones = new ZonesManager();
            ServerInstance server = new ServerInstance().Start("0.0.0.0", 2700);
        }
    }
}
