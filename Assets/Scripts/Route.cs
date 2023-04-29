using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Route
{
    public IEnumerable<Vector2Int> Path { get; }
    public float Time { get; }
    public float Distance { get; }

    public Route(IEnumerable<Vector2Int> path, float time, float distance)
    {
        Path = path;
        Time = time;
        Distance = distance;
    }
}
