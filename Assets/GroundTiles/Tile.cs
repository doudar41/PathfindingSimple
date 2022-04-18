using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Tile 
{

    public Vector2Int coordinates;
    public bool isWalkable = true;
    public bool isExplored = false;
    public bool isPath = false;
    public Tile connectedTo =null;



    public Tile(Vector2Int coordinates, bool isWalkable)
    {
        this.coordinates = coordinates;
        this.isWalkable = isWalkable;
    }


}
