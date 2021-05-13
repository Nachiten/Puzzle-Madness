using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioClip[] clipsMusica;

    AudioSource sourceMusica;

    /* -------------------------------------------------------------------------------- */

    void Awake() { sourceMusica = GetComponent<AudioSource>(); }

    /* -------------------------------------------------------------------------------- */

    public void reproducirMusica(int musica)
    {
        sourceMusica.Stop();

        sourceMusica.clip = clipsMusica[musica];
        sourceMusica.Play();
    }

    /* -------------------------------------------------------------------------------- */

    public void pararMusica() 
    {
        sourceMusica.Stop();
    }
}
