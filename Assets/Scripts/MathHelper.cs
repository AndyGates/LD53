using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathHelper
{
    public static bool Approximately(Vector3 a, Vector3 b)
    {
        return Approximately(a.x, b.x) &&
                Approximately(a.y, b.y) &&
                Approximately(a.z, b.z);
    }

    public static bool Approximately(float a, float b)
    {
        return Mathf.Abs(a - b) < 0.04f;
    }
}
