using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    public void Comenzar() {
        if (SceneManager.GetActiveScene().buildIndex == 0) loadLevel(1);

        else FindObjectOfType<ManejarMenu>().manejarMenu();
    }

    public void SeleccionarNivel() {

        loadLevel(7);
    }

    public void Salir() {

        GameObject.Find("GameManager").GetComponent<LevelLoader>().salir();
    }

    void loadLevel(int index) {
        GameObject.Find("GameManager").GetComponent<LevelLoader>().cargarNivel(index);
    }
}
