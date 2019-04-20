using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System;

public class CustomLevel : MonoBehaviour
{
    int tamañoMatriz;
    RawImage imagen;
    GameObject imagenPreview;

    bool imageSet = false;
    bool sizeSet = false;

    /* -------------------------------------------------------------------------------- */

    void Start() 
    { 
        imagen = GameObject.Find("Imagen Elegida").GetComponent<RawImage>();
        imagenPreview = GameObject.Find("Imagen Preview");

        imagenPreview.SetActive(false);
    }

    /* -------------------------------------------------------------------------------- */

    public void comenzarNivel() {

        if ( !(imageSet && sizeSet) )
        {
            EditorUtility.DisplayDialog("Respetar Orden", "Debes seleccionar primero una imagen luego un tamaño antes de comenzar", "Bueno ...");
            return;
        }

        FindObjectOfType<MovimientoBloques>().comenzar();

        GameObject.Find("Panel Seleccion").SetActive(false);
    }

    /* -------------------------------------------------------------------------------- */

    public void eliminarSobrantes() {

        if (!imageSet) {
            EditorUtility.DisplayDialog("Respetar Orden", "Debes primero seleccionar una imagen antes de seleccionar el tamaño.", "Bueno ...");
            return;
        }

        sizeSet = true;
        imagenPreview.SetActive(true);  

        tamañoMatriz = FindObjectOfType<MovimientoBloques>().tamañoMatriz;

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

        GameObject.Find("Bloque Modelo").GetComponent<Renderer>().material.mainTexture = imagen.texture;

        FindObjectOfType<MovimientoBloques>().ajustarPosiciones();
    }

    /* -------------------------------------------------------------------------------- */

    void asignarNombre(char nombre, char nombre2, int contador) 
    {
        GameObject.Find( nombre.ToString() + nombre2.ToString() ).name = contador.ToString();
        GameObject.Find(contador.ToString()).GetComponent<Renderer>().material.mainTexture = imagen.texture;
    }

    /* -------------------------------------------------------------------------------- */

    private void destruir(char nombre, char nombre2) { Destroy(GameObject.Find(nombre.ToString() + nombre2.ToString())); }

    /* -------------------------------------------------------------------------------- */

    public void dropDown(int valor){ FindObjectOfType<MovimientoBloques>().tamañoMatriz = valor + 3; }

    /* ----------------------------------- Explorer ----------------------------------- */
    
    string path;

    public void AbrirExplorer()
    {
        path = EditorUtility.OpenFilePanel("Seleccionar Imagen", "", "png");
        setearImagen();
    }

    void setearImagen()
    {
        if (path != null && path != "" && path.Substring(Math.Max(0, path.Length - 4)) == ".png")
        {
            WWW www = new WWW("file:///" + path);
            imagen.texture = www.texture;
            imageSet = true;
        }
        else
        {
            EditorUtility.DisplayDialog("Error !!", "Debes seleccionar una imagen valida en formato .PNG", "Bueno ...");
            imageSet = false;
        }
    }
}
