using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Transform playerCam;
    public GameObject playerHand;
    public CharacterController characterControl;
    public GameObject flashlight;
    public Transform testFocus;
    private GameManager gameManager;
    private RectTransform powerVisual;

    public Color batteryOutOfPowerColor;
    public Color batteryNormalColor;

    public float sensitivity = 2f;
    public float yRotationLimit = 90f;
    public float movementSpeed = 10f;
    public float powerDrainSpeed = -1f;
    public float powerRegenSpeed = 0.75f;
    public float minimumPowerRegenPercentage = 10f;
    public Vector2 powerVisual_positionOffset;
    public float powerVisual_MaxWidth;
    public float lockedCamMovementFreedom = 30f;

    public bool movementEnabled = true;

    string heldItem = "";
    bool outOfPower = false;
    float currentRot;
    float powerPercentage = 100;
    bool viewLocked = false;
    bool normalViewLock;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        powerVisual = gameManager.powerVisual;
        sensitivity = gameManager.GetMouseSensitivity();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        PlayerRotation();
        if(movementEnabled) PlayerMovement();
        PlayerFlashlight();
        if (viewLocked) RestrainView();
    }

    public void RestrainView()
    {
        float camDirection = transform.rotation.eulerAngles.y;
        camDirection = CorrectRotation(((camDirection / 2) - 180) * 2);

        if (normalViewLock)
        {
            if (camDirection < maxLeft) transform.rotation = Quaternion.Euler(Vector3.up * maxLeft);
            if (camDirection > maxRight) transform.rotation = Quaternion.Euler(Vector3.up * maxRight);
        }
        else
        {
            if (camDirection > maxRight && camDirection < 0f) transform.rotation = Quaternion.Euler(Vector3.up * maxRight);
            if (camDirection < maxLeft && camDirection > 0f) transform.rotation = Quaternion.Euler(Vector3.up * maxLeft);
        }
    }

    float maxLeft;
    float maxRight;

    public void LockView(Vector3 focusLocation)
    {
        testFocus.LookAt(focusLocation);
        float direction = testFocus.eulerAngles.y;

        maxLeft = CorrectRotation(direction - (lockedCamMovementFreedom / 2));
        maxRight = CorrectRotation(direction + (lockedCamMovementFreedom / 2));

        if (maxRight <= 90f && maxLeft > 0f) normalViewLock = false;
        else normalViewLock = true;
        viewLocked = true;
    }

    public void UnlockView()
    {

    }

    private float CorrectRotation(float angle)
    {
        float result = angle;

        if (result >= 180f) result = -180 + (result - 180);
        else if (result < -180) result = 180 + (result + 180);

        return result;
    }

    public void SetHeldItem(string item)
    {
        heldItem = item;
        foreach(MeshRenderer r in playerHand.GetComponentsInChildren<MeshRenderer>())
        {
            r.enabled = false;
            if (r.name == item)
            {
                r.enabled = true;
            }
        }
    }

    public string GetHeldItem()
    {
        return heldItem;
    }

    void PlayerFlashlight()
    {
        if(Input.GetMouseButton(1) && !outOfPower)
        {
            if (!flashlight.activeSelf) flashlight.SetActive(true);
            AddPower(powerDrainSpeed * Time.deltaTime);
        }
        else
        {
            if (flashlight.activeSelf) flashlight.SetActive(false);
            AddPower(powerRegenSpeed * Time.deltaTime);
        }
    }

    public void AddPower(float percent)
    {
        if (powerPercentage + percent > 100) powerPercentage = 100f;
        else if (powerPercentage + percent < 0) powerPercentage = 0f;
        else powerPercentage += percent;

        if (powerPercentage >= minimumPowerRegenPercentage)
        {
            outOfPower = false;
            powerVisual.GetComponent<RawImage>().color = batteryNormalColor;
        }
        else if (powerPercentage == 0f)
        {
            outOfPower = true;
            powerVisual.GetComponent<RawImage>().color = batteryOutOfPowerColor;
        }

        powerVisual.sizeDelta = new Vector2(powerPercentage, powerVisual.sizeDelta.y);
        powerVisual.localPosition = new Vector2(-50 + (powerPercentage / 2), 0f);
    }

    void PlayerMovement()
    {
        Vector3 movement = Vector3.zero;
        bool w = Input.GetKey("w");
        bool a = Input.GetKey("a");
        bool s = Input.GetKey("s");
        bool d = Input.GetKey("d");

        if (w && !s) movement.z = 1f;
        if (s && !w) movement.z = -1f;
        if (a && !d) movement.x = -1f;
        if (d && !a) movement.x = 1f;

        if (movement.x != 0f && movement.z != 0f) movement = movement * 0.5f;
        movement = movement * movementSpeed * Time.deltaTime;

        characterControl.Move((transform.forward * movement.z) + (transform.right * movement.x));
    }

    void PlayerRotation()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = -Input.GetAxis("Mouse Y");

        if(currentRot + mouseY > yRotationLimit)
        {
            playerCam.Rotate(yRotationLimit - currentRot, 0f, 0f);
            currentRot = yRotationLimit;
        }
        else if(currentRot + mouseY < -yRotationLimit)
        {
            playerCam.Rotate(-yRotationLimit - currentRot, 0f, 0f);
            currentRot = -yRotationLimit;
        }
        else
        {
            playerCam.Rotate(mouseY, 0f, 0f);
            currentRot += mouseY;
        }

        transform.Rotate(0f, mouseX, 0f);
    }
}
