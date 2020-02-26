using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using WebSocketSharp.Server;

namespace GlaiveServer
{
    public class ServerInstance
    {
        private Thread thread;

        public ServerInstance()
        {
            
        }

        public ServerInstance Start(string ip, int port)
        {
            var wssv = new WebSocketServer(IPAddress.Parse(ip), port, false);
            wssv.AddWebSocketService<User>("/");
            wssv.Start();
            while (true)
            {
                Thread.Sleep(100);
                foreach (var item in CharactersManager.characters.ToList())
                {
                    item.Value.Update();
                }

                CharacterRespawner.Instance.Update();
            }
            Console.ReadKey(true);
            wssv.Stop();
        }
    }
}
