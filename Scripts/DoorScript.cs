using UnityEngine;
using UnityEngine.UI;

public class DoorScript : MonoBehaviour
{
    public AudioClip wrongKey;
    public AudioClip doorUnlock;
    public AudioClip doorBanging;
    
    public Animator doorAnim;
    private GameManager gameManager;
    private GameObject messageDisplay;
    private PlayerController playerControl;
    private AudioSource audioSource;

    public int doorNum;
    private bool unlockable;
    private bool keyUsed;
    private bool waiting;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        messageDisplay = gameManager.messageDisplay;
        playerControl = FindObjectOfType<PlayerController>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if(Input.GetKeyDown("e") && unlockable && !keyUsed && !waiting)
        {
            if(playerControl.GetHeldItem().Substring(3, 1) == doorNum.ToString())
            {
                keyUsed = true;
                messageDisplay.SetActive(false);
                playerControl.SetHeldItem("");
                audioSource.clip = doorUnlock;
                audioSource.loop = false;
                audioSource.Play();
                doorAnim.SetTrigger("OpenDoor");

                if(doorNum == 0)
                {
                    gameManager.fadetoblack_panel.SetTrigger("Fade");
                    gameManager.FinishLevel();
                }

                Invoke("DeleteSelf", 3.3f);
            }
            else
            {
                waiting = true;
                audioSource.clip = wrongKey;
                audioSource.loop = false;
                audioSource.Play();
                SetDisplayText("Wrong key");
                Invoke("ResetDisplayText", 1f);
            }
        }
    }

    private void ResetDisplayText()
    {
        waiting = false;
        SetDisplayText("E - use key");
    }

    private void SetDisplayText(string text)
    {
        messageDisplay.GetComponentInChildren<Text>().text = text;
    }

    private void DeleteSelf()
    {
        Destroy(doorAnim.gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if(doorNum == 0 && playerControl.GetHeldItem() == "key0")
        {
            //Stop shaking door
            playerControl.movementEnabled = false;
            playerControl.LockView(transform.position);
            audioSource.Stop();
        }

        if (other.tag == "Player" && playerControl.GetHeldItem().Contains("key") && !keyUsed)
        {
            unlockable = true;
            ResetDisplayText();
            messageDisplay.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            unlockable = false;
            messageDisplay.SetActive(false);
        }
    }
}
