using System;
using System.Linq;
using UnityEngine;

public class PackageStream : MonoBehaviour
{
    [SerializeField]
    Map _map;

    // TODO: Generations settings. linear speed ramp??
    [SerializeField]
    StreamConfig _settings = new StreamConfig(); // default generation settings

    double _lastGenTime = 0;

    void Awake()
    {
        _lastGenTime = Time.time;
    }

    public Package Next()
    {
        // Increasing rate lower would increase difficulty
        float _rateUpper = _settings.RateLower * _settings.RateUpper;
        var nextPackageSeconds = UnityEngine.Random.Range(_settings.RateLower, _settings.RateUpper);
        // New package every 10s
        if (Time.time - _lastGenTime > nextPackageSeconds)
        {
            _lastGenTime = Time.time;
            return RandomPackage();
        }
        return null;
    }

    private Package RandomPackage()
    {
        int randomIndex = UnityEngine.Random.Range(0, _settings.PostageTypes.Count);
        PostageType deliveryType = _settings.PostageTypes[randomIndex];

        int randomLocationIndex = UnityEngine.Random.Range(0, _map.Locations.Count());
        Location Target = _map.Locations.ElementAt(randomLocationIndex);

        int randomSize = UnityEngine.Random.Range(1, _settings.MaxSize);
        int randomValue = UnityEngine.Random.Range(_settings.MinValue, _settings.MaxValue); // value of package contents

        return new Package(randomSize, randomValue, Target, deliveryType);
    }
}
