using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;

public class CustomLevel : MonoBehaviour
{
    // Tamaño de tabla
    public int columnas = 3;
    public int filas = 3;

    public bool imageSet = false;
    public bool sizeSet = false;

    RawImage imagen;

    GameObject imagenPreview;
    Text inputField;

    int mayor; 

    /* -------------------------------------------------------------------------------- */

    void Start() 
    {
        imagen = GameObject.Find("Imagen Elegida").GetComponent<RawImage>();
        imagenPreview = GameObject.Find("Imagen Preview");
        inputField = GameObject.Find("TextoURL").GetComponent<Text>();

        imagenPreview.SetActive(false);
    }

    /* -------------------------------------------------------------------------------- */

    public void comenzarNivel() {

        if ( !(imageSet && sizeSet) )
        {
            FindObjectOfType<PopUps>().abrirPopUp(1);
            return;
        }

        GameObject.Find("Panel Seleccion").SetActive(false);

        FindObjectOfType<Juego1>().comenzar();

    }

    /* --------------------------------------------------------------------------------  */

    public void ajustarTamaño() {

        if (!imageSet)
        {
            FindObjectOfType<PopUps>().abrirPopUp(2);
            return;
        }

        filas = Int32.Parse(GameObject.Find("TextoFilas").GetComponent<Text>().text);
        columnas = Int32.Parse(GameObject.Find("TextoColumnas").GetComponent<Text>().text);

        if (filas < 3 || columnas < 3)
        {
            FindObjectOfType<PopUps>().abrirPopUp(6);
            return;
        }
        else if (filas > 12 || columnas > 12)
        {
            FindObjectOfType<PopUps>().abrirPopUp(7);
            return;
        }

        sizeSet = true;

        FindObjectOfType<Juego1>().filas = filas;
        FindObjectOfType<Juego1>().columnas = columnas;

        FindObjectOfType<GameManager>().filas = filas;
        FindObjectOfType<GameManager>().columnas = columnas;

        if (filas > columnas) mayor = filas;
        else mayor = columnas;

        FindObjectOfType<Juego1>().RandomMoves = (mayor) * (mayor) * (mayor);

        GameObject.Find("Bloque Modelo").GetComponent<Renderer>().material.mainTexture = imagen.texture;

        FindObjectOfType<GameManager>().generarBloques();
        FindObjectOfType<GameManager>().ajustarPosiciones();
        FindObjectOfType<GameManager>().ajustarUbicacion();


        //ajustarUbicacion();

        imagenPreview.SetActive(true);
    }

   /*  -------------------------------------------------------------------------------- */

    void asignarNombre(char nombre, char nombre2, int contador) 
    {
        GameObject.Find( nombre.ToString() + nombre2.ToString() ).name = contador.ToString();
        GameObject.Find(contador.ToString()).GetComponent<Renderer>().material.mainTexture = imagen.texture;
    }

    /* -------------------------------------------------------------------------------- */

    private void destruir(char nombre, char nombre2) { Destroy(GameObject.Find(nombre.ToString() + nombre2.ToString())); }


    /* ----------------------------------- Explorer ----------------------------------- */

    public string url = "";

    public void abrirExplorer() { FindObjectOfType<PopUps>().abrirPopUp(4); }

    /* -------------------------------------------------------------------------------- */

    public void asignTexture()
    {
        url = (inputField.text).ToString();
        Debug.Log(url);
        StartCoroutine(GetTexture());
        imagen.material.color = new Vector4(0.20f, 0.20f, 0.20f, 1);
    }

    /* -------------------------------------------------------------------------------- */

    IEnumerator GetTexture()
    {
        Debug.Log("Loading ...");

        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);

        yield return www.SendWebRequest();

        Debug.Log("Loaded");

        imagen.material.color = Color.white;
        imagen.texture = ((DownloadHandlerTexture)www.downloadHandler).texture;

        FindObjectOfType<CustomLevel>().imageSet = true;
    }
}
