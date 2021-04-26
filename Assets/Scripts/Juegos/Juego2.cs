using UnityEngine;
using UnityEngine.SceneManagement;

public class Juego2 : MonoBehaviour
{
    #region Variables Publicas

    // Tamaño de tabla
    public int columnas = 3, filas = 3;

    // Flag de pausado, comienzo, ganarHack
    public bool pause = false, start = false, ganarHack = false;

    #endregion

    /* -------------------------------------------------------------------------------- */

    #region Variables Privadas

    // Objeto y posicion correcta objeto
    GameObject objetoAgarrado;
    Transform lugarCorrectoObjeto;

    // Objeto agarrado, juego ganado
    bool hayObjetoAgarrado = false, gano = false;

    // Offset de posicion
    Vector3 offset;

    // Posicion Z
    float coordZ, limite = 1.6f, elevamiento = 0.5f;

    Texture[] texturas;

    int puntosTotales, puntosActuales = 0;

    #endregion

    /* -------------------------------------------------------------------------------- */

    void Start()
    {
        puntosTotales = filas * columnas;

        int index = SceneManager.GetActiveScene().buildIndex;
         
        cambiarTexturas();

        if (index < 23) 
        {
            Renderer rendererModelo = GameObject.Find("Bloque Modelo").GetComponent<Renderer>();

            rendererModelo.material.mainTexture = texturas[index - 13];

            Mesh mesh = rendererModelo.GetComponent<MeshFilter>().mesh;
            Vector2[] UVs = new Vector2[mesh.vertices.Length];

            float xMin = 0.334f;
            float xMax = 0.666f;

            float yMin = 0.0f;
            float yMax = 0.333f;

            // Parte de arriba de la textura
            UVs[4] = new Vector2(xMin, yMax);
            UVs[5] = new Vector2(xMax, yMax);
            UVs[8] = new Vector2(xMin, yMin);
            UVs[9] = new Vector2(xMax, yMin);

            float cero = 0f;
            float unTercio = 0.333f;

            // El resto (todo toma color negro
            for (int i = 0; i<= 20; i += 4) 
            {
                if (i != 4 && i != 8)
                    UVs[i] = new Vector2(cero, cero);
                if (i+1 != 5 && i+1 != 9)
                    UVs[i+1] = new Vector2(unTercio, cero);

                UVs[i+2] = new Vector2(cero, unTercio);
                UVs[i+3] = new Vector2(unTercio, unTercio);
            }

            mesh.uv = UVs;

            // Comenzar juego desde GameManager
            FindObjectOfType<GameManager>().comenzarJuego2();

            // Inicializacion del juego
            inicializar();
        }
        
    }

    /* -------------------------------------------------------------------------------- */

    void Update()
    {
        if (ganarHack)
        {
            ganarJuego();
            ganarHack = false;
        }

        // No se hace nada en caso de estar en pausa, no haber comenzado o haber ganado
        if (pause || gano) return;

        // Si no hay un objeto agarrado, se trata de agarrar
        if (!hayObjetoAgarrado) 
            tirarRayoParaTocarObjeto();
        
        // Si hay un objeto agarrado, se mueve el bloque
        else  
        {
            objetoAgarrado.transform.position = GetMouseAsWorldPoint() + offset + new Vector3(0, elevamiento, 0);

            // Si solte el boton
            if (Input.GetMouseButtonUp(0))
            {
                mouseButtonUp();
                hayObjetoAgarrado = false;
            }
        }

        if (puntosActuales == puntosTotales)
            ganarJuego();
        
    }

    /* -------------------------------------------------------------------------------- */

    public void fijarFilasYColumnas(int filas, int columnas) 
    {
        puntosTotales = filas * columnas;

        this.filas = filas;
        this.columnas = columnas;
    }

