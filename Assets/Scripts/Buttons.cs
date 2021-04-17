using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    public int nivelACargar = 1;

    /* -------------------------------------------------------------------------------- */

    public void Comenzar()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0) 
            loadLevel(nivelACargar);
        else 
            FindObjectOfType<ManejarMenu>().manejarMenu();
    }

    /* -------------------------------------------------------------------------------- */

    public void Salir() { GameObject.Find("GameManager").GetComponent<LevelLoader>().salir(); }

    /* -------------------------------------------------------------------------------- */

    public void loadLevel(int index) 
    {
        GameObject.Find("GameManager").GetComponent<LevelLoader>().cargarNivel(index); 
    }

    public void manejarOpciones() 
    {
        GameObject.Find("SoundManager").GetComponent<SoundManager>().reproducirSonido(1);
        GameObject.Find("GameManager").GetComponent<ManejarMenu>().manejarOpciones(); 
    }

    public void manejarCreditos() 
    {
        GameObject.Find("SoundManager").GetComponent<SoundManager>().reproducirSonido(1);
        GameObject.Find("GameManager").GetComponent<ManejarMenu>().manejarCreditos(); 
    }
}