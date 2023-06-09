using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum Cell
{
    Impassable = 0,
    Road = 1,
    Location = 2,
}

public class Map : MonoBehaviour
{
    [SerializeField]
    List<Location> _locations = new List<Location>();

    [SerializeField]
    Vector2Int _depotLocation;

    [SerializeField]
    TextAsset _dataTable;

    [SerializeField]
    MapSectionManager _sectionManager;

    [SerializeField]
    float _scale = 2.5f;

    [SerializeField]
    bool _editing = false;

    Cell[][] Grid;

    Color[] CellColors = new Color[]{ Color.cyan, Color.magenta, Color.green };

    public List<Location> Locations{ get => _locations; }

    public Vector2Int DepotLocation { get => _depotLocation; }

    public float Scale { get => _scale; }

    void Start()
    {
        Load();
    }

    public void Refresh()
    {
        Load();
    }

    public Vector3 ToWorld(Vector2Int coord)
    {
        return new Vector3(coord.x, -coord.y) * _scale;
    }

    void OnDrawGizmos()
    {
        if (_dataTable == null)
        {
            return;
        }

        // Always load as we dont know if the asset changed
        if (_editing || Grid == null)
        {
            Load();
        }

        float padding = 0.01f;
        for(int row = 0; row < Grid.Length; row++) 
        {
            for(int col = 0; col < Grid[row].Length; col++) 
            {
                Gizmos.color = CellColors[(int)Grid[row][col]];
                List<Vector3> lines = new List<Vector3>();
                lines.Add(new Vector3(col + padding, -row - padding, 0.0f) * _scale);
                lines.Add(new Vector3(col + 1.0f - padding, -row - padding, 0.0f) * _scale);
                lines.Add(new Vector3(col + 1.0f - padding, -(row + 1.0f - padding), 0.0f) * _scale);
                lines.Add(new Vector3(col + padding, -(row + 1.0f - padding), 0.0f) * _scale);
                Gizmos.DrawLineStrip(new System.ReadOnlySpan<Vector3>(lines.ToArray()), true);
            }
        }
    }

    void Load()
    {
        if(_sectionManager != null)
        {
            LoadFromTilemap();
            return;
        }

        if (_dataTable == null)
        {
            return;
        }

        string[] rows = _dataTable.text.Split('\n');
        Grid = new Cell[rows.Length][];

        for(int row = 0; row < rows.Length; row++) 
        {
            string[] cells = rows[row].Split(',');
            Grid[row] = new Cell[cells.Length];
            for(int col = 0; col < cells.Length; col++) 
            {
                Grid[row][col] = (Cell)int.Parse(cells[col]);
            }
        }
    }

    void LoadFromTilemap()
    {
        if (_sectionManager == null)
        {
            return;
        }

        Tilemap source = _sectionManager.Roads;

        int xMin = source.cellBounds.min.x;
        int xMax = source.cellBounds.max.x;

        int yMin = source.cellBounds.min.y;
        int yMax = source.cellBounds.max.y;

        Grid = new Cell[yMax - yMin][];

        for(int y = yMin; y < yMax; y++)
        {

            //Inversing Y. I have descended into madness
            int yIndex = ((yMax-1)-yMin)-(y-yMin);

            Grid[yIndex] = new Cell[xMax - xMin];

            for(int x = xMin; x < xMax; x++)
            {
                Vector3Int pos = new Vector3Int(x,y,0);
                Grid[yIndex][x-xMin] = source.HasTile(pos) ? Cell.Road : Cell.Impassable;
            }
        }
    }

    public Route GenerateRoute(Vector2Int from, Vector2Int to)
    {
        /*float distance = Vector2Int.Distance(from, to);
        float time = distance;
        List<Vector2Int> path = new List<Vector2Int>();
        path.Add(to);
        return new Route(path, time, distance);*/

        Navigation nav = new Navigation(Grid);
        return nav.CalculateRoute(from, to);
    }

    public bool IsDepot(Vector2Int coord)
    {
        return coord == DepotLocation;
    }
}