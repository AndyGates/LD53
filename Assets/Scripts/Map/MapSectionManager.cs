using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapSectionManager : MonoBehaviour
{
    [SerializeField]
    Tilemap _roads;

    [SerializeField]
    Tilemap _ground; 

    [SerializeField]
    MapSection[] _mapSections;

    [SerializeField]
    Map _mapService;

    public Tilemap Roads {get { return _roads; } }

    [SerializeField]
    float _newSectionDelay = 20.0f;

    float _startTime = 0;
    int _sectionCount = 0;

    void Awake()
    {
        _startTime = Time.time;
        AppendNextSection();
    }

    void Update()
    {
        if(_mapSections.Length > _sectionCount && Time.time - _startTime > (_sectionCount * _newSectionDelay))
        {
            AppendNextSection();
        }
    }

    void AppendNextSection()
    {
        Append(_mapSections[_sectionCount]);
        _sectionCount++;
    }

    void Append(MapSection section)
    {
        AppendTilemap(_roads, section.RoadTiles);
        AppendTilemap(_ground, section.GroundTiles);
        AppendLocations(section.Locations);
        _mapService.Refresh();

        //If we are not a prefab, disable any renderers. Maybe used for "ghost"
        if(section.gameObject.scene.rootCount != 0)
        {
            foreach(TilemapRenderer r in section.GetComponentsInChildren<TilemapRenderer>())
            {
                r.enabled = false;
            }
        }
    }

    void AppendTilemap(Tilemap target, Tilemap source)
    {
        for(int x = source.cellBounds.min.x; x < source.cellBounds.max.x; x++)
        {
            for(int y = source.cellBounds.min.y; y < source.cellBounds.max.y; y++)
            {
                Vector3Int pos = new Vector3Int(x,y,0);
                if(source.HasTile(pos)) {
                    target.SetTile(pos, source.GetTile(pos));
                    target.SetTransformMatrix(pos, source.GetTransformMatrix(pos));
                }
            }
        }  
    }

    void AppendLocations(Location[] locations)
    {
        foreach(Location loc in locations)
        {
            Location newLoc = GameObject.Instantiate<Location>(loc);
            newLoc.transform.parent = transform;

            _mapService.Locations.Add(newLoc);
        }
    }
}
