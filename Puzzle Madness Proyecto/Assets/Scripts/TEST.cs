using System.Collections;
using UnityEngine;

public class TEST : MonoBehaviour
{
    Animator animacion;

    int contador = 1;
    float startTime;
    float tiempo = 1f;
    public bool flag = false;

    float t;

    bool start = false;
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        //tiempo = 0;
        if (flag)
        {
        startTime = Time.time;
        
        flag = false;
        start = true;
        }

        //Debug.Log(Time.time % 60);
        if (start && (t % 60) / 4 < tiempo / 4) {

            t = Time.time - startTime;
            Debug.Log(contador);
            GameObject.Find("5").GetComponent<Transform>().Translate( 5f / 243f, 0, 0);

            contador++;
            //Debug.Log(t);
        }

        /*

        if (Input.GetKeyDown("d")) {
            GameObject.Find("5").GetComponent<Animator>().SetTrigger("ActivarIzquierda");
        } else if (Input.GetKeyDown("a")) {
            animacion.SetTrigger("ActivarDerecha");
        }
        */
    }
}
