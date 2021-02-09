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

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            _pacman.SetDirection(Vector2.right);
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _pacman.SetDirection(Vector2.left);
        }
        else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            _pacman.SetDirection(Vector2.up);
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            _pacman.SetDirection(Vector2.down);
        }
    }
}
