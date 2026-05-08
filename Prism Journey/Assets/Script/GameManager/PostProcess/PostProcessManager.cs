using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using static UnityEngine.Rendering.DebugUI;

public class PostProcessManager : MonoBehaviour
{
    [Header("MainCamera")]
    [SerializeField] private UniversalAdditionalCameraData cameraData;

    [Header("Global Volume")]
    [SerializeField] private Volume globalVolume;
    private Bloom bloom;
    private void Start()
    {
        if ((cameraData!=null))
        {
            cameraData.renderPostProcessing = true;
        }

        if (globalVolume == null)
        {
            Debug.LogError("Global Volume is not assigned.");
            return;
        }

        if (!globalVolume.profile.TryGet(out bloom))
        {
            Debug.LogError("Bloom override not found in Global Volume.");
            return;
        }

        SetBloomIntensity(1);
        SetBloomThreshold(1.5f);

        if (PlayerPrefs.HasKey("masterBrightness"))
        {
            float value =bloom.threshold.value- PlayerPrefs.GetFloat("masterBrightness");
            if (value > 0)
            {
                bloom.threshold.value = value;
            }
            else
            {
                bloom.threshold.value = 0;
            }
        }
    }
    public void SetBloomIntensity(float value)
    {
        if (bloom == null)
            return;

        bloom.intensity.value = value;
    }
    public void SetBloomThreshold(float value)
    {
        if (bloom == null)
            return;
       
        bloom.threshold.value = value;
    }

}
