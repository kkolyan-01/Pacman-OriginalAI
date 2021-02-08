using System.Collections.Generic;
using UnityEngine;

public class GhostManager
{
    private List<Ghost> _ghosts;
    private GameObject[] _ghostPrefabs;
    private Vector3 _startGhostPosition;

    public GhostManager(List<Ghost> ghosts)
    {
        _ghosts = ghosts;
    }

    public void StopGhosts()
    {
        foreach (var ghost in _ghosts)
        {
            ghost.Stop();
        }
    }

    public void FrightenGhosts()
    {
        foreach (var ghost in _ghosts)
        {
            ghost.Frighten();
        }
    }

    public void DisableFrighten()
    {
        foreach (var ghost in _ghosts)
        {
            ghost.DisableFrighten();
        }
    }

    public void ResetGhosts()
    {
        foreach (var ghost in _ghosts)
        {
            ghost.ResetGhost();
        }
    }
}