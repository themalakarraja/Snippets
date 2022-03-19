using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CameraElevation : MonoBehaviour
{
    [SerializeField]
    private float MOVEMENT_SPEED = 2.0f;
    [SerializeField]
    private float ROTATION_SPEED = 2.0f;
    [SerializeField]
    private float ELEVATION_OFFSET = 0.02f;
    [SerializeField]
    private float ELEVATION_POSITION_MAX = 50f;
    private float ELEVATION_POSITION_MIN = 1f;

    private CharacterController CameraCharacterController;
    private float moveVertical;
    private float moveHorizontal;
    private float rotationX = 0.0f;
    private float rotationY = 0.0f;
    private Vector3 cameraPosition;

    private void Start()
    {
        cameraPosition = transform.position;
        CameraCharacterController = GetComponent<CharacterController>();
        CameraCharacterController.skinWidth = 0.0001f;
    }

    private void Update()
    {
        //Main Camera Rotation on Mouse Drag
        if (Input.GetMouseButton(0))
        {
            rotationX -= ROTATION_SPEED * Input.GetAxis("Mouse X");
            rotationY += ROTATION_SPEED * Input.GetAxis("Mouse Y");
            rotationY = Mathf.Clamp(rotationY, -90, 90);

            transform.eulerAngles = new Vector3(rotationY, rotationX, 0.0f);
        }

        //Camera Movement Vertically and Horizontally
        moveVertical = Input.GetAxis("Vertical") * MOVEMENT_SPEED;
        moveHorizontal = Input.GetAxis("Horizontal") * MOVEMENT_SPEED;
        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);

        movement = transform.rotation * movement;
        movement = new Vector3(movement.x, 0, movement.z);
        movement.y = 0.0f;
        CameraCharacterController.Move(movement * Time.deltaTime);

        //Camera Elevation
        if (Input.GetKey(KeyCode.E))
        {
            cameraPosition.y = cameraPosition.y + ELEVATION_OFFSET;
            cameraPosition = new Vector3(transform.position.x, cameraPosition.y, transform.position.z);
            cameraPosition.y = Mathf.Clamp(cameraPosition.y, ELEVATION_POSITION_MIN, ELEVATION_POSITION_MAX);
            transform.position = cameraPosition;
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            cameraPosition.y = cameraPosition.y - ELEVATION_OFFSET;
            cameraPosition = new Vector3(transform.position.x, cameraPosition.y, transform.position.z);
            cameraPosition.y = Mathf.Clamp(cameraPosition.y, ELEVATION_POSITION_MIN, ELEVATION_POSITION_MAX);
            transform.position = cameraPosition;
        }
    }
}
