using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeanTweenManager : MonoBehaviour
{
    public GameObject[] botones;
    float tiempoAnimacionBotones = 0.2f, tiempoAnimacionPanel = 0.15f; // 0.3, 0.2
    public GameObject menu, panel;

    public bool animacionEnEjecucion = false;

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

        //Debug.Log("Corri start");
    }

    /* -------------------------------------------------------------------------------- */

    public void ocultarMenusInicialmente() 
    {
        // Posicion inicial
        LeanTween.scale(panel, new Vector3(0, 0, 1), 0f).setOnComplete(cerrarMenu);
    }

    /* -------------------------------------------------------------------------------- */

    public void abrirMenu() 
    {
        animacionEnEjecucion = true;
        Debug.Log("Inicio Animacion [ABRIR]");

        // Posicion inicial
        LeanTween.scale(panel, new Vector3(0,0,1), 0f);

        LeanTween.scale(panel, new Vector3(1, 1, 1), tiempoAnimacionPanel).setOnComplete(abrirBotones);
    }

    /* -------------------------------------------------------------------------------- */

    void abrirBotones() 
    {
        for (int i = 0; i < botones.Length; i++)
        {
            GameObject boton = botones[i];

            bool terminarAnimacion = false;

            // Posiciones iniciales
            LeanTween.scaleX(boton, 0, 0f);
            LeanTween.scaleY(boton, 0.2f, 0f);

            if (i == botones.Length - 1)
                terminarAnimacion = true;

            LeanTween.scaleX(boton, 2.3f, tiempoAnimacionBotones).setOnComplete(_ => abrirMenuEnY(boton, terminarAnimacion));
        }
    }

    /* -------------------------------------------------------------------------------- */

    void abrirMenuEnY(GameObject unBoton, bool terminarAnimacion)
    {
        if (terminarAnimacion)
            LeanTween.scaleY(unBoton, 3.1f, tiempoAnimacionBotones).setOnComplete(loguearTerminarAnimacion);
        else
            LeanTween.scaleY(unBoton, 3.1f, tiempoAnimacionBotones);
    }

    /* -------------------------------------------------------------------------------- */

    void loguearTerminarAnimacion() {
        animacionEnEjecucion = false;
        Debug.Log("Termino Animacion [ABRIR]");
    }

    /* -------------------------------------------------------------------------------- */

    public void cerrarMenu()
    {
        animacionEnEjecucion = true;
        Debug.Log("Inicio Animacion [CERRAR]");

        for (int i = 0; i < botones.Length; i++)
        {
            GameObject botonActual = botones[i];

            // Posiciones iniciales
            LeanTween.scaleX(botonActual, 2.3f, 0f);
            LeanTween.scaleY(botonActual, 3.1f, 0f);

            bool cerrarMenu = false;

            if (i == botones.Length - 1)
                cerrarMenu = true;

            LeanTween.scaleY(botonActual, 0.2f, tiempoAnimacionBotones).setOnComplete(_ => cerrarMenuEnX(botonActual, cerrarMenu));
        }

    }

    /* -------------------------------------------------------------------------------- */

    void cerrarMenuEnX(GameObject unBoton, bool cerrarMenu) 
    {
        //Debug.Log("Entre en cerrarMenuX");

        if (cerrarMenu)
            LeanTween.scaleX(unBoton, 0f, tiempoAnimacionBotones).setOnComplete(ocularPanelYCerrarMenu);
        else
            LeanTween.scaleX(unBoton, 0f, tiempoAnimacionBotones);
    }

    /* -------------------------------------------------------------------------------- */

    void ocularPanelYCerrarMenu() 
    {
        // Posicion inicial
        LeanTween.scale(panel, new Vector3(1, 1, 1), 0f);

        LeanTween.scale(panel, new Vector3(0, 0, 1), tiempoAnimacionPanel).setOnComplete(desactivarMenuYDesactivarAnimacion);
    }

    /* -------------------------------------------------------------------------------- */

    void desactivarMenuYDesactivarAnimacion() {
        Debug.Log("Termino Animacion [CERRAR]");
        animacionEnEjecucion = false;
        menu.SetActive(false);
    }

    /* -------------------------------------------------------------------------------- */
}
