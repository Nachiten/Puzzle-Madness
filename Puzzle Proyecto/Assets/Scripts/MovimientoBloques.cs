using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MovimientoBloques : MonoBehaviour
{
    int[,] matriz = new int[3,3] { { 1, 2, 3}, { 4, 5, 6}, { 7, 8, 0} };
    int[,] emptySlot = new int[1, 2] { { 0, 0 } };
    int fila, columna;

    // Update is called once per frame
    void Update()
    {

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100.0f) && hit.transform != null && Input.GetMouseButtonDown(0))
        {
            Debug.LogWarning("Se presiono mouse");

            int slot = Int32.Parse(hit.transform.gameObject.name);

            scanEmptySlot(matriz, emptySlot, slot);

        }
    }

    void Start() {
        //mostrarMatriz(matriz);
    }

    void mostrarMatriz(int[,] matriz) {

        for (int i = 0; i < 3; i++) { 
            for (int j = 0; j < 3; j++) {
                Debug.Log(matriz[i,j]);
            }
        }

    }

    void scanEmptySlot(int[,] matriz, int[,] emptySlot , int slot) {

        // Debug.Log(slot);

        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 3; j++) {

                if (matriz[i, j] == slot) {

                    fila = i;
                    columna = j;

                }

            }
        }

        //Debug.Log( "FILA: " + fila + " | COLUMNA: " + columna );

        if (columna + 1 < 3 && matriz[fila, columna + 1] == 0)
        {
            Debug.Log("SE MUEVE A LA DERECHA");
            //GameObject.FindObjectOfType
        }
        else if (columna - 1 > -1 && matriz[fila, columna - 1] == 0)
        {
            Debug.Log("SE MUEVE A LA IZQUIERDA");
        }
        else if (fila + 1 < 3 && matriz[fila + 1, columna] == 0)
        {
            Debug.Log("SE MUEVE ABAJO");
        }
        else if (fila - 1 > -1 && matriz[fila - 1, columna] == 0)
        {
            Debug.Log("SE MUEVE ARRIBA");
        }
        else
        {
            Debug.Log("No hay espacio a donde mover este bloque");
        }

    }
}
