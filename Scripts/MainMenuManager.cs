using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameManager gameManager;
    public AudioSource ambience;
    public GameObject[] menus;
    public Text menuTitle;
    public Button continueButton;

    public Slider SFXSlider;
    public Slider ambienceSlider;
    public Slider mouseSensitivitySlider;

    private bool settingsApplied = false;
    
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (!PlayerPrefs.HasKey("floor"))
        {
            PlayerPrefs.SetFloat("sfxVolume", 0.75f);
            PlayerPrefs.SetFloat("ambienceVolume", 0.75f);
            PlayerPrefs.SetFloat("mouseSensitivity", 10f);

            PlayerPrefs.SetInt("floor", 0);
        }

        SFXSlider.value = PlayerPrefs.GetFloat("sfxVolume");
        ambienceSlider.value = PlayerPrefs.GetFloat("ambienceVolume");
        mouseSensitivitySlider.value = PlayerPrefs.GetFloat("mouseSensitivity");
        settingsApplied = true;

        if (PlayerPrefs.GetInt("floor") == 0) continueButton.interactable = false;
    }

    void Update()
    {
        
    }

    public void NewGame()
    {
        PlayerPrefs.SetInt("floor", 1);
        SceneManager.LoadScene("Floor1");
    }

    public void Continue()
    {
        if(PlayerPrefs.GetInt("floor") <= 2) SceneManager.LoadScene("Floor" + PlayerPrefs.GetInt("floor"));
        else SceneManager.LoadScene("Floor" + (PlayerPrefs.GetInt("floor") - 1));

        //if(CheckIfSceneExists("Floor" + PlayerPrefs.GetInt("floor")) != false) SceneManager.LoadScene("Floor" + PlayerPrefs.GetInt("floor"));
        //else SceneManager.LoadScene("Floor" + (PlayerPrefs.GetInt("floor") - 1));
    }

    private bool CheckIfSceneExists(string name)
    {
        bool result = false;
        
        for(int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            Debug.Log(SceneManager.GetSceneAt(i).name + " ?= " + name);
            if(SceneManager.GetSceneAt(i).name == name)
            {
                result = true;
            }
        }

        return result;
    }

    public void ApplySettings()
    {
        if (!settingsApplied) return;
        
        gameManager.SetSFXVolume(SFXSlider.value);
        gameManager.SetAmbienceVolume(ambienceSlider.value);
        ambience.volume = ambienceSlider.value;
        gameManager.SetMouseSensitivity(mouseSensitivitySlider.value);

        PlayerPrefs.SetFloat("sfxVolume", SFXSlider.value);
        PlayerPrefs.SetFloat("ambienceVolume", ambienceSlider.value);
        PlayerPrefs.SetFloat("mouseSensitivity", mouseSensitivitySlider.value);
    }

    public void ResetSettings()
    {
        SFXSlider.value = 0.75f;
        ambienceSlider.value = 0.75f;
        mouseSensitivitySlider.value = 10f;
    }
    
    public void TestSFX()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.volume = SFXSlider.value;
        audio.Play();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    
    public void SwitchToMenu(string menu)
    {
        if (menu == "Main") menuTitle.text = "Lower Floor";
        else menuTitle.text = "Lower Floor - " + menu;

        foreach(GameObject g in menus)
        {
            if (g.name == menu) g.SetActive(true);
            else g.SetActive(false);
        }
    }
}
