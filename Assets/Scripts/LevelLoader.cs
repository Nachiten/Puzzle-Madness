using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelLoader : MonoBehaviour
{
    static GameObject levelLoader, panelCargaColor, restoPanelCarga;
    static Slider slider;
    static TMP_Text textoProgreso, textoNivel;

    static bool variablesAsignadas = false;

    int indexACargar;

    /* -------------------------------------------------------------------------------- */

    void Start()
    {
        if (!variablesAsignadas)
        {
            // Aisgnar variables
            levelLoader = GameObject.Find("Panel Carga");
            textoProgreso = GameObject.Find("TextoProgreso").GetComponent<TMP_Text>();
            slider = GameObject.Find("Barra Carga").GetComponent<Slider>();
            panelCargaColor = GameObject.Find("PanelColorCarga");
            restoPanelCarga = GameObject.Find("RestoPanelCarga");

            textoNivel = GameObject.Find("Texto Cargando").GetComponent<TMP_Text>();

            variablesAsignadas = true;

            // Ocultar pantalla de carga
            levelLoader.SetActive(false);
        }
        else 
            quitarPanelCarga();
    }

    /* -------------------------------------------------------------------------------- */

    // Llamar a Corutina
    public void cargarNivel(int index)
    {
        GameObject.Find("SoundManager").GetComponent<SoundManager>().reproducirSonido(1);
        indexACargar = index;
        textoNivel.text = "...";

        ponerPanelCarga();
    }

    /* -------------------------------------------------------------------------------- */

    // Iniciar Corutina para cargar nivel en background
    IEnumerator cargarAsincronizadamente(int index)
    {
        // Iniciar carga de escena
        AsyncOperation operacion = SceneManager.LoadSceneAsync(index);

        operacion.allowSceneActivation = true;

        Debug.Log("[LevelLoader] Cargando escena: " + index);

        // Desde aca si encuentra la escena correcta (no se pq)
        string nombreEscena = SceneManager.GetSceneByBuildIndex(index).name;
        //Debug.Log("Escena que se carga: " + nombreEscena);
        textoNivel.text = "Cargando " + nombreEscena + " ...";

        // Mientras la operacion no este terminada
        while (!operacion.isDone)
        {
            // Generar valor entre 0 y 1
            float progress = Mathf.Clamp01(operacion.progress / 0.9f);
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

    #region AnimacionPonerPanelCarga

    /* --------------------------------------------------------------------------------------- */
    // ----------------------------- ANIMACION PONER PANEL CARGA ----------------------------- // 
    /* --------------------------------------------------------------------------------------- */

    float tiempoAnimacionColorPanel = 0.3f; // 0.3
    float tiempoAnimacionRestoPanel = 0.2f; // 0.2

    void ponerPanelCarga()
    {
        restoPanelCarga.SetActive(false);

        LeanTween.value(levelLoader, 0, 0, 0f)
            .setOnUpdate(mostrarColorPanelAlfa).setOnComplete(iniciarMostrarPanelCarga);  
    }

    void mostrarColorPanelAlfa(float value) 
    {
        panelCargaColor.GetComponent<Image>().color = new Color(0.149f, 0.149f, 0.149f, value);
    }

    void iniciarMostrarPanelCarga() {
        levelLoader.SetActive(true);
        LeanTween.value(levelLoader, 0, 1, tiempoAnimacionColorPanel)
                .setOnUpdate(mostrarColorPanelAlfa).setOnComplete(mostrarPanelCarga);
    }

    void mostrarPanelCarga()
    {
        LeanTween.scaleY(restoPanelCarga, 0, 0f).setOnComplete(mostrarRestoPanelCarga);
    }

    void mostrarRestoPanelCarga() 
    {
        restoPanelCarga.SetActive(true);
        LeanTween.scaleY(restoPanelCarga, 1, tiempoAnimacionRestoPanel).setOnComplete(completarCargaNivel);
    }

    void completarCargaNivel() 
    {
        StartCoroutine(cargarAsincronizadamente(indexACargar));

        if (indexACargar != 7)
        {
            AnalyticsResult result = AnalyticsEvent.Custom("Ingreso_" + SceneManager.GetSceneByBuildIndex(indexACargar).name);
            //Debug.Log("[LevelLoader] Analytics Result: " + result + " | DATA: " + "Ingreso_" + SceneManager.GetSceneByBuildIndex(index).name);
        }
    }

    #endregion

    #region AnimacionQuitarPanelCarga

    /* ---------------------------------------------------------------------------------------- */
    // ----------------------------- ANIMACION QUITAR PANEL CARGA ----------------------------- // 
    /* ---------------------------------------------------------------------------------------- */

    void quitarPanelCarga() 
    {
        LeanTween.scaleY(restoPanelCarga, 0, tiempoAnimacionRestoPanel).setOnComplete(ocultarColorPanelAlfa);
    }

    void ocultarColorPanelAlfa() 
    {
        LeanTween.value(levelLoader, 1, 0, tiempoAnimacionColorPanel)
            .setOnUpdate(mostrarColorPanelAlfa).setOnComplete(esconderPanelCarga);
    }

    void esconderPanelCarga() {
        levelLoader.SetActive(false);
    }

    #endregion
}
