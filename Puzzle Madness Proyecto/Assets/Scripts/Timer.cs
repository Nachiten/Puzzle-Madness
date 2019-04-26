using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    Text timerText;
    
    float time;
    public float speed = 1;
    bool start;

    /* -------------------------------------------------------------------------------- */

    void Start() { timerText = GameObject.Find("Timer").GetComponent<Text>(); }

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

    /* -------------------------------------------------------------------------------- */

    float playerPref;

    public void setPlayerPref() {

        playerPref = PlayerPrefs.GetFloat("Time_" + SceneManager.GetActiveScene().buildIndex);

        Debug.Log(playerPref );

        if (time < playerPref || playerPref == 0)
        {
            Debug.Log("Guardando player preff de tiempo .....");
            PlayerPrefs.SetFloat("Time_" + SceneManager.GetActiveScene().buildIndex, time);
        }
        else Debug.Log("Tiempo mayor al previamente guardado, no guardando...");
        

    }
}
