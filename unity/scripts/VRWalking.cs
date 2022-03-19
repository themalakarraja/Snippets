using UnityEngine;
using UnityEngine.UI;

public class VRWalking : MonoBehaviour
{
    [SerializeField]
    private float WALK_SPEED = 5.0f;

    public GameObject loderGameObject;
    public Image loderImage;
    public Text messageText;

    private float waitTime = 2.0f;
    private bool isWalkModeOn = false;
    private bool isWalkStopGameObj = true;

    private float currentWaitTime;

    private void Start ()
    {
        if (!loderGameObject)
        {
            Debug.LogError("Loder GameObject not initialized");
        }
        if (!loderImage)
        {
            Debug.LogError("Loder Image not initialized");
        }
        if (!messageText)
        {
            Debug.LogError("Message text not initialized");
        }
    }
	
	private void Update ()
    {
        if (!loderGameObject || !loderImage || !messageText)
            return;

        if (transform.eulerAngles.x > 75 && transform.eulerAngles.x <= 90)
        {
            startLoader();
        }
        else if (transform.eulerAngles.x < 75)
        {
            isWalkStopGameObj = true;
            resetLoader();
        }
        
        if (isWalkModeOn)
        {
            messageText.text = "Walking";

            Vector3 forwardVector = new Vector3(
                transform.position.x + (transform.forward.x * WALK_SPEED * Time.deltaTime),
                transform.position.y,
                transform.position.z + (transform.forward.z * WALK_SPEED * Time.deltaTime));

            transform.position = forwardVector;

            //transform.position += transform.forward * WALK_SPEED * Time.deltaTime; //free space walking
        }
        else
        {
            messageText.text = "";
        }
    }

    private void startLoader()
    {
        if(isWalkStopGameObj)
        {
            loderGameObject.SetActive(true);
            loderImage.fillAmount = (1 / waitTime) * currentWaitTime;

            currentWaitTime += Time.deltaTime * 1;

            if ((int)currentWaitTime == (int)waitTime)
            {
                isWalkModeOn = !isWalkModeOn;
                isWalkStopGameObj = false;
                resetLoader();
            }
        }
    }

    private void resetLoader()
    {
        currentWaitTime = 0f;
        loderImage.fillAmount = 0;
        loderGameObject.SetActive(false);
    }
}
