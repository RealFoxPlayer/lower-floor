using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject messageDisplay;
    public RectTransform powerVisual;
    public Animator fadetoblack_panel;

    public bool mainMenu = false;
    public bool lastLevel = false;
    public int levelNumber;

    public List<AudioSource> soundEffectSources;
    public List<AudioSource> ambienceSources;

    static float SFXVolume = 1f;
    static float ambienceVolume = 1f;
    static float mouseSensitivity = 10f;

    void Start()
    {
        if(!mainMenu) foreach(AudioSource source in FindObjectsOfType<AudioSource>())
        {
            if (source.clip == null) { soundEffectSources.Add(source); source.volume = SFXVolume; }
            else { ambienceSources.Add(source); source.volume = ambienceVolume; }
        }
    }

    void Update()
    {
        
    }

    public void SetSFXVolume(float f)
    {
        SFXVolume = f;
    }

    public void SetAmbienceVolume(float f)
    {
        ambienceVolume = f;
    }

    public void SetMouseSensitivity(float f)
    {
        mouseSensitivity = f;
    }

    public float GetSFXVolume()
    {
        return SFXVolume;
    }

    public float GetAmbienceVolume()
    {
        return ambienceVolume;
    }

    public float GetMouseSensitivity()
    {
        return mouseSensitivity;
    }
    
    public void FinishLevel()
    {
        PlayerPrefs.SetInt("floor", levelNumber + 1);
        Invoke("LoadNextScene", 5f);
    }

    private void LoadNextScene()
    {
        if (!lastLevel) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        else SceneManager.LoadScene("MainMenu");
    }
}
