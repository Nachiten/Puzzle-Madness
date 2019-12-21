using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    /* -------------------------------------------------------------------------------- */

    public void Comenzar()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0) loadLevel(1);

        else FindObjectOfType<ManejarMenu>().manejarMenu();
    }

    /* -------------------------------------------------------------------------------- */

    //public void SeleccionarNivel() { loadLevel(7);}

    /* -------------------------------------------------------------------------------- */

    public void Salir() { GameObject.Find("GameManager").GetComponent<LevelLoader>().salir(); }

    /* -------------------------------------------------------------------------------- */

    public void loadLevel(int index) { GameObject.Find("GameManager").GetComponent<LevelLoader>().cargarNivel(index); }

    public void borrarProgreso() {
        GameObject.Find("GameManager").GetComponent<LevelLoader>().borrarTodasLasKeys();

        int indexLevelSelector = 12;

        if (SceneManager.GetActiveScene().buildIndex == indexLevelSelector) GameObject.Find("GameManager").GetComponent<LevelLoader>().cargarNivel(indexLevelSelector);
    }
}