using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SoundType
{
    None,
    Suck,
    Explosion,
    Finish
}

[RequireComponent(typeof(AudioSource))]
public class   BlackHoleSoundManager : MonoBehaviour
{
   private static BlackHoleSoundManager instance;
    [SerializeField] private AudioClip[] soundList;
    private AudioSource audioSource;

    private SoundType currentType=SoundType.None;

    private void Awake()
    {
        instance=this;

        audioSource = GetComponent<AudioSource>();
        
    }

    public static void PlaySound(SoundType sound , float volume =1f)
    {
        if (instance == null)
            return;

        if (sound == SoundType.None)
            return;

        int index = (int)sound - 1; // because None is 0

        if (index < 0 || index >= instance.soundList.Length)
        {
            Debug.LogError($"Invalid sound index for {sound}. Index={index}, Length={instance.soundList.Length}");
            return;
        }
        AudioClip clip = instance.soundList[index];

        if (clip == null)
        {
            Debug.LogWarning($"Clip for {sound} is null.");
            return;
        }

        if (instance.currentType == sound && instance.audioSource.isPlaying)
            return;

        instance.currentType = sound;
        instance.audioSource.Stop();
        instance.audioSource.clip = clip;
        instance.audioSource.volume = volume;
        instance.audioSource.loop = false;
        instance.audioSource.Play();
    }
    public static void StopSound()
    {
        if (instance == null)
            return;

        instance.audioSource.Stop();
        instance.currentType = SoundType.None;
    }

    private void Update()
    {
        if (!audioSource.isPlaying)
        {
            currentType = SoundType.None;
        }
    }

}
