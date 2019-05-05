using UnityEngine;
using UnityEngine.SceneManagement;

public class Juego2 : MonoBehaviour
{
    // << -------- Varbiables publicas -------- >>

    // Ganar juego [Debug Only]
    public bool GanarHack = false;

    // Tamaño de tabla
    public int columnas = 3;
    public int filas = 3;

    // Objeto y posicion correcta objeto
    GameObject objeto;
    Transform lugar;

    // Flag de pausado
    public bool pause = false;
    // Flag de comenzado
    public bool start = false;


    // << -------- Varbiables privadas -------- >>

    // Flag juego ganado
    bool gano = false;
    // Flag para no cambiar de bloque al tener uno seleccionado
    bool flag = true;

    // Offset de posicion
    Vector3 offset;

    // Posicion Z
    float coordZ;

    // Limite de distancia
    float limite = 1.6f;
    float elevamiento = 0.5f;

    // Position when you pick it up
    //Vector3 inicialPos;

    // Rayo
    RaycastHit hit;

    Texture[] texturas;

    /* -------------------------------------------------------------------------------- */

    void Start()
    {
        int index = SceneManager.GetActiveScene().buildIndex;

        cambiarTexturas();

        GameObject.Find("Bloque Modelo").GetComponent<Renderer>().material.mainTexture = texturas[index - 13];

        // Asignar filas y columnas de GameManager
        FindObjectOfType<GameManager>().filas = filas;
        FindObjectOfType<GameManager>().columnas = columnas;

        determinarPos();

        // Generar bloques del mapa
        FindObjectOfType<GameManager>().generarBloques();
        // Ajustar texturas
        FindObjectOfType<GameManager>().ajustarPosiciones();
        // Ajustar ubicacion de bloques
        FindObjectOfType<GameManager>().ajustarUbicacion();

        generarLugares();

        // Randomizar lugares de piezas
        mezclarLugares();
    }

    /* -------------------------------------------------------------------------------- */

    void Update()
    {
        if (GanarHack)
        {
            ganarJuego();
            GanarHack = false;
        }

        if (pause || !start || gano) return;

        // Generar rayo para "clickear" bloque
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Determina si un rayo pego contra un objeto
        if (Physics.Raycast(ray, out hit, 100.0f) && hit.transform != null) {

            if (Input.GetMouseButton(0))
            {
                if (flag)
                {
                    // Asignar bloque seleccionado
                    objeto = hit.transform.gameObject;

                    // Lugar correcto del bloque
                    lugar = GameObject.Find("Lugar_" + objeto.name).GetComponent<Transform>();

                    // Si el bloque esta en lugar correcto retornar void
                    if (objeto.transform.position == new Vector3(lugar.position.x, lugar.position.y + 0.2f, lugar.position.z)) return;

                    flag = false;
                }
                // Al agarrar el bloque
                if (Input.GetMouseButtonDown(0)) mouseButtonDown();

                // Mover bloque
                else objeto.transform.position = GetMouseAsWorldPoint() + offset + new Vector3(0, elevamiento, 0);
            }
        }
        if (Input.GetMouseButtonUp(0)) mouseButtonUp();

        analizarGano();
    }

    /* -------------------------------------------------------------------------------- */

    // Cuando se agarra un bloque
    void mouseButtonDown()
    {
        //Debug.Log("Item Picked Up");

        // Set Z coordinateF
        coordZ = Camera.main.WorldToScreenPoint(objeto.transform.position).z;

        offset = objeto.transform.position - GetMouseAsWorldPoint();

        // Set inicial position
        //inicialPos = GetMouseAsWorldPoint() + offset;
    }

    /* -------------------------------------------------------------------------------- */

    // Cuando se suelta un bloque
    void mouseButtonUp()
    {
        // Si se clickeo algo que no es un bloque se retorna
        if (hit.transform == null) return;

        // Distance from object to correct place
        float distancia = Vector3.Distance(objeto.transform.position, lugar.position);

        // If its inside the limit
        if (distancia < limite)
        {
            Debug.Log("Correctly Placed");

            // Place in correct place
            objeto.transform.position = new Vector3(lugar.position.x, lugar.position.y + 0.2f, lugar.position.z);
        }
        else // Return to start position
        {
            objeto.transform.position = new Vector3(objeto.transform.position.x, objeto.transform.position.y - elevamiento, objeto.transform.position.z);
            Debug.Log("Not correctly placed");
        }
        flag = true;

        reajustarPosiciones();
    }

