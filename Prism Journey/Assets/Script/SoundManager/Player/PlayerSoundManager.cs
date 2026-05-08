using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.Rendering;
using System;
using Unity.VisualScripting;


public enum PlayerSoundType
{
  
    Run,
    Dash,
    TakeDamage,
    InteractSuccess,
    InteractFail,
    Dead
}
[RequireComponent(typeof(AudioSource)),ExecuteInEditMode]
public class PlayerSoundManager : MonoBehaviour
{

    private static PlayerSoundManager instance;
    private AudioSource audioSource;
    [SerializeField] private SoundList[] soundList;
    private void Awake()
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();
        if(audioSource == null) { Debug.LogError($"[PlayerSoundManager] | audio source component are missing ",this); }
        audioSource.volume = .5f;
    }

    public static void PlaySound( PlayerSoundType type , float volume=1)
    {

        instance.audioSource.PlayOneShot(instance.soundList[(int)type].sound, instance.soundList[(int)type].volumes );
    }

#if UNITY_EDITOR
    private void OnEnable()
    {
        string[] names= Enum.GetNames(typeof(PlayerSoundType));
        Array.Resize(ref soundList, names.Length);
        for (int i = 0; i < soundList.Length; i++)
        {
            soundList[i].name= names[i];
        }
    }
#endif
}
[Serializable]
public struct SoundList
{
    public AudioClip sound { get => sounds; }
    public float volumes { get => volume; }

    [SerializeField]public string name;
    [SerializeField] private AudioClip sounds;
    [SerializeField,Range(0,1)]private float volume;
}