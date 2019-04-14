using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class MovimientoBloques : MonoBehaviour
{
    int[,] matriz = new int[3, 3] { { 2, 6, 3 }, { 4, 0, 1 }, { 8, 7, 5 } };
    int[,] matrizGano = new int[3, 3] { { 1, 2, 3 }, { 4, 5, 6 }, { 8, 7, 0 } };
    int[] arrayNumeros;
    int fila, columna;
    int num;
    int movimientos;
    public Text textoMovimiento;

    Vector3 posicionBloque = new Vector3();
    RaycastHit hit;

    bool gano = false;

    Transform objeto1;
    Transform objeto2;
    Transform objeto3;
    Transform objeto4;
    Transform objeto5;
    Transform objeto6;
    Transform objeto7;
    Transform objeto8;
    Transform objeto9;

    // Update is called once per frame
    void Update()
    {
        if (!gano)
        {
            // Generar rayo para "clickear" bloque
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Si el rayo hace contacto con un bloque y se tiene el click IZQUIERDO persionado
            if (Physics.Raycast(ray, out hit, 100.0f) && hit.transform != null && Input.GetMouseButtonDown(0))
            {
                int slot = int.Parse(hit.transform.gameObject.name);
                scanEmptySlot(matriz, slot);
            }
            analizarGano(matriz);
        }
    }


    void Start()
    {
        Debug.Log("Game Started");
        generarMatriz();

        //Debug.Log("Mostrando Matriz Convertida: ");
        //mostrarMatriz();

        moverBloques();
    }

    void analizarGano(int[,] matriz)
    {
        gano = true;

        // Analizar si matrizes juego y gano son identicas
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
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
            GameObject.Find("GameManager").GetComponent<Timer>().flag = true;
        }

    }

    // Analizar si existe espacio vacio
    void scanEmptySlot(int[,] matriz, int slot)
    {
        // Analizar en que espacio estoy ahora
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (matriz[i, j] == slot)
                {
                    fila = i;
                    columna = j;
                }
            }
        }

        // Analizar movimientos posibles de bloque
        if (columna + 1 < 3 && matriz[fila, columna + 1] == 0)
        {
            // Se mueve hacia la derecha
            Transform posicion = GameObject.Find(hit.transform.gameObject.name).GetComponent<Transform>();

            Vector3 posicionVector = new Vector3(posicion.position.x + 6, posicion.position.y, posicion.position.z);
            posicion.position = posicionVector;

            matriz[fila, columna + 1] = slot;
            matriz[fila, columna] = 0;

            movimientos++;
        }

        else if (columna - 1 > -1 && matriz[fila, columna - 1] == 0)
        {
            // Se mueve hacia la izquierda
            Transform posicion = GameObject.Find(hit.transform.gameObject.name).GetComponent<Transform>();

            Vector3 posicionVector = new Vector3(posicion.position.x - 6, posicion.position.y, posicion.position.z);
            posicion.position = posicionVector;

            matriz[fila, columna - 1] = slot;
            matriz[fila, columna] = 0;

            movimientos++;
        }
        else if (fila + 1 < 3 && matriz[fila + 1, columna] == 0)
        {
            // Se mueve hacia abajo
            Transform posicion = GameObject.Find(hit.transform.gameObject.name).GetComponent<Transform>();

            Vector3 posicionVector = new Vector3(posicion.position.x, posicion.position.y, posicion.position.z - 6);
            posicion.position = posicionVector;

            matriz[fila + 1, columna] = slot;
            matriz[fila, columna] = 0;

            movimientos++;
        }
        else if (fila - 1 > -1 && matriz[fila - 1, columna] == 0)
        {
            // Se mueve hacia arriba
            Transform posicion = GameObject.Find(hit.transform.gameObject.name).GetComponent<Transform>();

            Vector3 posicionVector = new Vector3(posicion.position.x, posicion.position.y, posicion.position.z + 6);
            posicion.position = posicionVector;

            matriz[fila - 1, columna] = slot;
            matriz[fila, columna] = 0;

            movimientos++;
        }
        else
        {
            Debug.Log("No hay espacio a donde mover este bloque");
        }

        textoMovimiento.text = movimientos.ToString();

    }

    // Generar matriz con numeros aleatorios de hash
    void generarMatriz() {
        // Contador para leer hash
        int k;

        // Generar hash para almacenar numeros aleatorios
        var hash = new HashSet<int>();

        // Agregar numeros hasta que haya 0-8 sin repetir
        while (hash.Count < 9) { hash.Add(Random.Range(0, 9)); }

        // Convertir a un array de ints
        arrayNumeros = hash.ToArray();

        k = 0;

        // Cargar matriz con numeros aleatorios
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                matriz[i, j] = arrayNumeros[k];
                k++;
            }
        }

    }

    void moverBloques() {

        if (matriz[0, 0] != 0) { objeto1 = GameObject.Find(matriz[0, 0].ToString()).GetComponent<Transform>(); }
        if (matriz[0, 1] != 0) { objeto2 = GameObject.Find(matriz[0, 1].ToString()).GetComponent<Transform>(); }
        if (matriz[0, 2] != 0) { objeto3 = GameObject.Find(matriz[0, 2].ToString()).GetComponent<Transform>(); }
        if (matriz[1, 0] != 0) { objeto4 = GameObject.Find(matriz[1, 0].ToString()).GetComponent<Transform>(); }
        if (matriz[1, 1] != 0) { objeto5 = GameObject.Find(matriz[1, 1].ToString()).GetComponent<Transform>(); }
        if (matriz[1, 2] != 0) { objeto6 = GameObject.Find(matriz[1, 2].ToString()).GetComponent<Transform>(); }
        if (matriz[2, 0] != 0) { objeto7 = GameObject.Find(matriz[2, 0].ToString()).GetComponent<Transform>(); }
        if (matriz[2, 1] != 0) { objeto8 = GameObject.Find(matriz[2, 1].ToString()).GetComponent<Transform>(); }
        if (matriz[2, 2] != 0) { objeto9 = GameObject.Find(matriz[2, 2].ToString()).GetComponent<Transform>(); }

        if (objeto1 != null)
        {
            posicionBloque = new Vector3( -9.63f , 0.44f, 8f);

            objeto1.position = posicionBloque;
        }

        if (objeto2 != null)
        {
            posicionBloque = new Vector3(-3.63f, 0.44f, 8f);

            objeto2.position = posicionBloque;
        }

        if (objeto3 != null)
        {
            posicionBloque = new Vector3(2.37f, 0.44f, 8f);

            objeto3.position = posicionBloque;
        }

        if (objeto4 != null)
        {
            posicionBloque = new Vector3(-9.63f, 0.44f, 2f);

            objeto4.position = posicionBloque;
        }

        if (objeto5 != null)
        {
            posicionBloque = new Vector3(-3.63f, 0.44f, 2f);

            objeto5.position = posicionBloque;
        }

        if (objeto6 != null)
        {
            posicionBloque = new Vector3(2.37f, 0.44f, 2f);

            objeto6.position = posicionBloque;
        }

        if (objeto7 != null)
        {
            posicionBloque = new Vector3(-9.63f, 0.44f, -4f);

            objeto7.position = posicionBloque;
        }

        if (objeto8 != null)
        {
            posicionBloque = new Vector3(-3.63f, 0.44f, -4f);

            objeto8.position = posicionBloque;
        }

        if (objeto9 != null)
        {
            posicionBloque = new Vector3(2.37f, 0.44f, -4f);
        
            objeto9.position = posicionBloque;
        }
    }


    // Mostrar Matriz
    void mostrarMatriz() {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Debug.Log(matriz[i, j]);
            }
        }
    }

}
