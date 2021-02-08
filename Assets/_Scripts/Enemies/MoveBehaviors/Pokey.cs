using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pokey : GhostBehavior
{
    public Pokey(Ghost ghost) : base(ghost)
    {
    }

    protected override void ChaseMove()
    {
        float distanceToPacman = Vector3.Magnitude(_pacman.position - _ghost.position);
        
        if (distanceToPacman > 8)
        {
            ScatterMove();
        }
        else
        {
            MoveToCell(_pacman.position);
        }

    }
}
