using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static float FlatDistance(Vector3 vec, Vector3 target)
    {
        target.y = 0;
        vec.y = 0;
        return Vector3.Distance(vec, target);
    }
}
