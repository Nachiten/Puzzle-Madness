using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
     Text timerText;
     bool start = false;
    float startTime;

    /* -------------------------------------------------------------------------------- */

    void Start() { timerText = GameObject.Find("Timer").GetComponent<Text>(); }

    /* -------------------------------------------------------------------------------- */

    // Cada fotograma actualizar el timer
    void Update()
    {
        // Solo si el juego no esta ganado
        if (start) { 
            float t = Time.time - startTime;

            string minutes = ( (int)t / 60 ).ToString();
            string seconds = (t % 60).ToString("F0");
            float milisegundos;

            if ( (t % 60) % 1 * 10 - 5 < 0 ) milisegundos = 10 + ((t % 60) % 1 * 10 - 5);

            else milisegundos = ((t % 60) % 1 * 10 - 5);

            if (milisegundos > 9) milisegundos = 0;

            timerText.text = minutes + ":" + seconds + ":" + milisegundos.ToString("F0");

        }
    }

    /* -------------------------------------------------------------------------------- */

    public void toggleClock(bool flag)
    {
        start = flag;
        if (flag)
        {
            startTime = Time.time;
        }
    }
}
