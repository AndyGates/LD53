using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PackageStream : MonoBehaviour
{
    [SerializeField]
    Map _map;

    // TODO: Generations settings. linear speed ramp??

    double _lastGenTime = 0;

    void Awake()
    {
        _lastGenTime = Time.time;
    }

    public Package Next()
    {
        // New package every 10s
        if (Time.time - _lastGenTime > 10.0f)
        {
            _lastGenTime = Time.time;
            return new Package(2, _map.Locations.First());
        }
        return null;
    }
}
