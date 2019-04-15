using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MovimientoBloques : MonoBehaviour
{
    // Tamaño del mapa
    public int tamañoMatriz = 3;

    // Matrices para almacenar posiciones
    int[,] matriz;
    int[,] matrizGano;

    // Cantidad de movimientos
    int movimientos;

    // Bool juego Ganado
    bool gano = false;
    // Bool juego empezado
    bool start = false;

    // Texto de movimientos
    Text textoMovimiento;

    // Cantidad de movimientos aleatorios para mezclar
    public int RandomMoves = 30;
    // Activar random
    public bool activarRandom = true;

    /* -------------------------------------------------------------------------------- */

    // Se actualiza una vez por fotograma
    void Update()
    {
        if (!gano && start)
        {
            // Generar rayo para "clickear" bloque
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Si el rayo hace contacto con un bloque y se tiene el click IZQUIERDO persionado
            if (Physics.Raycast(ray, out RaycastHit hit, 100.0f) && hit.transform != null && Input.GetMouseButtonDown(0))
            {
                // Asignar slot correcto y escanear si movimiento es posible
                int slot = int.Parse(hit.transform.gameObject.name);
                scanEmptySlot(slot);
            }
            // Analizar si se gano con ese movimiento
            analizarGano();
        }
    }

    /* -------------------------------------------------------------------------------- */

    // Al iniciar juego
    void Start()
    {
        modificarCosas();

        // Inicializar Matrices
        matriz = new int[tamañoMatriz, tamañoMatriz];
        matrizGano = new int[tamañoMatriz, tamañoMatriz];

        // Asignar Texto Movimientos
        textoMovimiento = GameObject.Find("Numero Movimientos").GetComponent<Text>();

        // Llenar matrices de datos
        llenarMatrizes();

        if (activarRandom)
        {
            
            Debug.Log("Generando Movimientos Random");
            do // Se repite si queda en posicion ganada y hasta que haya 30 movimientos
            {
                // Generar movimiento random
                scanEmptySlot(Random.Range(1, (tamañoMatriz * tamañoMatriz)));

            } while (movimientos < RandomMoves || gano);

            // Reiniviar movimientos
            movimientos = 0;
            textoMovimiento.text = "0";

            Debug.Log("Movimientos aleatorios terminados | Comenzando juego...");
        }
        else
        {
            Debug.LogError("RANDOM DESACTIVADO");
        }
        
    }

    /* -------------------------------------------------------------------------------- */

    void analizarGano()
    {
        gano = true;

        // Analizar si matrizes juego y gano son identicas
        for (int i = 0; i < tamañoMatriz; i++)
        {
            for (int j = 0; j < tamañoMatriz; j++)
            {
                if (matriz[i, j] != matrizGano[i, j])
                {
                    gano = false;
                }
            }
        }

        // Si termino el juego parar el timer
        if (gano)
        {
            Debug.Log("YA GANO !!");
            GameObject.Find("GameManager").GetComponent<Timer>().toggleClock(false);
        }

    }

    /* -------------------------------------------------------------------------------- */

    int fila, columna;

    // Analizar si existe espacio vacio
    void scanEmptySlot(int slot)
    {
        // Analizar en que espacio estoy ahora
        for (int i = 0; i < tamañoMatriz; i++)
        {
            for (int j = 0; j < tamañoMatriz; j++)
            {
                if (matriz[i, j] == slot)
                {
                    fila = i;
                    columna = j;
                }
            }
        }

        Transform posicion = GameObject.Find(slot.ToString()).GetComponent<Transform>();

        // X= -6.514721 Y= 0.7505659 Z= 8.107872

        // Analizar movimientos posibles de bloque
        if (columna + 1 < tamañoMatriz && matriz[fila, columna + 1] == 0)
        {
            // Se mueve hacia derecha
            accion(5, 0);
        }
        else if (columna - 1 > -1 && matriz[fila, columna - 1] == 0)
        {
            // Se mueve hacia izquierda
            accion(-5, 0);
        }
        else if (fila + 1 < tamañoMatriz && matriz[fila + 1, columna] == 0)
        {
            // Se mueve hacia abajo
            accion(0, -5);
        }
        else if (fila - 1 > -1 && matriz[fila - 1, columna] == 0)
        {
            // Se mueve hacia arriba
            accion(0, 5);
        }
        else { Debug.Log("No hay espacio a donde mover este bloque"); }

        textoMovimiento.text = movimientos.ToString();

        void accion(int offsetX, int offsetZ)
        {

            // Se modifica el vector posicion con la posicion correspondiente
            Vector3 posicionVector = new Vector3(posicion.position.x + offsetX, posicion.position.y, posicion.position.z + offsetZ);
            posicion.position = posicionVector;

            // Se modifica la matriz para aplicar la nueva posicion
            matriz[fila + offsetZ / -5, columna + offsetX / 5] = slot;
            matriz[fila, columna] = 0;

            movimientos++;

        }
    }

    /* -------------------------------------------------------------------------------- */

    void llenarMatrizes()
    {
        int contador;

        contador = 1;

        for (int i = 0; i < tamañoMatriz; i++)
        {
            for (int j = 0; j < tamañoMatriz; j++)
            {
                if (contador < tamañoMatriz * tamañoMatriz)
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

    void modificarCosas() {
        Renderer objeto;

        int contador = 1;

        float scale = 1f / tamañoMatriz;
        float offsetX = 0f;
        float offsetY = scale * (tamañoMatriz - 1);


        Debug.Log("SCALE: " + scale);

        for (int i = 0; i < tamañoMatriz; i++) {
            for (int j = 0; j < tamañoMatriz; j++)
            {
                if (contador < tamañoMatriz * tamañoMatriz)
                { 
                    // Asignar renderer
                    objeto = GameObject.Find(contador.ToString()).GetComponent<Renderer>();
                    // Cambiar "Tiling" de textura
                    objeto.material.mainTextureScale = new Vector2( scale, scale);
                    // Ajustar "Offeset" de textura
                    objeto.material.mainTextureOffset = new Vector2( offsetX, offsetY);

                    contador++;
                }

                offsetX += scale;
                //if (offsetX > scale * (tamañoMatriz - 1)) { offsetX = 0; }
            }
            offsetX = 0;
            offsetY -= scale;
        }
    }

    /* -------------------------------------------------------------------------------- */

    public void startGame() { start = true; }
}
