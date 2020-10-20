using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountUpTimerBehaviour : MonoBehaviour
{
    private const float TenMinutes = 600;
    private TMP_Text _timer = null;
    private float _time = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        _timer = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTime();
        TimeSpan timeSpan = TimeSpan.FromSeconds(_time);
        string timeText = string.Format("{0:D2}:{1:D2}",  timeSpan.Minutes, timeSpan.Seconds);
        _timer.text = timeText;
    }

    private void UpdateTime()
    {
        if (_time < TenMinutes)
        {
            _time += Time.deltaTime;
        }
        else
        {
            _time = TenMinutes;
        }
    }
}
