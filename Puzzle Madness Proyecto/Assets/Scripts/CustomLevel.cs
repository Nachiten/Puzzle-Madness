using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

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

        sizeSet = true;

        imagenPreview.SetActive(true);


        //tamañoMatriz = FindObjectOfType<MovimientoBloques>().tamañoMatriz;

        FindObjectOfType<GameManager>().generarBloques();

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

    public void dropDown(int valor)
    {
        //FindObjectOfType<MovimientoBloques>().tamañoMatriz = valor + 3;

        Debug.Log((valor + 3).ToString() + "<- Valor+3");
        FindObjectOfType<MovimientoBloques>().RandomMoves = (valor + 3) * (valor + 3) * (valor + 3);
    }

    /* -------------------------------------------------------------------------------- */

    Transform referencia;

    void ajustarUbicacion()
    {
        Transform plataforma = GameObject.Find("Piso Mapa").GetComponent<Transform>();

        plataforma.localScale = new Vector3((5 * tamañoMatriz) + 2, plataforma.localScale.y, (5 * tamañoMatriz) + 2);

        int contador = 1;
        Transform objeto;
        
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
                    // Primer bloque es la referencia
                    referencia = GameObject.Find(contador.ToString()).GetComponent<Transform>();

                    posX = referencia.position.x;
                    posZ = referencia.position.z;

                    determinarPos(ref posX, ref posZ);
                    referencia.position = new Vector3(posX, referencia.position.y, posZ);
                }
                else if (!(i == tamañoMatriz - 1 && j == tamañoMatriz - 1))
                {
                    objeto = GameObject.Find(contador.ToString()).GetComponent<Transform>();

                    objeto.position = new Vector3(referencia.position.x + offsetX, objeto.position.y, referencia.position.z + offsetZ);

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
        float valor = 0;

        for (int i = 3; i <= tamañoMatriz; i++) {

            if (i == tamañoMatriz) {
                posicionX -= valor;
                posicionZ += valor;

                Transform modelo = GameObject.Find("Modelo").GetComponent<Transform>();
                modelo.position = new Vector3(modelo.position.x, modelo.position.y + valor, modelo.position.z);

                Transform camara = GameObject.Find("Main Camera").GetComponent<Transform>();
                camara.position = new Vector3(camara.position.x, camara.position.y + valor, camara.position.z);

                Debug.Log("VALORX: " + posicionX + " | VALORZ: " + posicionZ);
            }
            valor += 2.5f;
        }
    }

    /* ----------------------------------- Explorer ----------------------------------- */

    public string url = "";

    /* -------------------------------------------------------------------------------- */

    public void abrirExplorer() { FindObjectOfType<PopUps>().abrirPopUp(4); }

    /* -------------------------------------------------------------------------------- */

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

        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);

        //WWW wwwLoader = new WWW(url);

        yield return www.SendWebRequest();
        Debug.Log("Loaded");

        imagen.material.color = Color.white;
        imagen.texture = ((DownloadHandlerTexture)www.downloadHandler).texture;

        FindObjectOfType<CustomLevel>().imageSet = true;
    }
}
