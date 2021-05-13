using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ManejarMenu : MonoBehaviour
{
    #region Variables

    // Flags varios
    bool menuActivo = false, opcionesActivas = false, creditosActivos = false;

    // Flag de ya asigne las variables
    static bool variablesAsignadas = false;

    // GameObjects
    static GameObject menu, opciones, creditos, panelMenu;

    // Manager de las animaciones
    static LeanTweenManager tweenManager;

    // Textos varios
    static TMP_Text textoBoton;

    // Strings utilizados
    string continuar = "CONTINUAR";

    // Index de escena actual
    int index;

    #endregion

    /* -------------------------------------------------------------------------------- */

    #region FuncionStart

    private void Awake()
    {
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
    }

    void Start()
    {
        index = SceneManager.GetActiveScene().buildIndex;

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
            menu.SetActive(true);
            menuActivo = true;
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
        if (index < 11 && FindObjectOfType<Juego1>().start) 
            activarTimer();

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
    public void manejarCreditos() 
    {
        creditosActivos = !creditosActivos;

        if (creditosActivos)
            tweenManager.abrirCreditos();
        
        else
            tweenManager.cerrarCreditos();
        
    }
}
