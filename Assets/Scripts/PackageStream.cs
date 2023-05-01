using System;
using System.Collections.Generic;
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

    float _packetDispatchTime = 0; // How many seconds to wait before sending a package
    float _lastGenTime = 0;
    bool _firstPackage = true;
    int PackageCount = 0;

    private List<int> PreviousSizes = new List<int>();
    private bool HadBigPackage = false;

    public float NextPackageProgress { get { return _packetDispatchTime == 0.0f ? 1.0f : (Time.time - _lastGenTime) / _packetDispatchTime; } }

    void Awake()
    {
        _packetDispatchTime = _settings.InitialPacketDispatchTime;
    }

    void Update()
    {
        _packetDispatchTime -= _settings.PacketDispatchTimeScaler * Time.deltaTime;
        _packetDispatchTime = Mathf.Max(_packetDispatchTime, _settings.MinPacketDispatchTime);
    }

    public Package Next()
    {      
        if (Time.time - _lastGenTime > _packetDispatchTime || _firstPackage)
        {
            _firstPackage = false;
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
        bool small;
        // A big package must be given for every 5 packages
        if (PreviousSizes.Count == 5 && !HadBigPackage)
        {
            small = false;
            HadBigPackage = false;
            PreviousSizes.Clear();
        }
        // Not at end of 5-package cycle. Randomly decide if next should be large 
        else
        {
            if (!HadBigPackage)
            {
                small = System.Convert.ToBoolean(UnityEngine.Random.Range(0, 1)); // 50% changce
                HadBigPackage = true ? !small : false;
                randomSize = GetRandomSize(false);

            }
            else
            {
                small = true;
            }
            randomSize = GetRandomSize(small);
        }

        PreviousSizes.Add(randomSize);



        int randomValue = UnityEngine.Random.Range(_settings.MinValue, _settings.MaxValue); // value of package contents

        return new Package(deliveryTime, randomSize, randomValue, Target, deliveryType);
    }

    private int GetRandomSize(bool smallPackage)
    {
        int prev = 0;
        if (PreviousSizes.Count > 0)
        {
            prev = PreviousSizes.Last<int>();
        }
        int sizeLimit = 0;
        int sizeFloor = 0;
        if (smallPackage)
        {
            sizeLimit = 3; // Max size 2
            sizeFloor = 1;
        } else

        {
            sizeFloor = 3;
            sizeLimit = _settings.MaxSize + 1; 
        }

        int randomSize = 0;
        do
        {
            randomSize = UnityEngine.Random.Range(sizeFloor, sizeLimit);
        } while (prev == randomSize);
        return randomSize;
    }
}
