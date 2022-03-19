using UnityEngine;
using UnityEngine.UI;

public class BackgroundCamera : MonoBehaviour
{
    [SerializeField] private RawImage rawimage;

    private void Start()
    {
        WebCamTexture webcamTexture = new WebCamTexture();
        rawimage.texture = webcamTexture;
        rawimage.material.mainTexture = webcamTexture;
        webcamTexture.Play();
    }
}