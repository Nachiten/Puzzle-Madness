using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Pixeles para canvas
    public float ReferencePixelUnit = 80;

    bool gano = false;

    // Boton "Comenzar"
    GameObject boton;
    Text textoBoton;

    /* -------------------------------------------------------------------------------- */

    void Start()
    {
        // Asignar variables
        boton = GameObject.Find("Boton");
        textoBoton = GameObject.Find("TextoBoton").GetComponent<Text>();

        // Modificar texto
        textoBoton.text = "Comenzar Nivel";

        GameObject.Find("Canvas").GetComponent<CanvasScaler>().referencePixelsPerUnit = ReferencePixelUnit;
    }

    /* -------------------------------------------------------------------------------- */

    public void ganoJuego() {
        gano = true;
        // Desactivar reloj
        GetComponent<Timer>().toggleClock(false);
        // Modificar texto

        if (SceneManager.GetActiveScene().buildIndex == 6) textoBoton.text = "Regresar a Inicio";
                                                      else textoBoton.text = "Siguiente Nivel";
        // Mostrar boton
        boton.SetActive(true);
    }

    /* -------------------------------------------------------------------------------- */

    public void comenzarNivel()
    {
        if (!gano) // Si es el inicio, comenzar el juego
        {
            Debug.Log("Iniciando juego...");
            // Ocultar boton
            boton.SetActive(false);
            // Activar juego
            FindObjectOfType<MovimientoBloques>().comenzarNivel();
            // Activar reloj
            FindObjectOfType<Timer>().toggleClock(true);
        }
        else if (SceneManager.GetActiveScene().buildIndex == 6 || SceneManager.GetActiveScene().buildIndex == 5) // Si es custom level o nivel 5, regresar a inicio
        {
            FindObjectOfType<LevelLoader>().cargarNivel(0);
        }
        else // En otros, pasar al siguiente nivel
        { 
            Debug.Log("Avanzando al siguiente nivel...");
            FindObjectOfType<LevelLoader>().cargarNivel(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
