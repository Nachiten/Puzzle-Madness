using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropdownControl : MonoBehaviour
{

    /* -------------------------------------------------------------------------------- */

    public void cambiarCancionA(int cancion) 
    {
        MusicManager musicManager = GameObject.Find("MusicManager").GetComponent<MusicManager>();

        if (cancion != 3)
            musicManager.reproducirMusica(cancion);
        else
            musicManager.pararMusica();
    }

    /* -------------------------------------------------------------------------------- */

}

