using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapSection : MonoBehaviour
{
    [SerializeField]
    Tilemap _roadTiles;

    [SerializeField]
    Tilemap _groundTiles;

    public Location[] Locations { get { return GetComponentsInChildren<Location>(); } }

    public Tilemap RoadTiles {get { return _roadTiles; } }
    public Tilemap GroundTiles {get { return _groundTiles; } }
}
