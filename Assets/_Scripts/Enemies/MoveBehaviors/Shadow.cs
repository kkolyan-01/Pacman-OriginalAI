using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : GhostBehavior
{
    public Shadow(Ghost ghost) : base(ghost)
    {
    }

    protected override void ChaseMove()
    {
        MoveToCell(_pacman.position);
    }
}
