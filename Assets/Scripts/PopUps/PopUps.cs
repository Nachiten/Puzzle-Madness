﻿using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class PopUps : MonoBehaviour
{
    public Texture[] Textura;

    int popUpOpen = 0, currentImage = 0;

    RawImage simbolo;

    GameObject botonNo, popUp, inputField;

    TMP_Text textoPrincipal, TextoBanner, inputFieldTexto, botonSiTexto;

    float tiempoAnimacion = 0.18f;

    /* -------------------------------------------------------------------------------- */

    private void Awake()
    {
        popUp = GameObject.Find("Pop Up");
        botonNo = GameObject.Find("Boton No");
        inputField = GameObject.Find("URL Imagen");

        inputFieldTexto = GameObject.Find("TextoURL").GetComponent<TMP_Text>();
        TextoBanner = GameObject.Find("Texto Banner").GetComponent<TMP_Text>();
        botonSiTexto = GameObject.Find("BotonSiTexto").GetComponent<TMP_Text>();
        textoPrincipal = GameObject.Find("Texto Principal").GetComponent<TMP_Text>();

        simbolo = GameObject.Find("Icono").GetComponent<RawImage>();
    }

    /* -------------------------------------------------------------------------------- */

    void Start()
    {
        inputField.SetActive(false);
        popUp.SetActive(false);
    }

    /* -------------------------------------------------------------------------------- */

    public void abrirPopUp(int num)
    {

        popUpOpen = num;

        animarApertura();

        // Configs default
        botonNo.SetActive(false);
        currentImage = 0;
        botonSiTexto.text = "Ok";

        switch (num)
        {
            case 1:
                TextoBanner.text = "Respetar Orden";
                textoPrincipal.text = "Debes seleccionar primero una imagen y luego un tamaño antes de comenzar.";

                break;
            case 2:
                TextoBanner.text = "Respetar Orden";
                textoPrincipal.text = "Debes primero seleccionar una imagen antes de seleccionar el tamaño.";

                break;
            case 3:
                botonNo.SetActive(true);

                TextoBanner.text = "Respetar Orden";
                textoPrincipal.text = "Para cambiar el tamaño que ya fue elegido previamente se debe reiniciar el nivel en este momento.";
                botonSiTexto.text = "Si";

                break;
            case 4:
                inputField.SetActive(true);

                currentImage = 3;
                TextoBanner.text = "Seleccionar imagen a usar [Se recomienda el sitio imgur.com]";
                textoPrincipal.text = "Por favor ingresar el link de una imagen en uno de los siguientes formatos:          .PNG .JPG .JPEG.";
                botonSiTexto.text = "Listo";

                break;
            case 5:
                currentImage = 1;
                TextoBanner.text = "Seleccionar Imagen Correcta";
                textoPrincipal.text = "Debes seleccionar una imagen valida en formato .PNG .JPG o .JPEG.";

                break;

            case 6:
                TextoBanner.text = "Tamaño no valido";
                textoPrincipal.text = "El tamaño de la matriz debe ser 3x3 o mas.";

                break;

            case 7:
                TextoBanner.text = "Tamaño no valido";
                textoPrincipal.text = "El tamaño de la matriz debe ser 12x12 o menos.";

                break;
            case 8:
                TextoBanner.text = "Ingreso no valido";
                textoPrincipal.text = "Se debe ingresar un numero valido en ambos campos.";

                break;
        }

        simbolo.texture = Textura[currentImage];
    }

    /* -------------------------------------------------------------------------------- */

    void animarApertura()
    {
        // Posicion inicial
        LeanTween.moveLocalX(popUp, -1500, 0f).setOnComplete(_ => popUp.SetActive(true));

        LeanTween.moveLocalX(popUp, 0, tiempoAnimacion);
    }

    /* -------------------------------------------------------------------------------- */

    public void cerrarPopUp(bool accionUsada) // TRUE = si FALSE = no
    {
        GameObject.Find("SoundManager").GetComponent<SoundManager>().reproducirSonido(1);

        LeanTween.moveLocalX(popUp, 1500, tiempoAnimacion).setOnComplete(_ => realizarAccionAlCerrar(accionUsada));
    }

    /* -------------------------------------------------------------------------------- */

    void realizarAccionAlCerrar(bool accionUsada)
    {
        // Cierro el popup
        popUp.SetActive(false);

        switch (popUpOpen)
        {
            case 3:
                if (accionUsada) 
                    FindObjectOfType<LevelLoader>().cargarNivel(6);

                break;
            case 4:
                string url = (inputFieldTexto.text).ToString();
                url = url.Substring(0, url.Length - 1);

                if (url != "" && extensionValidaDeUrl(url))
                    FindObjectOfType<CustomLevel>().asignTexture(url);
                
                else
                    abrirPopUp(5);

                break;
        }

        inputField.SetActive(false);
    }

    /* -------------------------------------------------------------------------------- */

    bool extensionValidaDeUrl(string url) {
        return url.Substring(Math.Max(0, url.Length - 4)) == ".png" 
            || url.Substring(Math.Max(0, url.Length - 4)) == ".jpg" 
            || url.Substring(Math.Max(0, url.Length - 5)) == ".jpeg";
    }
}
