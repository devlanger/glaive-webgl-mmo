using System;
using System.Collections.Generic;
using System.Text;

namespace GlaiveServer
{
    public class Item
    {
        public int baseId;
        public int value1;
        public int value2;

        public ItemBase Base
        {
            get
            {
                return ItemsManager.Instance.items[baseId];
            }
        }

        public virtual void Use(ushort slot, Character user)
        {

        }
    }
}
