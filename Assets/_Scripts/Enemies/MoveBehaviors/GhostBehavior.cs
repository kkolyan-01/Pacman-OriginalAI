using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class GhostBehavior
{
    protected Ghost _ghost;
    protected Pacman _pacman;
    private bool _changedDirection;
    private GhostState _lastGhostState;
    
    private bool canChangeDirection => _ghost.isInCrossroadPosition && !_changedDirection;
    
    public GhostBehavior(Ghost ghost)
    {
        _ghost = ghost;
        _pacman = GameManager.singlton.pacman;
        _lastGhostState = _ghost.state;
    }

    public void Move()
    {
        switch (_ghost.state)
        {
            case GhostState.chase:
                ChaseMove();
                break;
            case GhostState.scatter:
                ScatterMove();
                break;
            case GhostState.frightened:
                FrightenedMove();
                break;
        }
        
        UpdateGhostState();
    }
    
    protected abstract void ChaseMove();
    
    protected void ScatterMove()
    {
        ChangeDirectionToCell(_ghost.corner);
        GhostMoveOneStep();
    }

    protected void FrightenedMove()
    {
        ChoiseRandomDirection();
        GhostMoveOneStep();
    }

    protected void MoveToCell(Vector2 cell)
    {
        ChangeDirectionToCell(cell);
        GhostMoveOneStep();
    }

    private void GhostMoveOneStep()
    {
        if(_ghost.MoveOneStep())
            _changedDirection = false;
    }
    
    private void UpdateGhostState()
    {
        GhostState newState = _ghost.state;
        if (_lastGhostState != newState && _lastGhostState != GhostState.frightened)
        {
            ChangeDirectionToOpposite();
        }
        _lastGhostState = newState;
    }

    private void ChangeDirectionToOpposite()
    {
        _ghost.direction *= -1;
    }

    private void ChangeDirectionToCell(Vector3 target)
    {
        if(!canChangeDirection) return;

        _ghost.goalCell = target;
        _ghost.direction = ChoiseDirectionToCell(target);
        _changedDirection = true;
    }

    private Vector2 ChoiseDirectionToCell(Vector3 target)
    {
        Vector2 bestDirection = _ghost.direction;
        float bestDistance = 0;
        List<Vector2> possibleDirections = GetPossibleDirections();
        
        foreach (var possibleDirection in possibleDirections)
        {
            Vector3 cellPosition = _ghost.position + (Vector3)possibleDirection;
            float distanceFromCellToTarget = Vector3.Magnitude(target - cellPosition);
            
            if (distanceFromCellToTarget < bestDistance || bestDistance == 0)
            {
                bestDistance = distanceFromCellToTarget;
                bestDirection = possibleDirection;
            }
        }

        return bestDirection;
    }
    
    
    
    private List<Vector2> GetPossibleDirections()
    {
        Vector2 oppositeDirection = _ghost.direction * -1;
        List<Vector2> possibleDirections = Map.GetPossibleDirections(_ghost.transform.position);
        possibleDirections.Remove(oppositeDirection);

        if (_ghost.isInAtadvantageCellPosition)
        {
            possibleDirections.Remove(Vector2.up);
        }
        
        return possibleDirections;
    }
    
    public void ChoiseRandomDirection()
    {
        if(!canChangeDirection) return;
        
        List<Vector2> possibleDirections = GetPossibleDirections();
        int randomIndex = Random.Range(0, possibleDirections.Count);
        _ghost.direction = possibleDirections[randomIndex];
        _changedDirection = true;
    }
    
}
