using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    static GameObject levelLoader;
    static Slider slider;
    static Text textoProgreso;
    static Text textoNivel;

    public Texture[] textura;

    public bool DeleteKeys = false;
    static bool flag = true;

    GameObject juego1;
    GameObject juego2;

    /* -------------------------------------------------------------------------------- */

    void Start()
    {
        if (DeleteKeys) {
            borrarTodasLasKeys();
        }

        if (flag) { 
            // Aisgnar variables
            levelLoader = GameObject.Find("Panel Carga");
            textoProgreso = GameObject.Find("TextoProgreso").GetComponent<Text>();
            slider = GameObject.Find("Barra Carga").GetComponent<Slider>();

            textoNivel = GameObject.Find("Texto Cargando").GetComponent<Text>();

            flag = false;
        }


        // Ocultar pantalla de carga
        levelLoader.SetActive(false);

        if (SceneManager.GetActiveScene().buildIndex == 12) {

            juego1 = GameObject.Find("Canvas Juego1");
            juego2 = GameObject.Find("Canvas Juego2");

            juego1.SetActive(false);
            scanJuego(2);

            juego1.SetActive(true);
            juego2.SetActive(false);
            scanJuego(1);
            
        }
    }

    /* -------------------------------------------------------------------------------- */

    void scanJuego(int nivel)
    {
        for (int i = 1; i < 11; i++)
        {
            RawImage imagen = GameObject.Find("Image" + i.ToString()).GetComponent<RawImage>();

            Text textoReloj = GameObject.Find("Timer" + i.ToString()).GetComponent<Text>();

            string index;

            if (nivel == 1) index = i.ToString();
            else index = index = (i + 12).ToString();

            if (PlayerPrefs.GetString(index) == "Ganado")
            {
                float time = PlayerPrefs.GetFloat("Time_" + index);

                string minutes = Mathf.Floor((time % 3600) / 60).ToString("00");
                string seconds = Mathf.Floor(time % 60).ToString("00");
                string miliseconds = Mathf.Floor(time % 6 * 10 % 10).ToString("0");

                textoReloj.text = minutes + ":" + seconds + ":" + miliseconds;

                imagen.texture = textura[0];
            }
            else
            {
                imagen.texture = textura[1];
                textoReloj.text = "";
            }
        }
    }

    /* -------------------------------------------------------------------------------- */

    // Llamar a Corutina
    public void cargarNivel(int index)
    {
        StartCoroutine(cargarAsincronizadamente(index));
        textoNivel.text ="Cargando " + SceneManager.GetSceneByBuildIndex(index).name + " ...";

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

    /* -------------------------------------------------------------------------------- */

    bool nivel2 = false;

    public void cambiarNivel()
    {
        juego1.SetActive(nivel2);
        juego2.SetActive(!nivel2);

        nivel2 = !nivel2;
    }

    public void borrarTodasLasKeys() {
        Debug.LogError("BORRANDO TODAS LAS KEYS !!!!");
        PlayerPrefs.DeleteAll();
    }
}
