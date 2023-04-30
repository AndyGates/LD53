using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FadeTiles : MonoBehaviour
{
    [SerializeField] private float FadeSpeed = 0.5f;

    private float totalDeltaTime;
    private bool isFading = false;
    private Color currentColor;
    private Vector3Int[] tilesToFade;
    private Tilemap tileMap;


    [SerializeField]
    Tilemap _tile;

    void Awake()
    {
        List<Vector3Int> locs = new List<Vector3Int>();

        for(int x = _tile.cellBounds.min.x; x < _tile.cellBounds.max.x; x++)
        {
            for(int y = _tile.cellBounds.min.y; y < _tile.cellBounds.max.y; y++)
            {
                for(int z = _tile.cellBounds.min.z; z < _tile.cellBounds.max.z; z++)
                {
                    Vector3Int pos = new Vector3Int(x,y,z);
                    if(_tile.HasTile(pos)) locs.Add(pos);
                }
            }
        }  
 
        BeginTileFade(_tile, locs.ToArray());
    }

    public void BeginTileFade(Tilemap _tileMap, Vector3Int[] _tilesToFade)
    {
        if (isFading)
            return;

        totalDeltaTime = 0;
        tileMap = _tileMap;
        tilesToFade = _tilesToFade;
        currentColor = Color.clear;

        isFading = true;
    }

    private void Update()
    {
        if (isFading && tilesToFade != null)
        {
            totalDeltaTime += Time.deltaTime;

            float step = totalDeltaTime / FadeSpeed;
            currentColor = Color.Lerp(Color.clear, Color.white, step);

            if (step >= 1.0f)
            {
                currentColor = Color.white;
                isFading = false;
            }

            for (int i = 0; i < tilesToFade.Length; i++)
            {
                Debug.Log($"Set color for {tilesToFade[i]} : {currentColor}");
                tileMap.SetTileFlags(tilesToFade[i], TileFlags.None);
                tileMap.SetColor(tilesToFade[i], currentColor);

            }
        }
    }
}