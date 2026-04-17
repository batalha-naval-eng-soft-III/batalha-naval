using UnityEngine;
using CodeMonkey.Utils;
using System.Collections.Generic;
public class Grid
{
    public int width, height;
    public float cellSize;
    public int[,] gridArray;
    public Vector3 originPosition;
    public TextMesh[,] debugTextArray;
    public Grid(int width, int height, float cellSize)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        gridArray = new int[width, height];
        debugTextArray = new TextMesh[width, height];
        for(int x = 0; x < gridArray.GetLength(0); x++)
        {
            for(int y = 0; y < gridArray.GetLength(1); y++)
            {
                gridArray[x, y] = 0;
                debugTextArray[x, y] = UtilsClass.CreateWorldText(gridArray[x, y].ToString(), null, GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * .5f, 20, Color.white, TextAnchor.MiddleCenter);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.red, 100f);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.red, 100f);
            }
        }
        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.red, 100f);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.red, 100f);
    }
    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize + originPosition;
    }
    public void SetValue(int x, int y, int value)
    {
        if(x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y] = value;
            debugTextArray[x, y].text = gridArray[x, y].ToString();
        }
    }

    public void SetValue(Vector3 worldPosition, int value)
    {
        (int x, int y) = GetXY(worldPosition);
        SetValue(x, y, value);
    }
    public (int x, int y) GetXY(Vector3 worldPosition)
    {
        int x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        int y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
        return (x, y);
    }
    public int GetValue(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        }
        else
        {
            return 0;
        }
    }
    public int GetValue(Vector3 worldPosition)
    {
        (int x, int y) = GetXY(worldPosition);
        return GetValue(x, y);
    }
    public bool CanPlaceShip(ShipSO ship, Vector3 worldPosition, ShipSO.Dir dir)
    {
        (int startX, int startY) = GetXY(worldPosition);
        Vector2Int startGridPos = new Vector2Int(startX, startY);

        List<Vector2Int> shipPositions = ship.GetGridPositionList(startGridPos, dir);

        foreach (Vector2Int pos in shipPositions)
        {
            if (pos.x < 0 || pos.y < 0 || pos.x >= width || pos.y >= height)
            {
                return false;
            }

            if (GetValue(pos.x, pos.y) != 0)
            {
                return false;
            }
        }

        return true;
    }
    public void PlaceShip(List<Vector2Int> shipPositions, int shipID)
    {
        foreach (Vector2Int pos in shipPositions)
        {
            SetValue(pos.x, pos.y, shipID);
        }
    }
}
