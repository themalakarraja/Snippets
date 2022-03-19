using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    public GameObject Player;

    private bool isPlayerMovementOn, isFlyModeOn, ShowToast;
    [SerializeField]
    private float movementSpeed = 3f;
    private Vector3 NewPlayerPostion;

    private void Awake()
    {
        isPlayerMovementOn = false;
        isFlyModeOn = false;
        ShowToast = true;
    }

    private void Update()
    {
        if (GetComponent<SoopersimsManager>().isSoopersimOn)
        {
            if (isPlayerMovementOn)
            {
                NewPlayerPostion = Player.transform.position;
                if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
                {
                    NewPlayerPostion += Player.transform.forward;
                }
                if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
                {
                    NewPlayerPostion -= Player.transform.forward;
                }
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                {
                    NewPlayerPostion -= Player.transform.right;
                }
                if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                {
                    NewPlayerPostion += Player.transform.right;
                }
                if (!isFlyModeOn)
                {
                    NewPlayerPostion = new Vector3(NewPlayerPostion.x, SPConstants.AVERAGE_HUMAN_HEIGHT,
                        NewPlayerPostion.z);
                }
                Player.transform.position = Vector3.Lerp(Player.transform.position, NewPlayerPostion,
                         Time.unscaledDeltaTime * movementSpeed);
            }
            else
            {
                if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) ||
                    Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) ||
                    Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) ||
                    Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                {
                    if (ShowToast)
                    {
                        GetComponent<SoopersimPlayGeneralScript>().ShowToast("Walk mode is disabled for this soopersim");
                    }
                }
            }
        }
    }

    public void ResetPlayerPosition()
    {
        Player.transform.position = new Vector3(0, SPConstants.AVERAGE_HUMAN_HEIGHT, 0);
    }

    public void AllowPlayerMovement()
    {
        isPlayerMovementOn = true;
    }

    public void DisablePlayerMovement(bool ShowToast)
    {
        isPlayerMovementOn = false;
        this.ShowToast = ShowToast;
    }

    public void StartFlyMode()
    {
        isFlyModeOn = true;
    }

    public void StopFlyMode()
    {
        isFlyModeOn = false;
        transform.position = new Vector3(0f, SPConstants.AVERAGE_HUMAN_HEIGHT, 0f);
        transform.eulerAngles = new Vector3(0f, 0f, 0f);
    }
}