    void tirarRayoParaTocarObjeto() {
        // Generar rayo para "clickear" bloque
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        bool rayoTiradoYObjetoTocado = Physics.Raycast(ray, out RaycastHit hit, 3000.0f) && hit.transform != null;

        // Si tiro un rayo, toca un objeto, y clickié el mouse
        if (rayoTiradoYObjetoTocado && Input.GetMouseButtonDown(0))
        {
            if (!start) 
            {
                Debug.Log("[Juego2] Comienzo Juego...");

                // Marco juego comenzado
                start = true;

                // Activar reloj
                GameObject.Find("GameManager").GetComponent<Timer>().toggleClock(true);
            }

            // Asignar bloque seleccionado
            objetoAgarrado = hit.transform.gameObject;

            // Lugar correcto del bloque
            lugarCorrectoObjeto = GameObject.Find("Lugar_" + objetoAgarrado.name).GetComponent<Transform>();

            // Si el bloque esta en lugar correcto retornar void
            if (objetoAgarrado.transform.position == new Vector3(lugarCorrectoObjeto.position.x, lugarCorrectoObjeto.position.y + 0.2f, lugarCorrectoObjeto.position.z)) return;

            // Se agarra el bloque
            mouseButtonDown();
            hayObjetoAgarrado = true;

        }
    }

    /* -------------------------------------------------------------------------------- */

    public void inicializar() 
    {
        Debug.Log("[Juego2] Inicializando Juego2");

        // Generar los slots donde se colocarán las piezas al resolver
        generarLugares();

        // Ajustar el piso donde corresponde
        ajustarUbicacionPiso();

        // Ajustar los Lugar_X donde corresponda
        ajustarUbicacionLugares();

        // Randomizar lugares de piezas
        mezclarLugares();
    }

    /* -------------------------------------------------------------------------------- */

