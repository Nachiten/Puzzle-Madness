using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip[] clipsSonido;

    AudioSource sourceSonido;

    /* -------------------------------------------------------------------------------- */

    private void Awake() { sourceSonido = GetComponent<AudioSource>(); }

    public void reproducirSonido(int sonido)
    {
        sourceSonido.clip = clipsSonido[sonido];
        sourceSonido.Play();
    }
}