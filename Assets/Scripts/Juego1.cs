using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

public class Juego1 : MonoBehaviour
{
    // << -------- Varbiables publicas -------- >>

    // Ganar juego [Debug Only]
    public bool GanarHack = false;

    // Flag de juego empezado
    public bool start = false;

    // Retraso de la animacion al mover bloques
    float tiempoAnimacion = 0.1f;

    // Flag de juego pausado
    public bool pause = false;

    // Activar random
    public bool activarRandom = true;

    // Tamaño de tabla
    public int columnas = 3;
    public int filas = 3;

    // Cantidad de movimientos aleatorios para mezclar
    public int RandomMoves = 30;

    Texture[] texturas;

    // << -------- Varbiables privadas -------- >>

    // Matrices para almacenar posiciones
    int[,] matriz;
    int[,] matrizGano;

    // Cantidad de movimientos
    int movimientos;

    // Flag juego ganado
    bool gano = false;
    
    // Texto movimientos
    Text textoMovimientos;

    // Nombre e index de escena actual
    int index;

    /* -------------------------------------------------------------------------------- */

    // Al iniciar juego
    void Start()
    {
        index  = SceneManager.GetActiveScene().buildIndex;

        cambiarTexturas();

        // Se calcula cantidad de random moves
        RandomMoves = (filas * columnas) * 5;

        // Si estamos en Juego1
        if (index < 11)
        {
            Renderer rendererModelo = GameObject.Find("Bloque Modelo").GetComponent<Renderer>();

            rendererModelo.material.mainTexture = texturas[index - 1];

            Mesh mesh = rendererModelo.GetComponent<MeshFilter>().mesh;
            Vector2[] UVs = new Vector2[mesh.vertices.Length];

            // ------ MODIFICAR PARA TENER BORDE -----

            // Cambiar "Tiling" de textura
            //objeto.material.mainTextureScale = new Vector2(scaleX, scaleY);
            // Ajustar "Offeset" de textura
            //objeto.material.mainTextureOffset = new Vector2(offsetX, offsetY);

            float xMin = 0.334f;
            float xMax = 0.666f;

            float yMin = 0.0f;
            float yMax = 0.333f;

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

            // Comenzar juego desde GameManager
            FindObjectOfType<GameManager>().comenzarJuego1();

            // Inicializacion del juego
            inicializar();
        }
    }

    /* -------------------------------------------------------------------------------- */

    // Se actualiza una vez por fotograma
    void Update()
    {
        if (GanarHack)
        {
            ganarJuego();
            GanarHack = false;
        }

        if (gano || !start || pause) return;

        // Generar rayo para "clickear" bloque
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        bool rayoTiradoYObjetoTocado = Physics.Raycast(ray, out RaycastHit hit, 3000f) && hit.transform != null;

        // Si el rayo hace contacto con un bloque y se tiene el click IZQUIERDO persionado
        if (rayoTiradoYObjetoTocado && Input.GetMouseButtonDown(0))
        {
            // Asignar slot correcto y escanear si movimiento es posible
            int slot = int.Parse(hit.transform.gameObject.name);
            scanEmptySlot(slot);
        }

        // Analizar si se gano con ese movimiento
        analizarGano();
    }

    /* -------------------------------------------------------------------------------- */

    public void inicializar(){

        // Inicializar Matrices
        matriz = new int[filas, columnas];
        matrizGano = new int[filas, columnas];

        // Asignar Texto Movimientos
        textoMovimientos = GameObject.Find("Numero Movimientos").GetComponent<Text>();

        // Llenar matrices de datos
        llenarMatrizes();

        if (activarRandom)
        {
            // Se repite hasta llegar a la cantidad de random moves o si se queda en posicion ganada
            do scanEmptySlot(Random.Range(1, (filas * columnas)));
            while (movimientos < RandomMoves || gano);

            // Reiniciar movimientos
            movimientos = 0;
            textoMovimientos.text = "0";

            Debug.Log("Movimientos aleatorios terminados. Comenzando juego...");
        }
        else Debug.LogError("RANDOM DESACTIVADO");
        
    }

