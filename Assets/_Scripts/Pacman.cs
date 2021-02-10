using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Pacman : MovingObject
{
    [SerializeField] private float _startSpeed;
    [SerializeField] private float _delayDeadAnimation;
    [SerializeField] private Vector3 _startPosition;
    [SerializeField] private AudioSource _eatAudio;
    
    private Vector3 _desiredDirection;
    private Tilemap _pelletsTiles;
    private SpriteRenderer _spriteRenderer;
    private bool isDead;
    private Animator _animator;

    private float angle
    {
        set
        {
            Vector3 tempEulerAngles = transform.eulerAngles;
            tempEulerAngles.z = value;
            transform.eulerAngles = tempEulerAngles;
        }
    }
    public void SetDirection(Vector2 direction)
    {
        _desiredDirection = direction;
    }
    
    private void Awake()
    {
        InitializeComponents();
        ResetPacman();
    }
    
    private void InitializeComponents()
    {
        _pelletsTiles = GameObject.FindGameObjectWithTag("Pellet").GetComponent<Tilemap>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        Move();
        Eat();
    }
    
    private void Move()
    {
        if (direction != _desiredDirection)
        {
            ChangeDirection();
        }
        RotationPacman();
        MoveOneStep();
    }
    
    private void ChangeDirection()
    {
        if (!CanChangeState()) return;

        direction = _desiredDirection;
    }

    private bool CanChangeState()
    {
        bool canMoveToDesiredCell = Map.CanMoveToNextCell(transform.position, _desiredDirection);
        return canMoveToDesiredCell && isInCenterCell;
    }

    private void RotationPacman()
    {
        float newAngle = 0;

        if (direction == Vector3.right)
        {
            newAngle = 0;
            _spriteRenderer.flipY = false;
        }
        else if (direction == Vector3.left)
        {
            newAngle = 180;
            _spriteRenderer.flipY = true;
        }
        else if (direction == Vector3.up)
        {
            newAngle = 90;
            _spriteRenderer.flipY = false;
        }
        else if (direction == Vector3.down)
        {
            newAngle = 270;
            _spriteRenderer.flipY = true;
        }

        angle = newAngle;
    }

    private void Eat()
    {
        if (_pelletsTiles.HasTile(positionInt))
        {
            _pelletsTiles.SetTile(positionInt, null);
            GameManager.singlton.AddScore(_pelletsTiles.gameObject);
            _eatAudio.Play();
                
        }
    }
    
    public void Die()
    {
        if(isDead) return;
        
        multiplySpeed = 0;
        GameManager.singlton.LoseLife();
        isDead = true;
        StartCoroutine(DeadAnimate());
    }

    private IEnumerator DeadAnimate()
    {
        yield return new WaitForSeconds(_delayDeadAnimation);
        _animator.SetBool("isDead", isDead);
    }

    public void ResetPacman()
    {
        transform.position = _startPosition;
        multiplySpeed = _startSpeed;
        direction = Vector3.right;
        _desiredDirection = Vector3.right;
        isDead = false;
        _targetCell = Vector3.zero;
        _animator.SetBool("isDead", isDead);
    }
}