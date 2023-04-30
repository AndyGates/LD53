using System;
using System.Linq;
using UnityEngine;

public class PackageStream : MonoBehaviour
{
    [SerializeField]
    Map _map;

    [SerializeField]
    GameManager _gameManager;

    // TODO: Generations settings. linear speed ramp??
    [SerializeField]
    StreamConfig _settings = new StreamConfig(); // default generation settings

    public int NextPackageSeconds = 0;
    double _lastGenTime = 0;

    private int PreviousSize;

    void Awake()
    {
        _lastGenTime = Time.time;
    }

    public Package Next()
    {      
        if (Time.time - _lastGenTime > NextPackageSeconds)
        {
            // Increasing rate lower would increase difficulty
            float _rateUpper = _settings.RateLower * _settings.RateUpper;
            NextPackageSeconds = UnityEngine.Random.Range(_settings.RateLower, _settings.RateUpper);

            _lastGenTime = Time.time;
            UpdateElapsedTime();
            return RandomPackage();
        }
        return null;
    }

    private void UpdateElapsedTime()
    {
        if (_gameManager?.State != null)
        {
            _gameManager.State.ElapsedSeconds += _lastGenTime;
        }
    } 

    private Package RandomPackage()
    {
        float deliveryTime = UnityEngine.Random.Range(_settings.ShortestTime, _settings.LongestTime);

        int randomIndex = UnityEngine.Random.Range(0, _settings.PostageTypes.Count);
        PostageType deliveryType = StreamConfig.GetDefaultTypes()[randomIndex];

        int randomLocationIndex = UnityEngine.Random.Range(0, _map.Locations.Count());
        Location Target = _map.Locations.ElementAt(randomLocationIndex);
        
        int randomSize = 0;
        do
        {
            randomSize = UnityEngine.Random.Range(1, _settings.MaxSize + 1);
        } while (PreviousSize == randomSize);

        PreviousSize = randomSize;

        int randomValue = UnityEngine.Random.Range(_settings.MinValue, _settings.MaxValue); // value of package contents

        return new Package(deliveryTime, randomSize, randomValue, Target, deliveryType);
    }
}
