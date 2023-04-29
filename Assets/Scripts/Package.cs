using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Package
{    public int Size { get; }
    public int Postage { get; }
    public int Value { get; }
    public Location Target { get; }

    public Package(int size, int postage, int value, Location target)
    {
        Size = size;
        Postage = postage;
        Value = value;
        Target = target;
    } 
}
