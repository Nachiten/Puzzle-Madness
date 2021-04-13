using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ManejarMenu : MonoBehaviour
{
    // Flag de menu abierto
    public bool menuActivo = true;

    // Flag de ya asigne las variables
    static bool variablesAsignadas = false;

    // Menu pausa
    static GameObject menu;
    static LeanTweenManager tweenManager;
    
    // Boton Continuar/Comenzar
    static Text boton;

    // Index de escena actual
    int index;

    /* -------------------------------------------------------------------------------- */

    void Start()
    {
        index = SceneManager.GetActiveScene().buildIndex;
        tweenManager = GameObject.Find("Canvas Menu").GetComponent<LeanTweenManager>();

        if (menuActivo)
        {
            if (!variablesAsignadas)
            {
                menu = GameObject.Find("Menu");
                boton = GameObject.Find("TextoBotonComenzar").GetComponent<Text>();
                variablesAsignadas = true;
            }

            menuActivo = false;
        }

        if (index != 0)
        {
            boton.text = "Continuar";
            tweenManager.cerrarMenu();
        }
        else {
            boton.text = "Comenzar";
            menu.SetActive(true);
        }
    }

    /* -------------------------------------------------------------------------------- */

    void Update()
    {
        index = SceneManager.GetActiveScene().buildIndex;

        if (index == 0) return;

        bool animacionEnEjecucion = GameObject.Find("Canvas Menu").GetComponent<LeanTweenManager>().animacionEnEjecucion;

        if (Input.GetKeyDown("escape") && !animacionEnEjecucion) 
            manejarMenu();
    }

    /* -------------------------------------------------------------------------------- */

    public void manejarMenu() {
        
        menuActivo = !menuActivo;

        if (menuActivo)
        {
            menu.SetActive(true);
            tweenManager.abrirMenu();
        }
        else 
        {
            tweenManager.cerrarMenu();
        }

        // Si es Juego1
        if (index < 11 && FindObjectOfType<Juego1>().start) activarTimer();

        // Si es Juego2
        if (index > 12 && FindObjectOfType<Juego2>().start)
        { 
            FindObjectOfType<Juego2>().pause = menuActivo;
            activarTimer();
        }
    }

    /* -------------------------------------------------------------------------------- */

    void activarTimer() { FindObjectOfType<Timer>().toggleClock(!menuActivo); }
}
