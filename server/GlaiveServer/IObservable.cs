using System;
using System.Collections.Generic;
using System.Text;

namespace GlaiveServer
{
    public interface IObservable
    {
        bool Hidden { get; set; }
    }
}
