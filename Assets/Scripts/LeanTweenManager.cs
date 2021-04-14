using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeanTweenManager : MonoBehaviour
{
    #region Variables

    public GameObject[] botones;
    float tiempoAnimacionBotones = 0.2f, tiempoAnimacionPanel = 0.15f; // 0.3, 0.2
    GameObject menu, panel;

    public bool animacionEnEjecucion = false;

    #endregion

    /* -------------------------------------------------------------------------------- */

    // Start is called before the first frame update
    void Start()
    {
        menu = GameObject.Find("Menu");
        panel = GameObject.Find("Panel");

        botones = new GameObject[]{
            GameObject.Find("Comenzar"),
            GameObject.Find("Seleccionar Nivel"),
            GameObject.Find("Borrar Progreso"),
            GameObject.Find("Salir")
        };
    }

    /* -------------------------------------------------------------------------------- */
    // ---------------------------- ANIMACION ARBRIR MENU ----------------------------- //
    /* -------------------------------------------------------------------------------- */

    public void abrirMenu() 
    {
        panel.SetActive(false);

        foreach (GameObject boton in botones) {
            boton.SetActive(false);
        }

        animacionEnEjecucion = true;
        Debug.Log("Inicio Animacion [ABRIR]");

        // Posicion inicial
        LeanTween.scale(panel, new Vector3(0,0,1), 0f).setOnComplete(abrirPanel);
    }

    void abrirPanel() {

        panel.SetActive(true);
        LeanTween.scale(panel, new Vector3(1, 1, 1), tiempoAnimacionPanel).setOnComplete(abrirBotones);
    }

    void abrirBotones() 
    {
        for (int i = 0; i < botones.Length; i++)
        {
            GameObject boton = botones[i];

            bool terminarAnimacion = i == botones.Length - 1;

            // Posiciones iniciales
            LeanTween.scale(boton, new Vector3(0,0.2f, 1), 0f).setOnComplete(_ => abrirBotonEnX(boton, terminarAnimacion));
        }
    }

    void abrirBotonEnX(GameObject boton, bool terminarAnimacion) 
    {
        boton.SetActive(true);
        LeanTween.scaleX(boton, 2.3f, tiempoAnimacionBotones).setOnComplete(_ => abrirBotonEnY(boton, terminarAnimacion));
    }

    void abrirBotonEnY(GameObject unBoton, bool terminarAnimacion)
    {
        if (terminarAnimacion)
            LeanTween.scaleY(unBoton, 3.1f, tiempoAnimacionBotones).setOnComplete(terminarAnimacionAbrir);
        else
            LeanTween.scaleY(unBoton, 3.1f, tiempoAnimacionBotones);
    }

    void terminarAnimacionAbrir() {
        animacionEnEjecucion = false;
        Debug.Log("Termino Animacion [ABRIR]");
    }

    /* -------------------------------------------------------------------------------- */
    // ---------------------------- ANIMACION CERRAR MENU ----------------------------- //
    /* -------------------------------------------------------------------------------- */

    public void cerrarMenu()
    {
        animacionEnEjecucion = true;
        Debug.Log("Inicio Animacion [CERRAR]");

        for (int i = 0; i < botones.Length; i++)
        {
            GameObject botonActual = botones[i];

            bool cerrarMenu = i == botones.Length - 1;

            // Posiciones iniciales
            LeanTween.scale(botonActual, new Vector3( 2.3f,3.1f,1), 0f).setOnComplete(_ => cerrarBotonEnY(botonActual, cerrarMenu));
        }

    }

    void cerrarBotonEnY(GameObject boton, bool cerrarMenu) {
        LeanTween.scaleY(boton, 0.2f, tiempoAnimacionBotones).setOnComplete(_ => cerrarBotonEnX(boton, cerrarMenu));
    }

    void cerrarBotonEnX(GameObject unBoton, bool cerrarMenu) 
    {
        //Debug.Log("Entre en cerrarMenuX");

        if (cerrarMenu)
            LeanTween.scaleX(unBoton, 0f, tiempoAnimacionBotones).setOnComplete(cerrarPanel);
        else
            LeanTween.scaleX(unBoton, 0f, tiempoAnimacionBotones);
    }

    void cerrarPanel() 
    {
        // Posicion inicial
        LeanTween.scale(panel, new Vector3(1, 1, 1), 0f);

        LeanTween.scale(panel, new Vector3(0, 0, 1), tiempoAnimacionPanel).setOnComplete(terminarAnimacionCerrar);
    }

    void terminarAnimacionCerrar() {
        Debug.Log("Termino Animacion [CERRAR]");
        animacionEnEjecucion = false;
        menu.SetActive(false);
    }

    /* -------------------------------------------------------------------------------- */
}
