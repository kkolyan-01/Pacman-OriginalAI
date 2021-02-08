using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    private Vector2 _direction;
    private float _delayMove;
    private float _lastMoveStep;
    public float multiplySpeed
    {
        set
        {
            float baseDelayMove = GameManager.singlton.baseDelayMove;
            if(baseDelayMove == 0) Debug.LogError("Base Delay Move equals zero!");
            _delayMove = baseDelayMove / value;
        }
    }
    
    protected bool canMove => Map.CanMoveToNextCell(position, direction) && Time.time > _lastMoveStep + _delayMove;
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
        
        Vector2 positionTemp = position;
        positionTemp += _direction * moveStep;
        positionTemp.x = (float) Math.Round(positionTemp.x, 1);
        positionTemp.y = (float) Math.Round(positionTemp.y, 1);
        position = positionTemp;
        _lastMoveStep = Time.time;
        
        return true;
    }
}
