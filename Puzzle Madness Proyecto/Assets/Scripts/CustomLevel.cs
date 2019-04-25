using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CustomLevel : MonoBehaviour
{
    int tamañoMatriz;

    RawImage imagen;

    GameObject imagenPreview;
    Text inputField;

    public bool imageSet = false;
    public bool sizeSet = false;

    /* -------------------------------------------------------------------------------- */

    void Start() 
    {
        imagen = GameObject.Find("Imagen Elegida").GetComponent<RawImage>();
        imagenPreview = GameObject.Find("Imagen Preview");
        inputField = GameObject.Find("TextoURL").GetComponent<Text>();

        imagenPreview.SetActive(false);
    }

    /* -------------------------------------------------------------------------------- */

    public void comenzarNivel() {

        if ( !(imageSet && sizeSet) )
        {
            FindObjectOfType<PopUps>().abrirPopUp(1);
            return;
        }

        FindObjectOfType<MovimientoBloques>().comenzar();

        GameObject.Find("Panel Seleccion").SetActive(false);
    }

    /* -------------------------------------------------------------------------------- */

    public void eliminarSobrantes() {

        if (!imageSet)
        {
            FindObjectOfType<PopUps>().abrirPopUp(2);
            return;
        }

        if (sizeSet)
        {
            FindObjectOfType<PopUps>().abrirPopUp(3);
            return;
        }

        sizeSet = true;
        imagenPreview.SetActive(true);  

        tamañoMatriz = FindObjectOfType<MovimientoBloques>().tamañoMatriz;

        char nombre = 'A';
        char nombre2 = '-';

        int contador = 1;

        for (int i = 0; i < 7; i++) {
            for (int j = 0; j < 7; j++) {
                if (i >= tamañoMatriz || j >= tamañoMatriz || (i == tamañoMatriz - 1 && j == tamañoMatriz - 1))
                {
                    if (nombre2 == '-') destruir(nombre, '\0');
                                   else destruir(nombre, nombre2);
                }
                else {
                    if (nombre2 == '-') asignarNombre(nombre, '\0', contador);
                                   else asignarNombre(nombre, nombre2, contador);
                    contador++;
                }

                if (nombre2 == '-') nombre++;

                else if (nombre2 != '-') nombre2++;

                if (nombre == 'Z' + 1) {
                    nombre = 'A';
                    nombre2 = 'A';
                }
            }
        }

        GameObject.Find("Bloque Modelo").GetComponent<Renderer>().material.mainTexture = imagen.texture;

        FindObjectOfType<MovimientoBloques>().ajustarPosiciones();

        ajustarUbicacion();
    }

    /* -------------------------------------------------------------------------------- */

    void asignarNombre(char nombre, char nombre2, int contador) 
    {
        GameObject.Find( nombre.ToString() + nombre2.ToString() ).name = contador.ToString();
        GameObject.Find(contador.ToString()).GetComponent<Renderer>().material.mainTexture = imagen.texture;
    }

    /* -------------------------------------------------------------------------------- */

    private void destruir(char nombre, char nombre2) { Destroy(GameObject.Find(nombre.ToString() + nombre2.ToString())); }

    /* -------------------------------------------------------------------------------- */

    public void dropDown(int valor){ FindObjectOfType<MovimientoBloques>().tamañoMatriz = valor + 3; }

    /* -------------------------------------------------------------------------------- */

    void ajustarUbicacion()
    {
        Transform plataforma = GameObject.Find("Piso Mapa").GetComponent<Transform>();
        plataforma.localScale = new Vector3((5 * tamañoMatriz) + 2, plataforma.localScale.y, (5 * tamañoMatriz) + 2);

        int contador = 1;
        Transform objeto;
        Transform referencia;

        float offsetX = 5;
        float offsetZ = 0;

        float posX = 0;
        float posZ = 0;

        for (int i = 0; i < tamañoMatriz; i++)
        {
            for (int j = 0; j < tamañoMatriz; j++)
            {
                if (i == 0 && j == 0)
                {
                    referencia = GameObject.Find(contador.ToString()).GetComponent<Transform>();

                    posX = referencia.position.x;
                    posZ = referencia.position.z;

                    determinarPos(ref posX, ref posZ);
                    referencia.position = new Vector3(posX, referencia.position.y, posZ);
                }
                else if (!(i == tamañoMatriz - 1 && j == tamañoMatriz - 1))
                {
                    objeto = GameObject.Find(contador.ToString()).GetComponent<Transform>();

                    objeto.position = new Vector3(posX + offsetX, objeto.position.y, posZ + offsetZ);

                    offsetX += 5;
                }
                contador++;
            }
            offsetX = 0;
            offsetZ -= 5;
        }
    }

    /* -------------------------------------------------------------------------------- */

    void determinarPos(ref float posicionX, ref float posicionZ)
    {
        switch (tamañoMatriz)
        {
            case 3:
                posicionX += 10;   posicionZ -= 10;
                break;

            case 4:
                posicionX += 7.5f; posicionZ -= 7.5f;
                break;

            case 5:
                posicionX += 5;    posicionZ -= 5;
                break;

            case 6:
                posicionX += 2.5f; posicionZ -= 2.5f;
                break;

            case 7:
                Debug.Log("Nothing");
                break;
        }
    }

    /* ----------------------------------- Explorer ----------------------------------- */

    public string url = "";

    public void abrirExplorer()
    {
        FindObjectOfType<PopUps>().abrirPopUp(4);
    }

    public void asignTexture()
    {
        url = (inputField.text).ToString();
        Debug.Log(url);
        StartCoroutine(GetTexture());
        imagen.material.color = new Vector4(0.20f, 0.20f, 0.20f, 1);
    }

    /* -------------------------------------------------------------------------------- */

    IEnumerator GetTexture()
    {
        Debug.Log("Loading ...");
        WWW wwwLoader = new WWW(url);

        yield return wwwLoader;
        Debug.Log("Loaded");

        imagen.material.color = Color.white;
        imagen.texture = wwwLoader.texture;

        FindObjectOfType<CustomLevel>().imageSet = true;
    }
}
