using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class VentanaEditor : EditorWindow
{
    bool mostrar = true;
    int escena = 200;

    GameObject pantallaCarga;

    [MenuItem("Window/[Vetana]")]

    // --------------------------------------------------------------------------------

    // Mostrar Ventana
    public static void ShowWindow()
    {
        GetWindow<VentanaEditor>("Ventana");
    }

    // --------------------------------------------------------------------------------

    // Codigo de la Vetana
    void OnGUI()
    {
        if (EditorSceneManager.GetActiveScene().buildIndex != escena)
        {
            if (pantallaCarga = GameObject.Find("Panel Carga")) escena = EditorSceneManager.GetActiveScene().buildIndex;
        }
        else
        {
            mostrar = EditorGUILayout.Toggle("Activar Panel Carga: ", mostrar);
            pantallaCarga.SetActive(mostrar);
        } 
      }
}

