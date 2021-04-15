using UnityEngine;
using UnityEngine.SceneManagement;

public class LeanTweenManager : MonoBehaviour
{
    #region Variables

    float tiempoAnimacionBotonesMenu = 0.2f, tiempoAnimacionPanelMenu = 0.15f, tiempoAnimacionOpciones = 0.5f; // 0.3, 0.2

    public GameObject[] botones;
    GameObject menu, panel, opciones, creditos, botonesInicio;

    public bool animacionEnEjecucion = false;

    int index;

    #endregion

    /* -------------------------------------------------------------------------------- */

    #region FuncionStart

    // Start is called before the first frame update
    void Start()
    {
        index = SceneManager.GetActiveScene().buildIndex;

        menu = GameObject.Find("Menu");
        panel = GameObject.Find("PanelMenu");
        opciones = GameObject.Find("MenuOpciones");
        creditos = GameObject.Find("MenuCreditos");
        botonesInicio = GameObject.Find("Botones Inicio");

        botones = new GameObject[]{
            GameObject.Find("Comenzar"),
            GameObject.Find("Seleccionar Nivel"),
            GameObject.Find("Opciones"),
            GameObject.Find("Salir"),
        };
    }

    #endregion

    #region AnimacionAbrirMenu

    /* --------------------------------------------------------------------------------- */
    // ----------------------------- ANIMACION ARBRIR MENU ----------------------------- //
    /* --------------------------------------------------------------------------------- */

    public void abrirMenu()
    {
        panel.SetActive(false);

        foreach (GameObject boton in botones) {
            boton.SetActive(false);
        }

        animacionEnEjecucion = true;
        //Debug.Log("[LeanTweenManager] Inicio Animacion [AbrirMenu]");

        // Posicion inicial
        LeanTween.scale(panel, new Vector3(0, 0, 1), 0f).setOnComplete(abrirPanel);
    }

    void abrirPanel()
    {
        panel.SetActive(true);
        LeanTween.scale(panel, new Vector3(1, 1, 1), tiempoAnimacionPanelMenu).setOnComplete(abrirBotones);
    }

