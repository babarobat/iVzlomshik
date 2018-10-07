using UnityEngine;
using System.Collections;
using System;


public class TimeCounter {

    public float countFrom;
    public float countTo;

    
    private DateTime _start;
    private float _elapsed = -1;
    private TimeSpan _duration;
    float _currentTimeLeft = 0;
    public void StartCountingDown()
    {

    }
    public void StartCountingDown(float to)
    {

    }

    public void StartCountingUp(DateTime from, DateTime to)
    {

    }
    public void StopTimer()
    {

    }
    void Tick(object state)
    {
        _duration = DateTime.Now - _start;
        Debug.Log(_duration);
    }

    public delegate void Action();
   

    public void Start(float elapsed)
    {
        _elapsed = elapsed;
        _start = DateTime.Now;
        _duration = TimeSpan.Zero;
    }

    public void Update()
    {
        if (_elapsed > 0)
        {
            _duration = DateTime.Now - _start;
            _currentTimeLeft = _elapsed - _duration.Seconds;
            if (_duration.TotalSeconds > _elapsed)
            {
                _elapsed = 0;
            }
        }
        else if (_elapsed == 0)
        {
            _elapsed = -1;
        }
    }
    public bool IsEvent()
    {
        return _elapsed == 0;
    }
    public string GetSecMilisec()
    {
        var tmpSec = _currentTimeLeft.ToString("f0");
        var tmpMilSec = ((_currentTimeLeft * 1000) % 1000).ToString("f3");
        return tmpSec + ":"+ tmpMilSec;
        
    }


}
