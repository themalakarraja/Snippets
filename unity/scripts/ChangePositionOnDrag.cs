using UnityEngine;
using UnityEngine.EventSystems;

public class ChangePositionOnDrag : MonoBehaviour, IDragHandler, IEndDragHandler
{
    private float DraggingSpeed = 10;
    private float WithoutDraggingSpeed = 25;


    private RectTransform PlaneRectTransform;
    private Vector2 PlaneStartingPosition;
    private float PlaneHeight;
    private bool isDragging = false;

    public void OnDrag(PointerEventData eventData)
    {
        isDragging = true;
        transform.position = new Vector2(PlaneStartingPosition.x,
            Mathf.Clamp(transform.position.y + DraggingSpeed * Input.GetAxis("Mouse Y"), PlaneHeight / 2, Screen.height - PlaneHeight / 2));
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
    }

    private void Start()
    {
        PlaneRectTransform = GetComponent<RectTransform>();
        PlaneStartingPosition = transform.position;
        PlaneHeight = PlaneRectTransform.rect.height;
    }



    private void Update()
    {
        if(!isDragging)
        {
            if (transform.position.y > Screen.height / 2)
            {
                transform.position = new Vector2(PlaneStartingPosition.x,
                Mathf.Clamp(transform.position.y + WithoutDraggingSpeed, PlaneHeight / 2, Screen.height - PlaneHeight / 2));
            }
            else
            {
                transform.position = new Vector2(PlaneStartingPosition.x,
                Mathf.Clamp(transform.position.y - WithoutDraggingSpeed, PlaneHeight / 2, Screen.height - PlaneHeight / 2));
            }
        }
    }

}
