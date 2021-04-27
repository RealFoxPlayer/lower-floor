using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ExitTrigger : MonoBehaviour
{
    GameManager gameManager;
    GameObject messageDisplay;

    bool menuOpen = false;
    bool leavingNighmare = false;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        messageDisplay = gameManager.messageDisplay;
    }

    void Update()
    {
        if(Input.GetKeyDown("e") && menuOpen && !leavingNighmare)
        {
            leavingNighmare = true;
            SceneManager.LoadScene("MainMenu");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            menuOpen = true;
            messageDisplay.GetComponentInChildren<Text>().text = "Press E to return to the main menu.\n\nStep back if you want to go deeper.";
            messageDisplay.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            menuOpen = false;
            messageDisplay.SetActive(false);
        }
    }
}
