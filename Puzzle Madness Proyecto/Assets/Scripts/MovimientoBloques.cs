using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class MovimientoBloques : MonoBehaviour
{
    int[,] matriz = new int[3, 3] { { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 } };
    int[,] matrizGano = new int[3, 3] { { 1, 2, 3 }, { 4, 5, 6 }, { 8, 7, 0 } };
    int[] arrayNumeros;
    int fila, columna;
    int movimientos;
    int num;
    
    public Text textoMovimiento;

    Vector3 posicionBloque = new Vector3();
    RaycastHit hit;

    bool gano = false;

    Transform objeto1;
    /*Transform objeto2;
    Transform objeto3;
    Transform objeto4;
    Transform objeto5;
    Transform objeto6;
    Transform objeto7;
    Transform objeto8;
    Transform objeto9;*/

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

        Transform posicion = GameObject.Find(hit.transform.gameObject.name).GetComponent<Transform>();

        // Analizar movimientos posibles de bloque
        if (columna + 1 < 3 && matriz[fila, columna + 1] == 0)
        {
            // Se mueve hacia la derecha
            accion(6,0);
        }
        else if (columna - 1 > -1 && matriz[fila, columna - 1] == 0)
        {
            // Se mueve hacia la izquierda
            accion(-6, 0);
        }
        else if (fila + 1 < 3 && matriz[fila + 1, columna] == 0)
        {
            // Se mueve hacia abajo
            accion(0, -6);
        }
        else if (fila - 1 > -1 && matriz[fila - 1, columna] == 0)
        {
            // Se mueve hacia arriba
            accion(0, 6);
        }
        else { Debug.Log("No hay espacio a donde mover este bloque"); }

        textoMovimiento.text = movimientos.ToString();
        
        void accion(int offsetX, int offsetZ) {

            Vector3 posicionVector = new Vector3(posicion.position.x + offsetX, posicion.position.y, posicion.position.z + offsetZ);
            posicion.position = posicionVector;

            matriz[fila + offsetZ / -6, columna + offsetX / 6] = slot;
            matriz[fila, columna] = 0;

            movimientos++;

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

    void moverBloques() {

        float posX = -9.63f;
        float posZ = 8f;

        for (int i = 0; i < 3; i++){
            for (int j = 0; j < 3; j++) {

                if (matriz[i, j] != 0) {
                    objeto1 = GameObject.Find(matriz[i, j].ToString()).GetComponent<Transform>();
                    posicionBloque = new Vector3(posX, 0.44f, posZ);
                    objeto1.position = posicionBloque;
                }
                
                posX += 6f;
                if (posX > 3)
                {
                    posX = -9.63f;
                    posZ -= 6;
                }
                
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