    void generarLugares()
    {
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

    /* -------------------------------------------------------------------------------- */

    public void ajustarUbicacionPiso()
    {
        int mayor = columnas;

        if (filas > columnas)
            mayor = filas;

        float offsetXPiso = (mayor - 3) * 1f;

        //Debug.Log("[Juego2] OffsetX aplicado a piso: " + offsetXPiso);

        tamañoXPlataforma = (5 * columnas) + 2;
        tamañoZPlataforma = (5 * filas) + 2;

        //Debug.Log("[Juego2] tamañoXPlataforma: " + tamañoXPlataforma);
        //Debug.Log("[Juego2] tamañoZPlataforma: " + tamañoZPlataforma);

        // Modificar tamaño de plataforma
        Transform plataforma = GameObject.Find("Piso").GetComponent<Transform>();
        plataforma.localScale = new Vector3(tamañoXPlataforma, plataforma.localScale.y, tamañoZPlataforma);

        posXPlataforma = plataforma.position.x - offsetXPiso;
        posZPlataforma = plataforma.position.z;

        //Debug.Log("[Juego2] posXPlataforma: " + posXPlataforma);
        //Debug.Log("[Juego2] posZPlataforma: " + posZPlataforma);

        // Modificar posicion de plataforma
        plataforma.position = new Vector3(posXPlataforma, plataforma.position.y, posZPlataforma);
    }

    /* -------------------------------------------------------------------------------- */

    void ajustarUbicacionLugares()
    {
        int contador = 1;

        float offsetX = 0;
        float offsetZ = 0;

        float posXReferencia = 0;
        float posZReferencia = 0;

        for (int i = 0; i < filas; i++)
        {
            for (int j = 0; j < columnas; j++)
            {
                if (i == 0 && j == 0)
                {
                    // Primer bloque es la referencia
                    Transform referenciaAjuste = GameObject.Find("Lugar_" + contador.ToString()).GetComponent<Transform>();

                    float offsetXReferencia = (columnas - 3) * 1.5555f;
                    float offsetYReferencia = (filas - 3) * 2.5f;

                    //Debug.Log("[Juego2] offsetX aplicado a referencia: " + offsetXReferencia);
                    //Debug.Log("[Juego2] offsetY aplicado a referencia: " + offsetXReferencia);

                    posXReferencia = referenciaAjuste.position.x + offsetXReferencia;
                    posZReferencia = referenciaAjuste.position.z + offsetYReferencia;

                    // Ajusto la posicion de la referencia
                    referenciaAjuste.position = new Vector3(posXReferencia, referenciaAjuste.position.y, posZReferencia);
                }
                else
                {
                    Transform objeto = GameObject.Find("Lugar_" + contador.ToString()).GetComponent<Transform>();

                    objeto.position = new Vector3(posXReferencia + offsetX, objeto.position.y, posZReferencia + offsetZ);
                }
                offsetX += 5;
                contador++;
            }
            offsetX = 0;
            offsetZ -= 5;
        }
    }

    /* -------------------------------------------------------------------------------- */

    float tamañoXPlataforma;
    float tamañoZPlataforma;

    float posXPlataforma;
    float posZPlataforma;

    void mezclarLugares()
    {
        int contador = 1;

        float rangoMinX = posXPlataforma - tamañoXPlataforma / 2;
        float rangoMaxX = posXPlataforma + tamañoXPlataforma / 2;

        float rangoMinZ = posZPlataforma - tamañoZPlataforma / 2;
        float rangoMaxZ = posZPlataforma + tamañoZPlataforma / 2;

        //Debug.Log("[Juego2] rangoMinX: " + rangoMinX);
        //Debug.Log("[Juego2] rangoMaxX: " + rangoMaxX);
        //Debug.Log("[Juego2] rangoMinZ: " + rangoMinZ);
        //Debug.Log("[Juego2] rangoMaxZ: " + rangoMaxZ);

        for (int i = 0; i < filas; i++)
        {
            for (int j = 0; j < columnas; j++)
            {
                Transform transform = GameObject.Find(contador.ToString()).GetComponent<Transform>();

                transform.position = new Vector3(Random.Range(rangoMinX, rangoMaxX), transform.position.y, Random.Range(rangoMinZ, rangoMaxZ));

                contador++;
            }
        }
    }

    /* -------------------------------------------------------------------------------- */

    //public void comenzarNivel() { start = true; }

    /* -------------------------------------------------------------------------------- */

    // Cuando se agarra un bloque
    void mouseButtonDown()
    {
        //Debug.Log("mouseButtonDown");

        // Set Z coordinate
        coordZ = Camera.main.WorldToScreenPoint(objetoAgarrado.transform.position).z;

        // Se actualiza offset
        offset = objetoAgarrado.transform.position - GetMouseAsWorldPoint();

    }

    /* -------------------------------------------------------------------------------- */

    // Cuando se suelta un bloque
    void mouseButtonUp()
    {
        //Debug.Log("mouseButtonUp");

        // Distancia desde objeto hasta lugar correcto
        float distancia = Vector3.Distance(objetoAgarrado.transform.position, lugarCorrectoObjeto.position);

        // Si está dentro del rango
        if (distancia < limite)
        {
            Debug.Log("[Juego2] Bloque posicionado correctamente");

            // Se coloca en el lugar correcto
            objetoAgarrado.transform.position = new Vector3(lugarCorrectoObjeto.position.x, lugarCorrectoObjeto.position.y + 0.2f, lugarCorrectoObjeto.position.z);

            puntosActuales++;

            GameObject.Find("SoundManager").GetComponent<SoundManager>().reproducirSonido(0);
        }
        // Dejarlo en la posicion donde se soltó
        else
        {
            objetoAgarrado.transform.position = new Vector3(objetoAgarrado.transform.position.x, objetoAgarrado.transform.position.y - elevamiento, objetoAgarrado.transform.position.z);
            //Debug.Log("No está en una posicion correcta");
        }
    }

    /* -------------------------------------------------------------------------------- */

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

    void ganarJuego()
    {
        gano = true;
        start = false;

        Debug.Log("[Juego2] Nivel ganado. Llamando a game manager...");
        FindObjectOfType<GameManager>().ganoJuego();
    }

    /* -------------------------------------------------------------------------------- */

    void cambiarTexturas()
    {
        texturas = new Texture[10];

        for (int i = 0; i < 10; i++)
            texturas[i] = Resources.Load("Juego2/Image" + (i + 1).ToString()) as Texture;
    }
}