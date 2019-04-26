using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ManejarMenu : MonoBehaviour
{
    static bool flag = true;
    static GameObject menu;
    static Text boton;

    /* -------------------------------------------------------------------------------- */

    void Start()
    {
        if (flag)
        {
            menu = GameObject.Find("Canvas Menu");
            boton = GameObject.Find("TextoBotonComenzar").GetComponent<Text>();
            flag = false;
        }

        if (SceneManager.GetActiveScene().buildIndex != 0)
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
        if (SceneManager.GetActiveScene().buildIndex == 0) return;

        else if (Input.GetKeyDown("escape")) manejarMenu();
    }

    public void manejarMenu() {

        menu.SetActive(!flag);
        flag = !flag;

        if (SceneManager.GetActiveScene().buildIndex != 7) FindObjectOfType<Timer>().toggleClock(!flag);
    }
}
