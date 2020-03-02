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
            DatabaseUtils.Initialize();
            ItemsManager items = new ItemsManager();
            ShopManager shop = new ShopManager();
            BaseMobsManager mobs = new BaseMobsManager();
            CharacterRespawner respawn = new CharacterRespawner();
            ZonesManager zones = new ZonesManager();
            ServerInstance server = new ServerInstance().Start("0.0.0.0", 2700);
        }
    }
}
