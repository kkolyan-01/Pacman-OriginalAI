using UnityEngine;


public class Speedy : GhostBehavior
{
    public Speedy(Ghost ghost) : base(ghost)
    {
    }

    protected override void ChaseMove()
    {
        Vector3 cell = _pacman.position + _pacman.direction * 4;
        MoveToCell(cell);
    }
}