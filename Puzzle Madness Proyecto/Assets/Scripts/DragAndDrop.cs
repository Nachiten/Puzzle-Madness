using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    

    public int tamañoMatriz = 3;

    // Offset of position
    Vector3 Offset;

    // Z Position
    float CoordZ;

    // Limit of distance
    float limite = 1.4f;

    // Position when you pick it up
    Vector3 inicialPos;

    // Flag to stop you from being able to move after placing in right position
    bool move = true;

    void Start() {
        ajustarPosiciones(); mezclarLugares();
    }

    void OnMouseDown()
    {
        if (move)
        {
            Debug.Log("Item Picked Up");
            // Seet Z coordinate
            CoordZ = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;

            // Store offset = gameobject world pos - mouse world pos
            Offset = gameObject.transform.position - GetMouseAsWorldPoint();

            // Se inicial position
            inicialPos = GetMouseAsWorldPoint() + Offset;
        }
        else Debug.Log("Item Already in Correct Position");
        
    }

    private Vector3 GetMouseAsWorldPoint()
    {
        // Pixel coordinates of mouse (x,y)
        Vector3 mousePoint = Input.mousePosition;

        // z coordinate of game object on screen
        mousePoint.z = CoordZ;

        // Convert it to world points
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    void OnMouseDrag()
    {
        // Move object
        if (move) transform.position = GetMouseAsWorldPoint() + Offset;
    }

    void OnMouseUp()
    {
        if (move)
        { 

            Debug.Log("Item dropped");

            // Correct place for the object
            Transform lugar = GameObject.Find("Lugar_" + gameObject.name).GetComponent<Transform>();

            // Distance from object to correct place
            float distancia = Vector3.Distance(gameObject.transform.position, lugar.position);

            Debug.Log("Distance: " + distancia + " | " + "Limit of Distance: " + limite);

            // If its inside the limit
            if (distancia < limite)
            {
                Debug.Log("Correctly Placed");

                // Place in correct place
                gameObject.transform.position = new Vector3(lugar.position.x, lugar.position.y + 0.2f, lugar.position.z);
                move = false;
            }
            else // Return to start position
            {
                Debug.Log("Not correctly placed");

                // Return to start position
                //gameObject.transform.position = inicialPos;
            }
        }
    }

    Renderer modelo;
    Renderer objeto;
    

    public void ajustarPosiciones()
    {
        modelo = GameObject.Find("Bloque Modelo").GetComponent<Renderer>();

        int contador = 1;

        float scale = 1f / tamañoMatriz;
        float offsetX = 0f;
        float offsetY = scale * (tamañoMatriz - 1);

        for (int i = 0; i < tamañoMatriz; i++)
        {
            for (int j = 0; j < tamañoMatriz; j++)
            {
                    // Asignar renderer
                    objeto = GameObject.Find(contador.ToString()).GetComponent<Renderer>();
                    // Cambiar "Tiling" de textura
                    objeto.material.mainTextureScale = new Vector2(scale, scale);
                    // Ajustar "Offeset" de textura
                    objeto.material.mainTextureOffset = new Vector2(offsetX, offsetY);

                    objeto.material.mainTexture = modelo.material.mainTexture;

                    contador++;

                offsetX += scale;
            }
            offsetX = 0;
            offsetY -= scale;
        }
    }

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