using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ManejarMenu : MonoBehaviour
{
    // Flag de menu abierto
    static bool flag = true;

    // Menu pausa
    static GameObject menu;

    // Boton Continuar/Comenzar
    static Text boton;

    // Index de escena actual
    int index;

    /* -------------------------------------------------------------------------------- */

    void Start()
    {
        index = SceneManager.GetActiveScene().buildIndex;

        if (flag)
        {
            menu = GameObject.Find("Menu");
            boton = GameObject.Find("TextoBotonComenzar").GetComponent<Text>();
            flag = false;
        }

        if (index != 0)
        {
            boton.text = "Continuar";
            menu.SetActive(false);
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

        else if (Input.GetKeyDown("escape")) manejarMenu();
    }

    /* -------------------------------------------------------------------------------- */

    public void manejarMenu() {

        menu.SetActive(!flag);
        flag = !flag;

        // Si es Juego1
        if (index < 11 && FindObjectOfType<MovimientoBloques>().start) activarTimer();

        // Si es Juego2
        if (index > 12 && FindObjectOfType<DragAndDrop>().start)
        { 
            FindObjectOfType<DragAndDrop>().pause = flag;
            activarTimer();
        }
    }

    /* -------------------------------------------------------------------------------- */

    void activarTimer() { FindObjectOfType<Timer>().toggleClock(!flag); }
}
