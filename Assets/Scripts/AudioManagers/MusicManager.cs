using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioClip[] clipsMusica;

    AudioSource sourceMusica;

    /* -------------------------------------------------------------------------------- */

    void Start()
    {
        sourceMusica = GetComponent<AudioSource>();
    }

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
