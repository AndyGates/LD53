using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using UnityEngine;

//public PostageType;


[Serializable]
public class Package {
    public PostageType Delivery;
    public int Value;
    public Location Target;
    public int Size;

    public Package(int size, int value, Location target, PostageType delivery)
    {
        Size = size;
        Value = value;
        Target = target;
        Delivery = delivery;
    } 
}
