using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttons : MonoBehaviour
{
    LevelLoader codigo;

    // Start is called before the first frame update
    void Start()
    {
        codigo = GameObject.Find("GameManager").GetComponent<LevelLoader>();
    }

    public void Comenzar() {
        codigo.cargarNivel(1);
    }

    public void SeleccionarNivel() {

        codigo.cargarNivel(7);
    }

    public void Salir() {

        codigo.salir();
    }
}
