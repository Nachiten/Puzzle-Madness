using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;
using TMPro;

public class GameManager : MonoBehaviour
{
    #region Variables

    // Boton "Comenzar"
    GameObject boton;
    TMP_Text textoBoton, textoNivel;

    // Cantidad de bloques
    [HideInInspector]
    public int filas = 3, columnas = 3;

    // Numero de escena actual
    int index;

    // Strings usados como texto del boton
    string regresarAInicio = "REGRESAR A INICIO", siguienteNivel = "SIGUIENTE NIVEL";

    #endregion

    /* -------------------------------------------------------------------------------- */

    #region FuncionStart

    void Start()
    {
        // Escena actual
        index = SceneManager.GetActiveScene().buildIndex;
        string nombreNivel = SceneManager.GetActiveScene().name;

        // Mostrar texto del nivel actual
        textoNivel = GameObject.Find("Nivel").GetComponent<TMP_Text>();
        textoNivel.text = nombreNivel;

        // Asignar variables
        boton = GameObject.Find("Boton");
        textoBoton = GameObject.Find("TextoBoton").GetComponent<TMP_Text>();

        boton.SetActive(false);
    }

    #endregion

    /* -------------------------------------------------------------------------------- */

    public void comenzarCustomLevel() 
    {
        if (index == 11)
            comenzarJuego1();
        else
            comenzarJuego2();
    }

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

        // Ajusta las texturas de los bloques
        ajustarTexturasBloques();

        // Ajustar ubicacion de bloques
        ajustarPosicionBloques();

        // Ajusta ubicacion del modelo y de la camara
        ajustarPosicionCamaraYModelo();
        
    }

    /* -------------------------------------------------------------------------------- */

    public void generarBloques()
    {
        //Debug.Log("[GameManager] Generando Bloques...");

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
        // Textura de imagen modelo
        Renderer modelo = GameObject.Find("Bloque Modelo").GetComponent<Renderer>();

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
                    Renderer objeto = GameObject.Find(contador.ToString()).GetComponent<Renderer>();

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

                        // Imagen de arriba
                        UVs[4] = new Vector2(xMin, yMax);
                        UVs[5] = new Vector2(xMax, yMax);
                        UVs[8] = new Vector2(xMin, yMin);
                        UVs[9] = new Vector2(xMax, yMin);

                        float cero = 0f;
                        float unTercio = 0.333f;

                        // El resto (todo toma color negro)
                        for (int meshCounter = 0; meshCounter <= 20; meshCounter += 4)
                        {
                            if (meshCounter != 4 && meshCounter != 8)
                                UVs[meshCounter] = new Vector2(cero, cero);
                            if (meshCounter + 1 != 5 && meshCounter + 1 != 9)
                                UVs[meshCounter + 1] = new Vector2(unTercio, cero);

                            UVs[meshCounter + 2] = new Vector2(cero, unTercio);
                            UVs[meshCounter + 3] = new Vector2(unTercio, unTercio);
                        }

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

    public void ajustarPosicionBloques()
    {
        if (index < 12)
        {
            Transform plataforma = GameObject.Find("Piso Mapa").GetComponent<Transform>();
            plataforma.localScale = new Vector3((5 * columnas) + 2, plataforma.localScale.y, (5 * filas) + 2);
        }

        int contador = 1;

        float offsetX = 0;
        float offsetZ = 0;

        float posXReferencia = 0;
        float posZReferencia = 0;

        for (int i = 0; i < filas; i++)
        {
            for (int j = 0; j < columnas; j++)
            {
                if (i == 0 && j == 0)
                {
                    // Primer bloque es la referencia
                    Transform referenciaAjuste = GameObject.Find(contador.ToString()).GetComponent<Transform>();

                    posXReferencia = referenciaAjuste.position.x - (columnas - 3) * 2.5f - 0.5f;
                    posZReferencia = referenciaAjuste.position.z + (filas - 3) * 2.5f + 0.5f;

                    referenciaAjuste.position = new Vector3(posXReferencia, referenciaAjuste.position.y, posZReferencia);
                }
                else if (!(i == filas - 1 && j == columnas - 1))
                {
                    Transform objeto = GameObject.Find(contador.ToString()).GetComponent<Transform>();

                    objeto.position = new Vector3(posXReferencia + offsetX, objeto.position.y, posZReferencia + offsetZ);
                }

                offsetX += 5;
                contador++;
            }
            offsetX = 0;
            offsetZ -= 5;
        }
    }

    /* -------------------------------------------------------------------------------- */

    float offsetMayorYCamara = 40f;
    float offsetMayorXModelo = 24.7f;
    float offsetMayorYModelo = 0f;

    // Hijo de ajustarPosicionBloques
    void ajustarPosicionCamaraYModelo()
    {
        int mayor = columnas;

        if (filas > columnas) 
            mayor = filas;

        index = SceneManager.GetActiveScene().buildIndex;

        // Si estoy en juego 2
        if (index > 12) 
        {
            offsetMayorYCamara = 81;
            offsetMayorYModelo = 41;
        }

        //Debug.Log("[GameManager] mayor: " + mayor);

        float offsetYCamara = (mayor - 3) * offsetMayorYCamara / 9;

        //Debug.Log("[GameManager] OffsetY aplicado a camara: " + offsetYCamara);

        Transform camara = GameObject.Find("Main Camera").GetComponent<Transform>();
        camara.position = new Vector3(camara.position.x, camara.position.y + offsetYCamara, camara.position.z);

        GameObject.Find("Main Camera").GetComponent<CameraAspectRatioScaler>().inicializacionFinalizada();

        //Modifico el modelo
        Transform modeloTransform = GameObject.Find("Bloque Modelo").GetComponent<Transform>();

        float offsetXModelo = (mayor - 3) * offsetMayorXModelo / 9;
        float offsetYModelo = (mayor - 3) * offsetMayorYModelo / 9;

        //Debug.Log("[GameManager] OffsetX aplicado a modelo: " + offsetXModelo);
        //Debug.Log("[GameManager] OffsetY aplicado a modelo: " + offsetYModelo);

        modeloTransform.position = new Vector3(modeloTransform.position.x - offsetXModelo, modeloTransform.position.y + offsetYModelo, modeloTransform.position.z);

        // Ajustar tamaño de imagen modelo a nivel actual
        modeloTransform.localScale = new Vector3(1.5f * columnas, modeloTransform.localScale.y, 1.5f * filas);
    }

    /* -------------------------------------------------------------------------------- */

    public void ganoJuego()
    {
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

        switch (index) 
        {
            case 10:
                Debug.Log("[GameManager] Pasando a Juego2 Nivel 01...");
                FindObjectOfType<LevelLoader>().cargarNivel(13);
                break;

            case 11:
            case 22:
            case 23:
                Debug.Log("[GameManager] Volviendo a menu principal...");
                FindObjectOfType<LevelLoader>().cargarNivel(0);
                break;

            default:
                Debug.Log("[GameManager] Avanzando al siguiente nivel...");
                FindObjectOfType<LevelLoader>().cargarNivel(index + 1);
                break;
        }
    }    
}
