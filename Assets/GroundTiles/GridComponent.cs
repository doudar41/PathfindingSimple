using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;


public enum TileColors
{
    defaultCol,
    isWalkableCol,
    isExploredCol,
    isPathCol,
    isBlockedCol,
    startCol,
    endCol
}

public class GridComponent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{

    [SerializeField]Tile tile;
    [SerializeField] TextMeshPro coordText;
    [SerializeField] Color defaultColor, isWalkableColor, isExploredColor, isPathColor, isBlockedColor, startColor, endColor;
    [SerializeField] MeshRenderer tileMatContainer;    
    public Tile _Tile {get { return tile; }}


    Color currentColor;
    bool flipFlop = true;

    GridManager gridManager;
    


    void Awake()
    {
        Changecolor(TileColors.isWalkableCol);
    }

    private void Start()
    {
        gridManager = FindObjectOfType<GridManager>();
    }


    public void Changecolor(TileColors tileColors)
    {
        switch (tileColors)
        {
            case TileColors.defaultCol:
                tileMatContainer.material.color = defaultColor;
                currentColor = defaultColor;
                break;
            case TileColors.isWalkableCol:
                tileMatContainer.material.color = isWalkableColor;
                currentColor = isWalkableColor;
                break;
            case TileColors.isExploredCol:
                tileMatContainer.material.color = isExploredColor;
                currentColor = isExploredColor;
                break;
            case TileColors.isPathCol:
                tileMatContainer.material.color = isPathColor;
                currentColor = isPathColor;
                break;
            case TileColors.isBlockedCol:
                tileMatContainer.material.color = isBlockedColor;
                currentColor = isBlockedColor;
                break;
            case TileColors.startCol:
                tileMatContainer.material.color = startColor;
                currentColor = startColor;
                break;
            case TileColors.endCol:
                tileMatContainer.material.color = endColor;
                currentColor = endColor;
                break;
        }
    }

    GridComponent GetGridComponent(Tile _tile)
    {
        if (_tile == tile)
        {
            return this;
        }
        else 
        return null;
    }

    public void SetTileCoordinates(Vector2Int coord)
    {
        tile.coordinates = coord;
        coordText.text = tile.coordinates.ToString();
    }


    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        currentColor = tileMatContainer.material.color;
        tileMatContainer.material.color = Color.white;
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        tileMatContainer.material.color = currentColor;
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (pointerEventData.button == PointerEventData.InputButton.Left)
        {
            flipFlop = !flipFlop;
            if (!flipFlop)
            {
                if (gridManager.StartPath == this._Tile || gridManager.EndPath == this._Tile)
                {
                    gridManager.ResetDestinationPoints();
                }
                tile.isWalkable = false;
                gridManager.Map[tile.coordinates].isWalkable = false;
                Changecolor(TileColors.isBlockedCol);
            }
            else
            {
                tile.isWalkable = true;
                gridManager.Map[tile.coordinates].isWalkable = true;
                Changecolor(TileColors.isWalkableCol);
            }
            return;
        }
        if (pointerEventData.button == PointerEventData.InputButton.Right)
        {
            Debug.Log("RightClick");
            gridManager.SetStartAndDestinationPoints(this);
        }
    }


}
