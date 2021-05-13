using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    public int nivelACargar = 1;

    static PopUpsMenu codigoPopUpsMenu;
    static LevelLoader codigoLevelLoader;
    static ManejarMenu codigoManejarMenu;
    static SoundManager codigoSoundManager;

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

    // Setup que se hace por unica vez
    void Awake()
    {
        codigoSoundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        codigoPopUpsMenu = GameObject.Find("Pop Up").GetComponent<PopUpsMenu>();
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
        Debug.Log("[Buttons] SetupInicial");

        GameObject objetoGameManager = GameObject.Find("GameManager");

        codigoManejarMenu = objetoGameManager.GetComponent<ManejarMenu>();
        codigoLevelLoader = objetoGameManager.GetComponent<LevelLoader>();
    }

    #region Botones

    /* --------------------------------------------------------------------------------- */
    /* ------------------------------------ BOTONES ------------------------------------ */
    /* --------------------------------------------------------------------------------- */

    public void botonComenzar()
    {
        reproducirSonidoClickBoton();

        if (SceneManager.GetActiveScene().buildIndex == 0)
            loadLevel(nivelACargar);
        else
            codigoManejarMenu.manejarMenu();
    }

    public void botonOpciones()
    {
        reproducirSonidoClickBoton();

        codigoManejarMenu.manejarOpciones();
    }

    public void botonSalir()
    {
        reproducirSonidoClickBoton();

        codigoPopUpsMenu.abrirPopUp(3);
    }

    public void botonVolverAInicio()
    {
        reproducirSonidoClickBoton();

        loadLevel(0);
    }

    public void botonBorrarProgreso()
    {
        reproducirSonidoClickBoton();

        codigoPopUpsMenu.abrirPopUp(0);
    }

    public void botonCreditos()
    {
        reproducirSonidoClickBoton();

        codigoManejarMenu.manejarCreditos();
    }

    public void botonSeleccionarNivel()
    {
        reproducirSonidoClickBoton();

        loadLevel(12);
    }

    #endregion

    /* ------------------------------------------------------------------------------------ */
    /* ------------------------------------ AUXILIARES ------------------------------------ */
    /* ------------------------------------------------------------------------------------ */

    /* -------------------------------------------------------------------------------- */

    public void loadLevel(int index) { codigoLevelLoader.cargarNivel(index); }

    /* -------------------------------------------------------------------------------- */

    void reproducirSonidoClickBoton() { codigoSoundManager.reproducirSonido(1); }

    /* -------------------------------------------------------------------------------- */
}