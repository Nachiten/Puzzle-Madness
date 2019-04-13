using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
//using System;

public class MovimientoBloques : MonoBehaviour
{
    int[,] matriz = new int[3, 3] { { 2, 6, 3 }, { 4, 0, 1 }, { 8, 7, 5 } };
    int[,] matrizGano = new int[3, 3] { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 0 } };
    int fila, columna;
    int[] arrayNumeros;
    RaycastHit hit;
    bool gano = false;

    int num;

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

        generarMatriz();

        Debug.Log("Mostrando Matriz Convertida: ");
        mostrarMatriz();

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
            Transform posicion = GameObject.Find(hit.transform.gameObject.name).GetComponent<Transform>();

            Vector3 posicionVector = new Vector3(posicion.position.x + 6, posicion.position.y, posicion.position.z);

            //Debug.Log("SE MUEVE A LA DERECHA");

            posicion.position = posicionVector;

            matriz[fila, columna + 1] = slot;
            matriz[fila, columna] = 0;
        }

        else if (columna - 1 > -1 && matriz[fila, columna - 1] == 0)
        {
            Transform posicion = GameObject.Find(hit.transform.gameObject.name).GetComponent<Transform>();

            Vector3 posicionVector = new Vector3(posicion.position.x - 6, posicion.position.y, posicion.position.z);

            //Debug.Log("SE MUEVE A LA IZQUIERDA");

            posicion.position = posicionVector;

            matriz[fila, columna - 1] = slot;
            matriz[fila, columna] = 0;
        }
        else if (fila + 1 < 3 && matriz[fila + 1, columna] == 0)
        {
            Transform posicion = GameObject.Find(hit.transform.gameObject.name).GetComponent<Transform>();

            Vector3 posicionVector = new Vector3(posicion.position.x, posicion.position.y, posicion.position.z - 6);

            //Debug.Log("SE MUEVE ABAJO");

            posicion.position = posicionVector;

            matriz[fila + 1, columna] = slot;
            matriz[fila, columna] = 0;
        }
        else if (fila - 1 > -1 && matriz[fila - 1, columna] == 0)
        {
            Transform posicion = GameObject.Find(hit.transform.gameObject.name).GetComponent<Transform>();

            Vector3 posicionVector = new Vector3(posicion.position.x, posicion.position.y, posicion.position.z + 6);

            //Debug.Log("SE MUEVE ARRIBA");

            posicion.position = posicionVector;

            matriz[fila - 1, columna] = slot;
            matriz[fila, columna] = 0;
        }
        else
        {
            Debug.Log("No hay espacio a donde mover este bloque");
        }

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

