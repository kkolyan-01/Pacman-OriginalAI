using System;
using UnityEngine;


public class GameTimer : MonoBehaviour
{
    private float _time;
    private bool _isPause;

    public float time => _time; 

    private void Awake()
    {
        _isPause = true;
    }
    
    private void FixedUpdate()
    {
        if(!_isPause)
            _time += Time.fixedDeltaTime;
    }

    public void Reset()
    {
        _time = 0;
    }

    public void Pause()
    {
        _isPause = true;
    }

    public void Run()
    {
        _isPause = false;
    }
    
}
