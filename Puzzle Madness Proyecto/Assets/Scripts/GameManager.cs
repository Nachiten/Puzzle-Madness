using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;

public class GameManager : MonoBehaviour
{
    bool gano = false;

    // Boton "Comenzar"
    GameObject boton;
    Text textoBoton;

    public int filas = 3;
    public int columnas = 3;

    // Numero de nivel
    Text textoNivel;

    // Numero y nombre de escena actual
    int index;
    string name;

    /* -------------------------------------------------------------------------------- */

    void Start()
    {
        index = SceneManager.GetActiveScene().buildIndex;
        name = SceneManager.GetActiveScene().name;

        GameObject.Find("Nivel").GetComponent<Text>().text = name;

        // Asignar variables
        boton = GameObject.Find("Boton");
        textoBoton = GameObject.Find("TextoBoton").GetComponent<Text>();

        // Modificar texto
        textoBoton.text = "Comenzar Nivel";
    }

    /* -------------------------------------------------------------------------------- */

    public void ganoJuego()
    {
        // Variable gano
        gano = true;

        // Desactivar reloj
        GetComponent<Timer>().toggleClock(false);

        AnalyticsResult result = AnalyticsEvent.Custom("Ganado_" + index);
        Debug.Log("Analytics Result: " + result + " | DATA: " + "Ganado_" + index);

        PlayerPrefs.SetString(index.ToString(), "Ganado");
        Debug.Log(PlayerPrefs.GetString(index.ToString()));

        FindObjectOfType<Timer>().setPlayerPref();

        // Modificar texto
        if (index == 10 || index == 11 || index == 22) textoBoton.text = "Regresar a Inicio";
                                                  else textoBoton.text = "Siguiente Nivel";

        // Mostrar boton
        boton.SetActive(true);
    }

    /* -------------------------------------------------------------------------------- */

    public void comenzarNivel()
    {
        int index = SceneManager.GetActiveScene().buildIndex;

        if (!gano) // Si no gano, comenzar juego
        {
            Debug.Log("Iniciando juego...");
            // Ocultar boton
            boton.SetActive(false);

            // Si es juego1
            if (index < 12)
            FindObjectOfType<Juego1>().comenzarNivel();

            // Si es juego2
            else if (index > 12)
            FindObjectOfType<Juego2>().start = true;

            // Activar reloj
            FindObjectOfType<Timer>().toggleClock(true);
        }

        // Si es custom level o nivel 5, regresar a inicio
        else if (index == 10 || index == 11 || index == 22 || index == 23) FindObjectOfType<LevelLoader>().cargarNivel(0);
        
        // En otros, pasar al siguiente nivel
        else
        {
            Debug.Log("Avanzando al siguiente nivel...");
            FindObjectOfType<LevelLoader>().cargarNivel(index + 1);
        }
    }

    /* -------------------------------------------------------------------------------- */

    GameObject referencia;

    public void generarBloques()
    {
        GameObject referencia = GameObject.Find("_Reference");

        Debug.Log(referencia);

        int contador = 1;

        float offsetX = 0f;
        float offsetZ = 0f;

        for (int i = 0; i < filas; i++){
            for (int j = 0; j < columnas; j++) {

                if (contador < filas * columnas || SceneManager.GetActiveScene().buildIndex > 12)
                {
                    // Crear el clon
                    GameObject clon = Instantiate(Resources.Load("1", typeof(GameObject))) as GameObject;
                    // Asignar nombre correcto
                    clon.name = contador.ToString();
                    // Asignar posicion de clon
                    clon.transform.position = new Vector3(referencia.transform.position.x + offsetX, referencia.transform.position.y, referencia.transform.position.z + offsetZ);
                    // Asignar rotacion de clon
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

    /* -------------------------------------------------------------------------------- */

    Renderer modelo;

    // Ajustar posiciones y offsets de imagenes
    public void ajustarPosiciones()
    {
        modelo = GameObject.Find("Bloque Modelo").GetComponent<Renderer>();

        Renderer objeto;

        int contador;

        float scaleX = 1f / columnas;
        float scaleY = 1f / filas;

        float offsetX = 0f;
        float offsetY = scaleY * (filas * columnas - 1);
        contador = 1;

        for (int i = 0; i < filas; i++) {
            for (int j = 0; j < columnas; j++) {

                if (contador < filas * columnas || SceneManager.GetActiveScene().buildIndex > 12)
                {
                    // Asignar renderer
                    objeto = GameObject.Find(contador.ToString()).GetComponent<Renderer>();
                    // Cambiar "Tiling" de textura
                    objeto.material.mainTextureScale = new Vector2(scaleX, scaleY);
                    // Ajustar "Offeset" de textura
                    objeto.material.mainTextureOffset = new Vector2(offsetX, offsetY);
                    // Cambiar la textura al modelo
                    objeto.material.mainTexture = modelo.material.mainTexture;

                    contador++;
                }
                offsetX += scaleX;
            }
            offsetX = 0;
            offsetY -= scaleY;
        }
    }
}
