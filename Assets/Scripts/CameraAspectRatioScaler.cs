using UnityEngine;

/// <summary>
/// Responsive Camera Scaler
/// </summary>
public class CameraAspectRatioScaler : MonoBehaviour
{
    public bool changeZoom = true, changePosition = true;

    bool inicializado = false;

    public Vector2 ReferenceResolution = new Vector3(1920, 1080);

    public Vector3 ZoomFactor = Vector3.one;

    [HideInInspector]
    public Vector3 OriginPosition = Vector3.zero;

    void Start()
    {
        OriginPosition = transform.position;
        Debug.Log("[CameraAspectRatioScaler] Posicion origen: " + OriginPosition);
    }

    public void inicializacionFinalizada() 
    {
        OriginPosition = transform.position;
        inicializado = true;
    }

    void Update()
    {
        if (ReferenceResolution.y == 0 || ReferenceResolution.x == 0 || !inicializado)
            return;

        var refRatio = ReferenceResolution.x / ReferenceResolution.y;
        var ratio = (float)Screen.width / (float)Screen.height;

        if (changeZoom) 
        {
            if (ratio > 1.85f)
            {
                ZoomFactor.x = ratio * 34.13f / 2.1f;
                ZoomFactor.z = ratio * -22.4f / 2.1f;
            }
            else if (ratio < 1.65f)
            {
                ZoomFactor.x = ratio * 5.6f / 1.2f;
                ZoomFactor.z = ratio * 4.5f / 1.2f;
            }
            else
            {
                ZoomFactor.x = 1f;
                ZoomFactor.y = 1f;
                ZoomFactor.z = 1f;
            }
        }

        if (changePosition)
            transform.position = OriginPosition + transform.forward * (1f - refRatio / ratio) * ZoomFactor.z
                                                + transform.right * (1f - refRatio / ratio) * ZoomFactor.x
                                                + transform.up * (1f - refRatio / ratio) * ZoomFactor.y;
        else
            transform.position = OriginPosition;

        //Debug.Log("Ratio: " + ratio.ToString("F2"));
    }
}