using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUIPackageOwner
{
    void Add(UIPackage package);
    void Remove(UIPackage package);
}
