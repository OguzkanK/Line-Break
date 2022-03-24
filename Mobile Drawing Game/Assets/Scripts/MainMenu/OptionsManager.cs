using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider; // Volume slider in options
    [SerializeField] private TMP_InputField newUsername; // New username input in options
    [SerializeField] private TMP_Text playerUsername; // Current username from PlayerPrefs

    public TMP_Text newUsernameInput;
    private float _changeSizeFontTo = Screen.height / 40;
    private void Start()
    {
        newUsernameInput.fontSize = _changeSizeFontTo;
        newUsername.characterLimit = 10;
        // If PlayerPrefs musicVolume is null default to 1
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1);
            LoadVolume();
        }
        else
        {
            LoadVolume();
        }
        
        
        // If PlayerPrefs playerUsername is null default to Guest
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

    // Save new volume preference on volume change
    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;
        SaveVolume();
    }

    // Load volume from PlayerPrefs
    private void LoadVolume()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
    }

    // Save volume to PlayerPrefs
    private void SaveVolume()
    {
        PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
    }
    
    // Load username from PlayerPrefs
    private void LoadUsername()
    {
        playerUsername.text = PlayerPrefs.GetString("playerUsername");
    }

    // Save username to PlayerPrefs
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
