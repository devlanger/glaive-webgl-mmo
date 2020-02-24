using System;
using System.Collections.Generic;
using System.Text;

namespace GlaiveServer
{
    public class Zone
    {
        public CharacterRespawner respawner;

        public Zone()
        {
            respawner = new CharacterRespawner();
        }
    }
}
