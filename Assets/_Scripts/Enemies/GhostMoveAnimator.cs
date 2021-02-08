using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMoveAnimator
{

    private Ghost _ghost;
    private Animator _animator;
    
    public GhostMoveAnimator(Ghost ghost, Animator animator)
    {
        _ghost = ghost;
        _animator = animator;
    }
    
    public void Play()
    {
        if(_ghost.state == GhostState.frightened) return;
        
        if (_ghost.direction.Equals(Vector3.up))
        {
            _animator.SetTrigger("Up");
        }
        else if (_ghost.direction.Equals(Vector3.right))
        {
            _animator.SetTrigger("Right");
        }
        else if (_ghost.direction.Equals(Vector3.down))
        {
            _animator.SetTrigger("Down");
        }
        else if (_ghost.direction.Equals(Vector3.left))
        {
            _animator.SetTrigger("Left");
        }
    }
}
