using UnityEngine;

public class PopUpMove : MonoBehaviour
{
    /*
    // Offset of position
    Vector3 Offset;

    // Z Position
    float CoordZ;

    // Limit of distance
    float limite = 1.4f;

    // Position when you pick it up
    Vector3 inicialPos;

    // Flag to stop you from being able to move after placing in right position
    //public bool move = false;

    void Start() { }

    void OnMouseDown()
    {
        
        Debug.Log("Item Picked Up");
        
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

    public void agarrar()
    {
        Debug.Log("SE agarro");
        // Move object
        gameObject.transform.position = Input.mousePosition;
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
            }
            else // Return to start position
            {
                Debug.Log("Not correctly placed");

                // Return to start position
                //gameObject.transform.position = inicialPos;
            }
        }
    }
    
    
    */
}