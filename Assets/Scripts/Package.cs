using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using UnityEngine;


[Serializable]
public class Package {
    public PostageType Delivery;
    public int Value;
    public Location Target;
    public int Size;
    public float DeliveryBy;
    public float DeliveryTime;

    public System.Action<Package> OnDelivered;
    public System.Action<Package> OnExpired;

    public Package(float deliverTime, int size, int value, Location target, PostageType delivery)
    {
        DeliveryTime = deliverTime;
        DeliveryBy = Time.time + deliverTime;
        Size = size;
        Value = value;
        Target = target;
        Delivery = delivery;
    }

    public bool HasExpired()
    {
        return DeliveryBy < Time.time;
    }
}
