using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;
using TMPro;

public class GameManager : MonoBehaviour
{
    #region Variables

    // Juego ganado
    bool gano = false;

    // Boton "Comenzar"
    GameObject boton;
    TMP_Text textoBoton, textoNivel;

    // Cantidad de bloques
    public int filas = 3, columnas = 3;

    // Numero de escena actual
    int index;

    string comenzarNivelTexto = "COMENZAR NIVEL", regresarAInicio = "REGRESAR A INICIO", siguienteNivel = "SIGUIENTE NIVEL";

    #endregion

    /* -------------------------------------------------------------------------------- */

    #region FuncionStart

    void Start()
    {
        index = SceneManager.GetActiveScene().buildIndex;
        string nombreNivel = SceneManager.GetActiveScene().name;

        textoNivel = GameObject.Find("Nivel").GetComponent<TMP_Text>();
        textoNivel.text = nombreNivel;

        // Asignar variables
        boton = GameObject.Find("Boton");
        textoBoton = GameObject.Find("TextoBoton").GetComponent<TMP_Text>();

        // Modificar texto
        textoBoton.text = comenzarNivelTexto;
    }

    #endregion

    /* -------------------------------------------------------------------------------- */

    public void comenzarJuego1()
    {
        // Asignar filas y columnas de Juego1
        filas = FindObjectOfType<Juego1>().filas;
        columnas = FindObjectOfType<Juego1>().columnas;

        realizarComienzoDeNivel();
    }

    /* -------------------------------------------------------------------------------- */

    public void comenzarJuego2()
    {
        // Asignar filas y columnas de Juego1
        filas = FindObjectOfType<Juego2>().filas;
        columnas = FindObjectOfType<Juego2>().columnas;

        realizarComienzoDeNivel();
    }

    /* -------------------------------------------------------------------------------- */

    void realizarComienzoDeNivel()
    {
        // Instanciar todos los bloques necesarios para el nivel
        generarBloques();

        // Ajusta la posicion del modelo
        ajustarPosicionModelo();

        // Ajusta las texturas de los bloques
        ajustarTexturasBloques();

        // Ajustar ubicacion de bloques
        ajustarPosicionBloques();
    }

    /* -------------------------------------------------------------------------------- */

