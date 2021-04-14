using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;


public class Controllers : MonoBehaviour
{
    /* --------------------------- Volumen --------------------------- */
    
    public AudioMixer mixer;
    GameObject numeroVolumen;
    Text texto;

    public void SetLevel(float valorSlider)
    {
        numeroVolumen = GameObject.Find("Numero Volumen");

        texto = numeroVolumen.GetComponent<Text>();

        texto.text = (valorSlider * 100).ToString("F2");

        valorSlider = valorSlider * 0.9999f + 0.0001f;

        mixer.SetFloat("Volume", Mathf.Log10 (valorSlider) * 20);
    }

    /* --------------------------------------------------------------- */

}
