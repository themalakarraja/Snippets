using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CameraFreeLook : MonoBehaviour
{
    [SerializeField]
    private float MOVEMENT_SPEED = 2.0f;
    [SerializeField]
    private float ROTATION_SPEED = 2.0f;

    private CharacterController CameraCharacterController;

    private float moveVertical;
    private float moveHorizontal;
    private float rotationX;
    private float rotationY;

    private void Start()
    {
        CameraCharacterController = GetComponent<CharacterController>();
        CameraCharacterController.skinWidth = 0.0001f;
    }

    private void Update()
    {
        //Camera Movement On Key Press
        moveVertical = Input.GetAxis("Vertical") * MOVEMENT_SPEED;
        moveHorizontal = Input.GetAxis("Horizontal") * MOVEMENT_SPEED;
        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);

        movement = transform.rotation * movement;
        CameraCharacterController.Move(movement * Time.deltaTime);

        //Camera Rotation On Mouse Move
        rotationX += ROTATION_SPEED * Input.GetAxis("Mouse X");
        rotationY -= ROTATION_SPEED * Input.GetAxis("Mouse Y");
        rotationY = Mathf.Clamp(rotationY, -90, 90);
        transform.eulerAngles = new Vector3(rotationY, rotationX, 0.0f);
    }
}
