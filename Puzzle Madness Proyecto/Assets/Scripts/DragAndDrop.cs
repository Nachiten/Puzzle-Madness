using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    public int tamañoMatriz = 3;

    // Offset of position
    Vector3 offset;

    // Z Position
    float coordZ;

    // Limit of distance
    float limite = 1.6f;
    float elevamiento = 0.5f;

    // Position when you pick it up
    Vector3 inicialPos;

    // Rayo
    RaycastHit hit;

    // Flag para no cambiar de bloque al tener uno seleccionado
    bool flag = true;

    bool gameStarted = false;

    public bool pause = true;

    // Objeto y posicion correcta objeto
    GameObject objeto;
    Transform lugar;

    /* -------------------------------------------------------------------------------- */

    void Start() { ajustarPosiciones(); mezclarLugares(); }

    /* -------------------------------------------------------------------------------- */

    void Update()
    {
        if (pause) return;
        
        // Generar rayo para "clickear" bloque
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Determina si un rayo pego contra un objeto
        if (Physics.Raycast(ray, out hit, 100.0f) && hit.transform != null) {

            if (Input.GetMouseButton(0))
            {
                if (!gameStarted)
                {
                    GameObject.Find("GameManager").GetComponent<Timer>().toggleClock(true);
                    gameStarted = true;
                }

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

    void mouseButtonDown()
    {
        Debug.Log("Item Picked Up");
        // Set Z coordinateF
        coordZ = Camera.main.WorldToScreenPoint(objeto.transform.position).z;

        offset = objeto.transform.position - GetMouseAsWorldPoint();

        // Set inicial position
        inicialPos = GetMouseAsWorldPoint() + offset;
    }

    /* -------------------------------------------------------------------------------- */

    void mouseButtonUp()
    {
        if (hit.transform == null)  return;

        if (objeto == null || objeto.transform == null) {
            Debug.Log("Se retorno en funcion dos");
            return;
        }

        // When item dropped
        Debug.Log("Item dropped");

        // Distance from object to correct place
        float distancia = Vector3.Distance(objeto.transform.position, lugar.position);

        //Debug.Log("Distance: " + distancia + " | " + "Limit of Distance: " + limite);

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

    Transform bloque;
    Transform lugarCorrecto;

    void analizarGano() {
        int contador;

        contador = 1;

        for (int i = 0; i < tamañoMatriz; i++) {
            for (int j = 0; j < tamañoMatriz; j++) {

                bloque = GameObject.Find(contador.ToString()).transform.GetComponent<Transform>();
                lugarCorrecto = GameObject.Find("Lugar_" + contador.ToString()).GetComponent<Transform>();

                if (bloque.position != new Vector3(lugarCorrecto.position.x, lugarCorrecto.position.y + 0.2f, lugarCorrecto.position.z))
                    return;

                contador++;
            }
        }
            Debug.Log("GANO !!");
    }

    /* -------------------------------------------------------------------------------- */

    public void ajustarPosiciones()
    {
        Renderer modelo = GameObject.Find("Bloque Modelo").GetComponent<Renderer>();

        int contador = 1;

        float scale = 1f / tamañoMatriz;
        float offsetX = 0f;
        float offsetY = scale * (tamañoMatriz - 1);

        for (int i = 0; i < tamañoMatriz; i++)
        {
            for (int j = 0; j < tamañoMatriz; j++)
            {
                    // Asignar renderer
                    Renderer renderer = GameObject.Find(contador.ToString()).GetComponent<Renderer>();
                    // Cambiar "Tiling" de textura
                    renderer.material.mainTextureScale = new Vector2(scale, scale);
                    // Ajustar "Offeset" de textura
                    renderer.material.mainTextureOffset = new Vector2(offsetX, offsetY);

                    renderer.material.mainTexture = modelo.material.mainTexture;

                    contador++;

                offsetX += scale;
            }
            offsetX = 0;
            offsetY -= scale;
        }
    }

    /* -------------------------------------------------------------------------------- */

    void mezclarLugares()
    {
        int contador = 1;

        for (int i = 0; i < tamañoMatriz; i++) {
            for (int j = 0; j < tamañoMatriz; j++) {

                Transform transform = GameObject.Find(contador.ToString()).GetComponent<Transform>();

                transform.position = new Vector3(Random.Range(-11, 11), transform.position.y, Random.Range(-11, 11));

                contador++;
            }
        }
    }
}