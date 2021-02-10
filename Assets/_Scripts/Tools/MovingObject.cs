using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    private Vector2 _direction;
    private float _lastMoveStep;
    protected Vector3 _targetCell;
    protected float multiplySpeed { set; get; }

    protected bool canMove => Map.CanMoveToNextCell(position, direction);
    public bool isInCenterCell => Map.IsInCenterCellPosition(position);
    public Vector3 direction
    {
        get => _direction;
        set => _direction = value;
    }
    public Vector3Int positionInt => Vector3Int.FloorToInt(position);
    public Vector3 position
    {
        set
        {
            Vector3 newPosition = value;
            transform.position = newPosition;
            CheckOutOfBounds();
        }
        get => transform.position;
    }

    protected void CheckOutOfBounds()
    {
        Vector2 rangeX = Map.singlton.rangeX;
        bool isOffScreen = position.x <= rangeX.x || position.x >= rangeX.y;
            
        if (isOffScreen)
        {
            ReturnToScreen();
        }
    }

    private void ReturnToScreen()
    {
        bool left = position.x <= Map.singlton.rangeX.x;
        bool right = position.x >= Map.singlton.rangeX.y;
        Vector3 newPosition = position;
        
        if (left)
        {
            newPosition.x = Map.singlton.rangeX.y;
        }

        if (right)
        {
            newPosition.x = Map.singlton.rangeX.x;
        }
        
        transform.position = newPosition;
    }
    
    public bool MoveOneStep()
    {
        if(!canMove) return false;
        
        float moveStep = GameManager.singlton.moveStep;
        
        if (isInCenterCell || _targetCell == Vector3.zero)
        {
            _targetCell = position + (Vector3)_direction;
            _targetCell = Vector3Int.FloorToInt(_targetCell);
            _targetCell = _targetCell + new Vector3(0.5f, 0.5f, 0);
        }
        
        Vector3 newPosition = position;

        newPosition.x = Mathf.MoveTowards(newPosition.x, _targetCell.x, moveStep * multiplySpeed);
        newPosition.y = Mathf.MoveTowards(newPosition.y, _targetCell.y, moveStep * multiplySpeed);
        newPosition.x = (float) Math.Round(newPosition.x, 3);
        newPosition.y = (float) Math.Round(newPosition.y, 3);
        position = newPosition;
        
        return true;
    }
}
