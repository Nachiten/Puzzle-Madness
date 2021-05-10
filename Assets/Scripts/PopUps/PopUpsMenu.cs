using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopUpsMenu : MonoBehaviour
{
    public Texture[] Textura;

    static int popUpOpen = 0, currentImage = 0;
    static bool variablesSeteadas = false;

    static RawImage simbolo;
    static GameObject botonNo, popUp;
    static TMP_Text textoPrincipal, textoBanner, botonSiTexto;

    float tiempoAnimacion = 0.18f;

    /* -------------------------------------------------------------------------------- */

    void Start()
    {
        //Debug.Log("[PopsUpsMenu] Start()");

        if (!variablesSeteadas) 
        {
            popUp = GameObject.Find("Pop Up");
            botonNo = GameObject.Find("Boton No");

            textoBanner = GameObject.Find("Texto Banner").GetComponent<TMP_Text>();
            botonSiTexto = GameObject.Find("BotonSiTexto").GetComponent<TMP_Text>();
            textoPrincipal = GameObject.Find("Texto Principal").GetComponent<TMP_Text>();

            simbolo = GameObject.Find("Icono").GetComponent<RawImage>();

            variablesSeteadas = true;
        }
        popUp.SetActive(false);
    }

    /* -------------------------------------------------------------------------------- */

    public void abrirPopUp(int num) {

        popUpOpen = num;

        animarApertura();

        // Configs default
        botonNo.SetActive(false);
        currentImage = 0;
        botonSiTexto.text = "Ok";

        switch (num)
        {
            case 0:
                GameObject.Find("SoundManager").GetComponent<SoundManager>().reproducirSonido(1);
                textoBanner.text = "Confirmación de Borrado";
                textoPrincipal.text = "Está seguro que desea borrar TODO el progreso actual del juego?";
                botonSiTexto.text = "Si";
                botonNo.SetActive(true);

                break;
            case 1:
                textoBanner.text = "Aviso Importante";
                textoPrincipal.text = "Todo el progreso del juego fue borrado satisfactoriamente.";

                break;
            case 2:
                textoBanner.text = "Acción Cancelada";
                textoPrincipal.text = "La acción fue cancelada con éxito.";
                currentImage = 4;

                break;
            case 3:
                textoBanner.text = "Confirmacion";
                textoPrincipal.text = "Está seguro que desea salir?";
                currentImage = 4;
                botonSiTexto.text = "Si";
                botonNo.SetActive(true);
                break;
        }

        simbolo.texture = Textura[currentImage];
    }

    /* -------------------------------------------------------------------------------- */

    void animarApertura()
    {
        // Posicion inicial
        LeanTween.moveLocalX(popUp, -1500, 0f).setOnComplete(_ => popUp.SetActive(true));

        Debug.Log("[PopUpsMenu] Animando apertura de PopUp");

        LeanTween.moveLocalX(popUp, 0, tiempoAnimacion);
    }

    /* -------------------------------------------------------------------------------- */

    public void cerrarPopUp( bool accionUsada) // TRUE = si FALSE = no
    {
        Debug.Log("[PopUpsMenu] Animando cerrado de PopUp");
        LeanTween.moveLocalX(popUp, 1500, tiempoAnimacion).setOnComplete(_ => realizarAccionAlCerrar(accionUsada));
    }

    /* -------------------------------------------------------------------------------- */

    public void borrarTodasLasKeys()
    {
        Debug.LogError("[PopUpsMenu] BORRANDO TODAS LAS KEYS !!!!");
        PlayerPrefs.DeleteAll();
    }

    /* -------------------------------------------------------------------------------- */

    void realizarAccionAlCerrar(bool accionUsada) 
    {
        // Cierro el popup
        popUp.SetActive(false);

        switch (popUpOpen) 
        {
            case 0:
                if (accionUsada)
                {
                    borrarTodasLasKeys();
                    abrirPopUp(1);
                }
                else
                    abrirPopUp(2);
                

                break;
            case 1:
                GameObject.Find("GameManager").GetComponent<LevelLoader>().cargarNivel(0);

                break;
            case 3:
                if (accionUsada)
                    GameObject.Find("GameManager").GetComponent<LevelLoader>().salir();

                break;

        }
    }
}

