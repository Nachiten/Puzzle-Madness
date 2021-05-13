using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class Controllers : MonoBehaviour
{
    public AudioMixer mixerMusica, mixerSonidos;

    TMP_Text textoVolumenMusica, textoVolumenSonidos;

    TMP_Dropdown seleccionarMusica;

    Scrollbar scrollMusica, scrollSonidos;

    MusicManager musicManager;

    /* -------------------------------------------------------------------------------- */

    void Awake()
    {
        textoVolumenMusica = GameObject.Find("NumeroVolumenMusica").GetComponent<TMP_Text>();
        textoVolumenSonidos = GameObject.Find("NumeroVolumenSonidos").GetComponent<TMP_Text>();
        musicManager = GameObject.Find("MusicManager").GetComponent<MusicManager>();

        scrollMusica = GameObject.Find("VolumenMusicaScroll").GetComponent<Scrollbar>();
        scrollSonidos = GameObject.Find("VolumenSonidosScroll").GetComponent<Scrollbar>();

        seleccionarMusica = GameObject.Find("SeleccionarMusica").GetComponent<TMP_Dropdown>();
    }

    /* -------------------------------------------------------------------------------- */

    void Start()
    {
        // Seteo la cancion elegida previamente
        if (PlayerPrefs.HasKey("ChosenSong")) 
        {
            int cancionElegida = PlayerPrefs.GetInt("ChosenSong");

            // Si hay alguna cancion elegida, la reproduczo
            if (cancionElegida != 0)
                musicManager.reproducirMusica(cancionElegida - 1);

            seleccionarMusica.value = cancionElegida;
        }

        // Seteo el sonido elegido previamente
        if (PlayerPrefs.HasKey("SoundLevel")) 
        {
            float soundLevel = PlayerPrefs.GetFloat("SoundLevel");

            setearValorSonido(soundLevel);
            scrollSonidos.value = soundLevel;
        }

        // Seteo la musica elegida previamente
        if (PlayerPrefs.HasKey("MusicLevel"))
        {
            float musicLevel = PlayerPrefs.GetFloat("MusicLevel");

            setearValorMusica(musicLevel);
            scrollMusica.value = musicLevel;
        }
    }

    /* -------------------------------------------------------------------------------- */

    public void setMusicLevel(float valorSlider)
    {
        PlayerPrefs.SetFloat("MusicLevel", valorSlider);

        setearValorMusica(valorSlider);
    }

    /* -------------------------------------------------------------------------------- */

    public void setSoundLevel(float valorSlider)
    {
        PlayerPrefs.SetFloat("SoundLevel", valorSlider);

        setearValorSonido(valorSlider);

        if (Input.GetMouseButtonDown(0))
            GameObject.Find("SoundManager").GetComponent<SoundManager>().reproducirSonido(0);
    }

    /* -------------------------------------------------------------------------------- */

    void setearValorSonido(float valorSonido) 
    {
        textoVolumenSonidos.text = (valorSonido * 100).ToString("F0");

        valorSonido = valorSonido * 0.9999f + 0.0001f;

        mixerSonidos.SetFloat("Volume", Mathf.Log10(valorSonido) * 20);
    }

    /* -------------------------------------------------------------------------------- */

    void setearValorMusica(float valorMusica) 
    {
        textoVolumenMusica.text = (valorMusica * 100).ToString("F0");

        valorMusica = valorMusica * 0.9999f + 0.0001f;

        mixerMusica.SetFloat("Volume", Mathf.Log10(valorMusica) * 20);
    }

    /* -------------------------------------------------------------------------------- */

    public void cambiarCancionA(int cancion)
    {
        PlayerPrefs.SetInt("ChosenSong", cancion);

        if (cancion != 0) 
            musicManager.reproducirMusica(cancion - 1);
        else
            musicManager.pararMusica();
    }
}