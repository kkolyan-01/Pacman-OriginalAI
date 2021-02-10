using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor.Animations;
using UnityEngine;

public class GhostLeaveHomeController
{

    private Ghost _ghost;
    private Animator _animator;
    private bool _isHome;

    private bool leavedHome => _animator.GetCurrentAnimatorStateInfo(0).IsName("Finish");

    private bool canLeaveHove
    {
        get
        {
            float pillets = GameManager.singlton.eatenPillets;
            return pillets >= _ghost.peletsForLeave && GameManager.singlton.timerTime > _ghost.minTimeForLeave;
        }
    }
    
    public GhostLeaveHomeController(Ghost ghost)
    {
        _ghost = ghost;
        _animator = ghost.GetComponent<Animator>();
        ResetGhost();
    }
    
    public void ResetGhost()
    {
        _isHome = true;
        _ghost.isHome = true;
    }
    
    private void LeaveHome()
    {
        _animator.SetTrigger("Leave");
    }
    
    public void TryLeaveHome()
    {
        if(!_isHome) return;
        
        if (canLeaveHove)
        {
            LeaveHome();
        }
        
        if (leavedHome)
        {
            _ghost.isHome = false;
            _isHome = false;
        }
    }
}
