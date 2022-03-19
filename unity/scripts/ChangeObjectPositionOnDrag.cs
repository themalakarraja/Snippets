using UnityEngine;

public class ChangeObjectPositionOnDrag : MonoBehaviour
{
    public float DragMovementSeed = 15f;
    public float TouchPressWaitTime = 1.5f;
    public Vector3 ScleIncrese = new Vector3(0.2f, 0.2f, 0.2f);

    private Vector3 ObjectScreenPos;
    private Vector2 VectorMousePosBtwObjPos;

    private float Radius;
    private float TouchTime = 0f;
    private bool isScled = false;

    private void OnMouseDown()
    {
        Radius = Vector3.Distance(Camera.main.transform.position, transform.position);

        ObjectScreenPos = Camera.main.WorldToScreenPoint(transform.position);
        VectorMousePosBtwObjPos = new Vector2(Input.mousePosition.x - ObjectScreenPos.x,
            Input.mousePosition.y - ObjectScreenPos.y);
    }

    private void OnMouseDrag()
    {
        if(TouchTime > TouchPressWaitTime)
        {
            if (!isScled)
            {
                transform.localScale = transform.localScale + ScleIncrese;
                isScled = true;
            }

            Vector3 curPos = new Vector3(Input.mousePosition.x - VectorMousePosBtwObjPos.x,
            Input.mousePosition.y - VectorMousePosBtwObjPos.y, ObjectScreenPos.z);
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(curPos);

            Vector3 centerToPosition = worldPos - Camera.main.transform.position;
            centerToPosition.Normalize();
            worldPos = Camera.main.transform.position + centerToPosition * Radius;

            transform.position = Vector3.Slerp(transform.position, worldPos, Time.deltaTime * DragMovementSeed);
            //transform.position = worldPos;
            transform.LookAt(Camera.main.transform);
        }
    }

    private void OnMouseUp()
    {
        if(TouchTime>TouchPressWaitTime)
        {
            transform.localScale = transform.localScale - ScleIncrese;
        }
        TouchTime = 0f;
        isScled = false;
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            TouchTime += Time.deltaTime * 1;
        }
    }
}