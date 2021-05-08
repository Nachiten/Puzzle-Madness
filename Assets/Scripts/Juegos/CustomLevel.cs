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
    Button botonTamaño;
    TMP_Text textoPreview;

    IJuegos interfazActual;

    /* -------------------------------------------------------------------------------- */

    void Start() 
    {
        textoPreview = GameObject.Find("TextoPreview").GetComponent<TMP_Text>();
        botonTamaño = GameObject.Find("BotonTamaño").GetComponent<Button>();
        imagen = GameObject.Find("Imagen Elegida").GetComponent<RawImage>();
        imagenPreview = GameObject.Find("Imagen Preview");

        imagenPreview.SetActive(false);

        int index = SceneManager.GetActiveScene().buildIndex;

        if (index == 11)
            interfazActual = GameObject.Find("GameManager").GetComponent<Juego1>().GetComponent<IJuegos>();
        else
            interfazActual = GameObject.Find("GameManager").GetComponent<Juego2>().GetComponent<IJuegos>();
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

        interfazActual.inicializar();
    }

    /* --------------------------------------------------------------------------------  */

    public void ajustarTamaño()
    {
        sizeSet = false;
        imagenPreview.SetActive(false);
        botonTamaño.interactable = false;

        GameObject.Find("SoundManager").GetComponent<SoundManager>().reproducirSonido(1);

        if (!imageSet)
        {
            FindObjectOfType<PopUps>().abrirPopUp(2);
            botonTamaño.interactable = true;
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
            Debug.Log("[CustomLevel] Excepcion: " + e.Message);
            FindObjectOfType<PopUps>().abrirPopUp(8);
            botonTamaño.interactable = true;
            return;
        }

        //Debug.Log("[CustomLevel] Filas: " + filas);
        //Debug.Log("[CustomLevel] Columnas: " + columnas);

        if (filas < 3 || columnas < 3)
        {
            FindObjectOfType<PopUps>().abrirPopUp(6);
            botonTamaño.interactable = true;
            return;
        }
        else if (filas > 12 || columnas > 12)
        {
            FindObjectOfType<PopUps>().abrirPopUp(7);
            botonTamaño.interactable = true;
            return;
        }

        textoPreview.text = "Generando\n nivel...";

        interfazActual.fijarFilasYColumnas(filas, columnas);

        GameObject.Find("Bloque Modelo").GetComponent<Renderer>().material.mainTexture = imagen.texture;

        FindObjectOfType<GameManager>().comenzarCustomLevel();
    }

    public void terminarAjustarTamaño() 
    {
        sizeSet = true;
        imagenPreview.SetActive(true);
        botonTamaño.interactable = true;
        textoPreview.text = "Luego de elegir el\n tamaño verás\n la vista previa\n aqui";
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
        Debug.Log("[CustomLevel URL Ingresado: " + url);
        StartCoroutine(GetTexture(url));
        imagen.material.color = new Vector4(0.20f, 0.20f, 0.20f, 1);
    }

    /* -------------------------------------------------------------------------------- */

    IEnumerator GetTexture(string url)
    {
        Debug.Log("[CustomLevel] Cargando imagen personalizada...");

        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);

        yield return www.SendWebRequest();

        imagen.material.color = Color.white;

        imagen.texture = ((DownloadHandlerTexture)www.downloadHandler).texture;

        Debug.Log("[CustomLevel] Imagen cargada correctamente.");

        imageSet = true;
    }
}
