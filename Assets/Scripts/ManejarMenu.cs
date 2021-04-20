using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class ManejarMenu : MonoBehaviour
{
    #region Variables

    // Flags varios
    bool menuActivo = false, opcionesActivas = false, creditosActivos = false, mostrandoContinuarDesdeNivel = false;

    // Flag de ya asigne las variables
    static bool variablesAsignadas = false;

    // GameObjects
    static GameObject menu, opciones, creditos, continuarDesdeNivel, panelMenu;

    // Manager de las animaciones
    static LeanTweenManager tweenManager;

    // Textos varios
    static TMP_Text textoNivelNoGanado, textoBoton;

    // Strings utilizados
    string continuar = "CONTINUAR", comenzar = "COMENZAR";

    // Index de escena actual
    int index;

    #endregion

    /* -------------------------------------------------------------------------------- */

    #region FuncionStart

    void Start()
    {
        index = SceneManager.GetActiveScene().buildIndex;
       
        if (!variablesAsignadas)
        {
            menu = GameObject.Find("Menu");
            panelMenu = GameObject.Find("PanelMenu");
            opciones = GameObject.Find("MenuOpciones");
            creditos = GameObject.Find("MenuCreditos");
            
            textoBoton = GameObject.Find("TextoBotonComenzar").GetComponent<TMP_Text>();
            
            tweenManager = GameObject.Find("Canvas Menu").GetComponent<LeanTweenManager>();

            variablesAsignadas = true;
        }

        // No estoy en el menu principal
        if (index != 0)
        {
            textoBoton.text = continuar;

            // Oculto todo de una patada pq se esta mostrando la pantalla de carga
            menu.SetActive(false);
            panelMenu.SetActive(false);
        }
        // Estoy en el menu principal
        else
        {
            continuarDesdeNivel = GameObject.Find("ContinuarDesdeNivel");
            textoNivelNoGanado = GameObject.Find("TextoContinuar").GetComponent<TMP_Text>();

            textoBoton.text = comenzar;
            menu.SetActive(true);
            menuActivo = true;

            if (yaJugoAntes())
            {
                textoBoton.text = continuar;
                mostrarUltimoNivelNoGanado();
                mostrandoContinuarDesdeNivel = true;
            }
            else
            {
                continuarDesdeNivel.SetActive(false);
            }
        }

        opciones.SetActive(false);
        creditos.SetActive(false);
        
    }

    #endregion

    /* -------------------------------------------------------------------------------- */

    #region FuncionUpdate

    void Update()
    {
        index = SceneManager.GetActiveScene().buildIndex;

        if (index == 0) return;

        bool animacionEnEjecucion = GameObject.Find("Canvas Menu").GetComponent<LeanTweenManager>().animacionEnEjecucion;

        if (Input.GetKeyDown("escape") && !animacionEnEjecucion)
            manejarMenu();
    }

    #endregion

    /* -------------------------------------------------------------------------------- */

    bool yaJugoAntes() 
    {
        if (PlayerPrefs.GetInt("YaJugoAntes") == 1)
        {
            return true;
        }
        else 
        {
            PlayerPrefs.SetInt("YaJugoAntes", 1);
            return false;
        }
    }

    /* -------------------------------------------------------------------------------- */

    public void ocultarContinuarDesdeNivelSiCorresponde() 
    {
        if (mostrandoContinuarDesdeNivel) 
        {
            bool continuarDesdeNivelActivo = continuarDesdeNivel.activeSelf;
            continuarDesdeNivel.SetActive(!continuarDesdeNivelActivo);
        }
    }

    /* -------------------------------------------------------------------------------- */

    void mostrarUltimoNivelNoGanado() 
    {
        int indexNoGanado = 0;

        for (int i = 1; i <= 22; i++)
        {
            if (i == 11 || i == 12)
                continue;

            float playerPrefGuardada = PlayerPrefs.GetFloat("Time_" + i);

            if (playerPrefGuardada == 0.0f) {
                indexNoGanado = i;
                break;
            }
        }

        int juegoNoGanado;
        int nivelNoGanado;

        if (indexNoGanado < 11)
        {
            juegoNoGanado = 1;
            nivelNoGanado = indexNoGanado;
            FindObjectOfType<Buttons>().nivelACargar = indexNoGanado;
        }
        else if (indexNoGanado < 23)
        {
            juegoNoGanado = 2;
            nivelNoGanado = indexNoGanado - 12;
            FindObjectOfType<Buttons>().nivelACargar = indexNoGanado;
        }
        else {
            juegoNoGanado = 0;
            nivelNoGanado = 0;
        }

        //Debug.Log("[ManejarMenu] Ultimo nivel no ganado. Nivel: " + nivelNoGanado + " | Juego: " + juegoNoGanado);

        textoNivelNoGanado.text = "Nivel: " + nivelNoGanado + " | Juego: " + juegoNoGanado;
        continuarDesdeNivel.SetActive(true);
    }

    /* -------------------------------------------------------------------------------- */

    public void manejarMenu() 
    {
        menuActivo = !menuActivo;

        if (menuActivo)
        {
            //Debug.Log("[ManejarMenu] Abriendo menu.");
            menu.SetActive(true);
            tweenManager.abrirMenu();
        }
        else 
        {
            //Debug.Log("[ManejarMenu] Cerrando menu.");
            tweenManager.cerrarMenu();
        }

        if (opcionesActivas) 
        {
            tweenManager.cerrarOpciones();
            opcionesActivas = false;
        }

        // Si es Juego1
        if (index < 11 && FindObjectOfType<Juego1>().start) activarTimer();

        // Si es Juego2
        if (index > 12 && FindObjectOfType<Juego2>().start)
        { 
            FindObjectOfType<Juego2>().pause = menuActivo;
            activarTimer();
        }
    }

    /* -------------------------------------------------------------------------------- */

    void activarTimer() { FindObjectOfType<Timer>().toggleClock(!menuActivo); }

    /* -------------------------------------------------------------------------------- */

    public void manejarOpciones()
    {
        opcionesActivas = !opcionesActivas;

        if (opcionesActivas) 
        { 
            tweenManager.abrirOpciones();
        }
        else
        { 
            tweenManager.cerrarOpciones();
        }
    }

    /* -------------------------------------------------------------------------------- */
    public void manejarCreditos() {

        creditosActivos = !creditosActivos;

        if (creditosActivos)
        {
            tweenManager.abrirCreditos();
        }
        else
        {
            tweenManager.cerrarCreditos();
        }
    }
}
