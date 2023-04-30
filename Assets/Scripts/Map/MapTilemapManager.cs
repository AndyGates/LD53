using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapTilemapManager : MonoBehaviour
{
    [SerializeField]
    Tilemap _roads;

    [SerializeField]
    Tilemap _ground; 

    [SerializeField]
    MapSection[] _testSections;

    [SerializeField]
    Map _mapService;

    void Awake()
    {
        foreach(MapSection ms in _testSections)
        {
            Append(ms);
        }
    }

    void Append(MapSection section)
    {
        AppendTilemap(_roads, section.RoadTiles);
        AppendTilemap(_ground, section.GroundTiles);
        AppendLocations(section.Locations);
    }

    void AppendTilemap(Tilemap target, Tilemap source)
    {
        for(int x = source.cellBounds.min.x; x < source.cellBounds.max.x; x++)
        {
            for(int y = source.cellBounds.min.y; y < source.cellBounds.max.y; y++)
            {
                for(int z = source.cellBounds.min.z; z < source.cellBounds.max.z; z++)
                {
                    Vector3Int pos = new Vector3Int(x,y,z);
                    if(source.HasTile(pos)) {
                        target.SetTile(pos, source.GetTile(pos));
                        target.SetTransformMatrix(pos, source.GetTransformMatrix(pos));
                    }
                }
            }
        }  
    }

    void AppendLocations(Location[] locations)
    {
        foreach(Location loc in locations)
        {
            Location newLoc = GameObject.Instantiate<Location>(loc);
            loc.transform.parent = transform;

            _mapService.Locations.Add(newLoc);
        }
    }
}
