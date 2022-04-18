using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{

    [SerializeField]Vector2Int startCoordinates, destinationCoordinates, currentSearchTile;

    Queue<Vector2Int> tileCheckedAndQueued  = new Queue<Vector2Int>();
    Dictionary<Vector2Int, Tile> map = new Dictionary<Vector2Int, Tile>();

    Vector2Int[] neighbours = new Vector2Int[] { Vector2Int.up,Vector2Int.right,Vector2Int.down,Vector2Int.left};
    GridManager gridManager;


    void Start()
    {
        gridManager = FindObjectOfType<GridManager>();
        map = gridManager.Map;
    }


    void SearchNeighbourTiles(Vector2Int cubeCoordinates)
    {
        List<Vector2Int> neighbourCubes = new List<Vector2Int>();
       
        foreach(Vector2Int n in neighbours)
        {
            Vector2Int checkingCoordinates = cubeCoordinates + n;

            if (map.ContainsKey(checkingCoordinates)&& checkingCoordinates != startCoordinates)
            {
                if (map[checkingCoordinates].isWalkable)
                {
                    neighbourCubes.Add(checkingCoordinates);
                }
            }
        }
        
        foreach (Vector2Int t in neighbourCubes)
        {
            if (map[t].connectedTo == null)
            {
                map[t].connectedTo = map[cubeCoordinates];
                tileCheckedAndQueued.Enqueue(t);
                gridManager.MapCubes[cubeCoordinates].Changecolor(TileColors.isExploredCol);
            }
        }
    }

    public void PathfindingThroughMap()
    {
        startCoordinates =  gridManager.StartPath.coordinates;
        destinationCoordinates = gridManager.EndPath.coordinates;
        ResetMap();
        if(!map[destinationCoordinates].isWalkable || !map[startCoordinates].isWalkable)
        {
            Debug.Log("Start or destination tiles is not walkable");
            return;

        }
        bool isRunning = true;
        tileCheckedAndQueued.Enqueue(startCoordinates);
        while (tileCheckedAndQueued.Count > 0 && isRunning)
        {
            currentSearchTile = tileCheckedAndQueued.Dequeue();
            SearchNeighbourTiles(currentSearchTile);
            if (currentSearchTile == destinationCoordinates)
            {
                isRunning = false;
            }
        }
       BuildPath();
    }

   List<Vector2Int> BuildPath()
    {
        List<Vector2Int> path = new List<Vector2Int>();
        currentSearchTile = destinationCoordinates;
        map[currentSearchTile].isPath = true;
        gridManager.MapCubes[currentSearchTile].Changecolor(TileColors.endCol);
        path.Add(currentSearchTile);

        while (currentSearchTile != startCoordinates)
        {
            currentSearchTile = map[currentSearchTile].connectedTo.coordinates;
            map[currentSearchTile].isPath = true;
            gridManager.MapCubes[currentSearchTile].Changecolor(TileColors.isPathCol);
            path.Add(currentSearchTile);
        }
        gridManager.MapCubes[currentSearchTile].Changecolor(TileColors.startCol);
        path.Reverse();
        return path;
    }


    void ResetMap()
    {
        foreach(Vector2Int v in map.Keys)
        {
            map[v].connectedTo = null;
        }
        foreach(Vector2Int v in gridManager.MapCubes.Keys)
        {
            if (gridManager.MapCubes[v]._Tile.isWalkable)
            {
                gridManager.MapCubes[v].Changecolor(TileColors.isWalkableCol);
            }
            else gridManager.MapCubes[v].Changecolor(TileColors.isBlockedCol);
        }
        tileCheckedAndQueued.Clear();
    }

}
