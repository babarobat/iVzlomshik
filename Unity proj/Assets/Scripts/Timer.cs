using UnityEngine;
using System.Collections;
using System;
using System.Timers;

public class Timer{ 


    public float Sec { get; private set; }
    public float Min { get; private set; }

    private float _currentCount = 0;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="countTo"></param>
    public void StartCountingTo(float countTo)
    {
        
    }

    void CountTo(float countTo)
    {
        if (_currentCount< countTo)
        {
            _currentCount++;
        }
    }
}
