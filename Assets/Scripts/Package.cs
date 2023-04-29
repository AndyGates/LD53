using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Package
{
    public Location Target { get; }
    public int Size { get; }

    public Package(int size, Location target)
    {
        Size = size;
        Target = target;
    } 
}
