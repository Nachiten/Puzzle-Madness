using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    GameObject levelLoader;
        Slider slider;
          Text textoProgreso;
          Text textoNivel;

    public Texture[] textura;

    public bool DeleteKeys = false;

    /* -------------------------------------------------------------------------------- */

    void Start()
    {
        if (DeleteKeys) {
        Debug.LogError("BORRANDO TODAS LAS KEYS !!!!");
        PlayerPrefs.DeleteAll();
        }

        if (!GameObject.Find("Panel Carga")) Debug.LogError("PANEL CARGA DESACTIVADO !!!");
        

        // Aisgnar variables
        levelLoader = GameObject.Find("Panel Carga");
        textoProgreso = GameObject.Find("TextoProgreso").GetComponent<Text>();
        slider = GameObject.Find("Barra Carga").GetComponent<Slider>();

        textoNivel = GameObject.Find("Texto Cargando").GetComponent<Text>();

        // Ocultar pantalla de carga
        levelLoader.SetActive(false);

        if (SceneManager.GetActiveScene().buildIndex == 7) {

            for (int i = 1; i < 6; i++) {

                RawImage imagen = GameObject.Find("Image" + i.ToString()).GetComponent<RawImage>();

                Text reloj = GameObject.Find("Timer " + i.ToString()).GetComponent<Text>();

                if (PlayerPrefs.GetString("Nivel0" + i.ToString()) == "Ganado")
                {
                    float time = PlayerPrefs.GetFloat("Time_" + i.ToString());

                    string minutes = Mathf.Floor((time % 3600) / 60).ToString("00");
                    string seconds = Mathf.Floor(time % 60).ToString("00");
                    string miliseconds = Mathf.Floor(time % 6 * 10 % 10).ToString("0");

                    reloj.text = minutes + ":" + seconds + ":" + miliseconds;

                    imagen.texture = textura[0];
                }
                else { 
                    imagen.texture = textura[1];
                    reloj.text = "";
                }
            }
        }
    }

    /* -------------------------------------------------------------------------------- */

    // Llamar a Corutina
    public void cargarNivel(int index)
    {
        StartCoroutine(cargarAsincronizadamente(index));
        textoNivel.text ="Cargando '" + SceneManager.GetSceneByBuildIndex(index).name + "' ...";

        if (index != 7) { 
            AnalyticsResult result =  AnalyticsEvent.Custom("Ingreso_" + SceneManager.GetSceneByBuildIndex(index).name);
            Debug.Log("Analytics Result: " + result + " | DATA: " + "Ingreso_" + SceneManager.GetSceneByBuildIndex(index).name);
        }
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

    /* -------------------------------------------------------------------------------- */

    public void salir() { Application.Quit(); }


}