    void abrirBotones()
    {
        for (int i = 0; i < botones.Length; i++)
        {
            GameObject boton = botones[i];

            bool terminarAnimacion = i == botones.Length - 1;

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
        //Debug.Log("[LeanTweenManager] Termino Animacion [AbrirMenu]");
    }
    

    #endregion

    #region AnimacionCerrarMenu

    /* --------------------------------------------------------------------------------- */
    // ----------------------------- ANIMACION CERRAR MENU ----------------------------- // 
    /* --------------------------------------------------------------------------------- */

    public void cerrarMenu()
    {
        animacionEnEjecucion = true;
        //Debug.Log("[LeanTweenManager] Inicio Animacion [CerrarMenu]");

        for (int i = 0; i < botones.Length; i++)
        {
            GameObject botonActual = botones[i];

            bool cerrarMenu = i == botones.Length - 1;

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
        LeanTween.scale(panel, new Vector3(1, 1, 1), 0f);

        LeanTween.scale(panel, new Vector3(0, 0, 1), tiempoAnimacionPanelMenu).setOnComplete(terminarAnimacionCerrar);
    }

    void terminarAnimacionCerrar()
    {
        //Debug.Log("[LeanTweenManager] Termino Animacion [CerrarMenu]");
        animacionEnEjecucion = false;
        menu.SetActive(false);
    }

    #endregion

    #region AnimacionAbrirOpcionesMenu

    /* ----------------------------------------------------------------------------------------- */
    // ----------------------------- ANIMACION ABRIR OPCIONES MENU ----------------------------- // 
    /* ----------------------------------------------------------------------------------------- */

    float posicionAfuera = 1920;

    public void abrirOpciones()
    {
        //Debug.Log("[LeanTweenManager] Inicio Animacion [AbrirOpciones]");

        ocultarContinuarDesdeNivelSiCorresponde();

        index = SceneManager.GetActiveScene().buildIndex;

        if (index == 0) {
            LeanTween.moveLocalX(botonesInicio, 0f, 0f).setOnComplete(_ => quitarBotonesInicio(posicionAfuera));
        }

        // Posicion Inicial
        LeanTween.moveLocalX(menu, 0f, 0f).setOnComplete(_ => quitarMenu(posicionAfuera));
        LeanTween.moveLocalX(opciones, -posicionAfuera, 0f).setOnComplete(ponerOpciones);

    }

    void quitarMenu(float posicion)
    {
        LeanTween.moveLocalX(menu, posicion, tiempoAnimacionOpciones).setOnComplete(ocultarMenu);
    }

    void ocultarMenu()
    {
        menu.SetActive(false);
    }

    void ponerOpciones()
    {
        opciones.SetActive(true);
        LeanTween.moveLocalX(opciones, 0f, tiempoAnimacionOpciones);
    }

    void quitarBotonesInicio(float posicion)
    {
        LeanTween.moveLocalX(botonesInicio, posicion, tiempoAnimacionOpciones).setOnComplete(ocultarBotonesInicio);
    }

    void ocultarBotonesInicio()
    {
        botonesInicio.SetActive(false);
        //Debug.Log("[LeanTweenManager] Termino Animacion [AbrirOpciones]");
    }

    #endregion

    #region AnimacionCerrarOpcionesMenu

    /* ------------------------------------------------------------------------------------------ */
    // ----------------------------- ANIMACION CERRAR OPCIONES MENU ----------------------------- // 
    /* ------------------------------------------------------------------------------------------ */

    public void cerrarOpciones() 
    {
        //Debug.Log("[LeanTweenManager] Inicio Animacion [CerrarOpciones]");

        index = SceneManager.GetActiveScene().buildIndex;

        if (index == 0)
        {
            LeanTween.moveLocalX(botonesInicio, posicionAfuera, 0f).setOnComplete(ponerBotonesInicio);
        }

        // Posicion Inicial
        LeanTween.moveLocalX(menu, posicionAfuera, 0f).setOnComplete(ponerMenu);
        LeanTween.moveLocalX(opciones, 0, 0f).setOnComplete(quitarOpciones);
    }

    void ponerBotonesInicio() 
    {
        botonesInicio.SetActive(true);
        LeanTween.moveLocalX(botonesInicio, 0, tiempoAnimacionOpciones);
    }

    void ponerMenu() 
    {
        menu.SetActive(true);
        LeanTween.moveLocalX(menu, 0, tiempoAnimacionOpciones).setOnComplete(ocultarContinuarDesdeNivelSiCorresponde);
    }

    void ocultarContinuarDesdeNivelSiCorresponde() 
    {
        FindObjectOfType<ManejarMenu>().ocultarContinuarDesdeNivelSiCorresponde();
    }

    void quitarOpciones() 
    {
        LeanTween.moveLocalX(opciones, -posicionAfuera, tiempoAnimacionOpciones).setOnComplete(ocultarOpciones);
    }

    void ocultarOpciones() 
    {
        opciones.SetActive(false);
        //Debug.Log("[LeanTweenManager] Termino Animacion [CerrarOpciones]");
    }

    #endregion

    #region AnimacionAbrirCreditos

    /* ------------------------------------------------------------------------------------ */
    // ----------------------------- ANIMACION ABRIR CREDITOS ----------------------------- // 
    /* ------------------------------------------------------------------------------------ */

    public void abrirCreditos() 
    {
        //Debug.Log("[LeanTweenManager] Inicio Animacion [AbrirCreditos]");

        ocultarContinuarDesdeNivelSiCorresponde();

        index = SceneManager.GetActiveScene().buildIndex;

        if (index == 0)
        {
            LeanTween.moveLocalX(botonesInicio, 0f, 0f).setOnComplete(_ => quitarBotonesInicio(-posicionAfuera));
        }

        // Posicion Inicial
        LeanTween.moveLocalX(menu, 0f, 0f).setOnComplete(_ => quitarMenu(-posicionAfuera));
        LeanTween.moveLocalX(creditos, posicionAfuera, 0f).setOnComplete(ponerCreditos);
    }

    void ponerCreditos() 
    {
        creditos.SetActive(true);
        LeanTween.moveLocalX(creditos, 0f, tiempoAnimacionOpciones);
    }

    #endregion

    #region AnimacionCerrarCreditos

    /* ------------------------------------------------------------------------------------- */
    // ----------------------------- ANIMACION CERRAR CREDITOS ----------------------------- // 
    /* ------------------------------------------------------------------------------------- */

    public void cerrarCreditos() 
    {
        index = SceneManager.GetActiveScene().buildIndex;

        if (index == 0)
        {
            LeanTween.moveLocalX(botonesInicio, -posicionAfuera, 0f).setOnComplete(ponerBotonesInicio);
        }

        // Posicion Inicial
        LeanTween.moveLocalX(menu, -posicionAfuera, 0f).setOnComplete(ponerMenu);
        LeanTween.moveLocalX(opciones, 0, 0f).setOnComplete(quitarCreditos);
    }

    void quitarCreditos()
    {
        LeanTween.moveLocalX(creditos, posicionAfuera, tiempoAnimacionOpciones).setOnComplete(ocultarCreditos);
    }

    void ocultarCreditos()
    {
        creditos.SetActive(false);
    }

    #endregion
}