using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using UnityEngine;

//public PostageType;

public class Package
{   public int Size { get; }
    public PostageType Delivery { get; }
    public int Value { get; }
    public Location Target { get; }

    public Package(int size, int value, Location target, PostageType delivery)
    {
        Size = size;
        Value = value;
        Target = target;
        Delivery = delivery;
    } 
}
