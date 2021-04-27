using UnityEngine;
using UnityEngine.UI;

public class KeySpot : MonoBehaviour
{
    GameManager gameManager;
    GameObject messageDisplay;
    PlayerController playerControl;

    public int keyNum;
    private bool itemPickupable;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        messageDisplay = gameManager.messageDisplay;
        playerControl = FindObjectOfType<PlayerController>();
        SetKeyNum(keyNum);
    }

    void Update()
    {
        if(Input.GetKeyDown("e") && itemPickupable)
        {
            if(keyNum == 0)
            {
                foreach (DoorScript door in FindObjectsOfType<DoorScript>())
                {
                    if (door.tag == "Main Door")
                    {
                        AudioSource audio = door.GetComponent<AudioSource>();
                        audio.clip = door.doorBanging;
                        audio.loop = true;
                        audio.Play();
                    }
                }
            }
            
            switch(playerControl.GetHeldItem())
            {
                case "key0":
                    playerControl.SetHeldItem("key" + keyNum);
                    SetKeyNum(0);
                    break;

                case "key1":
                    playerControl.SetHeldItem("key" + keyNum);
                    SetKeyNum(1);
                    break;

                case "key2":
                    playerControl.SetHeldItem("key" + keyNum);
                    SetKeyNum(2);
                    break;

                case "key3":
                    playerControl.SetHeldItem("key" + keyNum);
                    SetKeyNum(3);
                    break;
            }

            if(playerControl.GetHeldItem() == "")
            {
                playerControl.SetHeldItem("key" + keyNum);
                messageDisplay.SetActive(false);
                Destroy(gameObject);
            }
        }
    }

    public void SetKeyNum(int num)
    {
        keyNum = num;
        foreach (MeshRenderer r in GetComponentsInChildren<MeshRenderer>())
        {
            if(r.name.Length == 1) r.enabled = false;
            foreach (Light l in r.GetComponentsInChildren<Light>()) l.enabled = false;
            if (r.name == num.ToString())
            {
                r.enabled = true;
                foreach (Light l in r.GetComponentsInChildren<Light>()) l.enabled = true;
            }
        }
    }

    public int GetKeyNum()
    {
        return keyNum;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            itemPickupable = true;
            if (playerControl.GetHeldItem() == "") messageDisplay.GetComponentInChildren<Text>().text = "E - pick up key";
            else messageDisplay.GetComponentInChildren<Text>().text = "E - switch keys";
            messageDisplay.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            itemPickupable = false;
            messageDisplay.SetActive(false);
        }
    }
}
