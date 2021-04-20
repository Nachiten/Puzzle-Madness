using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Timer : MonoBehaviour
{
    TMP_Text timerText;
    
    float time;
    float speed = 1;

    bool start;

    /* -------------------------------------------------------------------------------- */

    void Start() { timerText = GameObject.Find("Timer").GetComponent<TMP_Text>(); }

    /* -------------------------------------------------------------------------------- */

    void Update()
    {
        if (start) 
        { 
            time += Time.deltaTime * speed;

            string minutes = Mathf.Floor((time % 3600) / 60).ToString("00");
            string seconds = Mathf.Floor(time % 60).ToString("00");
            string miliseconds = Mathf.Floor( time % 6 * 10 % 10 ).ToString("0");

            timerText.text = minutes + ":" + seconds + ":" + miliseconds;
        }

    }

    /* -------------------------------------------------------------------------------- */

    public void toggleClock(bool valor) { start = valor; }

    /* -------------------------------------------------------------------------------- */

    float playerPref;

    public void setPlayerPref() {

        playerPref = PlayerPrefs.GetFloat("Time_" + SceneManager.GetActiveScene().buildIndex);;

        if (time < playerPref || playerPref == 0)
        {
            Debug.Log("[Timer] Guardando PlayerPref .....");
            PlayerPrefs.SetFloat("Time_" + SceneManager.GetActiveScene().buildIndex, time);
        }
        else 
            Debug.Log("[Timer] Tiempo mayor al previamente guardado, no guardando...");
        

    }
}
