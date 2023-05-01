using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UICourierBag : MonoBehaviour, IUIPackageOwner
{
    List<UIPackage> _packages = new List<UIPackage>();

    public Courier Courier { get; set; }

    public void Add(UIPackage package)
    {
        float totalWidth = _packages.Sum(p => p.Width);

        package.transform.SetParent(transform, false);
        package.Position = new Vector2(totalWidth, 0.0f);

        _packages.Add(package);

        package.Package.OnDelivered += OnDestroyPackage;
        package.Package.OnExpired += OnDestroyPackage;

        Debug.Log($"Adding package to bag {_packages.Count}");
    }

    public void Remove(UIPackage package)
    {
        _packages.Remove(package);
        package.Package.OnDelivered -= OnDestroyPackage;
        package.Package.OnExpired -= OnDestroyPackage;

        Courier.RemovePackage(package.Package);
        package.Unloaded(Courier);

        // Only reorder if at depot so can see what expires when out delivering
        if (Courier.IsDispatched == false)
        {
            Debug.Log("Reordering bag");
            Reorder();
        }

        Debug.Log($"Removing package from bag {_packages.Count}");
    }

    void OnDestroyPackage(Package package)
    {
        UIPackage uiPackage = _packages.Find(p => p.Package == package);
        if (uiPackage != null)
        {
            Remove(uiPackage);
            Destroy(uiPackage.gameObject);
        }
    }

    public void SetCanDrag(bool canDrag)
    {
        foreach(UIPackage package in _packages)
        {
            package.CanDrag = canDrag;
        }
    }

    void Reorder()
    {
        float totalWidth = 0.0f;
        foreach(UIPackage package in _packages)
        {
            package.Position = new Vector2(totalWidth, 0.0f);
            totalWidth += package.Width;
        }
    }
}
