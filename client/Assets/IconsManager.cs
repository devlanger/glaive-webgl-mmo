using GameCoreEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconsManager : Singleton<IconsManager>
{
    public IconsScriptableManager manager;

    public Sprite GetIcon(int id)
    {
        return manager.GetItem(id);
    }
}
