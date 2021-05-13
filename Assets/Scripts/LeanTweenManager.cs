 using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LeanTweenManager : MonoBehaviour
{
    #region Variables

    float tiempoAnimacionBotonesMenu = 0.2f, tiempoAnimacionPanelMenu = 0.15f, tiempoAnimacionMenus = 0.5f, posicionAfuera = 1920;

    List<GameObject> botones;

    static GameObject menu, menuPanel, menuOpciones, menuCreditos, botonesInicio;
    static GameObject botonComenzar, botonSeleccionarNivel, botonOpciones, botonSalir, botonVolverInicio, botonBorrarProgreso, botonCreditos;

    public bool animacionEnEjecucion = false;

    static bool variablesSeteadas = false;
    static int indexActual = -1;

    #endregion

    /* -------------------------------------------------------------------------------- */

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /* -------------------------------------------------------------------------------- */

    // Setup que se hace una sola vez
    void Awake()
    {
        if (variablesSeteadas)
            return;

        // Objetos
        menu = GameObject.Find("Menu");
        menuPanel = GameObject.Find("PanelMenu");
        menuOpciones = GameObject.Find("MenuOpciones");
        menuCreditos = GameObject.Find("MenuCreditos");

        // Botones
        botonSalir = GameObject.Find("Salir");
        botonComenzar = GameObject.Find("Comenzar");
        botonOpciones = GameObject.Find("Opciones");
        botonesInicio = GameObject.Find("Botones Inicio");

        // Botonces condicionales
        botonVolverInicio = GameObject.Find("VolverAInicio");
        botonSeleccionarNivel = GameObject.Find("Seleccionar Nivel");

        variablesSeteadas = true;
    }

    /* -------------------------------------------------------------------------------- */

    // Se llama cuando una nueva escena se carga
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        setupInicial();
    }

    /* -------------------------------------------------------------------------------- */

    // Setup que se hace en cada nueva escena cargada
    void setupInicial()
    {
        Debug.Log("[LeanTweenManager] SetupInicial");

        indexActual = SceneManager.GetActiveScene().buildIndex;

        botones = new List<GameObject> { botonSalir, botonComenzar, botonOpciones };

        // Si estoy en inicio
        if (indexActual == 0)
        {
            botonVolverInicio.SetActive(false);
            botonesInicio.SetActive(true);
        }
        else
        {
            botones.Add(botonVolverInicio);
            botones.Add(botonSeleccionarNivel);
            botonesInicio.SetActive(false);
        }
    }

    #region AnimacionAbrirMenu

    /* --------------------------------------------------------------------------------- */
    // ----------------------------- ANIMACION ARBRIR MENU ----------------------------- //
    /* --------------------------------------------------------------------------------- */

    public void abrirMenu()
    {
        menuPanel.SetActive(false);

        foreach (GameObject boton in botones) {
            boton.SetActive(false);
        }

        animacionEnEjecucion = true;

        // Posicion inicial
        LeanTween.scale(menuPanel, new Vector3(0, 0, 1), 0f).setOnComplete(abrirPanel);
    }

    void abrirPanel()
    {
        menuPanel.SetActive(true);
        LeanTween.scale(menuPanel, new Vector3(1, 1, 1), tiempoAnimacionPanelMenu).setOnComplete(abrirBotones);
    }

    void abrirBotones()
    {
        int cantidadBotones = botones.Count;

        for (int i = 0; i < cantidadBotones; i++)
        {
            GameObject boton = botones[i];

            bool terminarAnimacion = i == cantidadBotones - 1;

            // Posiciones iniciales
            LeanTween.scale(boton, new Vector3(0, 0.2f, 1), 0f).setOnComplete(_ => abrirBotonEnX(boton, terminarAnimacion));
        }
    }

    void abrirBotonEnX(GameObject boton, bool terminarAnimacion)
    {
        boton.SetActive(true);
        LeanTween.scaleX(boton, 2.3f, tiempoAnimacionBotonesMenu).setOnComplete(_ => abrirBotonEnY(boton, terminarAnimacion));
    }

    void abrirBotonEnY(GameObject unBoton, bool terminarAnimacion)
    {
        if (terminarAnimacion)
            LeanTween.scaleY(unBoton, 3.1f, tiempoAnimacionBotonesMenu).setOnComplete(terminarAnimacionAbrir);
        else
            LeanTween.scaleY(unBoton, 3.1f, tiempoAnimacionBotonesMenu);
    }

    void terminarAnimacionAbrir()
    {
        animacionEnEjecucion = false;
    }
    
    #endregion

    #region AnimacionCerrarMenu

    /* --------------------------------------------------------------------------------- */
    // ----------------------------- ANIMACION CERRAR MENU ----------------------------- // 
    /* --------------------------------------------------------------------------------- */

    public void cerrarMenu()
    {
        animacionEnEjecucion = true;

        int cantidadBotones = botones.Count;

        for (int i = 0; i < cantidadBotones; i++)
        {
            GameObject botonActual = botones[i];

            bool cerrarMenu = i == cantidadBotones - 1;

            // Posiciones iniciales
            LeanTween.scale(botonActual, new Vector3(2.3f, 3.1f, 1), 0f).setOnComplete(_ => cerrarBotonEnY(botonActual, cerrarMenu));
        }

    }

    void cerrarBotonEnY(GameObject boton, bool cerrarMenu) {
        LeanTween.scaleY(boton, 0.2f, tiempoAnimacionBotonesMenu).setOnComplete(_ => cerrarBotonEnX(boton, cerrarMenu));
    }

    void cerrarBotonEnX(GameObject unBoton, bool cerrarMenu)
    {
        if (cerrarMenu)
            LeanTween.scaleX(unBoton, 0f, tiempoAnimacionBotonesMenu).setOnComplete(cerrarPanel);
        else
            LeanTween.scaleX(unBoton, 0f, tiempoAnimacionBotonesMenu);
    }

    void cerrarPanel()
    {
        // Posicion inicial
        LeanTween.scale(menuPanel, new Vector3(1, 1, 1), 0f);

        LeanTween.scale(menuPanel, new Vector3(0, 0, 1), tiempoAnimacionPanelMenu).setOnComplete(terminarAnimacionCerrar);
    }

    void terminarAnimacionCerrar()
    {
        animacionEnEjecucion = false;
        menu.SetActive(false);
    }

    #endregion

    #region AnimacionesAbrirMenus

    /* ------------------------------------------------------------------------------------- */
    // ------------------------------ ANIMACIONES ABRIR MENUS ------------------------------ // 
    /* ------------------------------------------------------------------------------------- */

    public void abrirMenu(GameObject menuAPoner, int signo)
    {
        animacionEnEjecucion = true;

        if (indexActual == 0)
        {
            ocultarContinuarDesdeNivel();
            LeanTween.moveLocalX(botonesInicio, 0f, 0f).setOnComplete(_ => quitarBotonesInicio(posicionAfuera * signo));
        }

        // Posicion Inicial
        LeanTween.moveLocalX(menu, 0f, 0f).setOnComplete(_ => quitarMenu(posicionAfuera * signo));
        LeanTween.moveLocalX(menuAPoner, -posicionAfuera * signo, 0f).setOnComplete(_ => ponerMenu(menuAPoner));
    }

    void quitarBotonesInicio(float posicion)
    {
        LeanTween.moveLocalX(botonesInicio, posicion, tiempoAnimacionMenus).setOnComplete(ocultarBotonesInicio);
    }

    void ocultarBotonesInicio()
    {
        botonesInicio.SetActive(false);
    }

    void quitarMenu(float posicion)
    {
        LeanTween.moveLocalX(menu, posicion, tiempoAnimacionMenus).setOnComplete(ocultarMenu);
    }

    void ocultarMenu()
    {
        menu.SetActive(false);
        animacionEnEjecucion = false;
    }

    void ponerMenu(GameObject menuAPoner)
    {
        menuAPoner.SetActive(true);
        LeanTween.moveLocalX(menuAPoner, 0f, tiempoAnimacionMenus);
    }

    public void abrirOpciones()
    {
        abrirMenu(menuOpciones, 1);
    }

    public void abrirCreditos()
    {
        abrirMenu(menuCreditos, -1);
    }

    #endregion

    #region AnimacionesCerrarMenus

    /* -------------------------------------------------------------------------------------- */
    // ------------------------------ ANIMACIONES CERRAR MENUS ------------------------------ // 
    /* -------------------------------------------------------------------------------------- */

    public void cerrarMenu(GameObject menuACerrar, int signo)
    {
        animacionEnEjecucion = true;

        indexActual = SceneManager.GetActiveScene().buildIndex;

        if (indexActual == 0)
        {
            LeanTween.moveLocalX(botonesInicio, posicionAfuera * signo, 0f).setOnComplete(ponerBotonesInicio);
        }

        // Posicion Inicial
        LeanTween.moveLocalX(menu, posicionAfuera * signo, 0f).setOnComplete(ponerMenu);
        LeanTween.moveLocalX(menuACerrar, 0, 0f).setOnComplete(_ => quitarMenuOtro(menuACerrar, -posicionAfuera * signo));
    }

    void ponerBotonesInicio()
    {
        botonesInicio.SetActive(true);
        LeanTween.moveLocalX(botonesInicio, 0, tiempoAnimacionMenus);
    }

    void ponerMenu()
    {
        menu.SetActive(true);
        LeanTween.moveLocalX(menu, 0, tiempoAnimacionMenus).setOnComplete(ocultarContinuarDesdeNivel);
    }

    void quitarMenuOtro(GameObject menuAQuitar, float posicion)
    {
        LeanTween.moveLocalX(menuAQuitar, posicion, tiempoAnimacionMenus).setOnComplete(_ => ocultarMenuOtro(menuAQuitar));
    }

    void ocultarMenuOtro(GameObject menuAQuitar)
    {
        menuAQuitar.SetActive(false);
        animacionEnEjecucion = false;
    }

    public void cerrarOpciones()
    {
        cerrarMenu(menuOpciones, 1);
    }

    public void cerrarCreditos()
    {
        cerrarMenu(menuCreditos, -1);
    }

    void ocultarContinuarDesdeNivel()
    {
        if (indexActual == 0)
            FindObjectOfType<ContinuarDesdeNivel>().ocultarContinuarDesdeNivelSiCorresponde();
    }

    #endregion
}