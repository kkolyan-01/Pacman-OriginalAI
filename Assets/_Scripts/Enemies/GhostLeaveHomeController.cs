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
    public bool _isHome;
    private AnimatorController _ghostMove;
    private AnimatorController _leaveHome;

    private bool ghostLeavedHome => _animator.GetCurrentAnimatorStateInfo(0).IsName("Finish");


    public GhostLeaveHomeController(Ghost ghost, AnimatorController ghostMove, AnimatorController leaveHome)
    {
        _ghost = ghost;
        _animator = ghost.GetComponent<Animator>();
        _leaveHome = leaveHome;
        _ghostMove = ghostMove;
        ResetGhost();
    }
    
    public void ResetGhost()
    {
        _isHome = true;
        _ghost.position = _ghost.startMovePosition;
        _animator.runtimeAnimatorController = _leaveHome;
        _animator.enabled = true;
    }
    
    private void LeaveHome()
    {
        _animator.SetTrigger("Leave");
    }
    
    public void TryLeaveHome()
    {
        if(!_isHome) return;
        
        
        float pillets = GameManager.singlton.eatenPillets;
        if (pillets >= _ghost.peletsForLeave && GameManager.singlton.timerTime > _ghost.minTimeForLeave)
        {
            LeaveHome();
        }
        
        if (ghostLeavedHome)
        {
            _animator.runtimeAnimatorController = _ghostMove;
            _isHome = false;
            _ghost.direction = Vector3.left;
            _ghost.position = _ghost.startMovePosition;
        }
    }
}
