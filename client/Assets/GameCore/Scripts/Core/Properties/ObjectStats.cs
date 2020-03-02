using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCoreEngine
{
    public enum ObjectStats : byte
    {
        LVL = 1,
        STR = 2,
        DEX = 3,
        VIT = 4,
        HP = 5,
        MANA = 6,
        ATT_POWER = 7,
        MAGIC_POWER = 8,
        CLASS = 9,
        EXPERIENCE = 10,
        NAME = 11,
        MAX_HP = 12,
        MAX_MANA = 13,
        MAX_EXPERIENCE = 14,
        INT = 15,
        STATPOINTS = 16,
        DEAD = 17,
        RESPAWN_TIME = 18,
        GOLD = 19,
    }

    public enum ObjectType : byte
    {
        UINT = 1,
        USHORT = 2,
        INT = 3,
        BYTE = 4,
    }
}