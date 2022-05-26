using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private TMP_InputField newUsername; 
    [SerializeField] private TMP_Text playerUsername; 

    public TMP_Text newUsernameInput;
    private float _changeSizeFontTo = Screen.height / 40;
    private void Start()
    {
        newUsernameInput.fontSize = _changeSizeFontTo;
        newUsername.characterLimit = 10;
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
