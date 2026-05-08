
using UnityEngine;
using Cinemachine;


public class CinmachieShake : MonoBehaviour
{
    public static CinmachieShake Instance{get; private set;}
    private CinemachineVirtualCamera virtualCamera;
    private float shakeTimer;
    private float shakerTimeTotal;
    private float startingIntensity;
    private void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        Instance = this;
    }

    private void Start()
    {
        if (virtualCamera != null)
        {
            CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
            virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain=0.0f;
        }
            
    }
    private void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;

            var perlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            perlin.m_AmplitudeGain =
                Mathf.Lerp(0f, startingIntensity, shakeTimer / shakerTimeTotal);
        }
        else
        {
            var perlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            perlin.m_AmplitudeGain = 0f;
            shakeTimer = 0f;
        }
    }
    public void ShakeCamera(float intensity ,float time)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
            virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
        startingIntensity = intensity;
        shakerTimeTotal=time;
        shakeTimer = time;

    }
}
