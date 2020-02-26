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
            Character mob1 = CharactersManager.CreateCharacter();
            mob1.Pos = new Vector2UInt16(250, 218);
            mob1.baseId = 0;
            Character mob2 = CharactersManager.CreateCharacter();
            mob2.Pos = new Vector2UInt16(250, 220);
            mob2.baseId = 0;

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
