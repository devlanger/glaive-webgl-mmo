using System;
using System.Collections.Generic;
using System.Text;

namespace GlaiveServer
{
    public class ZonesManager
    {
        public static ZonesManager Instance {get; set;}
        public Dictionary<int, Zone> zones = new Dictionary<int, Zone>();

        public Zone GetZone(int id)
        {
            return zones[id];
        }

        public ZonesManager()
        {
            Instance = this;
        }
    }
}
