using System;
using System.Collections.Generic;
using System.Text;

namespace GlaiveServer
{
    public class Drop : WorldObject
    {
        public override PacketsSender.SpawnData.SpawnType SpawnType
        {
            get
            {
                return PacketsSender.SpawnData.SpawnType.DROP;
            }
        }
    }
}