    /* -------------------------------------------------------------------------------- */

    void analizarGano()
    {
        gano = true;

        // Analizar si matrizes juego y gano son identicas
        for (int i = 0; i < filas; i++) {
            for (int j = 0; j < columnas; j++) {

                if (matriz[i, j] != matrizGano[i, j]) gano = false;   
            }
        }
        // Si gano el nivel
        if (gano) ganarJuego();
    }

     /* -------------------------------------------------------------------------------- */

    int fila, columna;

    // Analizar si existe espacio vacio
    void scanEmptySlot(int slot)
    {
        // Analizar en que espacio estoy ahora
        for (int i = 0; i < filas; i++) {
            for (int j = 0; j < columnas; j++) {

                if (matriz[i, j] == slot)
                {
                    fila = i;
                    columna = j;
                }
            }
        }

        // Analizar movimientos posibles de bloque
        if (columna + 1 < columnas && matriz[fila, columna + 1] == 0)
        {
            // Se mueve hacia derecha
            accion(5, 0, "ActivarIzquierda");
        }
        else if (columna - 1 > -1 && matriz[fila, columna - 1] == 0)
        {
            // Se mueve hacia izquierda
            accion(-5, 0, "ActivarDerecha");
        }
        else if (fila + 1 < filas && matriz[fila + 1, columna] == 0)
        {
            // Se mueve hacia abajo
            accion(0, -5, "ActivarArriba");
        }
        else if (fila - 1 > -1 && matriz[fila - 1, columna] == 0)
        {
            // Se mueve hacia arriba
            accion(0, 5, "ActivarAbajo");
        }
        //else { Debug.Log("No hay espacio a donde mover este bloque"); }

        // Actualizar movimientos
        textoMovimientos.text = movimientos.ToString();

        void accion(int offsetX, int offsetZ, string nombreAnimacion)
        {

            GameObject bloque = GameObject.Find(slot.ToString());
            Transform posicion = bloque.GetComponent<Transform>();

            // Se modifica el vector posicion con la posicion correspondiente
            Vector3 posicionVector = new Vector3(posicion.position.x + offsetX, posicion.position.y, posicion.position.z + offsetZ);

            if (start)
                // Se aplica la animacion
                LeanTween.moveLocal(bloque, posicionVector, tiempoAnimacion);
            else
                posicion.position = posicionVector;

            // Se modifica la matriz para aplicar la nueva posicion
            matriz[fila + offsetZ / -5, columna + offsetX / 5] = slot;
            matriz[fila, columna] = 0;

            // Se incrementan los movimientos
            movimientos++;
        }
    }

    /* -------------------------------------------------------------------------------- */

    void llenarMatrizes()
    {
        int contador;

        contador = 1;

        for (int i = 0; i < filas; i++) {
            for (int j = 0; j < columnas; j++) {

                if (contador < filas * columnas)
                {
                    matriz[i, j] = contador;
                    matrizGano[i, j] = contador;
                }
                else {
                    matriz[i, j] = 0;
                    matrizGano[i, j] = 0;
                }
                contador++;
            }
        }
    }

    /* -------------------------------------------------------------------------------- */

    public void comenzarNivel() { start = true; }

    /* -------------------------------------------------------------------------------- */

   void ganarJuego()
    {
        gano = true;
        start = false;

        Debug.Log("Se gano nivel !! Llamando a game manager...");
        FindObjectOfType<GameManager>().ganoJuego();
    }

    /* -------------------------------------------------------------------------------- */

    void cambiarTexturas()
    {
        texturas = new Texture[10];

        Debug.Log("CAMBIAR TEXTURAS");
        for (int i = 0; i < 10 ; i++)
            texturas[i] = Resources.Load("Juego1/Image" + (i+1).ToString()) as Texture;
    }
}