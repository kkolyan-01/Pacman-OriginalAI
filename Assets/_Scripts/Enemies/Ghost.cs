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
    [SerializeField] private float _startMultiplySpeed;
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

    private Vector2 _corner;
    private GhostBehavior _behavior;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private GhostStateManager _stateManager;
    private GhostLeaveHomeController _leaveController;
    private GhostMoveAnimator _moveAnimator;
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
        set => _state = value;
        get => _state;
    }
    public Vector3 goalCell { get; set; }
    public bool isHome { get; set; }
    public float normalMultyplaySpeed { get; set; }

    private void Awake()
    {
        InitializeComponents();
        GameManager.singlton.AddGhost(this);
        direction = Vector3.left;
        normalMultyplaySpeed = _startMultiplySpeed;
    }

    private void InitializeComponents()
    {
        _corner = _cornerTransform.position;
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _behavior = GhostBehaviorFactory.Create(_type, this);
        _leaveController = new GhostLeaveHomeController(this);
        _moveAnimator = new GhostMoveAnimator(this, _animator);
        _stateManager = new GhostStateManager(this, _ghostStateSystem);
    }

    private void Start()
    {
        ResetGhost();
    }

    private void FixedUpdate()
    {
        CheckCurrentCell();
        _behavior.Move();
        if (state != GhostState.frightened)
            _stateManager.SetTimerState();
    }

    private void LateUpdate()
    {
        _leaveController.TryLeaveHome();
        _moveAnimator.Play();
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
        _stateManager.SetTimerState();
        ResetGhost();
    }
    
    public void ResetGhost()
    {
        _spriteRenderer.enabled = true;
        enabled = true;
        _animator.runtimeAnimatorController = _leaveHome;
        _animator.enabled = true;
        _leaveController.ResetGhost();
        StartCoroutine(ResetCourotine());
    }

    public IEnumerator ResetCourotine()
    {
        yield return new WaitWhile(() => isHome);
        _animator.runtimeAnimatorController = _ghostMove;
        direction = Vector3.left;
        position = startMovePosition;
        _targetCell = Vector3.zero;
    }

    public void Stop()
    {
        enabled = false;
        _animator.enabled = false;
    }

    public void SetNormalSpeed()
    {
        multiplySpeed = normalMultyplaySpeed;
    }

    public void SetFrightenedSpeed()
    {
        multiplySpeed = _frightenedMultiplySpeed;
    }

    public void Frighten()
    {
        _stateManager.Frighten();
    }

    public void DisableFrighten()
    {
        _stateManager.DisableFrighten();
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
