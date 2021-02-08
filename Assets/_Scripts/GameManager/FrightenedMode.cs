using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrightenedMode
{
    public bool isActive { get; set; }
    private GhostManager _ghostManager;
    private GameTimer _timer;
    

    public FrightenedMode(GhostManager ghostManager, GameTimer timer)
    {
        _ghostManager = ghostManager;
        _timer = timer;
    }
    
    
    
    public void Run()
    {
        _ghostManager.FrightenGhosts();
        _timer.Pause();
        isActive = true;
    }

    public void Stop()
    {
        _ghostManager.DisableFrighten();
        _timer.Run();
        isActive = false;
    }
}
