using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Courier : MonoBehaviour
{
    [SerializeField]
    int _space = 10;

    Map _map;
    Queue<Package> _packages = new Queue<Package>();

    public bool IsAtDepot { get; private set; }

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
