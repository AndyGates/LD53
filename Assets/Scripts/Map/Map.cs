using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum Cell
{
    Impassable = 0,
    Road = 1,
    Location = 2,
}

public class Map : MonoBehaviour
{
    [SerializeField]
    TextAsset _dataTable;

    Cell[][] Grid;

    Color[] CellColors = new Color[]{ Color.cyan, Color.magenta, Color.green };

    void Start()
    {
        Load();
    }

    Vector3 GetWorldCoord(int row, int col)
    {
        return new Vector3(row, -col);
    }

    void OnDrawGizmos()
    {
        if (_dataTable == null)
        {
            return;
        }

        // Always load as we dont know if the asset changed
        Load();     

        float scale = 1.0f;
        float padding = 0.01f;
        for(int row = 0; row < Grid.Length; row++) 
        {
            for(int col = 0; col < Grid[row].Length; col++) 
            {
                Gizmos.color = CellColors[(int)Grid[row][col]];
                List<Vector3> lines = new List<Vector3>();
                lines.Add(new Vector3(col + padding, -row - padding, 0.0f) * scale);
                lines.Add(new Vector3(col + 1.0f - padding, -row - padding, 0.0f) * scale);
                lines.Add(new Vector3(col + 1.0f - padding, -(row + 1.0f - padding), 0.0f) * scale);
                lines.Add(new Vector3(col + padding, -(row + 1.0f - padding), 0.0f) * scale);
                Gizmos.DrawLineStrip(new System.ReadOnlySpan<Vector3>(lines.ToArray()), true);
            }
        }
    }

    void Load()
    {
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

        Debug.Log($"Map loaded. {Grid.Length}x{Grid.First().Length}");
    }

    IEnumerable<Vector2Int> GeneratePath(Vector2Int from, Vector2Int to)
    {
        return null;
    }
}