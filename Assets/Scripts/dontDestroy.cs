using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dontDestroy : MonoBehaviour
{
    static bool flag = true;

    /* -------------------------------------------------------------------------------- */

    void Start()
    {
        if (flag)
        {
            //Debug.Log("Objeto: " + gameObject.name + " NO es destruido.");
            DontDestroyOnLoad(gameObject);
            flag = false;
        }
        else
        {
            //Debug.Log("Objeto: " + gameObject.name + " SI es destruido.");
            Destroy(gameObject);
        }
            
    }
}