    public void generarBloques()
    {
        Debug.Log("[GameManager] Generando Bloques...");

        GameObject referencia = GameObject.Find("_Reference");

        int contador = 1;

        float offsetX = 0f;
        float offsetZ = 0f;

        for (int i = 0; i < filas; i++)
        {
            for (int j = 0; j < columnas; j++)
            {

                // En el juego 1 no genero el ultimo bloque, en el juego2 si
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

    void ajustarTexturasBloques()
    {
        Renderer objeto;

        float scaleX = 1f / columnas;
        float scaleY = 1f / filas;

        float offsetX = 0f;
        float offsetY = scaleY * (filas * columnas - 1);

        int contador = 1;

        for (int i = 0; i < filas; i++)
        {
            for (int j = 0; j < columnas; j++)
            {

                if (contador < filas * columnas || SceneManager.GetActiveScene().buildIndex > 12)
                {
                    // Asignar renderer
                    objeto = GameObject.Find(contador.ToString()).GetComponent<Renderer>();

                    Mesh mesh = objeto.GetComponent<MeshFilter>().mesh;
                    Vector2[] UVs = new Vector2[mesh.vertices.Length];

                    // Cambiar la textura al modelo
                    objeto.material.mainTexture = modelo.material.mainTexture;

                    index = SceneManager.GetActiveScene().buildIndex;

                    // Si es un custom level, usar forma standard de poner textura
                    if (index == 11 || index == 23)
                    {
                        // Cambiar "Tiling" de textura
                        objeto.material.mainTextureScale = new Vector2(scaleX, scaleY);
                        // Ajustar "Offeset" de textura
                        objeto.material.mainTextureOffset = new Vector2(offsetX, offsetY);
                    }
                    // Si es Juego1 o Juego2, usar los bordes negros
                    else
                    {
                        float xMin = 0.334f + 0.33333f / columnas * j;
                        float yMin = (0.33333f / filas) * (filas - 1) - 0.33333f / filas * i;

                        float xMax = xMin + 0.33333f / columnas;
                        float yMax = yMin + 0.33333f / filas;

                        // Top
                        UVs[4] = new Vector2(xMin, yMax);
                        UVs[5] = new Vector2(xMax, yMax);
                        UVs[8] = new Vector2(xMin, yMin);
                        UVs[9] = new Vector2(xMax, yMin);

                        // Front
                        UVs[0] = new Vector2(0.0f, 0.0f);
                        UVs[1] = new Vector2(0.333f, 0.0f);
                        UVs[2] = new Vector2(0.0f, 0.333f);
                        UVs[3] = new Vector2(0.333f, 0.333f);

                        // Back
                        UVs[6] = new Vector2(1.0f, 0.0f);
                        UVs[7] = new Vector2(0.667f, 0.0f);
                        UVs[10] = new Vector2(1.0f, 0.333f);
                        UVs[11] = new Vector2(0.667f, 0.333f);

                        // Bottom
                        UVs[12] = new Vector2(0.0f, 0.334f);
                        UVs[13] = new Vector2(0.0f, 0.666f);
                        UVs[14] = new Vector2(0.333f, 0.666f);
                        UVs[15] = new Vector2(0.333f, 0.334f);

                        // Left
                        UVs[16] = new Vector2(0.334f, 0.334f);
                        UVs[17] = new Vector2(0.334f, 0.666f);
                        UVs[18] = new Vector2(0.666f, 0.666f);
                        UVs[19] = new Vector2(0.666f, 0.334f);

                        // Right        
                        UVs[20] = new Vector2(0.667f, 0.334f);
                        UVs[21] = new Vector2(0.667f, 0.666f);
                        UVs[22] = new Vector2(1.0f, 0.666f);
                        UVs[23] = new Vector2(1.0f, 0.334f);
                        mesh.uv = UVs;
                    }
                    contador++;
                }
                offsetX += scaleX;
            }
            offsetX = 0;
            offsetY -= scaleY;
        }
    }

    /* -------------------------------------------------------------------------------- */

    Renderer modelo;

    public void ajustarPosicionModelo()
    {
        index = SceneManager.GetActiveScene().buildIndex;

        // Textura de imagen modelo
        modelo = GameObject.Find("Bloque Modelo").GetComponent<Renderer>();

        // Transform de iamgen modelo
        Transform modeloTransform = GameObject.Find("Bloque Modelo").GetComponent<Transform>();

        if (index < 11)
        {
            modeloTransform.position = new Vector3(modeloTransform.position.x - (index - 1) * 1.5111f, modeloTransform.position.y - (index - 1) * 2f, modeloTransform.position.z);

            // Ajustar tamaño de imagen modelo a nivel actual
            modeloTransform.localScale = new Vector3(1.5f * columnas, modeloTransform.localScale.y, 1.5f * filas);
        }
    }

    /* -------------------------------------------------------------------------------- */

    Transform referenciaAjuste;

    public void ajustarPosicionBloques()
    {
        if (index < 12)
        {
            Transform plataforma = GameObject.Find("Piso Mapa").GetComponent<Transform>();
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

    /* -------------------------------------------------------------------------------- */

    // Hijo de ajustarPosicionBloques
    void determinarPos(ref float posicionX, ref float posicionZ)
    {
        int mayor;

        index = SceneManager.GetActiveScene().buildIndex;

        if (filas > columnas) 
            mayor = filas;
        else 
            mayor = columnas;

        float valorX = (columnas - 3) * 2.5f;
        float valorZ = (filas - 3) * 2.5f;

        posicionX -= valorX;
        posicionZ += valorZ;

        float offset = (mayor - 3) * 2.5f;

        Transform modelo = GameObject.Find("Modelo").GetComponent<Transform>();

        if (index != 23)
            modelo.position = new Vector3(modelo.position.x, modelo.position.y + offset, modelo.position.z);

        else
            modelo.position = new Vector3(modelo.position.x - 22 * (mayor / 12), modelo.position.y + 12 * (mayor / 12), modelo.position.z);

        Transform camara = GameObject.Find("Main Camera").GetComponent<Transform>();
        camara.position = new Vector3(camara.position.x, camara.position.y + offset, camara.position.z);
    }

    /* -------------------------------------------------------------------------------- */

    public void ganoJuego()
    {
        // Variable gano
        gano = true;

        // Desactivar reloj
        GetComponent<Timer>().toggleClock(false);

        AnalyticsResult result = AnalyticsEvent.Custom("Ganado_" + index);
        Debug.Log("[GameManager] Analytics Result: " + result + " | DATA: " + "Ganado_" + index);

        PlayerPrefs.SetString(index.ToString(), "Ganado");

        FindObjectOfType<Timer>().setPlayerPref();

        // Modificar texto
        if (index == 10 || index == 11 || index == 22 || index == 23) textoBoton.text = regresarAInicio;
        else textoBoton.text = siguienteNivel;

        // Mostrar boton
        boton.SetActive(true);
    }

    /* -------------------------------------------------------------------------------- */

    public void comenzarNivel()
    {
        int index = SceneManager.GetActiveScene().buildIndex;

        if (!gano) // Si no gano, comenzar juego
        {
            Debug.Log("[GameManager] Iniciando juego...");
            // Ocultar boton
            boton.SetActive(false);

            // Si es juego1
            if (index < 12)
                FindObjectOfType<Juego1>().comenzarNivel();

            // Si es juego2
            else if (index > 12)
                FindObjectOfType<Juego2>().comenzarNivel();

            // Activar reloj
            FindObjectOfType<Timer>().toggleClock(true);
        }

        // Si es custom level o nivel 10, regresar a inicio
        else if (index == 10 || index == 11 || index == 22 || index == 23) FindObjectOfType<LevelLoader>().cargarNivel(0);

        // En otros, pasar al siguiente nivel
        else
        {
            Debug.Log("[GameManager] Avanzando al siguiente nivel...");
            FindObjectOfType<LevelLoader>().cargarNivel(index + 1);
        }
    }
}
