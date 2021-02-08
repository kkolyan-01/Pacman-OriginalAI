using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GhostType
{
    shadow,
    speedy,
    bashful,
    pokey,
}

public static class GhostBehaviorFactory
{
    public static GhostBehavior Create(GhostType type, Ghost ghost)
    {
        GhostBehavior ghostBehavior = null;
        
        switch (type)
        {
            case GhostType.shadow:
                ghostBehavior = new Shadow(ghost);
                break;
            case GhostType.speedy:
                ghostBehavior = new Speedy(ghost);
                break;
            case GhostType.bashful:
                ghostBehavior = new Bashful(ghost);
                break;
            case GhostType.pokey:
                ghostBehavior = new Pokey(ghost);
                break;
        }

        return ghostBehavior;
    }
}
