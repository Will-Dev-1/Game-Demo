using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class GameSettings : MonoBehaviour
{
    public Slider volumeSlider;
    public Dropdown resolutionDropdown;
    public Toggle fullscreenToggle;
    public Dropdown shadowQualityDropdown;
    public Dropdown antiAliasingDropdown; 

    private Resolution[] resolutions;

    private void Start()
    {
        // Populate resolution dropdown
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        foreach (Resolution res in resolutions)
        {
            resolutionDropdown.options.Add(new Dropdown.OptionData(res.width + " x " + res.height));
        }
        resolutionDropdown.RefreshShownValue();

        // Populate anti-aliasing dropdown
        antiAliasingDropdown.ClearOptions();
        antiAliasingDropdown.options.Add(new Dropdown.OptionData("None"));
        antiAliasingDropdown.options.Add(new Dropdown.OptionData("2x"));
        antiAliasingDropdown.options.Add(new Dropdown.OptionData("4x"));
        antiAliasingDropdown.options.Add(new Dropdown.OptionData("8x"));
        antiAliasingDropdown.RefreshShownValue();

        // Load saved settings
        LoadSettings();
    }

    public void ApplySettings()
    {
        // Apply volume
        AudioListener.volume = volumeSlider.value;

        // Apply resolution
        Resolution selectedResolution = resolutions[resolutionDropdown.value];
        Screen.SetResolution(selectedResolution.width, selectedResolution.height, fullscreenToggle.isOn);

        // Apply fullscreen
        Screen.fullScreen = fullscreenToggle.isOn;

        // Apply shadow quality
        QualitySettings.shadows = (ShadowQuality)shadowQualityDropdown.value;

        // Apply anti-aliasing
        switch (antiAliasingDropdown.value)
        {
            case 0:
                QualitySettings.antiAliasing = 0; // None
                break;
            case 1:
                QualitySettings.antiAliasing = 2; // 2x
                break;
            case 2:
                QualitySettings.antiAliasing = 4; // 4x
                break;
            case 3:
                QualitySettings.antiAliasing = 8; // 8x
                break;
        }

        // Save settings to a file
        SaveSettings();
    }

    private void SaveSettings()
    {
        string path = Application.persistentDataPath + "/gamesettings.txt";
        using (StreamWriter writer = new StreamWriter(path))
        {
            writer.WriteLine(volumeSlider.value);
            writer.WriteLine(resolutionDropdown.value);
            writer.WriteLine(fullscreenToggle.isOn);
            writer.WriteLine(shadowQualityDropdown.value);
            writer.WriteLine(antiAliasingDropdown.value); 
        }
    }
    public void LoadDefaultSettings()
    {
        // Set your default values for game settings
        volumeSlider.value = 0.5f;
        resolutionDropdown.value = resolutions.Length - 1; // Default to highest resolution
        fullscreenToggle.isOn = true;
        shadowQualityDropdown.value = (int)ShadowQuality.All;
        antiAliasingDropdown.value = 0; // Default to "None"

        ApplySettings(); // Apply the default settings
    }

    public void LoadSettings()
    {
        string path = Application.persistentDataPath + "/gamesettings.txt";
        if (File.Exists(path))
        {
            using (StreamReader reader = new StreamReader(path))
            {
                string line;
                if ((line = reader.ReadLine()) != null) volumeSlider.value = float.Parse(line);
                if ((line = reader.ReadLine()) != null) resolutionDropdown.value = int.Parse(line);
                if ((line = reader.ReadLine()) != null) fullscreenToggle.isOn = bool.Parse(line);
                if ((line = reader.ReadLine()) != null) shadowQualityDropdown.value = int.Parse(line);
                if ((line = reader.ReadLine()) != null) antiAliasingDropdown.value = int.Parse(line);
            }
        }
        else
        {
            // Set default values if the file does not exist or is incomplete
            volumeSlider.value = 0.5f;
            resolutionDropdown.value = resolutions.Length - 1;
            fullscreenToggle.isOn = true;
            shadowQualityDropdown.value = (int)ShadowQuality.All;
            antiAliasingDropdown.value = 0; 
        }
    }

}
