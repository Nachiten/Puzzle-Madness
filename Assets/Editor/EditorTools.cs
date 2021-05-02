using System.Collections;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class EditorTools : EditorWindow
{
    // Mostrar Ventana
    [MenuItem("Window/[EditorTools]")]
    public static void ShowWindow()
    {
        GetWindow<EditorTools>("EditorTools");
    }

    // --------------------------------------------------------------------------------

    bool menuViajarEscenasAbierto = false;
    bool menuGanarNivelAbierto = false;

    int cantidadEscenas;

    private void OnEnable()
    {
        cantidadEscenas= SceneManager.sceneCountInBuildSettings;
    }

    // Codigo de la Vetana
    void OnGUI()
    {
        if (!Application.isPlaying)
        {
            EditorGUILayout.LabelField("----------------------------------------------------------------------");
            EditorGUILayout.LabelField("------ Debes comenzar a jugar para ver las opciones de este menu.  ------");
            EditorGUILayout.LabelField("----------------------------------------------------------------------");
            return;
        }

        EditorGUILayout.LabelField("Viajar hacia escena:");

        string textoBoton = "Abrir Menu";
        if (menuViajarEscenasAbierto)
            textoBoton = "Cerrar Menu";

        if (GUILayout.Button(textoBoton))
        {
            menuViajarEscenasAbierto = !menuViajarEscenasAbierto;
        }

        if (menuViajarEscenasAbierto)
        {
            mostrarMenuViajarAEscena();
        }

        EditorGUILayout.LabelField("Ganar Nivel Actual:");

        if (GUILayout.Button("Ganar nivel actual"))
        {
            int indexActual = SceneManager.GetActiveScene().buildIndex;

            if (indexActual == 0 || indexActual == 12)
            {
                Debug.LogError("[EditorTools] No hay ningun nivel que ganar.");
            }
            else
            {
                Debug.Log("[EditorTools] Ganando nivel...");

                GameObject.Find("GameManager").GetComponent<GameManager>().ganoJuego();
            }
        }

        EditorGUILayout.LabelField("Ganar nivel especifico:");

        string textoBoton2 = "Abrir Menu";
        if (menuGanarNivelAbierto)
            textoBoton2 = "Cerrar Menu";

        if (GUILayout.Button(textoBoton2))
        {
            menuGanarNivelAbierto = !menuGanarNivelAbierto;
        }

        if (menuGanarNivelAbierto) 
        {
            mostrarMenuGanarNivel(false);
        }

        EditorGUILayout.LabelField("Borrar Todas las Keys:");

        if (GUILayout.Button("BORRAR TODO"))
        {
            PlayerPrefs.DeleteAll();

            if (SceneManager.GetActiveScene().buildIndex == 12)
            {
                GameObject.Find("GameManager").GetComponent<LevelLoader>().cargarNivel(12);
            }
        }

        EditorGUILayout.LabelField("Ganar todos los niveles:");

        if (GUILayout.Button("Ganar Todo"))
        {
            mostrarMenuGanarNivel(true);
        }
    }

    void mostrarMenuViajarAEscena() 
    {
        for (int i = 0; i < cantidadEscenas; i++)
        {
            string nombreEscena = System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));

            if (GUILayout.Button("Ir a escena: [" + nombreEscena + "]"))
            {
                GameObject.Find("GameManager").GetComponent<LevelLoader>().cargarNivel(i);
            }
        }
    }

    void mostrarMenuGanarNivel(bool ganaDirecto) 
    {
        for (int i = 0; i < cantidadEscenas; i++)
        {
            if (i == 0 || i == 12)
                continue;

            string nombreEscena = System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));

            if (GUILayout.Button("Ganar Nivel: [" + nombreEscena + "]") || ganaDirecto)
            {
                PlayerPrefs.SetString(i.ToString(), "Ganado");
                PlayerPrefs.SetFloat("Time_" + i, 25000f);

                if (SceneManager.GetActiveScene().buildIndex == 12 && !ganaDirecto) 
                {
                    GameObject.Find("GameManager").GetComponent<LevelLoader>().cargarNivel(12);
                }
            }
        }

        if (SceneManager.GetActiveScene().buildIndex == 12 && ganaDirecto)
        {
            GameObject.Find("GameManager").GetComponent<LevelLoader>().cargarNivel(12);
        }
    }
}

