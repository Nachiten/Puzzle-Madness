using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    public int tamañoMatriz = 3;

    // Offset of position
    Vector3 offset;

    // Z Position
    float coordZ;

    // Limit of distance
    float limite = 1.2f;
    float elevamiento = 0.5f;

    // Position when you pick it up
    Vector3 inicialPos;

    // Rayo
    RaycastHit hit;

    // Flag para no cambiar de bloque al tener uno seleccionado
    bool flag = true;

    // Objeto y posicion correcta objeto
    GameObject objeto;
    Transform lugar;
    
    void Start() { ajustarPosiciones(); mezclarLugares(); }

    void Update()
    {
        // Generar rayo para "clickear" bloque
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        bool operacion = Physics.Raycast(ray, out hit, 100.0f) && hit.transform != null;

        // Si el rayo hace contacto con un bloque y se tiene el click IZQUIERDO persionado
        if (operacion && Input.GetMouseButton(0))
        {
            if (flag)
            { 
                // Asignar bloque seleccionado
                objeto = hit.transform.gameObject;

                // Lugar correcto del bloque
                lugar = GameObject.Find("Lugar_" + objeto.name).GetComponent<Transform>();
                flag = false;
            }

            if (objeto.transform.position == new Vector3(lugar.position.x, lugar.position.y + 0.2f, lugar.position.z)) return;
             
            // When item Picked Up
            if (Input.GetMouseButtonDown(0))
            { 
                    Debug.Log("Item Picked Up");
                    // Set Z coordinateF
                    coordZ = Camera.main.WorldToScreenPoint(objeto.transform.position).z;

                    offset = objeto.transform.position - GetMouseAsWorldPoint();

                    // Set inicial position
                    inicialPos = GetMouseAsWorldPoint() + offset;
            }
            // While item still picked up
            else if (Input.GetMouseButton(0))
            {
                // Move object
                objeto.transform.position = GetMouseAsWorldPoint() + offset + new Vector3(0,elevamiento,0);
            }

        }
        else if (operacion && Input.GetMouseButtonUp(0))
        {
            GameObject Objeto = hit.transform.gameObject;

            // When item dropped
            Debug.Log("Item dropped");

            // Distance from object to correct place
            float distancia = Vector3.Distance(Objeto.transform.position, lugar.position);

            Debug.Log("Distance: " + distancia + " | " + "Limit of Distance: " + limite);

            // If its inside the limit
            if (distancia < limite)
            {
                Debug.Log("Correctly Placed");

                // Place in correct place
                Objeto.transform.position = new Vector3(lugar.position.x, lugar.position.y + 0.2f, lugar.position.z);
            }
            else // Return to start position
            {
                Objeto.transform.position = new Vector3(Objeto.transform.position.x, Objeto.transform.position.y - elevamiento, Objeto.transform.position.z);
                Debug.Log("Not correctly placed");
            }
            
            flag = true;
        }
    }

    /* -------------------------------------------------------------------------------- */

    private Vector3 GetMouseAsWorldPoint()
    {
        // Coordenadas de pixel de mouse (X,Y)
        Vector3 mousePoint = Input.mousePosition;

        // Coordenadas Z del objeto
        mousePoint.z = coordZ;

        // Convertir posicion de mouse en coordenadas 3D
        return Camera.main.ScreenToWorldPoint(mousePoint);
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