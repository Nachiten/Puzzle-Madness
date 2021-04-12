using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PopUpsMenu : MonoBehaviour
{
    int popUpOpen = 0;
    int currentImage = 0;

    public Texture[] Textura;
    RawImage simbolo;

    GameObject botonNo;
    GameObject popUp;
    //GameObject inputField;

    Text textoPrincipal;
    Text TextoBanner;
    Text botonSiTexto;

    float tiempoAnimacion = 0.18f;

    void Start()
    {
        popUp = GameObject.Find("Pop Up");
        botonNo = GameObject.Find("Boton No");
        //inputField = GameObject.Find("URL Imagen");

        TextoBanner = GameObject.Find("Texto Banner").GetComponent<Text>();
        botonSiTexto = GameObject.Find("BotonSiTexto").GetComponent<Text>();
        textoPrincipal = GameObject.Find("Texto Principal").GetComponent<Text>();

        simbolo = GameObject.Find("Icono").GetComponent<RawImage>();

        //inputField.SetActive(false);
        popUp.SetActive(false);

    }

    public void abrirPopUp(int num) {

        popUpOpen = num;

        animarApertura();

        botonNo.SetActive(false);

        currentImage = 0;

        botonSiTexto.text = "Ok";

        switch (num)
        {
            case 0:
                TextoBanner.text = "Confirmación de Borrado";
                textoPrincipal.text = "Está seguro que desea borrar TODO el progreso actual del juego?";
                botonSiTexto.text = "Si";
                botonNo.SetActive(true);

                break;
            case 1:
                TextoBanner.text = "Aviso Importante";
                textoPrincipal.text = "Todo el progreso del juego fue borrado satisfactoriamente.";

                break;
            case 2:
                TextoBanner.text = "Acción Cancelada";
                textoPrincipal.text = "La acción fue cancelada con éxito.";
                currentImage = 4;

                break;
            case 3:
                TextoBanner.text = "Confirmacion";
                textoPrincipal.text = "Está seguro que desea salir?";
                currentImage = 4;
                botonSiTexto.text = "Si";
                botonNo.SetActive(true);
                break;
        }

        simbolo.texture = Textura[currentImage];
    }

    public void cerrarPopUp( bool accionUsada) // TRUE = si FALSE = no
    {
        Debug.Log("Cerrando popup");
        LeanTween.moveLocalX(popUp, 1500, tiempoAnimacion).setOnComplete(_ => realizarAccionAlCerrar(accionUsada));
    }

    public void borrarTodasLasKeys()
    {
        Debug.LogError("BORRANDO TODAS LAS KEYS !!!!");
        PlayerPrefs.DeleteAll();

        int indexLevelSelector = 12;

        if (SceneManager.GetActiveScene().buildIndex == indexLevelSelector) GameObject.Find("GameManager").GetComponent<LevelLoader>().cargarNivel(indexLevelSelector);
    }

    void animarApertura() 
    {
        // Posicion inicial
        LeanTween.moveLocalX(popUp, -1500, 0f).setOnComplete(_ => popUp.SetActive(true));

        Debug.Log("Animando apertura");

        LeanTween.moveLocalX(popUp, 0, tiempoAnimacion);  
    }

    void realizarAccionAlCerrar(bool accionUsada) 
    {

        Debug.Log("Entre en accion a realizar");

        popUp.SetActive(false);

        if (popUpOpen == 0)
        {

            if (accionUsada)
            {
                borrarTodasLasKeys();
                abrirPopUp(1);
            }
            else
            {
                abrirPopUp(2);
            }
        }
        else if (popUpOpen == 3)
        {
            if (accionUsada)
            {
                GameObject.Find("GameManager").GetComponent<LevelLoader>().salir();
            }
        }
    }
}

