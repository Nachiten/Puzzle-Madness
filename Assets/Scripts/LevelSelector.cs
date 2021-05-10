using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class LevelSelector : MonoBehaviour
{
    GameObject juego1;
    GameObject juego2;

    public Texture[] textura;

    bool estoyEnNivel2 = false;

    // Start is called before the first frame update
    void Start()
    {
        juego1 = GameObject.Find("Canvas Juego1");
        juego2 = GameObject.Find("Canvas Juego2");

        juego1.SetActive(false);
        scanJuego(2);

        juego1.SetActive(true);
        juego2.SetActive(false);
        scanJuego(1);
    }

    /* -------------------------------------------------------------------------------- */

    void scanJuego(int nivel)
    {
        Debug.Log("Escaneando nivel: " + nivel);

        for (int i = 1; i < 11; i++)
        {
            RawImage imagen = GameObject.Find("Image" + i.ToString()).GetComponent<RawImage>();

            TMP_Text textoReloj = GameObject.Find("Timer" + i.ToString()).GetComponent<TMP_Text>();

            string index = (i + 12).ToString();

            if (nivel == 1)
            {
                index = i.ToString();

                if (PlayerPrefs.GetString(index) == "Ganado")
                {
                    TMP_Text textoMovimientos = GameObject.Find("Movements" + i.ToString()).GetComponent<TMP_Text>(); ;

                    int movimientos = PlayerPrefs.GetInt("Movements_" + index);

                    textoMovimientos.text = movimientos.ToString();
                }
                else 
                {
                    GameObject.Find("Movimientos" + i.ToString()).SetActive(false);
                }
            }
                
            if (PlayerPrefs.GetString(index) == "Ganado")
            {
                float time = PlayerPrefs.GetFloat("Time_" + index);

                string minutes = Mathf.Floor((time % 3600) / 60).ToString("00");
                string seconds = Mathf.Floor(time % 60).ToString("00");
                string miliseconds = Mathf.Floor(time % 6 * 10 % 10).ToString("0");

                textoReloj.text = minutes + ":" + seconds + ":" + miliseconds;

                imagen.texture = textura[0];
            }
            else
            {
                imagen.texture = textura[1];
                textoReloj.text = "";
            }
        }
    }

    /* -------------------------------------------------------------------------------- */

    public void cambiarNivel()
    {
        GameObject.Find("SoundManager").GetComponent<SoundManager>().reproducirSonido(1);

        estoyEnNivel2 = !estoyEnNivel2;

        juego1.SetActive(!estoyEnNivel2);
        juego2.SetActive(estoyEnNivel2);  
    }
}
