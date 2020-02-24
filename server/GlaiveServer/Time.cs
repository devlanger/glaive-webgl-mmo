using System;
using System.Collections.Generic;
using System.Text;

namespace GlaiveServer
{
    public class Time
    {
        public static int time
        {
            get
            {
                return Environment.TickCount / 1000;
            }
        }
    }
}
