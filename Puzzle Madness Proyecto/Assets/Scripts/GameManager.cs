using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float ReferencePixelUnit = 80;

    bool gano = false;

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

        if (SceneManager.GetActiveScene().buildIndex == 4) textoBoton.text = "Regresar a Inicio";
                                                      else textoBoton.text = "Siguiente Nivel";
        
        // Mostrar boton
        boton.SetActive(true);
    }

    /* -------------------------------------------------------------------------------- */

    public void comenzarNivel()
    {
        // Si el juego todavia no comenzo, comenzarlo
        if (!gano)
        {
            Debug.Log("Iniciando juego...");
            // Ocultar boton
            boton.SetActive(false);
            // Activar juego
            FindObjectOfType<MovimientoBloques>().comenzarNivel();
            // Activar reloj
            FindObjectOfType<Timer>().toggleClock(true);
        }

        // Si el juego termino, pasar al siguiente nivel
        else if (SceneManager.GetActiveScene().buildIndex == 4)
        {
            FindObjectOfType<LevelLoader>().cargarNivel(0);
        }
        else
        {
            Debug.Log("Avanzando al siguiente nivel...");
            FindObjectOfType<LevelLoader>().cargarNivel(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
