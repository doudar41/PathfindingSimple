using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GridManager : MonoBehaviour
{
    [SerializeField] int gridSizeX = 1, gridSizeY = 1, cubeSize =1;
    [SerializeField] GridComponent cube;

    Dictionary<Vector2Int, Tile> map = new Dictionary<Vector2Int, Tile>();
    public Dictionary<Vector2Int, Tile> Map { get { return map; } }

    Dictionary<Vector2Int,GridComponent> mapCubes = new Dictionary<Vector2Int, GridComponent>();
    public Dictionary<Vector2Int, GridComponent> MapCubes { get { return mapCubes; } }

    GridComponent startPath = null, endPath = null;
    public Tile StartPath { get { return startPath._Tile; } }
    public Tile EndPath { get { return endPath._Tile; } }


    GridComponent[] cubes;

    private void Awake()
    {
        cubes = FindObjectsOfType<GridComponent>();
        if (cubes.Length != 0)
        {
            foreach (GridComponent c in cubes)
            {
                mapCubes.Add(c._Tile.coordinates, c);
            }
        }
        MakeMap();
    }



    public void SetStartAndDestinationPoints(GridComponent target)
    {
        if(startPath == null && endPath == null)
        {
            startPath = target;
            startPath.Changecolor(TileColors.startCol);
            Debug.Log("Starting point " + startPath._Tile.coordinates);
            return;
        }
        if (endPath == null && target !=startPath && startPath != null)
        {
            endPath = target;
            endPath.Changecolor(TileColors.endCol);
            Debug.Log("Endingpoint " + endPath._Tile.coordinates);
            return; 
        }
        if (startPath == target)
        {
            ResetDestinationPoints();
            return;
        }
        if (endPath == target)
        {
            
            endPath.Changecolor(TileColors.isWalkableCol);
            endPath = null;
            return;
        }

    }


    public void ResetDestinationPoints()
    {
        foreach (Vector2Int v in MapCubes.Keys)
        {
            if (MapCubes[v]._Tile.isWalkable)
            {
                MapCubes[v].Changecolor(TileColors.isWalkableCol);
            }
            else MapCubes[v].Changecolor(TileColors.isBlockedCol);
        }
        startPath.Changecolor(TileColors.isWalkableCol);
        startPath = null;
        endPath.Changecolor(TileColors.isWalkableCol);
        endPath = null;

    }

    void ReadExistingMap()
    {
        foreach (GridComponent gC in cubes)
        {
            map.Add(gC._Tile.coordinates, new Tile(gC._Tile.coordinates, gC._Tile.isWalkable));
        }
    }


    void MakeMap()
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                map.Add(new Vector2Int(x, y), new Tile(new Vector2Int(x, y), true));
                var cubeCreated = Instantiate(cube, new Vector3(x* cubeSize, 0, y* cubeSize), Quaternion.identity);
                cubeCreated.transform.parent = transform;
                cubeCreated.SetTileCoordinates(new Vector2Int(x, y));
                cubeCreated._Tile.isWalkable = true;
                cubeCreated.name = cubeCreated._Tile.coordinates.ToString();
                mapCubes.Add(cubeCreated._Tile.coordinates, cubeCreated);
            }
        }
    }
}
