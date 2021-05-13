using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dontDestroy : MonoBehaviour
{
    static bool flag = true;

    /* -------------------------------------------------------------------------------- */

    void Awake()
    {
        if (flag)
        {
            DontDestroyOnLoad(gameObject);
            flag = false;
        }
        else
        {
            Destroy(gameObject);
        }
            
    }
}
