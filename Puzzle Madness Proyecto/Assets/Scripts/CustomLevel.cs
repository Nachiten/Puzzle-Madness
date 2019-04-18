using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomLevel : MonoBehaviour
{
    int tamañoMatriz;
    public bool start = false;

    /* -------------------------------------------------------------------------------- */

    public void comenzarNivel() {

        tamañoMatriz = FindObjectOfType<MovimientoBloques>().tamañoMatriz;

        eliminarSobrantes();

        GameObject.Find("Panel Seleccion").SetActive(false);

    }

    /* -------------------------------------------------------------------------------- */

    void eliminarSobrantes() {

        char nombre = 'A';
        char nombre2 = '-';

        int contador = 1;

        for (int i = 0; i < 7; i++) {
            for (int j = 0; j < 7; j++) {
                if (i >= tamañoMatriz || j >= tamañoMatriz || (i == tamañoMatriz - 1 && j == tamañoMatriz - 1))
                {
                    if (nombre2 == '-') destruir(nombre, '\0');
                                   else destruir(nombre, nombre2);
                }
                else {
                    if (nombre2 == '-') asignarNombre(nombre, '\0', contador);
                                   else asignarNombre(nombre, nombre2, contador);
                    contador++;
                }

                if (nombre2 == '-') nombre++;

                else if (nombre2 != '-') nombre2++;

                if (nombre == 'Z' + 1) {
                    nombre = 'A';
                    nombre2 = 'A';
                }
            }
        }
        start = true;

        FindObjectOfType<MovimientoBloques>().comenzar();
    }

    /* -------------------------------------------------------------------------------- */

    void asignarNombre(char nombre, char nombre2, int contador) { GameObject.Find((nombre.ToString() + nombre2.ToString())).name = contador.ToString(); }

    /* -------------------------------------------------------------------------------- */

    private void destruir(char nombre, char nombre2) { Destroy(GameObject.Find((nombre.ToString() + nombre2.ToString()))); }

    /* -------------------------------------------------------------------------------- */

    public void dropDown(int valor){ FindObjectOfType<MovimientoBloques>().tamañoMatriz = valor + 3; }
}
