using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System;

public class Explorer : MonoBehaviour
{
    string path;
    public RawImage imagen;

    public void AbrirExplorer() {

        path = EditorUtility.OpenFilePanel("Seleccionar Imagen", "", "png");
        setearImagen();
    }

    void setearImagen() {
        if (path != null && path != "" && path.Substring(Math.Max(0, path.Length - 4)) == ".png")
        {
            WWW www = new WWW("file:///" + path);
            imagen.texture = www.texture;

        }
        else {
            EditorUtility.DisplayDialog("Error !!", "Debes seleccionar una imagen valida en formato .PNG", "Bueno ...");  
        }
    }
}
