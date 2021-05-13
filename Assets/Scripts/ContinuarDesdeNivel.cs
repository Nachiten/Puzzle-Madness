using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ContinuarDesdeNivel : MonoBehaviour
{
    GameObject continuarDesdeNivel;

    static TMP_Text textoNivelNoGanado, textoBoton;

    string continuarString = "CONTINUAR", comenzarString = "COMENZAR";

    bool mostrandoContinuarDesdeNivel = false;

    /* -------------------------------------------------------------------------------- */

    private void Awake()
    {
        continuarDesdeNivel = GameObject.Find("ContinuarDesdeNivel");
        textoNivelNoGanado = GameObject.Find("TextoContinuar").GetComponent<TMP_Text>();

        textoBoton = GameObject.Find("TextoBotonComenzar").GetComponent<TMP_Text>();
    }

    /* -------------------------------------------------------------------------------- */

    void Start()
    {
        textoBoton.text = comenzarString;

        if (yaJugoAntes())
        {
            textoBoton.text = continuarString;
            mostrarUltimoNivelNoGanado();
            mostrandoContinuarDesdeNivel = true;
        }
        else
        {
            continuarDesdeNivel.SetActive(false);
            PlayerPrefs.SetInt("YaJugoAntes", 1);
        }
    }

    /* -------------------------------------------------------------------------------- */

    public void ocultarContinuarDesdeNivelSiCorresponde()
    {
        if (mostrandoContinuarDesdeNivel)
        {
            bool continuarDesdeNivelActivo = continuarDesdeNivel.activeSelf;
            continuarDesdeNivel.SetActive(!continuarDesdeNivelActivo);
        }
    }

    /* -------------------------------------------------------------------------------- */

    void mostrarUltimoNivelNoGanado()
    {
        int indexNoGanado = 0;

        for (int i = 1; i <= 22; i++)
        {
            if (i == 11 || i == 12)
                continue;

            if (PlayerPrefs.GetString(i.ToString()) != "Ganado")
            {
                indexNoGanado = i;
                break;
            }
        }

        int juegoNoGanado = 0;
        int nivelNoGanado = 0;

        // Si es juego1
        if (indexNoGanado < 11)
        {
            juegoNoGanado = 1;
            nivelNoGanado = indexNoGanado;
            FindObjectOfType<Buttons>().nivelACargar = indexNoGanado;
        }
        // Si es juego2
        else if (indexNoGanado < 23)
        {
            juegoNoGanado = 2;
            nivelNoGanado = indexNoGanado - 12;
            FindObjectOfType<Buttons>().nivelACargar = indexNoGanado;
        }

        textoNivelNoGanado.text = "Juego: " + juegoNoGanado + " | Nivel: " + nivelNoGanado;
        continuarDesdeNivel.SetActive(true);

        // Ya gano todos los niveles (o no gano nada) (vuelvo a poner comenzar)
        if (indexNoGanado == 0)
        {
            continuarDesdeNivel.SetActive(false);
            textoBoton.text = comenzarString;
            FindObjectOfType<Buttons>().nivelACargar = 1;
        }
    }

    /* -------------------------------------------------------------------------------- */

    bool yaJugoAntes()
    {
        return PlayerPrefs.GetInt("YaJugoAntes") == 1;
    }
}
