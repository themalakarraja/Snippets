using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class RotateObjectOnDrag : MonoBehaviour
{
    [SerializeField]
    private float ROOT_SPEED = 1f;
    [SerializeField]
    private GameObject gameObject;

    private EventTrigger trigger;
    private EventTrigger.Entry entry;

    private bool isDrag = false;

    private void Start()
    {
        trigger = GetComponent<EventTrigger>();
        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.Drag;
    }

    private void OnDragDelegate(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            ObjectRotateOnDrag();
        }
        else
        {
            if (Input.touchCount == 1)
            {
                ObjectRotateOnDrag();
            }
        }
    }

    private void ObjectRotateOnDrag()
    {
        float rotX = Input.GetAxis("Mouse X") * ROOT_SPEED * Mathf.Deg2Rad;
        float rotY = Input.GetAxis("Mouse Y") * ROOT_SPEED * Mathf.Deg2Rad;
        gameObject.transform.RotateAround(Vector3.up, -rotX);
        gameObject.transform.RotateAround(Vector3.right, rotY);
    }

    public void Btn_OnClick()
    {
        isDrag = !isDrag;

        if (isDrag)
        {
            entry.callback.AddListener((data) => { OnDragDelegate((PointerEventData)data); });
            trigger.triggers.Add(entry);
        }
        else
        {
            trigger.triggers.Remove(entry);
            entry.callback.RemoveListener((data) => { OnDragDelegate((PointerEventData)data); });
        }
    }
}
