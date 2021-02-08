using UnityEngine;


public class Bashful : GhostBehavior
{
    public Bashful(Ghost ghost) : base(ghost)
    {
    }

    protected override void ChaseMove()
    {
        Ghost shadow = GameManager.singlton.shadow;
        Vector3 cellInFrontPacman = _pacman.position + _pacman.direction * 2;
        Vector3 shadowVector = cellInFrontPacman - shadow.position;
        Vector3 cell = shadow.position + shadowVector * 2;
        MoveToCell(cell);
    }
}