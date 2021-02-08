using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Energizer : MonoBehaviour
{
    private Pacman _pacman;

    private void Start()
    {
        _pacman = GameManager.singlton.pacman;
    }

    private void FixedUpdate()
    {
        if (_pacman.position == transform.position)
        {
            GameManager.singlton.StartFrightenedMode();
            Destroy(gameObject);
        }
    }
}
