using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class Controllers : MonoBehaviour
{
    /* --------------------------- Volumen --------------------------- */
    
    public AudioMixer mixerMusica, mixerSonidos;
    TMP_Text textoVolumenMusica, textoVolumenSonidos;

    void Start()
    {
        textoVolumenMusica = GameObject.Find("NumeroVolumenMusica").GetComponent<TMP_Text>();
        textoVolumenSonidos = GameObject.Find("NumeroVolumenSonidos").GetComponent<TMP_Text>();
    }

    public void setMusicLevel(float valorSlider)
    {
        textoVolumenMusica.text = (valorSlider * 100).ToString("F0");

        valorSlider = valorSlider * 0.9999f + 0.0001f;

        mixerMusica.SetFloat("Volume", Mathf.Log10 (valorSlider) * 20);
    }

    /* --------------------------------------------------------------- */

    public void setSoundLevel(float valorSlider)
    {
        textoVolumenSonidos.text = (valorSlider * 100).ToString("F0");

        valorSlider = valorSlider * 0.9999f + 0.0001f;

        mixerSonidos.SetFloat("Volume", Mathf.Log10(valorSlider) * 20);

        // Reproduzco sonido de muestra
        GameObject.Find("SoundManager").GetComponent<SoundManager>().reproducirSonido(0);
    }

    /* --------------------------------------------------------------- */

}
