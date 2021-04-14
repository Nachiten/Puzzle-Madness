using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ManejarMenu : MonoBehaviour
{
    // Flag de menu abierto
    public bool menuActivo = true, opcionesActivas = false;

    // Flag de ya asigne las variables
    static bool variablesAsignadas = false;

    // Menu pausa
    static GameObject menu, opciones, botonesInicio;
    static LeanTweenManager tweenManager;
    
    // Boton Continuar/Comenzar
    static Text textoBoton;

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

                textoBoton = GameObject.Find("TextoBotonComenzar").GetComponent<Text>();
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
            botonesInicio = GameObject.Find("Botones Inicio");
            textoBoton.text = "Comenzar";
            menu.SetActive(true);

            mostrarUltimoNivelNoGanado();
        }

        opciones.SetActive(false);
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

    void mostrarUltimoNivelNoGanado() {


        // 1 - 22

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

        int numeroJuego;
        int nivelNoGanado;

        if (indexNoGanado < 11)
        {
            numeroJuego = 1;
            nivelNoGanado = indexNoGanado;
        }
        else if (indexNoGanado < 23)
        {
            numeroJuego = 2;
            nivelNoGanado = indexNoGanado - 12;
        }
        else {
            numeroJuego = 0;
            nivelNoGanado = 0;
        }

        Debug.Log("Numero Juego: " + numeroJuego);
        Debug.Log("Nivel no ganado: " + nivelNoGanado);

        
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
            Debug.Log("Abriendo Opciones");
            tweenManager.abrirOpciones();
        }
        else
        { 
            Debug.Log("Cerrando Opciones");
            tweenManager.cerrarOpciones();
        }
    }
}