    /* -------------------------------------------------------------------------------- */

    void reajustarPosiciones() {
        int contador;

        contador = 1;

        for (int i = 0; i < filas; i++) {
            for (int j = 0; j < columnas; j++) {
                Transform posicion = GameObject.Find(contador.ToString()).transform;

                if (posicion.position.y > 1)
                {
                    Debug.Log("Arreglando Posicion");
                    posicion.position = new Vector3(posicion.position.x, 0.9452782f, posicion.position.z);
                }
                //Debug.Log("Posicion Y de: " + GameObject.Find(contador.ToString()).name + " | " + posicion.position.y);

                contador++;
            }
        }
    }   

    Vector3 GetMouseAsWorldPoint()
    {
        // Coordenadas de pixel de mouse (X,Y)
        Vector3 mousePoint = Input.mousePosition;

        // Coordenadas Z del objeto
        mousePoint.z = coordZ;

        // Convertir posicion de mouse en coordenadas 3D
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    /* -------------------------------------------------------------------------------- */

    Transform bloque;
    Transform lugarCorrecto;

    void analizarGano()
    {
        int contador;

        contador = 1;

        for (int i = 0; i < filas; i++) {
            for (int j = 0; j < columnas; j++) {

                bloque = GameObject.Find(contador.ToString()).transform.GetComponent<Transform>();
                lugarCorrecto = GameObject.Find("Lugar_" + contador.ToString()).GetComponent<Transform>();

                if (bloque.position != new Vector3(lugarCorrecto.position.x, lugarCorrecto.position.y + 0.2f, lugarCorrecto.position.z))
                    return;

                contador++;
            }
        }
        Debug.Log("GANO !!");

        ganarJuego();
    }

    /* -------------------------------------------------------------------------------- */

    void mezclarLugares()
    {
        int contador = 1;

        for (int i = 0; i < filas; i++) {
            for (int j = 0; j < columnas; j++) {

                Transform transform = GameObject.Find(contador.ToString()).GetComponent<Transform>();

                transform.position = new Vector3(Random.Range(-11, 11), transform.position.y, Random.Range(-11, 11));

                contador++;
            }
        }
    }

    /* -------------------------------------------------------------------------------- */

    void ganarJuego()
    {
        gano = true;
        start = false;

        Debug.Log("Se gano nivel !! Llamando a game manager...");
        FindObjectOfType<GameManager>().ganoJuego();
    }

    /* -------------------------------------------------------------------------------- */

    void cambiarTexturas()
    {
        texturas = new Texture[10];

        for (int i = 0; i < 10; i++)
            texturas[i] = Resources.Load("Juego2/Image" + (i + 1).ToString()) as Texture;
    }

    void generarLugares() {

        GameObject referencia = GameObject.Find("Lugar_Reference");

        int contador = 1;

        float offsetX = 0f;
        float offsetZ = 0f;

        for (int i = 0; i < filas; i++)
        {
            for (int j = 0; j < columnas; j++)
            {
                    // Crear el clon
                    GameObject clon = Instantiate(Resources.Load("Lugar", typeof(GameObject))) as GameObject;
                    // Asignar nombre correcto
                    clon.name = "Lugar_" + contador.ToString();
                    // Asignar posicion de clon
                    clon.transform.position = new Vector3(referencia.transform.position.x + offsetX, referencia.transform.position.y, referencia.transform.position.z + offsetZ);
                    // Asignar rotacion de clon
                    clon.transform.rotation = referencia.transform.rotation;
                
                contador++;
                offsetX += 5f;
            }
            offsetX = 0f;
            offsetZ -= 5f;
        }
        Destroy(referencia);
    }

    void determinarPos()
    {
        int mayor;

        if (columnas > filas) mayor = columnas;
            else mayor = filas;

        float valor;

        valor = 0;

        for (int i = 3; i <= mayor; i++)
        {

            if (i == mayor)
            {
                //posicionX -= valor;
                //posicionZ += valor;

                Transform modelo = GameObject.Find("Modelo").GetComponent<Transform>();
                modelo.position = new Vector3(modelo.position.x, modelo.position.y + valor, modelo.position.z);

                Transform camara = GameObject.Find("Main Camera").GetComponent<Transform>();
                camara.position = new Vector3(camara.position.x, camara.position.y + valor, camara.position.z);

                //Debug.Log("VALORX: " + posicionX + " | VALORZ: " + posicionZ);
            }
            valor += 2.5f;
        }
    }
}