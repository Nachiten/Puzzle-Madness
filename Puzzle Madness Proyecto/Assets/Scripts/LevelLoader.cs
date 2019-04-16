using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    GameObject levelLoader;
        Slider slider;
          Text textoProgreso;

    /* -------------------------------------------------------------------------------- */

    void Start()
    {
        if (!GameObject.Find("Panel Carga")) {
            Debug.LogError("PANEL CARGA DESACTIVADO !!!");
        }

        // Aisgnar variables
        levelLoader = GameObject.Find("Panel Carga");
        textoProgreso = GameObject.Find("TextoProgreso").GetComponent<Text>();
        slider = GameObject.Find("Barra Carga").GetComponent<Slider>();

        // Ocultar pantalla de carga
        levelLoader.SetActive(false);
    }

    /* -------------------------------------------------------------------------------- */

    // Llamar a Corutina
    public void cargarNivel(int index)
    {
        StartCoroutine(cargarAsincronizadamente(index));
    }

    /* -------------------------------------------------------------------------------- */

    // Iniciar Corutina para cargar nivel en background
    IEnumerator cargarAsincronizadamente (int index)
    {

        // Iniciar carga de escena
        AsyncOperation operacion = SceneManager.LoadSceneAsync(index);

        // Mostrar pantalla de carga
        levelLoader.SetActive(true);

        Debug.Log("Cargando escena: " + index);

        // Mientras la operacion no este terminada
        while (!operacion.isDone)
        {
            // Generar valor entre 0 y 1
            float progress = Mathf.Clamp01(operacion.progress / .9f);
            // Modificar Slider
            slider.value = progress;
            // Modificar texto progreso
            textoProgreso.text = progress * 100f + "%";

            yield return null;
        }
    }
}
