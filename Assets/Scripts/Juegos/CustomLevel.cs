using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using TMPro;
using System;
using System.Collections;

public class CustomLevel : MonoBehaviour
{
    // Tamaño de tabla
    public int filas = 3, columnas = 3;

    bool imageSet = false, sizeSet = false;

    RawImage imagen;

    GameObject imagenPreview;

    int juegoActual;

    /* -------------------------------------------------------------------------------- */

    void Start() 
    {
        imagen = GameObject.Find("Imagen Elegida").GetComponent<RawImage>();
        imagenPreview = GameObject.Find("Imagen Preview");

        imagenPreview.SetActive(false);

        int index = SceneManager.GetActiveScene().buildIndex;

        juegoActual = 1;

        if (index == 23)
            juegoActual = 2;
    }

    /* -------------------------------------------------------------------------------- */

    public void comenzarNivel()
    {
        GameObject.Find("SoundManager").GetComponent<SoundManager>().reproducirSonido(1);

        if ( !(imageSet && sizeSet) )
        {
            FindObjectOfType<PopUps>().abrirPopUp(1);
            return;
        }

        GameObject.Find("Panel Seleccion").SetActive(false);

        if (juegoActual == 1)
            FindObjectOfType<Juego1>().inicializar();
        else
            FindObjectOfType<Juego2>().inicializar();
    }

    /* --------------------------------------------------------------------------------  */

    public void ajustarTamaño()
    {
        GameObject.Find("SoundManager").GetComponent<SoundManager>().reproducirSonido(1);

        if (!imageSet)
        {
            FindObjectOfType<PopUps>().abrirPopUp(2);
            return;
        }

        string textoFilas = GameObject.Find("TextoFilas").GetComponent<TMP_Text>().text;
        string textoColumnas = GameObject.Find("TextoColumnas").GetComponent<TMP_Text>().text;

        textoFilas = textoFilas.Substring(0, textoFilas.Length - 1);
        textoColumnas = textoColumnas.Substring(0, textoColumnas.Length - 1);

        // Intengo de parsear filas y columnas
        try
        {
            filas = int.Parse(textoFilas);
            columnas = int.Parse(textoColumnas);
        }
        // Si hay un fallo al parsear
        catch (Exception e)
        {
            Debug.Log(e.Message);
            FindObjectOfType<PopUps>().abrirPopUp(8);
            return;
        }

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

        if (juegoActual == 1)
            FindObjectOfType<Juego1>().fijarFilasYColumnas(filas, columnas);
        else
            FindObjectOfType<Juego2>().fijarFilasYColumnas(filas, columnas);

        GameObject.Find("Bloque Modelo").GetComponent<Renderer>().material.mainTexture = imagen.texture;

        FindObjectOfType<GameManager>().comenzarJuego1();

        imagenPreview.SetActive(true);
    }

    /* ----------------------------------- Explorer ----------------------------------- */

    public void abrirExplorer()
    {
        GameObject.Find("SoundManager").GetComponent<SoundManager>().reproducirSonido(1);
        FindObjectOfType<PopUps>().abrirPopUp(4);
    }

    /* -------------------------------------------------------------------------------- */

    public void asignTexture(string url)
    {
        Debug.Log("[CustomLevelJuego1] URL Ingresado: " + url);
        StartCoroutine(GetTexture(url));
        imagen.material.color = new Vector4(0.20f, 0.20f, 0.20f, 1);
    }

    /* -------------------------------------------------------------------------------- */

    IEnumerator GetTexture(string url)
    {
        Debug.Log("[CustomLevelJuego1] Cargando imagen personalizada...");

        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);

        yield return www.SendWebRequest();

        Debug.Log("[CustomLevelJuego1] Cargando imagen correctamente...");

        imagen.material.color = Color.white;
        imagen.texture = ((DownloadHandlerTexture)www.downloadHandler).texture;

        imageSet = true;
    }
}
