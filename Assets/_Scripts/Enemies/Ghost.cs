using System;
using System.Collections;
using Unity.Collections;
using UnityEditor.Animations;
using UnityEngine;

public enum GhostState
{
    chase,
    scatter,
    frightened,
}


public class Ghost : MovingObject
{
    
    [Header("Settings")]
    [SerializeField] private Pacman _pacman;
    [SerializeField] private GhostType _type;
    [SerializeField] private Transform _cornerTransform;
    [SerializeField] private float _normalMultiplySpeed;
    [SerializeField] private float _frightenedMultiplySpeed;
    [SerializeField] private GhostStateSystem _ghostStateSystem;
    [SerializeField] private float _rebornDelay;

    [Header("Leave Home Settings")] 
    [SerializeField] private int _peletsForLeave;
    [SerializeField] private float _minTimeForLeave;
    [SerializeField] private AnimatorController _ghostMove;
    [SerializeField] private AnimatorController _leaveHome;
    [SerializeField] private Vector3 _startMovePosition;

    [Header("Dynamically")]
    [SerializeField] private GhostState _state;
    [SerializeField] private GhostState _timerState;
    
    private Vector2 _corner;
    private GhostBehavior _ghostBehavior;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private GhostLeaveHomeController _leaveController;
    private GhostMoveAnimator _ghostMoveAnimator;
    private bool _isDead;

    public bool isInCrossroadPosition => Map.IsPositionAtCrossroad(position);
    public bool isInAtadvantageCellPosition
    {
        get
        {
            Vector2[] advantageCells = Map.singlton.GetAdvantageCells();
            foreach (var advantageCell in advantageCells)
            {
                Vector2 directionCell = position;
                if (directionCell.Equals(advantageCell))
                    return true;
            }
            return false;
        }
    }
    public Vector2 corner => _corner;
    public int peletsForLeave => _peletsForLeave;
    public float minTimeForLeave => _minTimeForLeave;
    public Vector3 startMovePosition => _startMovePosition;
    public GhostState state
    {
        get => _state;
    }
    public Vector3 goalCell { get; set; }

    private void Awake()
    {
        InitializeComponents();
        GameManager.singlton.AddGhost(this);
        direction = Vector3.left;
        multiplySpeed = _normalMultiplySpeed;
    }

    private void InitializeComponents()
    {
        _corner = _cornerTransform.position;
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _ghostBehavior = GhostBehaviorFactory.Create(_type, this);
        _leaveController = new GhostLeaveHomeController(this, _ghostMove, _leaveHome);
        _ghostMoveAnimator = new GhostMoveAnimator(this, _animator);
    }

    private void FixedUpdate()
    {
        CheckCurrentCell();
        _ghostBehavior.Move();
        FindTimerState();
        
        if(_state != GhostState.frightened)
            SetTimerState();
    }

    private void LateUpdate()
    {
        _leaveController.TryLeaveHome();
        _ghostMoveAnimator.Play();
    }

    private void CheckCurrentCell()
    {
        if (positionInt == _pacman.positionInt)
        {
            CollidePacman();
        }
    }

    private void CollidePacman()
    {
        if (state != GhostState.frightened)
        {
            _pacman.Die(); 
        }
        else
        {
            Die();
        }
    } 

    public void Die()
    {
        if(_isDead) return;
        _isDead = true;
        GameManager.singlton.AddScore(gameObject);
        StartCoroutine(Reborn());
    }

    private IEnumerator Reborn()
    {
        _spriteRenderer.enabled = false;
        enabled = false;
        yield return new WaitForSeconds(_rebornDelay);
        _isDead = false;
        SetTimerState();
        ResetGhost();
    }
    
    public void ResetGhost()
    {
        _spriteRenderer.enabled = true;
        enabled = true;
        _leaveController.ResetGhost();
        
    }
    
    private void FindTimerState()
    {
        var stateTimes = _ghostStateSystem._stateTimes;
        _timerState = FindTimerState(stateTimes);
    }

    private GhostState FindTimerState(StateTime[] stateTimes)
    {
        var timer = GameManager.singlton.timerTime;
        GhostState newState = stateTimes[0].state;
        foreach (var stateTime in stateTimes)
        {
            float timeStartState = stateTime.timeStart;
            if (timer > timeStartState)
            {
                newState = stateTime.state;
            }
        }

        return newState;
    }

    private void SetTimerState()
    {
        multiplySpeed = _normalMultiplySpeed;
        _state = _timerState;
    }

    public void Frighten()
    {
        if(_leaveController._isHome) return;
        
        _animator.SetBool("Frightened", true);
        _state = GhostState.frightened;
        multiplySpeed = _frightenedMultiplySpeed;
    }

    public void DisableFrighten()
    {
        if(_leaveController._isHome) return;
        
        _animator.SetBool("Frightened", false);
        SetTimerState();
    }

    public void Stop()
    {
        enabled = false;
        _animator.enabled = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (goalCell != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(goalCell, 0.5f);
        }
    }
    
}
