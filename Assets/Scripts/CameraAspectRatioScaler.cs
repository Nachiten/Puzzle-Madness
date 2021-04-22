using UnityEngine;

/// <summary>
/// Responsive Camera Scaler
/// </summary>
public class CameraAspectRatioScaler : MonoBehaviour
{

    public bool changeZoom = true;
    public bool changePosition = true;

    public bool fijePosicionCorrecta = false;
    bool inicializado = false;

    /// <summary>
    /// Reference Resolution like 1920x1080
    /// </summary>
    public Vector2 ReferenceResolution = new Vector3(1920, 1080);

    /// <summary>
    /// Zoom factor to fit different aspect ratios
    /// </summary>
    public Vector3 ZoomFactor = Vector3.one;

    /// <summary>
    /// Design time position
    /// </summary>
    [HideInInspector]
    public Vector3 OriginPosition = Vector3.zero;

    /// <summary>
    /// Start
    /// </summary>
    void Start()
    {
        OriginPosition = transform.position;
        Debug.Log("Posicion origen: " + OriginPosition);
    }

    /// <summary>
    /// Update per Frame
    /// </summary>
    void Update()
    {

        if (!inicializado) 
        {
            if (fijePosicionCorrecta)
            {
                OriginPosition = transform.position;
                inicializado = true;
            }
            else
            {
                return;
            }
        }

        if (ReferenceResolution.y == 0 || ReferenceResolution.x == 0)
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

        Debug.Log("Ratio: " + ratio.ToString("F2"));
    }
}