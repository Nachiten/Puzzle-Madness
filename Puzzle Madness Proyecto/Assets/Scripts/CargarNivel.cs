using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CargarNivel : MonoBehaviour
{
    public GameObject levelLoader;
    public Slider slider;
    public Text textoProgreso;

    /* -------------------------------------------------------------------------------- */

    // Cargar Nivel
    public void cargarNivel(int index)
    {
        StartCoroutine(cargarAsincronizadamente(index));
    }

    /* -------------------------------------------------------------------------------- */

    // Iniciar Corutina
    IEnumerator cargarAsincronizadamente (int index)
    {
        AsyncOperation operacion = SceneManager.LoadSceneAsync(index);

        levelLoader.SetActive(true);

        while (!operacion.isDone)
        {
            float progress = Mathf.Clamp01(operacion.progress / .9f);

            slider.value = progress;
            Debug.Log(progress);
            textoProgreso.text = progress * 100f + "%";

            yield return null;
        }
    }


}
