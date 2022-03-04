using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    [SerializeField] Slider volumeSlider;
    [SerializeField] TMP_InputField newUsername;
    [SerializeField] TMP_Text playerUsername;
    void Start()
    {
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1);
            LoadVolume();
        }
        else
        {
            LoadVolume();
        }
        
        
        if (!PlayerPrefs.HasKey("playerUsername"))
        {
            PlayerPrefs.SetString("playerUsername", "Guest");
            LoadUsername();
        }
        else
        {
            LoadUsername();
        }
    }

    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;
        SaveVolume();
    }

    private void LoadVolume()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
    }

    private void SaveVolume()
    {
        PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
    }
    
    
    private void LoadUsername()
    {
        playerUsername.text = PlayerPrefs.GetString("playerUsername");
    }

    public void SaveUsername()
    {
        if(newUsername.text != "")
        {
            PlayerPrefs.SetString("playerUsername", newUsername.text);
            playerUsername.text = newUsername.text;
        }
        else
        {
            PlayerPrefs.SetString("playerUsername", "Guest");
            playerUsername.text = "Guest";
        }
    }
}
