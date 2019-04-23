using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManejarMenu : MonoBehaviour
{
    static bool flag = true;
    static GameObject menu;

    /* -------------------------------------------------------------------------------- */

    void Start()
    {
        if (flag)
        {
            menu = GameObject.Find("Canvas Menu");
            flag = false;
        }

        if (SceneManager.GetActiveScene().buildIndex != 0) menu.SetActive(false);
    }

    /* -------------------------------------------------------------------------------- */

    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0) return;

        else if (Input.GetKeyDown("escape"))
        {
            menu.SetActive(!flag);
            flag = !flag;
        }
    }
}
