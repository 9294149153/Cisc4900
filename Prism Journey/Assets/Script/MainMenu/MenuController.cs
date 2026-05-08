using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class MenuController : MonoBehaviour
{
    [Header("Level To Load ")]
    public string newGameLevel="PlayScene";
    private string levelToLoad;
    [SerializeField] private GameObject noSaveGameDialog=null;


    [Header("Volume Setting")]
    
    [SerializeField]private TMP_Text volumeTextValue=null;
    [SerializeField] private Slider volumeSlider=null;
    [SerializeField] private float defaultVolume = 0.5f;

    [Header("Comfirmation")]
    [SerializeField] private GameObject comfirmationPrompt = null;



    [Header("Quality Setting")]
    [SerializeField] private TMP_Dropdown qualityDropDown;
    [SerializeField] private Toggle fullScreenToggle;

    [Header("Graphic Setting")]
    [SerializeField] private Slider brightnessSlider=null;
    [SerializeField] private TMP_Text brightnessTextValue=null;
    [SerializeField] private float defaultBrightness = 1f;
    
    private int qualityLevel;
    private bool isFullScreen;
    private float brightnessLevel;


    [Header("Resolution DropDown")]
    public TMP_Dropdown resolutionDropDown;
    private Resolution[] resolutions;
    
    private void Start()
    {
      

        resolutions = Screen.resolutions;
        resolutionDropDown.ClearOptions();


        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option= resolutions[i].width + " X " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width==Screen.width && resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropDown.AddOptions(options);
        resolutionDropDown.value = currentResolutionIndex;
        resolutionDropDown.RefreshShownValue();
    }

    //transfer from mainmenu into the play scene 
    public void NewGameDialogYes()
    {    
        SceneManager.LoadScene(newGameLevel);//load to the scene
    }

   
    public void LoadGameDialogYes()
    {

        if (SqliteManager.SaveFileExists())
        {
            SqliteManager.SetGameDataWillLoad(SqliteManager.SaveFileExists());
            SceneManager.LoadScene(newGameLevel);


        }
        else
        {
            // Not key putout the No file dialog panel active
            noSaveGameDialog.SetActive(true);
        }

       
    }

    public   void ExitButton()
    {
        //Game Exit
        Application.Quit();
    }

  /// <summary>
  /// Function to handle Volume
  /// </summary>
  /// <param name="volume"></param>
    public void SetVolume(float volume)
    {
        AudioListener.volume = volume; // AudioListener are the master of the entire game volume control
        volumeTextValue.text=volume.ToString("0.0");
    }
    

    public void VolumeApply()
    {
        PlayerPrefs.SetFloat("masterVolume", AudioListener.volume);
        StartCoroutine(ConfirmationBox());
    }

    /// <summary>
    /// Funciton to handle the Graphic
    /// </summary>
    /// <returns></returns>


    public void SetBrightness(float brightness)
    {
        brightnessLevel= brightness;
        brightnessTextValue.text = brightness.ToString("0.0");
       
    }
    

    public void SetFullScreen(bool value)
    {
        isFullScreen = value;
    }

    public void SetQuality(int qualityIndex)
    {
        qualityLevel= qualityIndex;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height,Screen.fullScreen);
    }

    public void GraphicsApply()
    {
        PlayerPrefs.SetFloat("masterBrightness", brightnessLevel);
        // Change your brightness with your post processing 

        PlayerPrefs.SetInt("masterQuality", qualityLevel);
        QualitySettings.SetQualityLevel(qualityLevel);
        PlayerPrefs.SetInt("masterFullScreen", (isFullScreen? 1:0));
        Screen.fullScreen = isFullScreen;

        StartCoroutine(ConfirmationBox());
    }

    public void ResetButton(string MenuType)
    {
        if (MenuType == "Audio")
        {
            SetVolume(defaultVolume);
            volumeSlider.value = defaultVolume;
            VolumeApply();
        }


        if(MenuType == "Graphic")
        {
            //Reset Brightness value
            brightnessSlider.value= defaultBrightness;
            brightnessTextValue.text = defaultBrightness.ToString("0.0");
            qualityDropDown.value = 1;
            QualitySettings.SetQualityLevel(1);

            fullScreenToggle.isOn = false;
            Screen.fullScreen = false;

            Resolution currentResolution =Screen.currentResolution;
            Screen.SetResolution(currentResolution.width, currentResolution.height,Screen.fullScreen);
            resolutionDropDown.value = resolutions.Length;
            GraphicsApply();

        }
    }
    public IEnumerator ConfirmationBox()
    {
        comfirmationPrompt.SetActive(true);     
        yield return new WaitForSeconds(2);
        comfirmationPrompt.SetActive(false);
    }
}
