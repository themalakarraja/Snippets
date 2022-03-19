using UnityEngine;
using UnityEngine.EventSystems;

public class RotateCameraOnDrag : MonoBehaviour
{
    public Camera MainCamera;
    public GameObject GeneralScriptsObj;

    [SerializeField]
    private float CameraTurnOffsetSpeed = 50f;
    [SerializeField]
    private float CameraRotationSpeedOnDrag = 50f;
    private Vector3 rotation;

    private void Awake()
    {
        gameObject.AddComponent<EventTrigger>();
    }

    private void OnEnable()
    {
        EventTrigger trigger = GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.Drag;
        entry.callback.AddListener(
            (data) =>
            {
                OnDrag();
            });
        trigger.triggers.Add(entry);
    }

    private void OnDrag()
    {
        if (GeneralScriptsObj.GetComponent<SoopersimsManager>().isSoopersimOn)
        {
            rotation = MainCamera.transform.localRotation.eulerAngles;

            rotation.y += -Input.GetAxis("Mouse X") * Time.unscaledDeltaTime * CameraTurnOffsetSpeed;
            rotation.x += Input.GetAxis("Mouse Y") * Time.unscaledDeltaTime * CameraTurnOffsetSpeed;
            rotation.z = 0.0f;

            MainCamera.transform.eulerAngles = Vector3.Lerp(MainCamera.transform.eulerAngles, rotation, Time.unscaledDeltaTime * CameraRotationSpeedOnDrag);
        }
    }
}