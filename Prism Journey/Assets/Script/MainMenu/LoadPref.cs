using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LoadPref : MonoBehaviour
{
    [Header("General Setting")]
    [SerializeField] private bool canUse = false;
    [SerializeField] private MenuController menuController;



    [Header("Volume Setting")]
    [SerializeField] private TMP_Text volumeTextValue=null;
    [SerializeField] private Slider volumeSlider;

    [Header("Graphic Setting")]


    [Header ("Brightness Setting")]
    [SerializeField] private Slider brightnessSlider = null;
    [SerializeField] private TMP_Text brightnessTextValue = null;

    [Header("FullScreen Setting ")]
    [SerializeField] private Toggle fullScreenToggle;

    [Header("Quality Setting")]
    [SerializeField] private TMP_Dropdown qualityDropDown;



    private void Awake()
    {
        if (canUse)
        {
            if (PlayerPrefs.HasKey("masterVolume"))
            {
                float localVolume = PlayerPrefs.GetFloat("masterVolume");

                volumeTextValue.text=localVolume.ToString("0.0");
                volumeSlider.value=localVolume; ;
                AudioListener.volume=localVolume;
            }
            else
            {
                menuController.ResetButton("Audio");
            }

            if (PlayerPrefs.HasKey("masterQuality"))
            {
                int localQuality = PlayerPrefs.GetInt("masterQuality");

                qualityDropDown.value=localQuality;
                QualitySettings.SetQualityLevel(localQuality);
            }


            if (PlayerPrefs.HasKey("masterFullScreen"))
            {
                int localFullScreen = PlayerPrefs.GetInt("masterFullScreen");
                if(localFullScreen == 1)
                {
                    Screen.fullScreen = true;
                    fullScreenToggle.isOn=true;

                }
                else
                {
                    Screen.fullScreen =false;
                    fullScreenToggle.isOn = false;

                }
            }

            if (PlayerPrefs.HasKey("masterBrightness"))
            {
                float localBrightness = PlayerPrefs.GetFloat("masterBrightness");

                brightnessTextValue.text = localBrightness.ToString("0.0");
                brightnessSlider.value=localBrightness;

            }
        }
    }
}
