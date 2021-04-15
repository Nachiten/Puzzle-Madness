using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class ManejarMenu : MonoBehaviour
{
    // Flag de menu abierto
    bool menuActivo = true, opcionesActivas = false, mostrandoContinuarDesdeNivel = false;

    // Flag de ya asigne las variables
    static bool variablesAsignadas = false;

    // Menu pausa
    static GameObject menu, opciones, continuarDesdeNivel;
    static LeanTweenManager tweenManager;
    
    // Boton Continuar/Comenzar
    static Text textoBoton;

    static TMP_Text textoNivelNoGanado;

    // Index de escena actual
    int index;

    /* -------------------------------------------------------------------------------- */

    void Start()
    {
        index = SceneManager.GetActiveScene().buildIndex;
       
        if (menuActivo)
        {
            if (!variablesAsignadas)
            {
                menu = GameObject.Find("Menu");
                opciones = GameObject.Find("MenuOpciones");
                continuarDesdeNivel = GameObject.Find("ContinuarDesdeNivel");

                textoBoton = GameObject.Find("TextoBotonComenzar").GetComponent<Text>();
                textoNivelNoGanado = GameObject.Find("TextoContinuar").GetComponent<TMP_Text>();

                tweenManager = GameObject.Find("Canvas Menu").GetComponent<LeanTweenManager>();
                
                variablesAsignadas = true;
            }

            menuActivo = false;
        }

        if (index != 0)
        {
            textoBoton.text = "Continuar";
            tweenManager.cerrarMenu();
            opciones.SetActive(false);
        }
        else 
        {
            textoBoton.text = "Comenzar";
            menu.SetActive(true);

            if (yaJugoAntes())
            {
                textoBoton.text = "Continuar";
                mostrarUltimoNivelNoGanado();
                mostrandoContinuarDesdeNivel = true;
            }
            else
            {
                continuarDesdeNivel.SetActive(false);
            }
        }

        opciones.SetActive(false);

        
    }

    bool yaJugoAntes() {
        if (PlayerPrefs.GetInt("YaJugoAntes") == 1)
        {
            return true;
        }
        else {
            PlayerPrefs.SetInt("YaJugoAntes", 1);
            return false;
        }
    }

    public void ocultarContinuarDesdeNivelSiCorresponde() 
    {
        if (mostrandoContinuarDesdeNivel) 
        {
            bool continuarDesdeNivelActivo = continuarDesdeNivel.activeSelf;

            continuarDesdeNivel.SetActive(!continuarDesdeNivelActivo);
        }
    }

    /* -------------------------------------------------------------------------------- */

    void Update()
    {
        index = SceneManager.GetActiveScene().buildIndex;

        if (index == 0) return;

        bool animacionEnEjecucion = GameObject.Find("Canvas Menu").GetComponent<LeanTweenManager>().animacionEnEjecucion;

        if (Input.GetKeyDown("escape") && !animacionEnEjecucion) 
            manejarMenu();
    }

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

        Debug.Log("[ManejarMenu] Ultimo nivel no ganado. Nivel: " + nivelNoGanado + " | Juego: " + juegoNoGanado);

        textoNivelNoGanado.text = "Nivel: " + nivelNoGanado + " | Juego: " + juegoNoGanado;

        continuarDesdeNivel.SetActive(true);
    }

    /* -------------------------------------------------------------------------------- */

    public void manejarMenu() 
    {
        menuActivo = !menuActivo;

        if (menuActivo)
        {
            menu.SetActive(true);
            tweenManager.abrirMenu();
        }
        else 
        {
            tweenManager.cerrarMenu();
        }

        if (opcionesActivas) {
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
}
