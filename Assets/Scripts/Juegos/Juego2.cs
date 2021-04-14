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

    #endregion

    /* -------------------------------------------------------------------------------- */

    void Start()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
         
        cambiarTexturas();

        if (index < 23) {
            Renderer rendererModelo = GameObject.Find("Bloque Modelo").GetComponent<Renderer>();

            rendererModelo.material.mainTexture = texturas[index - 13];

            Mesh mesh = rendererModelo.GetComponent<MeshFilter>().mesh;
            Vector2[] UVs = new Vector2[mesh.vertices.Length];

            float xMin = 0.334f;
            float xMax = 0.666f;

            float yMin = 0.0f;
            float yMax = 0.333f;

            // Top
            UVs[4] = new Vector2(xMin, yMax);
            UVs[5] = new Vector2(xMax, yMax);
            UVs[8] = new Vector2(xMin, yMin);
            UVs[9] = new Vector2(xMax, yMin);

            // Front
            UVs[0] = new Vector2(0.0f, 0.0f);
            UVs[1] = new Vector2(0.333f, 0.0f);
            UVs[2] = new Vector2(0.0f, 0.333f);
            UVs[3] = new Vector2(0.333f, 0.333f);

            // Back
            UVs[6] = new Vector2(1.0f, 0.0f);
            UVs[7] = new Vector2(0.667f, 0.0f);
            UVs[10] = new Vector2(1.0f, 0.333f);
            UVs[11] = new Vector2(0.667f, 0.333f);

            // Bottom
            UVs[12] = new Vector2(0.0f, 0.334f);
            UVs[13] = new Vector2(0.0f, 0.666f);
            UVs[14] = new Vector2(0.333f, 0.666f);
            UVs[15] = new Vector2(0.333f, 0.334f);

            // Left
            UVs[16] = new Vector2(0.334f, 0.334f);
            UVs[17] = new Vector2(0.334f, 0.666f);
            UVs[18] = new Vector2(0.666f, 0.666f);
            UVs[19] = new Vector2(0.666f, 0.334f);

            // Right        
            UVs[20] = new Vector2(0.667f, 0.334f);
            UVs[21] = new Vector2(0.667f, 0.666f);
            UVs[22] = new Vector2(1.0f, 0.666f);
            UVs[23] = new Vector2(1.0f, 0.334f);
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
        if (pause || !start || gano) return;

        // Si no hay un objeto agarrado, se trata de agarrar
        if (!hayObjetoAgarrado) 
        {
            tirarRayoParaTocarObjeto();
        }
        // Si hay un objeto agarrado, se mueve el bloque
        else  
        {
            objetoAgarrado.transform.position = GetMouseAsWorldPoint() + offset + new Vector3(0, elevamiento, 0);

            // Si solte el boton
            if (Input.GetMouseButtonUp(0))
            {
                mouseButtonUp();
                hayObjetoAgarrado = false;
                Debug.Log("hayObjetoAgarrado: FALSE");
            }
        }

        analizarGano();
    }

    /* -------------------------------------------------------------------------------- */

    void tirarRayoParaTocarObjeto() {
        // Generar rayo para "clickear" bloque
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        bool rayoTiradoYObjetoTocado = Physics.Raycast(ray, out RaycastHit hit, 3000.0f) && hit.transform != null;

        // Si tiro un rayo, toca un objeto, y clickié el mouse
        if (rayoTiradoYObjetoTocado && Input.GetMouseButtonDown(0))
        {
            // Asignar bloque seleccionado
            objetoAgarrado = hit.transform.gameObject;

            // Lugar correcto del bloque
            lugarCorrectoObjeto = GameObject.Find("Lugar_" + objetoAgarrado.name).GetComponent<Transform>();

            // Si el bloque esta en lugar correcto retornar void
            if (objetoAgarrado.transform.position == new Vector3(lugarCorrectoObjeto.position.x, lugarCorrectoObjeto.position.y + 0.2f, lugarCorrectoObjeto.position.z)) return;

            // Se agarra el bloque
            mouseButtonDown();
            hayObjetoAgarrado = true;
            Debug.Log("hayObjetoAgarrado: TRUE");

        }
    }

    /* -------------------------------------------------------------------------------- */

    public void inicializar() 
    {
        Debug.Log("Entre a inicializar Juego2");

        // Determina la posicion del modelo y de la camara
        determinarPos();

        // Generar los slots donde se colocarán las piezas al resolver
        generarLugares();

        // Ajustar los slots donde corresponde
        ajustarUbicacion();

        // Randomizar lugares de piezas
        mezclarLugares();
    }

    /* -------------------------------------------------------------------------------- */

    public void comenzarNivel() { start = true; }

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
            Debug.Log("Posicionado correctamente");

            // Se coloca en el lugar correcto
            objetoAgarrado.transform.position = new Vector3(lugarCorrectoObjeto.position.x, lugarCorrectoObjeto.position.y + 0.2f, lugarCorrectoObjeto.position.z);

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

    void analizarGano()
    {
        int contador;

        contador = 1;

        for (int i = 0; i < filas; i++) {
            for (int j = 0; j < columnas; j++) {

                Transform bloque = GameObject.Find(contador.ToString()).transform.GetComponent<Transform>();
                Transform lugarCorrecto = GameObject.Find("Lugar_" + contador.ToString()).GetComponent<Transform>();

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

    /* -------------------------------------------------------------------------------- */

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

    /* -------------------------------------------------------------------------------- */

    void determinarPos()
    {
        int mayor;

        if (columnas > filas) mayor = columnas;
            else mayor = filas;

        float valor;

        valor = 0;

        // ---- REVISAR CODIGO ----
        for (int i = 3; i <= mayor; i++)
        {

            if (i == mayor)
            {
                //posicionX -= valor;
                //posicionZ += valor;

                //Transform modelo = GameObject.Find("Modelo").GetComponent<Transform>();
                //modelo.position = new Vector3(modelo.position.x + 11, modelo.position.y + valor + 16, modelo.position.z);

                Transform camara = GameObject.Find("Main Camera").GetComponent<Transform>();
                camara.position = new Vector3(camara.position.x, camara.position.y + valor, camara.position.z);

                //Debug.Log("VALORX: " + posicionX + " | VALORZ: " + posicionZ);
            }
            valor += 2.5f;
        }
    }

    /* -------------------------------------------------------------------------------- */

    Transform plataforma;
    Transform referenciaAjuste;

    // Determinar posicion correcta de juego
    public void ajustarUbicacion()
    {
        // Modificar tamaño de plataforma
        plataforma = GameObject.Find("Piso").GetComponent<Transform>();
        plataforma.localScale = new Vector3((5 * columnas) + 2, plataforma.localScale.y, (5 * filas) + 2);

        int contador = 1;
        Transform objeto;

        float offsetX = 5;
        float offsetZ = 0;

        float posX = 0;
        float posZ = 0;

        for (int i = 0; i < filas; i++)
        {
            for (int j = 0; j < columnas; j++)
            {
                if (i == 0 && j == 0)
                {
                    // Primer bloque es la referencia
                    referenciaAjuste = GameObject.Find("Lugar_" + contador.ToString()).GetComponent<Transform>();

                    posX = referenciaAjuste.position.x;
                    posZ = referenciaAjuste.position.z;

                    determinarPos(ref posX, ref posZ);
                    referenciaAjuste.position = new Vector3(posX, referenciaAjuste.position.y, posZ);
                }
                else
                {
                    objeto = GameObject.Find("Lugar_" + contador.ToString()).GetComponent<Transform>();

                    objeto.position = new Vector3(referenciaAjuste.position.x + offsetX, objeto.position.y, referenciaAjuste.position.z + offsetZ);

                    offsetX += 5;
                }
                contador++;
            }
            offsetX = 0;
            offsetZ -= 5;
        }
    }

    /* -------------------------------------------------------------------------------- */

    // Hijo de ajustarUbicacion
    void determinarPos(ref float posicionX, ref float posicionZ)
    {
        int mayor;

        if (filas > columnas) mayor = filas;
        else mayor = columnas;

        float valorX = (columnas - 3) * 0.8f;
        float valorZ = (filas - 3) * 2.5f;

        posicionX += valorX;
        posicionZ += valorZ;

        Debug.Log("PosiciionX: " + (-valorX).ToString() + " | PosicionZ: " + valorZ);

        float offset = (mayor - 3) * 1f;

        // Modificar posicion de plataforma
        plataforma.position = new Vector3(plataforma.position.x - offset, plataforma.position.y, plataforma.position.z);
    }
}