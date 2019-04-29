using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Pixeles para canvas
    //public float ReferencePixelUnit = 80;

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
        Debug.Log("ENTROO");

        if (!gano) // Si no gano, comenzar juego
        {
            Debug.Log("Iniciando juego...");
            // Ocultar boton
            boton.SetActive(false);

            // Si es juego1
            if (SceneManager.GetActiveScene().buildIndex < 8)
            FindObjectOfType<MovimientoBloques>().comenzarNivel();

            // Si es juego2
            else if (SceneManager.GetActiveScene().buildIndex > 7)
            FindObjectOfType<DragAndDrop>().start = true;

            // Activar reloj
            FindObjectOfType<Timer>().toggleClock(true);
        }

        // Si es custom level o nivel 5, regresar a inicio
        else if (SceneManager.GetActiveScene().buildIndex == 6 || SceneManager.GetActiveScene().buildIndex == 5) 
        {
            FindObjectOfType<LevelLoader>().cargarNivel(0);
        }

        // En otros, pasar al siguiente nivel
        else
        { 
            Debug.Log("Avanzando al siguiente nivel...");
            FindObjectOfType<LevelLoader>().cargarNivel(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
