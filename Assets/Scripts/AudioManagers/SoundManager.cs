using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip[] clipsSonido;

    /* -------------------------------------------------------------------------------- */

    public void reproducirSonido(int sonido)
    {
        AudioSource sourceSonido = GetComponent<AudioSource>();

        sourceSonido.clip = clipsSonido[sonido];
        sourceSonido.Play();
    }
}