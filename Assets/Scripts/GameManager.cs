using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;

public class GameManager : MonoBehaviour
{
    // Juego ganado
    bool gano = false;

    // Boton "Comenzar"
    GameObject boton;
    Text textoBoton;

    // Cantidad de bloques
    public int filas = 3;
    public int columnas = 3;

    // Numero de nivel
    Text textoNivel;

    // Numero de escena actual
    int index;

    /* -------------------------------------------------------------------------------- */

    void Start()
    {
        index = SceneManager.GetActiveScene().buildIndex;
        string name = SceneManager.GetActiveScene().name;

        GameObject.Find("Nivel").GetComponent<Text>().text = name;

        // Asignar variables
        boton = GameObject.Find("Boton");
        textoBoton = GameObject.Find("TextoBoton").GetComponent<Text>();

        // Modificar texto
        textoBoton.text = "Comenzar Nivel";
    }

    /* -------------------------------------------------------------------------------- */
    
    public void comenzarJuego1(){
        // Asignar filas y columnas de Juego1
        filas    = FindObjectOfType<Juego1>().filas;        
        columnas = FindObjectOfType<Juego1>().columnas;
        
        // Generar bloques del mapa
        generarBloques();
        // Ajustar Texturas
        ajustarPosiciones();
        // Ajustar ubicacion de bloques
        ajustarUbicacion();
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

        // Si es custom level o nivel 10, regresar a inicio
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

    // Intanciar los bloques necesarios para el nivel
    public void generarBloques()
    {
        GameObject referencia = GameObject.Find("_Reference");

        //Debug.Log(referencia);

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
    Transform modeloTransform;

    // Ajustar posiciones y offsets de imagenes
    public void ajustarPosiciones()
    {
        // Textura de imagen modelo
        modelo = GameObject.Find("Bloque Modelo").GetComponent<Renderer>();

        // Transform de iamgen modelo
        modeloTransform = GameObject.Find("Bloque Modelo").GetComponent<Transform>();

        Debug.Log("Posicion Antes | " + modeloTransform.position.ToString());
        
        index = SceneManager.GetActiveScene().buildIndex;
        modeloTransform.position = new Vector3(modeloTransform.position.x - (index - 1) * 1.5111f, modeloTransform.position.y - (index - 1) * 2f, modeloTransform.position.z);

        Debug.Log("Posicion Despues | " + modeloTransform.position.ToString());
        Debug.Log("Index | " + index.ToString());

        // Ajustar tamaño de imagen modelo a nivel actual
        modeloTransform.localScale = new Vector3(1.5f * columnas, modeloTransform.localScale.y , 1.5f * filas );

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

    Transform referenciaAjuste;
    Transform plataforma;

    // Determinar posicion correcta de juego
    public void ajustarUbicacion()
    {
        if (index < 12)
        { 
            plataforma = GameObject.Find("Piso Mapa").GetComponent<Transform>();
            plataforma.localScale = new Vector3((5 * columnas) + 2, plataforma.localScale.y, (5 * filas) + 2);
        }

        int contador = 1;
        Transform objeto;

        float offsetX = 5;
        float offsetZ = 0;

        float posX = 0;
        float posZ = 0;

        for (int i = 0; i < filas; i++)
        {
            for (int j = 0; j < columnas; j++)
            {
                if (i == 0 && j == 0)
                {
                    // Primer bloque es la referencia
                    referenciaAjuste = GameObject.Find(contador.ToString()).GetComponent<Transform>();

                    posX = referenciaAjuste.position.x;
                    posZ = referenciaAjuste.position.z;

                    determinarPos(ref posX, ref posZ);
                    referenciaAjuste.position = new Vector3(posX, referenciaAjuste.position.y, posZ);
                }
                else if (!(i == filas - 1 && j == columnas - 1))
                {
                    objeto = GameObject.Find(contador.ToString()).GetComponent<Transform>();

                    objeto.position = new Vector3(referenciaAjuste.position.x + offsetX, objeto.position.y, referenciaAjuste.position.z + offsetZ);

                    offsetX += 5;
                }
                contador++;
            }
            offsetX = 0;
            offsetZ -= 5;
        }
    }

    int mayor;

    // Hijo de ajustarUbicacion
    void determinarPos(ref float posicionX, ref float posicionZ)
    {
        if (filas > columnas) mayor = filas;
        else mayor = columnas;

        float valorX = (columnas - 3) * 2.5f;
        float valorZ = (filas - 3) * 2.5f;

        posicionX -= valorX;
        posicionZ += valorZ;

        float offset = (mayor - 3) * 2.5f;

        Transform modelo = GameObject.Find("Modelo").GetComponent<Transform>();
        modelo.position = new Vector3(modelo.position.x, modelo.position.y + offset, modelo.position.z);

        Transform camara = GameObject.Find("Main Camera").GetComponent<Transform>();
        camara.position = new Vector3(camara.position.x, camara.position.y + offset, camara.position.z);

        //Debug.Log("VALORX: " + valorX + " | VALOR Z:" + valorZ);
    }
}
