using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;

[RequireComponent(typeof(TextMesh))]

public class AsignarTextos : MonoBehaviour
{

    /*

    
    TextMesh textComp;
    TextAsset textAsset;

    public static List<string> textArray;
    [SerializeField]
    public int[] lineasParaLeer;
    public string nombreArchivo;

   

    // Start is called before the first frame update
    void Start()
    {

        textAsset = Resources.Load("textFiles/" + nombreArchivo) as TextAsset;
        textComp = GetComponent<TextMesh>();
        leerArchivo();
    }

    // Update is called once per frame
    void leerArchivo()
    {
        textArray = 
            
            textAsset.text.Split('\0').ToList();

        for (int i = 0; i < lineasParaLeer.Length ; i++) {

            if (lineasParaLeer[0] < 0 || lineasParaLeer.Length == 0) textComp.text = textAsset.text;
            
            else textComp.text += textArray[lineasParaLeer[i]] + "\n";
        }
    }*/
}
