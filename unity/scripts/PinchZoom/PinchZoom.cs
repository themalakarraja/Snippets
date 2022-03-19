using UnityEngine;
using UnityEngine.UI;

public class PinchZoom : MonoBehaviour
{
    public GameObject PinchZoomParentPanel;
    public Slider ZoomSlider;

    public float perspectiveZoomSpeed = 0.3f; // The rate of change of the field of view in perspective mode.
    public float maxFieldOfViewValue = 60f;
    public float minFieldOfViewValue = 1f;
    public float interpolation = 5.0f;

    private Camera Camera;

    private void Start()
    {
        //Time.timeScale = 0;
        Camera = Camera.main;

        if (ZoomSlider)
        {
            ZoomSlider.onValueChanged.RemoveAllListeners();
            ZoomSlider.onValueChanged.AddListener((value) =>
            {
                OnSliderValueChange(value);
            });

            ZoomSlider.minValue = minFieldOfViewValue - 1;
            ZoomSlider.maxValue = maxFieldOfViewValue - 1;

            PinchZoomParentPanel.SetActive(false);
        }
        else
        {
            Debug.LogError("Slider not initialized");
        }
    }

    private void Update()
    {
        // If there are two touches on the device...
        if (Input.touchCount == 2)
        {
            // Store both touches.
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // Find the position in the previous frame of each touch.
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Find the magnitude of the vector (the distance) between the touches in each frame.
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Find the difference in the distances between each frame.
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            if (!Camera.orthographic)
            {
                float targetFieldOfView = Camera.fieldOfView + (deltaMagnitudeDiff * perspectiveZoomSpeed);

                Camera.fieldOfView = Mathf.Lerp(Camera.fieldOfView, targetFieldOfView,
                    interpolation * Time.unscaledDeltaTime);
                if (!PinchZoomParentPanel.activeSelf)
                {
                    PinchZoomParentPanel.SetActive(true);
                }

                if (ZoomSlider)
                {
                    ZoomSlider.value = maxFieldOfViewValue - Camera.fieldOfView;
                }
                // Clamp the field of view to make sure it's between minFieldOfViewValue and maxFieldOfViewValue.
                Camera.fieldOfView = Mathf.Clamp(Camera.fieldOfView, minFieldOfViewValue, maxFieldOfViewValue);
            }
        }
    }

    public void OnSliderValueChange(float value)
    {
        float zoomSliderValue = maxFieldOfViewValue - value;

        if ((int)Camera.fieldOfView != (int)zoomSliderValue)
        {
            Camera.fieldOfView = Mathf.Lerp(zoomSliderValue, Camera.fieldOfView,
                    interpolation * Time.unscaledDeltaTime);
        }
    }
}
