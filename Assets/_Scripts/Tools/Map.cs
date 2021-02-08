using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Map : MonoBehaviour
{
    public static Map singlton;
    
    [SerializeField] private Vector2 _rangeX;
    [SerializeField] private Transform[] _advantageCells;
    private Vector2[] _cashAdvantageCells;
    private static Vector3 _tileAnchor;
    private static Tilemap _wallsTiles;
    private static Vector3[] _fourDirections;
    
    public Vector2 rangeX => _rangeX;

    
    private void Awake()
    {
        if (singlton == null) singlton = this;
        else
        {
            Debug.LogError("Попытка создать второй GameManager!");
        }
        
        _wallsTiles = GetComponent<Tilemap>();
        _tileAnchor = _wallsTiles.tileAnchor;
        _fourDirections = new[]
        {
            Vector3.up,
            Vector3.right,
            Vector3.down,
            Vector3.left,
        };
    }

    public Vector2[] GetAdvantageCells()
    {
        if (_cashAdvantageCells == null)
        {
            CacheAdvantageCells();
        }

        return _cashAdvantageCells;
    }

    private void CacheAdvantageCells()
    {
        _cashAdvantageCells = new Vector2[_advantageCells.Length];
        for (int i = 0; i < _advantageCells.Length; i++)
        {
            _cashAdvantageCells[i] = _advantageCells[i].position;
        }
    }
    
    public static bool IsInCenterCellPosition(Vector3 position)
    {
        float absPostionX = Mathf.Abs(position.x);
        float absPostionY = Mathf.Abs(position.y);
        return absPostionX % _wallsTiles.cellSize.x == _tileAnchor.x % _wallsTiles.cellSize.x 
               && 
               absPostionY % _wallsTiles.cellSize.y == _tileAnchor.y % _wallsTiles.cellSize.y;
        
    }

    public static bool IsPositionAtCrossroad(Vector3 position)
    {
        if (!IsInCenterCellPosition(position))
            return false;
        
        int countWays = 0;
        foreach (var direction in _fourDirections)
        {
            if (CanMoveToNextCell(position, direction))
                countWays++;
        }

        return countWays > 1;
    }

    public static List<Vector2> GetPossibleDirections(Vector3 position)
    {
        List<Vector2> possibleDirections = new List<Vector2>();

        foreach (var direction in _fourDirections)
        {
            if (CanMoveToNextCell(position, direction))
                possibleDirections.Add(direction);
        }
        
        return possibleDirections;
    }
    
    public static bool CanMoveToNextCell(Vector3 position,Vector3 direction)
    {
        return !_wallsTiles.HasTile(NextTilePosition(position, direction));
    }
    private static Vector3Int NextTilePosition(Vector3 position,Vector3 direction)
    {
        Vector3Int nextTilePosition = Vector3Int.FloorToInt(direction * 0.51f + position);
        return nextTilePosition;
    }
}
