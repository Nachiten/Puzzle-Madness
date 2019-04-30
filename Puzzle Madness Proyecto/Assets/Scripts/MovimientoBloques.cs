using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

public class MovimientoBloques : MonoBehaviour
{
    // << --- Varbiables publicas --- >>

    // Ganar juego [Debug Only]
    public bool GanarHack = false;

    // Flag de juego empezado
    public bool start = false;

    // Flag de juego pausado
    public bool pause = false;

    // Activar random
    public bool activarRandom = true;

    // Tamaño de tabla
    public int columnas = 3;
    public int filas = 3;

    // Cantidad de movimientos aleatorios para mezclar
    public int RandomMoves = 30;


    // << --- Varbiables privadas --- >>

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
    string nombre;
    int index;

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

        // Si el rayo hace contacto con un bloque y se tiene el click IZQUIERDO persionado
        if (Physics.Raycast(ray, out RaycastHit hit, 5000f) && hit.transform != null && Input.GetMouseButtonDown(0))
        {
            // Asignar slot correcto y escanear si movimiento es posible
            int slot = int.Parse(hit.transform.gameObject.name);
            scanEmptySlot(slot);
        }

        // Analizar si se gano con ese movimiento
        analizarGano();
        
    }

    /* -------------------------------------------------------------------------------- */

    // Al iniciar juego
    void Start()
    {
        // Nombre e index de escena actual
        nombre = SceneManager.GetActiveScene().name;
        index = SceneManager.GetActiveScene().buildIndex;

        // Se calcula cantidad de random moves
        RandomMoves = (filas * columnas) * 5;

        // Si estamos en Juego1
        if (index < 11)
        {
            // Generar bloques del mapa
            GetComponent<GameManager>().generarBloques();

            ajustarPosiciones();
            comenzar();
        }
    }

    /* -------------------------------------------------------------------------------- */

    public void comenzar(){

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

            Debug.Log("Movimientos aleatorios terminados | Comenzando juego...");
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

        Transform posicion = GameObject.Find(slot.ToString()).GetComponent<Transform>();

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
        else { Debug.Log("No hay espacio a donde mover este bloque"); }

        // Actualizar movimientos
        textoMovimientos.text = movimientos.ToString();

        void accion(int offsetX, int offsetZ, string nombreAnimacion)
        {
            // Se modifica el vector posicion con la posicion correspondiente
            Vector3 posicionVector = new Vector3(posicion.position.x + offsetX, posicion.position.y, posicion.position.z + offsetZ);

            // Se aplica la posicion
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

    Renderer modelo;

    // Ajustar posiciones y offsets de imagenes
    public void ajustarPosiciones()
    {
        modelo = GameObject.Find("Bloque Modelo").GetComponent<Renderer>();

        Renderer objeto;

        int contador = 1;

        float scaleX = 1f / columnas;
        float scaleY = 1f / filas;

        float offsetX = 0f;
        float offsetY = scaleY * (filas * columnas - 1);

        for (int i = 0; i < filas; i++) {
            for (int j = 0; j < columnas; j++)
            {
                if (contador < filas * columnas)
                {
                    // Asignar renderer
                    objeto = GameObject.Find(contador.ToString()).GetComponent<Renderer>();
                    // Cambiar "Tiling" de textura
                    objeto.material.mainTextureScale = new Vector2(scaleX, scaleY);
                    // Ajustar "Offeset" de textura
                    objeto.material.mainTextureOffset = new Vector2(offsetX, offsetY);

                    objeto.material.mainTexture = modelo.material.mainTexture;

                    contador++;
                }

                offsetX += scaleX;
            }
            offsetX = 0;
            offsetY -= scaleY;
        }
    }

    /* -------------------------------------------------------------------------------- */

    public void comenzarNivel() { start = true; }

    /* -------------------------------------------------------------------------------- */

   void ganarJuego()
    {
        AnalyticsResult result = AnalyticsEvent.Custom("Ganado_" + nombre);
        Debug.Log("Analytics Result: " + result + " | DATA: " + "Ganado_" + nombre);

        PlayerPrefs.SetString(nombre, "Ganado");
        Debug.Log(PlayerPrefs.GetString(nombre));

        FindObjectOfType<Timer>().setPlayerPref();

        Debug.Log("Se gano el juego !! Llamando GameManager");
        FindObjectOfType<GameManager>().ganoJuego();
    }
}