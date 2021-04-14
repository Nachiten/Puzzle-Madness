using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;


public class Controllers : MonoBehaviour
{
    /* --------------------------- Volumen --------------------------- */
    
    public AudioMixer mixerMusica, mixerSonidos;
    GameObject numeroVolumen;
    Text texto;

    public void setMusicLevel(float valorSlider)
    {
        numeroVolumen = GameObject.Find("NumeroVolumenMusica");

        texto = numeroVolumen.GetComponent<Text>();

        texto.text = (valorSlider * 100).ToString("F0");

        valorSlider = valorSlider * 0.9999f + 0.0001f;

        mixerMusica.SetFloat("Volume", Mathf.Log10 (valorSlider) * 20);
    }

    /* --------------------------------------------------------------- */

    public void setSoundLevel(float valorSlider)
    {
        numeroVolumen = GameObject.Find("NumeroVolumenSonidos");

        texto = numeroVolumen.GetComponent<Text>();

        texto.text = (valorSlider * 100).ToString("F0");

        valorSlider = valorSlider * 0.9999f + 0.0001f;

        mixerSonidos.SetFloat("Volume", Mathf.Log10(valorSlider) * 20);

        GameObject.Find("SoundManager").GetComponent<SoundManager>().reproducirSonido(0);
    }

    /* --------------------------------------------------------------- */

}
