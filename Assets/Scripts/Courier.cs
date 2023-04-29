using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Courier : MonoBehaviour
{
    [SerializeField]
    int _space = 10;

    Map _map;
    Queue<Package> _packages = new Queue<Package>();

    Vector2Int _mapCoord;

    public bool IsAtDepot
    {
        get { return _map == null || _map.IsDepot(_mapCoord); }
    }

    public int CalculateAvailableSpace()
    {
        return 0;
    }

    public void AddPackage(Package package)
    {

    }

    public void Dispatch(Map map)
    {
        _map = map;
    }
}
