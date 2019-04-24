using UnityEngine;
using UnityEngine.UI;

public class PopUps : MonoBehaviour
{
    int popUpOpen = 0;

    Text textoPrincipal;
    Text TextoBanner;

    GameObject botonNo;

    GameObject popUp;
    GameObject inputField;

    void Start()
    {
        textoPrincipal = GameObject.Find("Texto Principal").GetComponent<Text>();
        TextoBanner = GameObject.Find("Texto Banner").GetComponent<Text>();
        popUp = GameObject.Find("Pop Up");
        inputField = GameObject.Find("URL Imagen");
        botonNo = GameObject.Find("Boton No");

        inputField.SetActive(false);
        popUp.SetActive(false);
        
    }


    public void abrirPopUp(int num) {
        popUp.SetActive(true);
        popUpOpen = num;
        botonNo.SetActive(false);

        switch (num)
        {
        case 1:
            TextoBanner.text = "Respetar Orden";
            textoPrincipal.text = "Debes seleccionar primero una imagen y luego un tamaño antes de comenzar.";

            break;
        case 2:
            TextoBanner.text = "Respetar Orden";
            textoPrincipal.text = "Debes primero seleccionar una imagen antes de seleccionar el tamaño.";
                
            break;
        case 3:
            botonNo.SetActive(true);
            
         // If (si) => Reiniciar Nivel
         // If (no) => return
            
            TextoBanner.text = "Respetar Orden";
            textoPrincipal.text = "Para cambiar el tamaño que ya fue elegido previamente se debe reiniciar el nivel en este momento.";

             break;
        case 4:
            inputField.SetActive(true);
            TextoBanner.text = "Seleccionar imagen a usar";
            textoPrincipal.text = "";

            break;

        }
    }

    public void cerrarPopUp( bool boton) // TRUE = si FALSE = no
    {
        
        popUp.SetActive(false);

        switch (popUpOpen)
        {
            case 3:
                Debug.Log("Depende el boton presionado");
                if (boton) {

                    FindObjectOfType<LevelLoader>().cargarNivel(6);

                }
                break;
            case 4:

                Debug.Log("Seleccionar Imagen");
              



                FindObjectOfType<CustomLevel>().asignTexture();

                break;

        }

        inputField.SetActive(false);
        popUpOpen = 0;
    }
}
