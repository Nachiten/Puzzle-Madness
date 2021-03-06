﻿using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Juego1 : MonoBehaviour, IJuegos
{
    #region Variables Publicas

    public bool activarRandom = true, GanarHack = false, start = false;

    public int columnas = 3, filas = 3, RandomMoves = 30;

    #endregion

    /* -------------------------------------------------------------------------------- */

    #region Variables Privadas

    // Matrices para almacenar posiciones
    int[,] matriz, matrizGano;

    // Cantidad de movimientos
    int movimientos, index;

    // Retraso de la animacion al mover bloques
    float tiempoAnimacion = 0.1f;

    // Flag juego ganado
    bool gano = false, animacionActiva = false, pause = false, terminoSetupInicial = false;

    // Texto movimientos
    TMP_Text textoMovimientos;

    Texture[] texturas;

    #endregion

    /* -------------------------------------------------------------------------------- */

    #region FuncionStart

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
            terminoSetupInicial = true;

            Renderer rendererModelo = GameObject.Find("Bloque Modelo").GetComponent<Renderer>();

            rendererModelo.material.mainTexture = texturas[index - 1];

            Mesh mesh = rendererModelo.GetComponent<MeshFilter>().mesh;
            Vector2[] UVs = new Vector2[mesh.vertices.Length];

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

    #endregion

    /* -------------------------------------------------------------------------------- */

    #region FuncionUpdate

    // Se actualiza una vez por fotograma
    void Update()
    {
        if (GanarHack)
        {
            ganarJuego();
            GanarHack = false;
        }

        if (gano || pause || animacionActiva || !terminoSetupInicial) return;

        // Generar rayo para "clickear" bloque
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        bool rayoTiradoYObjetoTocado = Physics.Raycast(ray, out RaycastHit hit, 3000f) && hit.transform != null;

        // Si el rayo hace contacto con un bloque y se tiene el click IZQUIERDO persionado
        if (rayoTiradoYObjetoTocado && Input.GetMouseButtonDown(0))
        {
            if (!start) 
            {
                Debug.Log("[Juego1] Comienzo Juego...");

                // Marco juego comenzado
                start = true;

                // Activar reloj
                GameObject.Find("GameManager").GetComponent<Timer>().toggleClock(true);
            }

            // Asignar slot correcto y escanear si movimiento es posible
            int slot = int.Parse(hit.transform.gameObject.name);
            scanEmptySlot(slot);

            // Analizar si se gano con ese movimiento
            analizarGano();
        }
    }

    #endregion

    /* -------------------------------------------------------------------------------- */

    void analizarGano()
    {
        gano = true;

        // Analizar si matrizes juego y gano son identicas
        for (int i = 0; i < filas; i++) 
            for (int j = 0; j < columnas; j++) 
                if (matriz[i, j] != matrizGano[i, j]) 
                    gano = false;   
            
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
            {
                animacionActiva = true;
                // Se aplica la animacion
                LeanTween.moveLocal(bloque, posicionVector, tiempoAnimacion).setOnComplete(terminarAnimacion);
            }  
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

    void terminarAnimacion() { animacionActiva = false; }

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

   void ganarJuego()
    {
        gano = true;
        start = false;

        Debug.Log("[Juego1] Se gano nivel!! Llamando a game manager...");
        FindObjectOfType<GameManager>().ganoJuego();
    }

    /* -------------------------------------------------------------------------------- */

    void cambiarTexturas()
    {
        texturas = new Texture[10];

        for (int i = 0; i < 10 ; i++)
            texturas[i] = Resources.Load("Juego1/Image" + (i+1).ToString()) as Texture;
    }

    /* -------------------------------------------------------------------------------- */

    void ejecutarMovimientosRandom()
    {
        if (activarRandom)
        {
            int intentos = 0;

            // Se repite hasta llegar a la cantidad de random moves o si se queda en posicion ganada
            do
            {
                if (intentos > 300000)
                {
                    Debug.LogError("[Juego1] DEMASIADOS INTENTOS!!");
                    break;
                }

                scanEmptySlot(Random.Range(1, filas * columnas));

                intentos++;
            } while (movimientos < RandomMoves || gano);

            // Reiniciar movimientos
            movimientos = 0;
            textoMovimientos.text = "0";

            //Debug.Log("[Juego1] Movimientos aleatorios terminados. Comenzando juego...");
        }
        else Debug.LogError("[Juego1] RANDOM DESACTIVADO!!");
    }

    /* ----------------------------------- Interfaz ----------------------------------- */

    public void fijarFilasYColumnas(int filas, int columnas)
    {
        this.filas = filas;
        this.columnas = columnas;

        int mayor = columnas;

        if (filas > columnas) 
            mayor = filas;

        RandomMoves = mayor * mayor * mayor;
    }

    /* -------------------------------------------------------------------------------- */

    public void inicializar()
    {
        terminoSetupInicial = true;

        // Inicializar matrices
        matriz = new int[filas, columnas];
        matrizGano = new int[filas, columnas];

        // Asignar texto movimientos
        textoMovimientos = GameObject.Find("Numero Movimientos").GetComponent<TMP_Text>();

        // Llenar matrices de datos
        llenarMatrizes();

        // Generar movimientos random
        ejecutarMovimientosRandom();
    }

    public int obtenerMovimientos() 
    {
        return movimientos;
    }
}