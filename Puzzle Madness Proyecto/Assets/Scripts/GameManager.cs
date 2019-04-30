using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
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
            if (SceneManager.GetActiveScene().buildIndex < 8 || SceneManager.GetActiveScene().buildIndex == 9)
            FindObjectOfType<MovimientoBloques>().comenzarNivel();

            // Si es juego2
            else if (SceneManager.GetActiveScene().buildIndex > 7 && SceneManager.GetActiveScene().buildIndex != 9)
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

    /* -------------------------------------------------------------------------------- */

    GameObject referencia;

    public void generarBloques() {

        int filas = GetComponent<MovimientoBloques>().filas;
        int columnas = GetComponent<MovimientoBloques>().columnas;

        GameObject referencia = GameObject.Find("_Reference");

        int contador = 1;

        float offsetX = 0f;
        float offsetZ = 0f;

        for (int i = 0; i < filas; i++)
        {
            for (int j = 0; j < columnas; j++)
            {
                if (contador < filas * columnas)
                {

                    GameObject clon = Instantiate(Resources.Load("1", typeof(GameObject))) as GameObject;

                    clon.name = contador.ToString();

                    clon.transform.position = new Vector3(referencia.transform.position.x + offsetX, referencia.transform.position.y, referencia.transform.position.z + offsetZ);

                    clon.transform.rotation = referencia.transform.rotation;

                }

                contador++;

                offsetX += 5f;
            }
            offsetX = 0f;
            offsetZ -= 5f;
        }

        Destroy(referencia);

    }
}
