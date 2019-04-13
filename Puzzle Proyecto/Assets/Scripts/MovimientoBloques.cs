﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MovimientoBloques : MonoBehaviour
{
    int[,] matriz = new int[3, 3] { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 0 } };
    int[,] emptySlot = new int[1, 2] { { 0, 0 } };
    int fila, columna;
    RaycastHit hit;

    // Update is called once per frame
    void Update()
    {

        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100.0f) && hit.transform != null && Input.GetMouseButtonDown(0))
        {
            Debug.LogWarning("Se presiono mouse");

            int slot = Int32.Parse(hit.transform.gameObject.name);

            scanEmptySlot(matriz, emptySlot, slot);

        }
    }

    void Start()
    {
        //mostrarMatriz(matriz);
    }

    void mostrarMatriz(int[,] matriz)
    {

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Debug.Log(matriz[i, j]);
            }
        }

    }

    void scanEmptySlot(int[,] matriz, int[,] emptySlot, int slot)
    {

        // Debug.Log(slot);

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

        //Debug.Log( "FILA: " + fila + " | COLUMNA: " + columna );

        if (columna + 1 < 3 && matriz[fila, columna + 1] == 0)
        {
            Transform posicion = GameObject.Find(hit.transform.gameObject.name).GetComponent<Transform>();

            Vector3 posicionVector = new Vector3(posicion.position.x + 6, posicion.position.y, posicion.position.z);

            Debug.Log("SE MUEVE A LA DERECHA");

            posicion.position = posicionVector;

            matriz[fila, columna + 1] = slot;
            matriz[fila, columna] = 0;
        }

        else if (columna - 1 > -1 && matriz[fila, columna - 1] == 0)
        {
            Transform posicion = GameObject.Find(hit.transform.gameObject.name).GetComponent<Transform>();

            Vector3 posicionVector = new Vector3(posicion.position.x - 6, posicion.position.y, posicion.position.z);

            Debug.Log("SE MUEVE A LA IZQUIERDA");

            posicion.position = posicionVector;

            matriz[fila, columna - 1] = slot;
            matriz[fila, columna] = 0;
        }
        else if (fila + 1 < 3 && matriz[fila + 1, columna] == 0)
        {
            Transform posicion = GameObject.Find(hit.transform.gameObject.name).GetComponent<Transform>();

            Vector3 posicionVector = new Vector3(posicion.position.x, posicion.position.y, posicion.position.z - 6);

            Debug.Log("SE MUEVE ABAJO");

            posicion.position = posicionVector;

            matriz[fila + 1, columna] = slot;
            matriz[fila, columna] = 0;
        }
        else if (fila - 1 > -1 && matriz[fila - 1, columna] == 0)
        {
            Transform posicion = GameObject.Find(hit.transform.gameObject.name).GetComponent<Transform>();

            Vector3 posicionVector = new Vector3(posicion.position.x, posicion.position.y, posicion.position.z + 6);

            Debug.Log("SE MUEVE ARRIBA");

            posicion.position = posicionVector;

            matriz[fila - 1, columna] = slot;
            matriz[fila, columna] = 0;
        }
        else
        {
            Debug.Log("No hay espacio a donde mover este bloque");
        }

    }
}
