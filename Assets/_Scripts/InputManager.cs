using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private Pacman _pacman;
    
    private void Start()
    {
        _pacman = FindObjectOfType<Pacman>();
        if (_pacman == null)
        {
            Debug.LogError("Pacman не найден!");
        }
    }

    private void Update()
    {
        if(_pacman == null) return;

        if (Input.GetKeyDown(KeyCode.D))
        {
            _pacman.SetDirection(Vector2.right);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            _pacman.SetDirection(Vector2.left);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            _pacman.SetDirection(Vector2.up);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            _pacman.SetDirection(Vector2.down);
        }
    }
}
