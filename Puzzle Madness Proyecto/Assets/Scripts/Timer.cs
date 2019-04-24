using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    Text timerText;
    
    float time;
    public float speed = 1;
    bool start;

    /* -------------------------------------------------------------------------------- */

    void Start()
    {
        timerText = GameObject.Find("Timer").GetComponent<Text>();
    }

    /* -------------------------------------------------------------------------------- */

    void Update()
    {
        if (start) { 
        time += Time.deltaTime * speed;

        string minutes = Mathf.Floor((time % 3600) / 60).ToString("00");
        string seconds = Mathf.Floor(time % 60).ToString("00");
        string miliseconds = Mathf.Floor( time % 6 * 10 % 10 ).ToString("0");

        timerText.text = minutes + ":" + seconds + ":" + miliseconds;
        }

    }

    /* -------------------------------------------------------------------------------- */

    public void toggleClock(bool valor) { start = valor; }
}
