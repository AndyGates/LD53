using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class Navigation
{
    public static readonly Vector2Int[] DIRS = new []
        {
            new Vector2Int(1, 0),
            new Vector2Int(0, -1),
            new Vector2Int(-1, 0),
            new Vector2Int(0, 1)
        };

    PriorityQueue<Vector2Int, double> _openSet = new PriorityQueue<Vector2Int, double>();
    // Cost of the cheapest path from start to n currently known
    Dictionary<Vector2Int, Vector2Int> _cameFrom = new Dictionary<Vector2Int, Vector2Int>();

    // For node n, fScore[n] := gScore[n] + h(n). fScore[n] represents our current best guess as to
    // how cheap a path could be from start to finish if it goes through n.
    Dictionary<Vector2Int, double> _costSoFar = new Dictionary<Vector2Int, double>();

    Cell[][] _grid;
    int _width = 0;
    int _height = 0;

    public Navigation(Cell[][] grid)
    {
        _grid = grid;
        _width = grid.Length;
        _height = grid.First().Length;
    }

    public Route CalculateRoute(Vector2Int start, Vector2Int goal)
    {
        // TODO: Reset/clear all things

        // The set of discovered nodes that may need to be (re-)expanded.
        // Initially, only the start node is known.
        _openSet.Enqueue(start, 0);

        _cameFrom[start] = start;
        _costSoFar[start] = 0;

        while(_openSet.Count > 0)
        {
            var current = _openSet.Dequeue();

            if (current == goal)
            {
                var path = _cameFrom.Select(c => c.Value).ToList();
                path.Add(goal);
                int distance = path.Count();

                return new Route(
                    path,
                    distance * 0.5f,
                    distance
                );
            }

            foreach (Vector2Int next in GetNeighbors(current))
            {
                double newCost = _costSoFar[current] + Cost(next);
                if (_costSoFar.ContainsKey(next) == false || newCost < _costSoFar[next])
                {
                    _costSoFar[next] = newCost;
                    double priority = newCost + Heuristic(next, goal);
                    _openSet.Enqueue(next, priority);
                    _cameFrom[next] = current;
                }
            }
        }

        return null;
    }

    // h is the heuristic function. h(n) estimates the cost to reach goal from node n.
    static public double Heuristic(Vector2Int a, Vector2Int b)
    {
        return Vector2Int.Distance(a, b);
    }

    IEnumerable<Vector2Int> GetNeighbors(Vector2Int id)
    {
        foreach (var dir in DIRS) {
            Vector2Int next = new Vector2Int(id.x + dir.x, id.y + dir.y);
            if (InBounds(next) && Passable(next)) {
                yield return next;
            }
        }
    }

    double Cost(Vector2Int to)
    {
        return 0;//(double)_grid[to.y][to.x];
    }

    bool InBounds(Vector2Int id)
    {
        return 0 <= id.x && id.x < _width
            && 0 <= id.y && id.y < _height;
    }

    bool Passable(Vector2Int id)
    {
        return _grid[id.y][id.x] == Cell.Road;
    }

}
